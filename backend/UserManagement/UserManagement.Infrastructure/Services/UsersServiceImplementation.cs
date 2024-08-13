using AutoMapper;
using UserManagement.Infrastructure.Common;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;
using UserManagement.Application.Repositories;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace UserManagement.Infrastructure.Services
{
    public class UsersServiceImplementation : UsersService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UsersServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthenticationResponseDto> Login(AuthenticationRequestDto request)
        {
            var user = await _unitOfWork.UsersRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("Email hasn't been registered");
                // TODO: Global exception handler
            }

            HashingHelper.Hash(request.Password, user.PasswordSalt, out string hash);
            if (hash != user.PasswordHash)
            {
                throw new Exception("Wrong password");
            }

            var token = JwtHelper.GenerateAccessToken(user, _configuration["key"]);

            return new AuthenticationResponseDto
            {
                Token = token
            };
        }

        public async Task Register(UserRegistrationRequestDto request)
        {
            if (_unitOfWork.UsersRepository.ExistByEmail(request.Email))
            {
                throw new Exception("Email has been registered");
            }

            var user = _mapper.Map<User>(request);
            HashingHelper.Hash(request.Password, out string hash, out string salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            
            await _unitOfWork.UsersRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
        }
    }
}

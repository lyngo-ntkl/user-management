using AutoMapper;
using System.Linq.Expressions;
using UserManagement.Infrastructure.Common;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;
using UserManagement.Application.Repositories;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using UserManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UserManagement.Application.Common;

namespace UserManagement.Infrastructure.Services
{
    public class UsersServiceImplementation : UsersService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthenticationResponseDto> Login(AuthenticationRequestDto request)
        {
            var user = await _unitOfWork.UsersRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BadRequestException(ExceptionMessage.UnregisteredEmail);
            }

            HashingHelper.Hash(request.Password, user.PasswordSalt, out string hash);
            if (hash != user.PasswordHash)
            {
                throw new BadRequestException(ExceptionMessage.WrongPassword);
            }

            var token = JwtHelper.GenerateAccessToken(user, _configuration["key"]!);

            return new AuthenticationResponseDto
            {
                AccessToken = token
            };
        }

        public async Task<List<UserResponseDto>> GetUsers(string? userName)
        {
            Expression expression = Expression.Constant(true);
            var parameters = Expression.Parameter(typeof(User));

            if (userName != null)
            {
                expression = Expression.And(
                    expression,
                    Expression.Call(
                        Expression.Call(
                            Expression.Property(parameters, nameof(User.UserName)),
                            typeof(string).GetMethod("ToLower", Type.EmptyTypes) ?? throw new InvalidOperationException("string.ToLower not found")
                        ),
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }) ?? throw new InvalidOperationException("string.Contains not found"),
                        Expression.Constant(userName.Trim().ToLower())
                    )
                );
            }

            var users = await _unitOfWork.UsersRepository.GetAsync(Expression.Lambda<Func<User,bool>>(expression, parameters));
            return _mapper.Map<List<UserResponseDto>>(users);
        }

        public async Task Register(UserRegistrationRequestDto request)
        {
            if (_unitOfWork.UsersRepository.ExistByEmail(request.Email))
            {
                throw new BadRequestException(ExceptionMessage.RegisteredEmail);
            }

            var user = _mapper.Map<User>(request);
            HashingHelper.Hash(request.Password, out string hash, out string salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            
            await _unitOfWork.UsersRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserResponseDto> GetUser(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException(ExceptionMessage.UserNotFound);
            }
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserPersonalResponseDto> GetUserPersonalInfo()
        {
            User user = await GetLoginUser();

            return _mapper.Map<UserPersonalResponseDto>(user);
        }

        public async Task<UserPersonalResponseDto> UpdateUser(UserUpdatedRequestDto request)
        {
            User user = await GetLoginUser();

            user = _mapper.Map(request, user);
            _unitOfWork.UsersRepository.Update(user);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UserPersonalResponseDto>(user);
        }

        public async Task DeleteUser()
        {
            User user = await GetLoginUser();
            _unitOfWork.UsersRepository.Delete(user);
            await _unitOfWork.SaveAsync();
        }

        private async Task<User> GetLoginUser()
        {
            var jwt = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString().Split("Bearer ", StringSplitOptions.RemoveEmptyEntries)[0];
            if (string.IsNullOrEmpty(jwt))
            {
                throw new UnauthorizedException();
            }

            var claims = JwtHelper.GetClaims(jwt);
            var id = int.Parse(claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid)?.Value ?? string.Empty);

            var user = await _unitOfWork.UsersRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException(ExceptionMessage.UserNotFound);
            }
            return user;
        }
    }
}

using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Repositories;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Services
{
    public class UsersServiceImplementation : UsersService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersServiceImplementation(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Register(UserRegistrationRequestDto request)
        {
            var user = _unitOfWork.UsersRepository.Get().FirstOrDefault(u => u.Email == request.Email);
            if (user != null)
            {
                throw new Exception("Email has been registered");
            }

            user = _mapper.Map<User>(request);
            HashingHelper.Hash(request.Password, out byte[] hash, out byte[] salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            await _unitOfWork.UsersRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
        }
    }

    public class HashingHelper
    {
        private const int SaltBytes = 16;
        private const int HashBytes = 32;
        private const int Iteration = 10000;
        private static HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
        public static void Hash(string value, out byte[] hash, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(SaltBytes);
            var pbkdf2 = new Rfc2898DeriveBytes(value, salt, Iteration, Algorithm);
            var hashBytes = pbkdf2.GetBytes(HashBytes);
            hash = hashBytes;
        }

        public static void Hash(string value, byte[] salt, out byte[] hash)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(value, salt, Iteration, Algorithm);
            hash = pbkdf2.GetBytes(HashBytes);
        }
    }
}

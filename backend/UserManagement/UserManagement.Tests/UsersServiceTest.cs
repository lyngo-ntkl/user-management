using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Exceptions;
using UserManagement.Application.Repositories;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Common;
using UserManagement.Infrastructure.Services;

namespace UserManagement.Tests
{
    [TestFixture]
    public class UsersServiceTest
    {
        private Mock<UnitOfWork>? _unitOfWork;
        private IMapper? _mapper;
        private Mock<IConfiguration> _configuration;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private UsersService? _usersService;
        [SetUp]
        public void Init()
        {
            _unitOfWork = new Mock<UnitOfWork>();
            _mapper = (new MapperConfiguration(config => config.AddProfile<UserManagementProfile>())).CreateMapper();
            _configuration = new Mock<IConfiguration>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _usersService = new UsersServiceImplementation(_unitOfWork.Object, _mapper, _configuration.Object, _httpContextAccessor.Object);

            _configuration.Setup(c => c["key"]).Returns("eyJhbGciOiJIUzUxMiJ9.ew0KICAic3ViIjogIjEyMzQ1Njc4OTAiLA0KICAibmFtZSI6ICJBbmlzaCBOYXRoIiwNCiAgImlhdCI6IDE1MTYyMzkwMjINCn0.0lAo-7g558HEbUzGjAZj6ZU4-lQ0t2W3fgkYf04RoxMb_YtO8Fh5ox3CPkGFy6N08EH4OEh9Dsjm2uS_QWZ96g");

            _unitOfWork.Setup(uow => uow.UsersRepository.ExistByEmail("user1@gmail.com"))
                .Returns(true);
            _unitOfWork.Setup(uow => uow.UsersRepository.ExistByEmail("user2@gmail.com"))
                .Returns(false);
            _unitOfWork.Setup(uow => uow.UsersRepository.GetByEmailAsync("user1@gmail.com"))
                .ReturnsAsync(new User {
                    Id = 1,
                    Email = "user1@gmail.com",
                    PasswordHash = "GIDMb7tMLaqSM8eVVbPCR0ADFswzdrquw7Rkjmkj/gs=",
                    PasswordSalt = "i+fZst82pFuWmNXR0zbceQ==",
                    Role = Domain.Enumerations.Role.USER,
                    UserName = "User 1"
                });
        }

        [Test]
        public void Register_ExistedEmail_ThrowException()
        {
            var request = new UserRegistrationRequestDto
            {
                Email = "user1@gmail.com",
                Password = "Helloworld12345!!",
                UserName = "user",
            };

            var exception = Assert.ThrowsAsync<BadRequestException>(async delegate {
                await _usersService!.Register(request);
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.TypeOf<BadRequestException>());
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.RegisteredEmail));
        }

        [Test]
        public void Register_GivenRightArgument_NotThrowException()
        {
            var request = new UserRegistrationRequestDto
            {
                Email = "user2@gmail.com",
                Password = "Helloworld12345!!",
                UserName = "user 2",
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                await _usersService!.Register(request);
            });
        }

        [Test]
        public void Login_GivenUnregisteredEmail_ThrowException()
        {
            var request = new AuthenticationRequestDto
            {
                Email = "user2@gmail.com",
                Password = "Helloworld12345!!"
            };

            var exception = Assert.ThrowsAsync<BadRequestException>(async delegate
            {
                await _usersService!.Login(request);
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.TypeOf<BadRequestException>());
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.UnregisteredEmail));
        }

        [Test]
        public void Login_GivenWrongPassword_ThrowException()
        {
            var request = new AuthenticationRequestDto
            {
                Email = "user1@gmail.com",
                Password = "Helloworld12346!!"
            };

            var exception = Assert.ThrowsAsync<BadRequestException>(async delegate
            {
                await _usersService!.Login(request);
            });

            Assert.That(exception, Is.Not.Null);
            Assert.That(exception, Is.TypeOf<BadRequestException>());
            Assert.That(exception.Message, Is.EqualTo(ExceptionMessage.WrongPassword));
        }

        [Test]
        public async Task Login_GiventRightArguments_ReturnToken()
        {
            var request = new AuthenticationRequestDto
            {
                Email = "user1@gmail.com",
                Password = "Helloworld12345!!"
            };

            var authToken = await _usersService!.Login(request);

            Assert.That(authToken, Is.Not.Null);
            Assert.That(authToken.AccessToken, Is.Not.Empty);
        }
    }
}

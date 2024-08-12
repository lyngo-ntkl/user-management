using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Text;
using UserManagement.Application.Repositories;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Configurations;
using UserManagement.Infrastructure.Services;

namespace UserManagement.Tests
{
    [TestFixture]
    public class UsersServiceTest
    {
        private Mock<UnitOfWork>? _unitOfWork;
        private IMapper? _mapper;
        private UsersService? _usersService;
        [SetUp]
        public void Init()
        {
            _unitOfWork = new Mock<UnitOfWork>();
            _mapper = (new MapperConfiguration(config => config.AddProfile<UserManagementProfile>())).CreateMapper();
            _usersService = new UsersServiceImplementation(_unitOfWork.Object, _mapper);

            _unitOfWork.Setup<List<User>>(uow => uow.UsersRepository.Get(null, null, "")).Returns(new List<User>
            {
                new User { Id = 1, Email = "user1@gmail.com", PasswordHash = Encoding.UTF8.GetBytes("13AEA64B44994160D9ED85D2C41C8131163E261A1E85FB22E5A4B71A81F0272F7CA309D8CC1EB16295E23DA62A955F1576FAE2877366DDD740881802E072F92A"), PasswordSalt = Encoding.UTF8.GetBytes("26C7F7795BBDA24AC524E648CBB32"), Role = Domain.Enumerations.Role.USER, UserName = "User 1"}
            });

        }

        [Test]
        public void Register_ExistedEmail_ThrowException()
        {
            var exception = Assert.ThrowsAsync<Exception>(async delegate {
                await _usersService!.Register(new Application.Dtos.Requests.UserRegistrationRequestDto
                {
                    Email = "user1@gmail.com",
                    Password = "1234567890",
                    UserName = "user",
                });
            });

            Assert.That(exception.Message, Is.EqualTo("Email has been registered"));
        }

        [Test]
        public void Register_GivenRightArgument_NotThrowException()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await _usersService.Register(new Application.Dtos.Requests.UserRegistrationRequestDto
                {
                    Email = "user2@gmail.com",
                    Password = "1234567890",
                    UserName = "user 2",
                });
            });
        }
    }
}

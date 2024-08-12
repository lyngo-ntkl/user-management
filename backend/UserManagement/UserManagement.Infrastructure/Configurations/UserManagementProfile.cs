using AutoMapper;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enumerations;

namespace UserManagement.Infrastructure.Configurations
{
    public class UserManagementProfile: Profile
    {
        public UserManagementProfile()
        {
            CreateMap<UserRegistrationRequestDto, User>()
                .ForMember(user => user.Role, config => config.MapFrom(dto => Role.USER))
                .ForAllMembers(config => config.Condition((src, dest, val) => val != null));
        }
    }
}

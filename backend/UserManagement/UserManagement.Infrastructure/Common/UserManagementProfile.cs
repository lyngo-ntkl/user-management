﻿using AutoMapper;
using UserManagement.Application.Dtos.Requests;
using UserManagement.Application.Dtos.Responses;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Enumerations;

namespace UserManagement.Infrastructure.Common
{
    public class UserManagementProfile : Profile
    {
        public UserManagementProfile()
        {
            CreateMap<UserRegistrationRequestDto, User>()
                .ForMember(user => user.Role, config => config.MapFrom(dto => Role.USER))
                .ForAllMembers(config => config.Condition((src, dest, val) => val != null));
            CreateMap<User, UserResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, val) => val != null));
            CreateMap<User, UserPersonalResponseDto>()
                .ForAllMembers(config => config.Condition((src, dest, val) => val != null));
            CreateMap<UserUpdatedRequestDto, User>()
                .AfterMap((src, dest) =>
                {
                    if (src != null && src.Password != null)
                    {
                        HashingHelper.Hash(src.Password, out string hash, out string salt);
                        dest.PasswordSalt = salt;
                        dest.PasswordHash = hash;
                    }
                })
                .ForAllMembers(config => config.Condition((src, dest, val) => val != null));
        }
    }
}

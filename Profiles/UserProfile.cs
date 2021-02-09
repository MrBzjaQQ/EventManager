using AutoMapper;
using EventManager.Dtos.User;
using EventManager.Models;

namespace EventManager.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, UserModel>();
            CreateMap<UserModel, UserReadDto>();
        }
    }
}
using Estate.erp.bll.Enums;
using Estate.erp.dal.Model;
using Estate.erp.Dto.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.erp.bll.Mappers
{
    public static class UserModelDtoMapping
    {
        public static List<UserDto> ModelListToDtoList(this List<User> users)
        {
            List<UserDto> _users = new List<UserDto>();
            foreach (var user in users)
            {
                UserDto _user = new UserDto();

                _user.id = user.ID;
                _user.applicationUserId = user.ApplicationUserId;
                _user.email = user.Email;
                _user.firstName = user.FirstName;
                _user.lastName = user.LastName;
                _user.address = user.Address;

                _users.Add(_user);
            }
            return _users;
        }

        public static UserDto ModelToDto(this User user)
        {
            UserDto _user = new UserDto();

            _user.id = user.ID;
            _user.applicationUserId = user.ApplicationUserId;
            _user.email = user.Email;
            _user.firstName = user.FirstName;
            _user.lastName = user.LastName;
            _user.address = user.Address;

            return _user;
        }

        public static User DtoToModel(this UserDto user, string applicationUserId)
        {
            User _user = new User();

            _user.ID = user.id.HasValue ? user.id.Value : 0;
            _user.Email = user.email;
            _user.ApplicationUserId = String.IsNullOrEmpty(applicationUserId) ? user.applicationUserId : applicationUserId;
            _user.FirstName = user.firstName;
            _user.LastName = user.lastName;
            _user.Address = user.address;

            return _user;
        }

        public static List<RoleDto> ModelListToDtoList(this List<RoleEnums> roles)
        {
            List<RoleDto> _roles = new List<RoleDto>();
            int index = 0;
            foreach (var role in roles)
            {
                RoleDto _role = new RoleDto();
                index = index + 1;
                _role.id = index;
                _role.description = role.ToString();

                _roles.Add(_role);
            }
            return _roles;
        }
        //public static List<UserProfileDto> ProfileModelListToProfileDtoList(this List<UserProfile> userProfiles)
        //{
        //    List<UserProfileDto> _userProfiles = new List<UserProfileDto>();
        //    foreach (var userProfile in userProfiles)
        //    {
        //        UserProfileDto _userProfile = new UserProfileDto();

        //        _userProfile.id = userProfile.ID;
        //        _userProfile.firstName = userProfile.FirstName;
        //        _userProfile.lastName = userProfile.LastName;
        //        _userProfile.address = userProfile.Address;

        //        _userProfiles.Add(_userProfile);
        //    }
        //    return _userProfiles;
        //}

        //public static UserProfileDto ProfileModelToProfileDto(this UserProfile userProfile)
        //{
        //    UserProfileDto _userProfile = new UserProfileDto();

        //    _userProfile.id = userProfile.ID;
        //    _userProfile.firstName = userProfile.FirstName;
        //    _userProfile.lastName = userProfile.LastName;
        //    _userProfile.address = userProfile.Address;

        //    return _userProfile;
        //}

        //public static UserProfile ProfileDtoToProfileModel(this UserProfileDto userProfile)
        //{
        //    UserProfile _userProfile = new UserProfile();

        //    _userProfile.ID = userProfile.id;
        //    _userProfile.UserId = userProfile.userId;
        //    _userProfile.FirstName = userProfile.firstName;
        //    _userProfile.LastName = userProfile.lastName;
        //    _userProfile.Address = userProfile.address;

        //    return _userProfile;
        //}
    }
}

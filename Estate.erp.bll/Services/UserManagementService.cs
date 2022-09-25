using Estate.erp.dal.Model;
using Estate.erp.dal.Repository;
using Estate.erp.Dto.UserManagement;
using Estate.erp.Interface;
using Estate.erp.bll.Mappers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estate.erp.dal;
using Microsoft.AspNetCore.Identity;
using Estate.erp.bll.Enums;
using System.ComponentModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Estate.erp.bll.Services
{
    public class UserManagementService : IUserManagementService
    {
        private IRepository<User> _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManagementService(IRepository<User> userRepository,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserDto> GetAllUsers()
        {
            var users = _userRepository.Table.ToList();
            return users.ModelListToDtoList();
        }

        public UserDto GetUserByEmail(string email)
        {
            var user = _userRepository.Table.FirstOrDefault(x=>x.Email == email);
            return user.ModelToDto();
        }

        public List<RoleDto> GetAllRole()
        {
            var roles = Enum.GetValues(typeof(RoleEnums)).Cast<RoleEnums>().ToList();
            return roles.ModelListToDtoList();
        }

        public async Task<UserDto> Login(UserDto loginDetail, string issuer, string key)
        {
            var user = await AuthenticateUser(loginDetail);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var tokenString = GenerateJSONWebToken(user, issuer, key, (List<string>)userRoles);
                var defaultRole = (int)Enum.Parse(typeof(RoleEnums), userRoles?.FirstOrDefault());
                var userId = _userRepository.Table.FirstOrDefault(x => x.ApplicationUserId == user.Id)?.ID;
                return new UserDto() { token = tokenString, email = user.Email, applicationUserId = user.Id, roleId = defaultRole, id = userId.Value };
            }
            return null;
        }

        public async Task<bool> InsertUser(UserDto user)
        {
            string roleName = Enum.GetName(typeof(RoleEnums), user.roleId);

            var userExists = await _userManager.FindByNameAsync(user.email);
            if (userExists != null)
                return false;

            ApplicationUser appUser = new ApplicationUser()
            {
                Email = user.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = user.email,
                PhoneNumber = user.phoneNumber
            };

            var result = await _userManager.CreateAsync(appUser, user.password);

            if (!await _roleManager.RoleExistsAsync(RoleEnums.Admin.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(RoleEnums.Admin.ToString()));
            if (!await _roleManager.RoleExistsAsync(RoleEnums.Supervisor.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(RoleEnums.Supervisor.ToString()));
            if (!await _roleManager.RoleExistsAsync(RoleEnums.Accountant.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(RoleEnums.Accountant.ToString()));
            if (!await _roleManager.RoleExistsAsync(RoleEnums.BookingAgent.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(RoleEnums.BookingAgent.ToString()));
            if (!await _roleManager.RoleExistsAsync(RoleEnums.Customer.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(RoleEnums.Customer.ToString()));

            if (await _roleManager.RoleExistsAsync(roleName) && result.Succeeded)
            {
                var _user = user.DtoToModel(appUser.Id);
                _user.CreatedDate = DateTime.Now;
                _userRepository.Insert(_user);

                if (_user.ID > 0)
                {
                    user.id = _user.ID;
                }

                await _userManager.AddToRoleAsync(appUser, roleName);
            }

            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(UserDto user)
        {
            var userExists = await _userManager.FindByNameAsync(user.email);
            if (userExists == null)
                return false;

            ApplicationUser appUser = new ApplicationUser()
            {
                Id = userExists.Id,
                Email = userExists.Email,
                UserName = userExists.UserName,
                PhoneNumber = user.phoneNumber,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            var result = await _userManager.UpdateAsync(appUser);

            string roleName = Enum.GetName(typeof(RoleEnums), user.roleId);

            if (await _roleManager.RoleExistsAsync(roleName) && result.Succeeded)
            {
                var _user = user.DtoToModel(appUser.Id);
                _user.UpdatedDate = DateTime.Now;
                _userRepository.Update(_user);

                if (_user.ID > 0)
                {
                    user.id = _user.ID;
                }

                await _userManager.AddToRoleAsync(appUser, roleName);
            }

            return result.Succeeded;
            //var _user = user.DtoToModel();
            //_user.UpdatedDate = DateTime.Now;
            //_userRepository.Update(_user);
            //user.userProfile.userId = _user.ID;
            //var _userProfile = user.userProfile.ProfileDtoToProfileModel();
            //_userProfile.UpdatedDate = DateTime.Now;
            //_userProfileRepository.Update(_userProfile);
            //if (_user.ID > 0 && _userProfile.ID > 0)
            //{
            //    user.id = _user.ID;
            //    user.userProfile.id = _userProfile.ID;
            //    return user;
            //}
            //return null;
        }

        public async Task<bool> DeleteUser(string email)
        {
            //get identity user by email
            var userExists = await _userManager.FindByNameAsync(email);
            if (userExists != null)
            {
                //get user by application user id
                var user = _userRepository.Table.FirstOrDefault(x => x.ApplicationUserId == userExists.Id);
                //delete user
                _userRepository.Delete(user);

                //remove user's identity roles
                var rolesForUser = await _userManager.GetRolesAsync(userExists);
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        await _userManager.RemoveFromRoleAsync(userExists, item);
                    }
                }

                //Delete identity user
                var result = await _userManager.DeleteAsync(userExists);

                return result.Succeeded;
            }
            else 
            {
                return false;
            }
        }

        private string GenerateJSONWebToken(ApplicationUser appUser, string issuer, string key, List<string> userRoles)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, appUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ApplicationUser> AuthenticateUser(UserDto login)
        {
            
            var user = await _userManager.FindByNameAsync(login.email);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.password))
            {
                return user;
            }

            return null;
        }
    }
}

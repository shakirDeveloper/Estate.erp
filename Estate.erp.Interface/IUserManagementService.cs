using Estate.erp.dal.Model;
using Estate.erp.Dto.UserManagement;

namespace Estate.erp.Interface
{
    public interface IUserManagementService
    {
        List<UserDto> GetAllUsers();
        UserDto GetUserByEmail(string email);
        List<RoleDto> GetAllRole();
        Task<UserDto> Login(UserDto loginDetail, string issuer, string key);
        Task<bool> InsertUser(UserDto user);
        Task<bool> UpdateUser(UserDto user);
        Task<bool> DeleteUser(string email);
    }
}
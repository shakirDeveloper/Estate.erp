using Estate.erp.bll.Enums;
using Estate.erp.Dto.UserManagement;
using Estate.erp.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estate.erp.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Microsoft.AspNetCore.Cors.EnableCors("AllowOrigin")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UserManagementController : ControllerBase
    {
        private readonly ILogger<UserManagementController> _logger;
        private IConfiguration _config;
        private IUserManagementService _userManagementService;

        public UserManagementController(IConfiguration config, ILogger<UserManagementController> logger, IUserManagementService userManagementService)
        {
            _logger = logger;
            _config = config;
            _userManagementService = userManagementService;
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            var users = _userManagementService.GetAllUsers();
            if (users != null && users.Count > 0)
                return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records retrieved successfully" });
            else
                return BadRequest(new
                {
                    message = "User record not found",
                    Status = StatusCodes.Status404NotFound
                });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto login)
        {
            IActionResult response = Unauthorized();
            var loginDto = await _userManagementService.Login(login, _config["Jwt:Issuer"], _config["Jwt:Key"]);
            if (loginDto != null && loginDto.id > 0 && !string.IsNullOrEmpty(loginDto.token))
            {
                response = Ok(
                    new
                    {
                        token = loginDto.token,
                        applicationUserId = loginDto.applicationUserId,
                        id = loginDto.id,
                        roleId = loginDto.roleId,
                        Status = StatusCodes.Status200OK
                    });

            }
            else
            {
                response = Unauthorized(new { message = loginDto.errorMessage, Status = StatusCodes.Status401Unauthorized });
            }

            return response;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var users = await _userManagementService.InsertUser(user);
            if (users == true)
                return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records saved successfully" });
            else
                return BadRequest(new
                {
                    message = "User record not saved",
                    Status = StatusCodes.Status500InternalServerError
                });
        }

        [HttpPost("CreateSupervisorUser")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateSupervisorUser(UserDto user)
        {
            if (user.roleId == (int)RoleEnums.Customer || user.roleId == (int)RoleEnums.Accountant || user.roleId == (int)RoleEnums.BookingAgent)
            {
                var users = await _userManagementService.InsertUser(user);
                if (users == true)
                    return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records saved successfully" });
                else
                    return BadRequest(new
                    {
                        message = "User record not saved",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Logged in user is not athorized to create user with select role",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("CreateBookingAgentUser")]
        [Authorize(Roles = "BookingAgent")]
        public async Task<IActionResult> CreateBookingAgentUser(UserDto user)
        {
            if (user.roleId == (int)RoleEnums.Customer)
            {
                var users = await _userManagementService.InsertUser(user);
                if (users == true)
                    return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records saved successfully" });
                else
                    return BadRequest(new
                    {
                        message = "User record not saved",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Logged in user is not athorized to create user with select role",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("UpdateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UserDto user)
        {
            var users = await _userManagementService.UpdateUser(user);
            if (users == true)
                return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records updated successfully" });
            else
                return BadRequest(new
                {
                    message = "User record not updated",
                    Status = StatusCodes.Status500InternalServerError
                });
        }

        [HttpPost("UpdateSupervisorUser")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> UpdateSupervisorUser(UserDto user)
        {
            if (user.roleId == (int)RoleEnums.Customer || user.roleId == (int)RoleEnums.Accountant || user.roleId == (int)RoleEnums.BookingAgent)
            {
                var users = await _userManagementService.UpdateUser(user);
                if (users == true)
                    return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records updated successfully" });
                else
                    return BadRequest(new
                    {
                        message = "User record not updated",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Logged in user is not athorized to create user with select role",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("UpdateBookingAgentUser")]
        [Authorize(Roles = "BookingAgent")]
        public async Task<IActionResult> UpdateBookingAgentUser(UserDto user)
        {
            if (user.roleId == (int)RoleEnums.Customer)
            {
                var users = await _userManagementService.UpdateUser(user);
                if (users == true)
                    return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records updated successfully" });
                else
                    return BadRequest(new
                    {
                        message = "User record not updated",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Logged in user is not athorized to create user with select role",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var users = await _userManagementService.DeleteUser(email);
            if (users == true)
                return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records deleted successfully" });
            else
                return BadRequest(new
                {
                    message = "User record not deleted",
                    Status = StatusCodes.Status500InternalServerError
                });
        }

        [HttpDelete("DeleteSupervisorUser")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> DeleteSupervisorUser(string email)
        {
            var user = _userManagementService.GetUserByEmail(email);

            if (user.roleId == (int)RoleEnums.Customer || user.roleId == (int)RoleEnums.Accountant || user.roleId == (int)RoleEnums.BookingAgent)
            {
                var users = await _userManagementService.DeleteUser(email);
                if (users == true)
                    return Ok(new { data = users, status = StatusCodes.Status200OK, message = "records deleted successfully" });
                else
                    return BadRequest(new
                    {
                        message = "User record not deleted",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Logged in user is not athorized to create user with select role",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
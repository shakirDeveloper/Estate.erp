using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.erp.Dto.UserManagement
{
    public class UserDto: BaseTransactionDto
    {
        public int? id { get; set; }
        public string? applicationUserId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string? phoneNumber { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? address { get; set; }
        public string? token { get; set; }
        public int? roleId { get; set; }
        public string? roleName { get; set; }        
    }
}

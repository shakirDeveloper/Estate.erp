using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estate.erp.dal.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}

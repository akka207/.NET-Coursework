using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParamContainers
{
    public class StaffRole
    {
        public int StaffId { get; set; }
        public int RoleId { get; set; }

        public StaffRole(int staffId, int roleId)
        {
            StaffId = staffId;
            RoleId = roleId;
        }
        public StaffRole()
        {

        }
    }
}

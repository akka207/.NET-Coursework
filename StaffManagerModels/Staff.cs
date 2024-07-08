using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManagerModels
{
    public class Staff
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public Schedule Schedule { get; set; }
        public int ScheduleId { get; set; }
    }
}

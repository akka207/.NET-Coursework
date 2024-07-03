using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManagerModels
{
    public class Schedule
    {
        public int Id { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}

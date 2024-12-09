using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParamContainers
{
    public class StafEvent
    {
        public Staff Staff { get; set; }
        public Event Event { get; set; }

        public StafEvent(Staff staff, Event newEvent)
        {
            Staff = staff;
            Event = newEvent;
        }
    }
}

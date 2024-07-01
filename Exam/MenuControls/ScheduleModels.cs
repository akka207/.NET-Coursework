using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.MenuControls
{
    public class Day
    {
        public DateTime Date { get; set; }
        public List<Event> Events { get; set; }
    }
    public class Week
    {
        public List<Day> Days { get; set; }
    }
    public class Month
    {
        public List<Week> Weeks { get; set; }
    }
}

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
        public bool IsSelected { get; set; } = false;
    }
    public class Week
    {
        public List<Day> Days { get; set; } = new List<Day>();
    }
    public class Month
    {
        public List<Week> Weeks { get; set; } = new List<Week>();
    }
}

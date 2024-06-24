using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffManagerModels
{
    public class Person
    {
        public int Id { get; set; }
        public string Login { get; set; } = default!;
        public string? HashedPasword { get; set; }
        public string? FullName { get; set; } = default!;
        public string? Phone { get; set; } = default!;
        public string? Email { get; set; } = default!;

    }
}

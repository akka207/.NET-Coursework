using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public interface IDBController
    {
        public Staff CurrentStaff { get; set; }

        public Task<bool> CheckPasswordAsync(string login, string password);

        public Task<Staff?> GetStaffAsync(string login);

        public Task SelectCurrentStaffAsync(string login);

        public void RemoveCurrentStaff();

        public Task UpdateCurrentStaffAsync();

        public Task<ICollection<Staff>> GetAllStaffAsync();

        public Task ChangeRoleAsync(Staff st, Role role);

        public Task RegisterPersonAsync(Person person, string password, int roleId = 3);

        public Task AddEventAsync(Staff staff, Event newEvent);

        public Task EditPersonInfoAsync(Person newPerson);

        public Task<bool> ChangePasswordAsync(string login, string oldPassword, string newPassword, bool oldPasswordRequired = true);
    }
}

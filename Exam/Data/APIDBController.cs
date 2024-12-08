using Exam.Services;
using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam.Data
{
    public class APIDBController : IDBController
    {
        private readonly ApiRequest _apiRequest = new ApiRequest();

        public Staff CurrentStaff { get; set; }

        public Task AddEventAsync(Staff staff, Event newEvent)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePasswordAsync(string login, string oldPassword, string newPassword, bool oldPasswordRequired = true)
        {
            throw new NotImplementedException();
        }

        public Task ChangeRoleAsync(Staff st, Role role)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckPasswordAsync(string login, string password)
        {
            if (GetMD5(password) == await _apiRequest.GetAsync("string", RequestHeader.PASSWORD, login))
            {
                return true;
            }
            return false;
        }

        public Task EditPersonInfoAsync(Person newPerson)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Staff>> GetAllStaffAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Staff?> GetStaffAsync(string login)
        {
            string json = await _apiRequest.GetAsync("collection", RequestHeader.STAFF, login);
            return JsonSerializer.Deserialize<Staff>(json);
        }

        public Task RegisterPersonAsync(Person person, string password, int roleId = 3)
        {
            throw new NotImplementedException();
        }

        public void RemoveCurrentStaff()
        {
            throw new NotImplementedException();
        }

        public Task SelectCurrentStaffAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCurrentStaffAsync()
        {
            throw new NotImplementedException();
        }

        private string GetMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}

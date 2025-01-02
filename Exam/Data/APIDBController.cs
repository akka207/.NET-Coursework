using Exam.Services;
using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonParamContainers;
using System.Collections;
using System.Security;

namespace Exam.Data
{
    public class APIDBController : IDBController
    {
        private readonly ApiRequest _apiRequest = new ApiRequest();
        private readonly AuthorizeRequest _authRequest = new AuthorizeRequest();

        public Staff CurrentStaff { get; set; }

        public async Task AddEventAsync(Staff staff, Event newEvent)
        {
            string json = JsonConvert.SerializeObject(new StafEvent(staff, newEvent));
            await _apiRequest.PostAsync(RequestHeader.ADD_EVENT, json);
        }

        public async Task<bool> ChangePasswordAsync(string login, string oldPassword, string newPassword, bool oldPasswordRequired = true)
        {
            string responce = ApiRequest.UNKNOWN;
            if (oldPasswordRequired)
            {
                string json = JsonConvert.SerializeObject(new StringArray([login, GetMD5(oldPassword), GetMD5(newPassword)]));
                responce = await _apiRequest.PostAsync(RequestHeader.CH_PASSWORD_SUBM, json);
            }
            else if (CurrentStaff.RoleId == Role.Admin.Id)
            {
                string json = JsonConvert.SerializeObject(new StringArray([login, GetMD5(newPassword)]));
                responce = await _apiRequest.PostAsync(RequestHeader.CH_PASSWORD, json);
            }

            if (responce != ApiRequest.UNKNOWN)
            {
                return Convert.ToBoolean(JsonConvert.DeserializeObject<Param>(responce).Value);
            }
            return false;
        }

        public async Task ChangeRoleAsync(Staff st, Role role)
        {
            string json = JsonConvert.SerializeObject(new StaffRole(st.Id, role.Id));
            st.Role = role;
            st.RoleId = role.Id;
            await _apiRequest.PostAsync(RequestHeader.CH_ROLE, json);
        }

        public async Task<bool> CheckPasswordAsync(string login, string password)
        {
            string cryptedPwd = GetMD5(password);

            await _authRequest.LoginAsync(login, cryptedPwd);

            if (cryptedPwd == await _apiRequest.GetAsync("string", RequestHeader.PASSWORD, string.Empty))
            {
                return true;
            }
            return false;
        }

        public async Task EditPersonInfoAsync(Person newPerson)
        {
            string json = JsonConvert.SerializeObject(newPerson);
            await _apiRequest.PostAsync(RequestHeader.EDT_PERSON, json);
        }

        public async Task<ICollection<Staff>> GetAllStaffAsync()
        {
            string json = await _apiRequest.GetAsync("collection", RequestHeader.ALL_STAFF, "");
            return JsonConvert.DeserializeObject<Staff[]>(json);
        }

        public async Task<Staff?> GetStaffAsync(string login)
        {
            string json = await _apiRequest.GetAsync("object", RequestHeader.STAFF, JsonBody(login));
            Staff staff = JsonConvert.DeserializeObject<Staff>(json);
            return staff;
        }

        public async Task RegisterPersonAsync(Person person, string password, int roleId = 3)
        {
            person.HashedPasword = GetMD5(password);
            Staff staff = new Staff() { Person = person, RoleId = roleId };
            string json = JsonConvert.SerializeObject(staff);
            await _apiRequest.PostAsync(RequestHeader.REG_PERSON, json);
        }

        public void RemoveCurrentStaff()
        {
            CurrentStaff = null;
        }

        public async Task SelectCurrentStaffAsync(string login)
        {
            CurrentStaff = await GetStaffAsync(login);
        }

        public async Task UpdateCurrentStaffAsync()
        {
            CurrentStaff = await GetStaffAsync(CurrentStaff.Person.Login);
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

        private string JsonBody(string param)
        {
            return JsonConvert.SerializeObject(new Param(param));
        }
    }
}

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
using System.Windows;
using System.Reflection.PortableExecutable;

namespace Exam.Data
{
    public class APIDBController : IDBController, IAPIController
    {
        private readonly ApiRequest _apiRequest = new ApiRequest();
        private readonly AuthorizeRequest _authRequest = new AuthorizeRequest();
        private readonly string tokensStorageFile = Config.Configuration["TokensStorageFile"] ?? string.Empty;

        private record Tokens(string jwt, string refreshToken);
        private record Payload(string sub);

        public Staff CurrentStaff { get; set; }


        public APIDBController()
        {
            _apiRequest.OnRefreshRequest += _apiRequest_OnRefreshRequest;
        }


        private async void _apiRequest_OnRefreshRequest(object? sender, EventArgs e)
        {
            MessageBox.Show("Your session was expired. Refreshing in process..", "Session expired", MessageBoxButton.OK, MessageBoxImage.Warning);

            await RefreshAsync();
        }

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

        public async Task<bool> LoginAsync(string login, string password)
        {
            if (await RefreshAsync())
            {
                return true;
            }

            string cryptedPwd = GetMD5(password);

            UserFileManager.Clear(tokensStorageFile);

            string loginResult = await _authRequest.LoginAsync(login, cryptedPwd);
            if (!loginResult.StartsWith("ERROR"))
            {
                UserFileManager.Write(tokensStorageFile, loginResult);

                if (cryptedPwd == await _apiRequest.GetAsync("string", RequestHeader.PASSWORD, string.Empty))
                {
                    return true;
                }

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

            Staff[]? staffs = null;

            try
            {
                staffs = JsonConvert.DeserializeObject<Staff[]>(json);
            }
            catch (Exception ex)
            {
                // Logger.Instance.ERROR(ex.Message);
            }

            return staffs == null ? new List<Staff>() : staffs;
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

        public async Task LogoutAsync()
        {
            await _authRequest.LogoutAsync();
            UserFileManager.Clear(tokensStorageFile);
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

        public async Task<bool> RefreshAsync()
        {
            string jsonTokens = UserFileManager.Read(tokensStorageFile);
            if (jsonTokens != string.Empty)
            {
                string refreshResult = await _authRequest.RefreshAsync(jsonTokens);

                if (!refreshResult.StartsWith("ERROR"))
                {
                    UserFileManager.Write(tokensStorageFile, refreshResult);
                    return true;
                }
            }

            return false;
        }

        public string GetLoginFromJWT()
        {
            string jwt = AuthorizeRequest.JWT;

            if (string.IsNullOrEmpty(jwt))
                return string.Empty;

            string[] parts = jwt.Split('.');
            if (parts.Length != 3)
                return string.Empty;

            string encPayload = parts[1];

            string? payloadJson = Encoding.ASCII.GetString(Convert.FromBase64String(encPayload));
            if (payloadJson != null)
            {
                Payload? payload = JsonConvert.DeserializeObject<Payload>(payloadJson);

                if (payload != null)
                {
                    return payload.sub;
                }
            }

            return string.Empty;
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

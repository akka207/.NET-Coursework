using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.RightsManagement;
using StaffManagerModels;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Exam.Services;

namespace Exam.Data
{
    public class LocalDBController : IDBController
    {
        public Staff CurrentStaff { get; set; }

        public LocalDBController()
        {
            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(userDirectory, "PManager");
            string dbName = "Staff.db";


            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            if (!File.Exists(Path.Combine(dbPath, dbName)))
            {
                InitDBAsync();
            }
        }

        private async void InitDBAsync()
        {
            using (var context = Config.DbContext)
            {
                await context.Database.MigrateAsync();

                await context.Roles.AddAsync(Role.Admin);
                await context.Roles.AddAsync(Role.Manager);
                await context.Roles.AddAsync(Role.User);

                await context.SaveChangesAsync();

                await RegisterPersonAsync(new Person() { Login = "admin", FullName = "Administator" }, "admin", Role.Admin.Id);
            }
        }

        public async Task<bool> CheckPasswordAsync(string login, string password)
        {
            using (var context = Config.DbContext)
            {
                var query = context.Persons.Where(p => p.Login == login);
                if ((await query.CountAsync()) > 0 && (await query.FirstAsync()).HashedPasword == GetMD5(password))
                {
                    return true;
                }
                return false;
            }
        }
        public bool CheckPassword(string login, string password)
        {
            using (var context = Config.DbContext)
            {
                var query = context.Persons.Where(p => p.Login == login);
                if (query.Count() > 0 && query.First().HashedPasword == GetMD5(password))
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<Staff?> GetStaffAsync(string login)
        {
            using (var context = Config.DbContext)
            {
                Staff staff = await context.Staffs.Where(s => s.Person.Login == login).Include(s => s.Person).Include(s => s.Schedule).Include(s => s.Role).Include(s=>s.Schedule.Events).FirstOrDefaultAsync();
                return staff;
            }
        }

        public async Task SelectCurrentStaffAsync(string login)
        {
            CurrentStaff = await GetStaffAsync(login);
        }

        public void RemoveCurrentStaff()
        {
            CurrentStaff = null;
        }

        public async Task UpdateCurrentStaffAsync()
        {
            using (var context = Config.DbContext)
            {
                CurrentStaff = await context.Staffs.Where(s => s.Id == CurrentStaff.Id).Include(s => s.Person).Include(s => s.Schedule).Include(s => s.Role).Include(s => s.Schedule.Events).FirstAsync();
            }
        }

        public async Task<ICollection<Staff>> GetAllStaffAsync()
        {
            using (var context = Config.DbContext)
            {
                List<Staff> list = await context.Staffs.Include(s => s.Person).Include(s => s.Schedule).Include(s => s.Role).ToListAsync();
                return list;
            }
        }

        public async Task ChangeRoleAsync(Staff st, Role role)
        {
            using (var context = Config.DbContext)
            {
                Staff staff = await context.Staffs.FirstOrDefaultAsync(s => s.Id == st.Id);

                st.RoleId = role.Id;
                st.Role = role;
                staff.RoleId = role.Id;

                await context.SaveChangesAsync();
            }
        }

        public async Task RegisterPersonAsync(Person person, string password, int roleId = 3)
        {
            using (var context = Config.DbContext)
            {
                person.HashedPasword = GetMD5(password);

                var schedule = new Schedule();
                var staff = new Staff();

                await context.Persons.AddAsync(person);
                await context.Schedules.AddAsync(schedule);
                await context.SaveChangesAsync();

                staff.PersonId = person.Id;
                staff.ScheduleId = schedule.Id;
                staff.RoleId = roleId;

                await context.Staffs.AddAsync(staff);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddEventAsync(Staff staff, Event newEvent)
        {
            using (var context = Config.DbContext)
            {
                newEvent.ScheduleId = staff.ScheduleId;
                await context.Events.AddAsync(newEvent);
                await context.SaveChangesAsync();
            }
        }

        public async Task EditPersonInfoAsync(Person newPerson)
        {
            using (var context = Config.DbContext)
            {
                Person? person = await context.Persons.FirstAsync(p => p.Id == newPerson.Id);
                if (person != null)
                {
                    person.Email = newPerson.Email;
                    person.Phone = newPerson.Phone;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> ChangePasswordAsync(string login, string oldPassword, string newPassword, bool oldPasswordRequired = true)
        {
            using (var context = Config.DbContext)
            {
                Person? findedPerson = await context.Persons.FirstOrDefaultAsync(p => p.Login == login);

                if (oldPasswordRequired)
                {
                    if (findedPerson != null && await CheckPasswordAsync(login, oldPassword))
                    {
                        findedPerson.HashedPasword = GetMD5(newPassword);
                        await context.SaveChangesAsync();
                        return true;
                        
                    }
                }
                else if (CurrentStaff.RoleId == Role.Admin.Id && findedPerson != null)
                {
                    findedPerson.HashedPasword = GetMD5(newPassword);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
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
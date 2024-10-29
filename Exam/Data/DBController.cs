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
    public static class DBController
    {
        public static Staff CurrentStaff { get; private set; }

        static DBController()
        {
            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(userDirectory, "PManager");
            string dbName = "Staff.db";

            //Logger.Instance.INFO($"Generated path: {dbPath}");

            if (!Directory.Exists(dbPath))
            {
                //Logger.Instance.INFO($"Creating folder PManager");
                Directory.CreateDirectory(dbPath);
                //Logger.Instance.INFO($"Succesful creating");
            }

            if (!File.Exists(Path.Combine(dbPath, dbName)))
            {
                InitDB();
            }
        }

        private static void InitDB()
        {
            //Logger.Instance.INFO($"DB initialization");
            try
            {
                using (var context = Config.DbContext)
                {
                    //Logger.Instance.INFO($"Start migrating");
                    context.Database.Migrate();

                    //Logger.Instance.INFO($"Adding roles");
                    context.Roles.Add(Role.Admin);
                    context.Roles.Add(Role.Manager);
                    context.Roles.Add(Role.User);

                    //Logger.Instance.INFO($"Saving changes");
                    context.SaveChanges();

                    //Logger.Instance.INFO($"Adding admin account");
                    RegisterPerson(new Person() { Login = "admin", FullName = "Administator" }, "admin", Role.Admin.Id);

                    //Logger.Instance.INFO($"Succesful initialization");
                }
            }
            catch (Exception ex)
            {
                //Logger.Instance.ERROR($"{ex.Message}");
            }
        }

        public static bool CheckPassword(string login, string password)
        {
            //Logger.Instance.INFO($"Start checking password");
            //Logger.Instance.DEBUG($"Checking for {login}:{password}");

            using (var context = Config.DbContext)
            {
                var query = context.Persons.Where(p => p.Login == login);
                if (query.Count() > 0 && query.First().HashedPasword == GetMD5(password))
                {
                    //Logger.Instance.DEBUG($"Succesful check");
                    return true;
                }
                //Logger.Instance.DEBUG($"Unsuccesful check");
                return false;
            }
        }

        public static Staff? GetStaff(string login, string password)
        {
            //Logger.Instance.INFO($"Fetching staff for login: {login}");
            if (CheckPassword(login, password))
            {
                //Logger.Instance.INFO($"Password validated for login: {login}");
                using (var context = Config.DbContext)
                {
                    Staff staff = context.Staffs.Where(s => s.Person.Login == login).FirstOrDefault();
                    if (staff != null)
                    {
                        //Logger.Instance.DEBUG($"Staff found for login: {login}");
                        if (staff.Person == null)
                        {
                            staff.Person = context.Persons.Where(p => p.Id == staff.PersonId).FirstOrDefault();
                        }
                        if (staff.Role == null)
                        {
                            staff.Role = context.Roles.Where(r => r.Id == staff.RoleId).FirstOrDefault();
                        }
                        if (staff.Schedule == null)
                        {
                            staff.Schedule = context.Schedules.Where(s => s.Id == staff.ScheduleId).FirstOrDefault();
                            staff.Schedule.Events = context.Events.Where(e => e.ScheduleId == staff.Schedule.Id).ToArray();
                        }
                    }
                    return staff;
                }
            }
            //Logger.Instance.ERROR($"Failed to validate password for login: {login}");
            return null;
        }

        public static void SelectCurrentStaff(string login, string password)
        {
            //Logger.Instance.INFO($"Selecting current staff for login: {login}");
            CurrentStaff = GetStaff(login, password);
        }

        public static void RemoveCurrentStaff()
        {
            //Logger.Instance.INFO("Removing current staff");
            CurrentStaff = null;
        }

        public static void UpdateCurrentStaff()
        {
            //Logger.Instance.INFO("Updating current staff");
            using (var context = Config.DbContext)
            {
                CurrentStaff = context.Staffs.Where(s => s.Id == CurrentStaff.Id).First();

                if (CurrentStaff.Person == null)
                {
                    CurrentStaff.Person = context.Persons.Where(p => p.Id == CurrentStaff.PersonId).FirstOrDefault();
                }
                if (CurrentStaff.Role == null)
                {
                    CurrentStaff.Role = context.Roles.Where(r => r.Id == CurrentStaff.RoleId).FirstOrDefault();
                }
                if (CurrentStaff.Schedule == null)
                {
                    CurrentStaff.Schedule = context.Schedules.Where(s => s.Id == CurrentStaff.ScheduleId).FirstOrDefault();
                    CurrentStaff.Schedule.Events = context.Events.Where(e => e.ScheduleId == CurrentStaff.Schedule.Id).ToArray();
                }
            }
        }

        public static ICollection<Staff> GetAllStaff()
        {
            using (var context = Config.DbContext)
            {
                //Logger.Instance.INFO("Fetching all staff");
                List<Staff> list = context.Staffs.Include(s => s.Person).Include(s => s.Schedule).Include(s => s.Role).ToList();

                //list = context.Staffs.Select(s => new Staff()
                //{
                //    Id = s.Id,
                //    PersonId = s.PersonId,
                //    Person = context.Persons.Where(p => p.Id == s.PersonId).FirstOrDefault(),
                //    ScheduleId = s.ScheduleId,
                //    Schedule = context.Schedules.Where(sc => sc.Id == s.ScheduleId).FirstOrDefault(),
                //    RoleId = s.RoleId,
                //    Role = context.Roles.Where(r => r.Id == s.RoleId).FirstOrDefault()
                //}).ToList();

                //foreach (var staff in list)
                //{
                //    staff.Person = context.Persons.Where(p => p.Id == staff.PersonId).FirstOrDefault();
                //    staff.Role = context.Roles.Where(r => r.Id == staff.RoleId).FirstOrDefault();
                //    staff.Schedule = context.Schedules.Where(s => s.Id == staff.ScheduleId).FirstOrDefault();
                //    staff.Schedule.Events = context.Events.Where(e => e.ScheduleId == staff.Schedule.Id).ToArray();
                //}

                return list;
            }
        }

        public static void ChangeRole(Staff st, Role role)
        {
            //Logger.Instance.INFO($"Changing role for staff ID: {st.Id} to role ID: {role.Id}");
            using (var context = Config.DbContext)
            {
                Staff staff = context.Staffs.FirstOrDefault(s => s.Id == st.Id);

                st.RoleId = role.Id;
                st.Role = role;
                staff.RoleId = role.Id;

                context.SaveChanges();
            }
        }

        public static void RegisterPerson(Person person, string password, int roleId = 3)
        {
            //Logger.Instance.INFO($"Registering new person with login: {person.Login}");
            using (var context = Config.DbContext)
            {
                person.HashedPasword = GetMD5(password);

                var schedule = new Schedule();
                var staff = new Staff();

                context.Persons.Add(person);
                context.Schedules.Add(schedule);
                context.SaveChanges();

                staff.PersonId = person.Id;
                staff.ScheduleId = schedule.Id;
                staff.RoleId = roleId;

                context.Staffs.Add(staff);
                context.SaveChanges();
                //Logger.Instance.INFO($"Registration successful for login: {person.Login}");
            }
        }

        public static void AddEvent(Staff staff, Event newEvent)
        {
            //Logger.Instance.INFO($"Adding event for staff ID: {staff.Id}");
            using (var context = Config.DbContext)
            {
                newEvent.ScheduleId = staff.ScheduleId;
                context.Events.Add(newEvent);
                context.SaveChanges();
                staff.Schedule.Events = context.Events.Where(e => e.ScheduleId == staff.Schedule.Id).ToArray();
            }
        }

        public static void EditPersonInfo(Person newPerson)
        {
            //Logger.Instance.INFO($"Editing person info for ID: {newPerson.Id}");
            using (var context = Config.DbContext)
            {
                Person? person = context.Persons.First(p => p.Id == newPerson.Id);
                if (person != null)
                {
                    person.Email = newPerson.Email;
                    person.Phone = newPerson.Phone;
                    context.SaveChanges();
                    //Logger.Instance.INFO($"Person info updated for ID: {newPerson.Id}");
                }
                else
                {
                    //Logger.Instance.WARN($"Person not found for ID: {newPerson.Id}");
                }
            }
        }

        public static bool ChangePassword(string login, string oldPassword, string newPassword, bool oldPasswordRequired = true)
        {
            //Logger.Instance.INFO($"Attempting password change for login: {login}");
            using (var context = Config.DbContext)
            {
                Person? findedPerson = context.Persons.FirstOrDefault(p => p.Login == login);

                if (oldPasswordRequired)
                {
                    if (findedPerson != null && CheckPassword(login, oldPassword))
                    {
                        findedPerson.HashedPasword = GetMD5(newPassword);
                        context.SaveChanges();
                        //Logger.Instance.INFO($"Password changed successfully for login: {login}");
                        return true;
                    }
                }
                else if (CurrentStaff.RoleId == Role.Admin.Id)
                {
                    findedPerson.HashedPasword = GetMD5(newPassword);
                    context.SaveChanges();
                    //Logger.Instance.INFO($"Password changed successfully for login: {login} by admin");
                    return true;
                }
                //Logger.Instance.ERROR($"Password change failed for login: {login}");
                return false;
            }
        }

        private static string GetMD5(string input)
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
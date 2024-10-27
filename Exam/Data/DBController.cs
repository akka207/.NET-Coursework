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

namespace Exam.Data
{
    public static class DBController
    {
        public static Staff CurrentStaff { get; private set; }

        static DBController()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"PManager");
            string dbName = "Staff.db";

            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            if (!File.Exists(Path.Combine(dbPath, dbName)))
            {
                InitDB();
            }
        }

        private static void InitDB()
        {
            using (var context = Config.DbContext)
            {
                context.Database.Migrate();

                context.Roles.Add(Role.Admin);
                context.Roles.Add(Role.Manager);
                context.Roles.Add(Role.User);
                context.SaveChanges();

                RegisterPerson(new Person() { Login = "admin", FullName = "Administator" }, "admin", Role.Admin.Id);
            }
        }

        public static bool CheckPassword(string login, string password)
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

        public static Staff? GetStaff(string login, string password)
        {
            if (CheckPassword(login, password))
            {
                using (var context = Config.DbContext)
                {
                    Staff staff = context.Staffs.Where(s => s.Person.Login == login).FirstOrDefault();
                    if (staff != null)
                    {
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
                    // Це я своїх вчу гітхаб юзать, не звертай увагу
                }
            }
            return null;
        }

        public static void SelectCurrentStaff(string login, string password)
        {
            CurrentStaff = GetStaff(login, password);
        }

        public static void RemoveCurrentStaff()
        {
            CurrentStaff = null;
        }

        public static void UpdateCurrentStaff()
        {
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
            }
        }

        public static void AddEvent(Staff staff, Event newEvent)
        {
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
            using (var context = Config.DbContext)
            {
                Person? person = context.Persons.First(p => p.Id == newPerson.Id);
                if (person != null)
                {
                    person.Email = newPerson.Email;
                    person.Phone = newPerson.Phone;
                    context.SaveChanges();
                }
            }
        }

        public static bool ChangePassword(string login, string oldPassword, string newPassword, bool oldPasswordRequired = true)
        {
            using (var context = Config.DbContext)
            {
                Person? findedPerson = context.Persons.FirstOrDefault(p => p.Login == login);

                if (oldPasswordRequired)
                {
                    if (findedPerson != null && CheckPassword(login, oldPassword))
                    {
                        findedPerson.HashedPasword = GetMD5(newPassword);
                        context.SaveChanges();
                        return true;
                    }
                }
                else if (CurrentStaff.RoleId == Role.Admin.Id)
                {
                    findedPerson.HashedPasword = GetMD5(newPassword);
                    context.SaveChanges();
                    return true;
                }

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

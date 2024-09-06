using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.RightsManagement;
using StaffManagerModels;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;

namespace Exam.Data
{
    public class DBController
    {
        public static Staff CurrentStaff { get; private set; }

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
                List<Staff> list = context.Staffs.ToList();

                foreach (var staff in list)
                {
                    staff.Person = context.Persons.Where(p => p.Id == staff.PersonId).FirstOrDefault();
                    staff.Role = context.Roles.Where(r => r.Id == staff.RoleId).FirstOrDefault();
                    staff.Schedule = context.Schedules.Where(s => s.Id == staff.ScheduleId).FirstOrDefault();
                    staff.Schedule.Events = context.Events.Where(e => e.ScheduleId == staff.Schedule.Id).ToArray();
                }

                return list;
            }
        }

        public static void RegisterPerson(Person person, string password)
        {
            using (var context = Config.DbContext)
            {
                person.HashedPasword = GetMD5(password);
                context.Persons.Add(person);
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

        public static bool ChangePassword(string login, string oldPassword, string newPassword)
        {
            using (var context = Config.DbContext)
            {
                Person? findedPerson = context.Persons.FirstOrDefault(p => p.Login == login);

                if (findedPerson != null && CheckPassword(login, oldPassword))
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

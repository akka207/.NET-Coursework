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
                }
            }
            return null;
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

        public static void EditPersonInfo(Person newPerson)
        {
            using (var context = Config.DbContext)
            {
                if (context.Persons.Any(p => p.Id == newPerson.Id))
                {
                    Person person = context.Persons.First(p => p.Id == newPerson.Id);
                    if (person != null)
                    {
                        person.Email = newPerson.Email;
                        person.Phone = newPerson.Phone;
                        context.SaveChanges();
                    }
                }
            }
        }

        public static bool ChangePassword(string login, string oldPassword, string newPassword)
        {
            using (var context = Config.DbContext)
            {
                try
                {
                    if (context.Persons.Any(p => p.Login == login))
                    {
                        Person findedPerson = context.Persons.First(p => p.Login == login);

                        if (findedPerson != null && CheckPassword(login, oldPassword))
                        {
                            findedPerson.HashedPasword = GetMD5(newPassword);
                            return true;
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
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

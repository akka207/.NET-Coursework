using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.RightsManagement;
using StaffManagerModels;
using System.Runtime.CompilerServices;

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
        public static async Task<bool> CheckPasswordAsync(string login, string password)
        {
            using (var context = Config.DbContext)
            {
                return await Task.Run(() =>
                {
                    return CheckPassword(login, password);
                });
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
        public static async Task RegisterPersonAsync(Person person, string password)
        {
            using (var context = Config.DbContext)
            {
                person.HashedPasword = GetMD5(password);
                await context.Persons.AddAsync(person);
                await context.SaveChangesAsync();
            }
        }

        public static List<Event> GetEvents(Staff staff, DateTime startDate, DateTime endDate)
        {
            using (var context = Config.DbContext)
            {
                return context.Staffs
                    .Where(s => s.Id == staff.Id)
                    .First().Schedule.Events
                    .Where(e => e.EndDateTime >= startDate && e.StartDateTime <= endDate)
                    .ToList();
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

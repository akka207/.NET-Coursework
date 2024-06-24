using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StaffManagerModels
{
    public static class DataValidators
    {
        public static List<string> ValidatePerson(Person person)
        {
            List<string> invalidDataMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(person.Login))
                invalidDataMessages.Add("Login can't be empty or spaces!");

            if (string.IsNullOrWhiteSpace(person.FullName))
                invalidDataMessages.Add("Full Name can't be empty or spaces!");

            if (string.IsNullOrWhiteSpace(person.Phone))
                invalidDataMessages.Add("Phone can't be empty or spaces!");

            if (string.IsNullOrWhiteSpace(person.Email))
                invalidDataMessages.Add("Email can't be empty or spaces!");

            if (person.Login.Length < 5)
                invalidDataMessages.Add("Login must be at least 5 characters long!");

            if (!Regex.IsMatch(person.FullName, @"\s"))
                invalidDataMessages.Add("Full Name must contain both first and last name!");

            if (!Regex.IsMatch(person.Phone, @"^\+?[0-9\s\-]+$"))
                invalidDataMessages.Add("Phone can only contain numbers, spaces, or hyphens, and can start with a '+'!");

            if (!person.Email.Contains('@'))
                invalidDataMessages.Add("Email must contain '@' symbol!");

            if (!Regex.IsMatch(person.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                invalidDataMessages.Add("Email format is invalid!");

            if (person.Phone.Length != 10)
                invalidDataMessages.Add("Phone number must be 10 characters long!");

            return invalidDataMessages;
        }

        public static List<string> ValidatePassword(string password, string passwordConfirmation)
        {
            List<string> invalidPasswordMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
                invalidPasswordMessages.Add("Password can't be empty or spaces!");

            if (password != passwordConfirmation)
                invalidPasswordMessages.Add("Password and confirmation do not match!");

            if (password.Length < 8)
                invalidPasswordMessages.Add("Password must be at least 8 characters long!");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                invalidPasswordMessages.Add("Password must contain at least one uppercase letter!");

            if (!Regex.IsMatch(password, @"[a-z]"))
                invalidPasswordMessages.Add("Password must contain at least one lowercase letter!");

            if (!Regex.IsMatch(password, @"[0-9]"))
                invalidPasswordMessages.Add("Password must contain at least one digit!");

            if (!Regex.IsMatch(password, @"[\W_]"))
                invalidPasswordMessages.Add("Password must contain at least one special character!");

            if (password.Contains(' '))
                invalidPasswordMessages.Add("Password cannot contain spaces!");

            return invalidPasswordMessages;
        }
    }
}

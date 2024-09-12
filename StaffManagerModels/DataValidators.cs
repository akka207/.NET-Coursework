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
        public record ValidationResult(bool IsAllOK, Dictionary<Fields, List<string>> Data);

        public enum Fields
        {
            Login,
            Password,
            FullName,
            CPassword,
            Phone,
            Email
        }

        public static ValidationResult ValidatePerson(Person person)
        {
            Dictionary<Fields, List<string>> invalidDataMessages = new Dictionary<Fields, List<string>>();
            bool isAllOK = true;
            foreach (var type in (Fields[])Enum.GetValues(typeof(Fields)))
            {
                invalidDataMessages.Add(type, new List<string>());
            }

            if (string.IsNullOrWhiteSpace(person.Login))
            {
                invalidDataMessages[Fields.Login].Add("Login can't be empty or spaces!");
                isAllOK = false;
            }

            if (string.IsNullOrWhiteSpace(person.FullName))
            {
                invalidDataMessages[Fields.Login].Add("Full Name can't be empty or spaces!");
                isAllOK = false;
            }

            if (string.IsNullOrWhiteSpace(person.Phone))
            {
                invalidDataMessages[Fields.Phone].Add("Phone can't be empty or spaces!");
                isAllOK = false;
            }

            if (string.IsNullOrWhiteSpace(person.Email))
            {
                invalidDataMessages[Fields.Email].Add("Email can't be empty or spaces!");
                isAllOK = false;
            }

            if (person.Login.Length < 5)
            {
                invalidDataMessages[Fields.Login].Add("Login must be at least 5 characters long!");
                isAllOK = false;
            }

            if (!Regex.IsMatch(person.FullName, @"\s"))
            {
                invalidDataMessages[Fields.FullName].Add("Full Name must contain both first and last name!");
                isAllOK = false;
            }

            if (!Regex.IsMatch(person.Phone, @"^\+?[0-9\s\-]+$"))
            {
                invalidDataMessages[Fields.Phone].Add("Phone can only contain numbers, spaces, or hyphens, and can start with a '+'!");
                isAllOK = false;
            }

            if (!person.Email.Contains('@'))
            {
                invalidDataMessages[Fields.Email].Add("Email must contain '@' symbol!");
				isAllOK = false;
			}

			if (!Regex.IsMatch(person.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                invalidDataMessages[Fields.Email].Add("Email format is invalid!");
				isAllOK = false;
			}

			if (person.Phone.Length != 10)
            {
                invalidDataMessages[Fields.Phone].Add("Phone number must be 10 characters long!");
				isAllOK = false;
			}

            return new ValidationResult(isAllOK, invalidDataMessages);
		}

        public static ValidationResult ValidatePassword(string password, string passwordConfirmation)
        {
            Dictionary<Fields, List<string>> invalidPasswordMessages = new Dictionary<Fields, List<string>>();
			bool isAllOK = true;
			invalidPasswordMessages.Add(Fields.Password, new List<string>());
			invalidPasswordMessages.Add(Fields.CPassword, new List<string>());
			
			

			if (string.IsNullOrWhiteSpace(password))
			{
				invalidPasswordMessages[Fields.Password].Add("Password can't be empty or spaces!");
				isAllOK = false;
			}
			else
			{
				if (password.Length < 8)
				{
					invalidPasswordMessages[Fields.Password].Add("Password must be at least 8 characters long!");
					isAllOK = false;
				}

				if (!Regex.IsMatch(password, @"[A-Z]"))
				{
					invalidPasswordMessages[Fields.Password].Add("Password must contain at least one uppercase letter!");
					isAllOK = false;
				}

				if (!Regex.IsMatch(password, @"[a-z]"))
				{
					invalidPasswordMessages[Fields.Password].Add("Password must contain at least one lowercase letter!");
					isAllOK = false;
				}

				if (!Regex.IsMatch(password, @"[0-9]"))
				{
					invalidPasswordMessages[Fields.Password].Add("Password must contain at least one digit!");
					isAllOK = false;
				}

				if (!Regex.IsMatch(password, @"[\W_]"))
				{
					invalidPasswordMessages[Fields.Password].Add("Password must contain at least one special character!");
					isAllOK = false;
				}

				if (password.Contains(' '))
				{
					invalidPasswordMessages[Fields.Password].Add("Password cannot contain spaces!");
					isAllOK = false;
				}

				if (password != passwordConfirmation)
				{
					invalidPasswordMessages[Fields.CPassword].Add("Password and confirmation do not match!");
					isAllOK = false;
				}
			}

			return new ValidationResult(isAllOK, invalidPasswordMessages);
		}
    }
}

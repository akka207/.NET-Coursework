using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Generators
{
    internal class PasswordGenerator
    {
        private static Random random = new Random();

        public static string GeneratePassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            int passwordLength = random.Next(8, 13); 
            var password = new char[passwordLength];
            password[0] = upperCase[random.Next(upperCase.Length)];
            password[1] = lowerCase[random.Next(lowerCase.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = specialChars[random.Next(specialChars.Length)];
            string allChars = upperCase + lowerCase + digits;
            for (int i = 4; i < password.Length; i++)
                password[i] = allChars[random.Next(allChars.Length)];
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

    }
}

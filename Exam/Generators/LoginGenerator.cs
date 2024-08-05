using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Generators
{
    internal class LoginGenerator
    {
        private static Random random = new Random();
        private static string Transliterate(string input)
        {
            var mapping = new[]
            {
            new { UA = "а", EN = "a" }, new { UA = "б", EN = "b" }, new { UA = "в", EN = "v" },
            new { UA = "г", EN = "h" }, new { UA = "ґ", EN = "g" }, new { UA = "д", EN = "d" },
            new { UA = "е", EN = "e" }, new { UA = "є", EN = "ye" }, new { UA = "ж", EN = "zh" },
            new { UA = "з", EN = "z" }, new { UA = "и", EN = "y" }, new { UA = "і", EN = "i" },
            new { UA = "ї", EN = "yi" }, new { UA = "й", EN = "y" }, new { UA = "к", EN = "k" },
            new { UA = "л", EN = "l" }, new { UA = "м", EN = "m" }, new { UA = "н", EN = "n" },
            new { UA = "о", EN = "o" }, new { UA = "п", EN = "p" }, new { UA = "р", EN = "r" },
            new { UA = "с", EN = "s" }, new { UA = "т", EN = "t" }, new { UA = "у", EN = "u" },
            new { UA = "ф", EN = "f" }, new { UA = "х", EN = "kh" }, new { UA = "ц", EN = "ts" },
            new { UA = "ч", EN = "ch" }, new { UA = "ш", EN = "sh" }, new { UA = "щ", EN = "shch" },
            new { UA = "ю", EN = "yu" }, new { UA = "я", EN = "ya" }, new { UA = "Я", EN = "Ya" },
            new { UA = "А", EN = "A" }, new { UA = "Б", EN = "B" }, new { UA = "В", EN = "V" },
            new { UA = "Г", EN = "H" }, new { UA = "Ґ", EN = "G" }, new { UA = "Д", EN = "D" },
            new { UA = "Е", EN = "E" }, new { UA = "Є", EN = "Ye" }, new { UA = "Ж", EN = "Zh" },
            new { UA = "З", EN = "Z" }, new { UA = "И", EN = "Y" }, new { UA = "І", EN = "I" },
            new { UA = "Ї", EN = "Yi" }, new { UA = "Й", EN = "Y" }, new { UA = "К", EN = "K" },
            new { UA = "Л", EN = "L" }, new { UA = "М", EN = "M" }, new { UA = "Н", EN = "N" },
            new { UA = "О", EN = "O" }, new { UA = "П", EN = "P" }, new { UA = "Р", EN = "R" },
            new { UA = "С", EN = "S" }, new { UA = "Т", EN = "T" }, new { UA = "У", EN = "U" },
            new { UA = "Ф", EN = "F" }, new { UA = "Х", EN = "Kh" }, new { UA = "Ц", EN = "Ts" },
            new { UA = "Ч", EN = "Ch" }, new { UA = "Ш", EN = "Sh" }, new { UA = "Щ", EN = "Shch" },
            new { UA = "Ю", EN = "Yu" }, 
            };

            foreach (var map in mapping)
            {
                input = input.Replace(map.UA, map.EN);
            }

            return input;
        }
        private static string GenerateRandomAddition()
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";

            var randomLetters = new string(Enumerable.Repeat(letters, 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var randomDigits = new string(Enumerable.Repeat(digits, 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomLetters + randomDigits;
        }
        public static string GenerateLogin(string fullName)
        {
            var parts = fullName.Split(' ');
            try
            {
                if (parts.Length < 2)
                    throw new ArgumentException("FullName must contain at least a first name and a surname");
                var surname = parts[1];
                var firstFourLetters = surname.Substring(0, Math.Min(4, surname.Length));
                var transliterated = Transliterate(firstFourLetters);
                var randomLetters = GenerateRandomAddition();

                return $"{transliterated}_{randomLetters}";
            }
            catch
            {
                // не дай боже якийсь інвалід не введе ім'я
            }

            return "Error";
        }
    }
}

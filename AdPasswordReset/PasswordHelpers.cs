using System;
using System.Linq;

namespace AdPasswordReset
{
    class PasswordHelpers
    {
        public static string Generate()
        {
            var password = "";

            for (int i = 0; i < 4; i++)
            {
                if (_random.Next(0, 2) == 0)
                    password += GetChars(Consonants, 1);
                else
                    password += GetChars(DoubleConsonants, 2);

                password += GetChars(Vowels, 1);

                if (i == 1)
                    password += "-";
            }

            if (_random.Next(0, 2) == 2)
                password = GetDigits() + password;
            else
                password = password + GetDigits();

            return password.First().ToString().ToUpper() + password.Substring(1);
        }

        private static string GetDigits()
        {
            var digits = "";
            var maxDigits = _random.Next(2, 5);

            for (int i = 0; i < maxDigits; i++)
            {
                digits += GetChars(Numbers, 1);
            }
            return digits;
        }

        private static string GetChars(string list, int count)
        {
            var pos = _random.Next(0, list.Length / count);
            return list.Substring(pos * count, count);
        }

        private static Random _random = new Random();

        private const string Vowels = "aeuy";
        private const string Consonants = "bcdfghjkmnpqrstvwxz";
        private const string DoubleConsonants = "blbrflfrglgrklkrplprslsttr";
        private const string Numbers = "123456789";
    }
}

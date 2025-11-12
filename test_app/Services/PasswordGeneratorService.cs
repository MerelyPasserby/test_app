using System.Text;
using System;

namespace test_app.Services
{
    public class PasswordGeneratorService
    {
        public static readonly string Digits = "0123456789";
        public static readonly string SmallLetters = "abcdefghijklmnopqrstuvwxyz";
        public static readonly string CapitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static readonly string Symbols = @"!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~";

        public static readonly string Letters = SmallLetters + CapitalLetters;
        public static readonly string LettersAndDigits = Letters + Digits;
        public static readonly string All = Letters + Digits + Symbols;

        private readonly Random _rnd = new Random();

        public string Generate(string alphabet, int length)
        {
            if (string.IsNullOrEmpty(alphabet))
                throw new ArgumentException("Alphabet must be chosen");

            var sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = _rnd.Next(alphabet.Length);
                sb.Append(alphabet[index]);
            }

            return sb.ToString();
        }
    }
}

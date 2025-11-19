using System;
using System.Linq;

namespace SerilogExample
{
    public class RandomStringGenerator
    {
        private static readonly Random _random = new Random();
        private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateRandomString()
        {
            int length = _random.Next(20, 51); // Random length between 20-50 inclusive
            return new string(Enumerable.Range(0, length)
                .Select(_ => ValidChars[_random.Next(ValidChars.Length)])
                .ToArray());
        }

        public static int GetRandomNumber()
        {
            int min = 1000, max = 5000;
            return _random.Next(min, max);
        }
    }
}
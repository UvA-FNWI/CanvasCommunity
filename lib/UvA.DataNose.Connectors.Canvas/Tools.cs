#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

#endregion

namespace UvA.Utilities
{
    public static partial class Tools
    {
        public static T ParseEnum<T>(string str) => (T)Enum.Parse(typeof(T), str);

        public static T? ParseEnumOrNull<T>(string str) where T : struct
            => Enum.TryParse<T>(str, out T val) ? (T?)val : null;

        public static Color ParseColor(string s)
        {
            var c = s.Split(',').Select(r => int.Parse(r)).ToArray();
            return Color.FromArgb(c[0], c[1], c[2]);
        }

        public static IEnumerable<T[]> Split<T>(this IEnumerable<T> input, int count)
        {
            var total = input.Count();
            var i = 0;
            while (i * count < total)
                yield return input.Skip((i++) * count).Take(count).ToArray();
        }

        public static List<T>[] SplitToColumns<T>(this List<T> list, int cols)
        {
            var ret = new List<List<T>> { };
            if (list.Count() < cols)
                return list.Select(item => new List<T>() { item }).ToArray();
            else
                for (var i = 0; i < cols; i++)
                    ret.Add(new List<T>() { });

            var j = 0;
            foreach (var item in list)
                ret[j++ % cols].Add(item);

            return ret.ToArray();
        }

        public static T[] GetEnumEntries<T>() => (T[])Enum.GetValues(typeof(T));

        public static DateTime? ParseDateOrNull(string date, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;
            DateTime test;
            return DateTime.TryParse(date, culture, DateTimeStyles.None, out test) ? (DateTime?) test : null;
        }

        public static int? ParseIntOrNull(string text)
            => int.TryParse(text, out int i) ? (int?)i : null;

        public static double? ParseDoubleOrNull(string text)
        {
            double test;
            return double.TryParse(text, out test) ? (double?)test : null;
        }

        /// <summary>
        /// Builds a string out of several by inserting a separator between them
        /// </summary>
        /// <param name="separator">Separator to use</param>
        /// <param name="strings">Set of strings</param>
        public static string ToSeparatedStringStrict(string separator, params string[] strings)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strings.Length; i++)
            {
                builder.Append(strings[i]);
                builder.Append(separator);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Builds a string out of several by inserting a separator between them, skipping empty strings and null values
        /// </summary>
        /// <param name="separator">Separator to use</param>
        /// <param name="strings">Set of strings</param>
        /// <returns></returns>
        public static string ToSeparatedString(string separator, params string[] strings)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i] == null)
                    continue;
                builder.Append(strings[i]);
                if (!string.IsNullOrEmpty(strings[i]) && strings.Skip(i + 1).Any(r => r != "" && r != null))
                    builder.Append(separator);
            }
            return builder.ToString();
        }

        public static bool IsEqual(object[] o1, object[] o2)
        {
            for (int i = 0; i < o1.Length; i++)
                if (!IsEqual(o1[i], o2[i]))
                    return false;
            return true;
        }

        public static bool IsEqual(object o1, object o2)
        {
            if (o1 == null || o2 == null)
                return o1 == o2;
            return o1.Equals(o2);
        }

        /// <summary>
        /// Computes the greatest common divisor of two numbers
        /// </summary>
        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int t = b;
                b = a%b;
                a = t;
            }
            return a;
        }

        /// <summary>
        /// Computes the least common multiple of a set of numbers
        /// </summary>
        public static int LCM(params int[] numbers)
        {
            if (numbers.Length == 2)
                return Math.Abs(numbers[0]*numbers[1])/GCD(numbers[0], numbers[1]);
            if (numbers.Length > 2)
                return LCM(numbers.Skip(2).Append(LCM(numbers[0], numbers[1])).ToArray());
            throw new ArgumentException("Unexpected input");
        }

        /// <summary>
        /// Generates an array containing all numbers in a specific range
        /// </summary>
        /// <param name="min">Start of range</param>
        /// <param name="max">End of range, inclusive</param>
        public static int[] Range(int min, int max, int step = 1)
        {
            List<int> result = new List<int>();
            for (int i = min; i <= max; i += step)
                result.Add(i);
            return result.ToArray();
        }

        /// <summary>
        /// Generates an array containing all numbers in a specific range
        /// </summary>
        /// <param name="min">Start of range</param>
        /// <param name="max">End of range, inclusive</param>
        public static double[] Range(double min, double max, double step = 1)
        {
            List<double> result = new List<double>();
            for (double i = min; i <= max; i += step)
                result.Add(i);
            return result.ToArray();
        }

        /// <summary>
        /// Generates an array containing all numbers in a specific range
        /// </summary>
        /// <param name="min">Start of range</param>
        /// <param name="max">End of range, inclusive</param>
        public static int[] Range(int min, int max) => Range(min, max, 1);
    }
}
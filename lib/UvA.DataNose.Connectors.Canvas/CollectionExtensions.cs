using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Utilities
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of objects to the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">A list</param>
        /// <param name="objects">Objects to add</param>
        public static void AddRange<T>(this List<T> list, params T[] objects)
        {
            list.AddRange(objects.AsEnumerable());
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var s in list)
                action(s);
        }

        /// <summary>
        /// Adds an object to a collection
        /// </summary>
        /// <param name="col">A collection of objects</param>
        /// <param name="item">Object to add</param>
        /// <param name="atStart">True to add the object at beginning, false to add at the end</param>
        /// <param name="unique">If true, only add the item if it is not yet in the collection</param>
        /// <returns></returns>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> col, T item, bool atStart = false, bool unique = false)
        {
            if (unique && col.Contains(item))
                return col;
            return atStart ? new[] { item }.Concat(col) : col.Concat(new[] { item });
        }

        /// <summary>
        /// Removes an object from a collection. Has no effect if the collection does not contain the object
        /// </summary>
        /// <param name="col">A collection of objects</param>
        /// <param name="item">Object to remove</param>
        /// <returns></returns>
        public static IEnumerable<T> Discard<T>(this IEnumerable<T> col, T item)
        {
            return col.Except(new[] { item });
        }

        /// <summary>
        /// Gets the value corresponding to the specified key from the dictionary or returns the default value if the key is not found
        /// </summary>
        public static TValue ValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value = default(TValue);
            if (key == null)
                return value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// Converts a list of objects to a separated string
        /// </summary>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <param name="list">The list to convert</param>
        /// <param name="displayFunction">A function that gets converts each object to a string</param>
        /// <param name="separator">The separator to use between the objects</param>
        /// <param name="word">An optional word to use for the last object, e.g. 'and'</param>
        /// <returns></returns>
        public static string ToSeparatedString<T>(this IEnumerable<T> list, Func<T, string> displayFunction = null,
            string separator = ", ", string word = null)
        {
            // If no function is specified, just call the ToString method on each object
            if (displayFunction == null)
                displayFunction = d => d == null ? "" : d.ToString();

            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (var l in list)
            {
                count++;

                // Append the object
                builder.Append(displayFunction(l));

                // If this is the last object, there is more than one object AND the word parameter is specified, 
                // add the word. Otherwise, add the separator if this is not the last object
                if (word != null && count > 0 && count == list.Count() - 1)
                    builder.Append(" " + word + " ");
                else if (count != list.Count())
                    builder.Append(separator);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Formats a collection of objects as tab-separated text
        /// </summary>
        /// <typeparam name="T">Type of the objects</typeparam>
        /// <param name="data">Collection to save</param>
        public static string ToTabSeparated<T>(this IEnumerable<T> data)
        {
            var props = typeof(T).GetProperties();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(props.ToSeparatedString(p => p.Name, "\t"));
            foreach (var d in data)
                builder.AppendLine(props.ToSeparatedString(p =>
                {
                    var val = p.GetValue(d, null);
                    return val == null
                        ? ""
                        : val.ToString()
                            .Replace("\n", " ")
                            .Replace("\r", "")
                            .Replace("\t", "   ");
                }, "\t"));
            return builder.ToString();
        }
    }
}

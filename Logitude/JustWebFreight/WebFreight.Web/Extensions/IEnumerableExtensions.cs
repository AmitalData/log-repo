using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static IEnumerable<List<T>> Partition<T>(this List<T> source, int partitionSize)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)partitionSize); i++)
                yield return new List<T>(source.Skip(partitionSize * i).Take(partitionSize));
        }

        public static IEnumerable<List<T>> Partition<T>(this IEnumerable<T> items, int totalPartitions)
        {
            return Partition(items.ToList(), totalPartitions);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
        {
            int i = 0;
            foreach (T item in sequence)
            {
                action(i, item);
                i++;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
            {
                action(item);
            }
        }
    }
}
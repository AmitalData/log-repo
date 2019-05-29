using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure//.Helpers
{
    public static class ListsExt
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source
     , int batchSize)
        {
                        List<T> buffer = new List<T>();

            foreach (T item in source)
            {
                buffer.Add(item);

                if (buffer.Count >= batchSize)
                {
                    yield return buffer;
                    buffer = new List<T>();
                }
            }
            if (buffer.Count >= 0)
            {
                yield return buffer;
            }
        }

        public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}

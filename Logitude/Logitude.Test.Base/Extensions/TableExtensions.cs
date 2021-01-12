using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Test.Base.Extensions
{
    public static class TableExtensions
    {
        public static T CreateComplexInstance<T>(this Table table)
        {
            T result = table.CreateInstance<T>();

            IEnumerable<string> propNames = table.Rows.OfType<TableRow>().Where(x => x[0].Contains(".")).Select(x => Regex.Replace(x[0], @"^(.+?)\..+$", "$1"));

            foreach (string propName in propNames)
            {
                PropertyInfo prop = typeof(T).GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (prop != null)
                {
                    Table subTable = new Table("field", "value");
                    Regex regex = new Regex(string.Format(@"^{0}\.([^\.]*).*$", propName), RegexOptions.IgnoreCase);
                    List<TableRow> subheadings = table.Rows.OfType<TableRow>().Where(x => regex.IsMatch(x[0])).ToList();

                    List<string[]> subheadingsFormatted = subheadings
                        .Select(x => new[] { string.Join(".", x[0].Split('.').Skip(1)), x[1] })
                        .ToList();

                    subheadingsFormatted.ForEach(x => subTable.AddRow(x));

                    MethodInfo createInstance = typeof(TableExtensions).GetMethod(
                            nameof(CreateComplexInstance),
                            BindingFlags.Public | BindingFlags.Static,
                            null,
                            CallingConventions.Any,
                            new Type[] { typeof(Table) },
                            null);
                    createInstance = createInstance.MakeGenericMethod(prop.PropertyType);
                    object propValue = createInstance.Invoke(null, new object[] { subTable });

                    prop.SetValue(result, propValue);
                }
            }

            return result;
        }

        public static IEnumerable<T> CreateComplexSet<T>(this Table table)
        {
            IEnumerable<T> items = table.CreateSet<T>();

            for (int i = 0; i < table.RowCount; i++)
            {
                TableRow tableRow = table.Rows[i];

                T result = items.ElementAt(i);

                IEnumerable<string> propNames = tableRow
                    .Where(x => x.Key.Contains("."))
                    .Select(x => Regex.Replace(x.Key, @"^(.+?)\..+$", "$1"));

                foreach (string propName in propNames)
                {
                    PropertyInfo prop = typeof(T).GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (prop != null)
                    {
                        Table subTable = new Table("field", "value");
                        Regex regex = new Regex(string.Format(@"^{0}\.([^\.]*)$", propName), RegexOptions.IgnoreCase);

                        tableRow.Where(x => regex.IsMatch(x.Key))
                              .Select(x => new[] { regex.Replace(x.Key, "$1"), x.Value })
                              .ToList()
                              .ForEach(x => subTable.AddRow(x));

                        MethodInfo createInstance = typeof(TableHelperExtensionMethods).GetMethod(
                                "CreateInstance",
                                BindingFlags.Public | BindingFlags.Static,
                                null,
                                CallingConventions.Any,
                                new Type[] { typeof(Table) },
                                null);
                        createInstance = createInstance.MakeGenericMethod(prop.PropertyType);
                        object propValue = createInstance.Invoke(null, new object[] { subTable });

                        prop.SetValue(result, propValue);
                    }
                }
            }

            return items;
        }
    }
}
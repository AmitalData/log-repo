using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class SearchFieldsFinder
    {
        public static string Find(string searchFields, string searchText)
        {
            string result = null;
            List<string> matchedFields = new List<string>();
            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(searchFields))
            {
                if (searchFields.Contains(searchText))
                {
                    string[] fieldsArr = searchFields.Split(',');
                    foreach (var f in fieldsArr)
                    {
                        if (f.Contains(searchText) && !matchedFields.Any(s => s == f))
                        {
                            matchedFields.Add(f);
                        }
                    }

                }

                result = string.Join(" ", matchedFields);//~

            }

            return result;
        }
        public static string HybridFind(string searchFields, string searchText)
        {
            string result = null;
            var SplittedFields = searchFields.Split('*');
            string mysearchFields = "";
            if (SplittedFields.Length > 0)
            {
                mysearchFields = SplittedFields[0];
            }
            else
            {
                mysearchFields = searchFields;
            }

            List<string> matchedFields = new List<string>();
            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(mysearchFields))
            {
                if (mysearchFields.ToLower().Contains(searchText.ToLower()))
                {
                    string[] fieldsArr = mysearchFields.Split(',');
                    foreach (string f in fieldsArr)
                    {
                        if (f.ToLower().Contains(searchText.ToLower()) && !matchedFields.Any(s => s == f))
                        {
                            var x = f.ToLower().IndexOf(searchText.ToLower());
                            var y = searchText.Length;
                            matchedFields.Add(f.Remove(x, y).Insert(x, "|~S~|" + searchText + "|~E~|"));//Replace(searchText, "|~S~|" + searchText + "|~E~|"));
                        }
                    }

                }
                //else
                //{
                //    if (searchText.StartsWith("0"))
                //    {
                //        var temp = searchText.Replace("0",""); 
                //    }
                //}

                result = string.Join(" ", matchedFields);//~

            }

            return result;
        }
    }
}

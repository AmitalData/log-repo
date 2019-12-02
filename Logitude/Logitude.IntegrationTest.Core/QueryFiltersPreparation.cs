using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core
{
    public class QueryFiltersPreparation
    {

        public static string Geturlparameters( string SearchKey=null, ApiQueryFilters filters = null)
        {
            string urlparameters = "/getbyfilters?";
            var properties= filters!=null?GetProperties(filters):null;
            ApiQueryFilters Generatedfilters = filters == null ? new ApiQueryFilters():null;
            if (filters != null)
            {
                filters.PageSize = 23;
            }
            else if(SearchKey != null)
            {
                Generatedfilters.Filter1Name = "SearchFields";
                Generatedfilters.Filter1Operator = "Contains";
                Generatedfilters.PageSize = 23;
                Generatedfilters.Filter1Value = SearchKey;
                properties = GetProperties(Generatedfilters);
            }
            else
            {
                return null;
            }

            foreach (var p in properties)
            {
                var propName = p.Name;
                var Selectedfilters = filters != null ? filters : Generatedfilters;
                var propValue = p.GetValue(Selectedfilters, null);
                var ignoreFilter = ((propName.IndexOf("Operator") > 0 && propValue as string == "Equals") || propValue == null);
                if (urlparameters[urlparameters.Length - 1] != '?' && !ignoreFilter)
                {
                    urlparameters += "&";
                }
                if (!ignoreFilter)
                {
                    urlparameters = urlparameters + (propName + '=' + (propValue));
                }
            }
            return urlparameters;
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}

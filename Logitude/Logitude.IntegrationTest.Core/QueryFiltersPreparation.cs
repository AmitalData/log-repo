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

        public static string GetUrlParameters( string searchKey=null, ApiQueryFilters filters = null)
        {
            if (filters == null)
            {
                filters = InitializeAPIQueryFilters(searchKey);
            }
           
            string urlparameters = CreateParametersUrl(filters);
            return urlparameters;
        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }

        private static ApiQueryFilters InitializeAPIQueryFilters(string searchKey)
        {
            ApiQueryFilters filters = new ApiQueryFilters();
            filters.Filter1Name = "SearchFields";
            filters.Filter1Operator = "Contains";
            filters.PageSize = 23;
            filters.Filter1Value = searchKey;
            return filters;
        }
        public static ApiQueryFilters QueryfilterByCode(string searchKey)
        {
            ApiQueryFilters filters = new ApiQueryFilters();
            filters.Filter1Name = "Code";
            filters.Filter1Operator = "Equals";
            filters.PageSize = 23;
            filters.Filter1Value = searchKey;
            return filters;
        }

        private static string CreateParametersUrl(ApiQueryFilters filters)
        {
            PropertyInfo[] properties = GetProperties(filters);
            string urlparameters = "/getbyfilters?";
            foreach (var p in properties)
            {
                var propName = p.Name;
                var propValue = p.GetValue(filters, null);
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
    }
}

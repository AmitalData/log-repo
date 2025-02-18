using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class QueryOperationsFilterValueGetter
    {
        private QueryOperations queryOperations;
        public QueryOperationsFilterValueGetter(QueryOperations queryOperations)
        {
            this.queryOperations = queryOperations;
        }

        public T GetFilterValue<T>(string filterFieldName)
        {
            QueryFilterItem filterItem = GetFilterItemByFieldName(filterFieldName);

            if (filterItem != null && filterItem.FieldValue != null)
            {
                if (filterItem.FieldDataType == "decimal")
                {
                    decimal value = Convert.ToDecimal(filterItem.FieldValue);
                    object x = value;

                    return (T)x;
                } else if (filterItem.FieldDataType == "int")
                {
                    int value = Convert.ToInt32(filterItem.FieldValue);
                    object x = value;

                    return (T)x;
                }
                else if (filterItem.FieldDataType == "double")
                {
                    double value = Convert.ToDouble(filterItem.FieldValue);
                    object x = value;

                    return (T)x;
                }
                else
                {
                    return (T)filterItem.FieldValue;
                }
            }

            return default(T);
        }
        public T GetFilterValue2<T>(string filterFieldName)
        {
            QueryFilterItem filterItem = GetFilterItemByFieldName(filterFieldName);

            if (filterItem != null && filterItem.FieldValue != null)
            {
                if (filterItem.FieldDataType == "decimal")
                {
                    decimal value = Convert.ToDecimal(filterItem.FieldValue2);
                    object x = value;

                    return (T)x;
                }
                else if (filterItem.FieldDataType == "int")
                {
                    int value = Convert.ToInt32(filterItem.FieldValue2);
                    object x = value;

                    return (T)x;
                }
                else if (filterItem.FieldDataType == "double")
                {
                    double value = Convert.ToDouble(filterItem.FieldValue2);
                    object x = value;

                    return (T)x;
                }
                else
                {
                    return (T)filterItem.FieldValue2;
                }
            }

            return default(T);
        }
         public string GetOperatorByFieldName(string FieldName)
        {
            return queryOperations.QueryFilterItems.Where(d => d.FieldName == FieldName).Select(x=>x.Operator).FirstOrDefault();
        }
		
 		private QueryFilterItem GetFilterItemByFieldName(string FieldName)
        {
            return queryOperations.QueryFilterItems.Where(d => d.FieldName == FieldName).FirstOrDefault();
        }
    }
}
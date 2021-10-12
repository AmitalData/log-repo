using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.ExcelReport
{
    public class ExcelDataProviderFieldsBuilder
    {
        public List<DataProviderField> Build(string code)
        {
            return BuildDataProviderFields(GetDataProvider(code).GetProperties(), GetDataProvider(code).Name);
        }


        private List<DataProviderField> BuildDataProviderFields(PropertyInfo[] propertyInfos, string parentClassName)
        {
            var dataProviderFields = new List<DataProviderField>();
            foreach (PropertyInfo property in propertyInfos)
            {
                dataProviderFields.Add(new DataProviderField
                {
                    Name = property.Name,
                    Text = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property.Name,
                    Type = GetPropertyName(property),
                    Expression = "{" + parentClassName + "." + property.Name + "}",
                    Fields = GetPropertyFields(property, parentClassName + "." + property.Name),
                });
            }

            return dataProviderFields;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            if (IsClassProperty(property))
            {
                return "Class";
            }

            if (IsListProperty(property))
            {
                return "List";
            }
            return property.PropertyType.Name;
        }

        private List<DataProviderField> GetPropertyFields(PropertyInfo property, string parentClassName)
        {
            if (IsClassProperty(property))
            {
                return BuildDataProviderFields(property.PropertyType.GetProperties(), parentClassName);
            }

            if (IsListProperty(property))
            {
                return BuildDataProviderFields(property.PropertyType.GetGenericArguments()[0].GetProperties(), parentClassName);
            }

            return null;
        }

        private bool IsClassProperty(PropertyInfo property)
        {
            if (property.PropertyType.IsClass && !property.PropertyType.FullName.StartsWith("System.") && !property.PropertyType.FullName.StartsWith("Microsoft."))
                return true;
            return false;
        }
        private bool IsListProperty(PropertyInfo property)
        {
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                return true;
            return false;
        }

        private Type GetDataProvider(string reportCode)
        {
            switch (reportCode)
            {

                case "AGER":
                    {
                        return typeof(AccountingAgingDataProvider);
                    }

                case "LTRP":
                    {
                        return typeof(LedgerTransactionsDataProvider);
                    }

                case "RALS":
                    {
                        return typeof(AirlineStatisticsDataProvider);
                    }

                case "RSLS":
                    {
                        return typeof(ShippingLineStatisticsDataProvider);
                    }

                default:
                    return null;
            }
        }
    }
}
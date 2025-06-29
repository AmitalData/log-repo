using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers.DataProviderHelpers;

namespace WebFreight.Web.Helpers.ExcelReport
{
    public class ExcelDataProviderFieldsBuilder
    {
        public List<DataProviderField> Build(string reportCode)
        {
            return BuildDataProviderFields(GetDataProviderType(reportCode).GetProperties(), GetDataProviderType(reportCode).Name, 1);
        }


        private List<DataProviderField> BuildDataProviderFields(PropertyInfo[] propertyInfos, string parentClassName, int maxSubLevels)
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
                    Fields = GetPropertyFields(property, parentClassName + "." + property.Name, maxSubLevels),
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

        private List<DataProviderField> GetPropertyFields(PropertyInfo property, string parentClassName, int maxSubLevels)
        {
            if (maxSubLevels == 0) return null;
            if (IsClassProperty(property))
            {
                return BuildDataProviderFields(property.PropertyType.GetProperties(), parentClassName, maxSubLevels - 1);
            }

            if (IsListProperty(property))
            {
                return BuildDataProviderFields(property.PropertyType.GetGenericArguments()[0].GetProperties(), parentClassName, maxSubLevels - 1);
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

        public Type GetDataProviderType(string reportCode)
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

                case "TRBR":
                    {
                        return typeof(RevenueExpenseDataProvider);
                    }

                case "RALS":
                    {
                        return typeof(AirlineStatisticsDataProvider);
                    }
                case "DSCA":
                case "AREX":
                    {
                        return typeof(ArchivoExportadoDataProvider);
                    }
                case "SRQR":
                    {
                        return typeof(SpotRateQuoteReportDataProvider);
                    }
                case "CSSR":
                    {
                        return typeof(CustomerStatusDataProvider);
                    }
                case "SHID":
                    {
                        return typeof(ShipmentDetailsDataProvider);
                    }
                case "RSID":
                    {
                        return typeof(StatementByInvoiceDateDataProvider);
                    }
                case "OSBC":
                    {
                        return typeof(OpenShipmentsByCustomerDataProvider);
                    }
                case "RSTA":
                    {
                        return typeof(StatementDataProvider);
                    }
                case "ATRE":
                    {
                        return typeof(AutomationTestReportDataProvider);
                    }
                case "NAGR":
                    {
                        return typeof(NewAccountingAgingDataProvider);
                    }
                case "NTRP":
                    {
                        return typeof(NewLedgerTransactionDataProvider);
                    }
                default:
                    return null;
            }
        }
    }
}
using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.Helpers
{
    public class DWQueryBuilderHelper
    {
        private string WhereStmt = " where ";

        private int Tenant { get; set; }

        public DWQueryBuilderHelper(int tenant)
        {
            WhereStmt = " where ";
            this.Tenant = tenant;
        }

        private void GetWhereJoined(List<DWObjectFieldsDetails> FiltersList, List<DWObjectFieldsDetails> InnerTables)
        {
            //var FiltersWithValues = FiltersList.Where(a => a.TextValue != null && !string.IsNullOrEmpty(a.TextValue.ToString()));
            foreach (var Myfilter in FiltersList)
            {
                if (Myfilter.FilterItems.Count > 0)
                {
                    GetWhereJoined(Myfilter.FilterItems, InnerTables);
                }
                else
                {
                    //if (Myfilter.ParentDimTabelName != null)// && InnerTables.Where(a => a.ParentDimTabelName == Myfilter.ParentDimTabelName).Count() == 0
                    //{
                    //    InnerTables.Add(Myfilter);
                    //}
                    /*the following Code Should be written again :( ---- Rabaia*/
                    if (Myfilter.TextValue != null && !string.IsNullOrEmpty(Myfilter.TextValue.ToString()))
                    {
                        if ((((Myfilter.ParentDataTypeCode == "Dimension" || Myfilter.ParentDataTypeCode.ToLower() == "lookup" || Myfilter.ParentDataTypeCode.ToLower() == "date") && string.IsNullOrEmpty(Myfilter.DimensionTableDisplayName)) || !string.IsNullOrEmpty(Myfilter.DimensionTableDisplayName)) && ((!string.IsNullOrEmpty(Myfilter.DimensionTableDisplayName) && InnerTables.Where(a => a.DimensionTableDisplayName == Myfilter.DimensionTableDisplayName).Count() == 0) || (string.IsNullOrEmpty(Myfilter.DimensionTableDisplayName) && InnerTables.Where(a => a.DimensionTableDisplayName == Myfilter.Name).Count() == 0)))// && InnerTables.Where(a => a.ParentDimTabelName == field.ParentDimTabelName).Count() == 0
                        {
                            if (!InnerTables.Any(a => a.DisplayName == Myfilter.DimensionTableDisplayName))
                            {
                                InnerTables.Add(Myfilter);
                            }

                        }
                    }

                }
            }

        }

        private SqlCommandDefinition GetWhereStmtForFiltersList(List<DWObjectFieldsDetails> FiltersList, string AndOr, SqlCommandDefinition sqlCommandDef)
        {
            SqlCommandDefinition sqlCommandDefinition = sqlCommandDef;
            foreach (var Myfilter in FiltersList)
            {
                var isHaveMultiSelect = false;
                if (Myfilter.FilterItems.Count > 0)
                {
                    if (this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true)
                    {
                        WhereStmt = WhereStmt;
                        if (WhereStmt != " where ")
                        {
                            WhereStmt = WhereStmt + " " + AndOr + " ( ";
                        }

                    }
                    sqlCommandDefinition = GetWhereStmtForFiltersList(Myfilter.FilterItems, !string.IsNullOrEmpty(Myfilter.AndOr) ? Myfilter.AndOr : "And", sqlCommandDefinition);
                    if (WhereStmt == " where ")
                    {
                        WhereStmt = "";
                    }
                    else if (WhereStmt.Length >= 4 && (WhereStmt.Substring(WhereStmt.Length - 4).Contains("And") || WhereStmt.Substring(WhereStmt.Length - 4).Contains("Or")))
                    {
                        WhereStmt = WhereStmt.Substring(0, WhereStmt.Length - 4);
                    }
                    if (WhereStmt != "" && GetIfFiltersHaveValues(Myfilter.FilterItems) == true)
                    {
                        WhereStmt = WhereStmt + " ) ";

                        WhereStmt = WhereStmt.Replace("And  (  )", "");
                        WhereStmt = WhereStmt.Replace("Or  (  )", "");
                    }

                }
                else
                {
                    //Myfilter.FilterItems.filter(a => a.TextValue != null).forEach((filter) => {
                    if (Myfilter.TextValue != null && !string.IsNullOrEmpty(Myfilter.TextValue.ToString()))
                    {


                        var filter = Myfilter;
                        var OperationSimpol = "";
                        if (filter.Operation == null)
                        {
                            filter.Operation = new ObjectFieldOperator();
                            filter.Operation.Code = filter.OperationCode;
                            filter.Operation.Name = filter.OperationName;
                        }


                        string parameterValue = Myfilter.TextValue.ToString();
                        string parameterName = "@ValueParameter" + (sqlCommandDefinition.Parameters.Count() + 1).ToString();

                        if (string.IsNullOrEmpty(filter.DimensionTableDisplayName) && (filter.DataTypeCode == "Dimension" || filter.DataTypeCode.ToLower() == "lookup" || Myfilter.ParentDataTypeCode.ToLower() == "date"))
                        {
                            filter.DimensionTableDisplayName = filter.Name;
                        }
                        var PDim = "[" + filter.ParentDimTabelName + "]";
                        var OTBL = "[" + filter.DWObjectTableCode + "]";
                        if (!string.IsNullOrEmpty(filter.DimensionTableDisplayName))
                        {
                            PDim = "[" + filter.ParentDimTabelName + filter.DimensionTableDisplayName + "]";
                            OTBL = "[" + filter.DWObjectTableCode + filter.DimensionTableDisplayName + "]";
                        }
                        if (filter.DataTypeCode != "Date" && filter.DataTypeCode != "DateTime")
                        {
                            if (filter.Operation.Code == "Equals")
                            {
                                //if (filter.DataTypeCode == "Integer" || filter.DataTypeCode == "Double" || filter.DataTypeCode == "Decimal")
                                //{
                                //    OperationSimpol = " =  " + parameterName;
                                //}
                                //else
                                if (filter.DataTypeCode == "Boolean")
                                {
                                    parameterValue = filter.TextValue.ToString().ToLower() == "true" ? "1" : "0";
                                    OperationSimpol = " IN (" + parameterName + ")";

                                }
                                else
                                {
                                    SqlCommandDefinition sqlCommandDefinitionMultiValue = GetMultiValueFilterAsSqlCommandDefinition(filter.TextValue.ToString(), " IN ( ", sqlCommandDefinition.Parameters.Count());
                                    sqlCommandDefinition.Parameters = sqlCommandDefinition.Parameters.Concat(sqlCommandDefinitionMultiValue.Parameters).ToList();
                                    OperationSimpol = sqlCommandDefinitionMultiValue.SQLString;


                                    isHaveMultiSelect = true;
                                }
                            }
                            else if (filter.Operation.Code == "NotEqual")
                            {
                                //if (filter.DataTypeCode == "Integer" || filter.DataTypeCode == "Double" || filter.DataTypeCode == "Decimal")
                                //{
                                //    OperationSimpol = " <>  " + parameterName;
                                //}
                                //else
                                if (filter.DataTypeCode == "Boolean")
                                {
                                    parameterValue = filter.TextValue.ToString().ToLower() == "true" ? "1" : "0";
                                    OperationSimpol = " not IN (" + parameterName + ")";

                                }
                                else
                                {

                                    SqlCommandDefinition sqlCommandDefinitionMultiValue = GetMultiValueFilterAsSqlCommandDefinition(filter.TextValue.ToString(), " not IN ( ", sqlCommandDefinition.Parameters.Count());
                                    sqlCommandDefinition.Parameters = sqlCommandDefinition.Parameters.Concat(sqlCommandDefinitionMultiValue.Parameters).ToList();
                                    OperationSimpol = sqlCommandDefinitionMultiValue.SQLString;
                                    isHaveMultiSelect = true;
                                }
                            }
                            else if (filter.Operation.Code == "StartsWith")
                            {
                                parameterValue += "%";
                                OperationSimpol = " like  " + parameterName;
                            }
                            else if (filter.Operation.Code == "IsNull")
                            {
                                OperationSimpol = " is null ";
                            }
                            else if (filter.Operation.Code == "IsNotNull")
                            {
                                OperationSimpol = " is not null ";
                            }
                            else if (filter.Operation.Code == "GreaterThanOrEqual")
                            {
                                OperationSimpol = " >=  " + parameterName;
                            }
                            else if (filter.Operation.Code == "LargerThan")
                            {
                                OperationSimpol = " >  " + parameterName;
                            }
                            else if (filter.Operation.Code == "LessThan")
                            {
                                OperationSimpol = " <  " + parameterName;
                            }
                            else if (filter.Operation.Code == "LessThanOrEqual")
                            {
                                OperationSimpol = " <=  " + parameterName;
                            }


                            if (filter.Operation.Code == "IsNull" || filter.Operation.Code == "IsNotNull")
                            {
                                if (WhereStmt.Length >= 4 && (WhereStmt.Substring(WhereStmt.Length - 4).Contains("And") || WhereStmt.Substring(WhereStmt.Length - 4).Contains("Or")))
                                {
                                    WhereStmt = WhereStmt.Substring(0, WhereStmt.Length - 4);
                                }

                                if (WhereStmt.Replace("(", "").Replace(")", "").Replace(" ", "") == "where") WhereStmt += "( ";
                                else if (WhereStmt.EndsWith("( ")) WhereStmt += (" ");
                                else WhereStmt += (" " + AndOr);
                            }


                            if (filter.Operation.Code == "IsNull")
                            {
                                WhereStmt += "(" + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " is null or " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " = '' " + " ) ";

                            }
                            else if (filter.Operation.Code == "IsNotNull")
                            {
                                WhereStmt += "(" + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " is not null and " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " <> '' " + " ) ";
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(parameterValue)) parameterValue = filter.TextValue.ToString();

                                string fieldName = (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code;

                                if (filter.IsCustom && filter.Operation.Code == "StartsWith")
                                {
                                    fieldName = "CONVERT(sysname," + fieldName + ")";
                                }
                                if (!isHaveMultiSelect)
                                {
                                    sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = parameterName, Value = parameterValue, DataType = filter.DataTypeCode, Operation = filter.Operation.Code });
                                }
                                if (WhereStmt.Length >= 4 && (WhereStmt.Substring(WhereStmt.Length - 4).Contains("And") || WhereStmt.Substring(WhereStmt.Length - 4).Contains("Or")))
                                {
                                    WhereStmt = WhereStmt.Substring(0, WhereStmt.Length - 4);
                                }
                                if (WhereStmt.Replace("(", "").Replace(")", "").Replace(" ", "") != "where")
                                {
                                    if (WhereStmt.EndsWith("( "))
                                    {

                                        //WhereStmt = WhereStmt.TrimEnd(' ').TrimEnd('('); 
                                        WhereStmt += " " + fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";
                                    }
                                    else
                                    {
                                        WhereStmt += " " + AndOr + " " + fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";
                                    }

                                }
                                else
                                {
                                    WhereStmt += "( " + fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";
                                }
                                //WhereStmt += AndOr + " " + fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";


                            }
                        }
                        else
                        {

                            DataWarehouseHelper dataWarehouseHelper = new DataWarehouseHelper();
                            string dataWarehouseDateFieldSqlString = dataWarehouseHelper.ResolveWarehoueDateField(((!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code), filter.OperationCode, filter.TextValue.ToString(), filter.DataTypeCode, Tenant);
                            SqlCommandDefinition sqlCommandDefinitionDateFilter = GetDateFieldValueFilterAsSqlCommandDefinition(dataWarehouseDateFieldSqlString, sqlCommandDefinition.Parameters.Count());
                            sqlCommandDefinition.Parameters = sqlCommandDefinition.Parameters.Concat(sqlCommandDefinitionDateFilter.Parameters).ToList();
                            if (WhereStmt.Length >= 4 && (WhereStmt.Substring(WhereStmt.Length - 4).Contains("And") || WhereStmt.Substring(WhereStmt.Length - 4).Contains("Or")))
                            {
                                WhereStmt = WhereStmt.Substring(0, WhereStmt.Length - 4);
                            }
                            if (WhereStmt.Replace("(", "").Replace(")", "").Replace(" ", "") != "where")
                            {
                                if (WhereStmt.EndsWith("( "))
                                {
                                    //WhereStmt = WhereStmt.TrimEnd(' ').TrimEnd('(');
                                    WhereStmt += " " + sqlCommandDefinitionDateFilter.SQLString;// + " " + AndOr + " ";

                                }
                                else
                                {
                                    WhereStmt += " " + AndOr + " " + sqlCommandDefinitionDateFilter.SQLString;// + " " + AndOr + " ";
                                }
                            }
                            else
                            {
                                WhereStmt += "( " + sqlCommandDefinitionDateFilter.SQLString;// + " " + AndOr + " ";
                            }


                        }



                    }


                }
            }

            return sqlCommandDefinition;
        }

        private SqlCommandDefinition AppendSqlCommandParameters(SqlCommandDefinition sqlCommandDefinition1, SqlCommandDefinition sqlCommandDefinition2)
        {
            SqlCommandDefinition sqlCommandDefinition = sqlCommandDefinition1;

            foreach (SqlParameterDetails item in sqlCommandDefinition2.Parameters)
            {
                sqlCommandDefinition.Parameters.Add(item);
            }
            return sqlCommandDefinition;
        }


        private SqlCommandDefinition GetDateFieldValueFilterAsSqlCommandDefinition(string sqlString, int parametersCount)
        {
            SqlCommandDefinition sqlCommandDefinition = new SqlCommandDefinition() { Parameters = new List<SqlParameterDetails>() };
            string result = sqlString;
            int sqlParametersCount = parametersCount;
            while (result.Contains("<DataFieldValue>") && result.Contains("</DataFieldValue>"))
            {
                var dataFieldValue = GetValueBetweenTwoString(result, "<DataFieldValue>", "</DataFieldValue>");
                if (!string.IsNullOrEmpty(dataFieldValue))
                {
                    sqlParametersCount += 1;
                    string parameterName = "@ValueParameter" + (sqlParametersCount).ToString();
                    result = result.Replace("<DataFieldValue>" + dataFieldValue + "</DataFieldValue>", parameterName);
                    sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = parameterName, Value = dataFieldValue, DataType = "Date" });
                }
            }
            sqlCommandDefinition.SQLString = result;

            return sqlCommandDefinition;
        }
        public string GetValueBetweenTwoString(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                int End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else return "";
        }
        private SqlCommandDefinition GetMultiValueFilterAsSqlCommandDefinition(string textValue, string operationSimpol, int parametersCount)
        {
            int sqlParametersCount = parametersCount;
            SqlCommandDefinition sqlCommandDefinition = new SqlCommandDefinition() { Parameters = new List<SqlParameterDetails>() };
            var result = operationSimpol;
            if (!string.IsNullOrEmpty(textValue))
            {
                string[] stringSeparators = new string[] { ";;" };
                var values = textValue.Replace("'", "''").Split(stringSeparators, StringSplitOptions.None);
                if (values.Length > 0)
                {
                    foreach (var item in values)
                    {
                        sqlParametersCount += 1;
                        string parameterName = "@ValueParameter" + (sqlParametersCount).ToString();
                        if (!string.IsNullOrEmpty(item)) result += (parameterName + ",");
                        sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = parameterName, Value = item, DataType = "MultiValue" });
                    }
                    result += (")");
                    result = result.Replace(",)", ")");
                }
                else result += ")";
            }
            else result += " )";

            sqlCommandDefinition.SQLString = result;
            return sqlCommandDefinition;
        }

        private bool GetIfFiltersHaveValues(List<DWObjectFieldsDetails> FiltersList)
        {
            return FiltersList.Where(a => a.TextValue != null && !string.IsNullOrEmpty(a.TextValue.ToString())).Count() > 0;
        }

        public SqlCommandDefinition GetQuerySQL(DWQueryData DWQueryParam)
        {
            SqlCommandDefinition sqlCommandDefinition = new SqlCommandDefinition();
            sqlCommandDefinition.Parameters = new List<SqlParameterDetails>();
            DWObjectFieldQuery OFieldQuery = new DWObjectFieldQuery(Tenant);
            var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(DWQueryParam.Columns);
            var FilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(DWQueryParam.Filters);
            var Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(ColumnsXML);
            var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(FilterXML);

            SqlStatmentDetails sqlStatmentDetails = new SqlStatmentDetails();
            sqlStatmentDetails.Columns = Columns;
            sqlStatmentDetails.Filters = Filters;
            string FinalQuery = "";
            bool HasMultipleSelection = Columns.Where(a => a.IsMultipleSelection == true && a.MultiSelectedValueLists.Count() > 0).Count() > 0;
            if (HasMultipleSelection)
            {
                sqlStatmentDetails = BuildSqlStatmentDetails(sqlStatmentDetails, -1, true);
                SqlStatmentDetails innerSqlStatmentDetails = new SqlStatmentDetails();
                innerSqlStatmentDetails.Columns = Columns;
                innerSqlStatmentDetails.Filters = Filters;
                int columnIndex = 0;
                Columns.ForEach(c =>
                {
                    columnIndex += 1;
                    if (c.IsMultipleSelection)
                    {
                        SqlColumnStatmentDetails sqlColumnStatmentDetails = new SqlColumnStatmentDetails();
                        sqlColumnStatmentDetails.Index = columnIndex;
                        sqlColumnStatmentDetails.MultiSelectedCount = c.MultiSelectedValueLists.Count();

                        innerSqlStatmentDetails = BuildSqlStatmentDetails(innerSqlStatmentDetails, columnIndex);

                        sqlCommandDefinition = BuildSqlCommandDefinition(innerSqlStatmentDetails, DWQueryParam, sqlColumnStatmentDetails);
                        FinalQuery += string.IsNullOrEmpty(FinalQuery) ? sqlCommandDefinition.SQLString : " Union " + sqlCommandDefinition.SQLString;
                    }
                });
                sqlStatmentDetails.FinalGroupByStmt = sqlStatmentDetails.FinalGroupByStmt == " group by" ? "" : sqlStatmentDetails.FinalGroupByStmt;
                sqlCommandDefinition.SQLString = sqlStatmentDetails.FinalSelectStmt + " from (" + FinalQuery + ") as AllQuery " + sqlStatmentDetails.FinalGroupByStmt;
            }
            else
            {
                sqlStatmentDetails = BuildSqlStatmentDetails(sqlStatmentDetails);
                sqlCommandDefinition = BuildSqlCommandDefinition(sqlStatmentDetails, DWQueryParam);
            }

            SQLSecurityTenantValidation(DWQueryParam, sqlCommandDefinition);

            return sqlCommandDefinition;
        }

        private  void SQLSecurityTenantValidation(DWQueryData DWQueryParam, SqlCommandDefinition sqlCommandDefinition)
        {
            string factName = DWQueryParam.FactTableName.ToLower();
            string querySQL = sqlCommandDefinition.SQLString.ToLower().Replace(" ", "");
            string whereByParenttenant = ("where" + factName + ".[parenttenant]=");
            string whereBySourcettenant = ("where" + factName + ".[sourcetenant]=");
            if (!querySQL.Contains(whereByParenttenant) && !querySQL.Contains(whereBySourcettenant))
            {
                throw new Exception("You are not authorized to view the content.");
            }
        }

        private SqlStatmentDetails BuildSqlStatmentDetails(SqlStatmentDetails sqlStatmentDetails, int columnIndex = -1, bool isMainSelectStmt = false)
        { 
            var InnerTables = new List<DWObjectFieldsDetails>();
            var SelectStmt = new StringBuilder();
            SelectStmt.Append("Select ");
            var GroupByStmt = new StringBuilder();
            GroupByStmt.Append(" group by ");
            
            var FromTables = new List<string>();
            int fieldColumnIndex = 0;
            foreach (var field in sqlStatmentDetails.Columns)
            {
                fieldColumnIndex += 1;
                if (!field.DisplayName.Contains("["))
                {
                    field.DisplayName = "[" + field.DisplayName + "]";
                }

                if (field.ParentDataTypeCode == "DateParts")
                {
                    string displayDateName = GetDatePartsSqlColum(field);
                    if (!string.IsNullOrEmpty(field.DimensionTableDisplayName) && InnerTables.Where(a => a.DimensionTableDisplayName == field.DimensionTableDisplayName).Count() == 0)
                    {
                        InnerTables.Add(field);
                    }
                    SelectStmt.Append(displayDateName);
                    string groupFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;
                    GroupByStmt.Append(groupFrom + ",");

                }
                else
                {
                    if ((((field.ParentDataTypeCode == "Dimension" || field.ParentDataTypeCode.ToLower() == "lookup") && string.IsNullOrEmpty(field.DimensionTableDisplayName)) || !string.IsNullOrEmpty(field.DimensionTableDisplayName)) && ((!string.IsNullOrEmpty(field.DimensionTableDisplayName) && InnerTables.Where(a => a.DimensionTableDisplayName == field.DimensionTableDisplayName).Count() == 0) || (string.IsNullOrEmpty(field.DimensionTableDisplayName) && InnerTables.Where(a => a.DimensionTableDisplayName == field.Name).Count() == 0)))// && InnerTables.Where(a => a.ParentDimTabelName == field.ParentDimTabelName).Count() == 0
                    {
                        InnerTables.Add(field);
                    }
                    if ((field.ParentDataTypeCode == "Dimension" || field.ParentDataTypeCode.ToLower() == "lookup") && string.IsNullOrEmpty(field.DimensionTableDisplayName))
                    {
                        field.DimensionTableDisplayName = field.Name;
                    }
                    if (field.IsMeasurement)
                    {
                        if (!string.IsNullOrEmpty(field.DimensionTableDisplayName))
                        {
                            string selectFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;
                            string selectAggregationCode = (columnIndex != -1) && (columnIndex != fieldColumnIndex) ? "0" : field.AggregationTypeCode + "(" + selectFrom + ")";
                            SelectStmt.Append(selectAggregationCode + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                        }
                        else
                        {
                            string selectFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : field.DWObjectTableCode + "." + field.Code;
                            string selectAggregationCode = (columnIndex != -1) && (columnIndex != fieldColumnIndex) ? "0" : field.AggregationTypeCode + "(" + selectFrom + ")";
                            SelectStmt.Append(selectAggregationCode + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(field.DimensionTableDisplayName))
                        {
                            //(case WHEN Fact_Shipments.[Is Arrived] = 1 then 'Yes' WHEN  Fact_Shipments.[Is Arrived] = 0 then 'No' end)
                            if (field.DataTypeCode.ToLower() == "boolean")
                            {
                                string selectFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;
                                SelectStmt.Append("case WHEN " + selectFrom + "= " + (isMainSelectStmt ? "'Yes'":"1") + " Then 'Yes' WHEN " + selectFrom + "= " + (isMainSelectStmt ? "'No'" : "0") + "Then 'No' End" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                            }
                            else
                            {
                                string selectFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;
                                SelectStmt.Append(selectFrom + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                            }
                            string groupFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;
                            GroupByStmt.Append(groupFrom + ",");
                        }
                        else
                        {
                            if (field.DataTypeCode.ToLower() == "boolean")
                            {
                                string selectFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : field.DWObjectTableCode + "." + field.Code;
                                SelectStmt.Append("case WHEN " + selectFrom + "= " + (isMainSelectStmt ? "'Yes'" : "1") + "Then 'Yes' WHEN " + selectFrom + "= " + (isMainSelectStmt ? "'No'" : "0") + "Then 'No' End" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                            }
                            else
                            {
                                string selectFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : field.DWObjectTableCode + "." + field.Code;
                                SelectStmt.Append(selectFrom + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                            }
                            string groupFrom = isMainSelectStmt ? "AllQuery." + field.DisplayName : "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;
                            GroupByStmt.Append(groupFrom + ",");
                        }

                    }
                }

                if (FromTables.Where(a => a == field.DWObjectTableCode).Count() == 0)
                {
                    FromTables.Add(field.DWObjectTableCode);
                }

            }

            sqlStatmentDetails.InnerTables = InnerTables;
            sqlStatmentDetails.FromTables = FromTables;
            sqlStatmentDetails.FinalSelectStmt = SelectStmt.ToString().Substring(0, SelectStmt.Length - 1);
            sqlStatmentDetails.FinalGroupByStmt = GroupByStmt.ToString().Substring(0, GroupByStmt.Length - 1);

            return sqlStatmentDetails;
        }


        private SqlCommandDefinition BuildSqlCommandDefinition(SqlStatmentDetails sqlStatmentDetails, DWQueryData DWQueryParam, SqlColumnStatmentDetails sqlColumnStatmentDetails = null)
        {
            SqlCommandDefinition sqlCommandDefinition = new SqlCommandDefinition();
            sqlCommandDefinition.Parameters = new List<SqlParameterDetails>();
            DWObjectFieldQuery OFieldQuery = new DWObjectFieldQuery(Tenant);
            var Filters = sqlStatmentDetails.Filters;
            bool HasMeasurement = sqlStatmentDetails.Columns.Where(a => a.IsMeasurement == true).Count() > 0;
            bool HasMultipleSelection = sqlStatmentDetails.Columns.Where(a => a.IsMultipleSelection && a.MultiSelectedValueLists.Count()>0).Count() > 0;
            string FinalSelectStmt = sqlStatmentDetails.FinalSelectStmt;
            string selectFieldTenantSql = string.Empty;
            string FinalGroupByStmt = sqlStatmentDetails.FinalGroupByStmt;
            string Fact = sqlStatmentDetails.FromTables.Find(a => a == "Fact");
            DWObjectTablePM dWObjectTablePM = null;
            if (string.IsNullOrEmpty(Fact))
            {
                Fact = DWQueryParam.FactTableName;
            }

            if (!string.IsNullOrEmpty(Fact))
            {
                DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(Tenant);
                dWObjectTablePM = dWObjectTableQuery.GetSinglePM(Fact , Tenant);
                 Fact = (dWObjectTablePM != null && !string.IsNullOrEmpty(dWObjectTablePM.ParentFactCode)) ? dWObjectTablePM.ParentFactCode : Fact;
            }


            var DWSettings = new DWHSettingRepository(Tenant);
            var isParentTenant = DWSettings.IsParentTenant(Tenant);

            selectFieldTenantSql = "," + (!isParentTenant ? (Fact + ".[Source Tenant]  ") : (Fact + ".[Parent Tenant]")) + "as Tenant";
            FinalSelectStmt += "@SelectFieldTenantSql from " + Fact;

            if (Filters != null)
            {
                var MyFilterList = new List<DWObjectFieldsDetails>();
                MyFilterList.Add(Filters);
                GetWhereJoined(MyFilterList, sqlStatmentDetails.InnerTables);
                sqlCommandDefinition = GetWhereStmtForFiltersList(MyFilterList, !string.IsNullOrEmpty(Filters.AndOr) ? Filters.AndOr : "And", sqlCommandDefinition);
            }

            sqlStatmentDetails.FromTables = sqlStatmentDetails.FromTables.Where(a => a != Fact).ToList();

            bool isDWQueryUsedAdditionalFact = false;
            DWObjectFieldAdditionalFactService dWObjectFieldAdditionalFactService = new DWObjectFieldAdditionalFactService(new DWObjectFieldAdditionalFactArgs() { FactTableCode = DWQueryParam.FactTableName, Tenant = Tenant, DontLoadDwObjectField = true });
            if (dWObjectFieldAdditionalFactService.IsHaveAddAdditionalFactFields)
            {
                dWObjectFieldAdditionalFactService.LoadDWObjectFieldsWithAdditionalFactFields();
                if (FinalSelectStmt.Contains(dWObjectFieldAdditionalFactService.DwObjectTable.AdditionalFactCode) || WhereStmt.Contains(dWObjectFieldAdditionalFactService.DwObjectTable.AdditionalFactCode)) isDWQueryUsedAdditionalFact = true;
            }


            string innerjoinSql = string.Empty;
            foreach (var mytbl in sqlStatmentDetails.InnerTables)
            {
                var Key = OFieldQuery.GetPrimaryKeyFieldForDWObjectTable(mytbl.ParentDimTabelName);
                var FactKey = mytbl.DimensionTableDisplayName;
                if ((mytbl.ParentDataTypeCode == "Dimension" || mytbl.ParentDataTypeCode.ToLower() == "lookup") && string.IsNullOrEmpty(mytbl.DimensionTableDisplayName))
                {
                    FactKey = mytbl.DisplayName;
                }
                else if (mytbl.HideTree)
                {
                    if (!mytbl.IsCustom)
                    {
                        FactKey = OFieldQuery.GetDWObjectFieldCodeByNameDimTable(mytbl.ParentDimTabelName, mytbl.DisplayName.Replace("[", "").Replace("]", ""));
                    }
                }
                if (FactKey != null && !FactKey.Contains("["))
                {
                    FactKey = "[" + FactKey + "]";
                }

                var factTable = dWObjectFieldAdditionalFactService.DWObjectFieldPMs.Where(d => d.Code == FactKey).Select(d => d.DWObjectTableCode).FirstOrDefault();
                if (string.IsNullOrEmpty(factTable)) factTable = Fact;

                if (factTable != Fact) isDWQueryUsedAdditionalFact = true;

                innerjoinSql += " inner join " + mytbl.ParentDimTabelName + " " + "[" + mytbl.ParentDimTabelName + mytbl.DimensionTableDisplayName + "]" + " on " + factTable + "." + (FactKey) + " = " + "[" + mytbl.ParentDimTabelName + mytbl.DimensionTableDisplayName + "]" + "." + Key.Code;
            }

            if (isDWQueryUsedAdditionalFact)
            {
                FinalSelectStmt += " inner join " + dWObjectFieldAdditionalFactService.DwObjectTable.AdditionalFactCode + " " + " on " + Fact + "." + (dWObjectFieldAdditionalFactService.DwObjectTable.AdditionalFactForeignKey) + " = " + dWObjectFieldAdditionalFactService.DwObjectTable.AdditionalFactCode + ".Id";
            }
            FinalSelectStmt += innerjoinSql;

            string pivotTableNickname = "";
            if (HasMultipleSelection)
            {
                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(Tenant);
                var multipleDiminsionSelection = dWObjectFieldQuery.GetDWObjectFieldByDimTable(dWObjectFieldAdditionalFactService.DwObjectTable.PivotFieldCode);
                pivotTableNickname = " [" + multipleDiminsionSelection.DimensionTableCode + multipleDiminsionSelection.Name + "]";
                if (sqlStatmentDetails.InnerTables.Where(innerTable => innerTable.DWObjectTableCode == dWObjectFieldAdditionalFactService.DwObjectTable.PivotFieldCode).Count() == 0)
                {
                    FinalSelectStmt += " inner join " + multipleDiminsionSelection.DimensionTableCode + pivotTableNickname + " on [" + multipleDiminsionSelection.DWObjectTableCode + "].[" + multipleDiminsionSelection.Name + "] = " + pivotTableNickname + ".[Id_Number]";
                }
            }
            var OrderByString = "" + Fact + ".Id_Number";

            //var HasAggregate = false;
            if (DWQueryParam.Columns.Where(a => a.IsMeasurement).Count() > 0)
            {
                OrderByString = "max(" + Fact + ".Id_Number)";
            }
            if (!string.IsNullOrEmpty(DWQueryParam.ColumnsSort))
            {
                OrderByString = DWQueryParam.ColumnsSort;
            }
            string PagingString = "";
            string offsetPagingString = "";

            if (LogitudeSettings.LogitudeURL != "http://localhost:9996" && DWQueryParam.PageSize != 0)
            {
                PagingString = " ORDER BY " + OrderByString;
                offsetPagingString = (" OFFSET " + DWQueryParam.PageIndex + " ROWS FETCH NEXT " + DWQueryParam.PageSize + " ROWS ONLY");
            }
            
            string FinalQuery = "";
            if (Filters != null)
            {
                FinalQuery = FinalSelectStmt + WhereStmt + ((HasMeasurement && FinalGroupByStmt != " group by") ? FinalGroupByStmt : "");
            }
            else
            {
                FinalQuery = FinalSelectStmt + (HasMeasurement && FinalGroupByStmt != " group by" ? FinalGroupByStmt : "");

            }

            string TenantWhere = ".[Parent Tenant] = ";
            var temp = DWSettings.GetSingleDWHSetting(Tenant);
            if (!isParentTenant)//temp != null &&  temp.Tenant != temp.ParentTenant)
            {
                TenantWhere = ".[Source Tenant] = ";
            }
            else
            {
                CheckBICentralDWHFeature();
            }




            string recordTypeCondation = "";
            List<string>shipmentLevelLists = GetShipmentLevelListsByRecordType(dWObjectTablePM.RecordType);
            if (shipmentLevelLists.Count() > 0)
            {

                recordTypeCondation = (" " + Fact + ".[DirectHouse] in (");
                foreach (string shipmentType in shipmentLevelLists)
                {
                    string parameterTenantName = "@ShipmentLevel" + shipmentType.ToString();
                    recordTypeCondation += parameterTenantName + ",";
                    sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = parameterTenantName, Value = shipmentType.ToString() ,DataType = "MultiValue" });
                }
                recordTypeCondation = recordTypeCondation.Remove(recordTypeCondation.Length - 1);
                recordTypeCondation += ") ";
            }


            if (DWQueryParam.UserEmail != "ahmadb@test.com")
            {
                if (FinalQuery.Contains("where"))
                {

                    string finarlCondition = ("where " + Fact + TenantWhere + "@Tenant" + " and");
                    if (!string.IsNullOrEmpty(recordTypeCondation)) finarlCondition += recordTypeCondation + " and";
                    FinalQuery = FinalQuery.Replace("where", finarlCondition);
                }
                else if (FinalQuery.Contains("group by"))
                {
                    string finarlCondition = ("where " + Fact + TenantWhere + "@Tenant");
                    if (!string.IsNullOrEmpty(recordTypeCondation)) finarlCondition += (" and " + recordTypeCondation);
                    finarlCondition += " group by";
                    FinalQuery = FinalQuery.Replace("group by", finarlCondition);

                }
                else
                {
                    FinalQuery = FinalQuery + " where " + Fact + TenantWhere + "@Tenant";
                    if (!string.IsNullOrEmpty(recordTypeCondation)) FinalQuery += (" and" + recordTypeCondation);

                }
            }


            if (HasMultipleSelection && (sqlColumnStatmentDetails != null && sqlColumnStatmentDetails.MultiSelectedCount > 0))
            {
                string replaceString = GetChargesTypeConditions(sqlStatmentDetails.Columns, pivotTableNickname, sqlColumnStatmentDetails.Index);
                FinalQuery = FinalQuery.Replace("@Tenant", replaceString);
            }

            if (sqlCommandDefinition.Parameters.Where(p => p.ParameterName == "@Tenant").Count() == 0)
            {
                sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = "@Tenant", Value = Tenant.ToString() });
            }

            if (sqlCommandDefinition.Parameters.Where(p => p.ParameterName == "@RecordType").Count() == 0)
            {
                sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = "@RecordType", Value = dWObjectTablePM.RecordType});
            }

            sqlCommandDefinition.SQLString = FinalQuery + PagingString + offsetPagingString;
            if (sqlCommandDefinition.SQLString.Contains("group by")) selectFieldTenantSql = "";
            sqlCommandDefinition.SQLString = sqlCommandDefinition.SQLString.Replace("@SelectFieldTenantSql", selectFieldTenantSql);

            return sqlCommandDefinition;
        }

        private List<string> GetShipmentLevelListsByRecordType(string recordType)
        {
            var result = new List<string>();
            if (!string.IsNullOrEmpty(recordType))
            {
                if (recordType == "Master")
                {
                    result.Add("Consol");
                    result.Add("Direct");
                }
                else if (recordType == "Shipment")
                {
                    result.Add("Direct");
                    result.Add("House");
                }
            }
            return result;
        }

        private string GetChargesTypeConditions(List<DWObjectFieldsDetails> columns,string pivotTableNickname, int columnIndex)
        {
            string codeString = pivotTableNickname + ".Code = ";
            string replaceString = "@Tenant and ( " + codeString;
            columns[columnIndex-1].MultiSelectedValueLists.ForEach(c => {
                replaceString += "'" + c.Value.Row + "' or " + codeString;
            });

            replaceString += " )";
            replaceString = replaceString.Replace(" or " + codeString + " )", " )");

            return replaceString;
        }

        private void CheckBICentralDWHFeature()
        {
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                {
                    SecurityUtility.CheckContactFeature("General", "BICentralDWH", Tenant);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Sorry! You have no permission to run reports");
            }
        }

        private static string GetDatePartsSqlColum(DWObjectFieldsDetails field)
        {
            string fieldCode = field.DWObjectTableCode + "." + field.Code;
            if (!string.IsNullOrEmpty(field.DimensionTableDisplayName))
                fieldCode = "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code;

            string datePartsSqlColum = field.DWObjectTableCode + "." + field.Code;
            if (field.DataTypeCode == "Time") datePartsSqlColum = "convert(varchar(5)," + fieldCode + ", 8)";
            else if (field.DataTypeCode == "Date") datePartsSqlColum = "convert(varchar(10)," + fieldCode + ", 120)";

            datePartsSqlColum += ((!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
            return datePartsSqlColum;
        }

        public DataTable GetDWQueryData(SqlCommandDefinition sqlCommandDefinition)
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                var currentDb = GlobalDbHelper.GetGlobalDBWithNoCache(0);
                string dbConnectionInfo = currentDb.SharedDWConnection;
                DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
                var dataTable = new DataTable();

                using (SqlConnection sourceConnection = new SqlConnection(connection.ConnectionString))
                {
                    sourceConnection.Open();

                    SqlCommand commandSourceData = new SqlCommand(sqlCommandDefinition.SQLString, sourceConnection);
                    commandSourceData.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                    foreach (SqlParameterDetails sqlParameter in sqlCommandDefinition.Parameters)
                    {
                        commandSourceData.Parameters.Add(GetNewInstanceFromSqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
                    }

                    SqlDataReader reader = commandSourceData.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                }
                scope.Complete();

                return dataTable;
            }

        }



        public SqlParameter GetNewInstanceFromSqlParameter(string parameterName, string parameterValue)
        {
            return new SqlParameter() { ParameterName = parameterName, Value = parameterValue };
        }

        public string GetSQLStringFromSqlCommandDefinition(SqlCommandDefinition sqlCommandDefinition)
        {
            string query = sqlCommandDefinition.SQLString;
            foreach (SqlParameterDetails sqlParameter in sqlCommandDefinition.Parameters)
            {
                string paramValue = sqlParameter.Value;
                if (sqlParameter.Operation == "StartsWith" || sqlParameter.DataType == "MultiValue" || sqlParameter.DataType == "Date") paramValue = "'" + paramValue + "'";
                query = query.Replace(sqlParameter.ParameterName, paramValue);
            }
            return query;
        }




    }

    public class SqlParameterDetails
    {

        public string ParameterName { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public string Operation { get; set; }
    }

    public class SqlCommandDefinition
    {
        public List<SqlParameterDetails> Parameters { get; set; }
        public string SQLString { get; set; }

    }

    public class SqlStatmentDetails
    {
        public List<DWObjectFieldsDetails> Columns { get; set; }
        public DWObjectFieldsDetails Filters { get; set; }
        public List<DWObjectFieldsDetails> InnerTables { get; set; }
        public List<string> FromTables { get; set; }
        public string FinalSelectStmt { get; set; }
        public string FinalGroupByStmt { get; set; }
    }

    public class SqlColumnStatmentDetails
    {
        public int Index { get; set; }
        public int MultiSelectedCount { get; set; }
    }

}
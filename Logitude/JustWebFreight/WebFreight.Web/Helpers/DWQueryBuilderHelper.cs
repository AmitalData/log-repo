using Logitude.BL.CommonDataModel.DataContracts;
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
                        WhereStmt = WhereStmt + " ( ";
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
                            if (filter.Operation.Code == "IsNull")
                            {

                                WhereStmt += "(" + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " is null or " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " = '' " + " ) " + AndOr + " ";
                            }
                            else if (filter.Operation.Code == "IsNotNull")
                            {
                                WhereStmt += "(" + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " is not null and " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code + " <> '' " + " ) " + AndOr + " ";
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
                                if (WhereStmt != " where  ( ")
                                {
                                    WhereStmt += " " + AndOr + " " + fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";
                                }
                                else
                                {
                                    WhereStmt += fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";
                                }
                                //WhereStmt += AndOr + " " + fieldName + OperationSimpol;// + " " +;//" = " + "'" + filter.TextValue + "' and ";


                            }
                        }
                        else
                        {

                            DataWarehouseHelper dataWarehouseHelper = new DataWarehouseHelper();
                            string dataWarehouseDateFieldSqlString = dataWarehouseHelper.ResolveWarehoueDateField(((!string.IsNullOrEmpty(filter.ParentDimTabelName) ? PDim : OTBL) + "." + filter.Code), filter.OperationCode, filter.TextValue.ToString(), Tenant);
                            SqlCommandDefinition sqlCommandDefinitionDateFilter = GetDateFieldValueFilterAsSqlCommandDefinition(dataWarehouseDateFieldSqlString, sqlCommandDefinition.Parameters.Count());
                            sqlCommandDefinition.Parameters = sqlCommandDefinition.Parameters.Concat(sqlCommandDefinitionDateFilter.Parameters).ToList();
                            if (WhereStmt.Length >= 4 && (WhereStmt.Substring(WhereStmt.Length - 4).Contains("And") || WhereStmt.Substring(WhereStmt.Length - 4).Contains("Or")))
                            {
                                WhereStmt = WhereStmt.Substring(0, WhereStmt.Length - 4);
                            }
                            if (WhereStmt != " where  ( ")
                            {
                                WhereStmt += " " + AndOr + " " + sqlCommandDefinitionDateFilter.SQLString;// + " " + AndOr + " ";
                            }
                            else
                            {
                                WhereStmt += sqlCommandDefinitionDateFilter.SQLString;// + " " + AndOr + " ";
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

            bool HasMeasurement = Columns.Where(a => a.IsMeasurement == true).Count() > 0;
            var InnerTables = new List<DWObjectFieldsDetails>();
            //this.SampleData = [];

            var SelectStmt = new StringBuilder();
            SelectStmt.Append("Select ");
            var GroupByStmt = new StringBuilder();
            GroupByStmt.Append(" group by ");
            //var Wheremt = " Where ";


            var FromTables = new List<string>();
            foreach (var field in Columns)
            {
                if (!field.DisplayName.Contains("["))
                {
                    field.DisplayName = "[" + field.DisplayName + "]";
                }
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
                        SelectStmt.Append(field.AggregationTypeCode + "(" + "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code + ")" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                    }
                    else
                    {
                        SelectStmt.Append(field.AggregationTypeCode + "(" + field.DWObjectTableCode + "." + field.Code + ")" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(field.DimensionTableDisplayName))
                    {
                        //(case WHEN Fact_Shipments.[Is Arrived] = 1 then 'Yes' WHEN  Fact_Shipments.[Is Arrived] = 0 then 'No' end)
                        if (field.DataTypeCode.ToLower() == "boolean")
                        {
                            SelectStmt.Append("case WHEN [" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code + "= 1 Then 'Yes' WHEN " + "[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code + "= 0 Then 'No' End" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                        }
                        else
                        {
                            SelectStmt.Append("[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                        }
                        GroupByStmt.Append("[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code + ",");
                    }
                    else
                    {
                        if (field.DataTypeCode.ToLower() == "boolean")
                        {
                            SelectStmt.Append("case WHEN " + field.DWObjectTableCode + "." + field.Code + "= 1 Then 'Yes' WHEN " + field.DWObjectTableCode + "." + field.Code + "= 0 Then 'No' End" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                        }
                        else
                        {
                            SelectStmt.Append(field.DWObjectTableCode + "." + field.Code + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                        }
                        GroupByStmt.Append("[" + field.DWObjectTableCode + field.DimensionTableDisplayName + "]." + field.Code + ",");
                    }

                }

                if (FromTables.Where(a => a == field.DWObjectTableCode).Count() == 0)
                {
                    FromTables.Add(field.DWObjectTableCode);
                }

            }


            string FinalSelectStmt = SelectStmt.ToString().Substring(0, SelectStmt.Length - 1);
            string FinalGroupByStmt = GroupByStmt.ToString().Substring(0, GroupByStmt.Length - 1);
            string Fact = FromTables.Find(a => a == "Fact");
            if (string.IsNullOrEmpty(Fact))
            {
                Fact = "Fact_Shipments";
            }
            FinalSelectStmt += " from " + Fact;


            if (Filters != null)
            {
                var MyFilterList = new List<DWObjectFieldsDetails>();
                MyFilterList.Add(Filters);
                GetWhereJoined(MyFilterList, InnerTables);
                sqlCommandDefinition = GetWhereStmtForFiltersList(MyFilterList, !string.IsNullOrEmpty(Filters.AndOr) ? Filters.AndOr : "And", sqlCommandDefinition);
            }

            //this.Notes = SelectStmt;
            FromTables = FromTables.Where(a => a != Fact).ToList();
            //var MeFactName = "";
            foreach (var mytbl in InnerTables)
            {
                //var Key = this.AllFieldsObsList.filter(a => a.DWObjectTableCode == mytbl.ParentDimTabelName && a.IsPrimaryKey == true)[0];
                var Key = OFieldQuery.GetPrimaryKeyFieldForDWObjectTable(mytbl.ParentDimTabelName);
                //MeFactName = OFieldQuery.GetFactTableCode(mytbl.ParentDimTabelName);
                var FactKey = mytbl.DimensionTableDisplayName;//.Split(' ')[0];

                if ((mytbl.ParentDataTypeCode == "Dimension" || mytbl.ParentDataTypeCode.ToLower() == "lookup") && string.IsNullOrEmpty(mytbl.DimensionTableDisplayName))
                {
                    FactKey = mytbl.DisplayName;// DisplayName.Split(' ')[0];//OFieldQuery.GetFactKeyFieldForDWDimTable(Fact, mytbl.ParentDimTabelName);

                    //if (!FactKey.Contains("]"))
                    //{
                    //    FactKey = FactKey + "]";
                    //}
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

                //this.AllFieldsDataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
                //(FactKey.DataTypeCode.ToLower() == "lookup" || FactKey.DataTypeCode.ToLower() == "dimension") ? FactKey.DisplayName : 
                FinalSelectStmt += " inner join " + mytbl.ParentDimTabelName + " " + "[" + mytbl.ParentDimTabelName + mytbl.DimensionTableDisplayName + "]" + " on " + Fact + "." + (FactKey) + " = " + "[" + mytbl.ParentDimTabelName + mytbl.DimensionTableDisplayName + "]" + "." + Key.Code;


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
            string PagingString = " ORDER BY " + OrderByString;
            string offsetPagingString = "";
            if (LogitudeSettings.LogitudeURL != "http://localhost:9996" && DWQueryParam.PageSize != 0)
            {
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
            var DWSettings = new DWHSettingRepository(Tenant);
            var temp = DWSettings.GetSingleDWHSetting(Tenant);
            var isParentTenant = DWSettings.IsParentTenant(Tenant);
            if (!isParentTenant)//temp != null &&  temp.Tenant != temp.ParentTenant)
            {
                TenantWhere = ".[Source Tenant] = ";
            }

            if (FinalQuery.Contains("where"))
            {
                FinalQuery = FinalQuery.Replace("where", "where " + Fact + TenantWhere + "@Tenant" + " and");
            }
            else if (FinalQuery.Contains("group by"))
            {
                FinalQuery = FinalQuery.Replace("group by", "where " + Fact + TenantWhere + "@Tenant" + " group by");
            }
            else
            {
                FinalQuery = FinalQuery + " where " + Fact + TenantWhere + "@Tenant";
            }

            sqlCommandDefinition.Parameters.Add(new SqlParameterDetails() { ParameterName = "@Tenant", Value = Tenant.ToString() });


            FinalQuery = FinalQuery + PagingString + offsetPagingString;
            //if (DWQueryParam.PageSize != 0)
            //{
            //    FinalQuery = FinalQuery + PagingString;
            //}
            //else 
            //if (!string.IsNullOrEmpty(DWQueryParam.ColumnsSort))
            //{
            //    FinalQuery = FinalQuery + " ORDER BY " + DWQueryParam.ColumnsSort;
            //}

            sqlCommandDefinition.SQLString = FinalQuery;



            return sqlCommandDefinition;
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

}
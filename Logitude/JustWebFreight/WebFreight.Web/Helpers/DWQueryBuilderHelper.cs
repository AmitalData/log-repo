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

            foreach (var Myfilter in FiltersList)
            {
                if (Myfilter.FilterItems.Count > 0)
                {
                    GetWhereJoined(Myfilter.FilterItems, InnerTables);
                }
                else
                {
                    if (Myfilter.ParentDimTabelName != null && InnerTables.Where(a => a.ParentDimTabelName == Myfilter.ParentDimTabelName).Count() == 0)
                    {
                        InnerTables.Add(Myfilter);
                    }
                }
            }

        }

        private void GetWhereStmtForFiltersList(List<DWObjectFieldsDetails> FiltersList, string AndOr)
        {
            foreach (var Myfilter in FiltersList)
            {
                var isHaveMultiSelect = false;
                if (Myfilter.FilterItems.Count > 0)
                {
                    if (this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true)
                    {
                        WhereStmt = WhereStmt + " ( ";
                    }
                    GetWhereStmtForFiltersList(Myfilter.FilterItems, Myfilter.AndOr);
                    if (WhereStmt == " where ")
                    {
                        WhereStmt = "";
                    }
                    else if (WhereStmt.Substring(WhereStmt.Length - 4).Contains("And") || WhereStmt.Substring(WhereStmt.Length - 4).Contains("Or"))
                    {
                        WhereStmt = WhereStmt.Substring(0, WhereStmt.Length - 4);
                    }
                    if (WhereStmt != "" && GetIfFiltersHaveValues(Myfilter.FilterItems) == true)
                    {
                        WhereStmt = WhereStmt + " ) ";
                    }

                }
                else
                {
                    //Myfilter.FilterItems.filter(a => a.TextValue != null).forEach((filter) => {
                    if (Myfilter.TextValue != null)
                    {
                        var filter = Myfilter;
                        var OperationSimpol = "";
                        if ( filter!= null && filter.Operation != null && filter.Operation.Code == "Equals")
                        {
                            if (filter.DataTypeCode == "Integer" || filter.DataTypeCode == "Double" || filter.DataTypeCode == "Decimal")
                            {
                                OperationSimpol = " = @@ ";
                            }
                            else
                            {

                                OperationSimpol = " IN ( '";
                                OperationSimpol = this.BuildMultiValueSql(filter.TextValue.ToString(), OperationSimpol);
                                isHaveMultiSelect = true;


                            }
                        }
                        else if (filter != null && filter.Operation != null && filter.Operation.Code == "NotEqual")
                        {
                            if (filter.DataTypeCode == "Integer" || filter.DataTypeCode == "Double" || filter.DataTypeCode == "Decimal")
                            {
                                OperationSimpol = " <> @@ ";
                            }
                            else
                            {

                                OperationSimpol = " not IN ( '";
                                OperationSimpol = BuildMultiValueSql(filter.TextValue.ToString(), OperationSimpol);
                                isHaveMultiSelect = true;

                                //abed
                            }
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "StartsWith")
                        {
                            OperationSimpol = " like '@@%' ";
                        }
                        //else if (filter.Operation.Code == filter.IsNullOp.Code) {
                        //    OperationSimpol = " like '%@@' ";
                        //}
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "IsNull")
                        {
                            OperationSimpol = " is null ";
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "IsNotNull")
                        {
                            OperationSimpol = " is not null ";
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "GreaterThanOrEqual")
                        {
                            OperationSimpol = " >= @@ ";
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "LargerThan")
                        {
                            OperationSimpol = " > @@ ";
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "LessThan")
                        {
                            OperationSimpol = " < @@ ";
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "LessThanOrEqual")
                        {
                            OperationSimpol = " <= @@ ";
                        }



                        if (filter != null && filter.Operation != null &&  filter.Operation.Code == "IsNull")
                        {
                            WhereStmt += (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is null or " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " = '' " + " " + AndOr + " ";
                        }
                        else if (filter != null && filter.Operation != null &&  filter.Operation.Code == "IsNotNull")
                        {
                            WhereStmt += (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is not null and " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " <> '' " + " " + AndOr + " ";
                        }
                        else
                        {
                            var operation = !isHaveMultiSelect ? OperationSimpol.Replace("@@", filter.TextValue.ToString()) : OperationSimpol;
                            WhereStmt += (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + operation + " " + AndOr + " ";//" = " + "'" + filter.TextValue + "' and ";
                        }
                    }
                    else
                    {
                        //if (this.WhereStmt == " where  ( ") {
                        //    this.WhereStmt = "";
                        //}
                    }
                    //});
                }
            }

        }

        private string BuildMultiValueSql(string textValue, string operationSimpol)
        {

            var result = operationSimpol;
            if (!string.IsNullOrEmpty(textValue))
            {
                var values = textValue.Split(';');
                if (values.Length > 0)
                {
                    foreach (var item in values)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            result += (item + "','");
                        }
                    }


                    result += ")";
                    result = result.Replace(",')", ")");

                }
                else
                {
                    result += " ')";
                }

            }
            else
            {
                result += " ')";
            }

            return result;
        }

        private bool GetIfFiltersHaveValues(List<DWObjectFieldsDetails> FiltersList)
        {
            return FiltersList.Where(a => a.TextValue != null).Count() > 0;
        }

        public string GetQuerySQL(DWQueryData DWQueryParam)
        {
            DWObjectFieldQuery OFieldQuery = new DWObjectFieldQuery(Tenant);
            var ColumnsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(DWQueryParam.Columns);
            var FilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(DWQueryParam.Filters);
            var Columns = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(ColumnsXML);
            var Filters = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(FilterXML);
            string PagingString = " ORDER BY " + "Id_Number OFFSET " + DWQueryParam.PageIndex + " ROWS FETCH NEXT " + DWQueryParam.PageSize + " ROWS ONLY";
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
                if (field.IsMeasurement)
                {
                    SelectStmt.Append(field.AggregationTypeCode + "(" + field.DWObjectTableCode + "." + field.Code + ")" + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                }
                else
                {
                    SelectStmt.Append(field.DWObjectTableCode + "." + field.Code + (!string.IsNullOrEmpty(field.DisplayName) ? " as " + field.DisplayName + "," : ","));
                    GroupByStmt.Append(field.DWObjectTableCode + "." + field.Code + ",");
                }

                if (FromTables.Where(a => a == field.DWObjectTableCode).Count() == 0)
                {
                    FromTables.Add(field.DWObjectTableCode);
                }
                if (field.ParentDimTabelName != null && InnerTables.Where(a => a.ParentDimTabelName == field.ParentDimTabelName).Count() == 0)
                {
                    InnerTables.Add(field);
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
                GetWhereStmtForFiltersList(MyFilterList, Filters.AndOr);
            }

            //this.Notes = SelectStmt;
            FromTables = FromTables.Where(a => a != Fact).ToList();
            //var MeFactName = "";
            foreach (var mytbl in InnerTables)
            {
                //var Key = this.AllFieldsObsList.filter(a => a.DWObjectTableCode == mytbl.ParentDimTabelName && a.IsPrimaryKey == true)[0];
                var Key = OFieldQuery.GetPrimaryKeyFieldForDWObjectTable(mytbl.ParentDimTabelName);
                //MeFactName = OFieldQuery.GetFactTableCode(mytbl.ParentDimTabelName);
                var FactKey = OFieldQuery.GetFactKeyFieldForDWDimTable(Fact);
                //this.AllFieldsDataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
                FinalSelectStmt += " inner join " + mytbl.ParentDimTabelName + " on " + Fact + "." + ((FactKey.DataTypeCode.ToLower() == "lookup" || FactKey.DataTypeCode.ToLower() == "dimension") ? FactKey.DisplayName : FactKey.Code) + " = " + mytbl.ParentDimTabelName + "." + Key.Code;


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

            if (FinalQuery.Contains("where"))
            {
                FinalQuery = FinalQuery.Replace("where", "where " + Fact + ".[Parent Tenant] = " + Tenant + " and");
            }
            else if (FinalQuery.Contains("group by"))
            {
                FinalQuery = FinalQuery.Replace("group by", "where " + Fact + ".[Parent Tenant] = " + Tenant + " group by");
            }
            else
            {
                FinalQuery = FinalQuery + " where " + Fact + ".[Parent Tenant] = " + Tenant;
            }
            if (DWQueryParam.PageIndex == 0 && DWQueryParam.PageSize == 0)
            {
                FinalQuery = FinalQuery + PagingString;
            }
            
            return FinalQuery;
        }

        public DataTable GetDWQueryData(string SQL)
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
                    SqlCommand commandSourceData = new SqlCommand(SQL, sourceConnection);
                    SqlDataReader reader = commandSourceData.ExecuteReader();
                    dataTable.Load(reader);
                    reader.Close();
                }
                scope.Complete();

                return dataTable;
            }

        }

    }
}
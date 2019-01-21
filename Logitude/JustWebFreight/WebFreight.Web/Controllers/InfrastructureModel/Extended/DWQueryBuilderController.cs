using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.Helpers;
using System.Transactions;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Global.Data.GlobalModel.Helpers;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Controllers.InfrastructureModel.Generated.PMControllers
{


    public partial class DWQueryBuilderController : ApiController
    {


        public HttpResponseMessage GetDWQueryData(string SQL, string Table)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                if (SQL.Contains("where"))
                {
                    SQL = SQL.Replace("where", "where " + Table + ".[Parent Tenant] = " + authToken.Tenant + " and");
                }
                else if (SQL.Contains("group by"))
                {
                    SQL = SQL.Replace("group by", "where " + Table + ".[Parent Tenant] = " + authToken.Tenant + " group by");
                }
                else
                {
                    SQL = SQL + " where " + Table + ".[Parent Tenant] = " + authToken.Tenant;
                }

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
                    //var DT = JsonConvert.SerializeObject(dataTable);
                    //var Rows = dataTable.Rows;
                    //var Columns = dataTable.Columns;

                    //string logKey = PerformanceLogger.LogCurrentTime();
                    //string token = HttpContext.Current.Request.Headers["Token"];
                    //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                    //DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                    //List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenant(authToken.Tenant, DWOTId).ToList();

                    //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                    return Request.CreateResponse(HttpStatusCode.OK, dataTable);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDWDataForDimTabel(string Tabel, string Field, string SearchData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(authToken.Tenant);
                DWObjectTablePM dWObjectTablePM = dWObjectTableQuery.GetSinglePM(Tabel, authToken.Tenant);
                bool IsClosed = dWObjectTablePM.IsClosed;
                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    var currentDb = GlobalDbHelper.GetGlobalDB(0);
                    string dbConnectionInfo = currentDb.SharedDWConnection;
                    DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
                    var dataTable = new DataTable();

                    using (SqlConnection sourceConnection = new SqlConnection(connection.ConnectionString))
                    {
                        var SQL = "select DISTINCT top 10 " + Field + " from " + Tabel + " where " + (!IsClosed ? (Tabel + ".[Parent Tenant] = " + authToken.Tenant + " and " + Field + " like '%" + SearchData + "'") : (Field + " like '%" + SearchData + "'"));
                        sourceConnection.Open();
                        SqlCommand commandSourceData = new SqlCommand(SQL, sourceConnection);
                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        dataTable.Load(reader);
                        reader.Close();
                    }
                    scope.Complete();
                    //var DT = JsonConvert.SerializeObject(dataTable);
                    //var Rows = dataTable.Rows;
                    //var Columns = dataTable.Columns;

                    //string logKey = PerformanceLogger.LogCurrentTime();
                    //string token = HttpContext.Current.Request.Headers["Token"];
                    //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                    //DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                    //List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenant(authToken.Tenant, DWOTId).ToList();

                    //PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                    return Request.CreateResponse(HttpStatusCode.OK, dataTable);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);
                int tenant = authToken.Tenant;
                var Tabel = filters.Filter1Name;
                var Field = filters.Filter2Name;
                var temp = filters.Filter3Name;
                var Field1 = "";
                var Field2 = "";
                var Field3 = "";
                var Field4 = "";
                var Field5 = "";
                var Field6 = "";
                var Field7 = "";
                var Field8 = "";
                var Field9 = "";
                var Field10 = "";
                var LovAdditionalFields = !string.IsNullOrEmpty(temp) ? temp.Split(',') : null;

                var SearchData = filters.Filter2Value;
                DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(authToken.Tenant);
                DWObjectTablePM dWObjectTablePM = dWObjectTableQuery.GetSinglePM(Tabel, authToken.Tenant);
                bool IsClosed = dWObjectTablePM.IsClosed;
                string WhereStmt = " where " + Field + " is not null";
                string PagingString = " ORDER BY " + Field + " OFFSET " + filters.PageIndex + " ROWS FETCH NEXT " + filters.PageSize + " ROWS ONLY";

                if (!string.IsNullOrEmpty(SearchData))
                {
                    WhereStmt = WhereStmt + " and " + (Field + " like '" + SearchData + "%'");
                }
                if (!IsClosed)
                {
                    if (Tabel != "DIM_Tenants")
                    {
                        WhereStmt = (string.IsNullOrEmpty(WhereStmt) ? " where " : WhereStmt + " and ") + (Tabel + ".[Parent Tenant] = " + authToken.Tenant); //authToken.Tenant
                    }

                    //if (!string.IsNullOrEmpty(SearchData))
                    //{
                    //    WhereStmt = WhereStmt + (" and " + Field + " like '%" + SearchData + "'");
                    //}
                }

                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    var currentDb = GlobalDbHelper.GetGlobalDBWithNoCache(0);
                    string dbConnectionInfo = currentDb.SharedDWConnection;
                    DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
                    var dataTable = new DataTable();

                    using (SqlConnection sourceConnection = new SqlConnection(connection.ConnectionString))
                    {
                        var SQL = "select DISTINCT " + Field + " ";
                        if (LovAdditionalFields != null)
                        {
                            int index = 0;
                            foreach (var item in LovAdditionalFields)
                            {
                                SQL = SQL + "," + item;
                                if (index == 0)
                                {
                                    Field1 = LovAdditionalFields[index];
                                }
                                else if (index == 1)
                                {
                                    Field2 = LovAdditionalFields[index];
                                }
                                else if (index == 2)
                                {
                                    Field3 = LovAdditionalFields[index];
                                }
                                else if (index == 3)
                                {
                                    Field4 = LovAdditionalFields[index];
                                }
                                else if (index == 4)
                                {
                                    Field5 = LovAdditionalFields[index];
                                }
                                else if (index == 5)
                                {
                                    Field6 = LovAdditionalFields[index];
                                }
                                else if (index == 6)
                                {
                                    Field7 = LovAdditionalFields[index];
                                }
                                else if (index == 7)
                                {
                                    Field8 = LovAdditionalFields[index];
                                }
                                else if (index == 8)
                                {
                                    Field9 = LovAdditionalFields[index];
                                }
                                else if (index == 9)
                                {
                                    Field10 = LovAdditionalFields[index];
                                }
                                index++;

                            }
                        }
                        SQL = SQL + " from " + Tabel + WhereStmt + PagingString;
                        sourceConnection.Open();
                        SqlCommand commandSourceData = new SqlCommand(SQL, sourceConnection);
                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        dataTable.Load(reader);
                        reader.Close();
                    }
                    object Count = 0;
                    using (SqlConnection sourceConnection = new SqlConnection(connection.ConnectionString))
                    {

                        var CountSQL = "select Count(DISTINCT " + Field + ") from " + Tabel + WhereStmt;
                        sourceConnection.Open();
                        SqlCommand commandSourceData = new SqlCommand(CountSQL, sourceConnection);
                        Count = commandSourceData.ExecuteScalar();
                    }

                    scope.Complete();

                    ServiceResponse response = new ServiceResponse();
                    List<FactDataTable> FactDataList = new List<FactDataTable>();
                    var TempFactDataList = (from DataRow dr in dataTable.Rows
                                            select dr);
                    //new FactDataTable()
                    //{ 
                    //    Name = dr[Field.Replace("[","").Replace("]","")].ToString(), 
                    //}).ToList();
                    foreach (var dr in TempFactDataList)
                    {
                        var temprec = new FactDataTable();
                        temprec.Field = dr[Field.Replace("[", "").Replace("]", "")].ToString();
                        if (!string.IsNullOrEmpty(Field1))
                        {
                            temprec.Field1 = dr[Field1.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field2))
                        {
                            temprec.Field2 = dr[Field2.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field3))
                        {
                            temprec.Field3 = dr[Field3.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field4))
                        {
                            temprec.Field4 = dr[Field4.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field5))
                        {
                            temprec.Field5 = dr[Field5.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field6))
                        {
                            temprec.Field6 = dr[Field6.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field7))
                        {
                            temprec.Field7 = dr[Field7.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field8))
                        {
                            temprec.Field8 = dr[Field8.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field9))
                        {
                            temprec.Field9 = dr[Field9.Replace("[", "").Replace("]", "")].ToString();
                        }
                        if (!string.IsNullOrEmpty(Field10))
                        {
                            temprec.Field10 = dr[Field10.Replace("[", "").Replace("]", "")].ToString();
                        }

                        FactDataList.Add(temprec);
                    }
                    if (filters.GetCount)
                    {
                        response.Count = (int)Count;
                    }
                    response.Result = FactDataList;
                    //entityLists = entityLists.Skip(skippedShipments);
                    //entityLists = entityLists.Take(queryOperations.PageSize);
                    //List<DataRow> sequence = dataTable.AsEnumerable().ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }

                //HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, ""); 

                //return reponseMessage;
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [ActionName("PostQueryData")]
        public HttpResponseMessage Post(DWQueryData QueryData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                var XML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.Columns);
                var FilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(QueryData.Filters);
                var temp = LogitudeXmlSerializer.DeserializeObject<List<DWObjectFieldsDetails>>(XML);
                var temp1 = LogitudeXmlSerializer.DeserializeObject<DWObjectFieldsDetails>(FilterXML);
                //IWebFreightContext objectContext = WebFreightContext.GetContext(entityPM.Tenant);
                //QueryColumnService service = new QueryColumnService(objectContext, entityPM.Tenant);
                //service.Create(entityPM);

                return Request.CreateResponse(HttpStatusCode.OK, FilterXML);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [ActionName("PostGetDWQueryData")]
        public HttpResponseMessage PostGetDWQueryData(DWQueryData DWQueryParam)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);
                DWObjectFieldQuery OFieldQuery = new DWObjectFieldQuery(authToken.Tenant);



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
                string WhereStmt = " where "; 
                
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

                //this.SelectedFieldsDataSource.forEach((field) => {
                //    if (field.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == field.ParentDimTabelName).length == 0)
                //    {
                //        this.InnerTables.push(field);
                //    }
                //});

                //this.SelectedFiltersDataSource.forEach((Myfilter) => {
                //    Myfilter.FilterItems.forEach((filter) => {
                //        if (filter.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == filter.ParentDimTabelName).length == 0) {
                //            this.InnerTables.push(filter);
                //        }
                //    }); 
                //});
                if (Filters != null)
                {
                    var MyFilterList = new List<DWObjectFieldsDetails>();
                    MyFilterList.Add(Filters);
                    GetWhereJoined(MyFilterList,InnerTables);
                    GetWhereStmtForFiltersList(MyFilterList, Filters.AndOr, WhereStmt);
                }

                //this.Notes = SelectStmt;
                FromTables = FromTables.Where(a => a != Fact).ToList();

                foreach (var mytbl in InnerTables)
                {
                    //var Key = this.AllFieldsObsList.filter(a => a.DWObjectTableCode == mytbl.ParentDimTabelName && a.IsPrimaryKey == true)[0];
                    var Key = OFieldQuery.GetPrimaryKeyFieldForDWObjectTable(mytbl.ParentDimTabelName);
                    var FactKey = OFieldQuery.GetFactKeyFieldForDWDimTable(mytbl.ParentDimTabelName);
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

                return Request.CreateResponse(HttpStatusCode.OK, FinalQuery);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

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

        private void GetWhereStmtForFiltersList(List<DWObjectFieldsDetails> FiltersList, string AndOr,string WhereStmt)
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
                    GetWhereStmtForFiltersList(Myfilter.FilterItems, Myfilter.AndOr, WhereStmt);
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
                        if (filter.Operation.Code == equalsOp.Code)
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
                        else if (filter.Operation.Code == notEqualsOp.Code)
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
                        else if (filter.Operation.Code == startsWithOp.Code)
                        {
                            OperationSimpol = " like '@@%' ";
                        }
                        //else if (filter.Operation.Code == filter.IsNullOp.Code) {
                        //    OperationSimpol = " like '%@@' ";
                        //}
                        else if (filter.Operation.Code == IsNullOp.Code)
                        {
                            OperationSimpol = " is null ";
                        }
                        else if (filter.Operation.Code == IsNotNullOp.Code)
                        {
                            OperationSimpol = " is not null ";
                        }
                        else if (filter.Operation.Code == greaterThanOrEqualOp.Code)
                        {
                            OperationSimpol = " >= @@ ";
                        }
                        else if (filter.Operation.Code == largerThanOp.Code)
                        {
                            OperationSimpol = " > @@ ";
                        }
                        else if (filter.Operation.Code == lessThanOp.Code)
                        {
                            OperationSimpol = " < @@ ";
                        }
                        else if (filter.Operation.Code == lessThanOrEqualOp.Code)
                        {
                            OperationSimpol = " <= @@ ";
                        }



                        if (filter.Operation.Code == IsNullOp.Code)
                        {
                            WhereStmt += (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is null or " + (!string.IsNullOrEmpty(filter.ParentDimTabelName) ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " = '' " + " " + AndOr + " ";
                        }
                        else if (filter.Operation.Code == IsNotNullOp.Code)
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

        ObjectFieldOperator startsWithOp = new ObjectFieldOperator("StartsWith", "Starts With");
        ObjectFieldOperator equalsOp = new ObjectFieldOperator("Equals", "Equals to");
        ObjectFieldOperator notEqualsOp = new ObjectFieldOperator("NotEqual", "Not Equal to");
        ObjectFieldOperator largerThanOp = new ObjectFieldOperator("LargerThan", "Greater Than");
        ObjectFieldOperator lessThanOp = new ObjectFieldOperator("LessThan", "Less Than");
        ObjectFieldOperator greaterThanOrEqualOp  = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
        ObjectFieldOperator lessThanOrEqualOp  = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
        ObjectFieldOperator BetweenOp = new ObjectFieldOperator("Between", "Between");
        ObjectFieldOperator IsNullOp = new ObjectFieldOperator("IsNull", "Is Empty");
        ObjectFieldOperator IsNotNullOp = new ObjectFieldOperator("IsNotNull", "Has Value");

    }

    class FactDataTable
    {
        public string Field { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
    }
}

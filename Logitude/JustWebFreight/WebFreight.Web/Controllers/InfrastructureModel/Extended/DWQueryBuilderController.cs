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
                    string TenantWhere = ".[Parent Tenant] = ";
                    var DWSettings = new DWHSettingRepository(authToken.Tenant);
                    var temp = DWSettings.GetSingleDWHSetting(authToken.Tenant);
                    if (temp != null && temp.Tenant != temp.ParentTenant)
                    {
                        TenantWhere = ".[Source Tenant] = ";
                    }
                    using (SqlConnection sourceConnection = new SqlConnection(connection.ConnectionString))
                    {
                        var SQL = "select DISTINCT top 10 " + Field + " from " + Tabel + " where " + (!IsClosed ? (Tabel + TenantWhere + authToken.Tenant + " and " + Field + " like '%" + SearchData + "'") : (Field + " like '%" + SearchData + "'"));
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
                string PagingString = " ORDER BY " + Field;//+ " OFFSET " + filters.PageIndex + " ROWS FETCH NEXT " + filters.PageSize + " ROWS ONLY";

                if (!string.IsNullOrEmpty(SearchData))
                {
                    WhereStmt = WhereStmt + " and (" + (Field + " like '" + SearchData + "%')");
                }
                
                if (!IsClosed)
                {
                    if (Tabel != "DIM_Tenants" && Tabel != "DIM_Dates")
                    {
                        WhereStmt = (string.IsNullOrEmpty(WhereStmt) ? " where " : WhereStmt + " and ") + (Tabel + ".[Parent Tenant] = " + authToken.Tenant); //authToken.Tenant
                    }
                    if (Tabel == "DIM_Dates")
                    {
                        WhereStmt = (string.IsNullOrEmpty(WhereStmt) ? " where " : WhereStmt + " and ") + (Tabel + ".[Date Key] not in (-1,-2,-3) "); //authToken.Tenant
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
                        if (!string.IsNullOrEmpty(SearchData))
                        {
                            if (!string.IsNullOrEmpty(Field1))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field1 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field2))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field2 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field3))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field3 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field4))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field4 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field5))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field5 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field6))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field6 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field7))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field7 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field8))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field8 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field9))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field9 + " like '" + SearchData + "%')");
                            }
                            if (!string.IsNullOrEmpty(Field10))
                            {
                                WhereStmt = WhereStmt.Replace(")", "");
                                WhereStmt = WhereStmt + " or " + (Field10 + " like '" + SearchData + "%')");
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
                

                DWQueryBuilderHelper QBHelper = new DWQueryBuilderHelper(authToken.Tenant);
                string MySqlString = QBHelper.GetQuerySQL(DWQueryParam);
                DataTable MyData = QBHelper.GetDWQueryData(MySqlString);
                DWQueryDataResult myResult = new DWQueryDataResult();
                myResult.SQLDataResult = MyData;
                myResult.SQLString = MySqlString;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [ActionName("PostGetDateFilterSample")]
        public HttpResponseMessage PostGetDateFilterSample(DWObjectFieldsDetails filter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("Shipment", "READ", authToken.Tenant);
                string DateSample = "";

                DataWarehouseHelper dataWarehouseHelper = new DataWarehouseHelper();
                if (filter != null)
                {
                    DateSample = dataWarehouseHelper.ResolveWarehoueDateField("", filter.OperationCode, filter.TextValue.ToString(), authToken.Tenant,true);
                }
                //DateSample = DateSample.Replace("'","");
                //DWQueryBuilderHelper QBHelper = new DWQueryBuilderHelper(authToken.Tenant);
                //string MySqlString = QBHelper.GetQuerySQL(DWQueryParam);
                //DataTable MyData = QBHelper.GetDWQueryData(MySqlString);
                //DWQueryDataResult myResult = new DWQueryDataResult();
                //myResult.SQLDataResult = MyData;
                //myResult.SQLString = MySqlString;
                return Request.CreateResponse(HttpStatusCode.OK, DateSample);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



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

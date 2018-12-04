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


        public HttpResponseMessage GetDWQueryData(string SQL,string Table)
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

        public HttpResponseMessage GetDWDataForDimTabel(string Tabel, string Field,string SearchData)
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
                    WhereStmt = (string.IsNullOrEmpty(WhereStmt) ? " where " : WhereStmt + " and ") + (Tabel + ".[Parent Tenant] = " + authToken.Tenant); //authToken.Tenant
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
                        var SQL = "select DISTINCT " + Field + " from " + Tabel + WhereStmt + PagingString;
                        
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
                    FactDataList = (from DataRow dr in dataTable.Rows
                                   select new FactDataTable()
                                   { 
                                       Name = dr[Field.Replace("[","").Replace("]","")].ToString(), 
                                   }).ToList();
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

    }

    class FactDataTable
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

///#define tzuri_req
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.SQL;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;
using System.Data.Entity.Infrastructure;
using Logitude.Customs.Data.EntityPOCOs;
using System.Windows.Media.Effects;
using System.Diagnostics;
using Simplog.Global.Data.GlobalModel.Helpers;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExternalTasksQueueWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ExternalTasksQueueWcfService.svc or ExternalTasksQueueWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class ExternalTasksQueueWcfService : IExternalTasksQueueWcfService, IExternalTasksQueueExportSignWcfService
    {




#if !tzuri_req
        public Response GetDataCFIRDEC( Dictionary<string, string> queryParams, int tenant)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var response = new Response();
            try
            {

                if (tenant == 0)
                {
                    response.HasError = true;
                    response.ErrorMessage = "Tenant parameter is missing.";
                    return (response);
                }
                string log_level = "";
                if(queryParams.ContainsKey("LOG_LEVEL")) log_level= queryParams["LOG_LEVEL"];
                string inv_field_list = queryParams["INV_FIELD_LIST"];
                string dec_field_list = queryParams["DEC_FIELD_LIST"];
                string mod_field_list = queryParams["MOD_FIELD_LIST"];
                string sup_field_list = queryParams["SUP_FIELD_LIST"];

                string det_field_list = queryParams["DET_FIELD_LIST"];
                string con_field_list = queryParams["CON_FIELD_LIST"];

                string dec_list = queryParams["DEC_LIST"];
                Dictionary<string, string> all_results = new Dictionary<string, string>();

                string sqlQuery = $"select {inv_field_list} from customs.SUPPLIERINVOICES where DECLARATIONID in ({dec_list})  and tenant={tenant}";
                if (log_level == "DEBUG") all_results.Add("SI_SQL", sqlQuery);

                var shipmentsContext = new Simplog.Data.ShipmentsModel.ShipmentsContext();
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = shipmentsContext.Database.Connection.ConnectionString;
                    connection.Open();


                    
                    using (var cmd = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    List<List<string>> all_lines = new List<List<string>>();
                                    Dictionary<string, string> results = new Dictionary<string, string>();
                                    List<string> one_line = new List<string>();
                                    Console.WriteLine(reader.ToString());
                                    string dec_id = "";
                                    string inv_counter = "";
                                    string key = "";
                                    string vendor_id = "";
                                    for (int pos = 0; reader.FieldCount > pos; pos++)
                                    {
                                        if(reader.GetName(pos)== "DECLARATIONID")
                                        { dec_id = reader[pos].ToString();key = dec_id; }
                                        if (reader.GetName(pos) == "INVOICECOUNTERKEY")
                                        { key = $"{key}_{reader[pos].ToString()}"; inv_counter = reader[pos].ToString(); }
                                        if (reader.GetName(pos) == "VENDORID")
                                        { vendor_id = reader[pos].ToString(); }
                                        one_line.Add(reader[pos].ToString());
                                    }
                                    all_lines.Add(one_line);

                                    if (!string.IsNullOrEmpty(vendor_id))
                                    {
                                        sqlQuery = $"select VENDORNUMBER,VENDORNAME from customs.CUSTOMSVENDORS where ID='{vendor_id}' and tenant={tenant}";
                                        get_table_lines("A28", sqlQuery, ref results, connection, log_level);//CUSTOMSVENDORS
                                    }

                                    results.Add("SI", JsonConvert.SerializeObject(all_lines));//SUPPLIERINVOICES
                                    
                                    sqlQuery = $"select {dec_field_list} from customs.DECLARATIONS where id='{dec_id}' and tenant={tenant}";
                                    get_table_lines("DEC",sqlQuery, ref results, connection, log_level);//DECLARATIONS

                                    sqlQuery = $"select {con_field_list} from customs.CONSIGNMENTS where DECLARATIONID='{dec_id}' and tenant={tenant}";
                                    get_table_lines("CON",sqlQuery, ref results, connection, log_level);//CONSIGNMENTS

                                    sqlQuery = $"select {det_field_list} from customs.DECLARATIONTAXES where DECLARATIONID='{dec_id}' and tenant={tenant}";
                                    get_table_lines("DET",sqlQuery, ref results, connection, log_level);//DECLARATIONTAXES

                                    sqlQuery = $"select {mod_field_list} from customs.SUPPLIERINVOICEMODIFICATIONS where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and tenant={tenant}";
                                    get_table_lines("SIM",sqlQuery, ref results, connection, log_level);//SUPPLIERINVOICEMODIFICATIONS

                                    sqlQuery = $"select {sup_field_list} from customs.SUPPLIERINVOICEITEMS where DECLARATIONID='{dec_id}' and COUNTERKEY={inv_counter} and tenant={tenant}";
                                    get_table_lines("SII",sqlQuery,  ref results, connection, log_level);//SUPPLIERINVOICEITEMS

                                    sqlQuery = $"select CURRENCYTYPECODE from customs.SUPPLIERINVOICEFREIGHTAMOUNTS where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and tenant={tenant}";
                                    get_table_lines("A29",sqlQuery , ref results, connection, log_level);//SUPPLIERINVOICEFREIGHTAMOUNTS



                                    sqlQuery = $"select LINENUMBER from customs.SUPPLIERINVOICEITEMS where DECLARATIONID='{dec_id}' and COUNTERKEY='{inv_counter}' and tenant={tenant}";
                                    List<List<string>> line_list = get_table_lines_list(sqlQuery, connection);
                                    foreach (List<string> line_num in line_list)
                                    {
                                        sqlQuery = $"select sum(TAXAMOUNT) from customs.SUPPLIERINVOICEITEMSTAXES where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and LINENUMBER={line_num[0]} and TAXTYPECODE='1' and tenant={tenant}";
                                        get_table_lines($"A19_1_{line_num[0]}", sqlQuery, ref results, connection, log_level);//SUPPLIERINVOICEITEMSTAXES

                                        sqlQuery = $"select sum(TAXAMOUNT) from customs.SUPPLIERINVOICEITEMSTAXES where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and LINENUMBER={line_num[0]} and TAXTYPECODE='15' and tenant={tenant}";
                                        get_table_lines($"A19_15_{line_num[0]}", sqlQuery, ref results, connection, log_level);//SUPPLIERINVOICEITEMSTAXES

                                        sqlQuery = $"select sum(TAXAMOUNT) from customs.SUPPLIERINVOICEITEMSTAXES where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and LINENUMBER={line_num[0]} and TAXTYPECODE='16' and tenant={tenant}";
                                        get_table_lines($"A19_16_{line_num[0]}", sqlQuery, ref results, connection, log_level);//SUPPLIERINVOICEITEMSTAXES

                                        sqlQuery = $"select TAXRATE from customs.SUPPLIERINVOICEITEMSTAXES where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and LINENUMBER={line_num[0]} and TAXTYPECODE='1' and tenant={tenant}";
                                        get_table_lines($"A30_{line_num[0]}", sqlQuery, ref results, connection, log_level);//SUPPLIERINVOICEITEMSTAXES

                                        sqlQuery = $"select CERTIFICATENUMBER from customs.SUPPLIERINVIOCEITEMCERTIFICATS where DECLARATIONID='{dec_id}' and INVOICECOUNTERKEY={inv_counter} and LINENUMBER={line_num[0]} and tenant={tenant}";
                                        get_table_lines($"A31_{line_num[0]}", sqlQuery, ref results, connection, log_level);//SUPPLIERINVIOCEITEMCERTIFICATS
                                    }

                                    sqlQuery = $"select distinct TYPECODE from customs.SUPPLIERINVOICEMODIFICATIONS where DECLARATIONID='{dec_id}' and tenant={tenant}";
                                    line_list = get_table_lines_list(sqlQuery, connection);
                                    foreach (List<string> line_num in line_list)
                                    {
                                        sqlQuery = $"select EXTRANUMERICDATA from customs.MODIFICATIONANDDISCOUNTTYPES where CODE='{line_num[0]}'";
                                        get_table_lines($"A32_{line_num[0]}", sqlQuery, ref results, connection, log_level);//MODIFICATIONANDDISCOUNTTYPES
                                    }


                                    sqlQuery = $"Select ID from customs.CUSTOMSCOLLATERALS where DECLARATIONID ='{dec_id}' AND TENANT={tenant}";
                                    line_list = get_table_lines_list(sqlQuery, connection);//CUSTOMSCOLLATERALS
                                    List<string> id_list = new List<string>();
                                    foreach (List<string> line_num in line_list)
                                    {
                                        id_list.Append(line_num[0]);
                                    }

                                    if (id_list.Count > 0)
                                    {
                                        string id_str = string.Join("','",id_list);

                                        sqlQuery = $"Select CUSTOMSTAPGFILE,CUSTOMSNUMERAL from customs.CUSTOMSCOLLATERALSANSWERS where CUSTOMSCOLLATERALID in ('{id_str}') AND TENANT={tenant}";
                                        get_table_lines("A34", sqlQuery, ref results, connection, log_level);//CUSTOMSCOLLATERALSANSWERS
                                    }




                                    sqlQuery = $"Select ID from customs.CUSTOMSCOLLATERALS where DECLARATIONID ='{dec_id}' AND COLLATERALREQUESTSTATUSCODE='2' AND TENANT={tenant}";
                                    line_list = get_table_lines_list(sqlQuery, connection);//CUSTOMSCOLLATERALS
                                    id_list = new List<string>();
                                    foreach (List<string> line_num in line_list)
                                    {
                                        id_list.Append(line_num[0]);
                                    }

                                    if (id_list.Count > 0)
                                    {
                                        string id_str = string.Join("','", id_list);

                                        sqlQuery = $"Select sum(ALLOCATEDAMOUNT) from customs.CUSTOMSCOLLATERALSANSWERS where CUSTOMSCOLLATERALID in ('{id_str}') AND TENANT={tenant}";
                                        get_table_lines("A35", sqlQuery, ref results, connection, log_level);//CUSTOMSCOLLATERALSANSWERS
                                    }




                                    sqlQuery = $"Select Sum(GROSSMASSMEASURE) from customs.CONSIGNMENTPACKAGES where DECLARATIONID = '{dec_id}' and PACKAGEMEASUREQUALIFIERCODE = '2' AND TENANT = {tenant}";
                                    get_table_lines("A36", sqlQuery, ref results, connection, log_level);//CONSIGNMENTPACKAGES



                                    all_results.Add(key, JsonConvert.SerializeObject(results));
                                }
                            }

                        }
                    }
                    response.HasError = false;

                    stopwatch.Stop();
                    double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                    if (log_level == "DEBUG") all_results.Add("ALL_SQL", elapsedSeconds.ToString());
                    response.Result = JsonConvert.SerializeObject(all_results);
                }
                return (response);
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return (response);
            }
        }


        void get_table_lines(string id,string sqlQuery,ref Dictionary<string, string> results, SqlConnection connection,string log_level)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<List<string>> all_lines = new List<List<string>>();
            using (var cmd = new SqlCommand(sqlQuery, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.ToString());
                            List<string> one_line = new List<string>();

                            for (int pos = 0; reader.FieldCount > pos; pos++)
                            {
                                one_line.Add(reader[pos].ToString());
                            }
                            all_lines.Add(one_line);
                        }
                        
                    }

                }
            }
            if (log_level == "DEBUG")
            {
                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                results.Add($"{id}_SQL", $"{elapsedSeconds.ToString()}:{sqlQuery}");
            }
            results.Add(id, JsonConvert.SerializeObject(all_lines));
            
            return;
        }

        List<List<string>> get_table_lines_list(string sqlQuery, SqlConnection connection)
        {
            List<List<string>> all_lines = new List<List<string>>();
            using (var cmd = new SqlCommand(sqlQuery, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.ToString());
                            List<string> one_line = new List<string>();

                            for (int pos = 0; reader.FieldCount > pos; pos++)
                            {
                                one_line.Add(reader[pos].ToString());
                            }
                            all_lines.Add(one_line);
                        }

                    }

                }
            }
            return (all_lines);
        }

        public Response LGTQuery(string queryId, Dictionary<string, string> queryParams, int tenant)
        {
            var response = new Response();
            try
            {
                bool from_global = false;
                bool convert_bool = false;
                if (queryParams.ContainsKey("from_global"))
                {
                    bool.TryParse(queryParams["from_global"], out from_global);
                    queryParams.Remove("from_global");
                }
                if (queryParams.ContainsKey("convert_bool"))
                {
                    bool.TryParse(queryParams["convert_bool"], out convert_bool);
                    queryParams.Remove("convert_bool");
                }

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant1 = 0;
                    if (authToken != null) tenant1 = authToken.Tenant;
                }

                if (tenant == 0)
                {
                    response.HasError = true;
                    response.ErrorMessage = "Tenant parameter is missing.";
                    return (response);
                }

                if (queryId == "CFIRDEC")
                {
                    response = GetDataCFIRDEC(queryParams, tenant);
                    return (response);
                }
                CFILOGIAPI sql_logi = new CFILOGIAPI();
                List<CFILOGIAPI> logi_list = new List<CFILOGIAPI>();
                logi_list = CFILOGIAPITask.GetLogiOcc();
                if (queryId == "EXTERNAL_LOGIAPI")
                {
                    sql_logi = JsonConvert.DeserializeObject<CFILOGIAPI>(queryParams["CFILOGIAPI"]);
                }
                else
                {
                    sql_logi = logi_list.Where(x => x.CODE == queryId).FirstOrDefault();
                }
                if (sql_logi == null)
                {
                    response.HasError = true;
                    response.ErrorMessage = $"Query id {queryId} not found.";
                    return (response);
                }

                string sqlQuery = sql_logi.TEMPLATE_SQL;
                if (string.IsNullOrEmpty(sqlQuery))
                {
                    response.HasError = true;
                    response.ErrorMessage = $"Query id {queryId} has no sql.";
                    return (response);
                }
                if (sql_logi.HAS_TENANT && sqlQuery.IndexOf("@Tenant") == -1)
                {
                    response.HasError = true;
                    response.ErrorMessage = $"Query id {queryId} has no @Tenant parameter.";
                    return (response);
                }

                String remark = "";

                Dictionary<string, string> results = new Dictionary<string, string>();
                List<List<string>> all_lines = new List<List<string>>();
                int rows_effected = 0;
                var shipmentsContext = new Simplog.Data.ShipmentsModel.ShipmentsContext();
                var GlobalContext = new Simplog.Global.Data.GlobalModel.GlobalContext();
                if (sqlQuery.IndexOf("@NEXTNUM") > -1)
                {
                    sqlQuery = sqlQuery.Replace("@NEXTNUM", queryParams["NEXTNUM"]);
                    sqlQuery = sqlQuery.Replace("@OFFSETNUM", queryParams["OFFSETNUM"]);
                }
                if (sqlQuery.IndexOf("@CLOSE_TABLE") > -1)
                {
                    sqlQuery = sqlQuery.Replace("@CLOSE_TABLE", queryParams["CLOSE_TABLE"]);
                }
                using (SqlConnection connection = new SqlConnection())
                {
                    if (from_global)
                    {
                        connection.ConnectionString = GlobalContext.Database.Connection.ConnectionString;
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug("global db : " + connection.ConnectionString);

                        remark = "GlobalContext" + connection.ConnectionString;
                    }
                    else
                    {
                        GlobalDB currentDb = GlobalDbHelper.GetGlobalDB(tenant);
                        string DBConnection = currentDb.DBConnection;
                        string[] sourceConnectionArray = DBConnection.Split(',');
                        ConnectionStringArguments sourceConnectionStringArguments = GetConnectionStringArguments(sourceConnectionArray);
                        string ConnectionString = BuildConnectionString(sourceConnectionStringArguments);

                        connection.ConnectionString = ConnectionString;
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug("GlobalDB : " + connection.ConnectionString);
                        remark = "GlobalDB tenant=" + tenant.ToString() + " " + connection.ConnectionString;
                    }

                    connection.Open();
                    using (var cmd = new SqlCommand(sqlQuery, connection))
                    {
                        foreach (var field in queryParams)
                        {

                            cmd.Parameters.Add(sql_logi.get_SqlParameter(field.Key, field.Value));
                        }
                        if (sql_logi.HAS_TENANT)
                        {
                            if (tenant > 0)
                            {
                                cmd.Parameters.Add(new SqlParameter
                                {
                                    ParameterName = "@Tenant",
                                    SqlDbType = SqlDbType.Int,
                                    SqlValue = tenant
                                });
                            }
                            else
                            {
                                // Log the invalid tenant or handle the error appropriately
                                throw new ArgumentException("Invalid tenant ID: " + tenant);
                            }
                        }

                        if (sql_logi.IS_INSERT)
                        {
                            rows_effected = cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {


                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine(reader.ToString());
                                        List<string> one_line = new List<string>();

                                        for (int pos = 0; reader.FieldCount > pos; pos++)
                                        {
                                            if (convert_bool)
                                            {
                                                object val = reader.IsDBNull(pos) ? null : reader.GetValue(pos);

                                                // Convert BIT/boolean to "1"/"0"
                                                if (val is bool b)
                                                {
                                                    one_line.Add(b ? "1" : "0");
                                                }
                                                else
                                                {
                                                    // Keep everything else as string (null -> empty)
                                                    one_line.Add(val?.ToString() ?? string.Empty);
                                                }
                                            }
                                            else
                                            {
                                                one_line.Add(reader[pos].ToString());
                                            }
                                        }
                                        all_lines.Add(one_line);
                                    }
                                }

                            }
                        }
                        connection.Close();
                        response.HasError = false;
                        results.Add("sql_result", JsonConvert.SerializeObject(all_lines));
                        results.Add("sql_query", sqlQuery);
                        results.Add("rows_effected", rows_effected.ToString());
                        results.Add("remark", remark);
                        response.Result = JsonConvert.SerializeObject(results);
                    }
                }
                return (response);
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return (response);
            }

        }
        public string BuildConnectionString(ConnectionStringArguments connectionStringArguments)
        {
            string result = "Data Source=" + connectionStringArguments.Server +
                            ";Initial Catalog=" + connectionStringArguments.Catalog +
                            ";Integrated Security=False;Persist Security Info=True;User ID=" + connectionStringArguments.UserName +
                            ";Password= " + connectionStringArguments.Password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }
        private ConnectionStringArguments GetConnectionStringArguments(string[] connectionArray)
        {
            ConnectionStringArguments connectionStringArguments = new ConnectionStringArguments()
            {
                Catalog = connectionArray[0],
                UserName = connectionArray[1],
                Password = connectionArray[2],
                Server = connectionArray[3],
            };

            return connectionStringArguments;
        }
        public Response LGTQueryExample(string queryId, Dictionary<string, string> queryParams, int tenant)
        {
            var response = new Response();
            try
            {

                tenant = 6;
                string id = "1-110456";
                var shipmentsContext = new Simplog.Data.ShipmentsModel.ShipmentsContext();
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = shipmentsContext.Database.Connection.ConnectionString;
                    connection.Open();
                    string sqlQuery = "SELECT IMPORTERID,ID from Customs.DECLARATIONS where (ID = @LOGITUDE_FILE ) AND TENANT = @Tenant";
                    using (var cmd = new SqlCommand(sqlQuery, connection))
                    {
                        cmd.Parameters.Add(new SqlParameter("@LOGITUDE_FILE", id));
                        cmd.Parameters.Add(new SqlParameter("@Tenant", tenant));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.ToString());
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }

        }


        public Response LGTQueryOld(string queryId, Dictionary<string, string> queryParams, int tenant)
        {
            Response res = new Response();

            res.HasError = false;
            res.Result = "big data";

            return (res);
        }
        public string GetTaskFromQueue(int tenant, int priority)
        {
            Envelope envelope = new Envelope();
            CommunicationLog commLog = null;
            QueueResponse queueResponse = null;
			string result = null;
			try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }



                string enableQueueWaitOnExternalWCFService = System.Configuration.ConfigurationManager.AppSettings.Get("EnableQueueWaitOnExternalWCFService");
                TimeSpan queueWaitTime = new TimeSpan(0, 0, 0);
                if (enableQueueWaitOnExternalWCFService == "true")
                {
                    queueWaitTime = new TimeSpan(0, 0, 20);
                }
                if (LogitudeSettings.IsCostomsDeploy)
                {
                    queueWaitTime = TimeSpan.FromMinutes(5);
                }
                string queueName = "externaltasksqueue" + tenant + priority;
                DbQueueService queueservice = new DbQueueService(queueName, tenant);//QueueServiceManager.GetQueueService(queueName, 0);
                queueResponse = queueservice.Receive(queueWaitTime);

 
                if (queueResponse.MessageId != null)
                {
                    string communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
                    int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                    commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    if (commLog.CommunicationStatusTypeCode == "D")
                    {
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "D", "Message removed from queue (Communication log status = Done) " + DateTime.Now.ToString(), null);
                        queueservice.Complete();
                    }
                    else if (queueResponse.RetryNumber >= 4)
                    {
                        queueservice.CompleteAsFailed();
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "F", "queue message exceeded 5 retries" + DateTime.Now.ToString(), null);
                    }

                   else {
                        Document document = documentRepository.GetSingleDocument(tenant, commLog.DocumentId);
                        Uploader uploader = new Uploader();

                        byte[] filedata = uploader.DownloadFile(document.Id, document.Extension, document.Folder, tenant);
                        if (filedata != null)
                        {
                            XmlDocument doc = new XmlDocument();
                            MemoryStream ms = new MemoryStream(filedata);
                            doc.Load(ms);
                            //result = doc.InnerXml;

                            List<QueueTask> taskslist = LogitudeXmlSerializer.DeserializeObject<List<QueueTask>>(doc.InnerXml);
                            envelope.CommunicationLogId = communicationLogId;

                            envelope.Tasks = taskslist;
                        }
                        else
                        {
                            envelope.HasError = true;
                            envelope.ErrorMessage = "File Not found";
                            Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "F", "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), envelope.ErrorMessage);
                            queueservice.CompleteAsFailed();

                        }


                        result = LogitudeXmlSerializer.SerializeObjectToXmlString(envelope);

                    }

                    commLog.MessageLockId = queueResponse.MessageId;
                    communicationLogRep.Update(commLog);
                    communicationLogRep.SubmitChanges();
                }

                return result;

            }
            catch (Exception ex)
            {

                envelope.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                envelope.HasError = true;
                envelope.ErrorMessage = ex.Message;
                envelope.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    envelope.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                if (queueResponse != null && commLog != null)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, commLog.CommunicationStatusTypeCode, "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), envelope.ErrorMessage);

                }

				result = LogitudeXmlSerializer.SerializeObjectToXmlString(envelope);
				return result;

            }
        }

        public Response MarkTaskAsDone(string communicationLogId, int tenant, int priority)
        {
            Response response = new Response();
            CommunicationLog commLog = null;
            try
            {

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }
                //HttpContext.Current.Items.Add("workerrolename", "production");
                //QueueClient client = Communications.GetQueueClient("externaltasksqueue" + tenant + priority);
                string queueName = "externaltasksqueue" + tenant + priority;
                DbQueueService queueservice = new DbQueueService(queueName, tenant);

                if (!string.IsNullOrEmpty(communicationLogId))
                {
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                    commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    //commLog.CommunicationStatusTypeCode = "D";

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, "D", "Start of mark as done " + DateTime.Now.ToString(), null);

                    //Guid lockToken = new Guid(commLog.MessageLockId);
                    //client.Complete(lockToken);
                    queueservice.Complete(commLog.MessageLockId);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, "D", "End of mark as done " + DateTime.Now.ToString(), null);

                    //commoncontext.SaveChanges();
                }
                else
                {
                    response.HasError = false;
                    response.ErrorMessage = "Invalid communicationLogId";

                }


            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                if (commLog != null)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, commLog.CommunicationStatusTypeCode, "Exception occured while marking the queue message as done " + DateTime.Now.ToString(), response.ErrorMessage);

                }


                return response;
            }

            return response;
        }

#else

        public string GetTaskByQueueDefinitionCode(int tenant, int priority, string queueDefinitionCode)
        {
            if (string.IsNullOrWhiteSpace(queueDefinitionCode))
            {
                queueDefinitionCode = "externaltasksqueue";
            }
            Envelope envelope = new Envelope();
            CommunicationLog commLog = null;
            QueueResponse queueResponse = null;
            string result = null;
            //BrokeredMessage message = null;
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);

                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }


                //HttpContext.Current.Items.Add("workerrolename", "production");

                string enableQueueWaitOnExternalWCFService = System.Configuration.ConfigurationManager.AppSettings.Get("EnableQueueWaitOnExternalWCFService");
                TimeSpan queueWaitTime = new TimeSpan(0, 0, 0);
                if (enableQueueWaitOnExternalWCFService == "true")
                {
                    queueWaitTime = new TimeSpan(0, 0, 20);
                }

                string queueName = queueDefinitionCode/*"externaltasksqueue"*/ + tenant + priority;
                DbQueueService queueservice = new DbQueueService(queueName, tenant);//QueueServiceManager.GetQueueService(queueName, 0);
                queueResponse = queueservice.Receive(queueWaitTime);

                // QueueClient client = Communications.GetQueueClient("externaltasksqueue" + tenant + priority);

                //message = client.Receive(new TimeSpan(0, 0, 20));
                if (queueResponse.MessageId != null)
                {
                    string communicationLogId = queueResponse.MessageValues["CommunicationLogId"].ToString();
                    int.TryParse(queueResponse.MessageValues["Tenant"].ToString(), out tenant);
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    DocumentRepository documentRepository = new DocumentRepository(commoncontext);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                    commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    if (commLog.CommunicationStatusTypeCode == "D")
                    {
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "D", "Message removed from queue (Communication log status = Done) " + DateTime.Now.ToString(), null);
                        queueservice.Complete();
                    }
                    else if (queueResponse.RetryNumber >= 4)
                    {
                        queueservice.CompleteAsFailed();
                        Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "F", "queue message exceeded 5 retries" + DateTime.Now.ToString(), null);
                    }

                    else
                    {
                        Document document = documentRepository.GetSingleDocument(tenant, commLog.DocumentId);
                        Uploader uploader = new Uploader();

                        byte[] filedata = uploader.DownloadFile(document.Id, document.Extension, document.Folder, tenant);
                        if (filedata != null)
                        {
                            XmlDocument doc = new XmlDocument();
                            MemoryStream ms = new MemoryStream(filedata);
                            doc.Load(ms);
                            //result = doc.InnerXml;

                            List<QueueTask> taskslist = LogitudeXmlSerializer.DeserializeObject<List<QueueTask>>(doc.InnerXml);
                            envelope.CommunicationLogId = communicationLogId;

                            envelope.Tasks = taskslist;
                        }
                        else
                        {
                            envelope.HasError = true;
                            envelope.ErrorMessage = "File Not found";
                            Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, "F", "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), envelope.ErrorMessage);
                            queueservice.CompleteAsFailed();

                        }
                        //envelope.Result = message.LockToken.ToString() + "," + communicationLogId;


                        result = LogitudeXmlSerializer.SerializeObjectToXmlString(envelope);

                    }

                    commLog.MessageLockId = queueResponse.MessageId;
                    communicationLogRep.Update(commLog);
                    communicationLogRep.SubmitChanges();
                }

                return result;

            }
            catch (Exception ex)
            {

                envelope.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                envelope.HasError = true;
                envelope.ErrorMessage = ex.Message;
                envelope.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    envelope.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                if (queueResponse != null && commLog != null)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, queueResponse.MessageId, commLog.CommunicationStatusTypeCode, "Exception occured while proccessing the queue message " + DateTime.Now.ToString(), envelope.ErrorMessage);

                }

                result = LogitudeXmlSerializer.SerializeObjectToXmlString(envelope);
                return result;

            }
        }
        public Response MarkTaskAsDoneByQueueDefinitionCode(string communicationLogId, int tenant, int priority, string queueDefinitionCode)
        {
            if (string.IsNullOrWhiteSpace(queueDefinitionCode))
            {
                queueDefinitionCode = "externaltasksqueue";
            }
            Response response = new Response();
            CommunicationLog commLog = null;
            try
            {

                SecurityUtility.AuthenticationOnTenant(tenant);

                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }
                //HttpContext.Current.Items.Add("workerrolename", "production");
                //QueueClient client = Communications.GetQueueClient("externaltasksqueue" + tenant + priority);
                string queueName = queueDefinitionCode/*"externaltasksqueue"*/ + tenant + priority;
                DbQueueService queueservice = new DbQueueService(queueName, tenant);

                if (!string.IsNullOrEmpty(communicationLogId))
                {
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);
                    commLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
                    //commLog.CommunicationStatusTypeCode = "D";

                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, "D", "Start of mark as done " + DateTime.Now.ToString(), null);

                    //Guid lockToken = new Guid(commLog.MessageLockId);
                    //client.Complete(lockToken);
                    queueservice.Complete(commLog.MessageLockId);
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, "D", "End of mark as done " + DateTime.Now.ToString(), null);

                    //commoncontext.SaveChanges();
                }
                else
                {
                    response.HasError = false;
                    response.ErrorMessage = "Invalid communicationLogId";

                }


            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                if (commLog != null)
                {
                    Communications.UpdateCommunicationLogStatus(commLog.Id, tenant, commLog.MessageLockId, commLog.CommunicationStatusTypeCode, "Exception occured while marking the queue message as done " + DateTime.Now.ToString(), response.ErrorMessage);

                }


                return response;
            }

            return response;
        }

        public string GetTaskFromQueue(int tenant, int priority)
        {
            return GetTaskByQueueDefinitionCode(tenant, priority, "externaltasksqueue");
        }
        
        public Response MarkTaskAsDone(string communicationLogId, int tenant, int priority)
        {
            return MarkTaskAsDoneByQueueDefinitionCode(communicationLogId, tenant, priority, "externaltasksqueue");
        }

        

#endif
    }
}

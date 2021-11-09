using Devart.Data.Oracle;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.Server.Tools.QueueService
{
    public partial class DbQueueService : IQueueService
    {
        protected int Tenant { get; set; }
        protected string QueueCode { get; set; }
        protected string CurrentMessageId { get; set; }
 
        public DbQueueService()
        {

        }

        public DbQueueService(string queueCode, int tenant)
        {
            this.InitializeQueue(queueCode, tenant);
        }
        public void InitializeQueue(string queueCode, int tenant)
        {
            if (!String.IsNullOrWhiteSpace(LogitudeSettings.DebugKey))
            {
                queueCode += @"\" + LogitudeSettings.DebugKey;
            }
            else
            {

            }
            this.Tenant = tenant;
            this.QueueCode = queueCode;
            QueueMessageRepository messagesRepository = new QueueMessageRepository(Tenant);
            QueueMessageMoreDetailsRepository messagesMoreDetailsRepository = new QueueMessageMoreDetailsRepository(Tenant);
            QueueDefinitionRepository queueDefRep = new QueueDefinitionRepository(tenant);
            QueueDefinition queueDefinition = GetQueueDefFromCache(queueCode, queueDefRep);
            if (queueDefinition == null)
            {
                queueDefinition = new QueueDefinition() { Code = queueCode, Name = queueCode };
                queueDefRep.Add(queueDefinition);
                queueDefRep.SubmitChanges();
            }

            
        }

        private static QueueDefinition GetQueueDefFromCache(string queueCode, QueueDefinitionRepository queueDefRep)
        {
            string key = $"GetQueueDefFromCache({queueCode})";
            var def = CacheManager.GetOrInsertNewObject<QueueDefinition>(key, () =>
            {
                return queueDefRep.GetSingleQueueDefinition(queueCode);
            });
            return def;
        }

        public void Send(Dictionary<string, string> messageValues, int tenant, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null)
        {
            SendReturnId(messageValues, tenant, delayTime, CustomerId, BatchNumber, NextRunDate);
        }
        protected int? SendReturnId(Dictionary<string, string> messageValues, 
            int tenant, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null,int tenantPriority = 7)
        {
            if (tenantPriority<1)
            {
                tenantPriority = 7;
            }
            int? queueMessageId = null;
            if (LogitudeSettings.IsCostomsDeploy) //ITZIK + YARON 
            {
                if (this.QueueCode == "ImportersShipmentDocumentsQueue")
                {
                    // i was try to avoid in Up Code - but Failed -
                    // due that unfourtuntlly - i finally Eliminated here !!
                    return null;
                }
            }
            if (delayTime!=null)
            {
                LogMessagingUtil.Instance.AppendLine($"Delay  {this.QueueCode} Queue {delayTime.GetValueOrDefault()}");
            }
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                string messageBody = DictionaryJsonConverter.FromDictionaryToJson(messageValues);
                string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                string bodyHashCode = MD5HashUtil.GenerateHashForString(messageBody);
                
                QueueResponse response = new QueueResponse();
                DataTable tblQueue = new DataTable();//
                int delaySeconds = 0;
                string CId = "";
                string BNo = "";
                if (delayTime != null)
                {
                    delaySeconds = (int)delayTime.Value.TotalSeconds;
                }
                else
                {
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {
                        if (NextRunDate.HasValue)
                        {
                            if (DateTime.UtcNow > NextRunDate)
                            {
                                delaySeconds = 0;

                            }
                            else
                            {
                                var ts = NextRunDate.Value.Subtract(DateTime.UtcNow);
                                delaySeconds = (int)ts.TotalSeconds;
                            }
                            NextRunDate = null;
                        }
                    }

                }
                if (CustomerId != null)
                {
                    CId = CustomerId;
                }
                if (BatchNumber != null)
                {
                    BNo = BatchNumber;
                }
                if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                {
                    if (NextRunDate.HasValue)
                    {
                        throw new Exception("Opps  in oracle use by delayInSec Params ");
                    }
                    using (OracleConnection cn = new OracleConnection(strConnString))
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = cn;
                        cmd.CommandText =
                            DbContextBaseUtil.GetStoredProcedureName("Queue_Enqueue", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                        //DbContextBaseUtil.GetStoredProcedureName("TSTQueue_Enqueue", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                        cmd.CommandType = CommandType.StoredProcedure;


                        OracleParameter queueCodePar = new OracleParameter("QueueDefinitionCode", OracleDbType.VarChar, 255);
                        OracleParameter msgBodyPar = new OracleParameter("MessageBody", OracleDbType.VarChar, 1000);
                        OracleParameter tenantPar = new OracleParameter("Tenant", OracleDbType.Number);
                        OracleParameter delayPar = new OracleParameter("DelaySeconds", OracleDbType.Number);
                        OracleParameter customerId = new OracleParameter("CustomerId", OracleDbType.VarChar, 15);
                        OracleParameter batchNumber = new OracleParameter("BatchNumber", OracleDbType.VarChar, 15);
                        OracleParameter hashCodePar = new OracleParameter("HashCode", OracleDbType.NVarChar, 1000);
                        //OracleParameter NextRunDateTime = new OracleParameter("NextRunDate", OracleDbType.Date);

                        OracleParameter queueMessageIdPar = new OracleParameter("v_QueueMessageId", OracleDbType.Number);
                        queueMessageIdPar.Direction = ParameterDirection.Output;

                        OracleParameter watingStatusPar = new OracleParameter("v_WatingStatus", OracleDbType.Number);
                        watingStatusPar.Direction = ParameterDirection.Input;

                        OracleParameter tenantPriPar = new OracleParameter("p_TenantPriority ", OracleDbType.Number);
                        tenantPriPar.Direction = ParameterDirection.Input;

                        queueCodePar.Direction = ParameterDirection.Input;
                        msgBodyPar.Direction = ParameterDirection.Input;
                        tenantPar.Direction = ParameterDirection.Input;
                        delayPar.Direction = ParameterDirection.Input;
                        customerId.Direction = ParameterDirection.Input;
                        batchNumber.Direction = ParameterDirection.Input;
                        hashCodePar.Direction = ParameterDirection.Input;
                        //NextRunDateTime.Direction = ParameterDirection.Input;

                        queueCodePar.Value = this.QueueCode;
                        msgBodyPar.Value = messageBody;
                        tenantPar.Value = tenant;
                        delayPar.Value = delaySeconds;
                        customerId.Value = CId;
                        batchNumber.Value = BNo;
                        watingStatusPar.Value = WorkerNameService.GetWorkerWaitingStatusForSending(tenant);
                        //NextRunDateTime.Value = NextRunDate;
                        hashCodePar.Value = bodyHashCode;
                        tenantPriPar.Value = tenantPriority;
                        cmd.Parameters.Add(queueCodePar);
                        cmd.Parameters.Add(msgBodyPar);
                        cmd.Parameters.Add(tenantPar);
                        cmd.Parameters.Add(delayPar);
                        cmd.Parameters.Add(customerId);
                        cmd.Parameters.Add(batchNumber);
                        cmd.Parameters.Add(hashCodePar);
                        cmd.Parameters.Add(watingStatusPar);
                        cmd.Parameters.Add(tenantPriPar);
                        cmd.Parameters.Add(queueMessageIdPar);
                        



                        //cmd.Parameters.Add(NextRunDateTime);

                        try
                        {
                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();
                            var v_QueueMessageId = cmd.Parameters["v_QueueMessageId"].Value;
                            if (v_QueueMessageId != null)
                            {
                                string sQueueMessageId = v_QueueMessageId.ToString();
                                if (!String.IsNullOrWhiteSpace(sQueueMessageId))
                                {
                                    queueMessageId = sQueueMessageId.ChangeValue<int>();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Exception: {0}", ex.ToString());
                            throw;
                        }

                        cn.Close();
                    }


                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(strConnString))
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[Queue_Enqueue]", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter queueCodePar = new SqlParameter("@QueueDefinitionCode", SqlDbType.VarChar, 255);
                        SqlParameter msgBodyPar = new SqlParameter("@MessageBody", SqlDbType.VarChar, 1000);
                        SqlParameter tenantPar = new SqlParameter("@Tenant", SqlDbType.Int);
                        SqlParameter delayPar = new SqlParameter("@DelaySeconds", SqlDbType.Int);
                        SqlParameter customerId = new SqlParameter("@CustomerId", SqlDbType.VarChar, 15);
                        SqlParameter batchNumber = new SqlParameter("@BatchNumber", SqlDbType.VarChar, 15);
                        SqlParameter NextRunDateTime = new SqlParameter("@NextRunDTime", SqlDbType.DateTime);
                        SqlParameter hashCodePar = new SqlParameter("@HashCode", SqlDbType.NVarChar, 1000);
                        SqlParameter watingStatusPar = new SqlParameter("@WatingStatus", SqlDbType.Int);
                        SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                        

                        queueCodePar.Direction = ParameterDirection.Input;
                        msgBodyPar.Direction = ParameterDirection.Input;
                        tenantPar.Direction = ParameterDirection.Input;
                        delayPar.Direction = ParameterDirection.Input;
                        customerId.Direction = ParameterDirection.Input;
                        batchNumber.Direction = ParameterDirection.Input;
                        NextRunDateTime.Direction = ParameterDirection.Input;
                        hashCodePar.Direction = ParameterDirection.Input;
                        watingStatusPar.Direction = ParameterDirection.Input;
                        messageIdPar.Direction = ParameterDirection.Output;

                        queueCodePar.Value = this.QueueCode;
                        msgBodyPar.Value = messageBody;
                        tenantPar.Value = tenant;
                        delayPar.Value = delaySeconds;
                        customerId.Value = CId;
                        batchNumber.Value = BNo;
                        NextRunDateTime.Value = NextRunDate;
                        hashCodePar.Value = bodyHashCode;
                        watingStatusPar.Value = WorkerNameService.GetWorkerWaitingStatusForSending(tenant);

                        cmd.Parameters.Add(queueCodePar);
                        cmd.Parameters.Add(msgBodyPar);
                        cmd.Parameters.Add(tenantPar);
                        cmd.Parameters.Add(delayPar);
                        cmd.Parameters.Add(customerId);
                        cmd.Parameters.Add(batchNumber);
                        cmd.Parameters.Add(NextRunDateTime);
                        cmd.Parameters.Add(hashCodePar);
                        cmd.Parameters.Add(watingStatusPar);
                        cmd.Parameters.Add(messageIdPar);

                        cn.Open();
                        var output = cmd.ExecuteNonQuery();
                        cn.Close();

                        var v_QueueMessageId = cmd.Parameters["@MessageId"].Value;
                        if (v_QueueMessageId != null)
                        {
                            string sQueueMessageId = v_QueueMessageId.ToString();
                            if (!String.IsNullOrWhiteSpace(sQueueMessageId))
                            {
                                queueMessageId = sQueueMessageId.ChangeValue<int>();
                                AddQueueDetailsToRequestHeaders(messageBody, sQueueMessageId);
                            }
                        }

                    }
                }

                scope.Complete();
            }



            return queueMessageId;
        }

        private static void AddQueueDetailsToRequestHeaders(string messageBody, string sQueueMessageId)
        { 
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                if (HttpContext.Current.Response.Headers["SentQueueMessages"] == null)
                {
                    Dictionary<string, string> dictionary = new Dictionary<string, string>
                    {
                        { sQueueMessageId, messageBody }
                    };
                    string addedQueues = dictionary.FromDictionaryToJson();
                    HttpContext.Current.Response.Headers.Add("SentQueueMessages", addedQueues);

                }
                else
                {
                    string openedQueues = HttpContext.Current.Response.Headers["SentQueueMessages"];
                    Dictionary<string, string> dictionary = openedQueues.FromJsonToDictionary();
                    dictionary.Add(sQueueMessageId, messageBody);
                    string addedQueues = dictionary.FromDictionaryToJson();
                    HttpContext.Current.Response.Headers["SentQueueMessages"] = addedQueues;


                }
            }
        }

        public QueueResponse Receive(TimeSpan? serverWaitTime = null)
        {
            if (serverWaitTime == null) { serverWaitTime = TimeSpan.FromSeconds(5); }

            long messageId = -1;

            string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
            QueueResponse response = new QueueResponse();
            if (string.IsNullOrEmpty(this.CurrentMessageId))
            {
                DataTable tblQueue = new DataTable();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_Peek", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;

                            OracleParameter messageIdPar = new OracleParameter("v_MessageId", OracleDbType.Number);
                            OracleParameter nextRunDelayInSecPar = new OracleParameter("v_NextRunDelayInSec", OracleDbType.Number);

                            OracleParameter queueCodePar = new OracleParameter("v_QueueDefinitionCode", OracleDbType.VarChar, 255);
                            OracleParameter messageBodyPar = new OracleParameter("v_MessageBody", OracleDbType.VarChar, 1000);
                            OracleParameter retryNumberPar = new OracleParameter("v_RetryNumber", OracleDbType.Number);
                            OracleParameter messageCreatedServerTimePar = new OracleParameter("v_MessageCreatedServerTime", OracleDbType.Date);
                            OracleParameter watingStatusPar = new OracleParameter("v_WatingStatus", OracleDbType.Number);

                            queueCodePar.Direction = ParameterDirection.Input;
                            nextRunDelayInSecPar.Direction = ParameterDirection.Input;

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            retryNumberPar.Direction = ParameterDirection.Output;
                            messageCreatedServerTimePar.Direction = ParameterDirection.Output;
                            watingStatusPar.Direction = ParameterDirection.Input;

                            watingStatusPar.Value = WorkerNameService.GetWorkerWaitingStatusForReceiving(this.Tenant);

                            queueCodePar.Value = QueueCode;
                            nextRunDelayInSecPar.Value = serverWaitTime.Value.Milliseconds;
                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(messageCreatedServerTimePar);
                            cmd.Parameters.Add(queueCodePar);
                            cmd.Parameters.Add(nextRunDelayInSecPar);
                            cmd.Parameters.Add(watingStatusPar);



                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();

                                object messageOb = cmd.Parameters["v_MessageId"].Value;
                                if (messageOb != null)
                                {

                                    if (long.TryParse(cmd.Parameters["v_MessageId"].Value.ToString(), out messageId))
                                    {
                                        this.CurrentMessageId = response.MessageId = messageId.ToString();
                                        response.RetryNumber = Convert.ToInt32(cmd.Parameters["v_RetryNumber"].Value);
                                        response.MessageCreatedServerTime = (DateTime)cmd.Parameters["v_MessageCreatedServerTime"].Value;
                                        string messageBody = cmd.Parameters["v_MessageBody"].Value as string;
                                        if (!string.IsNullOrEmpty(messageBody))
                                        {
                                            Dictionary<string, string> messageValues = DictionaryJsonConverter.FromJsonToDictionary(messageBody);
                                            response.MessageValues = messageValues;
                                        }

                                        RunDebuggerBreak();
                                    }

                                    
                                }

                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_Peek]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            SqlParameter queueCodePar = new SqlParameter("@QueueDefinitionCode", SqlDbType.NVarChar, 255);
                            SqlParameter messageBodyPar = new SqlParameter("@MessageBody", SqlDbType.VarChar, 1000);
                            SqlParameter retryNumberPar = new SqlParameter("@RetryNumber", SqlDbType.Int);
                            SqlParameter watingStatusPar = new SqlParameter("@WatingStatus", SqlDbType.Int);

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            queueCodePar.Direction = ParameterDirection.Input;
                            retryNumberPar.Direction = ParameterDirection.Output;
                            watingStatusPar.Direction = ParameterDirection.Input;

                            queueCodePar.Value = QueueCode;
                            watingStatusPar.Value = WorkerNameService.GetWorkerWaitingStatusForReceiving(this.Tenant);

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(queueCodePar);
                            cmd.Parameters.Add(watingStatusPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();

                            object messageOb = cmd.Parameters["@MessageId"].Value;
                            if (messageOb != null)
                            {

                                if (long.TryParse(cmd.Parameters["@MessageId"].Value.ToString(), out messageId))
                                {
                                    this.CurrentMessageId = response.MessageId = messageId.ToString();
                                    response.RetryNumber = (int)cmd.Parameters["@RetryNumber"].Value;
                                    string messageBody = cmd.Parameters["@MessageBody"].Value as string;
                                    if (!string.IsNullOrEmpty(messageBody))
                                    {
                                        Dictionary<string, string> messageValues = DictionaryJsonConverter.FromJsonToDictionary(messageBody);
                                        response.MessageValues = messageValues;
                                    }

                                    RunDebuggerBreak();
                                }

                                
                            }

                        }
                    }

                    scope.Complete();
                }


            }

            if (string.IsNullOrEmpty(response.MessageId))
            {
                Thread.Sleep(serverWaitTime.Value);
            }

            return response;
        }

        private static void RunDebuggerBreak()
        {
            if (Debugger.IsAttached && LogitudeSettings.RunWorkerRoleAutomaticBreakPoint)
                Debugger.Break();
        }

        public QueueResponse Receive()
        {
            int nextRunDelayInSec = 60;
            return Receive(nextRunDelayInSec);
        }
        public QueueResponse Receive(int nextRunDelayInSec = 60, TimeSpan? serverWaitTime = null)
        {
            if (serverWaitTime == null) { serverWaitTime = TimeSpan.FromSeconds(5); }
            long messageId = -1;

            string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
            QueueResponse response = new QueueResponse();
            if (string.IsNullOrEmpty(this.CurrentMessageId))
            {
                DataTable tblQueue = new DataTable();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_Peek", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;

                            OracleParameter messageIdPar = new OracleParameter("v_MessageId", OracleDbType.Number);
                            OracleParameter nextRunDelayInSecPar = new OracleParameter("v_NextRunDelayInSec", OracleDbType.Number);

                            OracleParameter queueCodePar = new OracleParameter("v_QueueDefinitionCode", OracleDbType.VarChar, 255);
                            OracleParameter messageBodyPar = new OracleParameter("v_MessageBody", OracleDbType.VarChar, 1000);
                            OracleParameter retryNumberPar = new OracleParameter("v_RetryNumber", OracleDbType.Number);
                            OracleParameter messageCreatedServerTimePar = new OracleParameter("v_MessageCreatedServerTime", OracleDbType.Date);
                            OracleParameter watingStatusPar = new OracleParameter("v_WatingStatus", OracleDbType.Number);

                            queueCodePar.Direction = ParameterDirection.Input;
                            nextRunDelayInSecPar.Direction = ParameterDirection.Input;

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            retryNumberPar.Direction = ParameterDirection.Output;
                            messageCreatedServerTimePar.Direction = ParameterDirection.Output;
                            watingStatusPar.Direction = ParameterDirection.Input;
                            watingStatusPar.Value = WorkerNameService.GetWorkerWaitingStatusForReceiving(this.Tenant);

                            queueCodePar.Value = QueueCode;
                            nextRunDelayInSecPar.Value = nextRunDelayInSec;
                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(messageCreatedServerTimePar);
                            cmd.Parameters.Add(queueCodePar);
                            cmd.Parameters.Add(nextRunDelayInSecPar);
                            cmd.Parameters.Add(watingStatusPar);



                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();

                                object messageOb = cmd.Parameters["v_MessageId"].Value;
                                if (messageOb != null)
                                {

                                    if (long.TryParse(cmd.Parameters["v_MessageId"].Value.ToString(), out messageId))
                                    {
                                        this.CurrentMessageId = response.MessageId = messageId.ToString();
                                        response.RetryNumber = Convert.ToInt32(cmd.Parameters["v_RetryNumber"].Value);
                                        response.MessageCreatedServerTime = (DateTime)cmd.Parameters["v_MessageCreatedServerTime"].Value;
                                        string messageBody = cmd.Parameters["v_MessageBody"].Value as string;
                                        if (!string.IsNullOrEmpty(messageBody))
                                        {
                                            Dictionary<string, string> messageValues = DictionaryJsonConverter.FromJsonToDictionary(messageBody);
                                            response.MessageValues = messageValues;
                                        }

                                        RunDebuggerBreak();
                                    }

                                    
                                }

                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_Peek]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            SqlParameter queueCodePar = new SqlParameter("@QueueDefinitionCode", SqlDbType.NVarChar, 255);
                            SqlParameter messageBodyPar = new SqlParameter("@MessageBody", SqlDbType.VarChar, 1000);
                            SqlParameter retryNumberPar = new SqlParameter("@RetryNumber", SqlDbType.Int);
                            SqlParameter watingStatusPar = new SqlParameter("@WatingStatus", SqlDbType.Int);

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            queueCodePar.Direction = ParameterDirection.Input;
                            retryNumberPar.Direction = ParameterDirection.Output;
                            watingStatusPar.Direction = ParameterDirection.Input;

                            watingStatusPar.Value = WorkerNameService.GetWorkerWaitingStatusForReceiving(this.Tenant);
                            queueCodePar.Value = QueueCode;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(queueCodePar);
                            cmd.Parameters.Add(watingStatusPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();

                            object messageOb = cmd.Parameters["@MessageId"].Value;
                            if (messageOb != null)
                            {

                                if (long.TryParse(cmd.Parameters["@MessageId"].Value.ToString(), out messageId))
                                {
                                    this.CurrentMessageId = response.MessageId = messageId.ToString();
                                    response.RetryNumber = (int)cmd.Parameters["@RetryNumber"].Value;
                                    string messageBody = cmd.Parameters["@MessageBody"].Value as string;
                                    if (!string.IsNullOrEmpty(messageBody))
                                    {
                                        Dictionary<string, string> messageValues = DictionaryJsonConverter.FromJsonToDictionary(messageBody);
                                        response.MessageValues = messageValues;
                                    }

                                    RunDebuggerBreak();
                                }
                            }

                            

                        }
                    }

                    scope.Complete();
                }


            }

            if (string.IsNullOrEmpty(response.MessageId))
            {
                Thread.Sleep(serverWaitTime.Value);
            }

            return response;
        }


        public void Delay(TimeSpan delayTime)
        {
            if (!string.IsNullOrEmpty(this.CurrentMessageId))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    int seconds = (int)delayTime.TotalSeconds;
                    string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                    QueueResponse response = new QueueResponse();
                    DataTable tblQueue = new DataTable();

                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_DelayMessage", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;


                            OracleParameter messageIdPar = new OracleParameter("MessageId", OracleDbType.Number, 18);
                            OracleParameter delayPar = new OracleParameter("DelaySeconds", OracleDbType.Number);

                            messageIdPar.Direction = ParameterDirection.Input;
                            delayPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = this.CurrentMessageId;
                            delayPar.Value = seconds;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(delayPar);

                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_DelayMessage]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            SqlParameter delayPar = new SqlParameter("@DelaySeconds", SqlDbType.Int);


                            messageIdPar.Direction = ParameterDirection.Input;
                            delayPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = this.CurrentMessageId;
                            delayPar.Value = seconds;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(delayPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();

                            this.CurrentMessageId = null;

                        }
                    }

                    scope.Complete();
                }
            }

        }

        public void Return()
        {
            if (!string.IsNullOrEmpty(this.CurrentMessageId))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                    QueueResponse response = new QueueResponse();
                    DataTable tblQueue = new DataTable();
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_ReturnMessage", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;


                            OracleParameter messageIdPar = new OracleParameter("MessageId", OracleDbType.Number, 18);
                            messageIdPar.Direction = ParameterDirection.Input;
                            messageIdPar.Value = this.CurrentMessageId;

                            cmd.Parameters.Add(messageIdPar);

                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_ReturnMessage]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            messageIdPar.Direction = ParameterDirection.Input;
                            messageIdPar.Value = this.CurrentMessageId;

                            cmd.Parameters.Add(messageIdPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();
                            this.CurrentMessageId = null;
                        }
                    }
                    scope.Complete();
                }
            }

        }
        /// <summary>
        /// Have TransactionScope Wrapper 
        /// </summary>
        public void Complete()
        {
            if (!string.IsNullOrEmpty(this.CurrentMessageId))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                    QueueResponse response = new QueueResponse();
                    DataTable tblQueue = new DataTable();
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_SetStatus", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;


                            OracleParameter messageIdPar = new OracleParameter("MessageId", OracleDbType.Number, 18);
                            OracleParameter statusPar = new OracleParameter("Statud", OracleDbType.Number);

                            messageIdPar.Direction = ParameterDirection.Input;
                            statusPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = this.CurrentMessageId;
                            statusPar.Value = 1;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(statusPar);

                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_SetStatus]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            SqlParameter statusPar = new SqlParameter("@Statud", SqlDbType.Int);


                            messageIdPar.Direction = ParameterDirection.Input;
                            statusPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = this.CurrentMessageId;
                            statusPar.Value = 1;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(statusPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();

                            this.CurrentMessageId = null;

                        }
                    }

                    scope.Complete();
                }
            }
        }

        public void CompleteAsFailed()
        {
            if (!string.IsNullOrEmpty(this.CurrentMessageId))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                    QueueResponse response = new QueueResponse();
                    DataTable tblQueue = new DataTable();
                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_SetStatus", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;


                            OracleParameter messageIdPar = new OracleParameter("MessageId", OracleDbType.Number, 18);
                            OracleParameter statusPar = new OracleParameter("Statud", OracleDbType.Number);

                            messageIdPar.Direction = ParameterDirection.Input;
                            statusPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = this.CurrentMessageId;
                            statusPar.Value = -1;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(statusPar);

                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_SetStatus]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            SqlParameter statusPar = new SqlParameter("@Statud", SqlDbType.Int);


                            messageIdPar.Direction = ParameterDirection.Input;
                            statusPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = this.CurrentMessageId;
                            statusPar.Value = -1;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(statusPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();

                            this.CurrentMessageId = null;

                        }
                    }

                    scope.Complete();
                }
            }
        }

        public void Complete(string messageId)
        {
            this.CurrentMessageId = messageId;
            this.Complete();
        }

        public void DelayAndReturnBackToQueue(TimeSpan delayTime, string myMessageId)
        {
            if (!string.IsNullOrEmpty(myMessageId))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    int seconds = (int)delayTime.TotalSeconds;
                    string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                    QueueResponse response = new QueueResponse();
                    DataTable tblQueue = new DataTable();

                    if (LogitudeSettings.DatabaseManagementSystem == "oracle")
                    {

                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = cn;
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Q_DelayMsgandChangeStatusTo0", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;


                            OracleParameter messageIdPar = new OracleParameter("MessageId", OracleDbType.Number);
                            OracleParameter delayPar = new OracleParameter("DelaySeconds", OracleDbType.Number);

                            messageIdPar.Direction = ParameterDirection.Input;
                            delayPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = myMessageId;
                            delayPar.Value = seconds;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(delayPar);

                            try
                            {
                                cn.Open();
                                var output = cmd.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Exception: {0}", ex.ToString());
                                throw;
                            }

                            cn.Close();
                        }


                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[Queue_DelayMessageandChangeStatusTozero]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter messageIdPar = new SqlParameter("@MessageId", SqlDbType.BigInt);
                            SqlParameter delayPar = new SqlParameter("@DelaySeconds", SqlDbType.Int);


                            messageIdPar.Direction = ParameterDirection.Input;
                            delayPar.Direction = ParameterDirection.Input;

                            messageIdPar.Value = myMessageId;
                            delayPar.Value = seconds;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(delayPar);

                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();

                            this.CurrentMessageId = null;

                        }
                    }

                    scope.Complete();
                }
            }

        }



    }
}

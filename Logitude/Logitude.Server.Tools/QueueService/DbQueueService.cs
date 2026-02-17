using Devart.Data.Oracle;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

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

        public void Send(Dictionary<string, string> messageValues, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null)
        {
            SendReturnId(messageValues, delayTime, CustomerId, BatchNumber, NextRunDate);
        }
        protected int? SendReturnId(Dictionary<string, string> messageValues, TimeSpan? delayTime = null, string CustomerId = null, string BatchNumber = null, DateTime? NextRunDate = null)
        {
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
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                string messageBody = DictionaryJsonConverter.FromDictionaryToJson(messageValues);
                string strConnString = TenantServerConfigration.GetDbConnection(this.Tenant);
                QueueResponse response = new QueueResponse();
                DataTable tblQueue = new DataTable();//
                int delaySeconds = 0;
                string CId = "";
                string BNo = ""; 
                if (delayTime != null)
                {
                    delaySeconds = (int)delayTime.Value.TotalSeconds;
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
                        cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_Enqueue", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                        cmd.CommandType = CommandType.StoredProcedure;


                        OracleParameter queueCodePar = new OracleParameter("QueueDefinitionCode", OracleDbType.VarChar, 255);
                        OracleParameter msgBodyPar = new OracleParameter("MessageBody", OracleDbType.VarChar, 1000);
                        OracleParameter delayPar = new OracleParameter("DelaySeconds", OracleDbType.Number);
                        OracleParameter customerId = new OracleParameter("CustomerId", OracleDbType.VarChar, 15);
                        OracleParameter batchNumber = new OracleParameter("BatchNumber", OracleDbType.VarChar, 15);
                        //OracleParameter NextRunDateTime = new OracleParameter("NextRunDate", OracleDbType.Date);

                        OracleParameter queueMessageIdPar = new OracleParameter("v_QueueMessageId", OracleDbType.Number);
                        queueMessageIdPar.Direction = ParameterDirection.Output;

                        queueCodePar.Direction = ParameterDirection.Input;
                        msgBodyPar.Direction = ParameterDirection.Input;
                        delayPar.Direction = ParameterDirection.Input;
                        customerId.Direction = ParameterDirection.Input;
                        batchNumber.Direction = ParameterDirection.Input;
                        //NextRunDateTime.Direction = ParameterDirection.Input;

                        queueCodePar.Value = this.QueueCode;
                        msgBodyPar.Value = messageBody;
                        delayPar.Value = delaySeconds;
                        customerId.Value = CId;
                        batchNumber.Value = BNo;
                        //NextRunDateTime.Value = NextRunDate;

                        cmd.Parameters.Add(queueCodePar);
                        cmd.Parameters.Add(msgBodyPar);
                        cmd.Parameters.Add(delayPar);
                        cmd.Parameters.Add(customerId);
                        cmd.Parameters.Add(batchNumber);
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
                        SqlParameter delayPar = new SqlParameter("@DelaySeconds", SqlDbType.Int);
                        SqlParameter customerId = new SqlParameter("@CustomerId", SqlDbType.VarChar, 15);
                        SqlParameter batchNumber = new SqlParameter("@BatchNumber", SqlDbType.VarChar, 15);
                        SqlParameter NextRunDateTime = new SqlParameter("@NextRunDTime", SqlDbType.DateTime);

                        queueCodePar.Direction = ParameterDirection.Input;
                        msgBodyPar.Direction = ParameterDirection.Input;
                        delayPar.Direction = ParameterDirection.Input;
                        customerId.Direction = ParameterDirection.Input;
                        batchNumber.Direction = ParameterDirection.Input;
                        NextRunDateTime.Direction = ParameterDirection.Input;

                        queueCodePar.Value = this.QueueCode;
                        msgBodyPar.Value = messageBody;
                        delayPar.Value = delaySeconds;
                        customerId.Value = CId;
                        batchNumber.Value = BNo;
                        NextRunDateTime.Value = NextRunDate;

                        cmd.Parameters.Add(queueCodePar);
                        cmd.Parameters.Add(msgBodyPar);
                        cmd.Parameters.Add(delayPar);
                        cmd.Parameters.Add(customerId);
                        cmd.Parameters.Add(batchNumber);
                        cmd.Parameters.Add(NextRunDateTime);

                        cn.Open();
                        var output = cmd.ExecuteNonQuery();
                        cn.Close();

                    }
                }

                scope.Complete();
            }



            return queueMessageId;
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

                            
                            queueCodePar.Direction = ParameterDirection.Input;
                            nextRunDelayInSecPar.Direction = ParameterDirection.Input;

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            retryNumberPar.Direction = ParameterDirection.Output;
                            messageCreatedServerTimePar.Direction = ParameterDirection.Output;

                            queueCodePar.Value = QueueCode;
                            nextRunDelayInSecPar.Value = serverWaitTime.Value.Milliseconds;
                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(messageCreatedServerTimePar);
                            cmd.Parameters.Add(queueCodePar);
                            cmd.Parameters.Add(nextRunDelayInSecPar);


                             
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

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            queueCodePar.Direction = ParameterDirection.Input;
                            retryNumberPar.Direction = ParameterDirection.Output;

                            queueCodePar.Value = QueueCode;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(queueCodePar);

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

        public QueueResponse Receive()
        {
            int nextRunDelayInSec = 60;
            return Receive(nextRunDelayInSec);
        }
        public QueueResponse Receive(int nextRunDelayInSec = 60)
        {
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


                            queueCodePar.Direction = ParameterDirection.Input;
                            nextRunDelayInSecPar.Direction = ParameterDirection.Input;

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            retryNumberPar.Direction = ParameterDirection.Output;
                            messageCreatedServerTimePar.Direction = ParameterDirection.Output;

                            queueCodePar.Value = QueueCode;
                            nextRunDelayInSecPar.Value = nextRunDelayInSec;
                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(messageCreatedServerTimePar);
                            cmd.Parameters.Add(queueCodePar);
                            cmd.Parameters.Add(nextRunDelayInSecPar);



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

                            messageIdPar.Direction = ParameterDirection.Output;
                            messageBodyPar.Direction = ParameterDirection.Output;
                            queueCodePar.Direction = ParameterDirection.Input;
                            retryNumberPar.Direction = ParameterDirection.Output;

                            queueCodePar.Value = QueueCode;

                            cmd.Parameters.Add(messageIdPar);
                            cmd.Parameters.Add(messageBodyPar);
                            cmd.Parameters.Add(retryNumberPar);
                            cmd.Parameters.Add(queueCodePar);

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
                                }
                            }

                        }
                    }

                    scope.Complete();
                }


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

        public void DelayAndReturnBackToQueue(TimeSpan delayTime,string myMessageId)
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
                            cmd.CommandText = DbContextBaseUtil.GetStoredProcedureName("Queue_DelayMessageandChangeStatusTozero", LogitudeDBSchema.LOGITUDE_MAIN, cmd.Connection.ConnectionString);
                            cmd.CommandType = CommandType.StoredProcedure;


                            OracleParameter messageIdPar = new OracleParameter("MessageId", OracleDbType.Number, 18);
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

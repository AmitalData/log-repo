using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.BL
{
    public class CancelOldCommunicationLogs
    {
        public void CancelOldECTHRDataMaman(int Tenant, string DeclarationId)
        {
            var Subjects = new List<string>();
            Subjects.Add("ש.מ.ב לממן");
            Subjects.Add("ש.מ.ב לאוברסיז");
            Subjects.Add("ש.מ.ב לסוויספורט");
            var communicationLog = Communications.GetSingleCommunicationLogInProccess(Tenant, DeclarationId, Subjects);
            var queueMessageRepository = new QueueMessageRepository(Tenant);
            if (queueMessageRepository != null && communicationLog != null)
            {
                var que = queueMessageRepository.GetSingleQueueMessage(communicationLog.Id, "communicationlog");
                this.DeleteFromQue(que, Tenant);
            }

        }
        public void DeleteFromQue(QueueMessage que,int Tenant)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                string strConnString = TenantServerConfigration.GetDbConnection(Tenant);
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

                        messageIdPar.Value = que.Id;
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
                        SqlParameter messageIdPar = new SqlParameter("@V_MessageId", SqlDbType.BigInt);
                        SqlParameter statusPar = new SqlParameter("@V_Statud", SqlDbType.Int);


                        messageIdPar.Direction = ParameterDirection.Input;
                        statusPar.Direction = ParameterDirection.Input;

                        messageIdPar.Value = que?.Id;
                        statusPar.Value = 1;

                        cmd.Parameters.Add(messageIdPar);
                        cmd.Parameters.Add(statusPar);

                        cn.Open();
                        var output = cmd.ExecuteNonQuery();
                        cn.Close();


                    }
                }

                scope.Complete();
            }

        }

    }
}

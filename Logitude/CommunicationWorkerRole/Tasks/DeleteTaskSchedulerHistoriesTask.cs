using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.SQL;
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

namespace CommunicationWorkerRole.Tasks
{
    public class DeleteTaskSchedulerHistoriesTask : TaskManagerBase
    {
        public DeleteTaskSchedulerHistoriesTask(string Id, int tenant)
            : base(Id, tenant)
        {

        }
        public override void StartTask()
        {
            string strConnString = TenantServerConfigration.GetDbConnection(0);
            TasksSchedulerRepository TasksRepo = new TasksSchedulerRepository(0);
            List<TasksScheduler> Tasks = TasksRepo.All();

            foreach (var Task in Tasks)
            {
                int numberOfExecuteRows = 1000;
                int numberOfRecords = 0;
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        //Delete from SchedulerLogs where HistoryId in (Select Id from[dbo].[TaskSchedulerHistory] where TaskId = @TaskId and[StartDateTime] < DATEADD(month,-3, GETDATE()))
                        //delete from[dbo].[TaskSchedulerHistory] where[TaskId] = @TaskId and[StartDateTime] < DATEADD(month,-3, GETDATE())
                        string sql = "delete from SchedulerLogs where HistoryId in (Select Id from TaskSchedulerHistory where TaskId = '" + Task.Id + "' and StartDateTime < GETDATE() - 90 )";
                        //if (dbms == "oracle")
                        //{
                        //    sql = "DELETE SchedulerLogs WHERE ROWID IN  (SELECT ROWID FROM TaskSchedulerHistory where TaskId = " + Task.Id + " and StartDateTime < (SELECT SYSDATE FROM DUAL) - 90 and rownum<= 1000);";
                        //}

                        SqlCommand cmd = new SqlCommand(sql, cn);
                        cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        //numberOfRecords += numberOfExecuteRows;
                        cn.Close();
                        scope.Complete();
                    }
                }
                while (numberOfExecuteRows == 1000)
                {
                    using (SqlConnection cn = new SqlConnection(strConnString))
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            //Delete from SchedulerLogs where HistoryId in (Select Id from[dbo].[TaskSchedulerHistory] where TaskId = @TaskId and[StartDateTime] < DATEADD(month,-3, GETDATE()))
                            //delete from[dbo].[TaskSchedulerHistory] where[TaskId] = @TaskId and[StartDateTime] < DATEADD(month,-3, GETDATE())
                            string sql = "delete top(1000) from TaskSchedulerHistory where TaskId = '" + Task.Id + "' and StartDateTime < GETDATE() - 90";
                            if (dbms == "oracle")
                            {
                                sql = "DELETE FROM TaskSchedulerHistory WHERE ROWID IN  (SELECT ROWID FROM TaskSchedulerHistory where TaskId = '" + Task.Id + "' and StartDateTime < (SELECT SYSDATE FROM DUAL) - 90 and rownum<= 1000);";
                            }

                            SqlCommand cmd = new SqlCommand(sql, cn);
                            cmd.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
                            cn.Open();
                            numberOfExecuteRows = cmd.ExecuteNonQuery();
                            numberOfRecords += numberOfExecuteRows;
                            cn.Close();
                            scope.Complete();
                        }
                    }
                    Thread.Sleep(1000);

                }

                //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                //{

                //    System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                //        {
                //            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 15, ParamName = "@TaskId",Value = Task.Id },
                //        };
                //    object value = ExecuteStoredProcedures.Execute("[dbo].[DeleteTaskSchedulerHistories]", 0, paramList);
                //    scope.Complete();
                //}
                //using (SqlConnection cn = new SqlConnection(strConnString))
                //{
                //    SqlCommand cmd = new SqlCommand("[dbo].[DeleteTaskSchedulerHistories]", cn);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cn.Open();
                //    var output = cmd.ExecuteNonQuery();
                //    cn.Close();
                //}
            }
        }
    }
}

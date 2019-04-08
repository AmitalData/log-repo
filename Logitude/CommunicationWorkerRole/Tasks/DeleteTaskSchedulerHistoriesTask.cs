using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.SQL;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {

                    System.Collections.Generic.List<StoredProcedureParam> paramList = new System.Collections.Generic.List<StoredProcedureParam>()
                        {
                            new StoredProcedureParam()   { Direction = ParameterDirection.Input, ParamDBType = SqlDbType.VarChar, ParamSize = 15, ParamName = "@TaskId",Value = Task.Id },
                        };
                    object value = ExecuteStoredProcedures.Execute("[dbo].[DeleteTaskSchedulerHistories]", 0, paramList);
                    scope.Complete();
                }
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

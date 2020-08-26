using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Tasks
{
    public class GLAccountsCSVTask : TaskManagerBase
    {

        private StringBuilder _SB;
        int tenant;

        public GLAccountsCSVTask(string Id, int tenant) : base(Id, tenant)
        {
            _SB = new StringBuilder();
            this.tenant = tenant;
        }

        public override void StartTask()
        {
            bool failed = false;
            try
            {


                _SB.Append(DateTime.Now.ToString()).AppendLine("GLAccountsCSVTask:Start");

                try
                {
                    GLAccountsAgingCSVBuilder csvBuilder = new GLAccountsAgingCSVBuilder(tenant);
                    var csvString = csvBuilder.BuildAndGet();

                    GLAccountCSVData csvData = new GLAccountCSVData()
                    {
                        Value = csvString
                    };

                    CommunicationsParams comParams = CreateCommunicationParamsForGLAccount();
                    List<QueueTask> queueTasks = CreateQueueTasks(csvData);
                    comParams.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);
                    Communications.AddCommunicationLog(comParams);


                    string responseText = "CSV File Built";


                    _SB.Append(DateTime.Now.ToString()).Append("responseText:").Append(responseText).AppendLine();
                }
                catch (Exception ex)
                {
                    failed = true;
                    _SB.Append(DateTime.Now.ToString()).Append("Exception:").Append(ex.Message).AppendLine();
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "WorkerRole", $"GLAccountsCSVTask()", null);
                }

            }
            finally
            {
                if (failed)
                {
                    throw new Exception(_SB.ToString());
                }
            }
        }

        private CommunicationsParams CreateCommunicationParamsForGLAccount()
        {
            ContactPM loggedUser = LoggedContactResolver.GetLoggedContact(tenant);

            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode("GLAccount", 0);




            CommunicationsParams comParams = new CommunicationsParams()
            {
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = loggedUser?.Id,
                LoggingObjectTableId = table?.Id, 
                LoggingEntityId = null,
                Subject = "GLAccountsCSV",
                FolderName = "ExternalTasksQueue",
            };
            return comParams;
        }
        private List<QueueTask> CreateQueueTasks(GLAccountCSVData csvData)
        {

            string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(csvData);

            List<QueueTask> queue1Tasks = new List<QueueTask>
                {
                    new QueueTask()
                    {
                        Action = "GLAccountsCSV",
                        Parameters = new List<Parameter>()
                        {
                            new Parameter{ Name = "GLAccountsCSV", Order = 1, Value = xmlstring }
                        }
                    }
                };
            return queue1Tasks;
        }

    }

    public class GLAccountCSVData
    {
        public string Value { get; set; }
    }
}

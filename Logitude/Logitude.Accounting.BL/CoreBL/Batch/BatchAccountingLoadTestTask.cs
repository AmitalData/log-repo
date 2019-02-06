using Logitude.Accounting.BL.CoreBL.BuildTenant;
using Logitude.Accounting.Data;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchAccountingLoadTestTask : BatchTaskExecutionsService
    {
        public BatchAccountingLoadTestTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(BatchAccountingLoadArg));
            var parameterArgs = serializer.Deserialize(stringReader) as BatchAccountingLoadArg;
            var accountingContext = AccountingContext.GetContext(parameterArgs.Tenant);

            try
            {
                var g = new DummyTenantProvider();
                var dummyTenantProviderArg = new DummyTenantProviderArg();
                var chartOfAccountProvider = new ChartOfAccountProvider();
                var displayNumberProvider = new DisplayNumberProvider();


                switch (parameterArgs.ActionType)
                {
                    case "CreateVendors"://, Name: "Create Vendors " });
                        {
                            dummyTenantProviderArg.CreateVendors = parameterArgs.Amount;
                            g.Amount2addMore = true;
                            g.GenrateGLAccount(dummyTenantProviderArg, accountingContext, chartOfAccountProvider, displayNumberProvider, null, parameterArgs.Tenant);
                        }
                        break;
                    case "CreateCustomers":///, Name: "Create Suppliers " });
                        {
                            dummyTenantProviderArg.CreateCustomers = parameterArgs.Amount;
                            g.Amount2addMore = true;
                            g.GenrateGLAccount(dummyTenantProviderArg, accountingContext, chartOfAccountProvider, displayNumberProvider, null, parameterArgs.Tenant);
                        }
                        break;

                    case "CreateJournal"://, Name: "Create Journal " });
                        {

                            g.GenrateJournals(parameterArgs.Amount, accountingContext, parameterArgs.JournalYYYY, parameterArgs.Tenant);
                        }
                        break;

                    case "CreateJournalEvery"://, Name: "Create Journal Every" });
                        {
                            while (true)
                            {
                                //string logStatus = "";
                                //this.ChangeStatus("I", null, logStatus);
                                //Action<string> action = new Action<string>(log=>)
                                    
                                Thread.Sleep(TimeSpan.FromMinutes(parameterArgs.SleepEveryMinute));
                                g = new DummyTenantProvider();
                                g.GenrateJournals(parameterArgs.Amount, accountingContext, parameterArgs.JournalYYYY, parameterArgs.Tenant);
                            }
                        }
                        break;


                    default:
                        break;
                }
            }

            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "AccLoadTest", new DateTime(2019, 5, 1));
                throw;

            }

            
        }

        public void CreateBatchAccountingLoadTestTask(int tenant ,string ActionType,int amount , int sleepEveryMinute,int year)
        {
            
            
                var args = new BatchAccountingLoadArg() { Tenant = tenant, ActionType = ActionType,Amount= amount, SleepEveryMinute= sleepEveryMinute, JournalYYYY= year };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(BatchAccountingLoadArg));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();
                BatchTaskExecutionPM taskExe = null;
                taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "CreateBatchAccountingLoadTestTask",
                    Tenant = tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchAccountingLoadTestTask,Logitude.Accounting.BL",
                    CreateDate = DateTime.Now,
                    PrametersXml = xmlParameters,
                    StatusCode = "C",

                };



                var MyContext = InfrastructureContext.GetContext(tenant);
                var bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                bteUpdateService.Update(taskExe, true);

            bool immdet = false;
            if (!immdet)
            {


                // 2- Send to queue
                var queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);


                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", tenant.ToString() }
                });
            }
            else
            {
                var taskIt = new BatchAccountingLoadTestTask(taskExe) as BatchTaskExecutionsService;
                taskIt.Execute();
            }
            
        }
    }
    public class BatchAccountingLoadArg {
        

        public int Tenant { get; set; }
        public string ActionType { get; set; }
        public int Amount { get;  set; }
        public int SleepEveryMinute { get; set; }
        public int JournalYYYY { get;  set; }
    }

}

using Logitude.Accounting.BL.CoreBL.FunctionalTests;
using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchAccFunctionalTestTask : BatchTaskExecutionsService
    {

        public enum AccFunctionalState
        {
            ClearAccountingDB=1,
            InsertApprovedJournalsDelay10Min,
            CheckTrailReport

        }
        public BatchAccFunctionalTestTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(BatchFunctionalTestTaskArg));
            var parameterArgs = serializer.Deserialize(stringReader) as BatchFunctionalTestTaskArg;


            string CommunicationsData =GetCommunicationsData(parameterArgs.Tenant, parameterArgs.CommunicationLogId);
            var parameterArgsFromCommunicationsData = serializer.Deserialize(new System.IO.StringReader(CommunicationsData)) as BatchFunctionalTestTaskArg;

            var tenantQuery = new TenantQuery(parameterArgs.Tenant);
            TenantPM tenant = tenantQuery.GetTenantFromDB(parameterArgs.Tenant);
            if (!tenant.IsTestTenant)
            {
                throw new Exception("!tenant.IsTestTenant");
            }
            try
            {
                AccFunctionalState accFunctionalState;
                if (!Enum.TryParse< AccFunctionalState>(parameterArgs.MyState,out accFunctionalState))
                {
                    throw new Exception($"Enum.TryParse< AccFunctionalState>(parameterArgs.MyState{parameterArgs.MyState}");
                }
                switch (accFunctionalState)
                {
                    case AccFunctionalState.ClearAccountingDB:
                    case AccFunctionalState.InsertApprovedJournalsDelay10Min:

                        using (var scope = TransactionFactory.GetTransaction())
                        {
                            var clearAccountingDB = new ClearAccountingDB();
                            clearAccountingDB.ClearDB(parameterArgs.Tenant);

                            JournalToGLAccountMoreData.BuildJournals(parameterArgsFromCommunicationsData.Tenant, parameterArgsFromCommunicationsData.JournalInput);
                            parameterArgs.MyState = AccFunctionalState.CheckTrailReport.ToString();
                            BatchAccFunctionalTestTask.CreateBatchFunctionalTestTask(parameterArgs);
                            scope.Complete();
                        }
                        break;

                    case AccFunctionalState.CheckTrailReport:

                        using (var scope = TransactionFactory.GetTransaction())
                        {
                            List<string> res = parameterArgsFromCommunicationsData.ExpectedGLAccount
    .Select(gLAccountOutput =>
    JournalToGLAccountMoreData.CheckGLAccount(parameterArgs.Tenant, gLAccountOutput)).ToList();
                            string result = string.Join(Environment.NewLine, res.ToArray());

                            CheckTrailReport(parameterArgsFromCommunicationsData.ExpectedGLAccount, parameterArgs);
                            this.ChangeStatus("D", null, result);



                            scope.Complete();
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchFunctionalTestTask", new DateTime(2019, 5, 1));
                throw;

            }



        }

        
        
        string GetCommunicationsData(int tenant, string CommunicationLogId)
        {
            var _CommunicationLog = Communications.GetCommunicationLog(tenant, CommunicationLogId);
            if (_CommunicationLog == null)
            {
                throw new Exception("Cannnot GetCommunicationLog");
            }
            var communicationsData = Communications.GetData(_CommunicationLog); ;
            if (string.IsNullOrWhiteSpace(communicationsData))
            {
                throw new Exception("communicationsData is null");
            }
            return communicationsData;

        }

        public string CheckTrailReport(List<GLAccountPM> expectedGLAccount, BatchFunctionalTestTaskArg parameterArgs)
        
        {
            ///{"Tenant":1148,"FromDate":"2016-06-10T00:00:00","ToDate":"2019-06-10T00:00:00+03:00","TrailReportLevelOption":"ChartofaccountType=1,Chartofaccount=2,GLAccount=3","MyTrailReportLevel":3,"CurrenciesDetailed":0,"Category1":"","Category5":"","DetailedControlClients":false,"DetailedControlVendors":false,"DetailedControlJob":false,"DetailedControlFile":false,"Suppress_DoNotShowCardWithoutActivity":0}

            using (var trailReportService = TrailReportFactory.CreateNew(new TrailReportParam()
            {
                Tenant = parameterArgs.Tenant,
                MyTrailReportLevel = ReportLevel.GLAccount,
                FromDate = new DateTime(DateTime.Now.Year, 1, 1),
                ToDate = DateTime.Now.Date,
            }))
            {

                var res = trailReportService.Execute();
                var myList = (
                    from expRow in expectedGLAccount
                    join trailRow in res
                    on expRow.DisplayNumber equals trailRow.GLAccountNumber
                    into trailRowJoin
                    from subtrailRowJoin in trailRowJoin.DefaultIfEmpty()
                    select new
                    {
                        expRow.DisplayNumber,
                        ExpectedLocalCloseBalance = expRow.BalanceInLocalCurrency,
                        subtrailRowJoin.LocalCloseBalance
                    }
                        ).ToList();

                var sb = new StringBuilder().AppendLine("CheckTrailReport");
                myList.ForEach(r => {
                    sb.Append("DisplayNumber:").Append(r.DisplayNumber);
                    if (r.LocalCloseBalance== r.ExpectedLocalCloseBalance)
                    {
                        sb.Append("equal:").AppendLine(r.ExpectedLocalCloseBalance.GetValueOrDefault().ToString());
                    }
                    else
                    {
                        sb.AppendLine(
$"Expected:{r.ExpectedLocalCloseBalance.GetValueOrDefault()}!=Real{r.LocalCloseBalance}");
                    }
                });
                return sb.ToString();
            }
        }


        public static void CreateBatchFunctionalTestTask(BatchFunctionalTestTaskArg args,bool delay2Min)
        {

            


            


            
            var stringwriter = new System.IO.StringWriter();
            var serializer1 = new XmlSerializer(typeof(BatchFunctionalTestTaskArg));
            serializer1.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();

            BatchTaskExecutionPM taskExe = null;
            taskExe = new BatchTaskExecutionPM()
            {
                Subject = $"CreateBatchFunctionalTestTask({args.MyState.ToString()})",
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchAccFunctionalTestTask,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",// wtf is "c" no alternative 

            };



            var MyContext = InfrastructureContext.GetContext(args.Tenant);
            var bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            bteUpdateService.Update(taskExe, true);

            bool immdet = false;
            if (!immdet)
            {


                // 2- Send to queue
                var queueservice = new DbQueueService();
                queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
                TimeSpan myTimeSpan = null;
                if (delay2Min) { TimeSpan.FromMinutes(2); }

                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", args.Tenant.ToString() }
                }, myTimeSpan);
            }
            else
            {
                var taskIt = new BatchAccFunctionalTestTask(taskExe) as BatchTaskExecutionsService;
                taskIt.Execute();
            }

        }

    }
    public class BatchFunctionalTestTaskArg
    {
        public int Tenant { get; set; }
        public string MyState { get; set; }
        public List<JournalPM> JournalInput { get; set; }
        public List<GLAccountPM> ExpectedGLAccount { get; set; }
        public string CommunicationLogId { get; set; }
    }
}

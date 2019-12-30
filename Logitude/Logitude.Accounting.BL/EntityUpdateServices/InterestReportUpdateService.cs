using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class InterestReportUpdateService
    {
        protected override void OnCreating(InterestReportPM entityPM, EntityPM entityParentPM)
        {
            entityPM.CreateDateTime = DateTime.UtcNow;
            entityPM.InterestReportStatusCode = "1";
            entityPM.ReportNumber = CodeCounter.GetNumber("InterestReport", entityPM.Tenant).ToString();
            GLAccountRepository gLAccountRepository = new GLAccountRepository(entityPM.Tenant);
            GLAccount gLAccount = gLAccountRepository.GetSingle(entityPM.GLAccountId, entityPM.Tenant);
            entityPM.GLAccountInterestCreditLimit = gLAccount.InterestCreditLimit;
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            if (gLAccount.ActiveForInterest == false)
            {
                throw new Exception(TextCodesTranslator.TranslateText("InterestReport.O.Customerisnotdefined", entityPM.Tenant, showLocals));
            }
            createBTE(entityPM);
        }

        protected override void OnUpdating(InterestReportPM entityPM, InterestReport entityPOCO)
        {
            entityPM.UpdateDateTime = DateTime.UtcNow;

        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

        private void createBTE(InterestReportPM entityPM)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            InterestReportArgs args = new InterestReportArgs() { Tenant = entityPM.Tenant, InterestReportId = entityPM.Id};
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(InterestReportArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Create Interest Report",
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchInterestReportService,Logitude.Accounting.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(entityPM.Tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", entityPM.Tenant.ToString() }
                });
        }


    }
}
public class InterestReportArgs
{
    public string InterestReportId { get; set; }
    public int Tenant { get; set; }
}
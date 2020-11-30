 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using System.Xml.Serialization;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class AccountingIntegrityCheckUpdateService
   {
        public double DelayQueueInMinutes { get;  set; }

        protected override void OnCreating(AccountingIntegrityCheckPM entityPM, EntityPM entityParentPM)
        {
            if (String.IsNullOrEmpty(entityPM.Id) || entityPM.Id == "new") entityPM.Id = IdCounter.GetNumber("AccountingIntegrityCheck", entityPM.Tenant);

            entityPM.CreateDateTimeUTC = DateTime.UtcNow;

            createBTE(entityPM);

            base.OnCreating(entityPM, entityParentPM);
        }

        private void createBTE(AccountingIntegrityCheckPM entityPM)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            IntegrityCheckArgs args = new IntegrityCheckArgs() { Tenant = entityPM.Tenant, EntityId = entityPM.Id ,SendEmailWhileError= entityPM.SendEmailWhileError };
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(IntegrityCheckArgs));
            serializer.Serialize(stringwriter, args);
            string xmlParameters = stringwriter.ToString();

            //string xmlParameters = entityPM.ParametersXML;

            //var _params = new AccountingIntegrityInParam()
            //{
            //    Tenant = 1,
            //    FromMonthInclusive = DateTime.Now,
            //    ToMonthInclusive = DateTime.Now,
            //};


            //var xmlParameters = LogitudeXmlSerializer.SerializeObjectToXmlElementString(_params);



            taskExe = new BatchTaskExecutionPM()
            {
                Subject = "Create Accounting Integrity Check",
                Tenant = entityPM.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchIntegrityCheckService,Logitude.Accounting.BL",
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
                }, Tenant, TimeSpan.FromMinutes(this.DelayQueueInMinutes));
        }


    }
    public class IntegrityCheckArgs
    {
        public int Tenant { get; set; }
        public DateTime FromMonthInclusive { get; set; }
        public DateTime ToMonthInclusive { get; set; }

        // others
        public string EntityId { get; set; }

        public bool SendEmailWhileError { get; set; }

    }

}
	 
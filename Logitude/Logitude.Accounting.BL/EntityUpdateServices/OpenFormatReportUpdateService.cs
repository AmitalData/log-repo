using Logitude.Accounting.BL.CoreBL;
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
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class OpenFormatReportUpdateService
    {


        protected override void OnCreating(OpenFormatReportPM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("OpenFormatReport", entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
            showLocals = SetShowLocalLabels(entityPM);
            if (entityPM.ToDate > DateTime.Today)
            {
                throw new Exception(TranslateTextsClass.Translate("Accounting.General.O.FutureDateNotAllowed", entityPM.Tenant, showLocals));
            }
            entityPM.CreatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
           entityPM.ToDate = new DateTime(entityPM.ToDate.Year, entityPM.ToDate.Month,  entityPM.ToDate.Day, 23, 59, 59);
            entityPM.ReportNumber= CodeCounter.GetNumber("OpenFormatReport", entityPM.Tenant).ToString(); 

            entityPM.StatusTypeCode = "1";
            entityPM.UpdateDate = DateTime.Now; ;
          
          
            Validate(entityPM);

         
        }
        bool showLocals;
        public bool SetShowLocalLabels(OpenFormatReportPM entityPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant) ?? new ContactPM();
            showLocals = !contact.DontShowLocal;
            return showLocals;
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

        protected override void AfterUpdating(OpenFormatReportPM entityPM, EntityPM entityParentPM)
        {
            base.AfterUpdating(entityPM, entityParentPM);
           
        

            if(entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                PNCFileArgs args = new PNCFileArgs() { ReportId = entityPM.Id, Tenant = entityPM.Tenant, TestingMode = entityPM.TestingMode };
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(PNCFileArgs));
                serializer.Serialize(stringwriter, args);
                string xmlParameters = stringwriter.ToString();
                BatchTaskExecutionPM taskExe = null;
                taskExe = new BatchTaskExecutionPM()
                {
                    Subject = "Create a flat file for Open Format Report",
                    Tenant = entityPM.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ClassName = "Logitude.Accounting.BL.CoreBL.Batch.BatchOpenFormatReportService,Logitude.Accounting.BL",
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
                entityPM.StatusTypeCode = "2";
                entityPM.ChangeSetOp = ChangeSetOperation.Update;
                this.Update(entityPM, true);
              

                queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant", entityPM.Tenant.ToString() }
                });

            }
        }

    }
}


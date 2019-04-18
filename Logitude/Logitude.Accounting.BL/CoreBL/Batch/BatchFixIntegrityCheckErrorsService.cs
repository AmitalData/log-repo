using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
   public class BatchFixIntegrityCheckErrorsService : BatchTaskExecutionsService
    {
        AccountingIntegrityCheckPM entityPM;
        public BatchFixIntegrityCheckErrorsService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
           
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(IntegrityCheckArgs)); // AccountingIntegrityInParam
            IntegrityCheckArgs parameterArgs = serializer.Deserialize(stringReader) as IntegrityCheckArgs;

         
            RunService(parameterArgs);

        }

        private void RunService(IntegrityCheckArgs args)
        {
          
            IAccountingContext MyContext = AccountingContext.GetContext(args.Tenant);
            AccountingIntegrityCheckUpdateService updateService = new AccountingIntegrityCheckUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            GetIntegrityCheckPM(args);
            entityPM.ChangeSetOp = ChangeSetOperation.Update;

            // Deserilaize parameters
            string xmlParameters = entityPM.ParametersXML;
            if (xmlParameters == null)
            {
                // update status
                entityPM.StatusCode = "5"; // Failed
                entityPM.HasException = true;
                entityPM.DoneDateTimeUTC = DateTime.UtcNow;
                entityPM.ResultXML = "Service Error: " + "Dates are not selected";

                updateService.Update(entityPM, true);

                throw new ApplicationException("Dates are not selected");
            }

            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(AccountingIntegrityInParam));
            AccountingIntegrityInParam _params = serializer.Deserialize(stringReader) as AccountingIntegrityInParam;


            // 2- update status
            entityPM.StatusCode = "2"; // In Progress
            updateService.Update(entityPM, true);

            // 3- check parameters
            AccountingIntegrityService accountingIntegrityService = new AccountingIntegrityService();
            string errorMessage = accountingIntegrityService.CheckParams(_params);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                // update status
                entityPM.StatusCode = "5"; // Failed
                entityPM.HasException = true;
                entityPM.DoneDateTimeUTC = DateTime.UtcNow;
                entityPM.ResultXML = "Service Error: " + errorMessage;
                updateService.Update(entityPM, true);

                throw new ApplicationException(errorMessage);
            }

            // 4- run service
            AccountingIntegrityResult res;
            try
            {
                res = accountingIntegrityService.CheckIntegrity(_params);
            }
            catch (Exception ex)
            {
                // update status
                entityPM.StatusCode = "5"; // Failed
                entityPM.HasException = true;
                entityPM.DoneDateTimeUTC = DateTime.UtcNow;
                entityPM.ResultXML = "Service Error: " + ex.Message;

                updateService.Update(entityPM, true);

                throw new ApplicationException(ex.Message);
            }

            // 5- store resultXML
            if (res != null)
            {
                // Check rows if have exception message
                List<AccountingIntegrityStep> integritySteps = res.MyAccountingIntegrityStep;
                foreach (AccountingIntegrityStep step in integritySteps)
                {
                    if (step.BadRows > 0 && step.ShouldFix == true)
                    //if (!string.IsNullOrWhiteSpace(step.ExceptionMessage))
                    {
                        entityPM.HasException = true;
                        break;
                    }
                }

                // serialize resultXML
                string stringXML = LogitudeXmlSerializer.SerializeObjectToXmlString<AccountingIntegrityResult>(res);

                // update
                entityPM.ResultXML = stringXML;
                entityPM.StatusCode = "3"; // Check Completed
                entityPM.DoneDateTimeUTC = DateTime.UtcNow;

                // save 
                updateService.Update(entityPM, true);
            }

        }

        public void GetIntegrityCheckPM(IntegrityCheckArgs args)
        {

            AccountingIntegrityCheckQueryService query = new AccountingIntegrityCheckQueryService(args.Tenant);
            entityPM = query.GetSingle(args.EntityId, false, false);
        }
    }
}

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
        AccountingIntegrityResult AccountingIntegrityResult;
        AccountingIntegrityService accountingIntegrityService;
        public BatchFixIntegrityCheckErrorsService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
           
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(IntegrityCheckArgs)); 
            IntegrityCheckArgs parameterArgs = serializer.Deserialize(stringReader) as IntegrityCheckArgs;

         
            RunService(parameterArgs);

        }

        private void RunService(IntegrityCheckArgs args)
        {
          
            IAccountingContext MyContext = AccountingContext.GetContext(args.Tenant);
            AccountingIntegrityCheckUpdateService updateService = new AccountingIntegrityCheckUpdateService(MyContext, new Dictionary<string, IContext>(), args.Tenant);
            GetIntegrityCheckPM(args);
            DeserializeAccountingIntegrityResult(entityPM.ResultXML);

           
            try
            {
                FixingIntegrityCheck(args.Tenant, AccountingIntegrityResult);
                
                UpdateIntegrityCheck(entityPM);
            }
            catch (Exception ex)
            {
                SetIntegrityCheckFaid(entityPM);
                
                UpdateIntegrityCheck(entityPM);
                throw new ApplicationException(ex.Message);
            }

         

        }

        public void GetIntegrityCheckPM(IntegrityCheckArgs args)
        {

            AccountingIntegrityCheckQueryService query = new AccountingIntegrityCheckQueryService(args.Tenant);
            entityPM = query.GetSingle(args.EntityId, false, false);
        }

        public void DeserializeAccountingIntegrityResult(string resultXml)
        {

            System.IO.StringReader stringReader = new System.IO.StringReader(entityPM.ResultXML);
            XmlSerializer serializer = new XmlSerializer(typeof(AccountingIntegrityResult)); 
            AccountingIntegrityResult = serializer.Deserialize(stringReader) as AccountingIntegrityResult;

        }

        public void UpdateIntegrityCheck(AccountingIntegrityCheckPM entityPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            AccountingIntegrityCheckUpdateService updateService = new AccountingIntegrityCheckUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(entityPM, true);


        }

        public void SetIntegrityCheckFaid(AccountingIntegrityCheckPM entityPM)
        {
            entityPM.StatusCode = "5";
        }
        public void SetIntegrityCheckFixCompleted(AccountingIntegrityCheckPM entityPM)
        {
            entityPM.StatusCode = "4";
        }

        public void FixingIntegrityCheck(int tenant, AccountingIntegrityResult AccountingIntegrityResult)
        {
            accountingIntegrityService = new AccountingIntegrityService();
            accountingIntegrityService.FixDBIntegrity(tenant, AccountingIntegrityResult.MyAccountingIntegrityStep);
            var itemWithException = AccountingIntegrityResult.MyAccountingIntegrityStep.Where(d => d.ExceptionMessage != null || d.BadRows >0 || d.ShouldFix ==true).FirstOrDefault();
            if (itemWithException != null)
            {
                SetIntegrityCheckFaid(entityPM);
            }
            else
            {
                SetIntegrityCheckFixCompleted(entityPM);
            }

            entityPM.ResultXML =  LogitudeXmlSerializer.SerializeObjectToXmlString(AccountingIntegrityResult);


        }

    }
}

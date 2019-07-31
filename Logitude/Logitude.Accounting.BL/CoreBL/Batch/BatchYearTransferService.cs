using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchYearTransferService : BatchTaskExecutionsService
    {

        public BatchYearTransferService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution) { }

        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(BatchYearTransferParams));
            var parameterArgs = serializer.Deserialize(stringReader) as BatchYearTransferParams;
            var accountingContext = AccountingContext.GetContext(parameterArgs.Tenant);

            try
            {

                JournalPM journal = null;
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
                {

                    

                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);

                    IYearTransferService yearTransferService = new YearTransferService();
                    journal = yearTransferService.ProccessJournal(accountingContext, parameterArgs.YYyear, parameterArgs.Tenant);
                    bool SuppressCheckGLAccountIsMultiCurrencyWI40640 = false;
                    var parser = new JournalApproveParser(journal, false,
                    AccountingValidationContextServiceProvider.NewJournalValidatorContextByAContext(accountingContext, journal, SuppressCheckGLAccountIsMultiCurrencyWI40640)
                    );
                    parser.ParseIt();

                    scope.Complete();
                    
                }

            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "AccLoadTest", new DateTime(2019, 5, 1));
                throw;

            }


        }



    }
    public class BatchYearTransferParams {
        
        
        public int Tenant { get; set; }
        public int YYyear { get; set; }
    }

}
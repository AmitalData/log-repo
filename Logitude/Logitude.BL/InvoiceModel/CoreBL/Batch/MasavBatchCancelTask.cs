using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.InvoiceModel.CoreBL.Batch
{
    public class MasavBatchCancelTask : BatchTaskExecutionsService
    {
        public MasavBatchCancelTask(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution) { }

        public override void RunCode()
        {
            try
            {
                MasavBatchTransmissionTaskArgs args = GetArgs<MasavBatchTransmissionTaskArgs>();

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    IInvoiceContext objectContext = InvoiceContext.GetContext(args.Tenant);
                    MasavInterfaceService masavInterfaceService = new MasavInterfaceService(objectContext, args.Tenant);
                    masavInterfaceService.Cancel(args.MasavInterfaceId, args.Tenant);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "MasavBatchTransmissionTask", new DateTime(2019, 10, 1));
                throw;
            }
        }
    }
   
}

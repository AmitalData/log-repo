using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.EntityOtherServices
{
    public class MessageEntityService
    {
        public void TransferARInvoices(List<ARInvoice> entities, string filename, int tenant)
        {
            ARInvoiceMessageHelper myHelper = new ARInvoiceMessageHelper(entities, filename, tenant);
            myHelper.Transfer();
        }
        public void TransferAPInvoices(List<APInvoice> entities, string filename, int tenant)
        {
            APInvoiceMessageHelper myHelper = new APInvoiceMessageHelper(entities, filename, tenant);
            myHelper.Transfer();
        }
        public void TransferARPayments(List<ARPayment> entities, string filename, int tenant)
        {
            ARPaymentMessageHelper myHelper = new ARPaymentMessageHelper(entities, filename, tenant);
            myHelper.Transfer();
        }
        public void TransferAPPayments(List<APPayment> entities, string filename, int tenant)
        {
            APPaymentMessageHelper myHelper = new APPaymentMessageHelper(entities, filename, tenant);
            myHelper.Transfer();
        }

        public void RebuildTransferFile(List<ARInvoice> entities, string filename, int tenant)
        {
            ARInvoiceMessageHelper myHelper = new ARInvoiceMessageHelper(entities, filename, tenant);
            myHelper.RebuildFile();
        }
        public void RebuildTransferFile(List<APInvoice> entities, string filename, int tenant)
        {
            APInvoiceMessageHelper myHelper = new APInvoiceMessageHelper(entities, filename, tenant);
            myHelper.RebuildFile();
        }
        public void RebuildTransferFile(List<ARPayment> entities, string filename, int tenant)
        {
            ARPaymentMessageHelper myHelper = new ARPaymentMessageHelper(entities, filename, tenant);
            myHelper.RebuildFile();
        }

        public void RebuildTransferFile(List<APPayment> entities, string filename, int tenant)
        {
            APPaymentMessageHelper myHelper = new APPaymentMessageHelper(entities, filename, tenant);
            myHelper.RebuildFile();
        }
    }
}

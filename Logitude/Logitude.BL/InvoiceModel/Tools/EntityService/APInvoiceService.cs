using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class APInvoiceService
    {
        private int tenant;
        private IInvoiceContext objectContext;
        public APInvoiceService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
        }

        public void Create(APInvoicePM entityPM)
        {
            entityPM.InvoiceNumber = MethodHelper.Trim(entityPM.InvoiceNumber);

            if (entityPM.IsMultipleEntities)
            {
                APInvoiceMultipleShipmentService service = new APInvoiceMultipleShipmentService(objectContext, entityPM);
                service.Create();
            }

            else
            {
                APInvoiceNormalService service = new APInvoiceNormalService(objectContext, entityPM);
                service.Create();
            }
        }

        public void Update(APInvoicePM entityPM, bool mapComposition = false)
        {
            entityPM.InvoiceNumber = MethodHelper.Trim(entityPM.InvoiceNumber);

            if (entityPM.IsMultipleEntities)
            {
                APInvoiceMultipleShipmentService service = new APInvoiceMultipleShipmentService(objectContext, entityPM);

                if (!mapComposition)
                {
                    service.SetChangeSets(invoiceMultipleShipmentsChangeSet, invoicePaymentsChangeSet);
                }

                service.Update(mapComposition);
            }

            else
            {

                APInvoiceNormalService service = new APInvoiceNormalService(objectContext, entityPM);

                if (!mapComposition)
                {
                    service.SetChangeSets(invoiceLinesChangeSet, invoicePaymentsChangeSet);
                }

                service.Update(mapComposition);
            }
        }

        private List<APInvoiceLinePM> invoiceLinesChangeSet;
        private List<APInvoicePaymentPM> invoicePaymentsChangeSet;
        private List<APInvoiceMultipleShipmentPM> invoiceMultipleShipmentsChangeSet;
        public void SetChangeSets(List<APInvoiceLinePM> invoiceLinesChangeSet, List<APInvoicePaymentPM> invoicePaymentsChangeSet, List<APInvoiceMultipleShipmentPM> invoiceMultipleShipmentsChangeSet)
        {
            this.invoiceLinesChangeSet = invoiceLinesChangeSet;
            this.invoicePaymentsChangeSet = invoicePaymentsChangeSet;
            this.invoiceMultipleShipmentsChangeSet = invoiceMultipleShipmentsChangeSet;
        }
    }
}

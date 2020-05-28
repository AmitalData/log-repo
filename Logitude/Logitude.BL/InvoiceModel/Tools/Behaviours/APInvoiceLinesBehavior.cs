using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.Initializers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours
{
    public class APInvoiceLinesBehavior : IServiceBehaviour
    {
        private APInvoicePM entityPM;

        private APInvoiceServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (APInvoiceServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            //this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.EntityPM.TotalVATOnly)
            {
                foreach (APInvoiceLinePM item in initializer.EntityPM.InvoiceLines)
                {
                    if (item.VatTypeId != null)
                    {
                        item.VatTypeId = null;
                        item.VatPercentage = null;

                        if (item.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.None)
                        {
                            item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        }
                    }
                }
            }
        }
    }
}

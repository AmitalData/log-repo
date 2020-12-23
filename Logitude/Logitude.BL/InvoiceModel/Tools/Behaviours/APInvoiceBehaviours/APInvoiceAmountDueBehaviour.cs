using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.Initializers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours
{
    public class APInvoiceAmountDueBehaviour : IServiceBehaviour
    {
        private APInvoicePM entityPM;

        private APInvoiceServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (APInvoiceServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;
            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {
            if (initializer.IsNewEntity)
            {
                this.MapAmountToAmountDue();
            }

            else
            {
                if (entityPM.StatusCode != "PP" && entityPM.StatusCode != "PD")
                {
                    if (entityPM.InvoicePayments.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert || d.ChangeSetOp == ChangeSetOperation.Delete).Count() == 0)
                    {
                        this.MapAmountToAmountDue();
                    }
                }
            }
        }

        private void MapAmountToAmountDue()
        {
            entityPM.AmountDue = entityPM.AmountInInvoiceCurrency == null ? 0 : entityPM.AmountInInvoiceCurrency.Value;
            entityPM.AmountDueInLocalCurrency = entityPM.AmountDueInLocalCurrency == null ? 0 : entityPM.AmountDueInLocalCurrency.Value;
            entityPM.AmountDueInProfitCurrency = entityPM.AmountDueInProfitCurrency == null ? 0 : entityPM.AmountDueInProfitCurrency.Value;
        }
    }
}

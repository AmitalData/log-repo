using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceFreightAmountUpdateService : EntityUpdateService<SupplierInvoiceFreightAmount, SupplierInvoiceFreightAmountPM, SupplierInvoicePM>

    {

        protected override void OnCreating(SupplierInvoiceFreightAmountPM entityPM, SupplierInvoicePM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.InvoiceCounterKey;
            entityPM.Id = IdCounter.GetNumber("Customs.SupplierInvoiceFreightAmount", entityPM.Tenant);
        }

        protected override void AfterUpdating(SupplierInvoiceFreightAmountPM entityPM, SupplierInvoicePM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPM.ChangeSetOp = ChangeSetOperation.None;
            }
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceFreightAmountRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceUCRUpdateService : EntityUpdateService<SupplierInvoiceUCR, SupplierInvoiceUCRPM, SupplierInvoicePM>

    {

        protected override void OnCreating(SupplierInvoiceUCRPM entityPM, SupplierInvoicePM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.InvoiceCounterKey;
         
       
        }

        protected override void AfterUpdating(SupplierInvoiceUCRPM entityPM, SupplierInvoicePM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPM.ChangeSetOp = ChangeSetOperation.None;
            }
        }

        //public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        //{
        //    (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoicePaymentRepository).FastDeleteMulti(entityKeyFields);
        //}
    }
}

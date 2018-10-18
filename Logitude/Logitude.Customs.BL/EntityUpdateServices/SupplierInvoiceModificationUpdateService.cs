using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceModificationUpdateService
    {
        protected override void OnCreating(SupplierInvoiceModificationPM entityPM, SupplierInvoicePM entityParentPM)
        {

              if (entityParentPM == null)
            {
                throw new Exception("Supplier Invoice Modification Update Service ,must be apart of Domain Model ");
            }
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.InvoiceCounterKey;
            entityPM.Tenant = entityParentPM.Tenant;
           // int lastKey = entityRepository.getLastModificationKey(entityPM.DeclarationId, entityPM.InvoiceCounterKey);
            int lastKey = 0;

            if (entityParentPM.SupplierInvoiceModifications.Count > 0)
            {
                lastKey = entityParentPM.SupplierInvoiceModifications.Max(d => d.ModificationCounterKey);
            }
           
            entityPM.ModificationCounterKey = lastKey + 1;
            base.OnCreating(entityPM, entityParentPM);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceModificationRepository).FastDeleteMulti(entityKeyFields);
        }
    }
     
}

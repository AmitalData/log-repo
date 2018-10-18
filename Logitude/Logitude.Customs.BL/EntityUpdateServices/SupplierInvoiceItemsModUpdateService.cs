using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceItemsModUpdateService
    {
        protected override void OnCreating(SupplierInvoiceItemsModPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {

            entityRepository = new Data.Repsitories.SupplierInvoiceItemsModRepository(entityPM.Tenant);
            if (entityParentPM == null)
            {
                throw new Exception("Supplier Invoice Items Modification Update Service ,must be apart of Domain Model "); 
            }
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.LineNumber = entityParentPM.LineNumber;
            entityPM.Tenant = entityParentPM.Tenant;
            //int lastKey = entityRepository.getLastModificationKey(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.LineNumber);
            //entityPM.ModificationCounterKey = lastKey + 1;
            int lastKey = 0;

            if (entityParentPM.SupplierInvoiceItemsMods.Count > 0)
            {
                lastKey = entityParentPM.SupplierInvoiceItemsMods.Max(d => d.ModificationCounterKey);
            }
            entityPM.ModificationCounterKey = lastKey + 1;
            

            base.OnCreating(entityPM, entityParentPM);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsModRepository).FastDeleteMulti(entityKeyFields);
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsModRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}

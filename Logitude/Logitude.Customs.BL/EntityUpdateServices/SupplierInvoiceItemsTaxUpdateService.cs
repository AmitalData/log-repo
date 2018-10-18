using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvoiceItemsTaxUpdateService
    {
        //<--- Yuval Chalup 26.05.2015 TASK-13473
        protected override void OnCreating(SupplierInvoiceItemsTaxPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.LineNumber = entityParentPM.LineNumber;

            
        }
        //Yuval Chalup 26.05.2015 TASK-13473 --->

        protected override void OnUpdating(SupplierInvoiceItemsTaxPM entityPM)
        {
            base.OnUpdating(entityPM);
        }

        protected override void UpdateComposition(SupplierInvoiceItemsTaxPM entityPM)
        {
            //SupplierInvoiceItemsTaxesModUpdateService supplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            //supplierInvoiceItemsTaxesModificationUpdateService.UpdateMulti(entityPM.SupplierInvoiceItemsTaxesMods, entityPM.DeletedSupplierInvoiceItemsTaxesMods, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsTaxRepository).FastDeleteMulti(entityKeyFields);
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvoiceItemsTaxRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}

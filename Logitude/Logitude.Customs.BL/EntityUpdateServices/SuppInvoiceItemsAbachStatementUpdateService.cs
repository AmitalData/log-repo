using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Utils;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class SuppInvoiceItemsAbachStatementUpdateService
    {

        protected override void OnCreating(SuppInvoiceItemsAbachStatementPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.InvoiceItemLineNumber = entityParentPM.LineNumber;
            


            base.OnCreating(entityPM, entityParentPM);
        }


        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
       {
           (Repository as Logitude.Customs.Data.Repsitories.SuppInvoiceItemsAbachStatementRepository).FastDeleteMulti(entityKeyFields);
       }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SuppInvoiceItemsAbachStatementRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }
    }
}

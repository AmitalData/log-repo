using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityUpdateServices
{

    public class SupplierInvoiceUpdateService : EntityUpdateService<CCUACCSUP, SupplierInvoicePM, CCUFILEMPM>
    {    
        public SupplierInvoiceUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUACCSUPRepository(context);

            Mapping = new CCUACCSUPDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(SupplierInvoicePM entityPM)
        {
            return new CCUACCSUPKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO, Tenant = entityPM.Tenant };
        }

        protected override void OnCreating(SupplierInvoicePM entityPM, CCUFILEMPM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO;
            entityParentPM.SupplierInvoicesLastLine++;
            entityPM.LINENO = entityParentPM.SupplierInvoicesLastLine;
        }

        protected override void UpdateComposition(SupplierInvoicePM entityPM)
        {
            foreach (var item in entityPM.SupplierInvoiceItems103s)
            {
                item.LastLine105PM = entityPM.LastLine105PM;
            }
            var mySupplierInvoiceItems103UpdateService = new SupplierInvoiceItems103UpdateService(this.MainContext as AmitalContext);          
            mySupplierInvoiceItems103UpdateService.UpdateMulti(entityPM.SupplierInvoiceItems103s, entityPM.DeletedSupplierInvoiceItems103s, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        protected override void OnUpdating(SupplierInvoicePM entityPM)
        {
            
        }

        protected override void AfterUpdating(SupplierInvoicePM entityPM, CCUFILEMPM entityParentPM)
        {
            if (entityParentPM.SupplierInvoices.Count == 0) return;
            entityParentPM.SupplierInvoicesLastLine = entityParentPM.SupplierInvoices.Max(rec => rec.LINENO);
        }

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            var mySupplierInvoiceItems103UpdateService = new SupplierInvoiceItems103UpdateService(this.MainContext as AmitalContext);
            mySupplierInvoiceItems103UpdateService.FastDeleteComposition(entityKeyFields);

            (Repository as CCUACCSUPRepository).FastDeleteMulti(entityKeyFields); 
        }
    }
}

using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{

    public class SupplierInvoiceQueryService : EntityQueryService<CCUACCSUP, CCUACCSUPKeys, SupplierInvoicePM, CCUFILEMPM, CCUFILEMKeys>
    {
        
        
        public SupplierInvoiceQueryService(AmitalContext context)
        {
            this.MainContext = context;
            this.Repository = new CCUACCSUPRepository(context);

            this.mapping = new CCUACCSUPDataMapping();

        }

     


        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUACCSUP entityPOCO)
        {
            return new CCUACCSUPKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO , Tenant=entityPOCO.TENANT};
        }

        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, SupplierInvoicePM entityPM)
        {
            var myCCUSUPITEMKeys = entityKeys as CCUACCSUPKeys;
            var mySupplierInvoiceItems103QueryService = new SupplierInvoiceItems103QueryService(MainContext as AmitalContext);
            entityPM.SupplierInvoiceItems103s = mySupplierInvoiceItems103QueryService.GetMulti(myCCUSUPITEMKeys, true);
            entityPM.SupplierInvoiceItems103LastLine = entityPM.SupplierInvoiceItems103s.Count == 0 ? 0 : entityPM.SupplierInvoiceItems103s.Max(rec => rec.LINENO);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}

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

    public class SupplierInvoiceItems103QueryService : EntityQueryService<CCUSUPITEM, CCUSUPITEMKeys, SupplierInvoiceItem103PM, SupplierInvoicePM, CCUACCSUPKeys>
    {
        
        public SupplierInvoiceItems103QueryService(AmitalContext context)
        {
            
            this.MainContext = context;
            this.Repository = new CCUSUPITEMRepository(context);

            this.mapping = new CCUSUPITEMDataMapping();
        }

        public SupplierInvoiceItem103PM GetSingle(int FILENO, int LINENO, int ACCLINENO, bool getComposition)
        {
            var keys = new CCUSUPITEMKeys() { FILENO = FILENO, LINENO = LINENO, ACCLINENO = ACCLINENO };
            return base.GetSingle(keys, getComposition ,false);
        }


        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUSUPITEM entityPOCO)
        {
            return new CCUSUPITEMKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO, ACCLINENO = entityPOCO.ACCLINENO };
        }
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, SupplierInvoiceItem103PM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var keys = entityKeys as CCUSUPITEMKeys;
            var mySupplierInvoiceItems105QueryService = new SupplierInvoiceItems105QueryService(amitalContext);
            entityPM.SupplierInvoiceItems105 = mySupplierInvoiceItems105QueryService.GetMulti(keys, true);
            //entityPM.SupplierInvoiceItems105LastLine = (entityPM.SupplierInvoiceItems105.Count == 0) ? 0 : entityPM.SupplierInvoiceItems105.Max(rec => rec.LINENO);

            var myCCUCRREQQueryService = new CCUCRREQQueryService(amitalContext);
            entityPM.CCUCRREQPM = myCCUCRREQQueryService.GetMulti(keys, true);
            entityPM.CCUCRREQPMLastLine = (entityPM.CCUCRREQPM.Count == 0) ? 0 : entityPM.CCUCRREQPM.Max(rec => rec.LINENO);

            var myCCUSUPITEMSIQueryService = new CCUSUPITEMSIQueryService(amitalContext);
            entityPM.CCUSUPITEMSIPM = myCCUSUPITEMSIQueryService.GetSingle(entityPM.FILENO, entityPM.LINENO, entityPM.ACCLINENO, false);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}

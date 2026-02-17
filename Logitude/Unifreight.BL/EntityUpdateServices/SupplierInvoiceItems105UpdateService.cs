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

    public class SupplierInvoiceItems105UpdateService : EntityUpdateService<CCUCUSTITEM, SupplierInvoiceItem105PM, SupplierInvoiceItem103PM>
    {    
        public SupplierInvoiceItems105UpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUCUSTITEMRepository(context);

            Mapping = new CCUCUSTITEMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(SupplierInvoiceItem105PM entityPM)
        {
            return new CCUCUSTITEMKeys() { FILENO = entityPM.FILENO, LINENO = entityPM.LINENO };
        }

        protected override void OnCreating(SupplierInvoiceItem105PM entityPM, SupplierInvoiceItem103PM entityParentPM)
        {
            entityPM.FILENO = entityParentPM.FILENO  ;
            if (entityPM.LINENO == 0)
            {
                entityParentPM.LastLine105PM.LastLine105++;
                entityPM.LINENO = entityParentPM.LastLine105PM.LastLine105;
            }
            entityPM.ORDERLINE = entityPM.LINENO;
        }

        protected override void UpdateComposition(SupplierInvoiceItem105PM entityPM)
        {

            var myCCUCARUpdateService = new CCUCARUpdateService(this.MainContext as AmitalContext);
            myCCUCARUpdateService.UpdateMulti(entityPM.CCUCARs, entityPM.DeletedCCUCARPM, entityPM, false);

            var myCCUCARLUpdateService = new CCUCARLUpdateService(this.MainContext as AmitalContext);
            myCCUCARLUpdateService.UpdateMulti(entityPM.CCUCARLs, entityPM.DeletedCCUCARLPM, entityPM, false);

            var myCCUCARSCUpdateService = new CCUCARSCUpdateService(this.MainContext as AmitalContext); // moran 11.7.16 - Task 20132
            myCCUCARSCUpdateService.UpdateMulti(entityPM.CCUCARSCs, entityPM.DeletedCCUCARSCPM, entityPM, false);

        }

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            var myCCUCARLUpdateService = new CCUCARLUpdateService(this.MainContext as AmitalContext);
            myCCUCARLUpdateService.FastDeleteComposition(entityKeyFields);

            var myCCUCARUpdateService = new CCUCARUpdateService(this.MainContext as AmitalContext);
            myCCUCARUpdateService.FastDeleteComposition(entityKeyFields);

            var myCCUCARSCUpdateService = new CCUCARSCUpdateService(this.MainContext as AmitalContext); // moran 11.7.16 - Task 20132
            myCCUCARSCUpdateService.FastDeleteComposition(entityKeyFields);
            
            (Repository as CCUCUSTITEMRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}

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
using Unifreight.BL.EntityPMs.UGenerated;

namespace Unifreight.BL.EntityUpdateServices
{

    public class SupplierInvoiceItems103UpdateService : EntityUpdateService<CCUSUPITEM, SupplierInvoiceItem103PM, SupplierInvoicePM >
    {
        public SupplierInvoiceItems103UpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUSUPITEMRepository(context);

            Mapping = new CCUSUPITEMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(SupplierInvoiceItem103PM entityPM)
        {
            return new CCUSUPITEMKeys() { FILENO = entityPM.FILENO, ACCLINENO = entityPM.ACCLINENO, LINENO = entityPM.LINENO };
        }

        protected override void UpdateComposition(SupplierInvoiceItem103PM entityPM)
        {
            //UpdateComposition move to  OnUpdating Due changing Entity PM

            SetLastLine(entityPM);
            var mySupplierInvoiceItems105UpdateService = new SupplierInvoiceItems105UpdateService(this.MainContext as AmitalContext);
            mySupplierInvoiceItems105UpdateService.UpdateMulti(entityPM.SupplierInvoiceItems105, entityPM.DeletedSupplierInvoiceItems105, entityPM, false);
            var supplierInvoiceItem105 = entityPM.SupplierInvoiceItems105.FirstOrDefault();
            if (supplierInvoiceItem105 != null)
            {
                this.EntityPOCO.ITEMLINENO = entityPM.ITEMLINENO = supplierInvoiceItem105.LINENO;
                
            }

            var myCCUCRREQUpdateService = new CCUCRREQUpdateService(this.MainContext as AmitalContext);
            myCCUCRREQUpdateService.UpdateMulti(entityPM.CCUCRREQPM, entityPM.DeletedCCUCRREQPM, entityPM, false);


            var myCCUSUPITEMSIUpdateService = new CCUSUPITEMSIUpdateService(this.MainContext as AmitalContext);
            //myCCUSUPITEMSIUpdateService.Update(entityPM.CCUSUPITEMSIPM, false);
            List<CCUSUPITEMSIPM> listCCUSUPITEMSIPM = new List<CCUSUPITEMSIPM>();
            List<CCUSUPITEMSIPM> listCCUSUPITEMSIPMDeleted = new List<CCUSUPITEMSIPM>();
            listCCUSUPITEMSIPM.Add(entityPM.CCUSUPITEMSIPM);
            listCCUSUPITEMSIPMDeleted.Add(entityPM.DeletedCCUSUPITEMSIPM);
            myCCUSUPITEMSIUpdateService.UpdateMulti(listCCUSUPITEMSIPM, listCCUSUPITEMSIPMDeleted, entityPM, false);

        }

        protected override void OnCreating(SupplierInvoiceItem103PM entityPM, SupplierInvoicePM entityParentPM)
        {
            entityPM.LastLine105PM = entityParentPM.LastLine105PM;
            SetLastLine(entityPM);

            string dirtyDeclarationPMId = entityPM.CurrentContextTag as string;

            entityPM.FILENO = entityParentPM.FILENO;
            entityPM.ACCLINENO = entityParentPM.LINENO;

            entityParentPM.SupplierInvoiceItems103LastLine++;
            entityPM.LINENO = entityParentPM.SupplierInvoiceItems103LastLine;
        }

        private static void SetLastLine(SupplierInvoiceItem103PM entityPM)
        {
            var SupplierInvoiceItem105 = entityPM.SupplierInvoiceItems105.FirstOrDefault();
            if (SupplierInvoiceItem105 != null)
            {
                SupplierInvoiceItem105.LastLine105PM = entityPM.LastLine105PM;
            }
        }

        protected override void OnUpdating(SupplierInvoiceItem103PM entityPM)
        {

        }

        protected override void AfterUpdating(SupplierInvoiceItem103PM entityPM, SupplierInvoicePM entityParentPM)
        {
            if (entityParentPM.SupplierInvoiceItems103s.Count == 0) return;
            
            entityParentPM.SupplierInvoiceItems103LastLine = entityParentPM.SupplierInvoiceItems103s.Max(rec => rec.LINENO);
        }

        internal void FastDeleteComposition(CCUFILEMKeys entityKeyFields)
        {
            var mySupplierInvoiceItems105UpdateService = new SupplierInvoiceItems105UpdateService(this.MainContext as AmitalContext);
            mySupplierInvoiceItems105UpdateService.FastDeleteComposition(entityKeyFields);
            
            var myCCUCRREQUpdateService = new CCUCRREQUpdateService(this.MainContext as AmitalContext);
            myCCUCRREQUpdateService.FastDeleteComposition(entityKeyFields);

            var myCCUTAXUpdateService = new CCUTAXUpdateService(this.MainContext as AmitalContext);
            myCCUTAXUpdateService.FastDeleteComposition(entityKeyFields, "SupplierInvoiceItem");

            var myCCUSUPITEMSIUpdateService = new CCUSUPITEMSIUpdateService(this.MainContext as AmitalContext);
            myCCUSUPITEMSIUpdateService.FastDeleteComposition(entityKeyFields);

            (Repository as CCUSUPITEMRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}

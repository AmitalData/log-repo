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
using Simplog.Server.Infrastructure;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityQueryServices
{
    public class CCUFILEMQueryService : EntityQueryService<CCUFILEM, CCUFILEMKeys, CCUFILEMPM, object, CCUFILEMKeys>
    {        
        public CCUFILEMQueryService(AmitalContext context)
        {
            this.MainContext=context;
            this.Repository = new CCUFILEMRepository(context);
            this.mapping = new CCUFILEMDataMapping();
        }

        public CCUFILEMPM GetSingle(int FILENO, bool getComposition, bool getFromCache )
        {
            var EntityKeys = new CCUFILEMKeys() { FILENO = FILENO};
            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUFILEM entityPOCO)
        {
            return new CCUFILEMKeys() { FILENO = entityPOCO.FILENO  };
        }

        public override void GetComposition(EntityKeyFields entityKeys, CCUFILEMPM entityPM)
        {
            var amitalContext = this.MainContext as AmitalContext;
            var myCCUFILEMKeys = entityKeys as CCUFILEMKeys;

            var CCUMSHGRQueryService = new CCUMSHGRQueryService(amitalContext);
            entityPM.CCUMSHGRs = CCUMSHGRQueryService.GetMulti(myCCUFILEMKeys, true);
            entityPM.CCUMSHGRLastLine = (entityPM.CCUMSHGRs.Count ==0) ? 0 :entityPM.CCUMSHGRs.Max(rec => rec.LINENO);

            var mySupplierInvoiceQueryService = new SupplierInvoiceQueryService(amitalContext);
            entityPM.SupplierInvoices = mySupplierInvoiceQueryService.GetMulti(myCCUFILEMKeys, true);
            entityPM.SupplierInvoicesLastLine = (entityPM.SupplierInvoices.Count == 0) ? 0 : entityPM.SupplierInvoices.Max(rec => rec.LINENO);

            var myCCUTAXQueryService = new CCUTAXQueryService(amitalContext);
            entityPM.CCUTAXPM = myCCUTAXQueryService.GetMulti(myCCUFILEMKeys, true);
            entityPM.CCUTAXPMLastLine = (entityPM.CCUTAXPM.Count == 0) ? 0 : entityPM.CCUTAXPM.Max(rec => rec.LINENO);

            //<--- Yuval Chalup 14.06.2015 TASK-13951
            var myCCUTRANSPVALQueryService = new CCUTRANSPVALQueryService(amitalContext);
            entityPM.CCUTRANSPVALs = myCCUTRANSPVALQueryService.GetMulti(myCCUFILEMKeys, true);
            //Yuval Chalup 14.06.2015 TASK-13951 --->

            var myCCUMESSAGEQueryService = new CCUMESSAGEQueryService(amitalContext);
            entityPM.CCUMESSAGEs = myCCUMESSAGEQueryService.GetMulti(myCCUFILEMKeys, true);

            var myCCUTSRUFOTQueryService = new CCUTSRUFOTQueryService(amitalContext);
            entityPM.CCUTSRUFOTs = myCCUTSRUFOTQueryService.GetMulti(myCCUFILEMKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }

        public int? GetFILENOByCUSTOMFILENO(long lCUSTOMFILENO)
        {
            return (this.Repository as CCUFILEMRepository).GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
        }

        public int? GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(long lCUSTOMFILENO)
        {
            return (this.Repository as CCUFILEMRepository).LockByCUSTOMFILENO_forUpdateNOWAIT(lCUSTOMFILENO);
        }

        //<--- Yuval Chalup 19.11.2015 TASK-17450        
        public CCUFILEM GetCCUFILEMByRESHIMONNO(string reshimonNumber)
        {
            return (this.Repository as CCUFILEMRepository).GetCCUFILEMByRESHIMONNO(reshimonNumber);
        }
        //Yuval Chalup 19.11.2015 TASK-17450 --->
    }
}

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
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs.UGenerated;

namespace Unifreight.BL.EntityQueryServices
{

    public class CCUSUPITEMSIQueryService : EntityQueryService<CCUSUPITEMSI, CCUSUPITEMSIKeys, CCUSUPITEMSIPM,
        SupplierInvoiceItem103PM, CCUSUPITEMKeys>
    {

        public CCUSUPITEMSIQueryService(AmitalContext context)
        {
            this.MainContext = context;
            this.Repository = new CCUSUPITEMSIRepository(context);

            this.mapping = new CCUSUPITEMSIDataMapping();
        }

        public CCUSUPITEMSIPM GetSingle(int FILENO, int LINENO, int ACCLINENO,int tenant,  bool getComposition)
        {
            var keys = new CCUSUPITEMSIKeys() { FILENO = FILENO, LINENO = LINENO, ACCLINENO = ACCLINENO , Tenant=tenant};

            return base.GetSingle(keys, getComposition, false);
        }


        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUSUPITEMSI entityPOCO)
        {
            return new CCUSUPITEMSIKeys() { FILENO = entityPOCO.FILENO, LINENO = entityPOCO.LINENO, ACCLINENO = entityPOCO.ACCLINENO , Tenant = entityPOCO.TENANT };
        }

        //public int? GetFILENOByCUSTOMFILENO(long lCUSTOMFILENO)
        //{
        //    return (this.Repository as CCUFILEMRepository).GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
        //}

        //public List<SupplierInvoiceItem105PM> GetFile105(int? FILENO, bool getComposition)
        //{
        //    var entityPOCOs = (this.Repository as CCUCUSTITEMRepository).GetFile105(FILENO);
        //    List<SupplierInvoiceItem105PM> entityPMs = new List<SupplierInvoiceItem105PM>();

        //    if (getComposition)
        //    {
        //        foreach (CCUCUSTITEM entityPOCO in entityPOCOs)
        //        {
        //            SupplierInvoiceItem105PM entityPM = new SupplierInvoiceItem105PM();
        //            EntityKeyFields entityKeys = GetKeys(entityPOCO);
        //            if (entityKeys != null)
        //            {
        //                GetComposition(entityKeys, entityPM);
        //                mapping.CustomPOCOToPM(entityPM, entityPOCO);
        //                mapping.POCOToPM(entityPM, entityPOCO);
        //                entityPMs.Add(entityPM);
        //            }
        //        }

        //    }
        //    else
        //    {
        //        entityPMs = entityPOCOs.Select(poco => this.GetEntityPM(poco)).ToList();
        //    }
        //    return entityPMs;
        //}

        //public override void GetComposition(EntityKeyFields entityKeys, SupplierInvoiceItem105PM entityPM)
        //{
        //    var amitalContext = this.MainContext as AmitalContext;
        //    var myCCUCUSTITEMKeys = entityKeys as CCUCUSTITEMKeys;

        //    var myCCUCARQueryService = new CCUCARQueryService(amitalContext);
        //    entityPM.CCUCARs = myCCUCARQueryService.GetMulti(myCCUCUSTITEMKeys, true);

        //    var myCCUCARLQueryService = new CCUCARLQueryService(amitalContext);
        //    entityPM.CCUCARLs = myCCUCARLQueryService.GetMulti(myCCUCUSTITEMKeys, true);

        //    base.GetComposition(entityKeys, entityPM);
        //}
    }
}

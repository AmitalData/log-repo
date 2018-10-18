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
using Logitude.Server.Tools.Models;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityUpdateServices
{
    public class CCUFILEMUpdateService : EntityUpdateService<CCUFILEM, CCUFILEMPM, EntityPM>
    {
        public CCUFILEMUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new CCUFILEMRepository(context);

            Mapping = new CCUFILEMDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(CCUFILEMPM entityPM)
        {
            return new CCUFILEMKeys() { FILENO = entityPM.FILENO };
        }

        protected override void OnCreating(CCUFILEMPM entityPM, EntityPM entityParentPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.DeclarationId))
            {
                new BusinessErrorException("please init entityPM.DeclarationId");
            }
            entityPM.FILENO = GetCounter(entityPM.DeclarationId);
            entityPM.OPENDATE = DateTime.Now;
            entityPM.FILECLOSE = 0;
        }

        private int GetCounter(string dirtyDeclarationPMId)
        {
            int i = Convert.ToInt32(dirtyDeclarationPMId.Replace("-", ""));
            return 50000000 + i;
        }

        protected override void UpdateComposition(CCUFILEMPM entityPM)
        {
            var myCCUMSHGRUpdateService = new CCUMSHGRUpdateService(this.MainContext as AmitalContext);
            myCCUMSHGRUpdateService.UpdateMulti(entityPM.CCUMSHGRs, entityPM.DeletedCCUMSHGRs, entityPM, false);

            if (entityPM.SupplierInvoices != null)
            {
                foreach (var item in entityPM.SupplierInvoices)
                {
                    item.LastLine105PM = entityPM.LastLine105PM;
                }
            }
            var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(this.MainContext as AmitalContext);
            mySupplierInvoiceUpdateService.UpdateMulti(entityPM.SupplierInvoices, entityPM.DeletedSupplierInvoices, entityPM, false);

            var myCCUTAXUpdateService = new CCUTAXUpdateService(this.MainContext as AmitalContext);
            myCCUTAXUpdateService.UpdateMulti(entityPM.CCUTAXPM, entityPM.DeletedCCUTAXPM, entityPM, false);

            //<--- Yuval Chalup 14.06.2015 TASK-13951
            var myCCUTRANSPVALUpdateService = new CCUTRANSPVALUpdateService(this.MainContext as AmitalContext);
            myCCUTRANSPVALUpdateService.UpdateMulti(entityPM.CCUTRANSPVALs, entityPM.DeletedCCUTRANSPVALs, entityPM, false);
            //Yuval Chalup 14.06.2015 TASK-13951 --->

            base.UpdateComposition(entityPM);
        }

        public void FastDeleteComposition(CCUFILEMPM entityPM)
        {
            var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(this.MainContext as AmitalContext);
            mySupplierInvoiceUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUMSHGRUpdateService = new CCUMSHGRUpdateService(this.MainContext as AmitalContext);
            myCCUMSHGRUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUTAXUpdateService = new CCUTAXUpdateService(this.MainContext as AmitalContext);
            myCCUTAXUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            //<--- Yuval Chalup 14.06.2015 TASK-13951
            var myCCUTRANSPVALUpdateService = new CCUTRANSPVALUpdateService(this.MainContext as AmitalContext);
            myCCUTRANSPVALUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);
            //Yuval Chalup 14.06.2015 TASK-13951 --->
        }

        public void FastDeleteComposition(CCUFILEMPM entityPM, bool isSupplerInvChanged, bool isDeclarationTaxesChanged, bool isConsignmentChanged)
        {
            if (isSupplerInvChanged)
            {
                var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(this.MainContext as AmitalContext);
                mySupplierInvoiceUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);
            }

            if (isConsignmentChanged)
            {
                var myCCUMSHGRUpdateService = new CCUMSHGRUpdateService(this.MainContext as AmitalContext);
                myCCUMSHGRUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);
            }

            if (isDeclarationTaxesChanged)
            {
                var myCCUTAXUpdateService = new CCUTAXUpdateService(this.MainContext as AmitalContext);
                myCCUTAXUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys, "Declaration");
            }

            //<--- Yuval Chalup 14.06.2015 TASK-13951
            var myCCUTRANSPVALUpdateService = new CCUTRANSPVALUpdateService(this.MainContext as AmitalContext);
            myCCUTRANSPVALUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);
            //Yuval Chalup 14.06.2015 TASK-13951 --->
        }

        public void FastTotalDeleteComposition(CCUFILEMPM entityPM) // moran 5.1.16 - AMI-55274
        {
            var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(this.MainContext as AmitalContext);
            mySupplierInvoiceUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUMSHGRUpdateService = new CCUMSHGRUpdateService(this.MainContext as AmitalContext);
            myCCUMSHGRUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUTAXUpdateService = new CCUTAXUpdateService(this.MainContext as AmitalContext);
            myCCUTAXUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUTRANSPVALUpdateService = new CCUTRANSPVALUpdateService(this.MainContext as AmitalContext);
            myCCUTRANSPVALUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUPAYHANDUpdateService = new CCUPAYHANDUpdateService(this.MainContext as AmitalContext);
            myCCUPAYHANDUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            var myCCUPAYLINEFUpdateService = new CCUPAYLINEFUpdateService(this.MainContext as AmitalContext);
            myCCUPAYLINEFUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

 //         var myCCUCARUpdateService = new CCUCARUpdateService(this.MainContext as AmitalContext); // not implemented yet
 //         myCCUCARUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

 //         var myCCUCARLUpdateService = new CCUCARLUpdateService(this.MainContext as AmitalContext); // not implemented yet
 //         myCCUCARLUpdateService.FastDeleteComposition(GetKeys(entityPM) as CCUFILEMKeys);

            (Repository as CCUFILEMRepository).FastDeleteMulti(GetKeys(entityPM) as CCUFILEMKeys);

        }
        
    }
}

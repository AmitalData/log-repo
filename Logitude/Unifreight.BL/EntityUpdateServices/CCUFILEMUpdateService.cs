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
using System.Data.SqlClient;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Devart.Data.Oracle;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.EntityMapping;

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
            return new CCUFILEMKeys() { FILENO = entityPM.FILENO, TENANT = entityPM.Tenant };
        }

        protected override void OnCreating(CCUFILEMPM entityPM, EntityPM entityParentPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.DeclarationId))
            {
                new BusinessErrorException("please init entityPM.DeclarationId");
            }
            entityPM.FILENO = GetCounter(entityPM.DeclarationId,entityPM.Tenant);
            entityPM.OPENDATE = DateTime.Now;
            entityPM.FILECLOSE = 0;
        }


        protected override void OnUpdating(CCUFILEMPM entityPM)
        {
            if(entityPM.Tenant != 0)
            {
                CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
                bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
                if (!isConnectedToUnifreight)
                {
                    entityPM.IS_SYNCH = false;
                    entityPM.LAST_UPDATE_DT = DateTime.Now;
                }
            }

        }

        private int GetCounter(string dirtyDeclarationPMId, int tenant)
        {
            int i = Convert.ToInt32(
                           dirtyDeclarationPMId.Contains('-')
                               ? "1" + dirtyDeclarationPMId.Split('-')[1]
                               : dirtyDeclarationPMId.Replace(tenant + "-", "1"));
            i = 50000000 + i;
            int fileNoLen = 15;
            if (i.ToString().Length > fileNoLen)
            {
                return i - 110_009_120;
            }
            return i;
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


        public object GetFileNoLen_Cache(int tenant)
        {
            string entityKeyString = $"GetFileNoLen_Cache({tenant})";
            var res = CacheManager
                .GetOrInsertNewObject<object>(entityKeyString,
                () => { return this.GetFileNoLen(tenant); });
            return res;

        }

        public object GetFileNoLen(int tenant)
        {
            object columnSize = 0;
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string owner = null;
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(tenant);
            CustomsSetting custSettings = custSettingsRepo.GetSettingByTenant(tenant);
            string strConnString = TenantServerConfigration.GetDbConnection(tenant);

            if (custSettings != null && !string.IsNullOrWhiteSpace(custSettings.UnfConnectionString))
            {
                owner = custSettings.UnfConnectionString.Split(',').Last().ToUpper();
            }
            try
            {

                if (dbms == "oracle")
                {
                    using (OracleConnection con = new OracleConnection(strConnString))
                    {
                        string cmd = "select data_precision from ALL_TAB_COLUMNS where table_name = 'CCUFILEM' and column_name ='FILE_NO' and owner=:p1";

                        OracleCommand oracleCommand = new OracleCommand(cmd, con);
                        oracleCommand.Parameters.Add(new OracleParameter("p1", owner));
                        con.Open(); 
                        columnSize = oracleCommand.ExecuteScalar();
                    }

                }
                return columnSize;
            }
            catch(Exception ex)
            {
                return columnSize;
            }

        }


    }
}

using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Utils;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvioceItemCertificatUpdateService
    {
        const string SIICerExemptionHD379305 = "SIICerExemptionHD379305.LogUntilDateyyyyMMdd";//Bug 149964: CALL #379305 פטור 92 מאותחל באישור עם מענה
        int? maxCounter;
        protected override void OnCreating(SupplierInvioceItemCertificatPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.LineNumber = entityParentPM.LineNumber;
            bool yaronRevertCS7859 = false;
            ICustomContext _Context = MainContext as CustomContext;
            if (yaronRevertCS7859)
            {
                entityPM.ItemCertificateCounterKey = CodeCounter.GetNumber("Customs.SupplierInvioceItemCertificat", entityPM.Tenant);
            }
            else
            {
                SupplierInvioceItemCertificatQueryService supplierInvioceItemCertificatQueryService = new SupplierInvioceItemCertificatQueryService(_Context);
                if (!this.maxCounter.HasValue)
                {
                    this.maxCounter = supplierInvioceItemCertificatQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.LineNumber, entityPM.Tenant);
                }
                entityPM.ItemCertificateCounterKey = maxCounter.Value + 1;
                maxCounter = entityPM.ItemCertificateCounterKey;
            }

            base.OnCreating(entityPM, entityParentPM);

        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvioceItemCertificatRepository).FastDeleteMulti(entityKeyFields);
        }

        

        private void SetDeclarationChanged(int tenant,string declarationId)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update Declarations set IsChanged = 1";
                    cmd = cmd + " where Id=" + "'" + declarationId + "'";

                    OracleCommand sqlCommand = new OracleCommand(cmd, con);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update Customs.Declarations set IsChanged = 1";
                    cmd = cmd + " where Id=" + "'" + declarationId + "'";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        protected override void OnUpdating(SupplierInvioceItemCertificatPM entityPM, SupplierInvioceItemCertificat entityPOCO)
        {
            if (entityPM?.CertificateExemptionTypeCode != entityPOCO?.CertificateExemptionTypeCode &&
                entityPM?.CertificateExemptionTypeCode=="92"
                )
            {
                var logChangesService = new LogChangesService();
                logChangesService.LogIt<SupplierInvioceItemCertificatPM, SupplierInvioceItemCertificat>(
                    appSettingKeyValueIsLogUntilDateyyyyMMdd:SIICerExemptionHD379305, 
                    entityPM, entityPOCO);
            }
        }



        public void UpdateCertificateWithoutCertificateExemptionTypeCode(SupplierInvioceItemCertificatPM cert, int tenant)
        {
            SupplierInvoiceItemQueryService query = new SupplierInvoiceItemQueryService(tenant);
            SupplierInvoiceItemPM item = query.GetSingle(cert.DeclarationId, cert.InvoiceCounterKey, cert.LineNumber, false, false);
            if (!item.SupplierInvioceItemCertificats.Contains(cert))
            {
                item.SupplierInvioceItemCertificats.Add(cert);
            }
            ICustomContext customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            item.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(item, true);
        }


        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvioceItemCertificatRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }

       




    }
}

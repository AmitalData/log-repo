using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class ReconciliationDataMapping: IMapping<ReconciliationPM, Reconciliation>
   {

        public void CustomPMToPOCO(ReconciliationPM entityPM, Reconciliation entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ReconciliationPM entityPM, Reconciliation entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AccountNumber);

            if (entityPOCO.CreatedByUserId != null)
            {
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
                if (userContact != null)
                {
                    entityPM.CreatedByUserName = userContact.EnglishName;
                }
            }

            if (entityPOCO.AccountId != null)
            {
                GLAccountQueryService query = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM account = query.GetSingle(entityPOCO.AccountId, false, false);

                if (account != null)
                {
                    entityPM.AccountName = account.LocalName;
                    entityPM.AccountNumber = account.DisplayNumber;
                    entityPM.CurrencyCode = account.CurrencyCode;
                    entityPM.AccountReconcileMethodCode = account.ReconcileMethodCode;
                }
            }

        }
   }


}
   
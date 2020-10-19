
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
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{

    public partial class CalculatedChartsOfAccountDataMapping : IMapping<CalculatedChartsOfAccountPM, CalculatedChartsOfAccount>
    {

        public void CustomPMToPOCO(CalculatedChartsOfAccountPM entityPM, CalculatedChartsOfAccount entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.CreateDateTime);
            AddPOCOPropertyName(POCOPropertyNames.UpdatedDateTime);
            entityPM.UpdatedDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPOCO.UpdatedDateTime = entityPM.UpdatedDateTime;
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.UserDefinedReportId = entityPM.UserDefinedReportId;
                entityPM.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            }

        }

        public void CustomPOCOToPM(CalculatedChartsOfAccountPM entityPM, CalculatedChartsOfAccount entityPOCO)
        {
            ContactQuery contactQuery = new ContactQuery(entityPOCO.Tenant);

            if (entityPOCO.UpdatedByUserId != null)
            {
                ContactPM updatedByContact = contactQuery.GetSinglePMFromCacheWithSystemUser(entityPOCO.UpdatedByUserId, entityPOCO.Tenant);
                if (updatedByContact == null)
                    updatedByContact = contactQuery.GetSinglePMFromCacheWithSystemUser(entityPOCO.UpdatedByUserId, 0); // user is customer care, get it from tenant 0
                if (updatedByContact != null)
                {
                    entityPM.UpdatedByLocalName = updatedByContact.LocalName;
                    entityPM.UpdatedByEnglishName = updatedByContact.EnglishName;

                }
            }
            if (entityPOCO.CreatedByUserId != null)
            {
                ContactPM CreatedByContact = contactQuery.GetSinglePMFromCacheWithSystemUser(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
                if (CreatedByContact == null)
                    CreatedByContact = contactQuery.GetSinglePMFromCacheWithSystemUser(entityPOCO.CreatedByUserId, 0); // user is customer care, get it from tenant 0
                if (CreatedByContact != null)
                {
                    entityPM.CretedByLocalNameName = CreatedByContact.LocalName;
                    entityPM.CreatedByEnglishName = CreatedByContact.EnglishName;

                }
            }

            if (entityPOCO.ChartOfAccountTypeCode != null)
            {
                ChartOfAccountsTypeQueryService chartOfAccountsTypeQuery = new ChartOfAccountsTypeQueryService(entityPOCO.Tenant);
                ChartOfAccountsTypePM chartOfAccountsTypePM = chartOfAccountsTypeQuery.GetSingle(entityPOCO.ChartOfAccountTypeCode, false, true);
                if (chartOfAccountsTypePM != null)
                {
                    entityPM.ChartOfAccountTypeEnglishName = chartOfAccountsTypePM.EnglishName;
                    entityPM.ChartOfAccountTypeLocalName = chartOfAccountsTypePM.LocalName;

                }
            }


        }
    }
}




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

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class CalculatedChartsOfAccountsLineDataMapping: IMapping<CalculatedChartsOfAccountsLinePM, CalculatedChartsOfAccountsLine>
   {

        public void CustomPMToPOCO(CalculatedChartsOfAccountsLinePM entityPM, CalculatedChartsOfAccountsLine entityPOCO)
        {
            
        }

        public void CustomPOCOToPM(CalculatedChartsOfAccountsLinePM entityPM, CalculatedChartsOfAccountsLine entityPOCO)
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
                    entityPM.CreatedByLocalName = CreatedByContact.LocalName;
                    entityPM.CreatedByEnglishName = CreatedByContact.EnglishName;

                }
            }

            if (entityPOCO.ChartOfAccountId != null)
            {
                ChartOfAccountQueryService chartOfAccountQuery = new ChartOfAccountQueryService(entityPOCO.Tenant);
                ChartOfAccountPM chartOfAccountPM = chartOfAccountQuery.GetSingle(entityPOCO.ChartOfAccountId, false, false);
                if (chartOfAccountPM != null)
                {
                    entityPM.ChartOfAccountEnglishName = chartOfAccountPM.EnglishName;
                    entityPM.ChartOfAccountLocalName = chartOfAccountPM.LocalName;

                }
            }

            if (entityPOCO.GLAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM gLAccountPM = gLAccountQueryService.GetSingle(entityPOCO.GLAccountId, false, false);
                if (gLAccountPM != null)
                {
                    entityPM.GLAccountEnglishName = gLAccountPM.EnglishName;
                    entityPM.GLAccountLocalName = gLAccountPM.LocalName;

                }
            }

            if (entityPOCO.LineTypeCode != null)
            {
                CalculatedChartsLineTypeQueryService calculatedChartsLineTypeQueryService = new CalculatedChartsLineTypeQueryService(entityPOCO.Tenant);
                CalculatedChartsLineTypePM calculatedChartsLineTypePM = calculatedChartsLineTypeQueryService.GetSingle(entityPOCO.GLAccountId, false, true);
                if (calculatedChartsLineTypePM != null)
                {
                    entityPM.LineTypeEnglishName = calculatedChartsLineTypePM.EnglishName;
                    entityPM.LineTypeLocalName = calculatedChartsLineTypePM.LocalName;

                }
            }


        }
    }


}
   
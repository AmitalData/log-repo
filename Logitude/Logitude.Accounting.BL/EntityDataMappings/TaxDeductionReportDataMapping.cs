
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
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using System.Web;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxDeductionReportDataMapping: IMapping<TaxDeductionReportPM, TaxDeductionReport>
   {

        public void CustomPMToPOCO(TaxDeductionReportPM entityPM, TaxDeductionReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(TaxDeductionReportPM entityPM, TaxDeductionReport entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusTypeCode);

            if (entityPOCO.StatusTypeCode != null)
            {
                TaxDeductionReportStatusQueryService queryService = new TaxDeductionReportStatusQueryService(entityPOCO.Tenant);
                TaxDeductionReportStatusPM status = queryService.GetSingle(entityPOCO.StatusTypeCode, false, false);
                if (status != null)
                {

                    ContactQuery contactQuery = new ContactQuery(entityPM.Tenant);
                    ContactPM loggedContact = null;

                    if (HttpContext.Current != null)
                    {
                        loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
                    }
                    else
                    {

                        loggedContact = contactQuery.GetSingleContactPM(entityPM.CreatedByUserId);
                    }


                    if (loggedContact.DontShowLocal)
                    {
                        entityPM.Status = status.EnglishName;

                    }
                    else
                    {
                        //entityPM.Status = status.LocalName;
                        entityPM.StatusLocalName = status.LocalName;
                    }
                }
            }

            if (entityPOCO.CreatedByUserId != null)
            {
                Contact userContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
                if (userContact != null)
                {
                    entityPM.CreatedByUser = userContact.LocalName;
                }
            }



        }
    }


}
   
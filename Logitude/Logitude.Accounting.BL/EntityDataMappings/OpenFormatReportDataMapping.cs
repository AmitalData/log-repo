
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
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using System.Web;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class OpenFormatReportDataMapping: IMapping<OpenFormatReportPM, OpenFormatReport>
   {

        public void CustomPMToPOCO(OpenFormatReportPM entityPM, OpenFormatReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(OpenFormatReportPM entityPM, OpenFormatReport entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusTypeCode);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);
         //   CustomMappedPOCOProperties.Add(POCOPropertyNames.DateTypeCode);

            ContactQuery query = new ContactQuery(entityPOCO.Tenant);
            ContactPM loggedContact = null;
            if (HttpContext.Current != null)
            {
                loggedContact = query.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            }
            else
            {

                loggedContact = query.GetSingleContactPM(entityPM.CreatedByUserId);
            }

            if (entityPOCO.StatusTypeCode != null)
            {
                OpenFormatReportStatusQueryService statusQueryService = new OpenFormatReportStatusQueryService(entityPOCO.Tenant);
                OpenFormatReportStatusPM status = statusQueryService.GetSingle(entityPOCO.StatusTypeCode, false, false);
                if (status != null)
                {
                   // ContactPM loggedContact = null;

                    //if (HttpContext.Current != null)
                    //{
                    //    loggedContact = query.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
                    //}
                    //else
                    //{

                    //    loggedContact = query.GetSingleContactPM(entityPM.CreatedByUserId);
                    //}
                  //  ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.Status = status.EnglishName;
                    }
                    else { entityPM.Status = status.LocalName; }
                   

                }

            }

            if (entityPOCO.CreatedByUserId != null)
            {
               
                ContactPM contact = query.GetSinglePM(entityPOCO.CreatedByUserId, entityPOCO.Tenant);
              
                   

                //  ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
                if (contact != null)
                {
                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.CreatedByUserName = contact.EnglishName;
                    }
                    else { entityPM.CreatedByUserName = contact.LocalName; }


                }

            }


            //if (entityPOCO.DateTypeCode != null)
            //{
            //    OpenFormatDateTypeQueryService typeQueryService = new OpenFormatDateTypeQueryService(entityPOCO.Tenant);
            //    OpenFormatDateTypePM dateType = typeQueryService.GetSingle(entityPOCO.DateTypeCode, false, false);
            //    if (dateType != null)
            //    {
            //       // loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            //        if (loggedContact.DontShowLocal)
            //        {

            //            entityPM.DateTypeName = dateType.EnglishName;
            //        }
            //        else { entityPM.DateTypeName = dateType.LocalName; }


            //    }

            //}


        }
    }


}
   

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
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

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

            ContactPM loggedContact = GetLoggedContact(entityPOCO.Tenant);


            if (entityPOCO.StatusTypeCode != null)
            {
                OpenFormatReportStatusQueryService statusQueryService = new OpenFormatReportStatusQueryService(entityPOCO.Tenant);
                OpenFormatReportStatusPM status = statusQueryService.GetSingle(entityPOCO.StatusTypeCode, false, false);
                if (status != null)
                {
                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.Status = status.EnglishName;
                    }
                    else { entityPM.StatusLocalName = status.LocalName; }
                   

                }

            }

            if (entityPOCO.CreatedByUserId != null)
            {
                ContactQuery query = new ContactQuery(entityPOCO.Tenant);
                ContactPM contact = query.GetSingleContactPM(entityPOCO.CreatedByUserId);

                if (contact != null)
                {
                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.CreatedByUserName = contact.EnglishName;
                    }
                    else { entityPM.UserLocalName = contact.LocalName; }


                }

            }

        }

        private ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
    }


}
   
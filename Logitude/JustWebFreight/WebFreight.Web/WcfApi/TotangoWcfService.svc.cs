using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TotangoWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TotangoWcfService.svc or TotangoWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TotangoWcfService : ITotangoWcfService
    {
        
        public void SendUserActivity(string Email, string orgDisplayName, string module, string activity, int tenant)
        {
            //totangoService.SendUserActivityAsync(TenantContext.Current.LoggedContactId, orgDisplayName, TenantContext.Current.LoggedContact.EnglishName, module, activity, TenantContext.Current.LoggedContactId, TenantContext.Current.Id, false, null, null);
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact contact = contactRepository.GetSingleContactByEmail(Email, tenant);
            ActivityLog.AddContactActivityWithTotango(tenant.ToString(), orgDisplayName, contact.EnglishName, module, activity, contact.Id, tenant, false,null, null, null);
            
        }
    }
}

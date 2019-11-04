using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class AirlineAreaTracing
    {
        public static void Trace(AirlineAreaPM entityPM, AirlineArea entityPOCO, bool isNewEntity)
        {
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            
            if (isNewEntity)
            {
               
            }

            else
            {
                
            }
        }
    }
}

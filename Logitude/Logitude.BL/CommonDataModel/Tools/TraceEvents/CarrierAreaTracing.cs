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
    public class CarrierAreaTracing
    {
        public static void Trace(CarrierAreaPM entityPM, CarrierArea entityPOCO, bool isNewEntity)
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

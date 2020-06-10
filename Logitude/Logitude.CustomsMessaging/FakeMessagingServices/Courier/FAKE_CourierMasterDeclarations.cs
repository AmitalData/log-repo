using Logitude.Customs.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class FAKE_CourierMasterDeclarations 
    {
        public List<string> GetCourierMasterDeclarations(string id,int tenant)
        {
            List<string> decLists = new List<string>();
            CourierDeclarationQueryService courierDeclarationQueryService = new CourierDeclarationQueryService(tenant);
            var courierMasterid = courierDeclarationQueryService.GetCourierMasterIdByDeclarationId(id, tenant);
            if(courierMasterid != null && courierMasterid != "")
            {
                decLists = courierDeclarationQueryService.GetDeclarationIdsByCourierMasterID(courierMasterid, tenant);
            }
            return decLists;
        }
    }
}

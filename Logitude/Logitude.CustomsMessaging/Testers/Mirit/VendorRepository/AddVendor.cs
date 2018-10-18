using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Testers.Mirit.VendorRepository
{
    public class AddVendor
    {

        public void Test()
        {
            var reqParam = new VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams();
            reqParam.CityName = "TLV";
            reqParam.VendorName = "TST @amital";


            var messageService = new VE_MSG010_VendorInsertUpdateDeleteMessagingService();
            var responseData= messageService.Send(reqParam); 
        }

    }
}



using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class VE_MSG013_VendorAddCommunicationDeviceRequestParams : RequestParamsBase
    {
        public int? VendorNumber { get; set; }
        List<VendorCommunicationResult> communicationDevices;
        public List<VendorCommunicationResult> CommunicationDevices
        {
            get
            {
                if (communicationDevices == null)
                {
                    communicationDevices = new List<VendorCommunicationResult>();
                }
                return communicationDevices;
            }
            set { communicationDevices = value; }
        }
    }
}

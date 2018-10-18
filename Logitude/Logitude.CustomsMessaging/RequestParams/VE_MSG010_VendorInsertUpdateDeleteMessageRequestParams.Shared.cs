using Logitude.Customs.BL.EntityPMs;
using Logitude.CustomsMessaging.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RequestParams
{
    public class VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams : RequestParamsBase
    {
        public enum OperationTypes 
        {
            Add=1,
            Update=2,
            Delete,
        }
        public string VendorNumber { get; set; }
        public string VendorTypeCode { get; set; }
        public string VendorName { get; set; }
        public string CountryCode { get; set; }
        public string SubCountryCode { get; set; }
        public string CityName { get; set; }
        public string MainAddressLine { get; set; }
        public string PostalCode { get; set; }
        public string DunsNumber { get; set; }
        public string VATNumber { get; set; }
        public string StatusCode { get; set; }
        public string TransactionTypeID { get; set; }
        public OperationTypes OperationType { get; set; }
        public Boolean IsAfterWarning { get; set; }

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

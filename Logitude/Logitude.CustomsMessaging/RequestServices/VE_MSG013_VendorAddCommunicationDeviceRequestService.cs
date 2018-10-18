using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VendorAddCommunicationDeviceServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class VE_MSG013_VendorAddCommunicationDeviceRequestService : RequestServiceBase<VE_MSG013_VendorAddCommunicationDevice, VE_MSG013_VendorAddCommunicationDeviceRequestParams>
    {
        public override VE_MSG013_VendorAddCommunicationDevice GetRequest(VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams)
        {
            //Build request 3680 - Add Vendor CommunicationDevice
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var vendorCommunicationQueryService = new VendorCommunicationQueryService(dbContext);

            var myVE_MSG013_VendorAddCommunicationDeviceRequest = new VE_MSG013_VendorAddCommunicationDevice();
            myVE_MSG013_VendorAddCommunicationDeviceRequest.RequestContentHeader = new RequestContentHeader() { Convertor = "1", RecieverID = new int[] { 1 } };
            myVE_MSG013_VendorAddCommunicationDeviceRequest.vendorID = (int)requestParams.VendorNumber;
            myVE_MSG013_VendorAddCommunicationDeviceRequest.CommunicationDevice = GetVendorCommunicationDevice(requestParams);
            myVE_MSG013_VendorAddCommunicationDeviceRequest.Attachment = GetVendorAttachment(requestParams); // to do...

            return myVE_MSG013_VendorAddCommunicationDeviceRequest;
        }

        private CommunicationDevice[] GetVendorCommunicationDevice(VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams)
        {
            var myCommunicationDeviceList = new List<CommunicationDevice>();
            foreach (var communicationItem in requestParams.CommunicationDevices)
            {
                var myCommunicationDevice = new CommunicationDevice();
                myCommunicationDevice.communicationType = communicationItem.CommunicationType;
                myCommunicationDevice.communicationAddress = communicationItem.CommunicationAddress;

                myCommunicationDeviceList.Add(myCommunicationDevice);
            }

            return myCommunicationDeviceList.ToArray();
        }

        private Attachment[] GetVendorAttachment(VE_MSG013_VendorAddCommunicationDeviceRequestParams requestParams)
        {
            /*byte[] myContent = stringToBase64ByteArray("GetApproveChangeTimeListXML()");
            var vendorAttachment = new Attachment[] {
                new Attachment()
            {

                documentType = "380",
                //"לא התקבלו כל שדות המטה-דטא חובה הבאים: : 3,39,55,87 עבור סוג מסמך : 380"
                //"צרופה לא תקינה סוג המסמך : <NULL> שם :  נתוני שדה נוסף : 3 שגויים - הערך : IL אינו מסוג : Int"
                AdditionalData = new AttachmentAdditionalData[] 
                { 
                    new  AttachmentAdditionalData (){fieldID = 3,fieldData="US"  } ,//ארץ חשבון 
                    new  AttachmentAdditionalData (){fieldID = 39,fieldData="12121"} ,///מספר חשבון
                    new  AttachmentAdditionalData (){fieldID = 55,fieldData=DateTime.Now.ToShortDateString()  },//תאריך החשבון
                    new  AttachmentAdditionalData (){fieldID = 87,fieldData=false.ToString()  } ,//האם מסמך מקורי
                },
                fileName = "sdd.txt",

                //documentType = "1",
                attachmentID = "USIGN-1",
                externalAttachmentID = "USIGN-1",
                Remark = "mY Remark ",
                content = myContent

            } };

            return vendorAttachment;*/
            return null;
        }
    }
}

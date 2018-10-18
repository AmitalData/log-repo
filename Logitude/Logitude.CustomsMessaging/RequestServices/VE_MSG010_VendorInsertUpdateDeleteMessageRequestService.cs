using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.VendorInsertUpdateDeleteServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class VE_MSG010_VendorInsertUpdateDeleteMessageRequestService : RequestServiceBase<VE_MSG010_VendorInsertUpdateDeleteMessage, VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams>
    {
        public static byte[] stringToBase64ByteArray(String input)
        {
            byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
            string s = Convert.ToBase64String(ret);
            ret = System.Text.Encoding.Unicode.GetBytes(s);
            return ret;
        }
        public override VE_MSG010_VendorInsertUpdateDeleteMessage GetRequest(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            var requestMessage = new VE_MSG010_VendorInsertUpdateDeleteMessage();
            requestMessage.RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } };
            requestMessage.UpdateType = new VE_MSG010_VendorInsertUpdateDeleteMessageUpdateType() { updateTypeID = (int)requestParams.OperationType };//1=add / 2 ==update
            if (requestParams.OperationType == Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Delete)
            {
                MapVendorWholeDetailsFromDB(ref requestParams);
            }

            requestMessage.VendorWholeDetails = GetVendorWholeDetails(requestParams);
            //requestMessage.Attachment = GetVendorAttachment(requestParams);

            switch (requestParams.OperationType)
            {
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Add:
                    {
                        //requestMessage.updateTypeID = 1;//1=add / 2 ==update
                        requestMessage.UpdateType = new VE_MSG010_VendorInsertUpdateDeleteMessageUpdateType() { updateTypeID = 1 };//1=add / 2 ==update
                    }
                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Update:
                    //requestMessage.updateTypeID = 2;//1=add / 2 ==update
                    requestMessage.UpdateType = new VE_MSG010_VendorInsertUpdateDeleteMessageUpdateType() { updateTypeID = 2 };//1=add / 2 ==update
                    /*
    https://172.22.3.220:443/SaveVE_MSG3670_VendorInsertUpdateDeleteMessage.svc: 
                     * cvc-simple-type 1: element {http://malam.com/customs/Vendors/VE_MSG010_VendorInsertUpdateDeleteMessage}
                     * vendorTypeID value '2442372' is not a valid instance of the element type
                     */

                    break;
                case Logitude.CustomsMessaging.Common.RequestParams.VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams.OperationTypes.Delete:
                    //requestMessage.updateTypeID = 3;//1=add / 2 ==update/ 3 ==Delete
                    requestMessage.UpdateType = new VE_MSG010_VendorInsertUpdateDeleteMessageUpdateType() { updateTypeID = 3 };//1=add / 2 ==update
                    break;
                default:
                    break;
            }

            if (requestParams.IsAfterWarning == true) // Indication whether the user has received the warning
            {
                requestMessage.UpdateType.isAfterWarning = true;
                requestMessage.UpdateType.isAfterWarningSpecified = true;
            }

            return requestMessage;
        }

        private void MapVendorWholeDetailsFromDB(ref VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var vendorQueryService = new CustomsVendorQueryService(dbContext);

            var customsVendorPM = vendorQueryService.GetSingle(requestParams.LoggingEntityId, true, false);
            if (customsVendorPM == null)
            {
                return;
            }

            //Map Vendor details
            requestParams.VendorNumber = customsVendorPM.VendorNumber;
            requestParams.VendorTypeCode = customsVendorPM.VendorTypeCode;
            requestParams.VendorName = customsVendorPM.VendorName;
            requestParams.DunsNumber = customsVendorPM.DunsNumber;
            requestParams.VATNumber = customsVendorPM.VATNumber;

            // Map Vendor Address Details
            requestParams.CountryCode = customsVendorPM.CountryCode;
            requestParams.SubCountryCode = customsVendorPM.SubCountryCode;
            requestParams.CityName = customsVendorPM.CityName;
            requestParams.MainAddressLine = customsVendorPM.MainAddressLine;
            requestParams.PostalCode = customsVendorPM.PostalCode;

            // Map Vendor Communication Details
            requestParams.CommunicationDevices = new List<VendorCommunicationResult>();
            foreach (var vendorCommunication in customsVendorPM.VendorCommunications)
            {
                var device = new VendorCommunicationResult() { CommunicationAddress = vendorCommunication.CommunicationAddress, CommunicationType = vendorCommunication.CommunicationTypeCode };
                requestParams.CommunicationDevices.Add(device);
            }
        }


        private Attachment[] GetVendorAttachment(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            byte[] myContent = stringToBase64ByteArray("GetApproveChangeTimeListXML()");
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
                    new  AttachmentAdditionalData (){fieldID = 55,fieldData=DataTypeConvertorUtil .Convert(DateTime.Now)  },//תאריך החשבון
                    new  AttachmentAdditionalData (){fieldID = 87,fieldData=false.ToString()  } ,//האם מסמך מקורי
                },
                fileName = "sdd.txt",

                //documentType = "1",
                //attachmentID = "USIGN-1",
                //attachmentID = null,
                //externalAttachmentID = "USIGN-1",
                IsAttachment = "true",
                Remark = "mY Remark ",
                content = myContent

            } };
            
            return vendorAttachment;
        }

        private VE_MSG010_VendorInsertUpdateDeleteMessageVendorWholeDetails GetVendorWholeDetails(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            var VendorWholeDetails = new VE_MSG010_VendorInsertUpdateDeleteMessageVendorWholeDetails();

            // Get Vendor details
            int vendorId;
            if (
                int.TryParse(requestParams.VendorNumber, out vendorId) 
                && vendorId != 0
                )
            {
                VendorWholeDetails.vendorID = vendorId;
                VendorWholeDetails.vendorIDSpecified = true;
            }
            int vendorTypeID;
            if (int.TryParse(requestParams.VendorTypeCode, out vendorTypeID))
            {
                VendorWholeDetails.vendorTypeID = vendorTypeID;
            }
            VendorWholeDetails.vendorName = requestParams.VendorName;
            int dunsNumber;
            int.TryParse(requestParams.DunsNumber, out dunsNumber);
            VendorWholeDetails.dunsNumber = dunsNumber;
            if (dunsNumber != 0)
            {
                VendorWholeDetails.dunsNumberSpecified = true;
            }
            VendorWholeDetails.licensedDealerNumber = requestParams.VATNumber; // to check which field?
            VendorWholeDetails.isPalestinian = false;
            
            // Get Vendor Address Details
            VendorWholeDetails.EnglishAdderss = GetEnglishAddress(requestParams);

            // Get Vendor Communication Details
            VendorWholeDetails.CommunicationDevice = GetCommunicationDevice(requestParams);

            return VendorWholeDetails;
        }

        private CommunicationDevice[] GetCommunicationDevice(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            List<CommunicationDevice> devices = new List<CommunicationDevice>();
            foreach (VendorCommunicationResult vendorCommunication in requestParams.CommunicationDevices)
            {
                CommunicationDevice device = new CommunicationDevice() { communicationAddress = vendorCommunication.CommunicationAddress, communicationType = vendorCommunication.CommunicationType };
                devices.Add(device);
            }
            return devices.ToArray();
        }

        private EnglishAddress GetEnglishAddress(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams requestParams)
        {
            var VendorEnglishAdderss = new EnglishAddress();
            VendorEnglishAdderss.englishCountry = requestParams.CountryCode;//"DE";
            VendorEnglishAdderss.englishSubCountry = requestParams.SubCountryCode;
            VendorEnglishAdderss.englishCityName = requestParams.CityName;//"BERLIN";
            VendorEnglishAdderss.englishMainAddressLine = requestParams.MainAddressLine;//"BERLIN 22";
            VendorEnglishAdderss.englishPostalCode = requestParams.PostalCode;//"3322";

            return VendorEnglishAdderss;
        }
    }
}

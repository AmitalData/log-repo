using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class PrivateLabelsBrandingDataService
    {
        //const string PrivateLabelsImageFolderPath = "HybridLabels/HybridLabelsImages";
        const string PrivateLabelsImageExtensionType = "png";

        string imageBrandingData = "";
        string imageBrandingDataRequest = "";
        string[] imagesFields = { "BackgroundImage", "LoginImage", "LoginProgressImage", "ForgetPasswordImage" };
        public PrivateLabelsBrandingData GePrivateLabelsBrandingDataByUrl(PrivateLabelsBrandingDataRequest BrandingDataRequest)
        {
            TenantManagmentPrivateLabelsQuery tenantManagementQuery = new TenantManagmentPrivateLabelsQuery();
            string loggedDomainURL = SecurityUtility.getLoggedDomain();
            TenantManagmentPrivateLabelsPM tenantManagementPM = tenantManagementQuery.GetSingleActivePMByUrl(loggedDomainURL);
            PrivateLabelsBrandingData BrandingData = MapBrandingData(tenantManagementPM, BrandingDataRequest);

            return BrandingData;
        }

        private PrivateLabelsBrandingData MapBrandingData(TenantManagmentPrivateLabelsPM tenantManagementPM, PrivateLabelsBrandingDataRequest BrandingDataRequest)
        {
            PrivateLabelsBrandingData privateBrandingData = null;
            if (tenantManagementPM != null)
            {
                privateBrandingData = new PrivateLabelsBrandingData()
                {
                    Id = tenantManagementPM.Id,
                    PrivateLabelName = tenantManagementPM.PrivateLabelName,
                    PrivateLabelShortName = tenantManagementPM.PrivateLabelShortName,
                    PrivateLabelUrl = tenantManagementPM.PrivateLabelUrl,
                    MainLogo = tenantManagementPM.MainLogo,
                    SmallLogo = tenantManagementPM.SmallLogo,
                    ContactUsEmail = tenantManagementPM.ContactUsEmail,
                    ReceiveAllStatuses = tenantManagementPM.ReceiveAllStatuses,
                    HybridPartnerId = tenantManagementPM.HybridPartnerId,
                    SearchFields = tenantManagementPM.SearchFields,
                    InActive = tenantManagementPM.InActive,
                    Tenant = tenantManagementPM.Tenant,
                    MainColor = tenantManagementPM.MainColor,
                    BackgroundImageId = tenantManagementPM.BackgroundImageId,
                    LoginImageId = tenantManagementPM.LoginImageId,
                    LoginProgressImageId = tenantManagementPM.LoginProgressImageId,
                    ForgetPasswordImageId = tenantManagementPM.ForgetPasswordImageId,
                    SecondaryColor = tenantManagementPM.SecondaryColor,
                };
                SetPrivateLabelsImages(privateBrandingData, BrandingDataRequest);
            }

            return privateBrandingData;
        }
        private void SetPrivateLabelsImages(PrivateLabelsBrandingData privateLabelsBrandingData,
                                            PrivateLabelsBrandingDataRequest privateLabelsBrandingDataRequest)
        {

            // SetBackgroundImageBase64(hybridLabelsBrandingData, hybridLabelsBrandingDataRequest, isFromPrivateSite); 

            foreach (string image in imagesFields)
            {
                switch (image)
                {
                    case "BackgroundImage":
                        imageBrandingData = privateLabelsBrandingData.BackgroundImageId;
                        imageBrandingDataRequest = privateLabelsBrandingDataRequest.BackgroundImageId;
                        break;
                    case "LoginImage":
                        imageBrandingData = privateLabelsBrandingData.LoginImageId;
                        imageBrandingDataRequest = privateLabelsBrandingDataRequest.LoginImageId;
                        break;
                    case "LoginProgressImage":
                        imageBrandingData = privateLabelsBrandingData.LoginProgressImageId;
                        imageBrandingDataRequest = privateLabelsBrandingDataRequest.LoginProgressImageId;
                        break;
                    case "ForgetPasswordImage":
                        imageBrandingData = privateLabelsBrandingData.ForgetPasswordImageId;
                        imageBrandingDataRequest = privateLabelsBrandingDataRequest.ForgetPasswordImageId;
                        break;
                    default:
                        break;
                }

                SetImageBase64(image, privateLabelsBrandingData);

            }

        }

        private void SetImageBase64(string image, PrivateLabelsBrandingData privateLabelsBrandingData)
        {
            if (!string.IsNullOrEmpty(imageBrandingData) &&
                                     imageBrandingDataRequest != imageBrandingData)
            {
                byte[] filedata = GeImageBytesById(imageBrandingData);
                if (filedata != null)
                {
                    setImageBytes(image, privateLabelsBrandingData, filedata);
                }
            }
        }

        private void setImageBytes(string image, PrivateLabelsBrandingData privateLabelsBrandingData, byte[] filedata)
        {
            switch (image)
            {
                case "BackgroundImage":
                    privateLabelsBrandingData.BackgroundImageBytes = filedata;
                    break;
                case "LoginImage":
                    privateLabelsBrandingData.LoginImageBytes = filedata;
                    break;
                case "LoginProgressImage":
                    privateLabelsBrandingData.LoginProgressImageBytes = filedata;
                    break;
                case "ForgetPasswordImage":
                    privateLabelsBrandingData.ForgetPasswordImageBytes = filedata;
                    break;
                default:
                    break;
            }
        }
          
        private byte[] GeImageBytesById(string ImageId)
        {
            byte[] imageBytes = null; 
            if (imageBytes == null || imageBytes.Length == 0)
            {
                Uploader uploaderService = new Uploader();
                imageBytes = uploaderService.DownloadFile(ImageId, PrivateLabelsImageExtensionType, "images", 0);
            }
            return imageBytes;
        }
  

        // private void SetBackgroundImageBase64(HybridLabelsBrandingData hybridBrandingData,
        //                                       HybridLabelsBrandingDataRequest hybridBrandingDataRequest,
        //                                       bool isFromPrivateSite)
        // {
        //     if (!string.IsNullOrEmpty(hybridBrandingData.BackgroundImageId) &&
        //                               hybridBrandingDataRequest.BackgroundImageId != hybridBrandingData.BackgroundImageId &&
        //                               !isFromPrivateSite)
        //     {
        //        byte[] filedata = GeImageBytesById(hybridBrandingData.BackgroundImageId);
        //         if (filedata != null)
        //         {
        //             hybridBrandingData.BackgroundImageBytes = filedata;
        //         }
        //     }
        // }
         
    }
}
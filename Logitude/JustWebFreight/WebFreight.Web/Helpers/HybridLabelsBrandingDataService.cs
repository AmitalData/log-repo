using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class HybridLabelsBrandingDataService
    {
        //const string PrivateLabelsImageFolderPath = "HybridLabels/HybridLabelsImages";
        const string PrivateLabelsImageExtensionType = "png";

        string imageBrandingData = "";
        string imageBrandingDataRequest = "";
        string[] imagesFields = { "BackgroundImage", "MainImage", "LoginProgressImage", "ForgetPasswordImage" };
        public HybridLabelsBrandingData GeHybridLabelsBrandingData(HybridLabelsBrandingDataRequest BrandingDataRequest, bool isFromPrivateSite = false)
        {

            TenantManagmentPrivateLabelsQuery tenantManagementQuery = new TenantManagmentPrivateLabelsQuery();
            TenantManagmentPrivateLabelsPM tenantManagementPM = tenantManagementQuery.GetSinglePM(BrandingDataRequest.Id);
            HybridLabelsBrandingData BrandingData = MapBrandingData(tenantManagementPM, BrandingDataRequest, isFromPrivateSite);

            return BrandingData;
        }

        private HybridLabelsBrandingData MapBrandingData(TenantManagmentPrivateLabelsPM tenantManagementPM, HybridLabelsBrandingDataRequest BrandingDataRequest, bool isFromPrivateSite)
        {
            HybridLabelsBrandingData hybridBrandingData = null;
            if (tenantManagementPM != null)
            {
                hybridBrandingData = new HybridLabelsBrandingData()
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
                    MainImageId = tenantManagementPM.MainImageId,
                    LoginProgressImageId = tenantManagementPM.LoginProgressImageId,
                    ForgetPasswordImageId = tenantManagementPM.ForgetPasswordImageId,
                };
                SetPrivateLabelsImages(hybridBrandingData, BrandingDataRequest, isFromPrivateSite);
            }

            return hybridBrandingData;
        }
        private void SetPrivateLabelsImages(HybridLabelsBrandingData hybridLabelsBrandingData,
                                            HybridLabelsBrandingDataRequest hybridLabelsBrandingDataRequest,
                                            bool isFromPrivateSite)
        {

            // SetBackgroundImageBase64(hybridLabelsBrandingData, hybridLabelsBrandingDataRequest, isFromPrivateSite); 

            foreach (string image in imagesFields)
            {
                switch (image)
                {
                    case "BackgroundImage":
                        imageBrandingData = hybridLabelsBrandingData.BackgroundImageId;
                        imageBrandingDataRequest = hybridLabelsBrandingDataRequest.BackgroundImageId;
                        break;
                    case "MainImage":
                        imageBrandingData = hybridLabelsBrandingData.MainImageId;
                        imageBrandingDataRequest = hybridLabelsBrandingDataRequest.MainImageId;
                        break;
                    case "LoginProgressImage":
                        imageBrandingData = hybridLabelsBrandingData.LoginProgressImageId;
                        imageBrandingDataRequest = hybridLabelsBrandingDataRequest.LoginProgressImageId;
                        break;
                    case "ForgetPasswordImage":
                        imageBrandingData = hybridLabelsBrandingData.ForgetPasswordImageId;
                        imageBrandingDataRequest = hybridLabelsBrandingDataRequest.ForgetPasswordImageId;
                        break;
                    default:
                        break;
                }

                SetImageBase64(image, hybridLabelsBrandingData);

            }

        }

        private void SetImageBase64(string image, HybridLabelsBrandingData hybridLabelsBrandingData)
        {
            if (!string.IsNullOrEmpty(imageBrandingData) &&
                                     imageBrandingDataRequest != imageBrandingData)
            {
                byte[] filedata = GeImageBytesById(imageBrandingData);
                if (filedata != null)
                {
                    setImageBytes(image, hybridLabelsBrandingData, filedata);
                }
            }
        }

        private void setImageBytes(string image, HybridLabelsBrandingData hybridLabelsBrandingData, byte[] filedata)
        {
            switch (image)
            {
                case "BackgroundImage":
                    hybridLabelsBrandingData.BackgroundImageBytes = filedata;
                    break;
                case "MainImage":
                    hybridLabelsBrandingData.MainImageBytes = filedata;
                    break;
                case "LoginProgressImage":
                    hybridLabelsBrandingData.LoginProgressImageBytes = filedata;
                    break;
                case "ForgetPasswordImage":
                    hybridLabelsBrandingData.ForgetPasswordImageBytes = filedata;
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
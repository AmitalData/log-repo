using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using System;
using WebFreight.Web.DataContracts;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class BrandingHelper
    {
        const string brandingImageExtensionType = "png";
        public BrandingData GetBrandingDataByDomain(string domain)
        {
            BrandingData brandingData = new BrandingData();
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetTenantBrandingDataByDomain(domain);

            if (tenantManagementPM == null)
            {
                return null;
            }

            brandingData.MainColor = string.IsNullOrEmpty(tenantManagementPM.MainColor)
                         ? "rgba(210,23,69,1)"
                         : tenantManagementPM.MainColor;
            brandingData.SecondaryColor = string.IsNullOrEmpty(tenantManagementPM.SecondaryColor)
                                          ? "rgba(23,105,170,1)"
                                          : tenantManagementPM.SecondaryColor;
            brandingData.TertiaryColor = string.IsNullOrEmpty(tenantManagementPM.TertiaryColor)
                                         ? "rgba(255,255,255,1)"
                                         : tenantManagementPM.TertiaryColor;

            if (!tenantManagementPM.EnableBranding)
            {
                brandingData.Tenant = tenantManagementPM.Id;
                brandingData.ComapnylogoId = tenantManagementPM.ComapnylogoId;
                brandingData.Email = tenantManagementPM.ContactEmail;
                SetBrandingImagesBytes(brandingData);
                return brandingData;
            }

            brandingData.Tenant = tenantManagementPM.Id;
            brandingData.BackgroundId = tenantManagementPM.BackgroundId;
            brandingData.MobileBackgroundId = tenantManagementPM.MobileBackgroundId;
            brandingData.BrowserIconId = tenantManagementPM.BrowserIconId;
            brandingData.ComapnylogoId = tenantManagementPM.ComapnylogoId;
            brandingData.InvertedLogoId = tenantManagementPM.InvertedLogoId;
            brandingData.CustomerURL = tenantManagementPM.CustomerURL;
            brandingData.ActivatePrivateSite = tenantManagementPM.ActivatePrivateSite;
            brandingData.EnableExportToExcel = tenantManagementPM.EnableExportToExcel;
            brandingData.Email = tenantManagementPM.ContactEmail;
            SetBrandingImagesBytes(brandingData);
            return brandingData;
        }

        private void SetBrandingImagesBytes(BrandingData brandingData)
        {
            brandingData.BackgroundBytes = SetImageBase64(brandingData.BackgroundId);
            brandingData.MobileBackgroundBytes = SetImageBase64(brandingData.MobileBackgroundId);
            var comapnylogoBytes = SetImageBase64(brandingData.ComapnylogoId);
            brandingData.ComapnylogoBytes = comapnylogoBytes == null ? this.GetTenantLogo(brandingData.Tenant) : comapnylogoBytes;
            brandingData.InvertedLogoBytes = SetImageBase64(brandingData.InvertedLogoId);
            brandingData.BrowserIconBytes = SetImageBase64(brandingData.BrowserIconId);
        }

        private byte[] GetTenantLogo(int tenant)
        {
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = "logo" + tenant,
                FolderName = "logos",
                Extension = "jpg",
                Tenant = tenant,
            };

            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);
            return datainByte;
        }

        private byte[] SetImageBase64(string backgroundId)
        {
            if (string.IsNullOrEmpty(backgroundId))
            {
                return null;
            }
            byte[] imageBytes = GeImageBytesById(backgroundId);
            return imageBytes;
        }
 
        private byte[] GeImageBytesById(string ImageId)
        {
            byte[] imageBytes = null;
            if (imageBytes == null || imageBytes.Length == 0)
            {
                Uploader uploaderService = new Uploader();
                imageBytes = uploaderService.DownloadFile(ImageId, brandingImageExtensionType, "images", 0);
            }
            return imageBytes;
        }
    }
}
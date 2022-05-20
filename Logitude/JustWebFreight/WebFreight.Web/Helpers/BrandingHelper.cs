using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.Helpers
{
    public class BrandingHelper
    {
        const string brandingImageExtensionType = "png";
        public BrandingData GetBrandingDataByTenant(int tenant)
        {
            BrandingData brandingData = new BrandingData();
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(tenant);

            if (tenantManagementPM == null)
            {
                return null;
            }

            brandingData = new BrandingData()
            {
                Tenant = tenantManagementPM.Id,
                MainColor = tenantManagementPM.MainColor,
                SecondaryColor = tenantManagementPM.SecondaryColor,
                BackgroundId = tenantManagementPM.BackgroundId,
                BrowserIconId = tenantManagementPM.BrowserIconId,
                ComapnylogoId = tenantManagementPM.ComapnylogoId,
                InvertedLogoId = tenantManagementPM.InvertedLogoId,
                CustomerURL = tenantManagementPM.CustomerURL,
                ActivatePrivateSite = tenantManagementPM.ActivatePrivateSite,
            };

            SetBrandingImagesBytes(brandingData);


            return brandingData;
        }

        private void SetBrandingImagesBytes(BrandingData brandingData)
        {
            brandingData.BackgroundBytes = SetImageBase64(brandingData.BackgroundId);
            brandingData.ComapnylogoBytes = SetImageBase64(brandingData.ComapnylogoId);
            brandingData.InvertedLogoBytes = SetImageBase64(brandingData.InvertedLogoId);
            brandingData.BrowserIconBytes = SetImageBase64(brandingData.BrowserIconId);
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
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
    public class CargoTrackingHelper
    {
        const string CargoTrackingImageFolder = "CargoTrackingImages";
        const string CargoTrackingImageExtensionType = "jpg";


        public CargoTrackingBrandingData GetCargoTrackingBrandingDataByDomain(string domain)
        {

            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePMByDomain(domain);
            CargoTrackingBrandingData BrandingData = MapBrandingDataByTenantManagement(tenantManagementPM);

            return BrandingData;
        }

        public string SetBrandingLogo(TenantManagementPM tenantManagement)
        {
            Uploader uploaderService = new Uploader();
            byte[] logodata = uploaderService.DownloadFile("sharedLogtsitcslogo" + tenantManagement.Id, "png", "logos", tenantManagement.Id);
            if (logodata != null)
            {
                return "data:image/" + "jpg" + ";base64," + Convert.ToBase64String(logodata);
            }
            else return null;
        }

        private CargoTrackingBrandingData MapBrandingDataByTenantManagement(TenantManagementPM tenantManagementPM)
        {
            CargoTrackingBrandingData cargoTrackingBrandingData = null;
            if (tenantManagementPM != null)
            {
                cargoTrackingBrandingData = new CargoTrackingBrandingData()
                {
                    Tenant = tenantManagementPM.Id,
                    MainColor = tenantManagementPM.MainColor,
                    SecondaryColor = tenantManagementPM.SecondaryColor,
                    BackgroundId = tenantManagementPM.BackgroundId,
                    Logo = SetBrandingLogo(tenantManagementPM)
                };
                SetBackgroundImageBase64(cargoTrackingBrandingData);
            }

            return cargoTrackingBrandingData;
        }

        private void SetBackgroundImageBase64(CargoTrackingBrandingData cargoTrackingBrandingData)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.BackgroundId))
            {
                byte[] filedata = GetBackgroundImageBytes(cargoTrackingBrandingData.BackgroundId);
                if (filedata != null)
                {
                    string base64StringData = Convert.ToBase64String(filedata);
                    //cargoTrackingBrandingData.BackgroundImg = "data:image/" + CargoTrackingImageExtensionType + ";base64," + base64StringData;
                    string imagePath = GetFilePath(GetFileNameWithExtension(cargoTrackingBrandingData.BackgroundId));
                    SaveImageOnCargoTrackingImagesIfNotExisit(base64StringData, imagePath);
                    cargoTrackingBrandingData.BackgroundURL = GetFileURL(cargoTrackingBrandingData.BackgroundId);
                }
            }

        }

        private string GetFilePath(string fileName)
        {
            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/" + CargoTrackingImageFolder + "/");
            CreateDirectoryIfNotExist(folderPath);
            string filePath = folderPath + fileName;
            return filePath;
        }
        private void CreateDirectoryIfNotExist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        private byte[] GetBackgroundImageBytes(string backgroundId)
        {
            byte[] imageBytes = GetImageBytesFromCargoTrackingImages(backgroundId);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                Uploader uploaderService = new Uploader();
                imageBytes = uploaderService.DownloadFile(backgroundId, CargoTrackingImageExtensionType, "images", 0);
            }
            return imageBytes;
        }

        private void SaveImageOnCargoTrackingImagesIfNotExisit(string base64StringData, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                byte[] daata = System.Convert.FromBase64String(base64StringData);
                MemoryStream ms = new MemoryStream(daata);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private byte[] GetImageBytesFromCargoTrackingImages(string imgId)
        {
            byte[] imageBytes = null;
            string imagePath = GetFilePath(GetFileNameWithExtension(imgId));
            if (File.Exists(imagePath))
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
                MemoryStream m = new MemoryStream();
                image.Save(m, image.RawFormat);
                imageBytes = m.ToArray();
            }
            return imageBytes;
        }
 
        private string GetFileNameWithExtension(string imgName)
        {
            return imgName + "." + CargoTrackingImageExtensionType;
        }
        private string GetFileURL(string imgName)
        {
            String FileURL = CargoTrackingImageFolder + "/" + GetFileNameWithExtension(imgName);
            return FileURL;
        }

      

    }
}
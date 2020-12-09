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
        const string CargoTrackingImageFolderPath = "CargoTracking/CargoTrackingImages";
        const string CargoTrackingImageExtensionType = "png";


        public CargoTrackingBrandingData GetCargoTrackingBrandingDataByDomain(string domain,bool isFromPrivateSite=false)
        {

            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePMByDomain(domain);
            CargoTrackingBrandingData BrandingData = MapBrandingDataByTenantManagement(tenantManagementPM, isFromPrivateSite);

            return BrandingData;
        }
 
        private CargoTrackingBrandingData MapBrandingDataByTenantManagement(TenantManagementPM tenantManagementPM,bool isFromPrivateSite)
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
                    BrowserIconId = tenantManagementPM.BrowserIconId,
                    ComapnylogoId = tenantManagementPM.ComapnylogoId,
                    CustomerURL = tenantManagementPM.CustomerURL,
                };
                SetCargoTrackingImages(cargoTrackingBrandingData, isFromPrivateSite);
            }

            return cargoTrackingBrandingData;
        }
        private void SetCargoTrackingImages(CargoTrackingBrandingData cargoTrackingBrandingData, bool isFromPrivateSite)
        {
            SetBackgroundImageBase64(cargoTrackingBrandingData, isFromPrivateSite);
            SetComapnyLogoBase64(cargoTrackingBrandingData);
            SetBrowserIconBase64(cargoTrackingBrandingData);
        }
        private void SetBackgroundImageBase64(CargoTrackingBrandingData cargoTrackingBrandingData, bool isFromPrivateSite)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.BackgroundId) && !isFromPrivateSite)
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.BackgroundId);
                if (filedata != null)
                {
                    string base64StringData = Convert.ToBase64String(filedata);
                    cargoTrackingBrandingData.BackgroundImg = "data:image/" + CargoTrackingImageExtensionType + ";base64," + base64StringData;
                    //SaveImageOnCargoTrackingImagesIfNotExisit(base64StringData, cargoTrackingBrandingData.BackgroundId);
                    //cargoTrackingBrandingData.BackgroundURL = GetFileURL(cargoTrackingBrandingData.BackgroundId);
                }
            }

        }

        private void SetComapnyLogoBase64(CargoTrackingBrandingData cargoTrackingBrandingData)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.ComapnylogoId))
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.ComapnylogoId);
                if (filedata != null)
                {
                    string base64StringData = Convert.ToBase64String(filedata);
                    cargoTrackingBrandingData.ComapnylogoImg = "data:image/" + CargoTrackingImageExtensionType + ";base64," + base64StringData;
                    //SaveImageOnCargoTrackingImagesIfNotExisit(base64StringData, cargoTrackingBrandingData.ComapnylogoId);
                    //cargoTrackingBrandingData.ComapnylogoURL = GetFileURL(cargoTrackingBrandingData.ComapnylogoId);
                }
            }

        }

        private void SetBrowserIconBase64(CargoTrackingBrandingData cargoTrackingBrandingData)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.BrowserIconId))
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.BrowserIconId);
                if (filedata != null)
                {
                    string base64StringData = Convert.ToBase64String(filedata);
                    cargoTrackingBrandingData.BrowserIconImg = "data:image/" + CargoTrackingImageExtensionType + ";base64," + base64StringData;
                    //SaveImageOnCargoTrackingImagesIfNotExisit(base64StringData, cargoTrackingBrandingData.BrowserIconId);
                    //cargoTrackingBrandingData.BrowserIconURL = GetFileURL(cargoTrackingBrandingData.BrowserIconId);
                }
            }

        }

        private string GetFilePath(string fileName)
        {
            string folderPath = CreateFoldersFromPathIfNotExist();
            string filePath = folderPath + fileName;
            return filePath;
        }

        private string CreateFoldersFromPathIfNotExist()
        {
            string[] AllFolders = CargoTrackingImageFolderPath.Split('/');
            string FullPath = null;
            foreach (string Folder  in AllFolders)
            {
                string folderPath = CargoTrackingImageFolderPath.Substring(0, CargoTrackingImageFolderPath.IndexOf(Folder) + Folder.Length);
                FullPath = System.Web.HttpContext.Current.Server.MapPath("~/" + folderPath + "/");
                CreateDirectoryIfNotExist(FullPath);
            }
            return FullPath;
        }
        private void CreateDirectoryIfNotExist(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }
        private byte[] GeImageBytesById(string ImageId)
        {
            byte[] imageBytes = GetImageBytesFromCargoTrackingImages(ImageId);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                Uploader uploaderService = new Uploader();
                imageBytes = uploaderService.DownloadFile(ImageId, CargoTrackingImageExtensionType, "images", 0);
            }
            return imageBytes;
        }

        private void SaveImageOnCargoTrackingImagesIfNotExisit(string base64StringData, string imgId)
        {
            string imagePath = GetFilePath(GetFileNameWithExtension(imgId));
            if (!File.Exists(imagePath))
            {
                byte[] data = System.Convert.FromBase64String(base64StringData);
                MemoryStream ms = new MemoryStream(data);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
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
            String FileURL = CargoTrackingImageFolderPath + "/" + GetFileNameWithExtension(imgName);
            return FileURL;
        }

      

    }
}
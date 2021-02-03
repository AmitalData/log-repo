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
 
        

        public CargoTrackingBrandingData GetCargoTrackingBrandingDataByDomain(CargoTrackingBrandingDataRequest BrandingDataRequest, bool isFromPrivateSite=false)
        {

            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePMByDomain(BrandingDataRequest.Domain);
            CargoTrackingBrandingData BrandingData = MapBrandingDataByTenantManagement(tenantManagementPM, BrandingDataRequest, isFromPrivateSite);

            return BrandingData;
        }
 
        private CargoTrackingBrandingData MapBrandingDataByTenantManagement(TenantManagementPM tenantManagementPM, CargoTrackingBrandingDataRequest BrandingDataRequest, bool isFromPrivateSite)
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
                    ShipmentHeaderImageId = tenantManagementPM.ShipmentHeaderImageId,
                    CustomerURL = tenantManagementPM.CustomerURL,
                };
                SetCargoTrackingImages(cargoTrackingBrandingData, BrandingDataRequest, isFromPrivateSite);
            }

            return cargoTrackingBrandingData;
        }
        private void SetCargoTrackingImages(CargoTrackingBrandingData cargoTrackingBrandingData, 
                                            CargoTrackingBrandingDataRequest BrandingDataRequest, 
                                            bool isFromPrivateSite)
        {
            SetBackgroundImageBase64(cargoTrackingBrandingData, BrandingDataRequest, isFromPrivateSite);
            SetComapnyLogoBase64(cargoTrackingBrandingData, BrandingDataRequest);
            SetBrowserIconBase64(cargoTrackingBrandingData, BrandingDataRequest);
            SetShipmentHeaderImageBase64(cargoTrackingBrandingData, BrandingDataRequest);
        }
        private void SetBackgroundImageBase64(CargoTrackingBrandingData cargoTrackingBrandingData, 
                                              CargoTrackingBrandingDataRequest BrandingDataRequest,
                                              bool isFromPrivateSite)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.BackgroundId)&& 
                                      BrandingDataRequest.BackgroundId!= cargoTrackingBrandingData.BackgroundId && 
                                      !isFromPrivateSite)
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.BackgroundId);
                if (filedata != null)
                {
                    cargoTrackingBrandingData.BackgroundBytes = filedata;
                }
            }

        }

        private void SetComapnyLogoBase64(CargoTrackingBrandingData cargoTrackingBrandingData, 
                                          CargoTrackingBrandingDataRequest BrandingDataRequest)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.ComapnylogoId) && 
                                      BrandingDataRequest.ComapnylogoId != cargoTrackingBrandingData.ComapnylogoId)
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.ComapnylogoId);
                if (filedata != null)
                {
                    cargoTrackingBrandingData.ComapnylogoBytes = filedata;
                }
            }

        }

        private void SetBrowserIconBase64(CargoTrackingBrandingData cargoTrackingBrandingData, 
                                          CargoTrackingBrandingDataRequest BrandingDataRequest)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.BrowserIconId) && 
                                      BrandingDataRequest.BrowserIconId != cargoTrackingBrandingData.BrowserIconId)
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.BrowserIconId);
                if (filedata != null)
                {
                    cargoTrackingBrandingData.BrowserIconBytes = filedata;
                }
            }

        }

        private void SetShipmentHeaderImageBase64(CargoTrackingBrandingData cargoTrackingBrandingData,
                                      CargoTrackingBrandingDataRequest BrandingDataRequest)
        {
            if (!string.IsNullOrEmpty(cargoTrackingBrandingData.ShipmentHeaderImageId) &&
                                      BrandingDataRequest.ShipmentHeaderImageId != cargoTrackingBrandingData.ShipmentHeaderImageId)
            {
                byte[] filedata = GeImageBytesById(cargoTrackingBrandingData.ShipmentHeaderImageId);
                if (filedata != null)
                {
                    cargoTrackingBrandingData.ShipmentHeaderBytes = filedata;
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
            byte[] imageBytes =   null;// GetImageBytesFromCargoTrackingImages(ImageId);
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
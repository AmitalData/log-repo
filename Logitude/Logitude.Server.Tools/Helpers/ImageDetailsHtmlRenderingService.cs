using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class ImageDetailsHtmlRenderingService
    {
        public string Render(string imageDetailsId, int tenant)
        {
            string htmlImage = string.Empty;
            StorageDataArgs storageDataArgs = GetStorageDataArgs(imageDetailsId, tenant);
            byte[] imageByte = StorageDataService.ReadFileFromStorage(storageDataArgs);
            if (imageByte != null)
            {
                string base64String = "data:image/" + storageDataArgs.FileName + "." + storageDataArgs.Extension + ";base64," + Convert.ToBase64String(imageByte, 0, imageByte.Length);
                htmlImage = "<img " + GetImageHtmlStyle(imageByte) + " src='" + base64String + "' />";
            }
            return htmlImage;
        }

        private string GetImageHtmlStyle(byte[]  imageByte)
        {
            Image image = ConvertImageByteToImage(imageByte);
            return "style = '"  + "Height:" + image.Height.ToString() + "px" + ";Width:" + image.Width.ToString() + "px" + "'";
        }

        private  Image ConvertImageByteToImage(byte[] imageByte)
        {
            MemoryStream ms = new MemoryStream(imageByte, 0, imageByte.Length);
            ms.Write(imageByte, 0, imageByte.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        private  StorageDataArgs GetStorageDataArgs(string signatureImageId, int tenant)
        {
            ImageDetailRepository imageDetailsRepository = new ImageDetailRepository(tenant);
            ImageDetail imageDetail = imageDetailsRepository.GetSingleImageDetail(signatureImageId, tenant);
            return new StorageDataArgs()
            {
                FileName = signatureImageId,
                FolderName = "images",
                Tenant = tenant,
                Extension = imageDetail != null ? imageDetail.Extension : "",

            };
        }


    }
}
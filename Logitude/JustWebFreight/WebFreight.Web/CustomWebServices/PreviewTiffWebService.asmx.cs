using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for PreviewTiffWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PreviewTiffWebService : System.Web.Services.WebService
    {

       
        [WebMethod]
        public byte[] GetTiffFile(string documentId,int tenant)
        {
            Uploader up = new Uploader();
            byte[] _DatainByte=null;
            string documentExtension = up.GetFileExtension(documentId, tenant);

            if (!string.IsNullOrEmpty(documentExtension) && (documentExtension =="tiff" || documentExtension=="tif"))
            {
                _DatainByte = up.DownloadFile(documentId, documentExtension, "", tenant);
            }
            if (_DatainByte != null)
            {
                MemoryStream st = new MemoryStream(_DatainByte);
                MemoryStream imageSt = new MemoryStream();
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(st);
             
                bmp.Save(imageSt, System.Drawing.Imaging.ImageFormat.Png);
                return imageSt.ToArray();
            }
            return null;
        }

        [WebMethod]
        public byte[] GetPageTiffAsB64FromTarByTenantComIdPage(
           string documentId, int tenant, int currPage,
           out string TiffPageLines, out string ErrorMessage)
        {
            Uploader up = new Uploader();
            byte[] _DatainByte = null;
            _DatainByte = up.GetPageTiffAsB64FromTarByTenantComIdPage(documentId, tenant, currPage, out TiffPageLines,out ErrorMessage);
            if (_DatainByte != null)
            {
                return BinaryImageToSerializeListBytes(_DatainByte);
            }

            return null;
        }

         [WebMethod]
        public byte[] GetMultiFrameTiffFile(string documentId, int tenant)
        {
            Uploader up = new Uploader();
            byte[] _DatainByte = null;
            string documentExtension = up.GetFileExtension(documentId, tenant);

            if (!string.IsNullOrEmpty(documentExtension) && (documentExtension == "tiff" || documentExtension == "tif"))
            {
                _DatainByte = up.DownloadFile(documentId, documentExtension, "", tenant);
            }
            if (_DatainByte != null)
            {
                return BinaryImageToSerializeListBytes(_DatainByte);
            }



            return null;
        }

         private static byte[] BinaryImageToSerializeListBytes(byte[] _DatainByte)
         {
             MemoryStream st = new MemoryStream(_DatainByte);
             List<byte[]> images = new List<byte[]>();

             System.Drawing.Bitmap bmp = (Bitmap)Image.FromStream(st);
             int count = bmp.GetFrameCount(FrameDimension.Page);
             for (int idx = 0; idx < count; idx++)
             {

                 bmp.SelectActiveFrame(FrameDimension.Page, idx);
                 MemoryStream byteStream = new MemoryStream();
                 bmp.Save(byteStream, ImageFormat.Tiff);
                 Bitmap imageBit = new Bitmap(byteStream);
                 MemoryStream imageSt = new MemoryStream();
                 imageBit.Save(imageSt, System.Drawing.Imaging.ImageFormat.Png);
                 images.Add(imageSt.ToArray());

             }


             XmlSerializer serializer = new XmlSerializer(typeof(List<byte[]>));
             MemoryStream memstream = new MemoryStream();
             serializer.Serialize(memstream, images);
             memstream.Seek(0, SeekOrigin.Begin);
             var reader = new StreamReader(memstream);
             string content = reader.ReadToEnd();
             byte[] bytearray = memstream.ToArray();
             return bytearray;
         }
    }
}

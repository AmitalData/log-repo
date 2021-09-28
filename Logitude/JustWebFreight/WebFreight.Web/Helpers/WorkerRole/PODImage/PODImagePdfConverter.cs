using EvoPdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.PODImage
{
    public class PODImagePdfConverter : IPODImageConverter
    {
        private Document pdfDocument;
        private PdfPage pdfPage;
        private string evoLicenseKey = "fvDj8eTh8eDg4vHk/+Hx4uD/4OP/6Ojo6A==";
        public string Extention { get { return "pdf"; } }

        public PODImagePdfConverter()
        {
            InitializeEvoPdf();
        }

        private void InitializeEvoPdf()
        {
            pdfDocument = new Document();
            pdfPage = pdfDocument.AddPage();
            pdfDocument.LicenseKey = evoLicenseKey;
        }
        

        public byte[] Convert(byte[] imageData)
        {
            if (imageData == null) return null;
            ImageElement imageElement = CreateImageElement(imageData);
            AddElementResult addElementResult = pdfPage.AddElement(imageElement);
            return pdfDocument.Save();
        }

        private ImageElement CreateImageElement(byte[] fileData)
        {
            var image = ConvertByteArrayToImage(fileData);
            var pdfWidth = pdfPage.PageSize.Width;
            var pdfHeight = pdfPage.PageSize.Height;
            float imageWidth = image.Width > pdfWidth ? pdfWidth : image.Width;
            float imageHeight = image.Height > pdfHeight ? pdfHeight : image.Height;
            ImageElement unscaledImageElement = new ImageElement(0, 0, imageWidth, imageHeight, image);
            return unscaledImageElement;
        }

        private Image ConvertByteArrayToImage(byte[] byteData)
        {
            MemoryStream ms = new MemoryStream(byteData, 0, byteData.Length);     
            ms.Write(byteData, 0, byteData.Length);
            return System.Drawing.Image.FromStream(ms, true);

        }
    }
}
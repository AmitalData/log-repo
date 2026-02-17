using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using Microsoft.WindowsAzure.Storage;

using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;

using WebFreight.Web.Azure;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for XapFilesStorageService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class XapFilesStorageService : System.Web.Services.WebService
    {
        CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer("xapflesblob");

        private List<string> xapFilesList
        {
            get
            {
                bool iscustoms = LogitudeSettings.WorkEnvironment == "customs";//bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsCustomsMode"));

                if (!iscustoms)
                {
                    return new List<string>()
                    {
            "Simplog.SilverlightBaseControlsLib.xap",
            "Simplog.FreightLib.xap",
            "Simplog.InvoiceLib.xap",
            "Simplog.QuoteLib.xap",
            "Simplog.ShipmentLib.xap",
            "ViewInjectionImplementation.xap",
            "Simplog.StimulsoftLib.xap",
            "Simplog.TelerikLib.xap",
            "Simplog.Reporting.xap",
            "SharedLogistics.xap",
            "Logitude.Reports.xap",
            //"Logitude.Customs.xap",
            "Logitude.CRM.xap",
            "Logitude.Social.xap",
            "Logitude.BookingLib.xap",
            "Logitude.Accounting.xap",
                        "Logitude.DashBoard.xap",
                        "Logitude.CommonLib.xap",
                        "Simplog.InfrastructureExt.xap",
                        //"Logitude.ApplicationThemes.xap",
                    };
                }
                else
                {
                    return new List<string>()
                    { 
                         "Simplog.SilverlightBaseControlsLib.xap",
                         "Simplog.FreightLib.xap",  
                         //"Simplog.InvoiceLib.xap",
                        // "Simplog.QuoteLib.xap",
                        // "Simplog.ShipmentLib.xap",
                         "ViewInjectionImplementation.xap",
                         "Simplog.StimulsoftLib.xap",
                         "Simplog.TelerikLib.xap",
                         "Simplog.Reporting.xap",
                        // "SharedLogistics.xap",
                       //  "Logitude.Reports.xap",
                         "Logitude.Customs.xap",
                          "Logitude.CommonLib.xap",
                        "Simplog.InfrastructureExt.xap",
                         //"Logitude.CRM.xap",
                         //"Logitude.Social.xap",
        };
                }
            }
        }

        [WebMethod]
        public string GetXapFilesDetails()
        {

            System.Xml.Linq.XElement detailsxml = new System.Xml.Linq.XElement("Xap_Details");
            foreach (string xapfile in xapFilesList)
            {
                DateTime lastmodified = new DateTime();
                string xapfilename = xapfile.ToLower();
                long xapfilesize = 0;
                if (
                    //!Simplog.Server.Infrastructure.Helpers.SettingUtil.ForceDownloadXapFromIIS() &&
                    !LogitudeSettings.IsCostomsDeploy &&
                    LogitudeSettings.DeploymentStage != "Dev" && LogitudeSettings.UsingAzure)
                {
                    var blobfile = blobContainer.GetBlockBlobReference(xapfilename);
                    if (blobfile.Exists())
                    {
                        using (MemoryStream memstream = new MemoryStream())
                        {
                            blobfile.DownloadToStream(memstream);
                        }

                        DateTimeOffset? dateTikeoffset = blobfile.Properties.LastModified;
                        lastmodified = dateTikeoffset.Value.DateTime;

                        xapfilesize = blobfile.Properties.Length;
                    }
                }
                else
                {
                    string xapPhysicalPath = Server.MapPath(@"ClientBin/" + xapfilename);
                    DateTime lastWrite = System.IO.File.GetLastWriteTime(xapPhysicalPath);

                    lastmodified = lastWrite;

                    FileStream fs = File.OpenRead(xapPhysicalPath);

                    xapfilesize = fs.Length;

                }

              
                System.Xml.Linq.XElement xapdetail = new System.Xml.Linq.XElement("XapDetail");

                System.Xml.Linq.XElement namenode = new System.Xml.Linq.XElement("Name");
                namenode.Value = xapfilename;
              
                System.Xml.Linq.XElement datenode = new System.Xml.Linq.XElement("CreateDate");
                datenode.Value = lastmodified.ToString();
              
                System.Xml.Linq.XElement sizenode = new System.Xml.Linq.XElement("Size");
                sizenode.Value = xapfilesize.ToString();
                

                xapdetail.Add(namenode);
                xapdetail.Add(datenode);
                xapdetail.Add(sizenode);

                detailsxml.Add(xapdetail);


                 
            }


            return detailsxml.ToString();
           

        }


        [WebMethod]
        public void UpdateToAzureStorageXapFiles()
        {
            foreach (string xapfile in xapFilesList)
            {

                string xapfilename = xapfile.ToLower();
                var blobfile = blobContainer.GetBlockBlobReference(xapfilename);
                blobfile.DeleteIfExists();

                UploadXapFile(xapfilename);


            }
        }

    

        private byte[] UploadXapFile(string filename)
        {
            filename = filename.ToLower();
            byte[] buffer = GetFileFromServer(filename);
             
            //temp file ( to be deleted when upload done)
            CloudBlockBlob tempcloudBlob = blobContainer.GetBlockBlobReference(filename);


            MemoryStream memorystream = new MemoryStream(buffer);

            tempcloudBlob.UploadFromStream(memorystream);

            return buffer;
        }



      

        public byte[] GetFileFromServer(string filename)
        {

            string path = Server.MapPath(".");
            path += "\\ClientBin\\";
            path += filename;


            FileStream fs = File.OpenRead(path);
            byte[] resultFile = new byte[fs.Length];
            fs.Read(resultFile, 0, resultFile.Length);
            fs.Close();

            return resultFile;

        }

        //public string GetXapFilesPath(string xapfilename)
        //{
        //    string xapPhysicalPath = "";


        //    if (WebFreightEntryPoint.DeploymentStage == "Dev")
        //    {
        //        xapfilename = xapfilename.ToLower();
        //        var blobfile = blobContainer.GetBlockBlobReference(xapfilename);
        //        if (blobfile.Exists())
        //        {
        //            xapPhysicalPath = blobfile.Uri.ToString();

        //            string path = Server.MapPath(".");
        //            path += "\\ClientBin\\";
        //            path += xapfilename;
        //            FileStream ff = new FileStream(path, FileMode.Create);

        //            using (MemoryStream memstream = new MemoryStream())
        //            {
                       
        //                blobfile.DownloadToStream(memstream);
        //                byte[] DatainByte = memstream.ToArray();


        //                ff.Write(DatainByte, 0, DatainByte.Length);
        //            }
        //        }

        //         xapPhysicalPath ="ClientBin/" + xapfilename;
        //    }
        //    else
        //    {
        //        xapPhysicalPath ="ClientBin/" + xapfilename;


        //    }


        //    return xapPhysicalPath;


        //}
        
    }
}

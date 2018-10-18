using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Viewer;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.SaveLoad;
using Stimulsoft.Report.Design;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Simplog.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.BL.StimulReport;

namespace WebFreight.Web.Helpers
{
    public class PdfUtil
    {
        public byte[] template {get ; set;}
        public Stream dataFile {get ; set;}

        

        ///
        /// This function Loads report definition from a fixed location
        /// and registers all Business Objects in the report definition
        ///

        ///
        /// StiReport object for the report
        ///


#if false
        
        public void Design_Click(object sender, EventArgs e)
        {
            // Get Report Object
            StiReport report = GetReport();
            // Launch Report Designer for the report
            
            report.Design();
        }
        private StiReport GetReport()
        {
            // Create a new object of StiReport Class
            StiReport report = new StiReport();
            DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();

            // Load the report definition file from C:\MyReport.mrt if the file exists
            // This tutorial assumes you have are storing the report definition in a fixed location
            // If the file does not exists then Designer will allow you to save the your newly created report in the location
            if (File.Exists("C:\\MyReport.mrt"))
            {
                report.Load("C:\\MyReport.mrt");
            }

            // Get of List of all customers from database using ADO.net Enity data model
            var obj = declarationSRMapping.GetSingle();

            // Register Business Objects for Customers in the report
            report.RegBusinessObject("Unifreight", "Customs", obj);

            // Return report to calling function
            return report;
        }
#endif
        public byte[] RunEmptyStiDesigner(object bussinessobject)
        {
            byte[] exportedStream = null;
            object businessObject = bussinessobject;
            var report = new StiReport();

            //StiDataColumnsCollection PackagesLinesColumns = new StiDataColumnsCollection();
            //PackagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
            //PackagesLinesColumns.Add("PackageQuantity", typeof(string));

            StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Cus", Name = "DecDataProvider", BusinessObjectValue = businessObject };

            
            //StiBusinessObject PackageLinesBusinessObject = new StiBusinessObject() { Category = "FBL", Name = "PackageLines", ParentBusinessObject = CurrentBusinessObject, Columns = PackagesLinesColumns };

            report.Dictionary.BusinessObjects.Clear();
            // CurrentBusinessObject.BusinessObjects.Add(PackageLinesBusinessObject);
          
            report.Dictionary.BusinessObjects.Add(currentBusinessObject);
            report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
            //StiBusinessObject CurrentBusinessObject = new StiBusinessObject() { Category = "FBL", Name = "FBLDataProvider", BusinessObjectValue = BusinessObject };
            //report.Dictionary.BusinessObjects.Clear();
            //report.Dictionary.BusinessObjects.Add(CurrentBusinessObject);
            //report.RegBusinessObject(CurrentBusinessObject.Category,CurrentBusinessObject.Name,CurrentBusinessObject.BusinessObjectValue);
            report.Dictionary.SynchronizeBusinessObjects(20);
            report.Render();
            
            using (MemoryStream memStream = new MemoryStream())
            {
                //report.ExportDocument(StiExportFormat.Pdf, memStream, pdfSettings);
                report.Save(memStream);
                exportedStream = memStream.ToArray();
            }

            return exportedStream;
        }

        public byte[] JustTestItObj(object myObj)
        {
            byte[] exportedStream = null;
            exportedStream = null;
            StiReport report = new StiReport();
            report.ReportName = @".mrt";
            var dataSet = new System.Data.DataSet();
            report = new StiReport();
            //load report definition
            
            //dataSet.ReadXml(dataFile);report.RegData(dataSet);

            report.Load(template);

            report.Dictionary.BusinessObjects.Clear();
            

                
                

            //var currentBusinessObject = new StiBusinessObject()
            //{
            //    Category = "DeclarationSReportPM",
            //    Name = "DeclarationSReport",
            //    BusinessObjectValue = myObj
            //};
            var currentBusinessObject = new StiBusinessObject() { Category = "Cus", Name = "DecDataProvider", BusinessObjectValue = myObj };

            report.Dictionary.BusinessObjects.Add(currentBusinessObject);    

            report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
            if (false)
            {
                report.Render(false);
            }
            else
            {

                
                
                //report.Dictionary.BusinessObjects.Clear();
                //report.Dictionary.BusinessObjects.Add(CurrentBusinessObject);
                //report.RegBusinessObject(CurrentBusinessObject.Category,CurrentBusinessObject.Name,CurrentBusinessObject.BusinessObjectValue);
                report.Dictionary.SynchronizeBusinessObjects(20);


                report.Render();
            }
            
            //report.AutoLocalizeReportOnRun = true;
            //report.Dictionary.Synchronize();
            StiExportSettings exportSettings = null;
            var stiExportFormat = StiExportFormat.Pdf;

            using (MemoryStream memStream = new MemoryStream())
            {
                //report.ExportDocument(StiExportFormat.Pdf, memStream, pdfSettings);
                report.ExportDocument(stiExportFormat, memStream);
                exportedStream = memStream.ToArray();
            }

            return exportedStream;

        }

        public byte[] ExportPdfFromXml()
        {
            byte[] exportedStream = null;
            exportedStream = null;
            StiReport report = new StiReport();
            report.ReportName = @".mrt";
            var dataSet = new System.Data.DataSet();
            report = new StiReport();
            //load report definition
            report.Load(template);
            dataSet.ReadXml(dataFile);
            report.RegData(dataSet);
            report.Render();
            StiExportSettings exportSettings = null;
            var stiExportFormat = StiExportFormat.Pdf;
            
            using (MemoryStream memStream = new MemoryStream())
            {
                //report.ExportDocument(StiExportFormat.Pdf, memStream, pdfSettings);
                report.ExportDocument(stiExportFormat, memStream);
                exportedStream = memStream.ToArray();
            }
            
            return exportedStream;

        }
    }
}
using Logitude.CRM.Data;
using Logitude.Customs.Data;
using Logitude.Social.Data;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Transactions;
using System.Xml.Linq;
using System.Xml;
using System.Data.Entity;
using Logitude.BookingLib.Data;
using Logitude.Accounting.Data;
using Logitude.SystemLogs;
using System.Configuration;
using Unifreight.Data.AmitalModel;
using Simplog.Server.Infrastructure.Helpers;
using Microsoft.VisualStudio.TextTemplating;
namespace Logitude.ViewsGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionInfo="";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                GlobalDBRepository globalDbRep = new GlobalDBRepository(globalContext);
                GlobalDB globalDb = globalDbRep.GetSingleGlobalDB("0");
                connectionInfo = globalDb.DBConnection;
            }

            var ms = new MemoryStream();

            DbConnection connection = null; 

            connection = DatabaseInitializer.GetConnection(connectionInfo);

           


            
           
            CustomContext customContext = new CustomContext(connection);
            CRMContext crmContext = new CRMContext(connection);
            BookingContext bookingContext = new BookingContext(connection);
            SocialContext socialContext = new SocialContext(connection);
            CommonDataContext commonContext = new CommonDataContext(connection);
            WebFreightContext webFreightContext = new WebFreightContext(connection);
            ShipmentsContext shipmentContext = new ShipmentsContext(connection);
            QuotesContext quotesContext = new QuotesContext(connection);
            InvoiceContext invoiceContext = new InvoiceContext(connection);
            AccountingContext accountingContext = new AccountingContext(connection);
            //SystemLogContext systemLogContext = SystemLogContext.GetContext();

        
			string dbConnectionInfo = string.Empty;

			if (LogitudeSettings.DatabaseManagementSystem == "oracle")
			{
				dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_SystemLogsStr"].ConnectionString; ;
			}
			else
			{
				dbConnectionInfo = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString;
			}
			DbConnection logconnection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            SystemLogContext systemLogContext = new SystemLogContext(logconnection);



#if amitalBranch
            string amitalDbConnectionInfo = System.Configuration.ConfigurationManager.ConnectionStrings["DevartAmitalDirect"].ConnectionString; ;
            var amitalContext = AmitalContext.GetContextByDBInfo(amitalDbConnectionInfo,0);
             GenerateViewsFor(amitalContext, "AmitalContext");
#endif
            System.IO.File.WriteAllText(@"C:\LogitudeGenerateViews.bat", "");

            //

            ExportMappings(systemLogContext, @"SystemLogContext.edmx");
            XElement systemLogCsdl = ExtractCsdlContent("SystemLogContext.edmx");
            systemLogCsdl.Save("SystemLogContext.csdl");
            XElement systemLogSsdl = ExtractSsdlContent("SystemLogContext.edmx");
            systemLogSsdl.Save("SystemLogContext.ssdl");
            XElement systemLogMsl = ExtractmslContent("SystemLogContext.edmx");
            systemLogMsl.Save("SystemLogContext.msl");
            GenerateViews("SystemLogContext");
            //
           // goto continueHere;
            ExportMappings(customContext, @"CustomContext.edmx");
            XElement customsCsdl = ExtractCsdlContent("CustomContext.edmx");
            customsCsdl.Save("CustomContext.csdl");
            XElement customsSsdl = ExtractSsdlContent("CustomContext.edmx");
            customsSsdl.Save("CustomContext.ssdl");
            XElement customsMsl = ExtractmslContent("CustomContext.edmx");
            customsMsl.Save("CustomContext.msl");
            GenerateViews("CustomContext");

            
            
            ExportMappings(crmContext, @"CRMContext.edmx");
            XElement crmCsdl = ExtractCsdlContent("CRMContext.edmx");
            crmCsdl.Save("CRMContext.csdl");
            XElement crmSsdl = ExtractSsdlContent("CRMContext.edmx");
            crmSsdl.Save("CRMContext.ssdl");
            XElement crmMsl = ExtractmslContent("CRMContext.edmx");
            crmMsl.Save("CRMContext.msl");
            GenerateViews("CRMContext");

            ExportMappings(bookingContext, @"BookingContext.edmx");
            XElement bookingCsdl = ExtractCsdlContent("BookingContext.edmx");
            bookingCsdl.Save("BookingContext.csdl");
            XElement bookingSsdl = ExtractSsdlContent("BookingContext.edmx");
            bookingSsdl.Save("BookingContext.ssdl");
            XElement bookingMsl = ExtractmslContent("BookingContext.edmx");
            bookingMsl.Save("BookingContext.msl");
            GenerateViews("BookingContext");



            
            GenerateViewsFor(accountingContext, "AccountingContext");
            

            ExportMappings(socialContext, @"SocialContext.edmx");
            XElement socialCsdl = ExtractCsdlContent("SocialContext.edmx");
            socialCsdl.Save("SocialContext.csdl");
            XElement socialSsdl = ExtractSsdlContent("SocialContext.edmx");
            socialSsdl.Save("SocialContext.ssdl");
            XElement socialMsl = ExtractmslContent("SocialContext.edmx");
            socialMsl.Save("SocialContext.msl");
            GenerateViews("SocialContext");

            ExportMappings(commonContext, @"CommonDataContext.edmx");
            XElement commonCsdl = ExtractCsdlContent("CommonDataContext.edmx");
            commonCsdl.Save("CommonDataContext.csdl");
            XElement commonSsdl = ExtractSsdlContent("CommonDataContext.edmx");
            commonSsdl.Save("CommonDataContext.ssdl");
            XElement commonMsl = ExtractmslContent("CommonDataContext.edmx");
            commonMsl.Save("CommonDataContext.msl");
            GenerateViews("CommonDataContext");

            ExportMappings(webFreightContext, @"WebFreightContext.edmx");
            XElement webFreightCsdl = ExtractCsdlContent("WebFreightContext.edmx");
            webFreightCsdl.Save("WebFreightContext.csdl");
            XElement webFreightSsdl = ExtractSsdlContent("WebFreightContext.edmx");
            webFreightSsdl.Save("WebFreightContext.ssdl");
            XElement webFreightMsl = ExtractmslContent("WebFreightContext.edmx");
            webFreightMsl.Save("WebFreightContext.msl");
            GenerateViews("WebFreightContext");

            ExportMappings(shipmentContext, @"ShipmentsContext.edmx");
            XElement shipmentCsdl = ExtractCsdlContent("ShipmentsContext.edmx");
            shipmentCsdl.Save("ShipmentsContext.csdl");
            XElement shipmentSsdl = ExtractSsdlContent("ShipmentsContext.edmx");
            shipmentSsdl.Save("ShipmentsContext.ssdl");
            XElement shipmentMsl = ExtractmslContent("ShipmentsContext.edmx");
            shipmentMsl.Save("ShipmentsContext.msl");
            GenerateViews("ShipmentsContext");

            ExportMappings(quotesContext, @"QuotesContext.edmx");
            XElement quotesCsdl = ExtractCsdlContent("QuotesContext.edmx");
            quotesCsdl.Save("QuotesContext.csdl");
            XElement quotesSsdl = ExtractSsdlContent("QuotesContext.edmx");
            quotesSsdl.Save("QuotesContext.ssdl");
            XElement quotesMsl = ExtractmslContent("QuotesContext.edmx");
            quotesMsl.Save("QuotesContext.msl");
            GenerateViews("QuotesContext");

            ExportMappings(invoiceContext, @"InvoiceContext.edmx");
            XElement invoiceCsdl = ExtractCsdlContent("InvoiceContext.edmx");
            invoiceCsdl.Save("InvoiceContext.csdl");
            XElement invoiceSsdl = ExtractSsdlContent("InvoiceContext.edmx");
            invoiceSsdl.Save("InvoiceContext.ssdl");
            XElement invoiceMsl = ExtractmslContent("InvoiceContext.edmx");
            invoiceMsl.Save("InvoiceContext.msl");
            GenerateViews("InvoiceContext");

 //continueHere:

            System.Diagnostics.Process.Start(@"C:\LogitudeGenerateViews.bat").WaitForExit();
            label1.Text = "Generate Views Completed.";
        }

        private void GenerateViewsFor(DbContext myContext, string contextName)
        {
            ExportMappings(myContext, contextName +@".edmx");
            XElement myCsdl = ExtractCsdlContent(contextName +".edmx");
            myCsdl.Save(contextName +".csdl");
            XElement mySsdl = ExtractSsdlContent(contextName +".edmx");
            mySsdl.Save(contextName +".ssdl");
            XElement myMsl = ExtractmslContent(contextName +".edmx");
            myMsl.Save(contextName +".msl");
            GenerateViews(contextName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionInfo = "";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                GlobalDBRepository globalDbRep = new GlobalDBRepository(globalContext);
                GlobalDB globalDb = globalDbRep.GetSingleGlobalDB("0");
                connectionInfo = globalDb.DBConnection;
            }

            var ms = new MemoryStream();
            DbConnection connection = DatabaseInitializer.GetConnection(connectionInfo);
            CustomContext customContext = new CustomContext(connection);
            CRMContext crmContext = new CRMContext(connection);
            BookingContext bookingContext = new BookingContext(connection);
            SocialContext socialContext = new SocialContext(connection);
            CommonDataContext commonContext = new CommonDataContext(connection);
            WebFreightContext webFreightContext = new WebFreightContext(connection);
            ShipmentsContext shipmentContext = new ShipmentsContext(connection);
            QuotesContext quotesContext = new QuotesContext(connection);
            InvoiceContext invoiceContext = new InvoiceContext(connection);
            AccountingContext accountingContext = new AccountingContext(connection);

            System.IO.File.WriteAllText(@"C:\LogitudeGenerateViews.bat", "");

            ExportMappings(customContext, @"CustomContext.edmx");
            XElement customsCsdl = ExtractCsdlContent("CustomContext.edmx");
            customsCsdl.Save("CustomContext.csdl");
            XElement customsSsdl = ExtractSsdlContent("CustomContext.edmx");
            customsSsdl.Save("CustomContext.ssdl");
            XElement customsMsl = ExtractmslContent("CustomContext.edmx");
            customsMsl.Save("CustomContext.msl");
            GenerateViews("CustomContext");

            System.Diagnostics.Process.Start(@"C:\LogitudeGenerateViews.bat").WaitForExit();
            label1.Text = "Generate Views Completed.";
        }



        public void GenerateViews(string contextName)
        {
            string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
            string solutionDirectory = solutionDir.FullName;
            //string solutionDirectory = ((EnvDTE.DTE)System.Runtime
            //                                  .InteropServices
            //                                  .Marshal
            //                                  .GetActiveObject("VisualStudio.DTE.11.0"))
            //                       .Solution
            //                       .FullName;
            //solutionDirectory = System.IO.Path.GetDirectoryName(solutionDirectory);

            string folderPath="";
            switch (contextName)
            {
                case "CustomContext":
                    {
                        folderPath = solutionDirectory + @"\Logitude.Customs.Data\";
                        break;
                    }
                case "AmitalContext":
                    {
                        folderPath = solutionDirectory + @"\Unifreight.Data\AmitalModel";
                        break;
                    }
                case "CRMContext":
                    {
                        folderPath = solutionDirectory + @"\Logitude.CRM.Data\";
                        break;
                    }

                case "BookingContext":
                    {
                        folderPath = solutionDirectory + @"\Logitude.BookingLib.Data\";
                        break;
                    }
                case "AccountingContext":
                    {
                        folderPath = solutionDirectory + @"\Logitude.Accounting.Data\";
                        break;
                    }

                case "SocialContext":
                    {
                        folderPath = solutionDirectory + @"\Logitude.Social.Data\";
                        break;
                    }
                case "CommonDataContext":
                    {
                        folderPath = solutionDirectory + @"\Simplog.Data\CommonDataModel";
                        break;
                    }
                case "WebFreightContext":
                    {
                        folderPath = solutionDirectory + @"\Simplog.Data\InfrastructureModel";
                        break;
                    }
                case "ShipmentsContext":
                    {
                        folderPath = solutionDirectory + @"\Simplog.Data\ShipmentsModel";
                        break;
                    }
                case "QuotesContext":
                    {
                        folderPath = solutionDirectory + @"\Simplog.Data\QuoteModel";
                        break;
                    }
                case "InvoiceContext":
                    {
                        folderPath = solutionDirectory + @"\Simplog.Data\InvoiceModel";
                        break;
                    }

                case "SystemLogContext":
                    {
                        folderPath = solutionDirectory + @"\Logitude.SystemLogs\";
                        break;
            }
            }
            string edmgenPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\EdmGen.exe";
            string mode = "/nologo /language:CSharp /mode:ViewGeneration ";
            string inssdl = "/inssdl:" + projectPath + @"\" + @"bin/Debug/" + contextName + ".ssdl";
            string incsdl = "/incsdl:" + projectPath + @"\" + @"bin/Debug/" + contextName + ".csdl";
            string inmsl = "/inmsl:" + projectPath + @"\" + @"bin/Debug/" + contextName + ".msl";
            string outviews = "/outviews:" + folderPath + @"\" + contextName + ".Views.cs";
            string strCmdText;
            strCmdText = ("\"" + edmgenPath + "\" ") + mode + ("\"" + inssdl + "\" ") + ("\"" + incsdl + "\" ") + ("\"" + inmsl + "\" ") + ("\"" + outviews + "\"");

            
            System.IO.File.AppendAllText(@"C:\LogitudeGenerateViews.bat", Environment.NewLine);
            System.IO.File.AppendAllText(@"C:\LogitudeGenerateViews.bat", Environment.NewLine);
            System.IO.File.AppendAllText(@"C:\LogitudeGenerateViews.bat", strCmdText);
            
        }

        void ExportMappings(DbContext context, string edmxFile)
        {
            var settings = new XmlWriterSettings { Indent = true };
            using (XmlWriter writer = XmlWriter.Create(edmxFile, settings))
            {
                System.Data.Entity.Infrastructure.EdmxWriter.WriteEdmx(context, writer);
            }
        }

        private XElement ExtractCsdlContent(string edmxFile)
        {
            XElement csdlContent = null;
            XNamespace edmxns = "http://schemas.microsoft.com/ado/2009/11/edmx";
            XNamespace edmns = "http://schemas.microsoft.com/ado/2009/11/edm";
            XDocument edmxDoc = XDocument.Load(edmxFile);
            if (edmxDoc != null)
            {
                XElement edmxNode = edmxDoc.Element(edmxns + "Edmx");
                if (edmxNode != null)
                {
                    XElement runtimeNode = edmxNode.Element(edmxns + "Runtime");
                    if (runtimeNode != null)
                    {
                        XElement conceptualModelsNode = runtimeNode.Element(edmxns +
                          "ConceptualModels");
                        if (conceptualModelsNode != null)
                        {
                            csdlContent = conceptualModelsNode.Element(edmns + "Schema");
                        }
                    }
                }
            }
            return csdlContent;
        }

        private XElement ExtractSsdlContent(string edmxFile)
        {
            XElement csdlContent = null;
            XNamespace edmxns = "http://schemas.microsoft.com/ado/2009/11/edmx";
            XNamespace edmns = "http://schemas.microsoft.com/ado/2009/11/edm";
            XNamespace ssdl = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl";
            XDocument edmxDoc = XDocument.Load(edmxFile);
            if (edmxDoc != null)
            {
                XElement edmxNode = edmxDoc.Element(edmxns + "Edmx");
                if (edmxNode != null)
                {
                    XElement runtimeNode = edmxNode.Element(edmxns + "Runtime");
                    if (runtimeNode != null)
                    {
                        XElement conceptualModelsNode = runtimeNode.Element(edmxns +
                          "StorageModels");
                        if (conceptualModelsNode != null)
                        {
                            csdlContent = conceptualModelsNode.Element(ssdl + "Schema");
                        }
                    }
                }
            }
            return csdlContent;
        }

        private XElement ExtractmslContent(string edmxFile)
        {
            XElement csdlContent = null;
            XNamespace edmxns = "http://schemas.microsoft.com/ado/2009/11/edmx";
            XNamespace edmns = "http://schemas.microsoft.com/ado/2009/11/edm";
            XNamespace mls = "http://schemas.microsoft.com/ado/2009/11/mapping/cs";
            XDocument edmxDoc = XDocument.Load(edmxFile);
            if (edmxDoc != null)
            {
                XElement edmxNode = edmxDoc.Element(edmxns + "Edmx");
                if (edmxNode != null)
                {
                    XElement runtimeNode = edmxNode.Element(edmxns + "Runtime");
                    if (runtimeNode != null)
                    {
                        XElement conceptualModelsNode = runtimeNode.Element(edmxns +
                          "Mappings");
                        if (conceptualModelsNode != null)
                        {
                            csdlContent = conceptualModelsNode.Element(mls + "Mapping");
                        }
                    }
                }
            }
            return csdlContent;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

      

    }
}

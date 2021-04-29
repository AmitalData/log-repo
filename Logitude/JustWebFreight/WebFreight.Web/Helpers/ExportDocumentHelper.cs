using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Services;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using WebFreight.Web.Azure;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers;
using WebFreight.Web.ReportsWebServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Testing;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using System.Threading;
using System.Web;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.WebServices;
using WebFreight.Web.AccountingModel.Reports.Journal;
using WebFreight.Web.AccountingModel.Reports.BankDeposit;
using Logitude.Server.Tools.QueueService;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.DataProviderHelpers;
using WebFreight.Web.AccountingModel.Reports.PaymentCheque;
using WebFreight.Web.AccountingModel.Reports.TaxDeductionReport;
using Logitude.Accounting.BL.DataContract;
using WebFreight.Web.AccountingModel.Reports.OpenFormatReport;
using Newtonsoft.Json;
using System.Net;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.AccountingModel.Reports.Interest;
using Simplog.Data.Helpers;
using Logitude.BL.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class ExportDocumentHelper
    {

        #region ExportDocument2Pdf

        public string ExportDocument2Pdf(string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string documentOutId, int tenant, string documentTypeCopyId, string userId = null)
        {
            string result = string.Empty;

            try
            {
                result = ExportDocument2PdfNormalWay(documentTypeId, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentOutId, tenant, documentTypeCopyId, userId);
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(AuthenticationUtil.AuthenticatedUserEmail))
                {
                    string authenticateduser = "";

                    try
                    {
                        authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                    }

                    catch
                    {
                        authenticateduser = "UnKnown";
                    }

                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(new Exception(ex.Message), DateTime.Now, 0, "", authenticateduser, "", ip);
                }
                throw new Exception(ex.Message);
            }



            return result;
        }


        public string ExportDocument2Pdf(ExportDocumentArgs exportDocumentArgs, string documentTypeCopyId)
        {
            string result = ExportDocument2PdfNormalWay(exportDocumentArgs.DocumentTypeId, exportDocumentArgs.EntityId, exportDocumentArgs.ObjectTableId, exportDocumentArgs.ChildEntityId, exportDocumentArgs.ChildObjectTableId, exportDocumentArgs.CurrentDocumentOutId, exportDocumentArgs.Tenant, documentTypeCopyId, exportDocumentArgs.LoggedContactId);
            return result;
        }





        private string ExportDocument2PdfViewWebService(string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string documentOutId, int tenant, string documentTypeCopyId, string userId)
        {
            string result;
            try
            {
                BuildDocumentParameter exportDocument2PdfParamete = new BuildDocumentParameter() { DocumentOutId = documentOutId, DocumentTypeId = documentTypeId, EntityId = entityId, EntityObjectTableId = entityObjectTableId, ChildEntityId = childEntityId, ChildObjectTableId = childObjectTableId, DocumentTypeCopyId = documentTypeCopyId, UserId = userId, Tenant = tenant };
                string exportDocument2PdfParameterxml = LogitudeXmlSerializer.SerializeObjectToXmlString(exportDocument2PdfParamete);
                exportDocument2PdfParameterxml = exportDocument2PdfParameterxml.Replace("<", "@TagOpen");
                string soap = GetSoapReportViaWebService("ExportDocument2Pdf", "exportDocument2PdfParameterxml", exportDocument2PdfParameterxml);

                string token = HttpContext.Current.Request.Headers["Token"];
                string url = LogitudeSettings.CPUIntensiveWebServicesURL.TrimEnd('/') + "/WebServices/BuildDocumentReportWebService.asmx";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Headers.Add("Token", token);
                req.ContentType = "application/soap+xml;";
                req.Method = "POST";
                using (Stream stm = req.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(soap);
                    }
                }
                using (WebResponse Serviceres = req.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                    {
                        var serviceResult = rd.ReadToEnd();
                        result = getBetween(serviceResult, "<ExportDocument2PdfResult>", "</ExportDocument2PdfResult>");
                        ExceptionDateExportPdfDocumentWebService = null;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionDateExportPdfDocumentWebService = DateTime.Now;
                result = ExportDocument2PdfNormalWay(documentTypeId, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentOutId, tenant, documentTypeCopyId, userId);

            }

            return result;
        }

        public string ExportDocument2PdfNormalWay(string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string documentOutId, int tenant, string documentTypeCopyId, string userId = null)
        {

            var currentthreaduser = Thread.CurrentPrincipal;
            long theT1 = new long();
            long theT2 = new long();
            long theA1 = new long();
            long theA2 = new long();
            theA1 = System.DateTime.Now.Ticks;
            Byte[] templatedata = null;

            DocumentTypeRepository repository = new DocumentTypeRepository(tenant);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);
            DocumentRepository docRepository = new DocumentRepository(tenant);
            DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(tenant);
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(tenant);
            DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
            DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentOut.DocumentTemplateId);
            DocumentTypeCopy documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(documentTypeCopyId);
            DocumentType documentType = repository.GetSingleDocumentTypes(documentTypeId, tenant);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);

            if (documentType.Code == "FTDT")
            {
                throw new Exception("This is a failure test document!");
            }

            bool isJsonBody = false;
            if (defaulttemplate != null)
            {
                templatedata = defaulttemplate.TemplateBody;
            }

            if (templatedata != null)
            {
                if (templatedata.Length != 0)
                {
                    StiReport report = GetReportDocument(documentType, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentTypeCopy, templatedata, defaulttemplate, tenant, theT1, theT2, theA1, theA2, userId);

                    LoadEditableFields(documentOut, report);

                    byte[] reportDdf = ExportDocumentReportToPDF(report, tenant, documentType.Code);

                    DocumentOutCopy documentOutCopy = documentOutCopyRep.GetDocumentOutCopyByDocumentOutAndType(documentOutId, documentTypeCopyId, tenant);

                    DocumentFileNameParameter documentFileNameParameter = new DocumentFileNameParameter
                    {
                        EntityId = entityId,
                        EntityObjectTableId = entityObjectTableId,
                        ChildEntityId = childEntityId,
                        Tenant = tenant,
                        UserId = userId,
                        DocumentTypeCopy = documentTypeCopy,
                        DocumentType = documentType,
                    };
                    string calculatedFileName = GetCalculatedDocumentFileName(documentFileNameParameter);

                    Document document = CreateOrUpdateDocument(documentOutId, tenant, documentTypeCopyId, docRepository, documentOutCopyRep, documentOut, documentTypeCopy, documentType, ref documentOutCopy, calculatedFileName);

                    SaveSTIDocumentInStorage(document, reportDdf, tenant);

                    var docFiling = documentsFilingRepository.GetSingleDocumentsFiling(documentOutId);
                    if (docFiling != null)
                    {
                        docFiling.DocumentId = document.Id;
                        documentsFilingRepository.Update(docFiling);
                        documentsFilingRepository.SubmitChanges();
                        if (docFiling.IsSharedOut)
                        {
                            ResharedAgentDocumentQueue(tenant, documentTypeCopyId, documentType, docFiling);
                        }
                    }

                    theA2 = System.DateTime.Now.Ticks;
                    return document.Id;
                }

                else
                    return null;
            }
            else
                return null;




        }

        #endregion


        private static void LoadEditableFields(DocumentOut documentOut, StiReport report)
        {
            using (MemoryStream memstr = new MemoryStream())
            {
                if (documentOut.EditableFields != null)
                {
                    // FileStream fileStream = new FileStream(@"C:\temp\Editable.txt", FileMode.Open, FileAccess.Read);
                    memstr.Write(documentOut.EditableFields, 0, documentOut.EditableFields.Length);
                    memstr.Seek(0, SeekOrigin.Begin);
                    report.LoadEditableFields(memstr);
                }
            }
        }

        private Document CreateOrUpdateDocument(string documentOutId, int tenant, string documentTypeCopyId, DocumentRepository docRepository, DocumentOutCopyRepository documentOutCopyRep, DocumentOut documentOut, DocumentTypeCopy documentTypeCopy, DocumentType documentType, ref DocumentOutCopy documentOutCopy, string calculatedFileName)
        {
            Document document;
            if (documentOutCopy == null)
            {
                using (TransactionScope scop = TransactionFactory.GetTransaction())
                {
                    string copyId = IdCounter.GetNumber("Document", tenant).ToString();
                    documentOutCopy = new DocumentOutCopy()
                    {
                        Id = copyId,
                        Tenant = tenant,
                        DocumentTypeCopyId = documentTypeCopyId,
                        DocumentOutId = documentOutId,
                        //DocumentId = copyId,
                    };

                    document = new Document()
                    {
                        CreateDate = DateTime.Now,
                        Extension = "pdf",
                        //FileSize = 999,
                        Tenant = Convert.ToInt32(documentOut.Tenant),
                        Id = documentOutCopy.Id,
                        HasFile = true,
                        Folder = "docsout",
                        FileName = documentType.Name,
                        CalculatedFileName = calculatedFileName,

                    };
                    docRepository.Add(document);
                    documentOutCopyRep.Add(documentOutCopy);
                    documentOutCopyRep.SubmitChanges();
                    docRepository.SubmitChanges();
                    scop.Complete();
                }
            }
            else
            {
                document = docRepository.GetSingleDocument(tenant, documentOutCopy.Id);
                document.CreateDate = DateTime.Now;
                document.Extension = "pdf";
                //document.FileSize = 999;
                document.FileName = documentTypeCopy.Name;
                document.HasFile = true;
                document.Folder = "docsout";
                document.IsEncrypted = true;
                document.CalculatedFileName = calculatedFileName;
                docRepository.Update(document);
                docRepository.SubmitChanges();
            }

            documentOutCopy.DocumentId = document.Id;
            documentOutCopyRep.Update(documentOutCopy);
            documentOutCopyRep.SubmitChanges();
            return document;
        }

        private static void ResharedAgentDocumentQueue(int tenant, string documentTypeCopyId, DocumentType documentType, DocumentsFiling docFiling)
        {
            if (documentType.SharedDocumentTypeCopyId == documentTypeCopyId)
            {

                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ResharedAgentDocumentQueue", tenant);
                queueservice.Send(new Dictionary<string, string>() { { "EntityId", docFiling.EntityId }, { "Tenant", docFiling.Tenant.ToString() }, { "DocumentTypeCode", documentType.Code }, { "SecurityId", docFiling.SecurityId } }, tenant, null, null, null, null);

            }
        }

        private string GetCalculatedDocumentFileName(DocumentFileNameParameter documentFileNameParameter)
        {
            string calculatedFileName = string.Empty;
            if (!string.IsNullOrEmpty(documentFileNameParameter.DocumentType.FileName))
            {
                if (documentFileNameParameter.DocumentType.FileName.Contains("["))
                    calculatedFileName = GetDocumentFileNameFromDataFields(documentFileNameParameter);
                else 
                    calculatedFileName = documentFileNameParameter.DocumentType.FileName;
            }

            if (string.IsNullOrEmpty(calculatedFileName)) calculatedFileName = documentFileNameParameter.DocumentType.Name;

            if (documentFileNameParameter.DocumentTypeCopy != null && documentFileNameParameter.DocumentType.Name != documentFileNameParameter.DocumentTypeCopy.Name && string.IsNullOrEmpty(documentFileNameParameter.DocumentType.FileName))
            {
                calculatedFileName += "_" + documentFileNameParameter.DocumentTypeCopy.Name;
            }

            return calculatedFileName;
        }

        private static string GetDocumentFileNameFromDataFields(DocumentFileNameParameter documentFileNameParameter)
        {
            string calculatedFileName = string.Empty;
            string htmlResolve = ResolveDocumentFileNameFromDataFields(documentFileNameParameter);
            if (!string.IsNullOrEmpty(htmlResolve))
            {
                if (htmlResolve.Length > 120)
                {
                    calculatedFileName = htmlResolve.Substring(0, 119);
                }
                else calculatedFileName = htmlResolve;
            }

            return calculatedFileName;
        }

        private static string ResolveDocumentFileNameFromDataFields(DocumentFileNameParameter documentFileNameParameter)
        {
            string calculatedDocumentFileName = ResolveMainObjectTableFields(documentFileNameParameter);
            calculatedDocumentFileName = ResolveDocumentTypeFields(documentFileNameParameter, calculatedDocumentFileName);
            calculatedDocumentFileName = ResolveDocumentFilingFields(documentFileNameParameter, calculatedDocumentFileName);

            return calculatedDocumentFileName;
        }

        private static string ResolveDocumentFilingFields(DocumentFileNameParameter documentFileNameParameter, string documentFileName)
        {
            string calculatedDocumentFileName = documentFileName;
            if (calculatedDocumentFileName.Contains("[DocumentsFiling"))
            {
                calculatedDocumentFileName = ResolveDocumentFileNameFromDocumentsFilingObjectTable(documentFileNameParameter, calculatedDocumentFileName);
            }

            return calculatedDocumentFileName;
        }

        private static string ResolveDocumentTypeFields(DocumentFileNameParameter documentFileNameParameter, string documentFileName)
        {
            string calculatedDocumentFileName = documentFileName;

            if (calculatedDocumentFileName.Contains("[DocumentTypeCopyName]"))
            {
                calculatedDocumentFileName = calculatedDocumentFileName.Replace("[DocumentTypeCopyName]", documentFileNameParameter?.DocumentTypeCopy?.Name);
            }

            if (calculatedDocumentFileName.Contains("[DocumentType"))
            {
                DocumentFileNameFromObjectTableParameter documentFileNameFromObjectTableParameter = new DocumentFileNameFromObjectTableParameter
                {
                    DocumentFileNameParameter = documentFileNameParameter,
                    DocumentFileName = calculatedDocumentFileName,
                    EntityId = documentFileNameParameter.DocumentType.Id,
                    ObjectTableName = "DocumentType",
                };
                calculatedDocumentFileName = GetDocumentFileNameFromDataFieldsByObjectTableName(documentFileNameFromObjectTableParameter);
            }

            return calculatedDocumentFileName;
        }

        private static string ResolveDocumentFileNameFromDocumentsFilingObjectTable(DocumentFileNameParameter documentFileNameParameter, string documentFileName)
        {
            string calculatedDocumentFileName = documentFileName;

            string id = documentFileNameParameter.EntityId;
            if (!string.IsNullOrEmpty(documentFileNameParameter.ChildEntityId))
                id = documentFileNameParameter.ChildEntityId;

            Logitude.BL.CommonDataModel.EntityQueries.DocumentsFilingQuery documentsFilingQuery = new Logitude.BL.CommonDataModel.EntityQueries.DocumentsFilingQuery(documentFileNameParameter.Tenant);
            DocumentsFilingPM documentsFilingPM = documentsFilingQuery.GetDocumentsFilingByDocumentType(documentFileNameParameter.DocumentType.Id, documentFileNameParameter.DocumentType.ObjectTableId, id, documentFileNameParameter.Tenant);
            if (documentsFilingPM != null)
            {
                DocumentFileNameFromObjectTableParameter documentFileNameFromObjectTableParameter = new DocumentFileNameFromObjectTableParameter
                {
                    DocumentFileNameParameter = documentFileNameParameter,
                    DocumentFileName = calculatedDocumentFileName,
                    EntityId = documentsFilingPM.Id,
                    ObjectTableName = "DocumentsFiling",
                };
                calculatedDocumentFileName = GetDocumentFileNameFromDataFieldsByObjectTableName(documentFileNameFromObjectTableParameter);
            }

            return calculatedDocumentFileName;
        }

        private static string ResolveMainObjectTableFields(DocumentFileNameParameter documentFileNameParameter)
        {
            HtmlEditorResolveArgs htmlResolveArgs = new HtmlEditorResolveArgs();
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

            string id = documentFileNameParameter.EntityId;
            if (!string.IsNullOrEmpty(documentFileNameParameter.ChildEntityId))
                id = documentFileNameParameter.ChildEntityId;

            htmlResolveArgs.EntityId = id;
            htmlResolveArgs.ObjectTableId = documentFileNameParameter.DocumentType.ObjectTableId;
            htmlResolveArgs.HtmlString = documentFileNameParameter.DocumentType.FileName;
            htmlResolveArgs.UserId = documentFileNameParameter.UserId;
            htmlResolveArgs.Tenant = documentFileNameParameter.Tenant;

            string htmlResolve = htmlEditorHelper.ResolveHtmlString(htmlResolveArgs);
            return htmlResolve;
        }

        private static string GetDocumentFileNameFromDataFieldsByObjectTableName(DocumentFileNameFromObjectTableParameter documentFileNameFromObjectTableParameter)
        {
            HtmlEditorResolveArgs htmlResolveArgs = new HtmlEditorResolveArgs();
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

            htmlResolveArgs.EntityId = documentFileNameFromObjectTableParameter.EntityId;
            htmlResolveArgs.ObjectTableId = "";
            htmlResolveArgs.ObjectTableName = documentFileNameFromObjectTableParameter.ObjectTableName;
            htmlResolveArgs.HtmlString = documentFileNameFromObjectTableParameter.DocumentFileName.Replace("[" + documentFileNameFromObjectTableParameter.ObjectTableName, "[");
            htmlResolveArgs.UserId = documentFileNameFromObjectTableParameter.DocumentFileNameParameter.UserId;
            htmlResolveArgs.Tenant = documentFileNameFromObjectTableParameter.DocumentFileNameParameter.Tenant;

            string resolveDocumentFileName = htmlEditorHelper.ResolveHtmlString(htmlResolveArgs);
            return resolveDocumentFileName;
        }

        private long t1;
        long t2;

        public StiReport GetReportDocument(DocumentType documentType, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, DocumentTypeCopy documentTypeCopy, Byte[] templatedata, DocumentTypeTemplate defaulttemplate, int tenant, long theT1, long theT2, long theA1, long theA2, string userId = null)
        {

            string documentTypeCopyId = documentTypeCopy != null ? documentTypeCopy.Id : "";
            string documentTypeCode = !string.IsNullOrEmpty(documentType.Code) ? documentType.Code.ToUpper() : "";
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
            StiReport report = BuildReport(documentType.Code, documentType.Id, entityId, entityObjectTableId, childEntityId, childObjectTableId, defaulttemplate.Id, tenant, userId, documentTypeCopyId);
            return report;
        }


        //private StiReport BuildReportViaWebService(BuildDocumentParameter buildDocumentParameter)
        //{
        //    try
        //    {
        //        string buildDocumentParameterxml = LogitudeXmlSerializer.SerializeObjectToXmlString(buildDocumentParameter);
        //        buildDocumentParameterxml = buildDocumentParameterxml.Replace("<", "@TagOpen");

        //        string soap = GetSoapReportViaWebService("BuildDocumentReport" , "buildDocumentParameterxml" , buildDocumentParameterxml);
        //        StiReport stiReport = new StiReport();

        //        string url = LogitudeSettings.CPUIntensiveWebServicesURL.TrimEnd('/') + "/WebServices/BuildDocumentReportWebService.asmx";
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        string token = HttpContext.Current.Request.Headers["Token"];
        //        req.Headers.Add("Token", token);
        //        req.ContentType = "application/soap+xml;";
        //        req.Method = "POST";
        //        using (Stream stm = req.GetRequestStream())
        //        {
        //            using (StreamWriter stmw = new StreamWriter(stm))
        //            {
        //                stmw.Write(soap);
        //            }
        //        }
        //        using (WebResponse Serviceres = req.GetResponse())
        //        {
        //            using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
        //            {
        //                var serviceResult = rd.ReadToEnd();
        //                string result = getBetween(serviceResult, "<BuildDocumentReportResult>", "</BuildDocumentReportResult>");

        //                if (!string.IsNullOrEmpty(result))
        //                {
        //                    result = HttpUtility.HtmlDecode(result);
        //                    stiReport.LoadDocumentFromString(result);
        //                    ExceptionDateExportPdfDocumentWebService = null;
        //                }

        //            }
        //        }

        //        return stiReport;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionDateExportPdfDocumentWebService = DateTime.Now;
        //        return BuildReport(buildDocumentParameter.DocumentTypeCode, buildDocumentParameter.DocumentTypeId, buildDocumentParameter.EntityId, buildDocumentParameter.EntityObjectTableId, buildDocumentParameter.ChildEntityId, buildDocumentParameter.ChildObjectTableId, buildDocumentParameter.DefaulttemplateId, buildDocumentParameter.Tenant, buildDocumentParameter.DocumentTypeCopyId, buildDocumentParameter.UserId);
        //    }
        //}

        public string GetSoapReportViaWebService(string methodName, string parameterName, string parameterxml)
        {
            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
 
xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"">
  <soap:Body>
    <methodName xmlns=""http://tempuri.org/"">
     <parameterName>" + parameterxml + "</parameterName></methodName></soap:Body></soap:Envelope>";

            result = result.Replace("methodName", methodName);
            result = result.Replace("parameterName", parameterName);
            return result;

        }

        public StiReport BuildReport(string documentTypeCode, string documentTypeId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, string defaulttemplateId, int tenant, string userId, string documentTypeCopyId)
        {
            long theT1;
            long theT2;
            StiReport report = new StiReport();
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
            DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(defaulttemplateId);
            switch (documentTypeCode)
            {
                case "EXCU":
                case "SELE":
                case "TML":
                case "740PP":
                case "740":
                case "AVISC":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        AWBWebService awbWebService = new AWBWebService();
                        byte[] byteArray = awbWebService.StartLoadingDataToAWB(entityId, tenant, documentTypeCopyId, true);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(AWBDataProvider));
                        AWBDataProvider awbDataProvider = (AWBDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(awbDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "AWB", Name = "AWBDataProvider", BusinessObjectValue = awbDataProvider };

                        //-----------
                        ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                        ShipmentPM shipmentPm = shipmentQuery.GetSinglePM(entityId, tenant);
                        StiBusinessObject shipmentPmBusinessObject = new StiBusinessObject() { Category = "ShipmentPM", Name = "ShipmentPMDataProvider", BusinessObjectValue = shipmentPm };

                        //-----------
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant, shipmentPmBusinessObject);
                    }
                    break;

                case "714":
                case "714PP":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        AWBWebService awbWebService = new AWBWebService();
                        byte[] byteArray = awbWebService.StartLoadingDataToAWB(entityId, tenant, documentTypeCopyId, true);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(AWBDataProvider));
                        AWBDataProvider awbDataProvider = (AWBDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(awbDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "HAWB", Name = "AWBDataProvider", BusinessObjectValue = awbDataProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }

                    break;

                case "TZU":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        DeclarationFormsWebService declarationWebService = new DeclarationFormsWebService();
                        byte[] byteArray = declarationWebService.StartLoadingDataToTzrufa(entityId, tenant, documentTypeCopyId, true);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(DeclarationFormsDataProvider));
                        DeclarationFormsDataProvider formsDataProvider = (DeclarationFormsDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(formsDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "FORM", Name = "DeclarationFormsDataProvider", BusinessObjectValue = formsDataProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }

                    break;

                case "ITDT":
                    {
                        InterestPrintService service = new InterestPrintService();
                        InterestDataProvider InterestReportDP = service.LoadDataProvider(entityId, tenant);
                        BaseDataProviderService.FillBaseVariableFields(InterestReportDP, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "ITDT", Name = "InterestDataProvider", BusinessObjectValue = InterestReportDP };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                    }
                    break;
                case "JRPR":
                    {
                        JournalPrintService service = new JournalPrintService();
                        JournalDataProvider journalDP = service.LoadDataProvider(entityId, tenant);
                        BaseDataProviderService.FillBaseVariableFields(journalDP, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "JRPR", Name = "JournalDataProvider", BusinessObjectValue = journalDP };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                    }

                    break;

                case "BDPR":
                    {
                        BankDepositPrintService service = new BankDepositPrintService();
                        BankDepositDataProvider bankDepositDP = service.LoadDataProvider(entityId, tenant);
                        BaseDataProviderService.FillBaseVariableFields(bankDepositDP, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "BDPR", Name = "BankDepositDataProvider", BusinessObjectValue = bankDepositDP };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                    }

                    break;

                case "PCDR":
                    {
                        PaymentChequePrintService service = new PaymentChequePrintService();
                        PaymentChequeDataProvider paymentChequeDP = service.LoadDataProvider(entityId, tenant);
                        BaseDataProviderService.FillBaseVariableFields(paymentChequeDP, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "PCDR", Name = "PaymentChequeDataProvider", BusinessObjectValue = paymentChequeDP };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "TDDP":
                    {
                        TaxDeductionReportPrintService service = new TaxDeductionReportPrintService();
                        TaxDeductionReportData taxDeductionDP = service.LoadDataProvider(entityId, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "TDDP", Name = "TaxDeductionReportData", BusinessObjectValue = taxDeductionDP };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                    }

                    break;

                case "OFDP":
                    {
                        OpenFormatReportPrintService service = new OpenFormatReportPrintService();
                        OpenFormatReportDataProvider OpenFormatReporDP = service.LoadDataProvider(entityId, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "OFDP", Name = "OpenFormatReportDataProvider", BusinessObjectValue = OpenFormatReporDP };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                    }

                    break;

                case "MBOL":
                case "SBOL":
                case "716":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        OceanExportWebService oceanWebService = new OceanExportWebService();
                        byte[] byteArray = oceanWebService.GetFBLData(entityId, tenant, documentTypeCopyId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(FBLDataProvider));
                        FBLDataProvider fbLdataprovider = (FBLDataProvider)serializer.Deserialize(memorystream);
                        fbLdataprovider.InServerSide = true;
                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        BaseDataProviderService.FillBaseVariableFields(fbLdataprovider, tenant);

                        StiDataColumnsCollection packagesLinesColumns = new StiDataColumnsCollection();
                        packagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns.Add("PackageType", typeof(string));
                        packagesLinesColumns.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns.Add("PackageVolume", typeof(string));
                        packagesLinesColumns.Add("PackageQuantityAndType", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "FBL", Name = "FBLDataProvider", BusinessObjectValue = fbLdataprovider };
                        StiBusinessObject packageLinesBusinessObject = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };
                        StiBusinessObject attachmentListBusinessObject = new StiBusinessObject() { Name = "AttachmentList", Alias = "AttachmentList", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(packageLinesBusinessObject);
                        currentBusinessObject.BusinessObjects.Add(attachmentListBusinessObject);

                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "ESU":
                case "890":
                case "DEOR":
                case "LCOT":
                case "ARNT":
                case "COO":
                case "BCO":
                case "716SD":
                case "PND":
                case "SFBL":
                case "BCS":
                case "IFI":
                case "GAPS":
                case "DOR":
                case "PGDF":
                case "REOR":
                case "860":
                case "865":
                case "852":
                case "PROD":
                case "ATME":
                case "DRA":
                case "SVDF":
                case "CA":
                case "WHR":
                case "AVDE":
                case "861":
                case "862":
                case "863":
                case "BCA":
                case "CRCT":
                case "CRCD":
                case "CRCC":
                case "PCRC":
                case "HORD":
                case "SOPI":
                case "ETO":
                case "ITO":
                case "SSN":
                case "CRCW":
                case "CRCO":
                case "CRCI":
                case "CRCCU":
                case "CRCCM":
                case "CRCCB":
                case "DESCH":
                case "WESL":
                case "SHCO":
                case "ABOCO":
                case "SHCMR":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ShippingDeclarationWebService shippingDeclarationWebService = new ShippingDeclarationWebService();
                        byte[] byteArray = shippingDeclarationWebService.GetShippingDeclarationData(entityId, tenant, documentTypeCode, documentTypeCopyId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ShippingDeclarationDataProvider));
                        ShippingDeclarationDataProvider shippingDeclarationdataprovider = (ShippingDeclarationDataProvider)serializer.Deserialize(memorystream);
                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");

                        BaseDataProviderService.FillBaseVariableFields(shippingDeclarationdataprovider, tenant);

                        StiDataColumnsCollection packagesLinesColumns = new StiDataColumnsCollection();
                        packagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns.Add("PackageType", typeof(string));
                        packagesLinesColumns.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns.Add("PackageVolume", typeof(string));
                        packagesLinesColumns.Add("PackageQuantityAndType", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipping declaration", Name = "ShippingDeclarationDataProvider", BusinessObjectValue = shippingDeclarationdataprovider };
                        StiBusinessObject packageLinesBusinessObject = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };
                        StiBusinessObject attachmentListBusinessObject = new StiBusinessObject() { Name = "AttachmentList", Alias = "AttachmentList", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(packageLinesBusinessObject);
                        currentBusinessObject.BusinessObjects.Add(attachmentListBusinessObject);

                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "740L":
                case "740HL":
                case "LCLL":
                case "LAL":
                    {
                        AWBLabelsWebSerivce awblabelsWebService = new AWBLabelsWebSerivce();
                        byte[] byteArray = awblabelsWebService.GetAWBLabelsData(entityId, tenant, documentTypeId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<AWBLabelsDataProvider>));
                        List<AWBLabelsDataProvider> awblabelsdataprovider = (List<AWBLabelsDataProvider>)serializer.Deserialize(memorystream);
                        foreach (AWBLabelsDataProvider aWBLabelsDataProvider in awblabelsdataprovider)
                        {
                            BaseDataProviderService.FillBaseVariableFields(aWBLabelsDataProvider, tenant);
                        }
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "AWB Labels", Name = "AWBLabelsDataProvider", BusinessObjectValue = awblabelsdataprovider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "BDE":
                case "782":
                case "783":
                case "784":// Delivery note
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        DeliveryNoteWebService deliverynoteWebService = new DeliveryNoteWebService();
                        byte[] byteArray = deliverynoteWebService.GetDeliveryData(entityId, entityObjectTableId, childEntityId, childObjectTableId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(DeliveryNoteDataProvider));
                        DeliveryNoteDataProvider deliverynotedataprovider = (DeliveryNoteDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(deliverynotedataprovider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant,User != null ? User.Identity.Name : "",User != null ? User.Identity.Name : "");

                        StiDataColumnsCollection packagesLinesColumns = new StiDataColumnsCollection();
                        packagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns.Add("PackageType", typeof(string));
                        packagesLinesColumns.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns.Add("PackageVolume", typeof(string));

                        StiDataColumnsCollection insidePackagesLinesColumns = new StiDataColumnsCollection();
                        insidePackagesLinesColumns.Add("PackageType", typeof(string));
                        insidePackagesLinesColumns.Add("Quantity", typeof(string));
                        insidePackagesLinesColumns.Add("Dimensions", typeof(string));
                        insidePackagesLinesColumns.Add("Volume", typeof(double));
                        insidePackagesLinesColumns.Add("VolumetricWeight", typeof(double));
                        insidePackagesLinesColumns.Add("Weight", typeof(double));
                        insidePackagesLinesColumns.Add("Description", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Delivery Note", Name = "DeliveryNoteDataProvider", BusinessObjectValue = deliverynotedataprovider };
                        StiBusinessObject packageLinesBusinessObject = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };
                        StiBusinessObject attachmentListBusinessObject = new StiBusinessObject() { Name = "AttachmentList", Alias = "AttachmentList", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };
                        StiBusinessObject insidePackagesLinesBusinessObject = new StiBusinessObject() { Name = "InsidePackagesLines", Alias = "InsidePackagesLines", ParentBusinessObject = packageLinesBusinessObject, Columns = insidePackagesLinesColumns };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(packageLinesBusinessObject);
                        currentBusinessObject.BusinessObjects.Add(attachmentListBusinessObject);
                        currentBusinessObject.BusinessObjects.Add(insidePackagesLinesBusinessObject);

                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "781":// Pickup note
                case "DORE":
                case "TBOL":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        DeliveryNoteWebService deliverynoteWebService = new DeliveryNoteWebService();
                        byte[] byteArray = deliverynoteWebService.GetPickupData(entityId, entityObjectTableId, childEntityId, childObjectTableId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(DeliveryNoteDataProvider));
                        DeliveryNoteDataProvider deliverynotedataprovider = (DeliveryNoteDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(deliverynotedataprovider, tenant);

                        deliverynotedataprovider.InServerSide = true;
                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");

                        StiDataColumnsCollection packagesLinesColumns = new StiDataColumnsCollection();
                        packagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns.Add("PackageType", typeof(string));
                        packagesLinesColumns.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns.Add("PackageVolume", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Pickup Note", Name = "DeliveryNoteDataProvider", BusinessObjectValue = deliverynotedataprovider };
                        StiBusinessObject packageLinesBusinessObject = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };
                        StiBusinessObject attachmentListBusinessObject = new StiBusinessObject() { Name = "AttachmentList", Alias = "AttachmentList", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(packageLinesBusinessObject);
                        currentBusinessObject.BusinessObjects.Add(attachmentListBusinessObject);

                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "999CI":
                case "999S":// Shipment invoice
                case "999M":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        InvoiceWebService invoiceWebService = new InvoiceWebService();
                        //byte[] byteArray = invoiceWebService.GetInvoiceData(childEntityId, documentTypeCopyId, tenant);
                        //MemoryStream memorystream = new MemoryStream(byteArray);
                        //XmlSerializer serializer = new XmlSerializer(typeof(InvoiceDataProvider));
                        InvoiceDataProvider invoicedataprovider = invoiceWebService.GetInvoiceDataProvider(childEntityId, documentTypeCopyId, tenant);//(InvoiceDataProvider)serializer.Deserialize(memorystream);
                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P");
                        BaseDataProviderService.FillBaseVariableFields(invoicedataprovider, tenant);


                        StiDataColumnsCollection reportInvoiceLines = new StiDataColumnsCollection();
                        reportInvoiceLines.Add("Description", typeof(string));
                        reportInvoiceLines.Add("Quantity", typeof(string));
                        reportInvoiceLines.Add("Measurement", typeof(string));
                        reportInvoiceLines.Add("UnitPrice", typeof(string));
                        reportInvoiceLines.Add("LocalAmount", typeof(string));
                        reportInvoiceLines.Add("ForeignAmount", typeof(string));
                        reportInvoiceLines.Add("InvoiceAmount", typeof(string));
                        reportInvoiceLines.Add("ForeignCurrency", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipment Invoice", Name = "InvoiceDataProvider", BusinessObjectValue = invoicedataprovider };
                        StiBusinessObject reportInvoiceLinesBusinessObject = new StiBusinessObject() { Name = "ReportInvoiceLine", Alias = "ReportInvoiceLine", ParentBusinessObject = currentBusinessObject, Columns = reportInvoiceLines };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(reportInvoiceLinesBusinessObject);
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                        break;
                    }

                case "999G":
                case "999C":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        InvoiceWebService invoiceWebService = new InvoiceWebService();
                        byte[] byteArray = invoiceWebService.GetInvoiceData(entityId, documentTypeCopyId, tenant);

                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(InvoiceDataProvider));
                        InvoiceDataProvider invoicedataprovider = (InvoiceDataProvider)serializer.Deserialize(memorystream);
                        theT2 = System.DateTime.Now.Ticks;
                        BaseDataProviderService.FillBaseVariableFields(invoicedataprovider, tenant);

                        StiDataColumnsCollection invoiceLinesColumnsCollection = new StiDataColumnsCollection();
                        invoiceLinesColumnsCollection.Add("Description", typeof(string));
                        invoiceLinesColumnsCollection.Add("Quantity", typeof(string));
                        invoiceLinesColumnsCollection.Add("Measurement", typeof(string));
                        invoiceLinesColumnsCollection.Add("UnitPrice", typeof(string));
                        invoiceLinesColumnsCollection.Add("LocalAmount", typeof(string));
                        invoiceLinesColumnsCollection.Add("ForeignAmount", typeof(string));
                        invoiceLinesColumnsCollection.Add("InvoiceAmount", typeof(string));
                        invoiceLinesColumnsCollection.Add("ForeignCurrency", typeof(string));

                        StiDataColumnsCollection constituentInvoicesColumnsCollection = new StiDataColumnsCollection();
                        constituentInvoicesColumnsCollection.Add("InvoiceNumber", typeof(string));
                        constituentInvoicesColumnsCollection.Add("BillToName", typeof(string));
                        constituentInvoicesColumnsCollection.Add("CustomerRef", typeof(string));
                        constituentInvoicesColumnsCollection.Add("InvoiceDate", typeof(string));
                        constituentInvoicesColumnsCollection.Add("InvoiceCurrencyCode", typeof(string));
                        constituentInvoicesColumnsCollection.Add("HouseNumber", typeof(string));
                        constituentInvoicesColumnsCollection.Add("MasterNumber", typeof(string));
                        constituentInvoicesColumnsCollection.Add("MainEntityReference", typeof(string));
                        constituentInvoicesColumnsCollection.Add("Shipper", typeof(string));
                        constituentInvoicesColumnsCollection.Add("Consignee", typeof(string));
                        constituentInvoicesColumnsCollection.Add("Carrier", typeof(string));
                        constituentInvoicesColumnsCollection.Add("CarrierNumber", typeof(string));

                        constituentInvoicesColumnsCollection.Add("DescriptionOfGoods", typeof(string));
                        constituentInvoicesColumnsCollection.Add("FromLocation", typeof(string));
                        constituentInvoicesColumnsCollection.Add("ToLocation", typeof(string));
                        constituentInvoicesColumnsCollection.Add("FinalDestination", typeof(string));

                        constituentInvoicesColumnsCollection.Add("Volume", typeof(string));
                        constituentInvoicesColumnsCollection.Add("GrossWeight", typeof(string));
                        constituentInvoicesColumnsCollection.Add("ChargeableWeight", typeof(string));
                        constituentInvoicesColumnsCollection.Add("PackagesQuantity", typeof(string));

                        constituentInvoicesColumnsCollection.Add("SubTotalInInvoiceCurrency", typeof(string));
                        constituentInvoicesColumnsCollection.Add("SubTotalInLocalCurrency", typeof(string));
                        constituentInvoicesColumnsCollection.Add("AmountInInvoiceCurrency", typeof(string));
                        constituentInvoicesColumnsCollection.Add("AmountInLocalCurrency", typeof(string));
                        constituentInvoicesColumnsCollection.Add("AmountInProfitCurrency", typeof(string));
                        constituentInvoicesColumnsCollection.Add("TotalVAT", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Consolidation Invoice", Name = "InvoiceDataProvider", BusinessObjectValue = invoicedataprovider };
                        StiBusinessObject StiBusinessObject1 = new StiBusinessObject() { Name = "ReportInvoiceLine", Alias = "ReportInvoiceLine", ParentBusinessObject = currentBusinessObject, Columns = invoiceLinesColumnsCollection };
                        StiBusinessObject StiBusinessObject2 = new StiBusinessObject() { Name = "ReportConstituentInvoiceLine", Alias = "ReportConstituentInvoiceLine", ParentBusinessObject = currentBusinessObject, Columns = constituentInvoicesColumnsCollection };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject1);
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject2);
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "CMR":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        CMRWebService cmrwebService = new CMRWebService();
                        byte[] byteArray = cmrwebService.GetDeliveryData(entityId, childEntityId, tenant, userId, documentTypeCopyId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(CMRDataProvider));
                        CMRDataProvider cmrDataProvider = (CMRDataProvider)serializer.Deserialize(memorystream);
                        theT2 = System.DateTime.Now.Ticks;
                        BaseDataProviderService.FillBaseVariableFields(cmrDataProvider, tenant);

                        StiDataColumnsCollection containerColumnsCollection = new StiDataColumnsCollection();
                        containerColumnsCollection.Add("Weight", typeof(string));
                        containerColumnsCollection.Add("MarksAndNumbers", typeof(string));
                        containerColumnsCollection.Add("HsCode", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "CMR", Name = "CMRDataProvider", BusinessObjectValue = cmrDataProvider };
                        StiBusinessObject StiBusinessObject1 = new StiBusinessObject() { Name = "ContainersList", Alias = "ContainersList", ParentBusinessObject = currentBusinessObject, Columns = containerColumnsCollection };
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject1);
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "SCMR":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ShipmentCMRWebService cmrwebService = new ShipmentCMRWebService();
                        byte[] byteArray = cmrwebService.GetDeliveryData(entityId, tenant, null, documentTypeCopyId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(CMRDataProvider));
                        CMRDataProvider cmrDataProvider = (CMRDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(cmrDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        // AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "SCMR", Name = "CMRDataProvider", BusinessObjectValue = cmrDataProvider };

                        // report.RegData("AWBDataProvider", awbDataProvider);
                        //report.Dictionary.BusinessObjects.Clear();
                        //report.Dictionary.BusinessObjects.Add(currentBusinessObject);
                        //report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
                        //report.Dictionary.SynchronizeBusinessObjects();
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "DELI":
                case "785A":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ManifestWebService cmrwebService = new ManifestWebService();
                        byte[] byteArray = cmrwebService.GetManifestData(entityId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ManifestDataProvider));
                        ManifestDataProvider manifestDataProvider = (ManifestDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(manifestDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        // AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "785A", Name = "ManifestDataProvider", BusinessObjectValue = manifestDataProvider };

                        // report.RegData("AWBDataProvider", awbDataProvider);
                        //report.Dictionary.BusinessObjects.Clear();
                        //report.Dictionary.BusinessObjects.Add(currentBusinessObject);
                        //report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
                        //report.Dictionary.SynchronizeBusinessObjects();
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "OMBC":
                case "785O":
                case "INMA":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ManifestWebService cmrwebService = new ManifestWebService();
                        byte[] byteArray = cmrwebService.GetManifestData(entityId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ManifestDataProvider));
                        ManifestDataProvider manifestDataProvider = (ManifestDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(manifestDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        // AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "785O", Name = "ManifestDataProvider", BusinessObjectValue = manifestDataProvider };

                        // report.RegData("AWBDataProvider", awbDataProvider);
                        //report.Dictionary.BusinessObjects.Clear();
                        //report.Dictionary.BusinessObjects.Add(currentBusinessObject);
                        //report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
                        //report.Dictionary.SynchronizeBusinessObjects();
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "PROF":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ShipmentProfitWebService shipmentprofitservice = new ShipmentProfitWebService();
                        byte[] byteArray = shipmentprofitservice.GetProfitData(entityId, tenant, null, null);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentProfitDataProvider));
                        ShipmentProfitDataProvider shipmentProfitProvider = (ShipmentProfitDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(shipmentProfitProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipment Profit", Name = "ShipmentProfitDataProvider", BusinessObjectValue = shipmentProfitProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "PAO":
                case "PAA":
                case "CPA":
                case "CPO":
                case "CPI":
                case "CPIO":
                case "CPE":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        PreAlertWebService prealertservice = new PreAlertWebService();
                        byte[] byteArray = prealertservice.GetPreAlertData(entityId, tenant, documentTypeId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(PreAlertDataProvider));
                        PreAlertDataProvider preAlertProvider = (PreAlertDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(preAlertProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Pre Alert", Name = "PreAlertDataProvider", BusinessObjectValue = preAlertProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }

                    break;

                case "ARP":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        PaymentWebService paymentService = new PaymentWebService();
                        PaymentDataProvider paymentProvider = paymentService.GetPaymentDataForAPi(entityId, tenant, documentTypeId);
                        BaseDataProviderService.FillBaseVariableFields(paymentProvider, tenant);

                        //MemoryStream memorystream = new MemoryStream(byteArray);
                        //XmlSerializer serializer = new XmlSerializer(typeof(PaymentDataProvider));
                        //PaymentDataProvider paymentProvider = (PaymentDataProvider)serializer.Deserialize(memorystream);
                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Paymant", Name = "paymentDataProvider", BusinessObjectValue = paymentProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }

                    break;

                case "APP":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        APPaymentWebService apPaymentService = new APPaymentWebService();
                        byte[] byteArray = apPaymentService.GetAPPaymentData(entityId, tenant, documentTypeId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(APPaymentDataProvider));
                        APPaymentDataProvider apPaymentProvider = (APPaymentDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(apPaymentProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "APPaymant", Name = "APPaymentDataProvider", BusinessObjectValue = apPaymentProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }

                    break;

                case "PALI":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ShipmentPackingWebService shipmentPackingService = new ShipmentPackingWebService();
                        byte[] byteArray = shipmentPackingService.GetShipmentPackingData(entityId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentPackingDataProvider));
                        ShipmentPackingDataProvider shipmentPackingProvider = (ShipmentPackingDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(shipmentPackingProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Packing List", Name = "ShipmentPackingDataProvider", BusinessObjectValue = shipmentPackingProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "PALN":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        ShipmentProfitWebService shipmentprofitservice = new ShipmentProfitWebService();
                        byte[] byteArray = shipmentprofitservice.GetProfitInvoicesData(entityId, tenant, null, null);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentProfitInvoicesDataProvider));
                        ShipmentProfitInvoicesDataProvider shipmentProfitProvider = (ShipmentProfitInvoicesDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(shipmentProfitProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipment Profit Invoice", Name = "ShipmentProfitInvoicesDataProvider", BusinessObjectValue = shipmentProfitProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "999P":// Shipment AP invoice
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        APInvoiceWebService invoiceWebService = new APInvoiceWebService();
                        APInvoiceDataProvider invoicedataprovider = invoiceWebService.GetAPInvoiceDataProvider(childEntityId, tenant);
                        theT2 = System.DateTime.Now.Ticks;

                        BaseDataProviderService.FillBaseVariableFields(invoicedataprovider, tenant);


                        StiDataColumnsCollection reportInvoiceLines = new StiDataColumnsCollection();
                        reportInvoiceLines.Add("ChargeTypeCode", typeof(string));
                        reportInvoiceLines.Add("ChargeTypeName", typeof(string));
                        reportInvoiceLines.Add("VatTypeName", typeof(string));
                        reportInvoiceLines.Add("VatTypePercentage", typeof(double));
                        reportInvoiceLines.Add("ExpectedAmount", typeof(double));
                        reportInvoiceLines.Add("OtherInvoicesAmount", typeof(double));
                        reportInvoiceLines.Add("ForeignAmount", typeof(double));
                        reportInvoiceLines.Add("ForeignCurrency", typeof(string));
                        reportInvoiceLines.Add("InvoiceAmount", typeof(double));
                        reportInvoiceLines.Add("OpenAmount", typeof(double));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipment AP Invoice", Name = "APInvoiceDataProvider", BusinessObjectValue = invoicedataprovider };
                        StiBusinessObject reportInvoiceLinesBusinessObject = new StiBusinessObject() { Name = "APReportInvoiceLine", Alias = "APReportInvoiceLine", ParentBusinessObject = currentBusinessObject, Columns = reportInvoiceLines };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(reportInvoiceLinesBusinessObject);
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                        break;
                    }

                case "999MP":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        APInvoiceWebService invoiceWebService = new APInvoiceWebService();
                        APInvoiceDataProvider invoicedataprovider = invoiceWebService.GetAPInvoiceDataProvider(entityId, tenant);
                        theT2 = System.DateTime.Now.Ticks;
                        BaseDataProviderService.FillBaseVariableFields(invoicedataprovider, tenant);

                        StiDataColumnsCollection multipleShipmentsColumnsCollection = new StiDataColumnsCollection();
                        multipleShipmentsColumnsCollection.Add("MasterNumber", typeof(string));
                        multipleShipmentsColumnsCollection.Add("HouseNumber", typeof(string));
                        multipleShipmentsColumnsCollection.Add("ShipmentNumber", typeof(string));
                        multipleShipmentsColumnsCollection.Add("PartnerName", typeof(string));
                        multipleShipmentsColumnsCollection.Add("ExpectedAmount", typeof(double));
                        multipleShipmentsColumnsCollection.Add("OpenAmount", typeof(double));
                        multipleShipmentsColumnsCollection.Add("Total", typeof(double));
                        multipleShipmentsColumnsCollection.Add("TotalVAT", typeof(double));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Multiple Shipment AP Invoice", Name = "APInvoiceDataProvider", BusinessObjectValue = invoicedataprovider };
                        StiBusinessObject StiBusinessObject2 = new StiBusinessObject() { Name = "APInvoiceMultipleEntity", Alias = "APInvoiceMultipleEntity", ParentBusinessObject = currentBusinessObject, Columns = multipleShipmentsColumnsCollection };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject2);
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                        break;
                    }

                case "POA":
                case "OPPA":
                case "OPPB":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        OpportunitySummaryWebService opportunitySummaryWebService = new OpportunitySummaryWebService();
                        OpportunitySummaryDataProvider opportunitySummaryDataProvider = opportunitySummaryWebService.GetOpportunitySummaryDataProvider(entityId, tenant, documentTypeCode);
                        theT2 = System.DateTime.Now.Ticks;
                        BaseDataProviderService.FillBaseVariableFields(opportunitySummaryDataProvider, tenant);

                        StiDataColumnsCollection opportunityProductColumnsCollection = new StiDataColumnsCollection();
                        opportunityProductColumnsCollection.Add("ProductName", typeof(string));
                        opportunityProductColumnsCollection.Add("ProductSummary", typeof(string));
                        opportunityProductColumnsCollection.Add("ProductNote", typeof(string));
                        opportunityProductColumnsCollection.Add("ProductPrepaidCollect", typeof(string));

                        StiDataColumnsCollection opportunityAdditionalSeviceColumnsCollection = new StiDataColumnsCollection();
                        opportunityAdditionalSeviceColumnsCollection.Add("AdditionalServiceName", typeof(string));
                        opportunityAdditionalSeviceColumnsCollection.Add("AdditionalServiceNote", typeof(string));

                        StiDataColumnsCollection opportunityTaskColumnsCollection = new StiDataColumnsCollection();
                        opportunityTaskColumnsCollection.Add("TaskSubject", typeof(string));
                        opportunityTaskColumnsCollection.Add("TaskOwner", typeof(string));
                        opportunityTaskColumnsCollection.Add("TaskDueDate", typeof(DateTime));

                        StiDataColumnsCollection opportunityContactolumnsCollection = new StiDataColumnsCollection();
                        opportunityContactolumnsCollection.Add("ContactName", typeof(string));
                        opportunityContactolumnsCollection.Add("ContactPhone", typeof(string));
                        opportunityContactolumnsCollection.Add("ContactEmail", typeof(string));
                        opportunityContactolumnsCollection.Add("ContactNotes", typeof(string));
                        opportunityContactolumnsCollection.Add("ContactPosition", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Opportunity Summary", Name = "OpportunitySummaryDataProvider", BusinessObjectValue = opportunitySummaryDataProvider };
                        StiBusinessObject StiBusinessObject1 = new StiBusinessObject() { Name = "OpportunityProducts", Alias = "OpportunityProducts", ParentBusinessObject = currentBusinessObject, Columns = opportunityProductColumnsCollection };
                        StiBusinessObject StiBusinessObject2 = new StiBusinessObject() { Name = "OpportunityAdditionalServices", Alias = "OpportunityAdditionalServices", ParentBusinessObject = currentBusinessObject, Columns = opportunityAdditionalSeviceColumnsCollection };
                        StiBusinessObject StiBusinessObject3 = new StiBusinessObject() { Name = "OpportunityTasks", Alias = "OpportunityTasks", ParentBusinessObject = currentBusinessObject, Columns = opportunityTaskColumnsCollection };
                        StiBusinessObject StiBusinessObject4 = new StiBusinessObject() { Name = "OpportunityContacts", Alias = "OpportunityContacts", ParentBusinessObject = currentBusinessObject, Columns = opportunityContactolumnsCollection };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject1);
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject2);
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject3);
                        currentBusinessObject.BusinessObjects.Add(StiBusinessObject4);
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);

                        break;
                    }
                case "CDE":
                case "WHL":
                    {
                        theT1 = System.DateTime.Now.Ticks;
                        CrossDockEntryDataProviderHelper crossDockEntryDataProviderHelper = new CrossDockEntryDataProviderHelper();
                        byte[] byteArray = crossDockEntryDataProviderHelper.LoadDataToCrossDockEntryDataProvider(entityId, tenant, userId);

                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(CrossDockEntryDataProvider));
                        CrossDockEntryDataProvider crossDockEntryDataProvider = (CrossDockEntryDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(crossDockEntryDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "CrossDockEntry", Name = "CrossDockEntryDataProvider", BusinessObjectValue = crossDockEntryDataProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "CDR":
                    {

                        theT1 = System.DateTime.Now.Ticks;
                        CrossDockReleaseDataProviderHelper crossDockReleaseDataProviderHelper = new CrossDockReleaseDataProviderHelper();
                        byte[] byteArray = crossDockReleaseDataProviderHelper.LoadDataToCrossDockReleaseDataProvider(entityId, tenant);

                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(CrossDockReleaseDataProvider));
                        CrossDockReleaseDataProvider crossDockReleaseDataProvider = (CrossDockReleaseDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(crossDockReleaseDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "CrossDockRelease", Name = "CrossDockReleaseDataProvider", BusinessObjectValue = crossDockReleaseDataProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "CRR":
                    {

                        theT1 = System.DateTime.Now.Ticks;
                        CrossDockReleaseDataProviderHelper crossDockReleaseDataProviderHelper = new CrossDockReleaseDataProviderHelper();
                        byte[] byteArray = crossDockReleaseDataProviderHelper.LoadCrossDockReleaseDataProvider_GroupByEntry(entityId, tenant);

                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(CrossDockReleaseDataProvider));
                        CrossDockReleaseDataProvider crossDockReleaseDataProvider = (CrossDockReleaseDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(crossDockReleaseDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "CrossDockRelease", Name = "CrossDockReleaseDataProvider", BusinessObjectValue = crossDockReleaseDataProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "INVS":
                    {

                        theT1 = System.DateTime.Now.Ticks;
                        ShipmentInventoryDataProviderHelper shipmentInventoryDataProviderHelper = new ShipmentInventoryDataProviderHelper();
                        byte[] byteArray = shipmentInventoryDataProviderHelper.LoadDataToShipmentInventoryDataProvider(entityId, tenant);

                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(ShipmentInventoryDataProvider));
                        ShipmentInventoryDataProvider shipmentInventoryDataProvider = (ShipmentInventoryDataProvider)serializer.Deserialize(memorystream);
                        BaseDataProviderService.FillBaseVariableFields(shipmentInventoryDataProvider, tenant);

                        theT2 = System.DateTime.Now.Ticks;
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipment Inventory", Name = "ShipmentInventoryDataProvider", BusinessObjectValue = shipmentInventoryDataProvider };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "SBOLP":
                    {
                        OceanExportWebService oceanWebService = new OceanExportWebService();
                        byte[] byteArray = oceanWebService.GetFBLDataForPickUp(entityId, childEntityId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(FBLDataProvider));
                        FBLDataProvider fbLdataprovider = (FBLDataProvider)serializer.Deserialize(memorystream);
                        fbLdataprovider.InServerSide = true;
                        theT2 = System.DateTime.Now.Ticks;
                        BaseDataProviderService.FillBaseVariableFields(fbLdataprovider, tenant);


                        StiDataColumnsCollection packagesLinesColumns = new StiDataColumnsCollection();
                        packagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns.Add("PackageType", typeof(string));
                        packagesLinesColumns.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns.Add("PackageVolume", typeof(string));
                        packagesLinesColumns.Add("PackageQuantityAndType", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "FBL", Name = "FBLDataProvider", BusinessObjectValue = fbLdataprovider };
                        StiBusinessObject packageLinesBusinessObject = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(packageLinesBusinessObject);

                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                        break;
                    }

                case "WELB":
                    {
                        CrossDockEntryLabelDataProviderHelper crossDockEntryLabelDataProviderHelper = new CrossDockEntryLabelDataProviderHelper();
                        byte[] byteArray = crossDockEntryLabelDataProviderHelper.LoadCrossDockEntryLabelDataProvider(entityId, tenant);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<CrossDockEntryDataProvider>));
                        List<CrossDockEntryDataProvider> crossDockEntryDataProviderLists = (List<CrossDockEntryDataProvider>)serializer.Deserialize(memorystream);
                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Cross Docks Entry Labels", Name = "CrossDockEntryDataProvider", BusinessObjectValue = crossDockEntryDataProviderLists };
                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant);
                    }
                    break;

                case "TEST":
                    {
                        //FBLDataProvider
                        OceanExportWebService oceanWebService = new OceanExportWebService();
                        byte[] byteArray = oceanWebService.GetFBLData(entityId, tenant, documentTypeCopyId);
                        MemoryStream memorystream = new MemoryStream(byteArray);
                        XmlSerializer serializer = new XmlSerializer(typeof(FBLDataProvider));
                        FBLDataProvider fbLdataprovider = (FBLDataProvider)serializer.Deserialize(memorystream);
                        fbLdataprovider.InServerSide = true;
                        theT2 = System.DateTime.Now.Ticks;
                        //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                        BaseDataProviderService.FillBaseVariableFields(fbLdataprovider, tenant);

                        StiDataColumnsCollection packagesLinesColumns = new StiDataColumnsCollection();
                        packagesLinesColumns.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns.Add("PackageType", typeof(string));
                        packagesLinesColumns.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns.Add("PackageVolume", typeof(string));
                        packagesLinesColumns.Add("PackageQuantityAndType", typeof(string));

                        StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "FBL", Name = "FBLDataProvider", BusinessObjectValue = fbLdataprovider };
                        StiBusinessObject packageLinesBusinessObject = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };
                        StiBusinessObject attachmentListBusinessObject = new StiBusinessObject() { Name = "AttachmentList", Alias = "AttachmentList", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns };

                        report.Dictionary.BusinessObjects.Clear();
                        currentBusinessObject.BusinessObjects.Add(packageLinesBusinessObject);
                        currentBusinessObject.BusinessObjects.Add(attachmentListBusinessObject);


                        //ShippingDeclarationDataProvider
                        ShippingDeclarationWebService shippingDeclarationWebService = new ShippingDeclarationWebService();
                        byte[] byteArray2 = shippingDeclarationWebService.GetShippingDeclarationData(entityId, tenant, documentTypeCode, documentTypeCopyId);
                        MemoryStream memorystream2 = new MemoryStream(byteArray2);
                        XmlSerializer serializer2 = new XmlSerializer(typeof(ShippingDeclarationDataProvider));
                        ShippingDeclarationDataProvider shippingDeclarationdataprovider = (ShippingDeclarationDataProvider)serializer2.Deserialize(memorystream2);
                        theT2 = System.DateTime.Now.Ticks;
                        BaseDataProviderService.FillBaseVariableFields(shippingDeclarationdataprovider, tenant);




                        StiDataColumnsCollection packagesLinesColumns2 = new StiDataColumnsCollection();
                        packagesLinesColumns2.Add("PackageMarksAndNumbers", typeof(string));
                        packagesLinesColumns2.Add("PackageQuantity", typeof(string));
                        packagesLinesColumns2.Add("PackageType", typeof(string));
                        packagesLinesColumns2.Add("PackageDescriptionOfGoods", typeof(string));
                        packagesLinesColumns2.Add("PackageGrossWeight", typeof(string));
                        packagesLinesColumns2.Add("PackageVolume", typeof(string));
                        packagesLinesColumns2.Add("PackageQuantityAndType", typeof(string));

                        StiBusinessObject currentBusinessObject2 = new StiBusinessObject() { Category = "Shipping declaration", Name = "ShippingDeclarationDataProvider", BusinessObjectValue = shippingDeclarationdataprovider };
                        StiBusinessObject packageLinesBusinessObject2 = new StiBusinessObject() { Name = "PackagesLines", Alias = "PackagesLines", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns2 };
                        StiBusinessObject attachmentListBusinessObject2 = new StiBusinessObject() { Name = "AttachmentList", Alias = "AttachmentList", ParentBusinessObject = currentBusinessObject, Columns = packagesLinesColumns2 };

                        currentBusinessObject2.BusinessObjects.Add(packageLinesBusinessObject2);
                        currentBusinessObject2.BusinessObjects.Add(attachmentListBusinessObject2);


                        report = LoadandRender(defaulttemplate, currentBusinessObject, tenant, currentBusinessObject2);
                    }
                    break;


            }

            return report;
        }

        public string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private byte[] ExportDocumentReportToPDF(StiReport report, int tenant, string documenttypecode)
        {
            byte[] reprotPdfData = null;
            StiPdfExportSettings pdfSettings = new StiPdfExportSettings();
            if (documenttypecode == "740L" || documenttypecode == "740HL" || documenttypecode == "LCLL")
            {
                pdfSettings.ImageFormat = StiImageFormat.Monochrome;
                pdfSettings.ImageResolution = 100;
                pdfSettings.ImageQuality = 75;
                pdfSettings.ImageCompressionMethod = StiPdfImageCompressionMethod.Jpeg;


            }
            else
            {
                pdfSettings.ImageResolution = 300;
                pdfSettings.ImageQuality = 100;
                pdfSettings.ImageCompressionMethod = StiPdfImageCompressionMethod.Jpeg;
            }



            //Clara's tenant : 497
            if (tenant == 497)
            {
                pdfSettings.EmbeddedFonts = true;
            }

            using (MemoryStream memStream = new MemoryStream())
            {
                report.ExportDocument(StiExportFormat.Pdf, memStream, pdfSettings);
                reprotPdfData = memStream.ToArray(); ;
            }

            return reprotPdfData;



            //}
            //}
            //catch (Exception e)
            //{
            //    string ip = "";
            //    if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //    {
            //        ip = HttpContext.Current.Request.UserHostAddress;
            //    }
            //    ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "ExportDocument : SaveSTIDocumentInStorage Method", ip);
            //}
        }

        private void SaveSTIDocumentInStorage(Document document, byte[] reportPdfData, int tenant)
        {
            if (reportPdfData != null)
            {
                string filename = document.Id + "." + document.Extension;

                //byte[] data = memStream.ToArray();
                document.FileSize = (int)reportPdfData.Length;


                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = reportPdfData.Length,

                };
                storageservice.Write(reportPdfData, fileInfo);
            }

        }

        public void SaveRichDocumentInStorage(Document document, byte[] data, int tenant)
        {
            try
            {
                //if (!WebFreightEntryPoint.UsingAzure)
                //{
                //    string path = Server.MapPath(".");
                //    path += "\\UserUploads\\";
                //    path += document.Id;
                //    path += "."+document.Extension;
                //    FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                //    BinaryWriter bw = new BinaryWriter(fs);
                //    bw.Write(data);
                //    bw.Close();
                //}
                //else
                //{
                string filename = document.Id + "." + document.Extension;
                //var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));
                //int filesize;


                //using (Stream blobstream = blobfile.OpenWrite())
                //{
                //    T1 = System.DateTime.Now.Ticks;
                //    MemoryStream memStream = new MemoryStream();

                //    blobstream.Write(data, 0, data.Length);
                //    t2 = System.DateTime.Now.Ticks;
                //    //AzureLog.SaveLogsInStorage("Report.ExportDocument :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                //    //filesize = (Convert.ToInt32(blobstream.Length)); // File size in KByte
                //}
                //}

                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = data.Length,

                };
                storageservice.Write(data, fileInfo);
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "ExportDocument : SaveRichDocumentInStorage Method", ip);
            }
        }



        public void AddLogo(StiReport report, byte[] logodata)
        {
            if (logodata != null)
            {
                Image image = Image.FromStream(new MemoryStream(logodata));
                report["Logo"] = image;
                StiVariable logo = new StiVariable("My variables", "Logo", "Logo", "", typeof(System.Drawing.Bitmap), "", false, false, false);//new StiVariable("My Variables", "Logo", "Logo", image);// // ***********
                logo.ValueObject = image;
                // _report.Dictionary.Variables.Clear();
                report.Dictionary.Variables.Add(logo);
                report.Dictionary.Synchronize();
            }
        }

        public long T1
        {
            get { return t1; }
            set { t1 = value; }
        }

        public byte[] GetDllFromStorage(string dllname, int tenant)
        {
            byte[] result = null;

            try
            {
                if (LogitudeSettings.UsingAzure)
                {
                    var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);

                    if (blobContainer != null)
                    {
                        var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(dllname, "dlls"));
                        if (BlobExtensions.Exists(blobfile))
                        {
                            using (MemoryStream memstream = new MemoryStream())
                            {
                                blobfile.DownloadToStream(memstream);
                                result = memstream.ToArray();
                            }
                        }
                    }
                }
                else
                {

                    string path = HttpContext.Current.Server.MapPath(".");
                    path += "\\stidlls\\";
                    path += dllname;

                    if (File.Exists(path))
                    {
                        using (FileStream fs = File.OpenRead(path))
                        {
                            result = new byte[fs.Length];
                            fs.Read(result, 0, result.Length);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "ExportDocument : GetDllFromStorage Method", ip);
            }
            return result;
        }

        public void SaveDllFileInStorage(byte[] template, StiReport report, int tenant)
        {
            try
            {
                string dllname = GetDllName(template, report);

                if (LogitudeSettings.UsingAzure)
                {
                    var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);

                    var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(dllname, "dlls"));

                    using (Stream blobstream = blobfile.OpenWrite())
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            report.Compile(memStream);
                            blobstream.Write(memStream.ToArray(), 0, (int)memStream.Length);
                        }
                    }
                }
                else
                {
                    string path = HttpContext.Current.Server.MapPath(".");
                    path += "\\stidlls\\";
                    path += dllname;

                    report.Compile(path);
                }
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "ExportDocument : SaveDllFileInStorage Method", ip);
            }
        }

        public string GetDllName(byte[] template, StiReport report)
        {
            string result = "";

            result += ComputeHash(template) + "_";
            result += System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion().ToString() + "_";
            result += StiVersion.Version;
            // result += _report.GetReportAssemblyCacheName(); 
            result += ".dll";

            return result.ToLower();
        }

        public static int ComputeHash(params byte[] data)
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < data.Length; i++)
                    hash = (hash ^ data[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
            }
        }

        public StiReport LoadandRender(DocumentTypeTemplate defaulttemplate, StiBusinessObject currentBusinessObject, int tenant, StiBusinessObject otherstiBusinessObject = null)
        {
            StiReport report = new StiReport();
            if (otherstiBusinessObject != null)
            {
                RegBusinessObject(report, otherstiBusinessObject);
            }


            long theT1 = System.DateTime.Now.Ticks;
            long theT2 = System.DateTime.Now.Ticks;

            byte[] template = defaulttemplate.TemplateBody;

            string dllName = GetDllName(template, report);

            Uploader uploaderservice = new Uploader();
            byte[] logodata = uploaderservice.DownloadFile("logo" + tenant, "jpg", "logos", tenant);

            if (WebFreightEntryPoint.IsComplieEnabled)
            {
                theT1 = System.DateTime.Now.Ticks;

                byte[] dllData = GetDllFromStorage(dllName, tenant);
                theT2 = System.DateTime.Now.Ticks;

                if (dllData != null && dllData.Count() != 0)
                {
                    theT1 = System.DateTime.Now.Ticks;
                    report = StiReport.GetReportFromAssembly(dllData);

                    theT2 = System.DateTime.Now.Ticks;

                    RegBusinessObject(report, currentBusinessObject);
                    report.NeedsCompiling = false;
                    report.ReportUnit = StiReportUnitType.Millimeters;
                    if (defaulttemplate.HorizontalShift != null && defaulttemplate.VerticalShift != null)
                    {
                        foreach (StiPage page in report.Pages)
                        {
                            page.Margins.Left = 10;
                            page.Margins.Top = 10;
                            page.Margins = new StiMargins(page.Margins.Left + defaulttemplate.HorizontalShift.Value, page.Margins.Right, page.Margins.Top + defaulttemplate.VerticalShift.Value, page.Margins.Bottom);
                        }
                    }

                    AddLogo(report, logodata);
                }
                else
                {
                    RegBusinessObject(report, currentBusinessObject);

                    theT1 = System.DateTime.Now.Ticks;
                    report.Load(template);
                    theT2 = System.DateTime.Now.Ticks;
                    report.ReportUnit = StiReportUnitType.Millimeters;
                    if (defaulttemplate.HorizontalShift != null && defaulttemplate.VerticalShift != null)
                    {
                        foreach (StiPage page in report.Pages)
                        {
                            page.Margins = new StiMargins(page.Margins.Left + defaulttemplate.HorizontalShift.Value, page.Margins.Right, page.Margins.Top + defaulttemplate.VerticalShift.Value, page.Margins.Bottom);
                        }
                    }

                    AddLogo(report, logodata);
                    SaveDllFileInStorage(template, report, tenant);
                }
            }
            else
            {
                RegBusinessObject(report, currentBusinessObject);

                theT1 = System.DateTime.Now.Ticks;
                report.Load(template);
                theT2 = System.DateTime.Now.Ticks;

                report.ReportUnit = StiReportUnitType.Millimeters;

                if (defaulttemplate.HorizontalShift != null && defaulttemplate.VerticalShift != null)
                {
                    foreach (StiPage page in report.Pages)
                    {
                        page.Margins = new StiMargins(page.Margins.Left + defaulttemplate.HorizontalShift.Value, page.Margins.Right, page.Margins.Top + defaulttemplate.VerticalShift.Value, page.Margins.Bottom);
                    }
                }

                AddLogo(report, logodata);

            }

            report.AutoLocalizeReportOnRun = true;
            theT1 = System.DateTime.Now.Ticks;
            report.Render(false);

            theT2 = System.DateTime.Now.Ticks;

            return report;
        }


        public void RegBusinessObject(StiReport report, StiBusinessObject currentBusinessObject)
        {
            report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
        }


        public byte[] GetDocumentTypebyte(string documentTypeTemplateId, string entityId, string entityObjectTableId, string childEntityId, string childObjectTableId, int tenant)
        {
            byte[] data = null;
            var currentthreaduser = Thread.CurrentPrincipal;
            long theT1 = new long();
            long theT2 = new long();
            long theA1 = new long();
            long theA2 = new long();
            theA1 = System.DateTime.Now.Ticks;
            Byte[] templatedata = null;
            //try
            //{
            DocumentTypeRepository repository = new DocumentTypeRepository(0);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(0);
            DocumentRepository docRepository = new DocumentRepository(0);
            DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(0);
            DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(0);
            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(0);
            // DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);

            DocumentTypeCopy documentTypeCopy = null;


            DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentTypeTemplateId);

            if (defaulttemplate != null)
            {

                DocumentType documentType = repository.GetSingleDocumentTypes(defaulttemplate.DocumentTypeId, 0);
                if (documentType != null)
                {
                    documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, 0);
                }

                if (defaulttemplate != null)
                    templatedata = defaulttemplate.TemplateBody;

                if (templatedata != null)
                {
                    if (templatedata.Length != 0)
                    {


                        StiReport report = GetReportDocument(documentType, entityId, entityObjectTableId, childEntityId, childObjectTableId, documentTypeCopy, templatedata, defaulttemplate, tenant, theT1, theT2, theA1, theA2);
                        theA2 = System.DateTime.Now.Ticks;

                        StiPdfExportSettings pdfSettings = new StiPdfExportSettings();
                        pdfSettings.ImageResolution = 300;
                        pdfSettings.ImageQuality = 100;
                        pdfSettings.ImageCompressionMethod = StiPdfImageCompressionMethod.Jpeg;


                        using (MemoryStream memStream = new MemoryStream())
                        {
                            report.ExportDocument(StiExportFormat.Pdf, memStream, pdfSettings);
                            data = memStream.ToArray();
                        }
                        return data;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
            //}
            //catch (Exception e)
            //{
            //    ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "ExportDocument : ExportDocument2Pdf Method");
            //    return null;
            //}            
        }

        public string SaveEditedReportToServer(string documentOutId, byte[] pdfDataFile, byte[] xamlDataFile, int tenant, string documentTypeCopyId, string documentTypeId = null, string entityId = null, string childEntityId = null)
        {
            DocumentTypeRepository repository = new DocumentTypeRepository(tenant);
            DocumentOutRepository internalDocumentRepository = new DocumentOutRepository(tenant);
            DocumentRepository docRepository = new DocumentRepository(tenant);
            DocumentOut documentOut = internalDocumentRepository.GetSingleDocumentOut(documentOutId, tenant);
            DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(tenant);
            DocumentOutCopyRepository documentOutCopyRep = new DocumentOutCopyRepository(tenant);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);
            DocumentTypeCopy documentTypeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopyByDocumentTypeId(documentTypeCopyId, tenant);
            DocumentType documentType = repository.GetSingleDocumentTypes(documentTypeId, tenant);
            DocumentOutCopy documentOutCopy = documentOutCopyRep.GetDocumentOutCopyByDocumentOutAndType(documentOutId, documentTypeCopyId, tenant);
            Document document = null;

            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
            string calculatedFileName = "";
            if (documentType != null)
            {
                if (!string.IsNullOrEmpty(documentType.FileName))
                {

                    if (documentType.FileName.Contains("["))
                    {
                        string id = entityId;
                        if (!string.IsNullOrEmpty(childEntityId)) id = childEntityId;
                        HtmlEditorResolveArgs htmlResolveArgs = new HtmlEditorResolveArgs
                        {
                            EntityId = id,
                            ObjectTableId = documentType.ObjectTableId,
                            HtmlString = documentType.FileName,
                            UserId = "",
                            Tenant = tenant,
                        };
                        string htmlResolve = htmlEditorHelper.ResolveHtmlString(htmlResolveArgs);
                        if (!string.IsNullOrEmpty(htmlResolve))
                        {
                            if (htmlResolve.Length > 120)
                            {
                                calculatedFileName = htmlResolve.Substring(0, 119);
                            }
                            else calculatedFileName = htmlResolve;
                        }

                    }
                    else calculatedFileName = documentType.FileName;
                }

                if (string.IsNullOrEmpty(calculatedFileName)) calculatedFileName = documentType.Name;

                if (documentTypeCopy != null && documentType.Name != documentTypeCopy.Name)
                {
                    calculatedFileName += "_" + documentTypeCopy.Name;
                }
            }

            if (documentOutCopy == null)
            {
                string copyId = IdCounter.GetNumber("Document", tenant).ToString();
                documentOutCopy = new DocumentOutCopy()
                {
                    Id = copyId,
                    Tenant = tenant,
                    DocumentTypeCopyId = documentTypeCopy != null ? documentTypeCopy.Id : "",
                    DocumentOutId = documentOutId,
                    //DocumentId = copyId,
                };

                document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "pdf",
                    FileSize = pdfDataFile.Length,
                    Tenant = Convert.ToInt32(documentOut.Tenant),
                    Id = documentOutCopy.Id,
                    HasFile = true,
                    Folder = "docsout",
                    CalculatedFileName = calculatedFileName,
                };
                docRepository.Add(document);
                documentOutCopyRep.Add(documentOutCopy);
                documentOutCopyRep.SubmitChanges();
            }
            else
            {
                document = docRepository.GetSingleDocument(tenant, documentOutCopy.Id);
                document.CreateDate = DateTime.Now;
                document.Extension = "pdf";
                document.FileSize = pdfDataFile.Length;
                document.HasFile = true;
                document.Folder = "docsout";
                document.CalculatedFileName = calculatedFileName;
            }

            Document xamlDocument = null;
            if (documentOut.XamlDocumentId != null)
            {
                xamlDocument = docRepository.GetSingleDocument(tenant, documentOut.XamlDocumentId);

                xamlDocument.CreateDate = DateTime.Now;
                xamlDocument.Extension = "xaml";
                xamlDocument.FileSize = xamlDataFile.Length;
                xamlDocument.HasFile = true;
                xamlDocument.Folder = "docsout";
                xamlDocument.IsEncrypted = true;
                docRepository.Update(xamlDocument);
            }
            else
            {
                xamlDocument = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xaml",
                    FileSize = xamlDataFile.Length,
                    Tenant = Convert.ToInt32(documentOut.Tenant),
                    Id = IdCounter.GetNumber("Document", tenant).ToString(),
                    HasFile = true,
                    Folder = "docsout",
                };

                docRepository.Add(xamlDocument);
            }

            docRepository.SubmitChanges();
            documentOutCopy.DocumentId = document.Id;
            documentOutCopyRep.Update(documentOutCopy);
            documentOutCopyRep.SubmitChanges();

            //internalDocument.DocumentId = document.Id;
            documentOut.XamlDocumentId = xamlDocument.Id;
            internalDocumentRepository.Update(documentOut);
            internalDocumentRepository.SubmitChanges();

            //docId = internalDocument.DocumentId;
            //"C:\Program Files\Common Files\Microsoft Shared\DevServer\10.0\WebDev.WebServer40.exe" /port:56454 /path:"C:\SourceCode\WebFreight.Web" /vpath:"/"

            SaveRichDocumentInStorage(document, pdfDataFile, tenant);
            SaveRichDocumentInStorage(xamlDocument, xamlDataFile, tenant);

            //string path = Server.MapPath(".");
            //path += "\\UserUploads\\";
            //path += document.Id;
            //path += ".pdf";            
            //    FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            //    BinaryWriter bw = new BinaryWriter(fs);
            //    bw.Write(data);
            //    bw.Close();
            return document.Id + "," + xamlDocument.Id;
        }

        public byte[] DownloadFileFromServer(string documentId, int tenant)
        {
            DocumentRepository docRepository = new DocumentRepository(tenant);
            Document document = docRepository.GetSingleDocument(tenant, documentId);

            CloudBlobContainer blobContainer;
            byte[] datainByte;

            //if (!WebFreightEntryPoint.UsingAzure)
            //{
            //    byte[] resultFile = null;
            //    try
            //    {
            //        string path = Server.MapPath(".");
            //        path += "\\UserUploads\\";
            //        path += documentId;
            //        path += "." + document.Extension;
            //        FileStream fs = File.OpenRead(path);
            //        resultFile = new byte[fs.Length];
            //        fs.Read(resultFile, 0, resultFile.Length);
            //        fs.Close();                   
            //    }
            //    catch (Exception e)
            //    {
            //        ExceptionHandler.HandleException(e, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
            //    }
            //    return resultFile;
            //}
            //else // In Azure
            //{
            try
            {

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = document.Tenant,
                    FileSize = document.FileSize,
                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                datainByte = storageservice.Read(fileInfo);

                return datainByte;


                //string filename = documentId + "." + document.Extension;

                //blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                //if (blobfile.Exists())
                //{
                //    using (MemoryStream memstream = new MemoryStream())
                //    {
                //        blobfile.DownloadToStream(memstream);
                //        datainByte = memstream.ToArray();
                //    }
                //    return datainByte;
                //}
                //else
                //    return null;
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "ExportDocument : DownloadFileFromServer Method", ip);
                return null;
            }
            //}
        }

        public string BuildInvoiceDocument(string invoiceId, string documentOutId, int tenant)
        {
            long theT1;
            long theT2;
            StiReport report = new StiReport();
            theT1 = System.DateTime.Now.Ticks;

            Byte[] template = null;
            try
            {
                DocumentTypeRepository repository = new DocumentTypeRepository(tenant);
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);
                DocumentRepository docRepository = new DocumentRepository(tenant);
                DocumentTypeTemplateRepository documentTypeTemplaterep = new DocumentTypeTemplateRepository(tenant);
                DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
                DocumentTypeTemplate defaulttemplate = documentTypeTemplaterep.GetSingleDocumentTypeTemplate(documentOut.DocumentTemplateId);

                if (defaulttemplate != null)
                    template = defaulttemplate.TemplateBody;

                InvoiceWebService invoiceWebService = new InvoiceWebService();
                byte[] byteArray = invoiceWebService.GetInvoiceData(invoiceId, null, tenant);
                MemoryStream memorystream = new MemoryStream(byteArray);
                XmlSerializer serializer = new XmlSerializer(typeof(InvoiceDataProvider));
                InvoiceDataProvider invoicedataprovider = (InvoiceDataProvider)serializer.Deserialize(memorystream);
                theT2 = System.DateTime.Now.Ticks;
                //AzureLog.SaveLogsInStorage("Data provider :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                invoicedataprovider.Today_DateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                StiDataColumnsCollection reportInvoiceLines = new StiDataColumnsCollection();
                reportInvoiceLines.Add("Description", typeof(string));
                reportInvoiceLines.Add("Quantity", typeof(string));
                reportInvoiceLines.Add("Measurement", typeof(string));
                reportInvoiceLines.Add("UnitPrice", typeof(string));
                reportInvoiceLines.Add("LocalAmount", typeof(string));
                reportInvoiceLines.Add("ForeignAmount", typeof(string));
                reportInvoiceLines.Add("InvoiceAmount", typeof(string));
                reportInvoiceLines.Add("ForeignCurrency", typeof(string));

                StiBusinessObject currentBusinessObject = new StiBusinessObject() { Category = "Shipment Invoice", Name = "InvoiceDataProvider", BusinessObjectValue = invoicedataprovider };
                StiBusinessObject reportInvoiceLinesBusinessObject = new StiBusinessObject() { Name = "ReportInvoiceLine", Alias = "ReportInvoiceLine", ParentBusinessObject = currentBusinessObject, Columns = reportInvoiceLines };

                report.Dictionary.BusinessObjects.Clear();
                currentBusinessObject.BusinessObjects.Add(reportInvoiceLinesBusinessObject);

                report.Dictionary.BusinessObjects.Add(currentBusinessObject);
                report.RegBusinessObject(currentBusinessObject.Category, currentBusinessObject.Name, currentBusinessObject.BusinessObjectValue);
                report.Dictionary.SynchronizeBusinessObjects();

                theT1 = System.DateTime.Now.Ticks;
                report.Load(template);
                theT2 = System.DateTime.Now.Ticks;
                //AzureLog.SaveLogsInStorage("Report.Load() :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");
                // StiOptions.Export.Pdf.AllowEditablePdf = true;

                theT1 = System.DateTime.Now.Ticks;
                report.Render();
                theT2 = System.DateTime.Now.Ticks;
                //AzureLog.SaveLogsInStorage("Report.Render() :" + Convert.ToString((t2 - t1) / TimeSpan.TicksPerMillisecond), "P", tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "");

                return "";
            }
            catch (Exception e)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "ExportDocument : BuildInvoiceDocument Method", ip);
                return null;
            }
        }



        public List<object> GetDownloadFileFromServer(string documentId, int tenant)
        {
            List<object> htmlResult = new List<object>();
            HtmlEditorHelper htmlEditorHelper = new Helpers.HtmlEditorHelper();

            byte[] data = DownloadFileFromServer(documentId, tenant);

            if (data != null)
            {
                var result = System.Text.Encoding.UTF8.GetString(data);

                if (result.Contains(":RadDocument"))
                {
                    htmlResult = htmlEditorHelper.GetTemplatePartsAsHtml(data);



                }
                else
                {
                    htmlResult = htmlEditorHelper.GetTemplatePartFromHtml(result);

                }

            }
            return htmlResult;
        }


        public static DateTime? ExceptionDateExportPdfDocumentWebService { get; set; }
        public bool IsCallBuildDocumentReportWebService(int tenant)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(LogitudeSettings.CPUIntensiveWebServicesURL) && FeatureToggleHelper.HasFeatureToggle("BDR", tenant))
            {
                if (ExceptionDateExportPdfDocumentWebService == null) result = true;
                else
                {
                    DateTime nowDate = DateTime.Now;
                    DateTime endDate = (DateTime)ExceptionDateExportPdfDocumentWebService;
                    if (endDate.AddMinutes(5) < nowDate)
                    {
                        result = true;
                    }
                }
            }
            return result;

        }

        public DocumentsExecutionLog GetNewInStanceFromDocumentsExecutionLog(ExportDocumentArgs exportDocumentArgs)
        {
            DocumentsExecutionLogRepository documentsExecutionLogRepository = new DocumentsExecutionLogRepository(exportDocumentArgs.Tenant);
            DocumentsExecutionLog documentsExecutionLog = new DocumentsExecutionLog()
            {
                Id = IdCounter.GetNumber("DocumentsExecutionLog", exportDocumentArgs.Tenant).ToString(),
                Tenant = exportDocumentArgs.Tenant,
                CreateDate = DateTime.Now,
                CreatedByUserId = exportDocumentArgs.LoggedContactId,
                RequestXML = LogitudeXmlSerializer.SerializeObjectToXmlString(exportDocumentArgs),
                StatusCode = "W",
                DocumentTypeId = exportDocumentArgs.DocumentTypeId,
                DocumentTypeTemplateId = exportDocumentArgs.DocumentTypeTemplateId,
                Subject = exportDocumentArgs.DocumentTypeName,
            };
            documentsExecutionLogRepository.Add(documentsExecutionLog);
            documentsExecutionLogRepository.SubmitChanges();

            return documentsExecutionLog;
        }




        public bool IsRunStimulDocumentViaWorkerRole()
        {
            bool result = false;
            string currentIP = AuthenticationUtil.GetIP4Address();
            if (!string.IsNullOrEmpty(currentIP))
            {
                int LastIpPart = 0;
                var IpParts = currentIP.Split('.');
                if (IpParts.Length == 4)
                {
                    int.TryParse(IpParts[3], out LastIpPart);
                    IGlobalContext objectContext = GlobalContext.GetContext();
                    var settingRepository = new SettingRepository(objectContext);
                    var settingQuery = new SettingQuery(settingRepository);
                    var settings = settingQuery.GetSinglePM();
                    if (settings != null && settings.System2RedirectFraction > 0 && LastIpPart != 0 && (LastIpPart % settings.System2RedirectFraction) == 0) result = true;
                }
            }
            return result;
        }






    }


    public class BuildDocumentParameter
    {
        public string DocumentTypeCode { get; set; }
        public string DocumentTypeId { get; set; }
        public string EntityId { get; set; }
        public string EntityObjectTableId { get; set; }
        public string ChildEntityId { get; set; }
        public string ChildObjectTableId { get; set; }
        public string DefaulttemplateId { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public long TheT1 { get; set; }
        public long TheT2 { get; set; }
        public string DocumentOutId { get; set; }
    }

    public class DocumentFileNameParameter
    {
        public string EntityId { get; set; }
        public string EntityObjectTableId { get; set; }
        public string ChildEntityId { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public DocumentTypeCopy DocumentTypeCopy { get; set; }
        public DocumentType DocumentType { get; set; }
    }

    public class DocumentFileNameFromObjectTableParameter
    {
        public string EntityId { get; set; }
        public DocumentFileNameParameter DocumentFileNameParameter { get; set; }
        public string DocumentFileName { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableName { get; set; }
    }
}
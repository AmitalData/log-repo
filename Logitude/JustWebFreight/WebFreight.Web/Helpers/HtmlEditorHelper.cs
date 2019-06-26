using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Services;
using System.Xml;
using Microsoft.WindowsAzure.Storage;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers; 
using WebFreight.Web.Azure;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.ShipmentsModel.DomainServices;
using WebFreight.Web.Testing;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using System.Web;
using Logitude.Server.Tools.Counters;
using Logitude.CRM.BL.EntityPMs;
using System.Transactions;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using WebFreight.Web.Security;
using WebFreight.Web.CRMModel.DomainServices;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.WebServices;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.FormatProviders.Xaml;
using System.Text;
using HtmlAgilityPack;
using EvoPdf;
using System.Drawing;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.Interfaces;

namespace WebFreight.Web.Helpers
{
    public class LogoClass
    {
        public LogoClass(XmlNode logoNode, string logoString)
        {
            this.LogoNode = logoNode;
            this.LogoString = logoString;
        }

        XmlNode logoNode;
        public XmlNode LogoNode
        {
            get { return logoNode; }
            set { logoNode = value; }
        }

        string logoString;

        public string LogoString
        {
            get { return logoString; }
            set { logoString = value; }
        }


    }

    public class LogoClassHtml
    {

        HtmlNode logoNodeHtml;
        public HtmlNode LogoNodeHtml
        {
            get { return logoNodeHtml; }
            set { logoNodeHtml = value; }
        }

        string logoString;

        public string LogoString
        {
            get { return logoString; }
            set { logoString = value; }
        }

        public LogoClassHtml(HtmlNode logoNode, string logoString)
        {
            this.LogoNodeHtml = logoNode;
            this.LogoString = logoString;
        }
    }

    public class HtmlEditorHelper : IHtmlEditorHelper
    {
        string DocumentTypeTemplateId = "";
        object entity = null;
        object childEntity = null;
        GeneralDomainService generalService = new GeneralDomainService();
        List<ObjectField> entityObjectFields = new List<ObjectField>();
        List<ObjectField> childEntityObjectFields = new List<ObjectField>();
        string signature = " ";
        bool isSendMail = false;
        string securityKey = "";
        List<LogoClass> logosList = new List<LogoClass>();
        List<LogoClassHtml> logosListHtml = new List<LogoClassHtml>();
        //XmlNode securityKeyNode = null;
        List<XmlNode> shipmentNumbersNodes = new List<XmlNode>();

        List<HtmlNode> shipmentNumbersNodesHtml = new List<HtmlNode>();

        ObjectTableRepository tablesRepository;
        Tenant CurrentTenant = null;

        // Ticket variables 
        string ticketHeader = " ", ticketFooter = " ";
        string ticketHeaderHtml = " ", ticketFooterHtml = " ";
        string ObjectTableName = "";
        public byte[] GetEditorXamlData(string docOutId, string entityId, string objectTableId, string childEntityId, string childEntityObjectTableId, int tenant, string userId, bool theIsSendMail, string documentTemplateId, ref string subject, ref string from, ref string replyTo)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            CommonDataDomainService commonService = new CommonDataDomainService();
            CurrentTenant = context.Tenants.Where(t => t.Id == tenant).FirstOrDefault();


            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string datetimeformat = @"dd\/MM\/yyyy";
            if (!string.IsNullOrEmpty(CurrentTenant.DateTimeFormat))
            {
                datetimeformat = CurrentTenant.DateTimeFormat;
            }

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";



            this.isSendMail = theIsSendMail;
            logosList = new List<LogoClass>();
            tablesRepository = new ObjectTableRepository(tenant);

            ObjectTable entityTable = tablesRepository.GetObjectTableById(objectTableId, tenant);
            if (entityTable != null) ObjectTableName = entityTable.Name;

            string entityName = "";
            if (entityTable != null)
            {
                entityName = entityTable.Name;
            }
            securityKey = "";
            //securityKeyNode = null;
            shipmentNumbersNodes = new List<XmlNode>();
            string childEntityName = "";
            if (childEntityId != null && childEntityObjectTableId != null)
            {
                ObjectTable childEntityTable = tablesRepository.GetObjectTableById(childEntityObjectTableId, tenant);

                if (childEntityTable != null)
                {
                    childEntityName = childEntityTable.Name;
                }
            }


            byte[] documentTemplateData = null;
            DocumentTypeTemplate documentTemplate = null;
            DocumentTypeTemplate documentTypeTemplate = null;
            DocumentType docType = null;
            if (string.IsNullOrEmpty(docOutId))
            {

                documentTemplate = (from a in context.DocumentTypeTemplates
                                    where a.Id == documentTemplateId

                                    select a).FirstOrDefault();


                docType = (from d in context.DocumentTypes
                           where d.Id == documentTemplate.DocumentTypeId
                           select d).FirstOrDefault();


                if (documentTemplate != null)
                {
                    documentTemplateData = documentTemplate.TemplateBody;
                }
                if (documentTemplateData != null)
                {

                    documentTypeTemplate = new DocumentTypeTemplate() { TemplateBody = documentTemplateData };
                }
            }
            else
            {


                DocumentOut documentOut = (from d in context.DocumentOuts.Include("DocumentsFiling")
                                           where d.Id == docOutId && d.Tenant == tenant
                                           select d).FirstOrDefault();

                docType = (from d in context.DocumentTypes
                           where d.Id == documentOut.DocumentsFiling.DocumentTypeId && d.Tenant == tenant
                           select d).FirstOrDefault();


                documentTemplate = (from a in context.DocumentTypeTemplates
                                    where a.Id == documentTemplateId

                                    select a).FirstOrDefault();

                if (documentTemplate != null)
                {
                    documentTemplateData = documentTemplate.TemplateBody;
                }



                if (documentTemplateData == null)
                {
                    if (docType.TemplateFormatCode == "M")
                    {
                        documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                where a.Id == documentOut.EmailTemplateId

                                                select a).FirstOrDefault();

                        if (documentTypeTemplate == null)
                        {
                            documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                    where a.Id == docType.DocumentTypeDefaultHTMLTemplateId

                                                    select a).FirstOrDefault();
                        }
                    }

                    else
                    {
                        if (theIsSendMail)
                        {

                            documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                    where a.Id == documentOut.EmailTemplateId

                                                    select a).FirstOrDefault();


                            if (documentTypeTemplate == null)
                            {
                                documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                        where a.Id == docType.DocumentTypeDefaultHTMLTemplateId

                                                        select a).FirstOrDefault();
                            }


                        }
                        else
                        {
                            documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                    where a.Id == documentOut.DocumentTemplateId

                                                    select a).FirstOrDefault();

                            if (documentTypeTemplate == null)
                            {
                                documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                        where a.Id == docType.DocumentTypeDefaultReportTemplateId

                                                        select a).FirstOrDefault();
                            }
                        }
                    }

                }
                else
                {
                    documentTypeTemplate = new DocumentTypeTemplate() { TemplateBody = documentTemplateData };

                }
            }

            generalService = new GeneralDomainService();



            SystemDataQuery systemDataQuery = new SystemDataQuery();
            SystemDataPM systemEntity = systemDataQuery.GetSinglePM(userId, tenant);

            List<ObjectField> systemEntityObjectFields = GetEntityObjectFields("SystemData", tenant);// generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName("SystemData", tenant).ToList();


            entity = GetEntity(entityName, entityId, tenant);
            entityObjectFields = GetEntityObjectFields(entityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(entityName, tenant).ToList();

            childEntityObjectFields = new List<ObjectField>();
            if (childEntityId != null && childEntityObjectTableId != null)
            {
                childEntity = GetEntity(childEntityName, childEntityId, tenant);
                childEntityObjectFields = GetEntityObjectFields(childEntityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(childEntityName, tenant).ToList();
            }



            if (documentTypeTemplate != null)
            {

                if (documentTemplate != null)
                {
                    if (!string.IsNullOrEmpty(documentTemplate.Subject))
                    {
                        subject = documentTemplate.Subject;


                    }
                    if (!string.IsNullOrEmpty(documentTemplate.From))
                    {
                        from = documentTemplate.From;
                    }

                    if (!string.IsNullOrEmpty(documentTemplate.ReplyTo))
                    {
                        replyTo = documentTemplate.ReplyTo;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(documentTypeTemplate.Subject))
                    {
                        subject = documentTypeTemplate.Subject;
                    }

                    if (!string.IsNullOrEmpty(documentTypeTemplate.From))
                    {
                        from = documentTypeTemplate.From;
                    }

                    if (!string.IsNullOrEmpty(documentTypeTemplate.ReplyTo))
                    {
                        replyTo = documentTypeTemplate.ReplyTo;
                    }


                }
                //}

                if (documentTypeTemplate.TemplateBody != null && documentTypeTemplate.TemplateBody.Count() > 0)
                {

                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    string htmlString = enc.GetString(documentTypeTemplate.TemplateBody);
                    string resultString = htmlString;
                    XmlReader reader = XmlReader.Create(new StringReader(htmlString));
                    XmlDataDocument messageDoc = new XmlDataDocument();
                    messageDoc.Load(reader);

                    XmlDocument doc2 = (XmlDocument)messageDoc.Clone();


                    List<XmlNode> signatureNodeList = new List<XmlNode>();

                    //Ticket Header and Footer
                    List<XmlNode> ticketHeaderNode = new List<XmlNode>();
                    List<XmlNode> ticketFooterNode = new List<XmlNode>();

                    XmlNodeList spansList = messageDoc.GetElementsByTagName("t:Span");

                    Dictionary<XmlNode, XmlNode> tablesDic = new Dictionary<XmlNode, XmlNode>();
                    signature = " ";
                    ticketFooter = " ";
                    ticketHeader = " ";
                    foreach (XmlNode node in spansList)
                    {
                        if (!String.IsNullOrEmpty(node.Attributes["Text"].Value))
                        {
                            string textValue = node.Attributes["Text"].Value;
                            if (textValue.Contains("[") && textValue.Contains("]"))
                            {
                                GetNodeValue(node, textValue, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                            }
                            else if (textValue.Contains("["))
                            {
                                node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[", "");
                            }
                            else if (textValue.Contains("]"))
                            {
                                node.Attributes["Text"].Value = textValue = "[" + node.Attributes["Text"].Value;
                                GetNodeValue(node, textValue, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                            }

                        }
                    }

                    if (!string.IsNullOrEmpty(subject))
                    {
                        if (subject.Contains("[") && subject.Contains("]"))
                        {
                            XmlDocument doc = new XmlDocument();
                            XmlNode subjectNode = doc.CreateNode(XmlNodeType.Element, "subject", "");

                            subjectNode.Attributes.Append(doc.CreateAttribute("Text"));
                            subjectNode.Attributes["Text"].Value = subject;
                            subjectNode.InnerText = subject;
                            GetNodeValue(subjectNode, subject, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);

                            subject = subjectNode.Attributes["Text"].Value;
                        }
                    }



                    if (!string.IsNullOrEmpty(from))
                    {
                        if (from.Contains("[") && from.Contains("]"))
                        {
                            XmlDocument doc = new XmlDocument();
                            XmlNode fromNode = doc.CreateNode(XmlNodeType.Element, "from", "");

                            fromNode.Attributes.Append(doc.CreateAttribute("Text"));
                            fromNode.Attributes["Text"].Value = from;
                            fromNode.InnerText = from;
                            GetNodeValue(fromNode, from, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);

                            from = fromNode.Attributes["Text"].Value;
                        }
                    }

                    if (!string.IsNullOrEmpty(replyTo))
                    {
                        if (replyTo.Contains("[") && replyTo.Contains("]"))
                        {
                            XmlDocument doc = new XmlDocument();
                            XmlNode replyToNode = doc.CreateNode(XmlNodeType.Element, "replyTo", "");

                            replyToNode.Attributes.Append(doc.CreateAttribute("Text"));
                            replyToNode.Attributes["Text"].Value = replyTo;
                            replyToNode.InnerText = replyTo;
                            GetNodeValue(replyToNode, replyTo, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);

                            replyTo = replyToNode.Attributes["Text"].Value;
                        }
                    }

                    if (signatureNodeList.Count() != 0)
                    {
                        if (!String.IsNullOrEmpty(signature.Trim()))
                        {
                            XmlNode newSignatureSection = GetNewSignatureNode(signature, systemEntity, systemEntityObjectFields, tenant, generalService);



                            foreach (XmlNode signatureNode in signatureNodeList)
                            {
                                foreach (XmlNode childNode in newSignatureSection.ChildNodes)
                                {
                                    XmlNode t2Node = messageDoc.ImportNode(childNode, true);

                                    signatureNode.ParentNode.ParentNode.InsertBefore(t2Node, signatureNode.ParentNode);
                                }
                            }
                        }

                        foreach (XmlNode signatureNode in signatureNodeList)
                        {
                            if (signatureNode.ParentNode.ParentNode != null)
                            {
                                signatureNode.ParentNode.ParentNode.RemoveChild(signatureNode.ParentNode);

                            }
                        }
                    }

                    //Ticket Header Work 
                    if (ticketHeaderNode.Count() != 0)
                    {
                        if (!String.IsNullOrEmpty(ticketHeader.Trim()))
                        {
                            XmlNode newTicketHeaderSection = GetNewTicketHeaderFooterNode(ticketHeader, entity);

                            foreach (XmlNode node in ticketHeaderNode)
                            {
                                foreach (XmlNode childNode in newTicketHeaderSection.ChildNodes)
                                {
                                    XmlNode t2Node = messageDoc.ImportNode(childNode, true);
                                    node.ParentNode.ParentNode.InsertBefore(t2Node, node.ParentNode);
                                }
                            }
                        }

                        foreach (XmlNode node in ticketHeaderNode)
                        {
                            if (node.ParentNode.ParentNode != null)
                            {
                                node.ParentNode.ParentNode.RemoveChild(node.ParentNode);
                            }
                        }
                    }

                    //Ticket Footer Work 
                    if (ticketFooterNode.Count() != 0)
                    {
                        if (!String.IsNullOrEmpty(ticketFooter.Trim()))
                        {
                            XmlNode newTicketFooterSection = GetNewTicketHeaderFooterNode(ticketFooter, entity);

                            foreach (XmlNode node in ticketFooterNode)
                            {
                                foreach (XmlNode childNode in newTicketFooterSection.ChildNodes)
                                {
                                    XmlNode t2Node = messageDoc.ImportNode(childNode, true);
                                    node.ParentNode.ParentNode.InsertBefore(t2Node, node.ParentNode);
                                }
                            }

                        }

                        foreach (XmlNode node in ticketFooterNode)
                        {
                            if (node.ParentNode.ParentNode != null)
                            {
                                node.ParentNode.ParentNode.RemoveChild(node.ParentNode);
                            }
                        }
                    }

                    //========================================================== logo

                    if (logosList.Count() != 0)
                    {

                        foreach (LogoClass logo in logosList)
                        {
                            if (!String.IsNullOrEmpty(logo.LogoString.Trim()))
                            {
                                XmlNode newLogoSection = GetNewNode(logo.LogoString);

                                foreach (XmlNode childNode in newLogoSection.ChildNodes)
                                {
                                    XmlNode t2Node = messageDoc.ImportNode(childNode, true);


                                    logo.LogoNode.ParentNode.ParentNode.InsertBefore(t2Node, logo.LogoNode.ParentNode);

                                }
                            }
                        }

                        foreach (LogoClass logo in logosList)
                        {
                            if (logo.LogoNode.ParentNode.ParentNode != null)
                            {
                                logo.LogoNode.ParentNode.ParentNode.RemoveChild(logo.LogoNode.ParentNode);
                            }

                        }


                    }

                    //================================================================

                    if (tablesDic.Count() != 0)
                    {
                        foreach (XmlNode node in tablesDic.Keys)
                        {
                            XmlNode t2Node = messageDoc.ImportNode(tablesDic[node], true);
                            if (node.ParentNode != null)
                            {
                                node.ParentNode.ReplaceChild(t2Node, node);
                            }
                        }
                    }


                    foreach (XmlNode securityKeyNode in shipmentNumbersNodes)
                    {

                        if (securityKeyNode != null && !string.IsNullOrEmpty(securityKey))
                        {

                            if (securityKeyNode.ParentNode != null)
                            {
                                XmlNode newLogoSection = GetNewNode(GetSecurityKeyLink(securityKey, entityId, securityKeyNode.OuterXml, tenant));


                                foreach (XmlNode childNode in newLogoSection.ChildNodes)
                                {
                                    XmlNode t2Node = messageDoc.ImportNode(childNode, true);


                                    securityKeyNode.ParentNode.ParentNode.InsertBefore(t2Node, securityKeyNode.ParentNode);

                                }

                                if (securityKeyNode.ParentNode.ParentNode != null)
                                {
                                    securityKeyNode.ParentNode.ParentNode.RemoveChild(securityKeyNode.ParentNode);
                                }


                            }
                        }
                    }

                    //bool hasFooter = messageDoc.GetElementsByTagName("t:Footers").Count > 0 ? true : false;

                    //XmlNode powerdByNode = GetPoweredByLink(hasFooter);

                    //foreach (XmlNode nn in powerdByNode.ChildNodes)
                    //{
                    //    XmlNode ttNode = messageDoc.ImportNode(nn, true);


                    //    if (hasFooter)
                    //    {
                    //        XmlNodeList sectionsList = messageDoc.GetElementsByTagName("t:Section");//("t:Paragraph");//
                    //        XmlNode sectionNode = sectionsList.Item(sectionsList.Count - 1);
                    //        sectionNode.AppendChild(ttNode);
                    //    }
                    //    else
                    //    {
                    //        XmlNodeList sectionsList = messageDoc.GetElementsByTagName("t:Paragraph");//("t:Paragraph");//
                    //        XmlNode sectionNode = sectionsList.Item(sectionsList.Count - 1);
                    //        sectionNode.ParentNode.InsertAfter(ttNode,sectionNode);
                    //    }
                    //}




                    return enc.GetBytes(messageDoc.InnerXml);

                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }


        public string GetEditorHtmlData(string docOutId, string entityId, string objectTableId, string childEntityId, string childEntityObjectTableId, int tenant, string userId, bool theIsSendMail, string documentTemplateId, ref string subject, ref string from, ref string replyTo, ref string cc, string mode = null, DocumentTypeTemplate template = null)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            CurrentTenant = context.Tenants.Where(t => t.Id == tenant).FirstOrDefault();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string datetimeformat = @"dd\/MM\/yyyy";
            if (!string.IsNullOrEmpty(CurrentTenant.DateTimeFormat))
            {
                datetimeformat = CurrentTenant.DateTimeFormat;
            }

            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";



            this.isSendMail = theIsSendMail;

            int headerHeight = 0;
            int footerHeight = 0;
            byte[] headerTemplateData = null;
            byte[] footerTemplateData = null;
            byte[] bodyTemplateData = null;
            DocumentTypeTemplate documentTemplate = null;
            DocumentTypeTemplate documentTypeTemplate = null;
            DocumentType docType = null;




            if (string.IsNullOrEmpty(docOutId))
            {

                if (template == null)
                {
                    documentTemplate = (from a in context.DocumentTypeTemplates
                                        where a.Id == documentTemplateId

                                        select a).FirstOrDefault();


                    //docType = (from d in context.DocumentTypes
                    //           where d.Id == documentTemplate.DocumentTypeId
                    //           select d).FirstOrDefault();
                }
                else documentTemplate = template;

                if (documentTemplate != null)
                {
                    bodyTemplateData = documentTemplate.TemplateBodyHtml;
                    headerTemplateData = documentTemplate.TemplateHeaderHtml;
                    footerTemplateData = documentTemplate.TemplateFooterHtml;

                    headerHeight = documentTemplate.TemplateHeaderHeight;
                    footerHeight = documentTemplate.TemplateFooterHeight;

                    documentTypeTemplate = new DocumentTypeTemplate() { TemplateBodyHtml = bodyTemplateData, TemplateHeaderHtml = headerTemplateData, TemplateFooterHtml = footerTemplateData, TemplateHeaderHeight = headerHeight, TemplateFooterHeight = footerHeight };
                }

            }
            else
            {


                DocumentOut documentOut = (from d in context.DocumentOuts.Include("DocumentsFiling")
                                           where d.Id == docOutId && d.Tenant == tenant
                                           select d).FirstOrDefault();

                docType = (from d in context.DocumentTypes
                           where d.Id == documentOut.DocumentsFiling.DocumentTypeId && d.Tenant == tenant
                           select d).FirstOrDefault();


                documentTemplate = (from a in context.DocumentTypeTemplates
                                    where a.Id == documentTemplateId

                                    select a).FirstOrDefault();

                if (documentTemplate != null)
                {
                    bodyTemplateData = documentTemplate.TemplateBodyHtml;
                    headerTemplateData = documentTemplate.TemplateHeaderHtml;
                    footerTemplateData = documentTemplate.TemplateFooterHtml;

                    headerHeight = documentTemplate.TemplateHeaderHeight;
                    footerHeight = documentTemplate.TemplateFooterHeight;
                }



                if (bodyTemplateData == null)
                {
                    if (docType.TemplateFormatCode == "M")
                    {
                        documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                where a.Id == documentOut.EmailTemplateId

                                                select a).FirstOrDefault();

                        if (documentTypeTemplate == null)
                        {
                            documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                    where a.Id == docType.DocumentTypeDefaultHTMLTemplateId

                                                    select a).FirstOrDefault();
                        }
                    }

                    else
                    {
                        if (theIsSendMail)
                        {

                            documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                    where a.Id == documentOut.EmailTemplateId

                                                    select a).FirstOrDefault();


                            if (documentTypeTemplate == null)
                            {
                                documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                        where a.Id == docType.DocumentTypeDefaultHTMLTemplateId

                                                        select a).FirstOrDefault();
                            }


                        }
                        else
                        {
                            documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                    where a.Id == documentOut.DocumentTemplateId

                                                    select a).FirstOrDefault();

                            if (documentTypeTemplate == null)
                            {
                                documentTypeTemplate = (from a in context.DocumentTypeTemplates
                                                        where a.Id == docType.DocumentTypeDefaultReportTemplateId

                                                        select a).FirstOrDefault();
                            }
                        }
                    }

                }
                else
                {
                    documentTypeTemplate = new DocumentTypeTemplate() { TemplateBodyHtml = bodyTemplateData, TemplateHeaderHtml = headerTemplateData, TemplateFooterHtml = footerTemplateData, TemplateHeaderHeight = headerHeight, TemplateFooterHeight = footerHeight };

                }
            }



            if (documentTypeTemplate != null)
            {

                if (documentTemplate != null)
                {
                    if (!string.IsNullOrEmpty(documentTemplate.Subject))
                    {
                        subject = documentTemplate.Subject;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(documentTypeTemplate.Subject))
                    {
                        subject = documentTypeTemplate.Subject;
                    }
                }


                string header = documentTypeTemplate.TemplateHeaderHtml != null ? enc.GetString(documentTypeTemplate.TemplateHeaderHtml) : "";
                string footer = documentTypeTemplate.TemplateFooterHtml != null ? enc.GetString(documentTypeTemplate.TemplateFooterHtml) : "";
                string htmlheaderString = "";
                string htmlBodyString = "";
                string htmlfooterString = "";
                if (mode == "Edit")
                {
                    htmlheaderString = "<header><height>" + "<div style='display:none'>" + documentTypeTemplate.TemplateHeaderHeight + "</div></height>" + header + "</header>";
                    htmlBodyString = documentTypeTemplate.TemplateBodyHtml != null ? "<div style ='width:100%'>" + enc.GetString(documentTypeTemplate.TemplateBodyHtml) + "</div>" : "";
                    htmlfooterString = "<footer><height>" + "<div style='display:none'>" + documentTypeTemplate.TemplateFooterHeight + "</div></height><div style ='bottom:0;'>" + footer + "</div></footer>";

                }
                else
                {
                    htmlheaderString = header;
                    htmlBodyString = documentTypeTemplate.TemplateBodyHtml != null ? enc.GetString(documentTypeTemplate.TemplateBodyHtml) : "";
                    htmlfooterString = footer;
                }

                var htmlString = htmlheaderString + htmlBodyString + htmlfooterString;

                var result = htmlString;
                bool isHaveDataVariable = CheckIfTemplateHaveDataVariable(subject, from, replyTo, cc, htmlString);
                if (isHaveDataVariable)
                {
                    result = ResolveHtmlData(entityId, objectTableId, userId, tenant, htmlString, ref subject, ref from, ref replyTo, ref cc, null, childEntityId, childEntityObjectTableId);
                }

                return result;




            }
            else return null;

        }
        private bool CheckIfTemplateHaveDataVariable(string subject, string from, string replyTo, string cc, string htmlString)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(subject))
            {
                if (subject.Contains("[") && subject.Contains("]")) { return true; }
            }
            if (!string.IsNullOrEmpty(from))
            {
                if (from.Contains("[") && from.Contains("]")) { return true; }
            }
            if (!string.IsNullOrEmpty(replyTo))
            {
                if (replyTo.Contains("[") && replyTo.Contains("]")) { return true; }
            }

            if (!string.IsNullOrEmpty(cc))
            {
                if (cc.Contains("[") && cc.Contains("]")) { return true; }
            }

            if (!string.IsNullOrEmpty(htmlString))
            {
                if (htmlString.Contains("[") && htmlString.Contains("]")) { return true; }
            }

            return result;

        }
        public string ResolveHtmlData(string entityId, string objectTableId, string userId, int tenant, string htmlString, ref string subject, ref string from, ref string replyTo, ref string cc, object customEntity, string childEntityId = null, string childEntityObjectTableId = null)
        {

            if (CurrentTenant == null)
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);

                CurrentTenant = context.Tenants.Where(t => t.Id == tenant).FirstOrDefault();


                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string datetimeformat = @"dd\/MM\/yyyy";
                if (!string.IsNullOrEmpty(CurrentTenant.DateTimeFormat))
                {
                    datetimeformat = CurrentTenant.DateTimeFormat;
                }

                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";

            }


            logosListHtml = new List<LogoClassHtml>();
            tablesRepository = new ObjectTableRepository(tenant);

            ObjectTable entityTable = tablesRepository.GetObjectTableById(objectTableId, tenant);

            if (entityTable != null) ObjectTableName = entityTable.Name;
            string entityName = "";
            if (entityTable != null)
            {
                entityName = entityTable.Name;
            }
            securityKey = "";
            //securityKeyNode = null;
            shipmentNumbersNodesHtml = new List<HtmlNode>();
            string childEntityName = "";
            if (childEntityId != null && childEntityObjectTableId != null)
            {
                ObjectTable childEntityTable = tablesRepository.GetObjectTableById(childEntityObjectTableId, tenant);

                if (childEntityTable != null)
                {
                    childEntityName = childEntityTable.Name;
                }
            }

            generalService = new GeneralDomainService();

            #region BrandingEnabled
            bool IsBrandingEnabled = false;
            bool HideSharedlogistics = false;


            string SystemUrl = "";

            #endregion

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(tenant);
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetTenantManagementPM(tenant);
                if (tenantManagementPM != null)
                {
                    IsBrandingEnabled = tenantManagementPM.EnableBranding;
                    SystemUrl = tenantManagementPM.CustomerURL;

                    if (tenantManagementPM.EnableBranding)
                    {
                        HideSharedlogistics = tenantManagementPM.HideSharedlogistics;
                    }
                }
                scope.Complete();
            }





            if (string.IsNullOrEmpty(SystemUrl))
            {
                SystemUrl = LogitudeSettings.LogitudeURL;
                if (IsBrandingEnabled)
                {

                    SystemUrl = LogitudeSettings.LogitudeURL + "/login.aspx?tenant=" + tenant;
                }
            }



            if (entityTable != null && entityTable.Name == "SharedLogistics")
            {
                SharedLogisticsQuery sharedLogisticsQuery = new SharedLogisticsQuery();
                entity = sharedLogisticsQuery.GetSinglePM(entityTable.Tenant, SystemUrl);
            }

            else
            {
                if (!string.IsNullOrEmpty(entityId))
                {
                    entity = GetEntity(entityName, entityId, tenant);
                }
                else entity = customEntity;
            }

            entityObjectFields = GetEntityObjectFields(entityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(entityName, tenant).ToList();

            childEntityObjectFields = new List<ObjectField>();
            if (childEntityId != null && childEntityObjectTableId != null)
            {
                childEntity = GetEntity(childEntityName, childEntityId, tenant);
                childEntityObjectFields = GetEntityObjectFields(childEntityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(childEntityName, tenant).ToList();
            }

            SystemDataQuery systemDataQuery = new SystemDataQuery();
            SystemDataPM systemEntity = systemDataQuery.GetSinglePM(userId, tenant);


            List<ObjectField> systemEntityObjectFields = GetEntityObjectFields("SystemData", tenant);

            List<HtmlNode> signatureNodeList = new List<HtmlNode>();
            List<HtmlNode> ticketHeaderNode = new List<HtmlNode>();
            List<HtmlNode> ticketFooterNode = new List<HtmlNode>();

             

            Dictionary<HtmlNode, HtmlNode> tablesDic = new Dictionary<HtmlNode, HtmlNode>();
            HtmlDocument document = null;
            signature = " ";

            if (!string.IsNullOrEmpty(htmlString))
            {
                htmlString = htmlString.Replace("<tbody>", "");
                htmlString = htmlString.Replace("</tbody>", "");
                htmlString = htmlString.Replace("]</P>", "]</span></P>");
                htmlString = htmlString.Replace("<P>[", "<P><span>[");
                htmlString = htmlString.Replace("[PageBreak]", "<p style='page-break-after:always;'> <span style=visibility:collapse>Page Break</span></p>");



                if (htmlString.Contains("[") && htmlString.Contains("]"))
                {
                    ReplaceHtmlStringWithTageHtml = true;
                    document = new HtmlDocument();
                    document.LoadHtml(htmlString);
                    CorrectingBuildingHtml(document , htmlString);
                    HtmlNodeCollection spansList = document.DocumentNode.SelectNodes("//span");
                    if (spansList != null)
                    {
                        foreach (HtmlNode node in spansList)
                        {

                            string textValue = node.InnerHtml;
                            if (!String.IsNullOrEmpty(textValue))
                            {
                                if (textValue.Contains("[") && textValue.Contains("]"))
                                {
                                    string[] properties = textValue.Split('[');
                                    foreach (string property in properties)
                                    {
                                        if (!string.IsNullOrEmpty(property))
                                        {
                                            string[] array = property.Split(']');
                                            string propertyName = array[0];
                                            string propPath = "[" + propertyName + "]";
                                            GetHtmlNodeValue(node, propPath, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                                        }


                                    }

                                }
                                else if (textValue.Contains("["))
                                {
                                    node.InnerHtml.Replace("[", "");
                                }
                                else if (textValue.Contains("]"))
                                {
                                    node.InnerHtml = textValue = "[" + node.InnerHtml;
                                    string propPath = "[" + getBetween(textValue, "[", "]") + "]";
                                    GetHtmlNodeValue(node, propPath, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                                }

                            }

                        }
                    }

                }
            }


            ReplaceHtmlStringWithTageHtml = false;

            if (!string.IsNullOrEmpty(subject))
            {
                if (subject.Contains("[") && subject.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode subjectNode = doc.CreateElement(subject);
                    subjectNode.InnerHtml = subject;
                    GetHtmlNodeValue(subjectNode, subject, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                    subject = subjectNode.InnerHtml;
                }
            }



            if (!string.IsNullOrEmpty(from))
            {
                if (from.Contains("[") && from.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode fromNode = doc.CreateElement(from);
                    fromNode.InnerHtml = from;
                    GetHtmlNodeValue(fromNode, from, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                    from = fromNode.InnerHtml;
                }
            }


            if (!string.IsNullOrEmpty(replyTo))
            {
                if (replyTo.Contains("[") && replyTo.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode replyToNode = doc.CreateElement(replyTo);
                    replyToNode.InnerHtml = replyTo;
                    GetHtmlNodeValue(replyToNode, replyTo, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                    replyTo = replyToNode.InnerHtml;
                }
            }



            if (!string.IsNullOrEmpty(cc))
            {
                if (cc.Contains("[") && cc.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode ccNode = doc.CreateElement(cc);
                    ccNode.InnerHtml = cc;
                    GetHtmlNodeValue(ccNode, cc, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                    cc = ccNode.InnerHtml;
                }
            }



            if (signatureNodeList.Count() != 0)
            {
                if (!String.IsNullOrEmpty(signature.Trim()))
                {
                    HtmlNode newSignatureSection = GetNewSignatureNodeHtml(signature, systemEntity, systemEntityObjectFields, tenant, generalService);



                    foreach (HtmlNode signatureNode in signatureNodeList)
                    {
                        foreach (HtmlNode childNode in newSignatureSection.ChildNodes)
                        {

                            if (signatureNode.ParentNode != null && signatureNode.ParentNode.ParentNode != null)
                            {
                                signatureNode.ParentNode.ParentNode.InsertBefore(childNode, signatureNode.ParentNode);
                            }


                        }
                    }


                }


                foreach (HtmlNode signatureNode in signatureNodeList)
                {
                    if (signatureNode.ParentNode != null && signatureNode.ParentNode.ParentNode != null)
                    {
                        signatureNode.ParentNode.ParentNode.RemoveChild(signatureNode.ParentNode);
                    }

                }

            }

            #region TicketHeaderAndFooter

            #region Header
            if (ticketHeaderNode.Count() != 0)
            {
                if (!String.IsNullOrEmpty(ticketHeaderHtml) && !String.IsNullOrEmpty(ticketHeaderHtml.Trim()))
                {
                    HtmlNode newticketHeaderSection = GetNewTicketHeaderFooterNodeHtml(ticketHeaderHtml, entity);


                    foreach (HtmlNode node in ticketHeaderNode)
                    {
                        foreach (HtmlNode childNode in newticketHeaderSection.ChildNodes)
                        {

                            if (node.ParentNode != null && node.ParentNode.ParentNode != null)
                            {
                                node.ParentNode.ParentNode.InsertBefore(childNode, node.ParentNode);
                            }


                        }
                    }


                }

                foreach (HtmlNode node in ticketHeaderNode)
                {
                    if (node.ParentNode != null && node.ParentNode.ParentNode != null)
                    {
                        node.ParentNode.ParentNode.RemoveChild(node.ParentNode);
                    }

                }

            }

            #endregion

            #region Footer
            if (ticketFooterNode.Count() != 0)
            {
                if (!String.IsNullOrEmpty(ticketFooterHtml) && !String.IsNullOrEmpty(ticketFooterHtml.Trim()))
                {
                    HtmlNode newticketFooterSection = GetNewTicketHeaderFooterNodeHtml(ticketFooterHtml, entity);

                    foreach (HtmlNode node in ticketFooterNode)
                    {
                        foreach (HtmlNode childNode in newticketFooterSection.ChildNodes)
                        {

                            if (node.ParentNode != null && node.ParentNode.ParentNode != null)
                            {
                                node.ParentNode.ParentNode.InsertBefore(childNode, node.ParentNode);
                            }


                        }
                    }


                }

                foreach (HtmlNode node in ticketFooterNode)
                {
                    if (node.ParentNode != null && node.ParentNode.ParentNode != null)
                    {
                        node.ParentNode.ParentNode.RemoveChild(node.ParentNode);
                    }

                }

            }

            #endregion


            #endregion

            //========================================================== logo

            if (logosListHtml.Count() != 0)
            {

                foreach (LogoClassHtml logo in logosListHtml)
                {
                    if (!String.IsNullOrEmpty(logo.LogoString.Trim()))
                    {
                        HtmlNode newLogoSection = GetNewNodeHtml(logo.LogoString);

                        foreach (HtmlNode childNode in newLogoSection.ChildNodes)
                        {
                            if (logo.LogoNodeHtml != null && logo.LogoNodeHtml.ParentNode != null)
                            {
                                logo.LogoNodeHtml.ParentNode.InsertBefore(childNode, logo.LogoNodeHtml);
                            }

                        }



                    }


                }

                foreach (LogoClassHtml logo in logosListHtml)
                {

                    if (logo.LogoNodeHtml != null && logo.LogoNodeHtml.ParentNode != null)
                    {
                        logo.LogoNodeHtml.ParentNode.RemoveChild(logo.LogoNodeHtml);
                    }


                }


            }

            //================================================================

            if (tablesDic.Count() != 0)
            {
                foreach (HtmlNode node in tablesDic.Keys)
                {
                    HtmlNode t2Node = node.AppendChild(tablesDic[node]);

                    if (node.ParentNode != null)
                    {
                        node.ParentNode.ReplaceChild(t2Node, node);
                    }
                }
            }


            foreach (HtmlNode securityKeyNode in shipmentNumbersNodesHtml)
            {

                if (securityKeyNode != null && !string.IsNullOrEmpty(securityKey))
                {

                    if (securityKeyNode.ParentNode != null)
                    {

                        HtmlNode newLogoSection = GetNewNodeHtml(GetSecurityKeyLinkHtml(securityKey, entityId, securityKeyNode.OuterHtml, tenant, HideSharedlogistics, SystemUrl));
                        foreach (HtmlNode childNode in newLogoSection.ChildNodes)
                        {
                            if (securityKeyNode.ParentNode != null && securityKeyNode.ParentNode.ParentNode != null)
                            {
                                securityKeyNode.ParentNode.ParentNode.InsertBefore(childNode, securityKeyNode.ParentNode);
                            }

                        }
                        if (securityKeyNode.ParentNode != null)
                        {
                            if (securityKeyNode.ParentNode != null && securityKeyNode.ParentNode.ParentNode != null)
                            {
                                securityKeyNode.ParentNode.ParentNode.RemoveChild(securityKeyNode.ParentNode);
                            }

                        }

                    }
                }


            }


            var result = htmlString;
            if (document != null)
            {
                var sw = new StringWriter();
                document.Save(sw);
                result = sw.ToString();
            }

            return result;
        }

        private  void CorrectingBuildingHtml(HtmlDocument document, string htmlString)
        {
            if (!string.IsNullOrEmpty(htmlString) &&  htmlString.Contains("]</p>"))
            {
                List<HtmlNode> pTagList = document.DocumentNode.SelectNodes("//p").Where(d => !string.IsNullOrEmpty(d.InnerHtml) && d.InnerHtml.Contains("[") && d.InnerHtml.Contains("]") && !d.InnerHtml.Contains("</span>")).ToList();
                if (pTagList.Count>0)
                {
                    foreach (HtmlNode node in pTagList)
                    {
                        node.InnerHtml = node.InnerHtml.Replace("[", "<span>[").Replace("]", "]</span>");
                    }
                }
            }
        }

        bool ReplaceHtmlStringWithTageHtml = false;
        public string ResolveHtmlString(string entityId, string objectTableId, string htmlString, string userId, int tenant)
        {
            tablesRepository = new ObjectTableRepository(tenant);
            ObjectTable entityTable = tablesRepository.GetObjectTableById(objectTableId, tenant);

            if (entityTable != null) ObjectTableName = entityTable.Name;
            string entityName = "";
            if (entityTable != null)
            {
                entityName = entityTable.Name;
            }


            if (!string.IsNullOrEmpty(entityId))
            {
                entity = GetEntity(entityName, entityId, tenant);
            }

            generalService = new GeneralDomainService();
            entityObjectFields = GetEntityObjectFields(entityName, tenant);

            List<HtmlNode> signatureNodeList = new List<HtmlNode>();
            Dictionary<HtmlNode, HtmlNode> tablesDic = new Dictionary<HtmlNode, HtmlNode>();

            if (!string.IsNullOrEmpty(htmlString))
            {
                if (htmlString.Contains("[") && htmlString.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode subjectNode = doc.CreateElement(htmlString);
                    subjectNode.InnerHtml = htmlString;
                    GetHtmlNodeValue(subjectNode, htmlString, entity, entityObjectFields, tablesDic, null, null, signatureNodeList, tenant, generalService, null, null);
                    htmlString = subjectNode.InnerHtml;
                }
            }


            //HtmlNodeCollection spansList = document.DocumentNode.SelectNodes("//span");

            //Dictionary<HtmlNode, HtmlNode> tablesDic = new Dictionary<HtmlNode, HtmlNode>();

            //signature = " ";

            //if (spansList != null)
            //{
            //    foreach (HtmlNode node in spansList)
            //    {

            //        string textValue = node.InnerHtml;
            //        if (!String.IsNullOrEmpty(textValue))
            //        {
            //            if (textValue.Contains("[") && textValue.Contains("]"))
            //            {
            //                string[] properties = textValue.Split('[');
            //                foreach (string property in properties)
            //                {
            //                    string[] array = property.Split(']');
            //                    string propertyName = array[0];

            //                    string propPath = "[" + propertyName + "]";
            //                    GetHtmlNodeValue(node, propPath, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);

            //                }

            //            }
            //            else if (textValue.Contains("["))
            //            {
            //                node.InnerHtml.Replace("[", "");
            //            }
            //            else if (textValue.Contains("]"))
            //            {
            //                node.InnerHtml = textValue = "[" + node.InnerHtml;
            //                string propPath = "[" + getBetween(textValue, "[", "]") + "]";
            //                GetHtmlNodeValue(node, propPath, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);
            //            }

            //        }

            //    }
            //}



            return htmlString;
        }



        public string ResolveSystemDataHtml(string htmlString, string userId, ref string subject, ref string from, ref string replyTo, ref string cc, int tenant)
        {

            if (CurrentTenant == null)
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);

                CurrentTenant = context.Tenants.Where(t => t.Id == tenant).FirstOrDefault();


                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string datetimeformat = @"dd\/MM\/yyyy";
                if (!string.IsNullOrEmpty(CurrentTenant.DateTimeFormat))
                {
                    datetimeformat = CurrentTenant.DateTimeFormat;
                }

                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";

            }


            logosListHtml = new List<LogoClassHtml>();



            generalService = new GeneralDomainService();



            SystemDataQuery systemDataQuery = new SystemDataQuery();
            SystemDataPM systemEntity = systemDataQuery.GetSinglePM(userId, tenant);

            tablesRepository = new ObjectTableRepository(tenant);
            List<ObjectField> systemEntityObjectFields = GetEntityObjectFields("SystemData", tenant);

            List<HtmlNode> signatureNodeList = new List<HtmlNode>();
            List<HtmlNode> ticketHeaderNode = new List<HtmlNode>();
            List<HtmlNode> ticketFooterNode = new List<HtmlNode>();



            Dictionary<HtmlNode, HtmlNode> tablesDic = new Dictionary<HtmlNode, HtmlNode>();
            HtmlDocument document = null;
            signature = " ";

            if (!string.IsNullOrEmpty(htmlString))
            {
                htmlString = htmlString.Replace("<tbody>", "");
                htmlString = htmlString.Replace("</tbody>", "");


                if (htmlString.Contains("[") && htmlString.Contains("]"))
                {
                    ReplaceHtmlStringWithTageHtml = true;
                    document = new HtmlDocument();
                    document.LoadHtml(htmlString);
                    HtmlNodeCollection spansList = document.DocumentNode.SelectNodes("//span");
                    if (spansList != null)
                    {
                        foreach (HtmlNode node in spansList)
                        {

                            string textValue = node.InnerHtml;
                            if (!String.IsNullOrEmpty(textValue))
                            {
                                if (textValue.Contains("[") && textValue.Contains("]"))
                                {
                                    if (textValue.Contains("SystemData."))
                                    {
                                        string[] properties = textValue.Split('[');
                                        foreach (string property in properties)
                                        {
                                            if (!string.IsNullOrEmpty(property))
                                            {
                                                string[] array = property.Split(']');
                                                string propertyName = array[0];
                                                string propPath = "[" + propertyName + "]";
                                                GeSystemDataNodeValue(node, propPath, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);
                                            }


                                        }
                                    }
                                }
                                else if (textValue.Contains("["))
                                {
                                    node.InnerHtml.Replace("[", "");
                                }
                                else if (textValue.Contains("]"))
                                {
                                    node.InnerHtml = textValue = "[" + node.InnerHtml;
                                    string propPath = "[" + getBetween(textValue, "[", "]") + "]";

                                    if (textValue.Contains("SystemData."))
                                    {
                                        GeSystemDataNodeValue(node, propPath, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);
                                    }
                                }

                            }

                        }
                    }

                }
            }


            ReplaceHtmlStringWithTageHtml = false;

            if (!string.IsNullOrEmpty(subject))
            {
                if (subject.Contains("[") && subject.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode subjectNode = doc.CreateElement(subject);
                    subjectNode.InnerHtml = subject;
                    GeSystemDataNodeValue(subjectNode, subject, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);
                    subject = subjectNode.InnerHtml;
                }
            }

            if (!string.IsNullOrEmpty(from))
            {
                if (from.Contains("[") && from.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode fromNode = doc.CreateElement(from);
                    fromNode.InnerHtml = from;
                    GeSystemDataNodeValue(fromNode, from, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);

                    from = fromNode.InnerHtml;
                }
            }

            if (!string.IsNullOrEmpty(replyTo))
            {
                if (replyTo.Contains("[") && replyTo.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode replyToNode = doc.CreateElement(replyTo);
                    replyToNode.InnerHtml = replyTo;
                    GeSystemDataNodeValue(replyToNode, replyTo, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);

                    replyTo = replyToNode.InnerHtml;
                }
            }

            if (!string.IsNullOrEmpty(cc))
            {
                if (cc.Contains("[") && cc.Contains("]"))
                {
                    HtmlDocument doc = new HtmlDocument();
                    HtmlNode ccNode = doc.CreateElement(cc);
                    ccNode.InnerHtml = cc;
                    GeSystemDataNodeValue(ccNode, cc, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);

                    cc = ccNode.InnerHtml;
                }
            }

            if (signatureNodeList.Count() != 0)
            {
                if (!String.IsNullOrEmpty(signature.Trim()))
                {
                    HtmlNode newSignatureSection = GetNewSignatureNodeHtml(signature, systemEntity, systemEntityObjectFields, tenant, generalService);

                    foreach (HtmlNode signatureNode in signatureNodeList)
                    {
                        foreach (HtmlNode childNode in newSignatureSection.ChildNodes)
                        {

                            if (signatureNode.ParentNode != null && signatureNode.ParentNode.ParentNode != null)
                            {
                                signatureNode.ParentNode.ParentNode.InsertBefore(childNode, signatureNode.ParentNode);
                            }


                        }
                    }

                }


                foreach (HtmlNode signatureNode in signatureNodeList)
                {
                    if (signatureNode.ParentNode != null && signatureNode.ParentNode.ParentNode != null)
                    {
                        signatureNode.ParentNode.ParentNode.RemoveChild(signatureNode.ParentNode);
                    }

                }

            }

            if (logosListHtml.Count() != 0)
            {

                foreach (LogoClassHtml logo in logosListHtml)
                {
                    if (!String.IsNullOrEmpty(logo.LogoString.Trim()))
                    {
                        HtmlNode newLogoSection = GetNewNodeHtml(logo.LogoString);

                        foreach (HtmlNode childNode in newLogoSection.ChildNodes)
                        {
                            if (logo.LogoNodeHtml != null && logo.LogoNodeHtml.ParentNode != null)
                            {
                                logo.LogoNodeHtml.ParentNode.InsertBefore(childNode, logo.LogoNodeHtml);
                            }

                        }



                    }


                }

                foreach (LogoClassHtml logo in logosListHtml)
                {

                    if (logo.LogoNodeHtml != null && logo.LogoNodeHtml.ParentNode != null)
                    {
                        logo.LogoNodeHtml.ParentNode.RemoveChild(logo.LogoNodeHtml);
                    }


                }


            }



            var result = htmlString;
            if (document != null)
            {
                var sw = new StringWriter();
                document.Save(sw);
                result = sw.ToString();
            }

            return result;
        }





        public string GetQuoteHtmlTemplate(string htmlString, string entityId, string objectTableId, string childEntityId, string childEntityObjectTableId, int tenant, string userId)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            CommonDataDomainService commonService = new CommonDataDomainService();

            CurrentTenant = context.Tenants.Where(t => t.Id == tenant).FirstOrDefault();



            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string datetimeformat = @"dd\/MM\/yyyy";
            if (!string.IsNullOrEmpty(CurrentTenant.DateTimeFormat))
            {
                datetimeformat = CurrentTenant.DateTimeFormat;
            }
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = datetimeformat;
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm";



            logosList = new List<LogoClass>();
            tablesRepository = new ObjectTableRepository(tenant);

            ObjectTable entityTable = tablesRepository.GetObjectTableById(objectTableId, tenant);
            string entityName = entityTable.Name;

            securityKey = "";
            //securityKeyNode = null;
            shipmentNumbersNodes = new List<XmlNode>();
            string childEntityName = "";
            if (childEntityId != null && childEntityObjectTableId != null)
            {
                ObjectTable childEntityTable = tablesRepository.GetObjectTableById(childEntityObjectTableId, tenant);

                childEntityName = childEntityTable.Name;
            }



            generalService = new GeneralDomainService();



            SystemDataQuery systemDataQuery = new SystemDataQuery();
            SystemDataPM systemEntity = systemDataQuery.GetSinglePM(userId, tenant);

            List<ObjectField> systemEntityObjectFields = GetEntityObjectFields("SystemData", tenant);// generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName("SystemData", tenant).ToList();


            entity = GetEntity(entityName, entityId, tenant);
            entityObjectFields = GetEntityObjectFields(entityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(entityName, tenant).ToList();

            childEntityObjectFields = new List<ObjectField>();
            if (childEntityId != null && childEntityObjectTableId != null)
            {
                childEntity = GetEntity(childEntityName, childEntityId, tenant);
                childEntityObjectFields = GetEntityObjectFields(childEntityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(childEntityName, tenant).ToList();
            }


            if (entity != null)
            {


                string resultString = htmlString;
                XmlReader reader = XmlReader.Create(new StringReader(htmlString));
                XmlDataDocument messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);

                XmlDocument doc2 = (XmlDocument)messageDoc.Clone();

                List<XmlNode> signatureNodeList = new List<XmlNode>();

                // Ticket Work 
                List<XmlNode> ticketHeaderNode = new List<XmlNode>();
                List<XmlNode> ticketFooterNode = new List<XmlNode>();

                XmlNodeList spansList = messageDoc.GetElementsByTagName("t:Span");

                Dictionary<XmlNode, XmlNode> tablesDic = new Dictionary<XmlNode, XmlNode>();
                signature = " ";
                foreach (XmlNode node in spansList)
                {
                    if (!String.IsNullOrEmpty(node.Attributes["Text"].Value))
                    {
                        string textValue = node.Attributes["Text"].Value;
                        if (textValue.Contains("[") && textValue.Contains("]"))
                        {
                            GetNodeValue(node, textValue, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService, ticketHeaderNode, ticketFooterNode);
                        }

                    }
                }

                if (signatureNodeList.Count() != 0)
                {
                    if (!String.IsNullOrEmpty(signature.Trim()))
                    {
                        XmlNode newSignatureSection = GetNewSignatureNode(signature, systemEntity, systemEntityObjectFields, tenant, generalService);



                        foreach (XmlNode signatureNode in signatureNodeList)
                        {
                            foreach (XmlNode childNode in newSignatureSection.ChildNodes)
                            {
                                XmlNode t2Node = messageDoc.ImportNode(childNode, true);

                                signatureNode.ParentNode.ParentNode.InsertBefore(t2Node, signatureNode.ParentNode);
                            }
                        }


                    }


                    foreach (XmlNode signatureNode in signatureNodeList)
                    {
                        if (signatureNode.ParentNode.ParentNode != null)
                        {
                            signatureNode.ParentNode.ParentNode.RemoveChild(signatureNode.ParentNode);

                        }
                    }



                }


                if (logosList.Count() != 0)
                {

                    foreach (LogoClass logo in logosList)
                    {
                        if (!String.IsNullOrEmpty(logo.LogoString.Trim()))
                        {
                            XmlNode newLogoSection = GetNewNode(logo.LogoString);

                            foreach (XmlNode childNode in newLogoSection.ChildNodes)
                            {
                                XmlNode t2Node = messageDoc.ImportNode(childNode, true);


                                logo.LogoNode.ParentNode.ParentNode.InsertBefore(t2Node, logo.LogoNode.ParentNode);

                            }



                        }


                    }

                    foreach (LogoClass logo in logosList)
                    {
                        if (logo.LogoNode.ParentNode.ParentNode != null)
                        {
                            logo.LogoNode.ParentNode.ParentNode.RemoveChild(logo.LogoNode.ParentNode);
                        }

                    }


                }

                //================================================================

                if (tablesDic.Count() != 0)
                {
                    foreach (XmlNode node in tablesDic.Keys)
                    {
                        XmlNode t2Node = messageDoc.ImportNode(tablesDic[node], true);
                        if (node.ParentNode != null)
                        {
                            node.ParentNode.ReplaceChild(t2Node, node);
                        }
                    }
                }


                foreach (XmlNode securityKeyNode in shipmentNumbersNodes)
                {

                    if (securityKeyNode != null && !string.IsNullOrEmpty(securityKey))
                    {

                        if (securityKeyNode.ParentNode != null)
                        {
                            XmlNode newLogoSection = GetNewNode(GetSecurityKeyLink(securityKey, entityId, securityKeyNode.ParentNode.InnerXml, tenant));

                            foreach (XmlNode childNode in newLogoSection.ChildNodes)
                            {
                                XmlNode t2Node = messageDoc.ImportNode(childNode, true);


                                securityKeyNode.ParentNode.ParentNode.InsertBefore(t2Node, securityKeyNode.ParentNode);

                            }

                            if (securityKeyNode.ParentNode.ParentNode != null)
                            {
                                securityKeyNode.ParentNode.ParentNode.RemoveChild(securityKeyNode.ParentNode);
                            }


                        }
                    }
                }




                return messageDoc.InnerXml;

            }
            else
            {
                return null;
            }
        }

        public byte[] GetSentMessageHtmlBody(string documentId, int tenant)
        {
            byte[] resultFile = null;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            Simplog.Data.CommonDataModel.EntityPOCOs.Document document = (from doc in commonContext.Documents
                                                                          where doc.Id == documentId
                                                                          select doc).FirstOrDefault();
            if (document != null)
            {
                try
                {
                    //Check if InAzure 
                    //if (!WebFreightEntryPoint.UsingAzure)
                    //{

                    //    string filePath = Server.MapPath(".");
                    //    string path = Server.MapPath(".");
                    //    path += "\\UserUploads\\";
                    //    path += documentId;
                    //    path += "." + document.Extension;

                    //    FileStream fs = File.OpenRead(path);
                    //    resultFile = new byte[fs.Length];
                    //    fs.Read(resultFile, 0, resultFile.Length);
                    //    fs.Close();

                    //}

                    //else // In Azure = true
                    //{

                    //string filename = documentId + "." + document.Extension;
                    //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                    //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));

                    //        blobfile.DownloadToStream(memstream);
                    //        resultFile = memstream.ToArray();

                    //    }
                    //}

                    // string filename = documentId + "." + document.Extension;
                    // string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = document.FileSize,

                    };
                    resultFile = storageservice.Read(fileInfo);


                    // }

                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    string htmlString = enc.GetString(resultFile);


                    string[] htmlStringArray = htmlString.Split('<');

                    List<string> imagesList = htmlStringArray.Where(s => s.StartsWith("img")).ToList();
                    ////img width="300" height="192" src='cid:logo1' />

                    foreach (string imageString in imagesList)
                    {
                        if (imageString.Contains("cid:"))
                        {
                            //int startIndex = imageString.IndexOf("cid:") + 4;
                            //int endIndex = imageString.IndexOf("/>") - startIndex;
                            //string imageName = imageString.Substring(startIndex, endIndex).Trim();

                            string imageName = getBetween(imageString, "cid:", "'");


                            if (imageName.Contains("'"))
                            {
                                imageName = imageName.Replace("'", "");
                            }

                            byte[] logoFile = GetFileFromServer(imageName, "jpg", "logos", tenant);
                            //img width="300" height="192" src = 'cid:logo1' />
                            char[] base64Data;
                            if (logoFile != null)
                            {
                                base64Data = new char[(int)(Math.Ceiling((double)logoFile.Length / 3) * 4)];

                                Convert.ToBase64CharArray(logoFile, 0, logoFile.Length, base64Data, 0);

                                string rawData = new String(base64Data);

                                try
                                {
                                    MemoryStream ms = new MemoryStream(logoFile, 0, logoFile.Length);          // Convert byte[] to Image    
                                    ms.Write(logoFile, 0, logoFile.Length);
                                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);


                                    string width = image.Width.ToString();
                                    string height = image.Height.ToString();


                                    string[] srcStringArray = imageString.Split(' ');

                                    List<string> srcList = srcStringArray.Where(s => s.StartsWith("src")).ToList();
                                    if (srcList.FirstOrDefault() != null)
                                    {
                                        string src = srcList.FirstOrDefault();
                                        if (!src.Contains("/>"))
                                        {
                                            htmlString = htmlString.Replace(src, "src='data:image/jpg;base64," + rawData + "'");
                                        }
                                        else
                                        {
                                            htmlString = htmlString.Replace(src, "src='data:image/jpg;base64," + rawData + "'/>");
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
                                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "HtmlEditorWebService : GetSentMessageHtmlBody1 Method", ip);
                                }

                            }


                            resultFile = enc.GetBytes(htmlString);
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
                    ExceptionHandler.HandleException(e, DateTime.Now, tenant, HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "", "HtmlEditorWebService : GetSentMessageHtmlBody2 Method", ip);
                }

            }
            return resultFile;
        }

        public string SendEmailOutActivityForEntity(byte[] htmlData, byte[] textData, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string myEntityId, string customerId, string objectTableId, string attachments, string entityReference, string documentTypeCode, string eventTypeCode)
        {
            string result = null;

            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            ICRMContext crmContext = CRMContext.GetContext(tenant);

            DocumentOutRepository documentOutRepository = new DocumentOutRepository(context);
            DocumentRepository documentRep = new DocumentRepository(context);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(new DocumentTypeRepository(context));
            UserRepository userRepository = new UserRepository(context);
            CustomerRepository customerRepository = new CustomerRepository(context);

            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ContactRepository contactRepository = new ContactRepository(context);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(context);
            ObjectTable table = objectTableRepository.GetSingleObjectTable(objectTableId, 0, false);

            User user = userRepository.GetSingleUser(userId, tenant, false);
            if (user == null)
            {
                user = userRepository.GetSingleUser(userId, 0, false);
            }

            Customer customer = customerRepository.GetSingleCustomer(customerId, tenant, false);

            if (HttpContext.Current != null)
            {
                SecurityUtility.CheckFeatureAccessLevelPermission("Activity", "NEW", userId, user.BusinessUnitId, tenant);
            }


            if (!string.IsNullOrEmpty(toEmail))
            {
                string entityId = null;
                string messagesubject = subject;

                if (table.Name == "Opportunity" || table.Name == "Quote")
                {
                    entityId = myEntityId;
                }
                else
                {

                    entityId = customerId;

                }


                DocumentTypePM documentType = documentTypeQuery.GetSinglePMByCodeAndTenant(documentTypeCode, tenant);
                if (documentType == null)
                {
                    throw new Exception("Document Type with code  " + documentTypeCode + " is not found!");

                }

                DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(entityId, documentType.Id, tenant);//documentOutQuery.GetDocumentOutByDocumentTypeEntityAndChild(entityId, null, documentType.Id, tenant);

                if (documentout == null)
                {

                    DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentType.Id, EntityId = entityId, Tenant = tenant, ObjectTableId = objectTableId, ChildEntityId = null, ChildEntityReference = null, DirectionCode = "O" };
                    newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
                    newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                    newDocumentFiling.CreatedByUserId = userId;
                    newDocumentFiling.OwnerId = userId;
                    newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.UpdatedByUserId = userId;
                    newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;

                    newDocumentFiling.SecurityId = newDocumentFiling.Id + StringHelper.GetRandomString(10);

                    documentsFilingRepository.Add(newDocumentFiling);

                    documentout = new DocumentOut() { Id = newDocumentFiling.Id, EmailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId, DocumentTemplateId = documentType.DocumentTypeDefaultReportTemplateId, Tenant = tenant, Issued = false };
                    //documentout.Id = IdCounter.GetNumber("Document", tenant).ToString();
                    documentout.IsBlobExist = true;

                    documentOutRepository.Add(documentout);
                    documentOutRepository.SubmitChanges();

                }
                else
                {
                    documentout.IsBlobExist = true;
                    documentout.Issued = true;
                    documentout.IssuedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentout.IssuedByUserId = userId;
                    documentout.DocumentsFiling.UpdatedByUserId = userId;
                    documentout.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentOutRepository.Update(documentout);
                }



                Simplog.Data.CommonDataModel.EntityPOCOs.Document document = new Simplog.Data.CommonDataModel.EntityPOCOs.Document()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    Extension = "html",
                    FileSize = Convert.ToInt32(htmlData.Length),
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant).ToString(),
                    Folder = "docsout",
                    HasFile = true,

                };

                documentRep.Add(document);
                context.SaveChanges();

                //string filename = document.Id + ".html";
                //CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));


                //using (Stream memstream = blobfile.OpenWrite())
                //{
                //    memstream.Write(htmlData, 0, htmlData.Length);
                //    memstream.Close();

                //}
                string filename = document.Id + ".html";
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = htmlData.Length,

                };
                storageservice.Write(htmlData, fileInfo);

                // Save Html to CommunicationLog

                CommunicationLog log = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    InOut = "O",
                    To = toEmail,
                    CC = cc,
                    BCC = bcc,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    DocumentId = document.Id,
                    CreatedByUserId = userId,
                    EntityId = entityId,
                    ObjectTableId = objectTableId,
                    DocumentOutId = documentout.Id,
                    DocumentsFilingId = null,
                    EntityReference = entityReference,
                    Subject = subject,
                    Tenant = tenant,
                    LastStatusDateUTC = DateTime.UtcNow,
                    CreateDateUTC = DateTime.UtcNow,
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationLogTypeCode = "E",
                    CommunicationStatusTypeCode = "W",
                };



                context.CommunicationLogs.Add(log);

                context.SaveChanges();

                if (attachments != null)
                {
                    string[] attachmentsArray = attachments.Split(',');
                    if (attachmentsArray.Count() != 0)
                    {
                        foreach (string docId in attachmentsArray)
                        {
                            if (!String.IsNullOrEmpty(docId))
                            {
                                CommunicationAttachment attachment = new CommunicationAttachment()
                                {
                                    Id = IdCounter.GetNumber("CommunicationAttachment", tenant).ToString(),
                                    CommunicationLogId = log.Id,
                                    DocumentId = docId,
                                    Tenant = tenant,
                                };

                                context.CommunicationAttachments.Add(attachment);
                            }
                        }

                        context.SaveChanges();
                    }
                }
                try
                {


					//IQueueService queueservice = QueueServiceManager.GetQueueService("EmailQueue", tenant);
					DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
					queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
                }
                catch (Exception ex)
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
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
                }



                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                string messageText = enc.GetString(textData);
                ActivityPM activityPM = new ActivityPM()
                {
                    Subject = messagesubject,
                    Tenant = tenant,
                    IsOpen = true,
                    ActivityStatusCode = "C",
                    PriorityCode = "02",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CreatedByUserId = user.Id,
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdatedByUserId = user.Id,
                    BranchId = user.BranchId,
                    OwnerId = user.Id,
                    BusinessUnitId = user.BusinessUnitId,
                    ActivityTypeCode = "EO",
                    CustomerId = customerId,
                    CommunicationLogId = log.Id,
                    Description = messageText,
                    SenderEmail = user.Contact.Email,
                    SenderContactId = user.Id,
                    IsMarkedCompleted = true,
                    SendReceiveDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                };

                if (table.Name == "Opportunity")
                {
                    activityPM.OpportunityId = myEntityId;
                }

                else if (table.Name == "Quote")
                {
                    activityPM.QuoteId = myEntityId;
                }

                ActivityUpdateService service = new ActivityUpdateService(crmContext, new Dictionary<string, IContext>(), tenant);
                activityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                List<Contact> contacts = contactRepository.GetActiveContacts(tenant).ToList();

                if (!string.IsNullOrEmpty(toEmail))
                {
                    string[] toEmails = toEmail.Split(';');
                    foreach (string email in toEmails)
                    {
                        Contact contact = contacts.Where(c => c.Email == email).FirstOrDefault();
                        ActivityEmailRecipientPM emailRecipientPM = new ActivityEmailRecipientPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            ContactId = contact != null ? contact.Id : null,
                            Email = email,
                            SenderContactId = user.Id,
                            RecipientTypeCode = "TO",
                            Tenant = tenant,
                        };

                        activityPM.ActivityEmailRecipients.Add(emailRecipientPM);

                    }
                }

                if (!string.IsNullOrEmpty(cc))
                {
                    string[] ccEmails = cc.Split(';');
                    foreach (string email in ccEmails)
                    {
                        Contact contact = contacts.Where(c => c.Email == email).FirstOrDefault();
                        ActivityEmailRecipientPM emailRecipientPM = new ActivityEmailRecipientPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            ContactId = contact != null ? contact.Id : null,
                            Email = email,
                            SenderContactId = user.Id,
                            RecipientTypeCode = "CC",
                            Tenant = tenant,
                        };

                        activityPM.ActivityEmailRecipients.Add(emailRecipientPM);

                    }

                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    string[] bccEmails = bcc.Split(';');
                    foreach (string email in bccEmails)
                    {
                        Contact contact = contacts.Where(c => c.Email == email).FirstOrDefault();
                        ActivityEmailRecipientPM emailRecipientPM = new ActivityEmailRecipientPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            ContactId = contact != null ? contact.Id : null,
                            Email = email,
                            SenderContactId = user.Id,
                            RecipientTypeCode = "BCC",
                            Tenant = tenant,
                        };

                        activityPM.ActivityEmailRecipients.Add(emailRecipientPM);

                    }
                }


                service.Update(activityPM, true);

                result = document.Id;



                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityId,
                    EventTypeCode = eventTypeCode,
                    Tenant = tenant,
                    Notes = messagesubject,
                    ObjectTableName = table.Name,
                    UserId = userId,
                });

                //    scope.Complete();
                //}


                //[opportunity subject / customer code] message subject
            }

            return result;
        }

        public string SendHtmlDocument(byte[] htmlData, string internalDocumentId, string externalDocumentId, int tenant, string toEmail, string subject, string cc, string bcc, string userId, string entityId, string objectTableId, string attachments, string entityReference, string from, string replyTo)
        {
            ICommonDataContext context = CommonDataContext.GetContext(tenant);
            //ShipmentsContext shipmentsContext = new ShipmentsContext();
            DocumentOutRepository internalDocRep = new DocumentOutRepository(context);
            DocumentRepository documentRep = new DocumentRepository(context);

            ShipmentsDomainService shipmentsService = new ShipmentsDomainService();


            //DocumentType documentType = (from d in context.DocumentTypes
            //                             where d.Id == documentTypeId && d.Tenant == tenant
            //                             select d).FirstOrDefault();


            DocumentOut internalDocument = internalDocRep.GetSingleDocumentOut(internalDocumentId, tenant);
            DocumentOutCopyRepository documentoutCopyRep = new DocumentOutCopyRepository(tenant);
            List<DocumentOutCopy> documentOutCopies = null;
            if (internalDocument != null)
            {
                if (internalDocument.DocumentsFiling.DocumentType.IsDocumentOneTimePrintLimited)
                {
                    documentOutCopies = documentoutCopyRep.GetDocumentOutCopyByDocumentOutId(internalDocument.Id, tenant);
                }

                //internalDocument.DocumentId = document.Id;
                internalDocument.IssuedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                internalDocument.IssuedByUserId = userId;

                internalDocument.Issued = true;
                internalDocument.DocumentsFiling.UpdatedByUserId = userId;
                internalDocument.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);


                internalDocRep.Update(internalDocument);
            }



            Simplog.Data.CommonDataModel.EntityPOCOs.Document document = new Simplog.Data.CommonDataModel.EntityPOCOs.Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "html",
                FileSize = Convert.ToInt32(htmlData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Folder = "docsout",
                HasFile = true,
            };


            documentRep.Add(document);
            documentRep.SubmitChanges();


            internalDocRep.SubmitChanges();

            //if (!WebFreightEntryPoint.UsingAzure)
            //{
            //    try
            //    {
            //        string filePath = Server.MapPath(".");
            //        filePath += "\\UserUploads\\";
            //        filePath += document.Id;
            //        filePath += ".html";

            //        FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            //        BinaryWriter bw = new BinaryWriter(fs);
            //        bw.Write(htmlData);
            //        bw.Close();
            //    }
            //    catch
            //    {
            //    }
            //}
            //else // In Azure
            //{
            try
            {
                string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(document.Id + ".html", document.Folder);

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = htmlData.Length,


                };
                storageservice.Write(htmlData, fileInfo);
                //string filename = document.Id + ".html";
                // CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
                //var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder));


                //using (Stream memstream = blobfile.OpenWrite())
                //{
                //    memstream.Write(htmlData, 0, htmlData.Length);
                //    memstream.Close();
                //    //memstream.Write(htmlData, 0, htmlData.Length);
                //    //blobfile.UploadFromStream(memstream);
                //}
            }
            catch
            {
            }

            // }



            // Send Html Document by email
            // SendHtmlDocumentByEmail(ToEmail, Subject, CC, filePath);
            //============================

            // Save Html to CommunicationLog

            CommunicationLog log = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                InOut = "O",
                To = toEmail,
                CC = cc,
                BCC = bcc,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = document.Id,
                CreatedByUserId = userId,
                EntityId = entityId,
                ObjectTableId = objectTableId,
                DocumentOutId = internalDocumentId,
                DocumentsFilingId = externalDocumentId,
                EntityReference = entityReference,
                Subject = subject,
                Tenant = tenant,
                LastStatusDateUTC = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreateDateUTC = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationLogTypeCode = "E",
                CommunicationStatusTypeCode = "W",


            };

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrWhiteSpace(from))
            {
                log.From = from;
            }
            if (!string.IsNullOrEmpty(replyTo) && !string.IsNullOrWhiteSpace(replyTo))
            {
                log.ReplyToList = replyTo;
            }



            //EventTracer.CreateTraceEvent(new TraceEvent(), "CRCR", currency.Tenant, contact.Id, currency.Id, null, "Currency", null, null, false);

            context.CommunicationLogs.Add(log);
            try
            {
                context.SaveChanges();
            }
            catch (Exception eeee)
            { }


            if (attachments != null)
            {
                string[] attachmentsArray = attachments.Split(',');
                if (attachmentsArray.Count() != 0)
                {
                    foreach (string docId in attachmentsArray)
                    {
                        if (!String.IsNullOrEmpty(docId))
                        {
                            if (internalDocument != null)
                            {
                                DocumentOutCopy copy = null;
                                DocumentType documentType = internalDocument.DocumentsFiling.DocumentType;
                                if (documentOutCopies == null)
                                {
                                    copy = documentoutCopyRep.GetSingleDocumentOutCopy(docId);
                                }
                                if (copy != null)
                                {
                                    DocumentTypeCopyRepository documentTypeCopyRep = new DocumentTypeCopyRepository(context);
                                    DocumentTypeCopy typeCopy = documentTypeCopyRep.GetSingleDocumentTypeCopy(copy.DocumentTypeCopyId);
                                    DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(context);
                                    documentType = documentTypeRep.GetSingleDocumentTypes(typeCopy.DocumentTypeId, tenant);

                                }

                                if (documentType.IsDocumentOneTimePrintLimited)
                                {
                                    if (copy == null)
                                    {
                                        copy = documentOutCopies.Where(d => d.Id == docId).FirstOrDefault();
                                    }
                                    if (copy != null && copy.DocumentTypeCopyId == documentType.LimitedPrintCopyId)
                                    {
                                        string email = HttpContext.Current.User.Identity.Name;
                                        UserRepository userRep = new UserRepository(tenant);
                                        User printedBy = userRep.GetSingleUserByCodeOrEmailForTenant(null, email, tenant, false);
                                        copy.LastPrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                        copy.LastPrintedByUserId = printedBy.Id;
                                        documentoutCopyRep.Update(copy);
                                        documentoutCopyRep.SubmitChanges();
                                    }

                                }
                            }

                            CommunicationAttachment attachment = new CommunicationAttachment()
                            {
                                Id = IdCounter.GetNumber("CommunicationAttachment", tenant).ToString(),
                                CommunicationLogId = log.Id,
                                DocumentId = docId,
                                Tenant = tenant,
                            };

                            context.CommunicationAttachments.Add(attachment);
                        }
                    }

                    context.SaveChanges();
                }
            }
            try
            {

				//IQueueService queueservice = QueueServiceManager.GetQueueService("EmailQueue", tenant);
				DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
				queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
            }
            catch (Exception ex)
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
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "web role", null, ip);
            }




            return document.Id;
        }

        #region GetEntity


        public object GetEntity(string entityName, string entityId, int tenant)
        {
            if (entityName == "Master")
            {
                entityName = "Shipment";
            }


            //string entityNameValue = entityName +"Entity"+ entityId + tenant;
            //if (HttpContext.Current.Cache.Get(entityNameValue) == null)
            //{

            Assembly blAssembly = Assembly.Load("Logitude.BL");


            object entityQuery = null;
            object theEntity = null;
            string typePath = "Logitude.BL.ShipmentsModel.EntityQueries." + entityName + "Query";
            string pmtypePath = "Logitude.BL.ShipmentsModel.EntityPMs." + entityName + "PM";

            Type pmtype = blAssembly.GetType(pmtypePath);
            Type type = blAssembly.GetType(typePath);


            if (type == null)
            {
                typePath = "Logitude.BL.CommonDataModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.CommonDataModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                typePath = "Logitude.BL." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {

                typePath = "Logitude.BL.InfrastructureModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.InfrastructureModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {

                typePath = "Logitude.BL.QuoteModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.QuoteModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }


            if (type == null)
            {

                typePath = "Logitude.BL.InvoiceModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.InvoiceModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {

                Assembly assembly = Assembly.Load("Logitude.CRM.BL");
                typePath = "Logitude.CRM.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.CRM.BL.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);

            }
            if (type == null)
            {

                Assembly assembly = Assembly.Load("Logitude.Customs.BL");
                typePath = "Logitude.Customs.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.Customs.Def.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);

            }

            if (type == null)
            {

                Assembly assembly = Assembly.Load("Logitude.BookingLib.BL");
                typePath = "Logitude.BookingLib.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.BookingLib.BL.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);

            }

            if (type == null)
            {

                Assembly assembly = Assembly.Load("Logitude.WarehouseLib.BL");
                typePath = "Logitude.WarehouseLib.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.WarehouseLib.BL.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);

            }


            if (type != null)
            {
                entityQuery = Activator.CreateInstance(type, tenant);

                MethodInfo methodInfo = null;

                if (entityName == "Shipment")
                {
                    methodInfo = entityQuery.GetType().GetMethod("GetSinglePMWithLists");
                }
                else
                {
                    //methodInfo = entityQuery.GetType().GetMethod("GetSinglePM");

                    MethodInfo[] MethodInfoList = entityQuery.GetType().GetMethods();
                    methodInfo = MethodInfoList.Where(d => d.Name == "GetSinglePM").FirstOrDefault();


                    if (methodInfo == null)
                    {
                        methodInfo = entityQuery.GetType().GetMethod("GetSingle");
                    }
                    if (methodInfo == null)
                    {
                        methodInfo = entityQuery.GetType().GetMethod("GetSingle" + entityName + "PM");
                    }
                }

                ParameterInfo[] methodParameters = methodInfo.GetParameters();
                object[] parameters = new object[] { };
                switch (methodParameters.Count())
                {
                    case 1:
                        parameters = new object[] { entityId };
                        break;
                    case 2:
                        parameters = new object[] { entityId, tenant };
                        break;
                    case 3:
                        parameters = new object[] { entityId, true, false };
                        break;
                }

                if (methodInfo != null)
                {
                    theEntity = methodInfo.Invoke(entityQuery, parameters);
                }
                else
                {
                    throw new Exception("GetSingle" + entityName + "PM" + "not found!");
                }


            }

            if (theEntity == null && pmtype != null)
            {
                theEntity = Activator.CreateInstance(pmtype);
            }

            //}
            //else
            //{

            //    entity = (object)HttpContext.Current.Cache.Get(entityNameValue);

            //}

            //if (entity != null)
            //{
            //    if (entityName == "Shipment")
            //    {
            //        ShipmentPM shipment = entity as ShipmentPM;
            //        if(shipment.GrossWeightUnitCode == "MT")
            //        {
            //            shipment.OrderGrossWeight = shipment.OrderGrossWeight != null ? shipment.OrderGrossWeight / 1000 : null;
            //            shipment.OrderVolumetricWeight = shipment.OrderVolumetricWeight != null ? shipment.OrderVolumetricWeight / 1000 : null;
            //            shipment.OrderChargeableWeight = shipment.OrderChargeableWeight != null ? shipment.OrderChargeableWeight / 1000 : null;
            //            shipment.GrossWeightInKG = shipment.GrossWeightInKG != null ? shipment.GrossWeightInKG / 1000 : null;
            //            shipment.VolumetricWeight = shipment.VolumetricWeight != null ? shipment.VolumetricWeight / 1000 : null;
            //            shipment.ChargeableWeightInKG = shipment.ChargeableWeightInKG != null ? shipment.ChargeableWeightInKG / 1000 : null;


            //        }

            //        if (shipment.GrossWeightUnitCode == "LB")
            //        {
            //            shipment.OrderGrossWeight = shipment.OrderGrossWeight != null ? shipment.OrderGrossWeight * 2.20458 : null;
            //            shipment.OrderVolumetricWeight = shipment.OrderVolumetricWeight != null ? shipment.OrderVolumetricWeight * 2.20458 : null;
            //            shipment.OrderChargeableWeight = shipment.OrderChargeableWeight != null ? shipment.OrderChargeableWeight * 2.20458 : null;
            //            shipment.GrossWeight = shipment.GrossWeight != null ? shipment.GrossWeight * 2.20458 : null;
            //            shipment.VolumetricWeight = shipment.VolumetricWeight != null ? shipment.VolumetricWeight * 2.20458 : null;
            //            shipment.ChargeableWeight = shipment.ChargeableWeight != null ? shipment.ChargeableWeight * 2.20458 : null;


            //        }
            //    }

            //    if (entityName == "Quote")
            //    {

            //        QuotePM qoute = entity as QuotePM;
            //        if (qoute.GrossWeightUnitCode == "MT")
            //        {

            //            qoute.GrossWeight = qoute.GrossWeight != null ? qoute.GrossWeight / 1000 : null;
            //            qoute.VolumetricWeight = qoute.VolumetricWeight != null ? qoute.VolumetricWeight / 1000 : null;
            //            qoute.ChargeableWeight = qoute.ChargeableWeight != null ? qoute.ChargeableWeight / 1000 : null;


            //        }

            //        if (qoute.GrossWeightUnitCode == "LB")
            //        {

            //            qoute.GrossWeight = qoute.GrossWeight != null ? qoute.GrossWeight * 2.20458 : null;
            //            qoute.VolumetricWeight = qoute.VolumetricWeight != null ? qoute.VolumetricWeight * 2.20458 : null;
            //            qoute.ChargeableWeight = qoute.ChargeableWeight != null ? qoute.ChargeableWeight * 2.20458 : null;


            //        }
            //    }
            //}

            return theEntity;
        }

        #endregion

        #region GetEntityObjectFields
        private List<ObjectField> GetEntityObjectFields(string objectTableName, int tenant)
        {
            List<ObjectField> theEntityObjectFields;
            //string ObjectFieldsListName = objectTableName + "ObjectFieldList" + tenant;
            //if (HttpContext.Current.Cache.Get(ObjectFieldsListName) == null)
            //{
            //ObjectFieldsRepository ObjectFieldsRep = new ObjectFieldsRepository(tenant);
            theEntityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList();
            //    HttpContext.Current.Cache.Insert(ObjectFieldsListName, entityObjectFields, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //}
            //else
            //{
            //    entityObjectFields = (List<ObjectField>)HttpContext.Current.Cache.Get(ObjectFieldsListName);
            //}

            return theEntityObjectFields;
        }
        #endregion

        #region GetNodeValue


        public string GetNodeValue(XmlNode node, string nodeText, object theEntity, List<ObjectField> theEntityObjectFields, Dictionary<XmlNode, XmlNode> tablesDic, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, List<XmlNode> signatureNodeList, int tenant, GeneralDomainService theGeneralService, List<XmlNode> ticketHeaderNode, List<XmlNode> ticketFooterNode)
        {

            string nodeTextValue = " ";
            string[] properties = nodeText.Split('[');

            foreach (string property in properties)
            {
                if (property.Contains("]"))
                {
                    string[] array = property.Split(']');
                    string propertyName = array[0];
                    if (propertyName.Contains("."))
                    {
                        string[] fields = propertyName.Split('.');
                        ObjectField objectField = null;
                        if (fields[0] != "SystemData")
                        {

                            if (childEntity != null)
                            {
                                objectField = childEntityObjectFields.Where(f => f.FieldName == fields[0]).FirstOrDefault();
                                if (objectField != null)
                                {

                                    nodeTextValue = ResolveObjectFieldValue(objectField, fields, tenant, propertyName, systemEntity, systemEntityObjectFields, tablesDic, node, nodeTextValue, childEntity, childEntityObjectFields);
                                }
                                else
                                {
                                    objectField = theEntityObjectFields.Where(f => f.FieldName == fields[0]).FirstOrDefault();

                                    if (objectField != null)
                                    {

                                        nodeTextValue = ResolveObjectFieldValue(objectField, fields, tenant, propertyName, systemEntity, systemEntityObjectFields, tablesDic, node, nodeTextValue, theEntity, theEntityObjectFields);
                                    }
                                }
                            }
                            else
                            {
                                objectField = theEntityObjectFields.Where(f => f.FieldName == fields[0]).FirstOrDefault();

                                if (objectField != null)
                                {

                                    nodeTextValue = ResolveObjectFieldValue(objectField, fields, tenant, propertyName, systemEntity, systemEntityObjectFields, tablesDic, node, nodeTextValue, theEntity, theEntityObjectFields);
                                }
                            }

                        }
                        else
                        {
                            propertyName = propertyName.Replace("SystemData.", "");
                            if (propertyName.Contains("."))
                            {
                                string[] systemFields = propertyName.Split('.');

                                string resultValue = GetInsideEntityFieldValue(systemEntity, systemEntityObjectFields, systemFields, tenant, theGeneralService);

                                node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", resultValue);

                                nodeTextValue = resultValue;
                            }
                            else
                            {
                                if (propertyName != "Signature" && propertyName != "Logo" && propertyName != "SmallLogo" && propertyName != "TicketHeader" && propertyName != "TicketFooter")
                                {


                                    string resultValue = GetEntityFieldValue(systemEntity, propertyName, systemEntityObjectFields, tenant);
                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", resultValue);

                                    nodeTextValue = resultValue;

                                }
                                else
                                {
                                    if (propertyName == "Logo" || propertyName == "SmallLogo")
                                    {
                                        string logoString = GetTenantLogo(propertyName, tenant);

                                        if (!String.IsNullOrEmpty(logoString.Trim()))
                                        {
                                            LogoClass logo = new LogoClass(node, logoString);
                                            logosList.Add(logo);

                                            //logoNodeList.Add(node);
                                        }
                                        else
                                        {
                                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", " ");
                                        }
                                    }
                                    else
                                    {
                                        //if (propertyName == "TicketHeader" || propertyName == "TicketFooter")
                                        //{
                                        //    if (propertyName == "TicketHeader")
                                        //    {
                                        //        ticketHeader = GetTicketHeader("");
                                        //        if (!String.IsNullOrEmpty(ticketHeader.Trim()))
                                        //        {
                                        //            ticketHeaderNode = node;
                                        //        }
                                        //        else
                                        //        {
                                        //            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", " ");
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        ticketFooter = GetTicketFooter("");
                                        //        if (!String.IsNullOrEmpty(ticketFooter.Trim()))
                                        //        {
                                        //            ticketFooterNode = node;
                                        //        }
                                        //        else
                                        //        {
                                        //            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", " ");
                                        //        }
                                        //    }
                                        //}

                                        //else
                                        //{
                                        signature = GetUserSignature(systemEntity);
                                        if (!String.IsNullOrEmpty(signature.Trim()))
                                        {
                                            signatureNodeList.Add(node);
                                        }
                                        else
                                        {
                                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", " ");
                                        }
                                        //}
                                    }
                                }
                            }

                        }


                    }

                    else
                    {
                        ObjectField objectField = null;
                        if (childEntity != null)
                        {
                            objectField = childEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();

                            if (objectField != null && childEntity != null)
                            {
                                string resultValue = GetEntityFieldValue(childEntity, propertyName, childEntityObjectFields, tenant);
                                node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue);

                                nodeTextValue = resultValue;
                            }
                            else
                            {
                                objectField = theEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();

                                if (objectField != null)
                                {

                                    string resultValue = GetEntityFieldValue(theEntity, propertyName, childEntityObjectFields, tenant);
                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue);

                                    nodeTextValue = resultValue;
                                }
                            }
                        }
                        else
                        {
                            objectField = theEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();

                            if (objectField != null)
                            {
                                string resultValue = GetEntityFieldValue(theEntity, propertyName, theEntityObjectFields, tenant);

                                if ((propertyName == "SecurityKey" || (propertyName == "ShipmentNumber" && ObjectTableName == "Shipment")) && CurrentTenant != null && CurrentTenant.SharedLogisticsMessageLink)
                                {
                                    if (propertyName == "SecurityKey")
                                    {
                                        this.securityKey = resultValue;
                                        shipmentNumbersNodes.Add(node);

                                        node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", " ");
                                    }
                                    else
                                    {
                                        this.securityKey = GetEntityFieldValue(theEntity, "SecurityKey", theEntityObjectFields, tenant);
                                        node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue + " ");
                                        //this.securityKeyNode = node;
                                        shipmentNumbersNodes.Add(node);
                                    }

                                }
                                else if (propertyName == "TicketHeader" || propertyName == "TicketFooter")
                                {
                                    if (propertyName == "TicketHeader")
                                    {
                                        ticketHeader = GetTicketHeader(resultValue);
                                        if (!String.IsNullOrEmpty(ticketHeader.Trim()))
                                        {
                                            ticketHeaderNode.Add(node);
                                        }
                                        else
                                        {
                                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", " ");
                                        }
                                    }
                                    else
                                    {
                                        ticketFooter = GetTicketFooter(resultValue);
                                        if (!String.IsNullOrEmpty(ticketFooter.Trim()))
                                        {
                                            ticketFooterNode.Add(node);
                                        }
                                        else
                                        {
                                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", " ");
                                        }
                                    }
                                }
                                else
                                {
                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue);
                                    nodeTextValue = resultValue;
                                }
                            }
                        }
                    }
                }
            }

            return nodeTextValue;
        }

        #endregion

        #region GetNewNode

        public XmlNode GetNewNode(String nodeString)
        {
            XmlDocument ndoc = new XmlDocument();
            ndoc.LoadXml(nodeString);
            XmlNode xNode = ndoc.DocumentElement;
            return xNode;
        }


        #endregion

        #region GetNewSignatureNode

        public XmlNode GetNewSignatureNode(String nodeString, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, int tenant, GeneralDomainService theGeneralService)
        {
            XmlDataDocument ndoc = new XmlDataDocument();
            ndoc.LoadXml(nodeString);

            ResolveSignatureFields(ndoc, systemEntity, systemEntityObjectFields, tenant, theGeneralService);


            if (signatureLogosList.Count() != 0)
            {

                foreach (LogoClass logo in signatureLogosList)
                {
                    if (!String.IsNullOrEmpty(logo.LogoString.Trim()))
                    {
                        XmlNode newLogoSection = GetNewNode(logo.LogoString);

                        foreach (XmlNode childNode in newLogoSection.ChildNodes)
                        {
                            XmlNode t2Node = ndoc.ImportNode(childNode, true);


                            logo.LogoNode.ParentNode.ParentNode.InsertBefore(t2Node, logo.LogoNode.ParentNode);

                        }



                    }


                }


                foreach (LogoClass logo in signatureLogosList)
                {
                    if (logo.LogoNode.ParentNode.ParentNode != null)
                    {
                        logo.LogoNode.ParentNode.ParentNode.RemoveChild(logo.LogoNode.ParentNode);
                    }

                }
            }




            XmlNode xNode = ndoc.DocumentElement;
            return xNode;
        }


        #endregion

        #region GetSecurityKeyLink
        public string GetSecurityKeyLink(string key, string entityId, string originalNodeInnerText, int tenant)
        {



            string linkCode = "";
            string linkNodeString = " ";


            string serverPath = LogitudeSettings.LogitudeURL;//System.Configuration.ConfigurationManager.AppSettings.Get("LogitudeURL");//HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Replace(HttpContext.Current.Request.UrlReferrer.PathAndQuery, "");

            string pageLink = (serverPath + @"/SharedLogistic/ShipmentPage.aspx").ToLower() + "?securitykey=" + key + ":" + entityId + ":" + tenant;
            int annotationId = Math.Abs(System.Guid.NewGuid().GetHashCode());
            linkNodeString = originalNodeInnerText;
            linkNodeString += "<t:HyperlinkRangeStart AnnotationID='" + annotationId + "'>"
                                 + "<t:HyperlinkInfo NavigateUri='" + pageLink + "' " + "/>"
                      + "</t:HyperlinkRangeStart>"
                      + "<t:Span FontFamily='Arial Black' FontSize='18.666667' Text='View online'  ForeColor='#FF0000FF' UnderlineColor='#FF0000FF' UnderlineDecoration='Line'/>"
                      + "<t:HyperlinkRangeEnd AnnotationID='" + annotationId + "' />";


            //       <t:HyperlinkRangeStart AnnotationID="1">
            //  <t:HyperlinkInfo NavigateUri="http://google.com" />
            //</t:HyperlinkRangeStart>
            //<t:Span StyleName="Hyperlink" Text="google link" />
            //<t:HyperlinkRangeEnd AnnotationID="1" />

            linkCode = "<t:Section xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents'><t:Paragraph>" + linkNodeString + "</t:Paragraph></t:Section>";


            //           <t:Section>
            //  <t:Paragraph>
            //    <t:HyperlinkRangeStart AnnotationID="1">
            //      <t:HyperlinkInfo NavigateUri="http://google.com" />
            //    </t:HyperlinkRangeStart>
            //    <t:Span StyleName="Hyperlink" Text="test" />
            //    <t:HyperlinkRangeEnd AnnotationID="1" />
            //  </t:Paragraph>
            //</t:Section>

            return linkCode;
        }
        #endregion

        #region ResolveSignatureFields

        List<LogoClass> signatureLogosList = new List<LogoClass>();
        public void ResolveSignatureFields(XmlDataDocument signatureDoc, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, int tenant, GeneralDomainService theGeneralService)
        {

            XmlNodeList spansList = signatureDoc.GetElementsByTagName("t:Span");

            foreach (XmlNode node in spansList)
            {
                if (!String.IsNullOrEmpty(node.Attributes["Text"].Value))
                {
                    string textValue = node.Attributes["Text"].Value;
                    if (textValue.Contains("[") && textValue.Contains("]"))
                    {
                        string[] properties = textValue.Split('[');

                        foreach (string property in properties)
                        {
                            if (property.Contains("]"))
                            {
                                string[] array = property.Split(']');
                                string propertyName = array[0];
                                if (propertyName.Contains("."))
                                {
                                    string[] fields = propertyName.Split('.');
                                    //ObjectField objectField = null;
                                    if (fields[0] == "SystemData")
                                    {

                                        propertyName = propertyName.Replace("SystemData.", "");
                                        if (propertyName.Contains("."))
                                        {
                                            string[] systemFields = propertyName.Split('.');

                                            string resultValue = GetInsideEntityFieldValue(systemEntity, systemEntityObjectFields, systemFields, tenant, theGeneralService);

                                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", resultValue);


                                        }
                                        else
                                        {

                                            if (propertyName != "Signature" && propertyName != "Logo" && propertyName != "SmallLogo" && propertyName != "TicketHeader" && propertyName != "TicketFooter")
                                            {
                                                string resultValue = GetEntityFieldValue(systemEntity, propertyName, systemEntityObjectFields, tenant);
                                                node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", resultValue);

                                            }
                                            else
                                            {
                                                if (propertyName == "Logo" || propertyName == "SmallLogo")
                                                {
                                                    string logoString = GetTenantLogo(propertyName, tenant);

                                                    if (!String.IsNullOrEmpty(logoString.Trim()))
                                                    {
                                                        LogoClass logo = new LogoClass(node, logoString);
                                                        signatureLogosList.Add(logo);

                                                        //logoNodeList.Add(node);
                                                    }
                                                    else
                                                    {
                                                        node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", " ");
                                                    }
                                                }
                                                else
                                                {
                                                    // signature = GetUserSignature(systemEntity);
                                                    // if (!String.IsNullOrEmpty(signature.Trim()))
                                                    // {
                                                    //signatureNodeList.Add(node);
                                                    // }
                                                    // else
                                                    // {
                                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[SystemData." + propertyName + "]", " ");
                                                    // }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //GetNodeValue(node, textValue, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);
                    }
                }
            }
        }

        #endregion

        #region New Ticket Header / Footer
        public XmlNode GetNewTicketHeaderFooterNode(String nodeString, object entity)
        {
            XmlDataDocument ndoc = new XmlDataDocument();
            ndoc.LoadXml(nodeString);

            ResolveTicektFields(ndoc, entity);

            XmlNode xNode = ndoc.DocumentElement;
            return xNode;
        }


        public HtmlNode GetNewTicketHeaderFooterNodeHtml(String nodeString, object entity)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(nodeString);

            ResolveTicektFieldsHtml(htmlDocument, entity);

            HtmlNode htmlNode = htmlDocument.DocumentNode;
            return htmlNode;


        }

        #endregion

        #region ResolveTicektFields


        public void ResolveTicektFieldsHtml(HtmlDocument ticketDoc, object ticketPM)
        {
            HtmlNodeCollection spansList = ticketDoc.DocumentNode.SelectNodes("//span");

            foreach (HtmlNode node in spansList)
            {
                if (!String.IsNullOrEmpty(node.InnerHtml))
                {
                    string textValue = node.InnerHtml;
                    if (textValue.Contains("[") && textValue.Contains("]"))
                    {
                        string[] properties = textValue.Split('[');
                        foreach (string property in properties)
                        {
                            if (property.Contains("]"))
                            {
                                string[] array = property.Split(']');
                                string propertyName = array[0];
                                PropertyInfo propInfo = entity.GetType().GetProperty(propertyName);
                                if (propInfo != null)
                                {
                                    object value = propInfo.GetValue(entity, null);
                                    string propValue = value != null ? value.ToString() : " ";
                                    node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", propValue);

                                }
                                else node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", " ");

                            }

                        }
                    }

                }

            }
        }



        public void ResolveTicektFields(XmlDataDocument ticketDoc, object ticketPM)
        {
            XmlNodeList spansList = ticketDoc.GetElementsByTagName("t:Span");

            foreach (XmlNode node in spansList)
            {
                if (!String.IsNullOrEmpty(node.Attributes["Text"].Value))
                {
                    string textValue = node.Attributes["Text"].Value;
                    if (textValue.Contains("[") && textValue.Contains("]"))
                    {
                        string[] properties = textValue.Split('[');

                        foreach (string property in properties)
                        {
                            if (property.Contains("]"))
                            {
                                string[] array = property.Split(']');
                                string propertyName = array[0];

                                PropertyInfo propInfo = entity.GetType().GetProperty(propertyName);
                                if (propInfo != null)
                                {
                                    object value = propInfo.GetValue(entity, null);
                                    string propValue = value != null ? value.ToString() : " ";
                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", propValue);
                                }
                                else
                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", " ");
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public XmlNode GetPoweredByLink(bool hasfooter)
        {

            string poweredBy = @"<t:RadDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents' xmlns:s='clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents' version='1.2' LayoutMode='Flow' LineSpacing='1.14999997615814' LineSpacingType='Auto' ParagraphDefaultSpacingAfter='10' ParagraphDefaultSpacingBefore='0' SectionDefaultPageMargin='95,95,95,95' SectionDefaultPageSize='816,1056' StyleName='defaultDocumentStyle'>"
    + "<t:RadDocument.Captions>"
     + " <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Figure' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />"
      + "<t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Table' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />"
    + "</t:RadDocument.Captions>"
    + "<t:RadDocument.ProtectionSettings>"
      + "<t:DocumentProtectionSettings EnableDocumentProtection='False' Enforce='False' HashingAlgorithm='None' HashingSpinCount='0' ProtectionMode='ReadOnly' />"
   + " </t:RadDocument.ProtectionSettings>"
    + "<t:RadDocument.Styles>"
      + "<s:StyleDefinition DisplayName='Document Default Style' IsCustom='False' IsDefault='False' IsPrimary='True' Name='defaultDocumentStyle' Type='Default'>"
        + "<s:StyleDefinition.ParagraphStyle>"
         + " <s:ParagraphProperties LineSpacing='1.14999997615814' SpacingAfter='10' />"
       + " </s:StyleDefinition.ParagraphStyle>"
        + "<s:StyleDefinition.SpanStyle>"
         + " <s:SpanProperties FontFamily='Verdana' FontSize='16' FontStyle='Normal' FontWeight='Normal' />"
        + "</s:StyleDefinition.SpanStyle>"
       + "</s:StyleDefinition>"
      + " <s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 1' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading1Char' Name='Heading1' NextStyleName='Normal' Type='Paragraph'>"
         + "<s:StyleDefinition.ParagraphStyle>"
          + " <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='32' />"
        + " </s:StyleDefinition.ParagraphStyle>"
       + "</s:StyleDefinition>"
       + "<s:StyleDefinition DisplayName='Heading 1 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading1' Name='Heading1Char' Type='Character'>"
        + " <s:StyleDefinition.SpanStyle>"
         + "  <s:SpanProperties FontSize='18.6666660308838' FontWeight='Bold' ForeColor='#FF4F81BD' ThemeFontFamily='major' ThemeForeColor='accent1' />"
        + " </s:StyleDefinition.SpanStyle>"
       + "</s:StyleDefinition>"
      + " <s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 2' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading2Char' Name='Heading2' NextStyleName='Normal' Type='Paragraph'>"
        + " <s:StyleDefinition.ParagraphStyle>"
          + " <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
        + " </s:StyleDefinition.ParagraphStyle>"
       + "</s:StyleDefinition>"
      + " <s:StyleDefinition DisplayName='Heading 2 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading2' Name='Heading2Char' Type='Character'>"
         + "<s:StyleDefinition.SpanStyle>"
           + "<s:SpanProperties FontSize='17.3333339691162' FontWeight='Bold' ForeColor='#FF4F81BD' ThemeFontFamily='major' ThemeForeColor='accent1' />"
         + "</s:StyleDefinition.SpanStyle>"
      + " </s:StyleDefinition>"
       + "<s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 3' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading3Char' Name='Heading3' NextStyleName='Normal' Type='Paragraph'>"
        + " <s:StyleDefinition.ParagraphStyle>"
           + "<s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
        + " </s:StyleDefinition.ParagraphStyle>"
      + "</s:StyleDefinition>"
      + "<s:StyleDefinition DisplayName='Heading 3 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading3' Name='Heading3Char' Type='Character'>"
        + "<s:StyleDefinition.SpanStyle>"
          + "<s:SpanProperties FontWeight='Bold' ForeColor='#FF4F81BD' ThemeFontFamily='major' ThemeForeColor='accent1' />"
        + "</s:StyleDefinition.SpanStyle>"
     + " </s:StyleDefinition>"
      + "<s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 4' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading4Char' Name='Heading4' NextStyleName='Normal' Type='Paragraph'>"
       + " <s:StyleDefinition.ParagraphStyle>"
          + " <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
        + " </s:StyleDefinition.ParagraphStyle>"
      + " </s:StyleDefinition>"
       + "<s:StyleDefinition DisplayName='Heading 4 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading4' Name='Heading4Char' Type='Character'>"
         + "<s:StyleDefinition.SpanStyle>"
          + " <s:SpanProperties FontStyle='Italic' FontWeight='Bold' ForeColor='#FF4F81BD' ThemeFontFamily='major' ThemeForeColor='accent1' />"
        + " </s:StyleDefinition.SpanStyle>"
      + " </s:StyleDefinition>"
     + "  <s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 5' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading5Char' Name='Heading5' NextStyleName='Normal' Type='Paragraph'>"
        + " <s:StyleDefinition.ParagraphStyle>"
         + "  <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
         + "</s:StyleDefinition.ParagraphStyle>"
       + "</s:StyleDefinition>"
      + " <s:StyleDefinition DisplayName='Heading 5 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading5' Name='Heading5Char' Type='Character'>"
        + " <s:StyleDefinition.SpanStyle>"
         + "  <s:SpanProperties ForeColor='#FF4F81BD' ThemeFontFamily='major' ThemeForeColor='accent1' />"
        + "</s:StyleDefinition.SpanStyle>"
       + "</s:StyleDefinition>"
     + "  <s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 6' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading6Char' Name='Heading6' NextStyleName='Normal' Type='Paragraph'>"
        + " <s:StyleDefinition.ParagraphStyle>"
          + " <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
       + "  </s:StyleDefinition.ParagraphStyle>"
      + " </s:StyleDefinition>"
      + " <s:StyleDefinition DisplayName='Heading 6 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading6' Name='Heading6Char' Type='Character'>"
        + " <s:StyleDefinition.SpanStyle>"
         + "  <s:SpanProperties FontStyle='Italic' ForeColor='#FF4F81BD' ThemeFontFamily='major' ThemeForeColor='accent1' />"
         + "</s:StyleDefinition.SpanStyle>"
       + "</s:StyleDefinition>"
      + " <s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 7' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading7Char' Name='Heading7' NextStyleName='Normal' Type='Paragraph'>"
       + "  <s:StyleDefinition.ParagraphStyle>"
          + " <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
        + " </s:StyleDefinition.ParagraphStyle>"
       + "</s:StyleDefinition>"
      + " <s:StyleDefinition DisplayName='Heading 7 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading7' Name='Heading7Char' Type='Character'>"
        + " <s:StyleDefinition.SpanStyle>"
         + "  <s:SpanProperties FontStyle='Italic' ForeColor='#FF000000' ThemeFontFamily='major' ThemeForeColor='text1' />"
        + " </s:StyleDefinition.SpanStyle>"
       + "</s:StyleDefinition>"
       + "<s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 8' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading8Char' Name='Heading8' NextStyleName='Normal' Type='Paragraph'>"
        + "<s:StyleDefinition.ParagraphStyle>"
          + "<s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
        + "</s:StyleDefinition.ParagraphStyle>"
      + "</s:StyleDefinition>"
     + " <s:StyleDefinition DisplayName='Heading 8 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading8' Name='Heading8Char' Type='Character'>"
        + "<s:StyleDefinition.SpanStyle>"
          + "<s:SpanProperties FontSize='13.3333330154419' ForeColor='#FF000000' ThemeFontFamily='major' ThemeForeColor='text1' />"
        + "</s:StyleDefinition.SpanStyle>"
     + " </s:StyleDefinition>"
     + " <s:StyleDefinition BasedOnName='Normal' DisplayName='Heading 9' IsCustom='False' IsDefault='False' IsPrimary='True' LinkedStyleName='Heading9Char' Name='Heading9' NextStyleName='Normal' Type='Paragraph'>"
      + "  <s:StyleDefinition.ParagraphStyle>"
         + " <s:ParagraphProperties KeepLines='True' SpacingAfter='0' SpacingBefore='13.3333330154419' />"
        + "</s:StyleDefinition.ParagraphStyle>"
     + " </s:StyleDefinition>"
    + "  <s:StyleDefinition DisplayName='Heading 9 Char' IsCustom='True' IsDefault='False' IsPrimary='False' LinkedStyleName='Heading9' Name='Heading9Char' Type='Character'>"
       + " <s:StyleDefinition.SpanStyle>"
        + "  <s:SpanProperties FontSize='13.3333330154419' FontStyle='Italic' ForeColor='#FF000000' ThemeFontFamily='major' ThemeForeColor='text1' />"
       + " </s:StyleDefinition.SpanStyle>"
     + " </s:StyleDefinition>"
      + "<s:StyleDefinition DisplayName='Normal' IsCustom='False' IsDefault='True' IsPrimary='True' Name='Normal' Type='Paragraph' />"
     + " <s:StyleDefinition DisplayName='TableNormal' IsCustom='False' IsDefault='True' IsPrimary='True' Name='TableNormal' Type='Table' />"
    + "</t:RadDocument.Styles>";
            if (!hasfooter)
            {
                poweredBy +=
                    "<t:Section>"
     + " <t:Section.Footers>"
       + " <t:Footers>"
        + "  <t:Footers.Default>"
          + "  <t:Footer IsLinkedToPrevious='False'>"
            + "  <t:Footer.Body>"
              + "  <t:RadDocument LayoutMode='Flow' LineSpacing='1.14999997615814' LineSpacingType='Auto' ParagraphDefaultSpacingAfter='10' ParagraphDefaultSpacingBefore='0' SectionDefaultPageMargin='95,95,95,95' SectionDefaultPageOrientation='Portrait' SectionDefaultPageSize='816,1056' StyleName='defaultDocumentStyle'>"
               + "   <t:RadDocument.ProtectionSettings>"
                + "    <t:DocumentProtectionSettings EnableDocumentProtection='False' Enforce='False' HashingAlgorithm='None' HashingSpinCount='0' ProtectionMode='ReadOnly' />"
                 + " </t:RadDocument.ProtectionSettings>"
                 + " <t:Section FirstPageNumber='1' PageMargin='95,95,95,95' PageSize='816,1056'>"
                   + " <t:Paragraph TextAlignment='Center'>"
                     + " <t:HyperlinkRangeStart AnnotationID='22625485'>"
                      + "  <t:HyperlinkInfo NavigateUri='http://www.logitudeworld.com' />"
                     + " </t:HyperlinkRangeStart>"
                     + "<t:Span ForeColor='#FF0000FF' Text='POWERED BY LOGITUDE' UnderlineColor='#FF0000FF' UnderlineDecoration='Line' />"
                     + "<t:HyperlinkRangeEnd AnnotationID='22625485' />"
                   + "</t:Paragraph>"
                 + "</t:Section>"
               + "</t:RadDocument>"
             + "</t:Footer.Body>"
           + "</t:Footer>"
         + "</t:Footers.Default>"
       + "</t:Footers>"
     + "</t:Section.Footers>"
     + "<t:Paragraph TextAlignment='Center' />"
   + "</t:Section>"
   + "</t:RadDocument>";
            }
            else
            {
                poweredBy += " <t:Section>"
   + " <t:Paragraph TextAlignment='Center'>"
    + "  <t:HyperlinkRangeStart AnnotationID='1'>"
      + "  <t:HyperlinkInfo NavigateUri='http://www.logitudeworld.com' />"
     + " </t:HyperlinkRangeStart>"
     + " <t:Span FontSize='13.33' ForeColor='#FF0000FF' Text='POWERED BY LOGITUDE' UnderlineColor='#FF0000FF' UnderlineDecoration='Line' />"
      + "<t:HyperlinkRangeEnd AnnotationID='1' />"
   + " </t:Paragraph>"
  + "</t:Section>"
+ "</t:RadDocument>";
            }

             





            XmlReader reader = XmlReader.Create(new StringReader(poweredBy));
            XmlDataDocument signDoc = new XmlDataDocument();
            signDoc.Load(reader);

            XmlNodeList sectionsList = signDoc.GetElementsByTagName("t:Section");
            XmlNode sectionNode = sectionsList.Item(0);


            string xml = sectionNode.OuterXml;

            XmlDataDocument ndoc = new XmlDataDocument();
            ndoc.LoadXml(xml);

            XmlNode xNode = ndoc.DocumentElement;
            return xNode;


            //            string poweredBy = @"<t:Paragraph FontSize='13.3299999237061' SpacingAfter='10' TextAlignment='Center'>"
            //              +"<t:HyperlinkRangeStart AnnotationID='1'>"
            //                +"<t:HyperlinkInfo NavigateUri='http://www.logitudeworld.com/' />"
            //              +"</t:HyperlinkRangeStart>"
            //              +"<t:Span FontFamily='Verdana' FontSize='13.3299999237061' ForeColor='#FF0000FF' Text='POWERED BY LOGITUDE' UnderlineColor='#FF0000FF' UnderlineDecoration='Line' />"
            //             + "<t:HyperlinkRangeEnd AnnotationID='1' />"
            //            + "</t:Paragraph>";
        }

        #region GetTenantLogo

        public string GetTenantLogo(string logoName, int tenant)
        {
            string logoCode = " ";
            string logoFileName = logoName + tenant;
            byte[] logoFile = GetFileFromServer(logoFileName, "jpg", "logos", tenant);
            char[] base64Data;
            if (logoFile != null)
            {
                base64Data = new char[(int)(Math.Ceiling((double)logoFile.Length / 3) * 4)];

                Convert.ToBase64CharArray(logoFile, 0, logoFile.Length, base64Data, 0);
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                string rawData = new String(base64Data);



                MemoryStream ms = new MemoryStream(logoFile, 0, logoFile.Length);          // Convert byte[] to Image    
                ms.Write(logoFile, 0, logoFile.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string width = image.Width.ToString();
                string height = image.Height.ToString();
                //"'" + " UriSource='" + uri + "' " +
                // string uri = GetFileServerPath(logoName + tenant, "jpg", "logos");
                // string m = @"/";
                // UriSource="/Silverlight.Help.RadRichTextBoxSamples;component/Demos/Images/RadRichTextBox.png"
                // uri = @"../"+ uri.Replace(@"\", @"/");
                // string ChildIndex = tenant.ToString();//GetFileServerPath(logoName + tenant, "jpg", "logos");
                string imageNodeString = " ";
                if (isSendMail)
                {
                    imageNodeString = "<t:ImageInline  Extension='" + logoFileName + "' Height='" + height + "'  Width='" + width + "'  RawData = '" + rawData + "'" + " />";

                }
                else
                {
                    imageNodeString = "<t:ImageInline  Extension='" + "jpg" + "' Height='" + height + "'  Width='" + width + "'  RawData = '" + rawData + "'" + " />";
                }
                logoCode = "<t:Section xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents'><t:Paragraph>" + imageNodeString + "</t:Paragraph></t:Section>";

            }



            return logoCode;
        }
        #endregion

        #region ResolveObjectFieldValue


        public string ResolveObjectFieldValue(ObjectField objectField, string[] fields, int tenant, string propertyName, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, Dictionary<XmlNode, XmlNode> tablesDic, XmlNode node, string nodeTextValue, object theEntity, List<ObjectField> theEntityObjectFields)
        {
            string value = " ";
            if (objectField.DataTypeCode == "LookUp" || !string.IsNullOrEmpty(objectField.LookUpTableId))
            {
                string resultValue = GetInsideEntityFieldValue(theEntity, theEntityObjectFields, fields, tenant, generalService);

                node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue);

                nodeTextValue = resultValue;
                value = resultValue;

            }
            else if (objectField.DataTypeCode == "DateTime")
            {
                PropertyInfo propertyPathPi = theEntity.GetType().GetProperty(fields[0].Trim());
                if (propertyPathPi != null)
                {
                    object datetimevalue = propertyPathPi.GetValue(theEntity, null);
                    if (datetimevalue != null)
                    {
                        DateTime? datetime = datetimevalue as DateTime?;
                        if (datetime != null)
                        {
                            string resultValue = " ";
                            if (fields[1].Trim().ToLower() == "date")
                            {
                                resultValue = datetime.Value.ToShortDateString();
                            }
                            else
                            {
                                resultValue = datetime.Value.ToShortTimeString();
                            }

                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue);
                            nodeTextValue = resultValue;
                            value = resultValue;
                        }
                    }
                    else
                    {
                        node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", " ");
                        nodeTextValue = " ";
                        value = " ";
                    }

                }
            }

            else if (objectField.IsMulti)
            {
                object valuesList = null;
                PropertyInfo propertyPathPi = theEntity.GetType().GetProperty(fields[0].Trim());
                if (propertyPathPi == null && !string.IsNullOrEmpty(objectField.PMPropertyPath))
                {
                    propertyPathPi = theEntity.GetType().GetProperty(objectField.PMPropertyPath);
                }
                if (propertyPathPi != null)
                {
                    valuesList = propertyPathPi.GetValue(theEntity, null);
                }
                else if (fields[0].Trim() == "Events")
                {
                    TraceEventQuery eventsQuery = new TraceEventQuery(tenant);
                    valuesList = eventsQuery.GetTraceEventPMsByTenantByEntityId(tenant, entity.GetType().GetProperty("Id").GetValue(entity, null).ToString(), objectField.ObjectTableId).Where(e => e.IsCustomerView).ToList();

                }

                if (valuesList != null)
                {
                    IList collection = (IList)valuesList;
                    XmlNode rowNode = node.ParentNode.ParentNode.ParentNode;
                    XmlNode tableNode = rowNode.ParentNode;
                    ObjectTable multiTable = tablesRepository.GetObjectTableById(objectField.MultiTableId, tenant);
                    List<ObjectField> localObjectFields = GetEntityObjectFields(multiTable.Name, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(objectField.ObjectTable_MultiTable.Name, tenant).ToList();
                    XmlNode newTable = GetNewTable(rowNode, tableNode, collection, localObjectFields, theEntity, theEntityObjectFields, generalService, tenant, systemEntity, systemEntityObjectFields, objectField);
                    if (newTable != null)
                    {
                        if (!tablesDic.Keys.Contains(tableNode))
                        {
                            tablesDic.Add(tableNode, newTable);
                        }
                    }
                    else
                    {
                        if (collection.Count > 0)
                        {
                            object dataObject = collection[0];


                            string originalName = "[" + propertyName + "]";
                            propertyName = propertyName.Replace(objectField.FieldName + ".", "");
                            propertyName = "[" + propertyName + "]";

                            string resultValue = GetNodeValue(node, propertyName, dataObject, localObjectFields, tablesDic, systemEntity, systemEntityObjectFields, null, tenant, generalService, null, null);

                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace(originalName, resultValue);


                            value = resultValue;
                        }
                        else
                        {
                            node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", " ");

                        }
                    }
                }

            }

            return value;
        }

        #endregion

        #region GetNewTable


        public XmlNode GetNewTable(XmlNode oldRow, XmlNode oldTable, IList multidataList, List<ObjectField> multiEntityObjectFields, object theEntity, List<ObjectField> theEntityObjectFields, GeneralDomainService theGeneralService, int tenant, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, ObjectField multiObjectField)
        {
            XmlDocument ndoc = new XmlDocument();
            ndoc.LoadXml(oldTable.OuterXml);

            XmlNodeList tableList = ndoc.GetElementsByTagName("t:Table");
            if (tableList.Count != 0)
            {
                if (tableList[0].ChildNodes.Count > 1)
                {

                    tableList[0].RemoveChild(tableList[0].ChildNodes[tableList[0].ChildNodes.Count - 1]);

                    foreach (object dataObject in multidataList)
                    {
                        XmlNode newRow = GetNewTableRow(oldRow, dataObject, multiEntityObjectFields, theEntity, theEntityObjectFields, systemEntity, systemEntityObjectFields, theGeneralService, tenant, multiObjectField);

                        XmlNode t2Node = ndoc.ImportNode(newRow, true);
                        tableList[0].AppendChild(t2Node);
                    }
                    XmlNode newTable = ndoc.DocumentElement;

                    return newTable;

                }
                else

                    return null;

            }
            else
                return null;

        }

        #endregion

        #region GetNewTableRow


        public XmlNode GetNewTableRow(XmlNode oldRow, object dataObject, List<ObjectField> multiEntityObjectFields, object theEntity, List<ObjectField> theEntityObjectFields, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, GeneralDomainService theGeneralService, int tenant, ObjectField multiObjectField)
        {
            Dictionary<XmlNode, XmlNode> tablesDic = new Dictionary<XmlNode, XmlNode>();
            List<XmlNode> signatureNodeList = new List<XmlNode>();
            List<XmlNode> ticketHeaderNode = new List<XmlNode>();
            List<XmlNode> ticketFooterNode = new List<XmlNode>();
            string theSignature = " ";

            XmlDocument ndoc = new XmlDocument();
            ndoc.LoadXml(oldRow.OuterXml);

            XmlNodeList spansList = ndoc.GetElementsByTagName("t:Span");

            foreach (XmlNode node in spansList)
            {
                if (!String.IsNullOrEmpty(node.Attributes["Text"].Value))
                {
                    string textValue = node.Attributes["Text"].Value;

                    if (textValue.Contains("[") && textValue.Contains("]"))
                    {


                        string[] properties = textValue.Split('[');

                        foreach (string property in properties)
                        {
                            if (property.Contains("]"))
                            {
                                string[] array = property.Split(']');
                                string propertyName = array[0];



                                string[] fields = propertyName.Split('.');

                                if (fields.Count() > 1)
                                {



                                    if (fields[0] == multiObjectField.FieldName)
                                    {
                                        string originalName = "[" + propertyName + "]";
                                        propertyName = propertyName.Replace(multiObjectField.FieldName + ".", "");
                                        propertyName = "[" + propertyName + "]";

                                        string resultValue = GetNodeValue(node, propertyName, dataObject, multiEntityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, theGeneralService, ticketHeaderNode, ticketFooterNode);

                                        node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace(originalName, resultValue);

                                    }
                                    else
                                    {
                                        propertyName = "[" + propertyName + "]";
                                        GetNodeValue(node, propertyName, theEntity, theEntityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, theGeneralService, ticketHeaderNode, ticketFooterNode);
                                    }

                                }
                                else
                                {
                                    string resultValue = GetEntityFieldValue(theEntity, fields[0], theEntityObjectFields, tenant);

                                    node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue);


                                }
                            }

                        }
                    }

                }
            }




            if (signatureNodeList.Count() != 0)
            {
                XmlNode newSignatureSection = GetNewNode(theSignature);



                foreach (XmlNode signatureNode in signatureNodeList)
                {
                    foreach (XmlNode childNode in newSignatureSection.ChildNodes)
                    {
                        XmlNode t2Node = ndoc.ImportNode(childNode, true);

                        signatureNode.ParentNode.ParentNode.InsertBefore(t2Node, signatureNode.ParentNode);
                    }
                }


                foreach (XmlNode signatureNode in signatureNodeList)
                {
                    signatureNode.ParentNode.ParentNode.RemoveChild(signatureNode.ParentNode);
                }


            }

            if (tablesDic.Count() != 0)
            {
                foreach (XmlNode node in tablesDic.Keys)
                {
                    XmlNode t2Node = ndoc.ImportNode(tablesDic[node], true);
                    node.ParentNode.ReplaceChild(t2Node, node);
                }
            }



            XmlNode newRow = ndoc.DocumentElement;

            return newRow;
        }

        #endregion

        #region GetUserSignature

        public string GetUserSignature(SystemDataPM systemData)
        {
            if (systemData.Signature != null && systemData.Signature.Length > 0)
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                string signatureString = encoding.GetString(systemData.Signature);

                XmlReader reader = XmlReader.Create(new StringReader(signatureString));
                XmlDataDocument signDoc = new XmlDataDocument();
                signDoc.Load(reader);

                XmlNodeList sectionsList = signDoc.GetElementsByTagName("t:Section");
                XmlNode sectionNode = sectionsList.Item(0);


                return sectionNode.OuterXml;
            }
            else
            {
                return " ";
            }
        }


        #endregion

        #region Ticket Header
        public string GetTicketHeader(string header)
        {
            if (!string.IsNullOrEmpty(header))
            {
                XmlReader reader = XmlReader.Create(new StringReader(header));
                XmlDataDocument signDoc = new XmlDataDocument();
                signDoc.Load(reader);

                XmlNodeList sectionsList = signDoc.GetElementsByTagName("t:Section");
                XmlNode sectionNode = sectionsList.Item(0);
                return sectionNode.OuterXml;
            }
            else
            {
                return " ";
            }
        }


        #endregion

        #region Ticket Footer
        public string GetTicketFooter(string footer)
        {
            if (!string.IsNullOrEmpty(footer))
            {
                XmlReader reader = XmlReader.Create(new StringReader(footer));
                XmlDataDocument signDoc = new XmlDataDocument();
                signDoc.Load(reader);

                XmlNodeList sectionsList = signDoc.GetElementsByTagName("t:Section");
                XmlNode sectionNode = sectionsList.Item(0);
                return sectionNode.OuterXml;
            }
            else
            {
                return " ";
            }
        }
        #endregion

        #region GetEntityFieldValue

        public string GetEntityFieldValue(object theEntity, string propertyName, List<ObjectField> theEntityObjectFields, int tenant, string propertyNameFullName = " ")
        {
            string resultValue = propertyNameFullName;
            PropertyInfo propertyPathPi = theEntity.GetType().GetProperty(propertyName.Trim());
            if (propertyPathPi != null)
            {
                resultValue = " ";
                object value = propertyPathPi.GetValue(theEntity, null);

                if (value != null)
                {

                    //if (value is DateTime)
                    //{
                    //    DateTime date = (DateTime)value;
                    //    if (propertyName == "ATD" || propertyName == "ATA" || propertyName == "ETD" || propertyName == "ETA")
                    //    {
                    //        value = date.ToString();
                    //    }
                    //    else
                    //    {
                    //        value = date.ToShortDateString();
                    //    }

                    //}

                    resultValue = (value != null ? value.ToString() : " ");

                    ObjectField field = theEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();

                    if (field != null && field.IsCustom)
                    {
                        field = theEntityObjectFields.Where(f => f.FieldName == propertyName && f.Tenant == tenant).FirstOrDefault();
                    }


                    if (field != null)
                    {
                        if (field.IsCustom)
                        {
                            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                            object newValue = customFieldResolver.GetFieldValue(theEntity, field, tenant);


                            resultValue = (newValue != null ? newValue.ToString() : " ");
                        }

                        resultValue = ResolveFieldValue(resultValue, field);
                    }

                }
                else resultValue = " ";
      

                if (!string.IsNullOrEmpty(resultValue) && !CheckIfFieldHaveValueHtml(propertyName) && ReplaceHtmlStringWithTageHtml)
                {
                    resultValue = resultValue.Replace('\n', '\r');
                    resultValue = resultValue.Replace("\r", "<br/>");
                    // resultValue = resultValue.Replace(" ", "&nbsp;");
                }


                if (resultValue == "") resultValue = " ";


                if ((propertyName == "TicketHeader" || propertyName == "TicketFooter") && ReplaceHtmlStringWithTageHtml)
                {
                    if (!string.IsNullOrEmpty(resultValue))
                    {
                        byte[] result = ConvertXmlByteToHtmlByte(System.Text.Encoding.UTF8.GetBytes(resultValue));
                        if (result != null)
                        {
                            resultValue = System.Text.Encoding.UTF8.GetString(result);
                        }

                    }

                }



            }





            return resultValue;
        }

        private string FormatNumber(string value, ObjectField field)
        {

            string result = string.Empty;
            //  bool isEndZero = false;
            //if (value.Contains("."))
            //{
            //    string[] digits = value.Split('.');
            //    var trimmedStr = digits[1].Trim('0');
            //    if (string.IsNullOrEmpty(trimmedStr)) isEndZero = true;

            //}

            result = ShowDigitsAfterPoint(value, field);

            //if ((!value.Contains(".") || isEndZero) && result.Contains("00"))
            //{
            //    result = result.Split('.')[0];
            //}
            //else if (!isEndZero && value.Contains(".") && result.Contains("00"))
            //{
            //    result = ShowDigitsAfterPoint(value, field,3);
            //}


            return result;
        }

        private static string ShowDigitsAfterPoint(string value, ObjectField field, int digitsAfterPoint = 2)
        {
            string result = string.Empty;
            if (field.DataTypeCode.ToLower() == "double")
            {
                double db = 0;
                double.TryParse(value, out db);
                if (digitsAfterPoint == 3)
                {
                    result = db.ToString("#,##0." + new string('0', 3));
                }
                else result = db.ToString("N");
            }

            if (field.DataTypeCode.ToLower() == "decimal")
            {
                decimal db = 0;
                decimal.TryParse(value, out db);
                if (digitsAfterPoint == 3)
                {
                    result = db.ToString("#,##0." + new string('0', 3));
                }
                else result = db.ToString("N");
            }

            return result;
        }

        #endregion

        private bool CheckIfFieldHaveValueHtml(string fieldName)
        {
            bool result = false; 
            if (!string.IsNullOrEmpty(fieldName))
            {
                fieldName = fieldName.ToLower();
                if (fieldName == "ticketheader" || fieldName == "ticketfooter" || fieldName == "iosapplink" || fieldName == "androidapplink" || fieldName == "resetpasswordurl" || fieldName == "systemurl" || fieldName == "InviteeName")
                {
                    result = true;
                }
            }

            return result;
        }




        #region GetInsideEntityFieldValue

        public string GetInsideEntityFieldValue(object theEntity, List<ObjectField> theEntityObjectFields, string[] fields, int tenant, GeneralDomainService theGeneralService, string propertyNameFullName = " ")
        {

            int i = 0;
            string resultValue = propertyNameFullName;
            object currentEntity = theEntity;
            List<ObjectField> currentEntityObjectFields = theEntityObjectFields;

            while (true)
            {
                PropertyInfo propertyPathPi = currentEntity.GetType().GetProperty(fields[i].Trim());
                if (propertyPathPi != null)
                {

                    object value = propertyPathPi.GetValue(currentEntity, null);

                    if (value == null || i >= fields.Count())
                    {
                        if (value == null) resultValue = " ";
                        break;
                    }

                    ObjectField objectField = currentEntityObjectFields.Where(f => f.FieldName == fields[i]).FirstOrDefault();


                    if (objectField != null && !string.IsNullOrEmpty(objectField.LookUpTableId))
                    {
                        ObjectTable lookTable = tablesRepository.GetObjectTableById(objectField.LookUpTableId, tenant);
                        string insideEntityName = lookTable.Name;
                        if (insideEntityName == "Carrier")
                        {
                            insideEntityName = "Card";
                        }
                        // ==================================================================================
                        List<ObjectField> insideEntityObjectFields = GetEntityObjectFields(insideEntityName, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(insideEntityName, tenant).ToList();

                        if ((i + 1) >= fields.Count())
                        {
                            break;
                        }

                        ObjectField insideObjectField = insideEntityObjectFields.Where(f => f.FieldName == fields[i + 1]).FirstOrDefault();

                        if (insideObjectField != null && insideObjectField.IsCustom)
                        {
                            insideObjectField = insideEntityObjectFields.Where(f => f.FieldName == fields[i + 1] && f.Tenant == tenant).FirstOrDefault();
                        }



                        Assembly blAssembly = Assembly.Load("Logitude.BL");

                        string insideTypePath = "Logitude.BL.ShipmentsModel.EntityQueries." + insideEntityName + "Query";

                        Type insideEntityType = blAssembly.GetType(insideTypePath);
                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.CommonDataModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {

                            insideTypePath = "Logitude.BL.InfrastructureModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {

                            insideTypePath = "Logitude.BL.QuoteModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {

                            Assembly assembly = Assembly.Load("Logitude.CRM.BL");
                            insideTypePath = "Logitude.CRM.BL.EntityQueryServices." + insideEntityName + "QueryService";
                            insideEntityType = assembly.GetType(insideTypePath);

                        }

                        if (insideEntityType == null)
                        {

                            Assembly assembly = Assembly.Load("Logitude.Customs.BL");
                            insideTypePath = "Logitude.Customs.BL.EntityQueryServices." + insideEntityName + "QueryService";
                            insideEntityType = assembly.GetType(insideTypePath);

                        }

                        if (insideEntityType == null)
                        {

                            Assembly assembly = Assembly.Load("Logitude.BookingLib.BL");
                            insideTypePath = "Logitude.BookingLib.BL.EntityQueryServices." + insideEntityName + "QueryService";
                            insideEntityType = assembly.GetType(insideTypePath);

                        }

                        if (insideEntityType == null)
                        {

                            Assembly assembly = Assembly.Load("Logitude.WarehouseLib.BL");
                            insideTypePath = "Logitude.WarehouseLib.BL.EntityQueryServices." + insideEntityName + "QueryService";
                            insideEntityType = assembly.GetType(insideTypePath);

                        }




                        object insideEntityRepository = null;
                        if (insideEntityType != null)
                        {
                            insideEntityRepository = Activator.CreateInstance(insideEntityType, new object[] { tenant });

                            //MethodInfo insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSinglePM");
                            MethodInfo[] MethodInfoList = insideEntityRepository.GetType().GetMethods();
                            MethodInfo insideMethodInfo = MethodInfoList.Where(d => d.Name == "GetSinglePM").FirstOrDefault();

                            object insideEntity = null;
                            if (insideMethodInfo != null)
                            {

                                if (value != null && value.GetType() == typeof(CustomFieldClass))
                                {
                                    CustomFieldClass c = value as CustomFieldClass;
                                    value = c.Value;
                                }


                                ParameterInfo[] parametersInfo = insideMethodInfo.GetParameters();
                                object[] parameters = new object[] { };
                                switch (parametersInfo.Count())
                                {
                                    case 1:
                                        parameters = new object[] { value };
                                        break;
                                    case 2:
                                        parameters = new object[] { value, tenant };
                                        break;
                                    case 3:
                                        parameters = new object[] { value, tenant, false };
                                        break;
                                    default:
                                        parameters = new object[] { value, tenant };
                                        break;
                                }


                                //InsideEntity = insideMethodInfo.Invoke(InsideEntityRepository, parameters);

                                insideEntity = insideMethodInfo.Invoke(insideEntityRepository, parameters);
                                if (insideEntity == null && insideEntityName == "User")
                                {
                                    parameters = new object[] { value, 0 };
                                    insideEntity = insideMethodInfo.Invoke(insideEntityRepository, parameters);
                                    if (insideEntity != null)
                                    {
                                        MethodInfo userMethodInfo = insideEntityRepository.GetType().GetMethod("GetSingleUserPMByEmail");
                                        parameters = new object[] { "system@tenant" + tenant + ".com", tenant, false };
                                        insideEntity = userMethodInfo.Invoke(insideEntityRepository, parameters);
                                    }
                                }

                                if (insideEntity != null)
                                {
                                    if ((i + 1) < fields.Count())
                                    {
                                        PropertyInfo insidePropertyPathPi = insideEntity.GetType().GetProperty(fields[i + 1].Trim());
                                        if (insidePropertyPathPi != null)
                                        {
                                            object insideValue = insidePropertyPathPi.GetValue(insideEntity, null);
                                            if (insideValue != null)
                                            {
                                                if (insideValue.GetType() == typeof(CustomFieldClass))
                                                {
                                                    if (insideObjectField.IsCustom)
                                                    {
                                                        CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                                                        object newValue = customFieldResolver.GetFieldValue(insideEntity, insideObjectField, tenant);
                                                        resultValue = (newValue != null ? newValue.ToString() : " ");
                                                    }
                                                }

                                                else if (insideValue is DateTime)
                                                {
                                                    DateTime date = (DateTime)insideValue;
                                                    insideValue = date.ToShortDateString();
                                                    resultValue = (insideValue != null ? insideValue.ToString() : " ");
                                                }

                                                else resultValue = insideValue.ToString();
                                            }
                                            else resultValue = string.Empty;


                                            resultValue = ResolveFieldValue(resultValue, insideObjectField);
                                        }
                                        else
                                        {
                                            resultValue = propertyNameFullName;
                                            break;
                                        }
                                    }
                                    else
                                    {

                                        break;
                                    }

                                    if (insideObjectField != null)
                                    {
                                        if (insideObjectField.DataTypeCode == "LookUp" || !string.IsNullOrEmpty(insideObjectField.LookUpTableId))
                                        {
                                            currentEntity = insideEntity;
                                            currentEntityObjectFields = insideEntityObjectFields;
                                            i++;
                                        }
                                        else
                                        {
                                            // check if its a multi value
                                            // ResolveObjectFieldValue
                                            break;
                                        }
                                    }
                                    else
                                    {

                                        break;
                                    }
                                }
                                else
                                {
                                    break;

                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {

                            break;
                        }

                    }

                    else if (objectField.DataTypeCode == "DateTime")
                    {
                        if (value != null && fields.Count() == 2)
                        {
                            DateTime? datetime = (DateTime?)DateTime.Parse(value.ToString());
                            if (datetime != null)
                            {
                                if (fields[1].Trim().ToLower() == "date")
                                {
                                    resultValue = datetime.Value.ToShortDateString();
                                }
                                else
                                {
                                    resultValue = datetime.Value.ToShortTimeString();
                                }
                            }


                        }
                        break;
                    }
                    else
                    {

                        break;
                    }

                }
                else
                {
                    break;
                }
            } 
             
            if (!string.IsNullOrEmpty(resultValue) && !resultValue.Contains("Telerik.Windows.Documents") && ReplaceHtmlStringWithTageHtml)
            {
                resultValue = resultValue.Replace('\n', '\r');
                resultValue = resultValue.Replace("\r", "<br/>");
                // resultValue = resultValue.Replace(" ", "&nbsp;");
            }
             
            if (resultValue == "") resultValue = " ";
            return resultValue;
        }

        private string ResolveFieldValue(string fieldValue, ObjectField field)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(fieldValue))
            {
                result = fieldValue.Replace(" ","");
                if (!string.IsNullOrEmpty(result)) result = fieldValue;
                else if (field != null && field.DataTypeCode.ToLower() == "boolean") result = "false";
            }
             
            if (field != null)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    if ((field.DataTypeCode.ToLower() == "double" || field.DataTypeCode.ToLower() == "decimal"))
                    {
                        result = FormatNumber(result, field);
                    }
                    else if (field.DataTypeCode.ToLower() == "boolean") result = result.ToLower() == "false" ? "No" : "Yes";
                }
            }
           


            return result;
        }


        #endregion

        #region GetFileFromServer

        public string GetFileServerPath(string fileName, string fileExtension, string folderName)
        {
            string uriSource = "";
            //if (!WebFreightEntryPoint.UsingAzure)
            //{
            //    UriSource = Server.MapPath(".");

            //    UriSource += "\\" + folderName + "\\";
            //    UriSource += fileName;
            //    UriSource += "." + fileExtension;
            //}
            //else
            //{
            folderName = folderName.ToLower();
            uriSource = fileName + "." + fileExtension;
            //  }

            return uriSource;
        }

        public byte[] GetFileFromServer(string fileName, string fileExtension, string folderName, int tenant)
        {
            byte[] resultFile = null;

            if (!fileName.Contains("sharedLogtsitcslogo"))
            {
                if (folderName.ToLower() == "logos")
                {
                    fileName = fileName.ToLower();
                }
            }

            //Check if InAzure 
            // if (!WebFreightEntryPoint.UsingAzure)
            //{

            //    string path = Server.MapPath(".");
            //    path += "\\"+folderName+"\\";
            //    path += fileName;
            //    path += "." + fileExtension;

            //    FileStream fs = File.OpenRead(path);
            //    resultFile = new byte[fs.Length];
            //    fs.Read(resultFile, 0, resultFile.Length);
            //    fs.Close();
            //}

            //else // In Azure = true
            //{
            //folderName = folderName.ToLower();
            //fileName = fileName + "." + fileExtension;
            //////////////////CloudBlobContainer blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
            //////////////////var blobfile = blobContainer.GetBlockBlobReference(StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), folderName));

            //////////////////if (blobfile.Exists())
            //////////////////{
            //////////////////    using (MemoryStream memstream = new MemoryStream())
            //////////////////    {

            //////////////////        blobfile.DownloadToStream(memstream);
            //////////////////        resultFile = memstream.ToArray();

            //////////////////    }
            //////////////////}


            //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), folderName);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = fileName,
                FolderName = folderName,
                Extension = fileExtension,
                Tenant = tenant,


            };
            resultFile = storageservice.Read(fileInfo);


            // }

            return resultFile;
        }

        #endregion



        #region HtmlNode


        private HtmlNode GetNewSignatureNodeHtml(string signature, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, int tenant, GeneralDomainService theGeneralService)
        {


            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(signature);

            ResolveSignatureFieldsHtml(htmlDocument, systemEntity, systemEntityObjectFields, tenant, theGeneralService);


            if (signatureLogosListHtml.Count() != 0)
            {

                foreach (LogoClassHtml logo in signatureLogosListHtml)
                {
                    if (!String.IsNullOrEmpty(logo.LogoString.Trim()))
                    {
                        HtmlNode newLogoSection = GetNewNodeHtml(logo.LogoString);

                        foreach (HtmlNode childNode in newLogoSection.ChildNodes)
                        {


                            //  HtmlNode t2Node = htmlDocument.DocumentNode.AppendChild(childNode);

                            logo.LogoNodeHtml.ParentNode.ParentNode.InsertBefore(childNode, logo.LogoNodeHtml.ParentNode);
                        }



                    }


                }


                foreach (LogoClassHtml logo in signatureLogosListHtml)
                {
                    if (logo.LogoNodeHtml.ParentNode.ParentNode != null)
                    {

                        logo.LogoNodeHtml.ParentNode.ParentNode.RemoveChild(logo.LogoNodeHtml.ParentNode);
                    }

                }
            }

            HtmlNode htmlNode = htmlDocument.DocumentNode;
            return htmlNode;
        }



        #region ResolveSignatureFieldsHtml

        List<LogoClassHtml> signatureLogosListHtml = new List<LogoClassHtml>();
        public void ResolveSignatureFieldsHtml(HtmlDocument signatureDoc, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, int tenant, GeneralDomainService theGeneralService)
        {


            HtmlNodeCollection spansList = signatureDoc.DocumentNode.SelectNodes("//span");

            if (spansList != null)
            {
                foreach (HtmlNode node in spansList)
                {
                    if (!String.IsNullOrEmpty(node.InnerHtml))
                    {
                        string textValue = node.InnerHtml;
                        if (textValue.Contains("[") && textValue.Contains("]"))
                        {
                            string[] properties = textValue.Split('[');

                            foreach (string property in properties)
                            {
                                if (property.Contains("]"))
                                {
                                    string[] array = property.Split(']');
                                    string propertyName = array[0];
                                    if (propertyName.Contains("."))
                                    {
                                        string[] fields = propertyName.Split('.');
                                        //ObjectField objectField = null;
                                        if (fields[0] == "SystemData")
                                        {

                                            propertyName = propertyName.Replace("SystemData.", "");
                                            if (propertyName.Contains("."))
                                            {
                                                string[] systemFields = propertyName.Split('.');

                                                string resultValue = GetInsideEntityFieldValue(systemEntity, systemEntityObjectFields, systemFields, tenant, theGeneralService);


                                                node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", resultValue);

                                            }
                                            else
                                            {
                                                if (propertyName != "Signature" && propertyName != "Logo" && propertyName != "SmallLogo" && propertyName != "WideLogo")
                                                {


                                                    string resultValue = GetEntityFieldValue(systemEntity, propertyName, systemEntityObjectFields, tenant);

                                                    node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", resultValue);

                                                }
                                                else
                                                {
                                                    if (propertyName == "Logo" || propertyName == "SmallLogo" || propertyName == "WideLogo")
                                                    {
                                                        string logoString = GetTenantLogoHtml(propertyName, tenant);

                                                        if (!String.IsNullOrEmpty(logoString.Trim()))
                                                        {
                                                            LogoClassHtml logo = new LogoClassHtml(node, logoString);
                                                            signatureLogosListHtml.Add(logo);

                                                            //logoNodeList.Add(node);
                                                        }
                                                        else
                                                        {

                                                            node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", " ");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // signature = GetUserSignature(systemEntity);
                                                        // if (!String.IsNullOrEmpty(signature.Trim()))
                                                        // {
                                                        //signatureNodeList.Add(node);
                                                        // }
                                                        // else
                                                        // {

                                                        node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", " ");
                                                        // }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //GetNodeValue(node, textValue, entity, entityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, generalService);
                        }

                    }
                }



            }


        }

        #endregion


        private string GeSystemDataNodeValue(HtmlNode node, string nodeText, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, List<HtmlNode> signatureNodeList, int tenant, GeneralDomainService theGeneralService)
        {
            string nodeTextValue = " ";
            string[] properties = nodeText.Split('[');
            foreach (string property in properties)
            {
                if (property.Contains("]"))
                {
                    string[] array = property.Split(']');
                    string propertyName = array[0];
                    if (propertyName.Contains("."))
                    {
                        string[] fields = propertyName.Split('.');
                        ObjectField objectField = null;

                        propertyName = propertyName.Replace("SystemData.", "");
                        if (propertyName.Contains("."))
                        {
                            string[] systemFields = propertyName.Split('.');

                            string resultValue = GetInsideEntityFieldValue(systemEntity, systemEntityObjectFields, systemFields, tenant, theGeneralService);

                            node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", resultValue);

                            nodeTextValue = resultValue;
                        }
                        else
                        {
                            if (propertyName != "Signature" && propertyName != "Logo" && propertyName != "SmallLogo")
                            {


                                string resultValue = GetEntityFieldValue(systemEntity, propertyName, systemEntityObjectFields, tenant);

                                node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", resultValue);
                                nodeTextValue = resultValue;

                            }
                            else
                            {
                                if (propertyName == "Logo" || propertyName == "SmallLogo")
                                {
                                    string logoString = GetTenantLogoHtml(propertyName, tenant);

                                    if (!String.IsNullOrEmpty(logoString.Trim()))
                                    {
                                        LogoClassHtml logo = new LogoClassHtml(node, logoString);
                                        logosListHtml.Add(logo);

                                        //logoNodeList.Add(node);
                                    }
                                    else
                                    {


                                        node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", " ");

                                    }
                                }
                                else
                                {
                                    signature = GetUserSignatureHtml(systemEntity);
                                    if (!String.IsNullOrEmpty(signature.Trim()))
                                    {
                                        signatureNodeList.Add(node);
                                    }
                                    else
                                    {
                                        node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", " ");

                                    }
                                }
                            }
                        }


                    }

                }
            }



            return nodeTextValue;
        }




        public string GetUserSignatureHtml(SystemDataPM systemData)
        {
            if (systemData.SignatureHtml != null && systemData.SignatureHtml.Length > 0)
            {
                string html = System.Text.Encoding.UTF8.GetString(systemData.SignatureHtml);

                html = html.Replace("[", "<span>[");
                html = html.Replace("]", "]</span>");
                html = html.Replace("<p>", "<p><span>");
                html = html.Replace("</p>", "</span></p>");
                return html;

            }
            else
            {
                return " ";
            }
        }

        private string GetHtmlNodeValue(HtmlNode node, string nodeText, object theEntity, List<ObjectField> theEntityObjectFields, Dictionary<HtmlNode, HtmlNode> tablesDic, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, List<HtmlNode> signatureNodeList, int tenant, GeneralDomainService theGeneralService, List<HtmlNode> ticketHeaderNode = null, List<HtmlNode> ticketFooterNode = null)
        {

            string nodeTextValue = nodeText;
            string[] properties = nodeText.Split('[');

            foreach (string property in properties)
            {
                if (property.Contains("]"))
                {
                    string[] array = property.Split(']');
                    string propertyName = array[0];
                    if (propertyName.Contains("."))
                    {
                        string[] fields = propertyName.Split('.');
                        ObjectField objectField = null;
                        if (fields[0] != "SystemData")
                        {

                            if (childEntity != null)
                            {
                                objectField = childEntityObjectFields.Where(f => f.FieldName == fields[0]).FirstOrDefault();
                                if (objectField != null)
                                {

                                    nodeTextValue = ResolveObjectFieldValueHtml(objectField, fields, tenant, propertyName, systemEntity, systemEntityObjectFields, tablesDic, node, nodeTextValue, childEntity, childEntityObjectFields, nodeText);
                                }
                                else
                                {
                                    if (theEntityObjectFields != null)
                                    {
                                        objectField = theEntityObjectFields.Where(f => f.FieldName == fields[0]).FirstOrDefault();

                                        if (objectField != null)
                                        {

                                            nodeTextValue = ResolveObjectFieldValueHtml(objectField, fields, tenant, propertyName, systemEntity, systemEntityObjectFields, tablesDic, node, nodeTextValue, theEntity, theEntityObjectFields, nodeText);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (theEntityObjectFields != null)
                                {
                                    objectField = theEntityObjectFields.Where(f => f.FieldName == fields[0]).FirstOrDefault();

                                    if (objectField != null)
                                    {

                                        nodeTextValue = ResolveObjectFieldValueHtml(objectField, fields, tenant, propertyName, systemEntity, systemEntityObjectFields, tablesDic, node, nodeTextValue, theEntity, theEntityObjectFields, nodeText);
                                    }
                                }
                            }

                        }
                        else
                        {
                            propertyName = propertyName.Replace("SystemData.", "");
                            if (propertyName.Contains("."))
                            {
                                string[] systemFields = propertyName.Split('.');

                                string resultValue = GetInsideEntityFieldValue(systemEntity, systemEntityObjectFields, systemFields, tenant, theGeneralService, nodeText);

                                node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", resultValue);

                                nodeTextValue = resultValue;
                            }
                            else
                            {
                                if (propertyName != "Signature" && propertyName != "Logo" && propertyName != "SmallLogo" && propertyName != "WideLogo")
                                {

                                    string resultValue = GetEntityFieldValue(systemEntity, propertyName, systemEntityObjectFields, tenant, nodeText);

                                    node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", resultValue);
                                    nodeTextValue = resultValue;

                                }
                                else
                                {
                                    if (propertyName == "Logo" || propertyName == "SmallLogo" || propertyName == "WideLogo")
                                    {
                                        string logoString = GetTenantLogoHtml(propertyName, tenant);

                                        if (!String.IsNullOrEmpty(logoString.Trim()))
                                        {
                                            LogoClassHtml logo = new LogoClassHtml(node, logoString);
                                            logosListHtml.Add(logo);

                                            //logoNodeList.Add(node);
                                        }
                                        else
                                        {


                                            node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", " ");

                                        }
                                    }
                                    else
                                    {
                                        signature = GetUserSignatureHtml(systemEntity);
                                        if (!String.IsNullOrEmpty(signature.Trim()))
                                        {
                                            signatureNodeList.Add(node);
                                        }
                                        else
                                        {
                                            node.InnerHtml = node.InnerHtml.Replace("[SystemData." + propertyName + "]", " ");

                                        }
                                    }
                                }
                            }

                        }


                    }

                    else
                    {
                        ObjectField objectField = null;
                        if (childEntity != null)
                        {
                            objectField = childEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();

                            if (objectField != null && childEntity != null)
                            {
                                string resultValue = GetEntityFieldValue(childEntity, propertyName, childEntityObjectFields, tenant);
                                node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);
                                nodeTextValue = resultValue;
                            }
                            else
                            {
                                if (theEntityObjectFields != null)
                                {
                                    objectField = theEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();
                                }
                                if (objectField != null)
                                {

                                    string resultValue = GetEntityFieldValue(theEntity, propertyName, childEntityObjectFields, tenant);


                                    node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);


                                    nodeTextValue = resultValue;
                                }
                            }
                        }
                        else
                        {

                            if (theEntityObjectFields != null)
                            {
                                objectField = theEntityObjectFields.Where(f => f.FieldName == propertyName).FirstOrDefault();

                            }
                            if (objectField != null)
                            {

                                string resultValue = GetEntityFieldValue(theEntity, propertyName, theEntityObjectFields, tenant);

                                if ((propertyName == "SecurityKey" || (propertyName == "ShipmentNumber" && ObjectTableName == "Shipment")) && CurrentTenant != null && CurrentTenant.SharedLogisticsMessageLink)
                                {
                                    if (propertyName == "SecurityKey")
                                    {
                                        this.securityKey = resultValue;
                                        shipmentNumbersNodesHtml.Add(node);
                                        node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", "");

                                    }
                                    else
                                    {
                                        this.securityKey = GetEntityFieldValue(theEntity, "SecurityKey", theEntityObjectFields, tenant);
                                        // node.Attributes["Text"].Value = node.Attributes["Text"].Value.Replace("[" + propertyName + "]", resultValue + " ");

                                        node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue + " ");



                                        //this.securityKeyNode = node;
                                        shipmentNumbersNodesHtml.Add(node);
                                    }

                                }
                                else if (propertyName == "TicketHeader" || propertyName == "TicketFooter")
                                {
                                    if (propertyName == "TicketHeader")
                                    {
                                        ticketHeaderHtml = resultValue;

                                        if (!String.IsNullOrEmpty(resultValue) && !String.IsNullOrEmpty(resultValue.Trim()))
                                        {
                                            if (ticketHeaderNode != null) ticketHeaderNode.Add(node);

                                        }
                                        else
                                        {
                                            node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", "");
                                        }
                                    }
                                    else
                                    {
                                        ticketFooterHtml = resultValue;
                                        if (!String.IsNullOrEmpty(resultValue) && !String.IsNullOrEmpty(resultValue.Trim()))
                                        {
                                            if (ticketFooterNode != null) ticketFooterNode.Add(node);

                                        }
                                        else
                                        {
                                            node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", " ");

                                        }
                                    }
                                }
                                else if (propertyName == "OwnerLink" && ObjectTableName == "Shipment")
                                {
                                    var shipmentNumber = GetEntityFieldValue(theEntity, "ShipmentNumber", theEntityObjectFields, tenant);
                                    string href = LogitudeSettings.LogitudeURL + "?Menu=LogBox&Tenant=" + tenant + "&Parmters=%7b%22SearchField%22%3a%22" + shipmentNumber + "%22%7d";
                                    resultValue = "<a style=" + "'font-family:Arial;font-size:18px;color:#0000FF'" + " href='" + href + "'" + ">Link</a>";

                                    node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);
                                    nodeTextValue = resultValue;
                                }
                                else
                                {
                                    node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);
                                    nodeTextValue = resultValue;
                                }
                            }
                        }


                    }

                }


            }

            return nodeTextValue;



        }



        private string ResolveObjectFieldValueHtml(ObjectField objectField, string[] fields, int tenant, string propertyName, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, Dictionary<HtmlNode, HtmlNode> tablesDic, HtmlNode node, string nodeTextValue, object theEntity, List<ObjectField> theEntityObjectFields, string propertyNameFullName = " ")
        {
            string value = propertyNameFullName;

            if (objectField.DataTypeCode == "LookUp" || !string.IsNullOrEmpty(objectField.LookUpTableId))
            {
                string resultValue = GetInsideEntityFieldValue(theEntity, theEntityObjectFields, fields, tenant, generalService, propertyNameFullName);
                node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);
                nodeTextValue = resultValue;
                value = resultValue;

            }
            else if (objectField.DataTypeCode == "DateTime")
            {
                PropertyInfo propertyPathPi = theEntity.GetType().GetProperty(fields[0].Trim());
                if (propertyPathPi != null)
                {
                    value = " ";
                    object datetimevalue = propertyPathPi.GetValue(theEntity, null);
                    if (datetimevalue != null)
                    {
                        DateTime? datetime = datetimevalue as DateTime?;
                        if (datetime != null)
                        {
                            string resultValue = " ";
                            if (fields[1].Trim().ToLower() == "date")
                            {
                                resultValue = datetime.Value.ToShortDateString();
                            }
                            else
                            {
                                resultValue = datetime.Value.ToShortTimeString();
                            }

                            node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);
                            nodeTextValue = resultValue;
                            value = resultValue;
                        }
                    }
                    else
                    {
                        node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", " ");
                        nodeTextValue = " ";
                        value = " ";
                    }

                }
            }

            else if (objectField.IsMulti)
            {
                object valuesList = null;
                PropertyInfo propertyPathPi = theEntity.GetType().GetProperty(fields[0].Trim());
                if (propertyPathPi == null && !string.IsNullOrEmpty(objectField.PMPropertyPath))
                {
                    propertyPathPi = theEntity.GetType().GetProperty(objectField.PMPropertyPath);
                }
                if (propertyPathPi != null)
                {
                    valuesList = propertyPathPi.GetValue(theEntity, null);
                }
                else if (fields[0].Trim() == "Events")
                {
                    TraceEventQuery eventsQuery = new TraceEventQuery(tenant);
                    valuesList = eventsQuery.GetTraceEventPMsByTenantByEntityId(tenant, entity.GetType().GetProperty("Id").GetValue(entity, null).ToString(), objectField.ObjectTableId).Where(e => e.IsCustomerView).ToList();

                }

                if (valuesList != null)
                {
                    IList collection = (IList)valuesList;
                    HtmlNode newTable = null;
                    HtmlNode tableNode = null;
                    List<ObjectField> localObjectFields = null;
                    if (node != null)
                    {
                        HtmlNode tdNode = null;
                        if (node.Name == "td")
                        {
                            tdNode = node;
                        }
                        else
                        {
                            var parnetNode = node.ParentNode;
                            while (parnetNode != null)
                            {
                                if (parnetNode.Name == "td")
                                {
                                    tdNode = parnetNode;
                                    break;
                                }

                                parnetNode = parnetNode.ParentNode;
                            }
                        }
                        if (tdNode != null)
                        {
                            HtmlNode rowNode = tdNode.ParentNode;
                            if (rowNode != null && rowNode.ParentNode != null)
                            {
                                tableNode = rowNode.ParentNode;
                                ObjectTable multiTable = tablesRepository.GetObjectTableById(objectField.MultiTableId, tenant);
                                localObjectFields = GetEntityObjectFields(multiTable.Name, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(objectField.ObjectTable_MultiTable.Name, tenant).ToList();
                                newTable = GetNewTableHtml(rowNode, tableNode, collection, localObjectFields, theEntity, theEntityObjectFields, generalService, tenant, systemEntity, systemEntityObjectFields, objectField);
                            }
                        }
                    }
                    //if (node != null && node.ParentNode != null && node.ParentNode.ParentNode!=null && node.ParentNode.ParentNode.ParentNode != null)
                    //{
                    //    HtmlNode rowNode = node.ParentNode.ParentNode.ParentNode;
                    //    if (rowNode != null && rowNode.ParentNode != null)
                    //    {
                    //        tableNode = rowNode.ParentNode;
                    //        ObjectTable multiTable = tablesRepository.GetObjectTableById(objectField.MultiTableId, tenant);
                    //        localObjectFields = GetEntityObjectFields(multiTable.Name, tenant);//generalService.ObjectFieldsRepository.GetObjectFieldsByObjectTableName(objectField.ObjectTable_MultiTable.Name, tenant).ToList();
                    //        newTable = GetNewTableHtml(rowNode, tableNode, collection, localObjectFields, theEntity, theEntityObjectFields, generalService, tenant, systemEntity, systemEntityObjectFields, objectField);
                    //    }
                    //}

                    if (newTable != null)
                    {
                        if (!tablesDic.Keys.Contains(tableNode))
                        {
                            tablesDic.Add(tableNode, newTable);
                        }
                    }
                    else
                    {
                        if (collection.Count > 0)
                        {
                            object dataObject = collection[0];


                            string originalName = "[" + propertyName + "]";
                            propertyName = propertyName.Replace(objectField.FieldName + ".", "");
                            propertyName = "[" + propertyName + "]";

                            string resultValue = GetHtmlNodeValue(node, propertyName, dataObject, localObjectFields, tablesDic, systemEntity, systemEntityObjectFields, null, tenant, generalService);


                            node.InnerHtml = node.InnerHtml.Replace(originalName, resultValue);

                            value = resultValue;
                        }
                        else
                        {

                            node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", " ");
                        }
                    }
                }

            }

            return value;
        }




        private HtmlNode GetNewTableHtml(HtmlNode oldRow, HtmlNode oldTable, IList multidataList, List<ObjectField> multiEntityObjectFields, object theEntity, List<ObjectField> theEntityObjectFields, GeneralDomainService theGeneralService, int tenant, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, ObjectField multiObjectField)
        {
            HtmlDocument ndoc = new HtmlDocument();
            ndoc.LoadHtml(oldTable.OuterHtml);



            HtmlNodeCollection tableList = ndoc.DocumentNode.SelectNodes("//table");

            if (tableList != null)
            {
                if (tableList.Count != 0)
                {

                    int x = tableList[0].ParentNode.ChildNodes.Count;
                    //if (tableList[0].ChildNodes.Count > 1)
                    //{

                    //    tableList[0].RemoveChild(tableList[0].ChildNodes[tableList[0].ChildNodes.Count - 1]);

                    //    foreach (object dataObject in multidataList)
                    //    {
                    //        HtmlNode newRow = GetNewTableRowHtml(oldRow, dataObject, multiEntityObjectFields, theEntity, theEntityObjectFields, systemEntity, systemEntityObjectFields, theGeneralService, tenant, multiObjectField);

                    //        HtmlNode t2Node = ndoc.LoadHtml(newRow.InnerHtml);
                    //        //t2Node.ChildNodes.Add(newRow);
                    //        // HtmlNode t2Node = ndoc.DocumentNode.AppendChild(newRow);
                    //        tableList[0].AppendChild(t2Node);
                    //    }
                    //    HtmlNode newTable = ndoc.DocumentNode;

                    //    return newTable;

                    //}
                    if (tableList[0].ChildNodes.Count > 1)
                    {

                        tableList[0].RemoveChild(tableList[0].ChildNodes[tableList[0].ChildNodes.Count - 1]);

                        foreach (object dataObject in multidataList)
                        {
                            HtmlNode newRow = GetNewTableRowHtml(oldRow, dataObject, multiEntityObjectFields, theEntity, theEntityObjectFields, systemEntity, systemEntityObjectFields, theGeneralService, tenant, multiObjectField);


                            tableList[0].AppendChild(newRow);
                        }



                        HtmlNode newTable = tableList[0];

                        return newTable;

                    }

                    else

                        return null;

                }
                else return null;
            }
            else return null;
        }


        public HtmlNode GetNewTableRowHtml(HtmlNode oldRow, object dataObject, List<ObjectField> multiEntityObjectFields, object theEntity, List<ObjectField> theEntityObjectFields, SystemDataPM systemEntity, List<ObjectField> systemEntityObjectFields, GeneralDomainService theGeneralService, int tenant, ObjectField multiObjectField)
        {
            Dictionary<HtmlNode, HtmlNode> tablesDic = new Dictionary<HtmlNode, HtmlNode>();
            List<HtmlNode> signatureNodeList = new List<HtmlNode>();
            string theSignature = " ";

            HtmlDocument ndoc = new HtmlDocument();
            ndoc.LoadHtml(oldRow.OuterHtml);
            HtmlNodeCollection spansList = ndoc.DocumentNode.SelectNodes("//span");

            if (spansList != null)
            {
                foreach (HtmlNode node in spansList)
                {
                    if (!String.IsNullOrEmpty(node.InnerHtml))
                    {
                        string textValue = node.InnerHtml;

                        if (textValue.Contains("[") && textValue.Contains("]"))
                        {


                            string[] properties = textValue.Split('[');

                            foreach (string property in properties)
                            {
                                if (property.Contains("]"))
                                {
                                    string[] array = property.Split(']');
                                    string propertyName = array[0];



                                    string[] fields = propertyName.Split('.');

                                    if (fields.Count() > 1)
                                    {



                                        if (fields[0] == multiObjectField.FieldName)
                                        {
                                            string originalName = "[" + propertyName + "]";
                                            propertyName = propertyName.Replace(multiObjectField.FieldName + ".", "");
                                            propertyName = "[" + propertyName + "]";

                                            string resultValue = GetHtmlNodeValue(node, propertyName, dataObject, multiEntityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, theGeneralService);
                                            node.InnerHtml = node.InnerHtml.Replace(originalName, resultValue);


                                        }
                                        else
                                        {
                                            propertyName = "[" + propertyName + "]";
                                            GetHtmlNodeValue(node, propertyName, theEntity, theEntityObjectFields, tablesDic, systemEntity, systemEntityObjectFields, signatureNodeList, tenant, theGeneralService);
                                        }

                                    }
                                    else
                                    {
                                        string resultValue = GetEntityFieldValue(theEntity, fields[0], theEntityObjectFields, tenant);

                                        node.InnerHtml = node.InnerHtml.Replace("[" + propertyName + "]", resultValue);

                                    }
                                }

                            }
                        }
                    }
                }
            }




            if (signatureNodeList.Count() != 0)
            {
                HtmlNode newSignatureSection = GetNewNodeHtml(theSignature);



                foreach (HtmlNode signatureNode in signatureNodeList)
                {
                    foreach (HtmlNode childNode in newSignatureSection.ChildNodes)
                    {
                        HtmlNode t2Node = ndoc.DocumentNode.AppendChild(childNode);

                        signatureNode.ParentNode.ParentNode.InsertBefore(t2Node, signatureNode.ParentNode);
                    }
                }


                foreach (HtmlNode signatureNode in signatureNodeList)
                {
                    signatureNode.ParentNode.ParentNode.RemoveChild(signatureNode.ParentNode);
                }


            }

            if (tablesDic.Count() != 0)
            {
                foreach (HtmlNode node in tablesDic.Keys)
                {
                    HtmlNode t2Node = node.AppendChild(tablesDic[node]);
                    node.ParentNode.ReplaceChild(t2Node, node);
                }
            }



            HtmlNode newRow = ndoc.DocumentNode;

            return newRow;
        }

        public HtmlNode GetNewNodeHtml(string nodeString)
        {
            HtmlDocument ndoc = new HtmlDocument();
            ndoc.LoadHtml(nodeString);
            HtmlNode htmlNode = ndoc.DocumentNode;
            return htmlNode;
        }


        #region GetTenantLogo

        public string GetTenantLogoHtml(string logoName, int tenant)
        {
            string logoCode = " ";
            string fileName = logoName;
            string fileExtension = "jpg";


            if (fileName == "WideLogo")
            {
                fileName = "sharedLogtsitcslogo";
                fileExtension = "png";
            }

            string logoFileName = fileName + tenant;

            byte[] logoFile = GetFileFromServer(logoFileName, fileExtension, "logos", tenant);

            if (logoFile != null)
            {
                //base64Data = new char[(int)(Math.Ceiling((double)logoFile.Length / 3) * 4)];

                //Convert.ToBase64CharArray(logoFile, 0, logoFile.Length, base64Data, 0);
                // System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                // string rawData = new String(base64Data);
                string rawData = System.Convert.ToBase64String(logoFile.ToArray(), 0, logoFile.ToArray().Length);


                MemoryStream ms = new MemoryStream(logoFile, 0, logoFile.Length);          // Convert byte[] to Image    
                ms.Write(logoFile, 0, logoFile.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                string width = image.Width.ToString() + "px";
                string height = image.Height.ToString() + "px";

                if (isSendMail)
                {
                    logoFileName = !string.IsNullOrEmpty(logoFileName) ? logoFileName.ToLower() : "";
                    rawData = "'" + "data:image/" + logoFileName + ";base64," + rawData + "'";
                }
                else
                {
                    rawData = "'" + "data:image/jpg;base64," + rawData + "'";

                }


                string style = "'" + "Height:" + height + ";Width:" + width + "'";

                logoCode = "<img style=" + style + " src=" + rawData + "/>";
                //   logoCode = "<img   src="+ rawData + "style='" + style+ "'" + " />";


            }



            return logoCode;
        }
        #endregion

        #region GetSecurityKeyLinkHtml
        public string GetSecurityKeyLinkHtml(string key, string entityId, string originalNodeInnerText, int tenant, bool hideSharedlogistics, string systemUrl)
        {
            //   "<t:Span FontFamily=\"Verdana\" FontSize=\"13.3299999237061\" Text=\"Our File  : SHIP_7180 \" UnderlineColor=\"#FF000000\" UnderlineDecoration=\"None\" xmlns:t=\"clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents\" />"
            originalNodeInnerText = originalNodeInnerText.Replace("\"", "'");

            //   string serverPath = LogitudeSettings.LogitudeURL;//System.Configuration.ConfigurationManager.AppSettings.Get("LogitudeURL");//HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Replace(HttpContext.Current.Request.UrlReferrer.PathAndQuery, "");
            string url = systemUrl;
            if (systemUrl.Contains("login.aspx"))
            {
                string[] test = systemUrl.Split('/');
                if (test != null && test.Length > 0)
                {
                    url = systemUrl.Replace("/" + test[test.Length - 1], "");
                }
            }



            string pageLink = (url + @"/SharedLogistic/ShipmentPage.aspx").ToLower() + "?securitykey=" + key + ":" + entityId + ":" + tenant + ":" + hideSharedlogistics;
            string styleLink = "'font-family:Arial;font-size:18px;color:#0000FF'";

            string Textlink = "<a style=" + styleLink + " href='" + pageLink + "'" + ">View online</a>";

            string reslut = "<p>" + originalNodeInnerText + Textlink + "</p>";

            return reslut;
        }
        #endregion

        #endregion


        #region Convert Xaml  Html


        string frOriginalClass = "";
        bool HideBorderTable = true;
        string tableColor = "";
        private HtmlDocument BuildTableHtml(string htmlstring)
        {


            //htmlstring = htmlstring.Replace("style=" , "fr-original-style='' style=");

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlstring);

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                double widthtotal = 0;
                HideBorderTable = true;
                bool hasColSpan = false;
                tableColor = "";
                string styleTable = "";

                if (table.Attributes["style"] != null) styleTable = table.Attributes["style"].Value;
                double widthTabletotal = 0;
                int numberOfTr = 0;
                int Trcount = table.SelectNodes("tr").Count;
                if (!string.IsNullOrEmpty(styleTable))
                {
                    styleTable = styleTable.ToLower().Trim();
                    string tablewidth = GetValueOfProperty(styleTable, "width");
                    if (!string.IsNullOrEmpty(tablewidth))
                    {
                        if (tablewidth.Contains("px")) widthTabletotal = double.Parse(tablewidth.Split('p')[0]);
                        else widthTabletotal = 751;

                    }
                }


                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    widthtotal = 0;
                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        if (!hasColSpan)
                        {
                            if (cell.Attributes["colspan"] != null)
                            {
                                hasColSpan = !string.IsNullOrEmpty(cell.Attributes["colspan"].Value) ? true : false;
                            }
                        }

                        var style = cell.Attributes["style"].Value;
                        string stringWidth = GetValueOfProperty(style, "width");
                        if (stringWidth.Contains("px"))
                        {
                            double widthnumber = double.Parse(stringWidth.Split('p')[0]);
                            widthtotal += widthnumber;
                        }
                    }

                    numberOfTr += 1;

                    if (widthTabletotal < widthtotal)
                    {
                        if (hasColSpan)
                        {
                            if (Trcount == 1)
                            {
                                if (widthTabletotal == 0) widthTabletotal = widthtotal;
                            }
                            else
                            {
                                if (widthTabletotal == 0 && Trcount == numberOfTr) widthTabletotal = widthtotal;

                            }
                        }
                        else widthTabletotal = widthtotal;
                    }


                    double total = widthTabletotal;
                    if (total == 0 || total == 0.0) total = 750;
                    double perc = (double)100 / total;

                    foreach (HtmlNode cell in row.SelectNodes("th|td"))
                    {
                        var style = cell.Attributes["style"].Value;
                        string stringWidth = GetValueOfProperty(style, "width");


                        string backgroudcolor = GetValueOfProperty(style, "background-color");
                        string vertical = GetValueOfProperty(style, "vertical-align");
                        string padding = GetValueOfProperty(style, "padding");

                        cell.Attributes.Add("fr-original-style", "");
                        if (stringWidth.Contains("px"))
                        {
                            double widthtd = double.Parse(stringWidth.Split('p')[0]);
                            if (widthtd != 0 && total != 0)
                            {
                                if (widthtd > total) widthtd = total;
                                double computewidthtd = perc * widthtd;
                                cell.Attributes["style"].Value = cell.Attributes["style"].Value.Replace(stringWidth + "px", computewidthtd.ToString() + "%");
                                cell.Attributes["fr-original-style"].Value += ("width:" + computewidthtd.ToString() + "%;");
                            }
                            else
                            {
                                cell.Attributes["style"].Value = cell.Attributes["style"].Value += "width:auto;";
                                cell.Attributes["fr-original-style"].Value += ("width:auto;");
                            }

                        }
                        else if (stringWidth.Contains("%"))
                        {
                            cell.Attributes["fr-original-style"].Value += ("width:" + stringWidth);
                        }

                        if (!string.IsNullOrEmpty(backgroudcolor)) cell.Attributes["fr-original-style"].Value += ("background-color:" + backgroudcolor + ";");

                        if (!string.IsNullOrEmpty(padding)) cell.Attributes["fr-original-style"].Value += ("padding:" + padding + ";");

                        if (!string.IsNullOrEmpty(vertical)) cell.Attributes["fr-original-style"].Value += ("vertical-align:" + vertical + ";");


                        frOriginalClass = "";

                        CustomBorderStyle(cell, style, "border-left");
                        CustomBorderStyle(cell, style, "border-right");
                        CustomBorderStyle(cell, style, "border-top");
                        CustomBorderStyle(cell, style, "border-bottom");

                        if (!string.IsNullOrEmpty(frOriginalClass))
                        {
                            cell.Attributes.Add("fr-original-class", frOriginalClass);

                        }
                    }
                    hasColSpan = false;
                }



                table.Attributes.Add("fr-original-style", "originalWidth");
                table.Attributes.Add("fr-original-class", "All");

                if (!string.IsNullOrEmpty(styleTable) && styleTable.Contains("width"))
                {


                    string tablewidth = GetValueOfProperty(styleTable, "width");
                    if (!string.IsNullOrEmpty(tablewidth) && tablewidth.Replace(" ", "") != "100%")
                    {
                        if (widthTabletotal < 750)
                        {
                            table.Attributes["style"].Value = table.Attributes["style"].Value.Replace("width:" + tablewidth, "width:" + widthTabletotal.ToString() + "px;");
                            table.Attributes["fr-original-style"].Value = table.Attributes["fr-original-style"].Value.Replace("originalWidth", "width:" + widthTabletotal.ToString() + "px;");
                        }
                        else
                        {
                            table.Attributes["style"].Value = table.Attributes["style"].Value.Replace("width:" + tablewidth, "width:100%");
                            table.Attributes["fr-original-style"].Value = table.Attributes["fr-original-style"].Value.Replace("originalWidth", "width:100%;");
                        }

                    }
                    else
                    {
                        table.Attributes["fr-original-style"].Value = table.Attributes["fr-original-style"].Value.Replace("originalWidth", "width:100%;");
                    }
                }
                else
                {

                    if (widthTabletotal < 750 && widthTabletotal != 0)
                    {

                        table.Attributes["style"].Value += "width:" + widthTabletotal.ToString() + "px;";
                        table.Attributes["fr-original-style"].Value = table.Attributes["fr-original-style"].Value.Replace("originalWidth", "width:" + widthTabletotal.ToString() + "px;");
                    }

                    else
                    {
                        table.Attributes["style"].Value += "width:100%;";
                        table.Attributes["fr-original-style"].Value = table.Attributes["fr-original-style"].Value.Replace("originalWidth", "width:100%;");
                    }


                }


                table.Attributes["fr-original-style"].Value += "table-layout:fixed;";

                if (!string.IsNullOrEmpty(tableColor))
                {
                    switch (tableColor.ToLower())
                    {
                        case "#ffffff":
                            table.Attributes["fr-original-class"].Value += " White";
                            break;
                        case "#000000":
                            table.Attributes["fr-original-class"].Value += " Black";
                            break;
                        case "#ff0000":
                            table.Attributes["fr-original-class"].Value += " Red";
                            break;

                        case "#ffff00":
                            table.Attributes["fr-original-class"].Value += " Yellow";
                            break;

                        case "#00b050":
                            table.Attributes["fr-original-class"].Value += " Green";
                            break;


                        case "#74a6e2":
                            table.Attributes["fr-original-class"].Value += " Blue";
                            break;

                        case "#1f497d":
                            table.Attributes["fr-original-class"].Value += " Darkblue";
                            break;

                        case "#808080":
                            table.Attributes["fr-original-class"].Value += " Gray";
                            break;


                        case "#d9d9d9":
                            table.Attributes["fr-original-class"].Value += " LightGray";
                            break;



                        case "#c00000":
                            table.Attributes["fr-original-class"].Value += " Brown";
                            break;

                        case "#9f2936":
                            table.Attributes["fr-original-class"].Value += " Maroon";
                            break;

                    }

                }

                RemoveTableStyle(table, "border-left");
                RemoveTableStyle(table, "border-right");
                RemoveTableStyle(table, "border-top");
                RemoveTableStyle(table, "border-bottom");


            }
            return doc;


        }

        private void GetBorderColor(HtmlNode cell, string style, string propName)
        {
            string borderstyle = GetValueOfProperty(style, propName);

            if (!string.IsNullOrEmpty(borderstyle))
            {
                borderstyle = borderstyle.Trim().ToLower().Replace(" ", "");
                borderstyle += ";";
                if (!borderstyle.Contains("none") && !borderstyle.Contains("0px") && !borderstyle.Contains("nan"))
                {
                    string color = getBetween(borderstyle, "#", ";").ToLower();

                    if (!string.IsNullOrEmpty(color))
                    {
                        if (color == "537db1" || color == "4f81bd" || color == "00b0f0") color = "74a6e2";// blue
                        if (color == "1f497d" || color == "376092") color = "1f497d";// Dark blue
                        if (color == "f96b77" || color == "d96b77") color = "9f2936"; // Maroon
                        if (color == "bfbfbf") color = "808080";//gray
                                                                // if ( color == "d9d9d9") color = "d9d9d9";//light gray

                        if (string.IsNullOrEmpty(tableColor) && !string.IsNullOrEmpty(color)) tableColor = "#" + color;
                        bool isdefultColor = false;

                        if (!string.IsNullOrEmpty(tableColor) && (tableColor == "#ffffff" || tableColor == "#000000" || tableColor == "#ff0000" || tableColor == "#ffff00" || tableColor == "#00b050" || tableColor == "#c00000" || tableColor == "#74a6e2" || tableColor == "#537db1" || tableColor == "#376092" || tableColor == "#4f81bd" || tableColor == "#1f497d" || tableColor == "#808080" || tableColor == "#9f2936" || tableColor == "#d9d9d9"))
                        {
                            isdefultColor = true;
                        }

                        if (!isdefultColor)
                        {
                            cell.Attributes["fr-original-style"].Value += (propName + "-color:#" + color + ";");
                        }
                    }



                }
            }


        }


        private void CustomBorderStyle(HtmlNode cell, string style, string prop)
        {
            string oldBorderleftstyle = getBetween(style, prop, ";");

            if (string.IsNullOrEmpty(tableColor)) GetBorderColor(cell, style, prop);

            if (!string.IsNullOrEmpty(oldBorderleftstyle))
            {
                string oldBorderValue = getBetween(oldBorderleftstyle, ":", "px");

                if (!string.IsNullOrEmpty(oldBorderValue))
                {

                    if (oldBorderleftstyle.ToLower().Contains("none") || oldBorderValue.Replace(" ", "").Trim() == "0")
                    {
                        if (prop == "border-left")
                        {
                            frOriginalClass += string.IsNullOrEmpty(frOriginalClass) ? "BorderLeft" : " BorderLeft";
                        }
                        if (prop == "border-right")
                        {
                            frOriginalClass += string.IsNullOrEmpty(frOriginalClass) ? "BorderRight" : " BorderRight";
                        }

                        if (prop == "border-top")
                        {
                            frOriginalClass += string.IsNullOrEmpty(frOriginalClass) ? "BorderTop" : " BorderTop";
                        }
                        if (prop == "border-bottom")
                        {
                            frOriginalClass += string.IsNullOrEmpty(frOriginalClass) ? "BorderBottom" : " BorderBottom";

                        }
                    }
                    else if (oldBorderValue.Contains("0."))
                    {
                        string newBorderleftstyle = oldBorderleftstyle.Replace(oldBorderValue, "1");
                        cell.Attributes["style"].Value = cell.Attributes["style"].Value.Replace(oldBorderleftstyle, newBorderleftstyle);
                    }

                }



            }
        }

        private void RemoveTableStyle(HtmlNode tableNode, string propName)
        {

            if (tableNode.Attributes["style"] != null)
            {
                var styleTable = tableNode.Attributes["style"].Value;
                string styleProp = GetValueOfProperty(styleTable, propName);
                tableNode.Attributes["style"].Value = tableNode.Attributes["style"].Value.Replace(propName + ":" + styleProp + ";", "");
            }

        }


        private double ResolveWith(string stringwidth)
        {
            double widthnumber = 0;


            if (!string.IsNullOrEmpty(stringwidth) && stringwidth.Trim().ToLower() != "nan")
            {
                widthnumber = double.Parse(stringwidth);
            }
            return widthnumber;
        }


        private static string GetHtmlSrtingFromByte(Byte[] bytedata)
        {
            string html = "";
            XamlFormatProvider provider = new XamlFormatProvider();
            RadDocument document = provider.Import(bytedata);
            HtmlFormatProvider htmlFormatProvider = new HtmlFormatProvider();
            Telerik.Windows.Documents.FormatProviders.Html.HtmlExportSettings htmlExportSettings = new HtmlExportSettings();
            htmlExportSettings.DocumentExportLevel = DocumentExportLevel.Fragment;
            htmlExportSettings.StylesExportMode = StylesExportMode.Inline;
            htmlExportSettings.StyleRepositoryExportMode = StyleRepositoryExportMode.DontExportStyles;
            htmlExportSettings.ExportFontStylesAsTags = true;
            htmlExportSettings.DocumentExportLevel = DocumentExportLevel.Fragment;
            htmlExportSettings.ImageExportMode = Telerik.Windows.Documents.FormatProviders.Html.ImageExportMode.Base64Encoded;
            htmlFormatProvider.ExportSettings = htmlExportSettings;
            html = htmlFormatProvider.Export(document);
            return html;
        }

        // Convert byte xaml to byte string



        #region  DocumentType Template Convert Headerr And Footer And Body To Html And Update
        public DocumentTypeTemplate UpdateHeaderAndFooterAndBodyHtml(DocumentTypeTemplate documentTypeTemplate)
        {

            XmlDataDocument messageDoc = new XmlDataDocument();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            string xamlString = enc.GetString(documentTypeTemplate.TemplateBody);

            documentTypeTemplate.TemplateHeaderHtml = null;
            documentTypeTemplate.TemplateFooterHtml = null;
            documentTypeTemplate.TemplateBodyHtml = null;
            documentTypeTemplate.TemplateHeaderHeight = 0;
            documentTypeTemplate.TemplateFooterHeight = 0;

            if (!string.IsNullOrEmpty(xamlString))
            {
                XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);
                XmlDocument doc2 = (XmlDocument)messageDoc.Clone();

                XmlDataDocument xmlDataDocumentHeader = new XmlDataDocument();
                XmlDataDocument xmlDataDocumentFooter = new XmlDataDocument();

                XmlNodeList spansListHeader = messageDoc.GetElementsByTagName("t:Header.Body");
                XmlNodeList spansListFooter = messageDoc.GetElementsByTagName("t:Footer.Body");

                byte[] bytedata = null;

                //Header
                if (spansListHeader != null && spansListHeader.Count > 0)
                {
                    xmlDataDocumentHeader.LoadXml(spansListHeader[0].InnerXml);
                    bytedata = enc.GetBytes(xmlDataDocumentHeader.InnerXml);
                    documentTypeTemplate.TemplateHeaderHtml = ConvertXmalToHtml(bytedata);
                    documentTypeTemplate.TemplateHeaderHeight = GetHeightHeaderAndfooter(xmlDataDocumentHeader.InnerXml);

                }

                //Footer
                if (spansListFooter != null && spansListFooter.Count > 0)
                {
                    xmlDataDocumentFooter.LoadXml(spansListFooter[0].InnerXml);
                    bytedata = enc.GetBytes(xmlDataDocumentFooter.InnerXml);
                    documentTypeTemplate.TemplateFooterHtml = ConvertXmalToHtml(bytedata);
                    documentTypeTemplate.TemplateFooterHeight = GetHeightHeaderAndfooter(xmlDataDocumentFooter.InnerXml);
                }


                //Body
                bytedata = enc.GetBytes(messageDoc.InnerXml);
                documentTypeTemplate.TemplateBodyHtml = ConvertXmalToHtml(bytedata);


            }

            return documentTypeTemplate;

        }

        public DocumentTypeTemplatePM UpdateHeaderAndFooterAndBodyHtmlDocumentTypeTemplatePM(DocumentTypeTemplatePM documentTypeTemplate)
        {

            try
            {
                XmlDataDocument messageDoc = new XmlDataDocument();
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                string htmlString = enc.GetString(documentTypeTemplate.TemplateBody);
                XmlReader reader = XmlReader.Create(new StringReader(htmlString));
                messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);
                XmlDocument doc2 = (XmlDocument)messageDoc.Clone();

                XmlDataDocument xmlDataDocumentHeader = new XmlDataDocument();
                XmlDataDocument xmlDataDocumentFooter = new XmlDataDocument();

                XmlNodeList spansListHeader = messageDoc.GetElementsByTagName("t:Header.Body");
                XmlNodeList spansListFooter = messageDoc.GetElementsByTagName("t:Footer.Body");

                byte[] bytedata = null;

                documentTypeTemplate.TemplateHeaderHtml = null;
                documentTypeTemplate.TemplateFooterHtml = null;
                documentTypeTemplate.TemplateBodyHtml = null;
                documentTypeTemplate.TemplateHeaderHeight = 0;
                documentTypeTemplate.TemplateFooterHeight = 0;
                //Header
                if (spansListHeader != null && spansListHeader.Count > 0)
                {
                    xmlDataDocumentHeader.LoadXml(spansListHeader[0].InnerXml);
                    bytedata = enc.GetBytes(xmlDataDocumentHeader.InnerXml);
                    documentTypeTemplate.TemplateHeaderHtml = ConvertXmalToHtml(bytedata);
                    documentTypeTemplate.TemplateHeaderHeight = GetHeightHeaderAndfooter(xmlDataDocumentHeader.InnerXml);
                }

                //Footer
                if (spansListFooter != null && spansListFooter.Count > 0)
                {
                    xmlDataDocumentFooter.LoadXml(spansListFooter[0].InnerXml);
                    bytedata = enc.GetBytes(xmlDataDocumentFooter.InnerXml);
                    documentTypeTemplate.TemplateFooterHtml = ConvertXmalToHtml(bytedata);
                    documentTypeTemplate.TemplateFooterHeight = GetHeightHeaderAndfooter(xmlDataDocumentFooter.InnerXml);
                }


                //Body
                bytedata = enc.GetBytes(messageDoc.InnerXml);
                documentTypeTemplate.TemplateBodyHtml = ConvertXmalToHtml(bytedata);


                return documentTypeTemplate;
            }
            catch (Exception ex)
            {
                return documentTypeTemplate;
            }



        }

        private int GetHeightHeaderAndfooter(string data, bool isDateField = true)
        {

            int height = 0;
            if (!string.IsNullOrEmpty(data))
            {
                height = 2;
                data = data.ToLower();
                if (isDateField)
                {
                    if (data.Contains("systemdata.logo")) height = 8;
                    else if (data.Contains("systemdata.smalllogo")|| data.Contains("systemdata.widelogo")) height = 4;

                }
                else
                {
                    if (data.Contains("img"))
                    {
                        if (data.Trim().Contains("300")) height = 8;
                        else height = 4;
                    }

                }


            }
            return height;
        }

        public byte[] ConvertXmalToHtml(byte[] bytedata)
        {
            byte[] result = null;
            string html = "";

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            if (bytedata != null)
            {
                html = GetHtmlSrtingFromByte(bytedata);

                if (!string.IsNullOrEmpty(html))
                {
                    if (html.Contains("table"))
                    {
                        var doc = BuildTableHtml(html);
                        html = doc.DocumentNode.OuterHtml;

                    }
                }
            }

            if (!string.IsNullOrEmpty(html))
            {
                html = html.Replace("\"", "'");
                html = html.Replace(": '", ":");
                html = html.Replace(":'", ":");
                html = html.Replace("';", ";");
                html = html.Replace("' ;", ";");




                result = enc.GetBytes(html);
            }


            return result;


        }


        #endregion


        #region Convert Signature XML To Html
        public byte[] ConvertXmlByteToHtmlByte(byte[] xmalbyte)
        {
            byte[] result = null;

            if (xmalbyte != null)
            {
                XmlDataDocument messageDoc = new XmlDataDocument();
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                string xamlString = enc.GetString(xmalbyte);
                if (!string.IsNullOrEmpty(xamlString))
                {
                    XmlReader reader = XmlReader.Create(new StringReader(xamlString));
                    messageDoc = new XmlDataDocument();
                    messageDoc.Load(reader);
                    XmlDocument doc2 = (XmlDocument)messageDoc.Clone();
                    byte[] bytedata = enc.GetBytes(messageDoc.InnerXml);
                    result = ConvertXmalToHtml(bytedata);
                }
            }

            return result;
        }

        #endregion







        #region  Convert xmal Template To Part Html (Header Footer Body as List)

        public List<object> GetTemplatePartsAsHtml(byte[] xmal)
        {
            List<object> result = new List<object>();

            try
            {
                XmlDataDocument messageDoc = new XmlDataDocument();
                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                string htmlString = enc.GetString(xmal);
                string resultString = htmlString;
                XmlReader reader = XmlReader.Create(new StringReader(htmlString));
                messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);
                XmlDocument doc2 = (XmlDocument)messageDoc.Clone();

                result = GetHtmlStringPartFromXmlDataDocument(messageDoc);

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }

        }
        List<object> GetHtmlStringPartFromXmlDataDocument(XmlDataDocument messageDoc)
        {
            string htmlHeader = "";
            string htmlFooter = "";
            string htmlstring = "";
            int HeightHeader = 0;
            int HeightFooter = 0;

            List<object> result = new List<object>();

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            XmlDataDocument xmlDataDocumentHeader = new XmlDataDocument();
            XmlDataDocument xmlDataDocumentFooter = new XmlDataDocument();

            XmlNodeList spansListHeader = messageDoc.GetElementsByTagName("t:Header.Body");
            XmlNodeList spansListFooter = messageDoc.GetElementsByTagName("t:Footer.Body");

            byte[] bytedata = null;

            //Header
            if (spansListHeader != null && spansListHeader.Count > 0)
            {
                xmlDataDocumentHeader.LoadXml(spansListHeader[0].InnerXml);
                bytedata = enc.GetBytes(xmlDataDocumentHeader.InnerXml);
                htmlHeader = enc.GetString(ConvertXmalToHtml(bytedata));
                HeightHeader = GetHeightHeaderAndfooter(htmlHeader, false);

                htmlHeader = "<header><height>" + "<div style='display:none'>" + HeightHeader + "</div></height>" + htmlHeader + "</header>";


            }


            //Footer
            if (spansListFooter != null && spansListFooter.Count > 0)
            {
                xmlDataDocumentFooter.LoadXml(spansListFooter[0].InnerXml);
                bytedata = enc.GetBytes(xmlDataDocumentFooter.InnerXml);
                htmlFooter = enc.GetString(ConvertXmalToHtml(bytedata));

                HeightFooter = GetHeightHeaderAndfooter(htmlFooter, false);
                htmlFooter = "<footer><height>" + "<div style='display:none'>" + HeightFooter + "</div></height><div style ='bottom:0;'>" + htmlFooter + "</div></footer>";
            }


            //Body
            bytedata = enc.GetBytes(messageDoc.InnerXml);
            string body = enc.GetString(ConvertXmalToHtml(bytedata));

            result.Add(htmlHeader);
            result.Add(body);
            result.Add(htmlFooter);



            return result;
        }

        private string CorrectionHtml(string html)
        {

            html = html.Replace("\"", "'");
            html = html.Replace(": '", ":");
            html = html.Replace(":'", ":");
            html = html.Replace("';", ";");
            html = html.Replace("' ;", ";");


            if (html.Contains("table"))
            {
                var doc = BuildTableHtml(html);
                html = doc.DocumentNode.OuterHtml;

            }

            return html;
        }


        #endregion


        #region getTemplatePartFromHtml
        public List<object> GetTemplatePartFromHtml(string html)
        {
            List<object> result = new List<object>();
            string footerHtmlString = "";
            string headerHtmlString = "";
            int HeightHeader = 0;
            int FooterHeader = 0;
            #region Header
            if (html.Contains("<header>"))
            {
                headerHtmlString = getBetween(html, "<header>", "</header>");
                html = html.Replace("<header>" + headerHtmlString + "</header>", "");


                var height = getBetween(headerHtmlString, "<height>", "</height>");
                headerHtmlString = headerHtmlString.Replace("<height>" + height + "</height>", "");

                height = getBetween(height, "<div style='display:none'>", "</div>");


                if (!string.IsNullOrEmpty(height) && height != "undefined" && height != "null")
                {
                    HeightHeader = Int32.Parse(height);
                }


            }
            #endregion

            #region Footer
            if (html.Contains("<footer>"))
            {
                PDFImageHeight = 0;
                footerHtmlString = getBetween(html, "<footer>", "</footer>");
                html = html.Replace("<footer>" + footerHtmlString + "</footer>", "");


                var height = getBetween(footerHtmlString, "<height>", "</height>");
                footerHtmlString = footerHtmlString.Replace("<height>" + height + "</height>", "");

                height = getBetween(height, "<div style='display:none'>", "</div>");
                if (!string.IsNullOrEmpty(height) && height != "undefined" && height != "null")
                {
                    FooterHeader = Int32.Parse(height);
                }



            }
            #endregion

            result.Add(headerHtmlString);
            result.Add(html);
            result.Add(footerHtmlString);
            result.Add(HeightHeader);
            result.Add(FooterHeader);
            return result;

        }

        #endregion


        #region BuildPdfDocumentHtml
        double HeaderImageHeight = 0;
        double FooterImageHeight = 0;
        double PDFImageHeight = 0;
        public byte[] BuildPdfDocumentHtml(string htmlParts)
        {
            byte[] result = null;

            htmlParts = htmlParts.Replace("<tbody>", "").Replace("</tbody>", "");

            string headerHtmlString = "";
            string footerHtmlString = "";
            float HeaderHeight = 0;
            float FooterHeight = 0;

            #region Header
            if (htmlParts.Contains("<header>"))
            {
                headerHtmlString = getBetween(htmlParts, "<header>", "</header>");
                htmlParts = htmlParts.Replace("<header>" + headerHtmlString + "</header>", "");
                if (!string.IsNullOrEmpty(headerHtmlString))
                {
                    var heightheader = getBetween(headerHtmlString, "<height>", "</height>");
                    headerHtmlString = headerHtmlString.Replace("<height>" + heightheader + "</height>", "");

                    heightheader = getBetween(heightheader, "<div style='display:none'>", "</div>");
                    if (!string.IsNullOrEmpty(heightheader) && heightheader != "undefined" && heightheader != "null") HeaderHeight = float.Parse(heightheader);

                    headerHtmlString = ConvertNormalHtmlToEvoHtml(headerHtmlString);


                }

            }
            #endregion

            #region Footer
            PDFImageHeight = 0;
            if (htmlParts.Contains("<footer>"))
            {

                footerHtmlString = getBetween(htmlParts, "<footer>", "</footer>");
                htmlParts = htmlParts.Replace("<footer>" + footerHtmlString + "</footer>", "");

                if (!string.IsNullOrEmpty(footerHtmlString))
                {
                    var heightfooter = getBetween(footerHtmlString, "<height>", "</height>");
                    footerHtmlString = footerHtmlString.Replace("<height>" + heightfooter + "</height>", "");

                    heightfooter = getBetween(heightfooter, "<div style='display:none'>", "</div>");

                    if (!string.IsNullOrEmpty(heightfooter) && heightfooter != "undefined" && heightfooter != "null") FooterHeight = float.Parse(heightfooter);

                    footerHtmlString = ConvertNormalHtmlToEvoHtml(footerHtmlString);
                }

            }
            #endregion

            string bodyHtml = htmlParts; // ConvertNormalHtmlToEvoHtml(htmlParts,  "Body");



            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.LicenseKey = "fvDj8eTh8eDg4vHk/+Hx4uD/4OP/6Ojo6A==";
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.HtmlViewerWidth = 770;
            pdfConverter.PdfDocumentOptions.LeftMargin = 10;
            pdfConverter.PdfDocumentOptions.TopMargin = 10;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.EnhancedGraphicsQuality = true;
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            pdfConverter.ClipHtmlView = true;

            if (!string.IsNullOrEmpty(headerHtmlString))
            {
                pdfConverter.PdfDocumentOptions.ShowHeader = true;
                HtmlToPdfElement headerHtml = new HtmlToPdfElement(0, 0, 0, 0, headerHtmlString, null, 2040, 0);
                pdfConverter.PdfHeaderOptions.HeaderHeight = HeaderHeight != 0 ? HeaderHeight * (float)28.3465 : 1;
                pdfConverter.PdfHeaderOptions.AddElement(headerHtml);
            }
            if (!string.IsNullOrEmpty(footerHtmlString))
            {
                pdfConverter.PdfDocumentOptions.ShowFooter = true;
                HtmlToPdfElement footerHtml = new HtmlToPdfElement(0, 0, 0, 0, footerHtmlString, null, 2040, 0);
                pdfConverter.PdfFooterOptions.AddElement(footerHtml);
                pdfConverter.PdfFooterOptions.FooterHeight = FooterHeight != 0 ? FooterHeight * (float)28.3465 : 1;


            }

            pdfConverter.MediaType = "Screen";

            result = pdfConverter.GetPdfBytesFromHtmlString(bodyHtml);

            return result;
        }

        public string ConvertNormalHtmlToEvoHtml(string html, string area = "HeaderFooter")
        {

            if (!string.IsNullOrEmpty(html))
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                if (html.ToLower().Contains("img")) html = ChangeStyleElement(html, "img", area, doc);
                if (html.ToLower().Contains("table")) html = ChangeStyleElement(html, "table", area, doc);
                if (html.ToLower().Contains("div")) html = ChangeStyleElement(html, "div", area, doc);
                if (html.ToLower().Contains("p")) html = ChangeStyleElement(html, "p", area, doc);
                if (html.ToLower().Contains("span")) html = ChangeStyleElement(html, "span", area, doc);

            }


            return html;
        }

        private string ChangeStyleElement(string htmlString, string htmlElement, string area, HtmlAgilityPack.HtmlDocument doc)
        {
            var elementDoc = doc.DocumentNode.SelectNodes("//" + htmlElement);
            if (elementDoc != null)
            {
                #region Table 
                if (htmlElement == "table")
                {
                    foreach (HtmlNode table in elementDoc)
                    {
                        var trDoc = table.SelectNodes("tr");
                        if (trDoc != null)
                        {
                            foreach (HtmlNode tr in trDoc)
                            {
                                ChangePropertyElement(area, tr);

                                foreach (HtmlNode td in tr.SelectNodes("th|td")) ChangePropertyElement(area, td);


                            }
                        }

                    }
                }
                #endregion

                else
                {
                    foreach (HtmlNode element in elementDoc)
                    {
                        ChangePropertyElement(area, element);
                    }
                }


            }



            htmlString = doc.DocumentNode.OuterHtml;
            return htmlString;
        }

        private void ChangePropertyElement(string area, HtmlNode element)
        {
            if (element != null)
            {
                if (element.Attributes["style"] != null)
                {
                    element.Attributes["style"].Value = element.Attributes["style"].Value.ToLower();
                    var style = element.Attributes["style"].Value;
                    if (!string.IsNullOrEmpty(style))
                    {
                        foreach (string values in style.Split(';'))
                        {
                            if (!string.IsNullOrEmpty(values))
                            {
                                string propName = values.Trim().ToLower().Split(':')[0]; // border - width: 1px 0px 0px 1px
                                if (propName == "font-size") ChangeSize(element, values, "font-size:", area);
                                else if (propName == "width") ChangeSize(element, values, "width:", area);
                                else if (propName == "height") ChangeSize(element, values, "height:", area);
                            }

                        }
                    }

                }

                if (element.Name == "img" && area != "Body")
                {
                    if (element.Attributes["width"] != null) ChangeSizeImage(element, "width");
                    if (element.Attributes["height"] != null) ChangeSizeImage(element, "height");
                }

            }
        }


        private void ChangeSize(HtmlNode elment, string values, string propName, string area)
        {
            if (!string.IsNullOrEmpty(values))
            {
                if (values.Contains("px"))
                {
                    var size = getBetween(values, propName, "px");

                    if (!string.IsNullOrEmpty(size) && size.Trim().ToLower() != "nan")
                    {
                        double sizenumber = double.Parse(size);
                        if (area != "Body")
                        {
                            if (elment.Name == "img") sizenumber = sizenumber * 1.7;
                            else sizenumber = sizenumber * 2.3;

                        }

                        elment.Attributes["style"].Value = elment.Attributes["style"].Value.Replace(values, propName + sizenumber.ToString() + "px");

                    }
                }
            }

        }

        private void ChangeSizeImage(HtmlNode element, string propName)
        {
            if (!string.IsNullOrEmpty(element.Attributes[propName].Value))
            {
                var imagesize = double.Parse(element.Attributes[propName].Value);
                imagesize = imagesize * 1.7;
                //  imagesize = ((double)imagesize) * ((double)96 / (double)72);
                element.Attributes[propName].Value = imagesize.ToString();
                if (propName == "height") PDFImageHeight = imagesize;


            }
        }
        #endregion

        #endregion


        //private double GetFontSizeInPixles(double value)
        //{
        //    double pixels = (double)points * ((double)per / (double)72);*/
        //    return pixels;
        //}

        public static MessageArgs GetHtmlFromTemplate(string templateId, string objectTableId, int tenant)
        {
            MessageArgs result = new MessageArgs();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);
                DocumentTypeTemplate template = documentTypeTemplateRepository.GetSingleDocumentTypeTemplateByTenant(templateId, tenant);
                HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                if (template != null && template.TemplateType == "M")
                {
                    string subject = template.Subject;
                    string from = template.From;
                    string replyTo = template.ReplyTo;
                    string cc = template.CC;
                    result.HtmlTemplate = htmlEditorHelper.GetEditorHtmlData("", "", objectTableId, "", "", tenant, template.LastUpdatedByUserId, true, template.Id, ref subject, ref from, ref replyTo, ref cc);
                    result.HtmlTemplate = htmlEditorHelper.GetLogoHtmlString(result.HtmlTemplate);


                    result.From = from;
                    result.ReplyTo = replyTo;
                    result.Subject = subject;
                }

                scope.Complete();

            }

            return result;
        }

        public string GetLogoHtmlString(string htmlString)
        {


            htmlString = htmlString.Trim();
            htmlString = htmlString.Replace("\"", "'");
            htmlString = htmlString.Replace(": '", ":");
            htmlString = htmlString.Replace(":'", ":");
            htmlString = htmlString.Replace("' ;", ";");

            string[] htmlStringArray = htmlString.Split('<');

            List<string> imagesList = htmlStringArray.Where(s => s.StartsWith("img")).ToList();
            foreach (string image in imagesList)
            {
                var imageString = image;

                if (imageString.Contains("class"))
                {
                    var class1 = getBetween(imageString, "class='", "'");
                    var class2 = "class='" + class1 + "'";
                    imageString = imageString.Replace(class2, "");
                    htmlString = htmlString.Replace(image, imageString);
                }

                if (imageString.Contains("base64"))
                {
                    int startIndex = imageString.IndexOf("image/") + 6;
                    int endIndex = imageString.IndexOf(";base64") - startIndex;
                    string imageName = imageString.Substring(startIndex, endIndex);

                    string[] srcStringArray = imageString.Split(' ');

                    List<string> srcList = srcStringArray.Where(s => s.StartsWith("src")).ToList();
                    if (srcList.FirstOrDefault() != null)
                    {
                        string src = srcList.FirstOrDefault();
                        if (!src.Contains("/>") && imageString.Contains("/>"))
                        {
                            htmlString = htmlString.Replace(src, "src='cid:" + imageName + "'");
                        }
                        else if (!src.Contains("/>") && imageString.Contains(">"))
                        {

                            src = src.Replace("/>", "").Replace(">", "");
                            htmlString = htmlString.Replace(imageString, imageString.Replace(">", "/>"));
                            htmlString = htmlString.Replace(src, "src='cid:" + imageName + "'");
                        }
                        else if (!src.Contains("/>") && !imageString.Contains("/>"))
                        {
                            src = src.Replace("/>", "").Replace(">", "");
                            htmlString = htmlString.Replace(imageString, imageString + "/>");
                            htmlString = htmlString.Replace(src, "src='cid:" + imageName + "'");
                        }

                        else
                        {
                            htmlString = htmlString.Replace(src, "src='cid:" + imageName + "'/>");
                        }
                    }
                }
                else
                {
                    if (!imageString.Contains("/>"))
                    {
                        if (!imageString.Contains(">")) htmlString = htmlString.Replace(imageString, imageString + "/>");
                        else htmlString = htmlString.Replace(imageString, imageString.Replace(">", "/>"));

                    }
                }

            }

            return htmlString;
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


        public string GetValueOfProperty(string style, string propName)
        {
            string result = "";

            if (!string.IsNullOrEmpty(style))
            {
                style = style.ToLower().Trim();
                propName = propName.ToLower().Trim();
                string[] propTable = style.Split(';');
                if (propTable != null && style.Contains(propName))
                {
                    string prop = propTable.Where(d => !string.IsNullOrEmpty(d) && d.Contains(':') && d.Split(':')[0] == propName).FirstOrDefault();
                    string[] propParts = prop.Split(':');

                    if (propParts != null && propParts.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(propParts[1]))
                        {
                            result = propParts[1];
                            if (propName == "width")
                            {

                                if (result.Trim().Replace(" ", "").Contains("nan")) result = "";
                                else if (result.Contains("px"))
                                {
                                    string valueArray = result.Split('p')[0];
                                    if (string.IsNullOrEmpty(valueArray)) result = "";

                                }
                            }

                        }
                    }
                }
            }

            return result;


        }




    }
}
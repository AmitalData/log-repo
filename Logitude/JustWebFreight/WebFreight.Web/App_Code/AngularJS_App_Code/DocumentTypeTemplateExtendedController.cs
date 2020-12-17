using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using System.Web;
using System.Text;
using System.Xml;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools;
using WebFreight.Web.DataContracts;

using Logitude.BL.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class DocumentTypeTemplateExtendedController : ApiController
    {
        public HttpResponseMessage GetDocumentTypeTemplatesPMForDocumentType(string documentTypeId , string templateType,int tenant )
        {
            try
            {
                Authentication();
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                List<DocumentTypeTemplatePM> DocumentTypeTemplatePMList = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeId(documentTypeId, tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, DocumentTypeTemplatePMList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDocumentTypeTemplateListsForDocumentType(string documentTypeId, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                List<DocumentTypeTemplateList> documentTypeTemplateLists = documentTypeTemplateQuery.GetDocumentTypeTemplateListsByDocumentTypeId(documentTypeId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypeTemplateLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage PutSaveDocumentTypeTemplate(DocumentTypeTemplateFilter filter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                EncodedHtmlHelper encodedHtmlHelper = new EncodedHtmlHelper();

                #region SaveEditFields
                if (!string.IsNullOrEmpty(filter.Processtype) && filter.Processtype == "SaveEditFields")
                {

                    if (filter.EditableFieldLists == null) filter.EditableFieldLists = new List<EditableFieldPosition>();
                     var reslut = SaveEditableField(authToken.Tenant, filter.DocumentOutId, filter.EditableFieldLists.Where(d => d.Status == "Change" ).ToList(), filter.ReportKey);
                    return Request.CreateResponse(HttpStatusCode.OK, reslut);

                }
                #endregion

                #region Signature
                else if (filter.Processtype == "Signature")
                {
                    SecurityUtility.CheckContactFeature("Contact", "UPDATE", authToken.Tenant);

                    ContactRepository contactRepository = new ContactRepository(filter.Tenant);

                    Contact contact = contactRepository.GetSingleContact(filter.Id, filter.Tenant);
                    if (contact != null)
                    {

                   
                        if (!string.IsNullOrEmpty(filter.Body)) filter.Body = filter.Body.Replace("\"", "'");
                        filter.Body = encodedHtmlHelper.EncodedHtmlScript(filter.Body);
                        byte[] bytedata = enc.GetBytes(filter.Body);
                        contact.SignatureHtml = bytedata;
                        contactRepository.Update(contact);
                        contactRepository.SubmitChanges();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }

                #endregion

                else
                {
                    SecurityUtility.CheckContactFeature("DocumentTypeTemplate", "UPDATE", authToken.Tenant);
                    DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(filter.Tenant);
                    DocumentTypeTemplate documentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplateByTenant(filter.Id, filter.Tenant);
                    if (string.IsNullOrEmpty(filter.TemplateType))
                    {
                        documentTypeTemplate.InActive = filter.InActive;
                    }
                    else
                    {
                       

                        if (filter.TemplateType == "HTML")
                        {
                            if (!string.IsNullOrEmpty(filter.Body)) filter.Body = filter.Body.Replace("\"", "'");
                            filter.Body = encodedHtmlHelper.EncodedHtmlScript(filter.Body);
                            byte[] bytedata = enc.GetBytes(filter.Body);
                            documentTypeTemplate.TemplateBodyHtml = bytedata;
                        }
                        else if (filter.TemplateType == "Stimul")
                        {
                            byte[] bytedata = enc.GetBytes(filter.Body);
                            documentTypeTemplate.TemplateBodyjson = bytedata;
                        }
                    }

                    documentTypeTemplate.Subject = filter.Subject;
                    documentTypeTemplateRepository.Update(documentTypeTemplate);
                    documentTypeTemplateRepository.SubmitChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, documentTypeTemplate);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PutConvertXmalByteTojosnObject(byte[] xmal)
        {
            try
            {
                Authentication();
                HtmlTemplateClass htmlTemplateClass = new HtmlTemplateClass();
                if (xmal != null) {
                    htmlTemplateClass=  LogitudeXmlSerializer.DeserializeObject<HtmlTemplateClass>(xmal);
                }
              
             return Request.CreateResponse(HttpStatusCode.OK, htmlTemplateClass);
                
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        


        public HttpResponseMessage GetTemplateBodyhtmlOrJsonByDocumentTemplateId(string documentTyptemplateId , int tenant,  bool isHtml, string pagetype)
        {
            try
            {

                Authentication();
                string result = "";
                byte[] data = null;
                if (pagetype == "Signature")
                {
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    data = contactQuery.GetSignatureHtmlByContactId(documentTyptemplateId, tenant);
                }

                else
                {
                    DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                    if (isHtml)
                    {
                        data = documentTypeTemplateQuery.GetTemplateBodyHtmlByDocumentTypeTemplateId(documentTyptemplateId, tenant);
                    }
                    else
                    {
                        data = documentTypeTemplateQuery.GetTemplateBodyjsonByDocumentTypeTemplateId(documentTyptemplateId, tenant);
                    }
                }

                if (data != null)
                {
                    result = System.Text.Encoding.UTF8.GetString(data);
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        private byte[] SaveEditableField( int tenant,string documentOutId, List<EditableFieldPosition> editableFieldLists ,string reportKey)
        {
            byte[] result = null;
            
            if (editableFieldLists != null && editableFieldLists.Count > 0)
            {
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(tenant);
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(tenant);

                #region Get Current  Editable Field

                DocumentOut documentOut = documentOutRepository.GetSingleDocumentOut(documentOutId, tenant);
                byte[] EditableFieldsByte = documentOut != null ? documentOut.EditableFields : null;
                string currentEditableFields = EditableFieldsByte != null ? System.Text.Encoding.UTF8.GetString(EditableFieldsByte) : "<?xml version='1.0' encoding='utf-8' standalone='yes'?><StiSerializer version='1.02' type='Silverlight' application='Editable Fields of Rendered Report'><Items isList='true' count='0'></Items></StiSerializer>"; ;
                XmlReader reader = XmlReader.Create(new StringReader(currentEditableFields));
                XmlDataDocument messageDoc = new XmlDataDocument();
                messageDoc.Load(reader);
                XmlNodeList ItemsList = messageDoc.GetElementsByTagName("Items");
                XmlNodeList childNodes = ItemsList[0].ChildNodes;
                List<XmlNode> Items = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(childNodes))).ToList();
                #endregion

                #region Read mdc
                List<XmlNode> pageNodeLists = new List<XmlNode>();
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = reportKey,
                    FolderName = "others",
                    Extension = "mdc",
                    Tenant = tenant,
                };

                byte[] data = storageservice.Read(fileInfo);
                if (data != null)
                {
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                    string mdc = encoding.GetString(data);

                    XmlReader xmlReader = null;
                    XmlDataDocument xmlDataDocument = new XmlDataDocument();
        
                    if (!string.IsNullOrEmpty(mdc))
                    {
                        xmlReader = XmlReader.Create(new StringReader(mdc));
                        xmlDataDocument.Load(xmlReader);
                        XmlNodeList xmlNodeList = xmlDataDocument.GetElementsByTagName("Components");
                        pageNodeLists = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(xmlNodeList))).Where(d => d.ParentNode != null && !string.IsNullOrEmpty(d.ParentNode.Name) && d.ParentNode.Name.ToLower().Contains("page")).ToList();
                    }

                }

                #endregion

                #region Delete Items

                int itemCount = Items.Count();

                foreach (EditableFieldPosition editableFieldPosition in editableFieldLists)
                {
                    int indexNumber = -1;
                    foreach (XmlNode item in pageNodeLists)
                    {
                        indexNumber += 1;
                        string index = indexNumber.ToString();

                        if (!string.IsNullOrEmpty(index))
                        {
                            XmlNode fieldNode = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(item.ChildNodes))).Where(d => (d.Attributes["name"] != null && d.Attributes["name"].Value == editableFieldPosition.FieldName) || (d.Attributes["type"] != null && d.Attributes["type"].Value.ToLower() == "checkbox" && d.Name == editableFieldPosition.FieldName)).FirstOrDefault();

                            if (fieldNode != null)
                            {
                                if (indexNumber != editableFieldPosition.PageFieldIndex)
                                {
                                    string type = fieldNode.Attributes["pl"]!=null ? fieldNode.Attributes["pl"].Value:"";
                                    if (!string.IsNullOrEmpty(type) && type.ToLower().Contains("d.data"))
                                    {
                                        fieldNode = null;
                                    }
                                }

                                if (fieldNode != null)
                                {
                                    XmlNode node = Items.Where(d => d["ComponentName"] != null && d["ComponentName"].InnerText == editableFieldPosition.FieldName && d["PageIndex"] != null && d["PageIndex"].InnerText == index && d["Position"] != null && d["Position"].InnerText == editableFieldPosition.FieldPosition).FirstOrDefault();
                                    if (node != null)
                                    {
                                        itemCount -= 1;
                                        if (ItemsList[0].Attributes["count"] != null && !string.IsNullOrEmpty(ItemsList[0].Attributes["count"].Value))
                                        {
                                            ItemsList[0].Attributes["count"].Value = itemCount.ToString();
                                        }

                                        ItemsList[0].RemoveChild(node);
                                    }
                                }

                            }
                        }


                    }


                }

                #endregion

                #region Add Items
                string newNodestring = "";
                int indexnode = 0;
                int countList = 0;
                if (childNodes != null)
                {
                    indexnode = childNodes.Count + 1;
                    countList = childNodes.Count;
                }
                else indexnode += 1;



                string AddedNode = "";
                editableFieldLists = editableFieldLists.Where(d => !d.ReturnToOriginValue).ToList();

                if (editableFieldLists != null && editableFieldLists.Count > 0)
                {
                    foreach (EditableFieldPosition field in editableFieldLists)
                    {
                        int indexNumber = -1;
                        foreach (XmlNode item in pageNodeLists)
                        {
                            indexNumber += 1;
                            string index = indexNumber.ToString();

                            XmlNode fieldNode = (new System.Collections.Generic.List<XmlNode>(Shim<XmlNode>(item.ChildNodes))).Where(d => (d.Attributes["name"] != null && d.Attributes["name"].Value == field.FieldName) || (d.Attributes["type"] != null && d.Attributes["type"].Value.ToLower() == "checkbox" && d.Name == field.FieldName)).FirstOrDefault();

                            if (fieldNode!=null) 
                            {
                                if (indexNumber != field.PageFieldIndex)
                                {
                                    string type = fieldNode.Attributes["pl"] != null ? fieldNode.Attributes["pl"].Value : "";
                                    if (!string.IsNullOrEmpty(type) && type.ToLower().Contains("d.data"))
                                    {
                                        fieldNode = null;
                                    }
                                }

                                if (fieldNode!=null)
                                {
                                    while (currentEditableFields.Contains("<Item" + indexnode.ToString()) || AddedNode.Contains("<Item" + indexnode.ToString()))
                                    {
                                        indexnode += 1;
                                    }

                                    newNodestring = "<Item" + indexnode.ToString() + "  Ref=\"" + indexnode.ToString() + "\" type=\"Stimulsoft.Report.StiEditableItem\" isKey=\"true\">\r\n      <ComponentName>" + field.FieldName + "</ComponentName>\r\n <PageIndex>" + index + "</PageIndex>\r\n  <Position>" + field.FieldPosition + "</Position>\r\n <TextValue>" + "</TextValue>\r\n    </Item" + indexnode.ToString() + ">\r\n ";
                                    AddedNode += newNodestring;
                                    XmlTextReader textReader = new XmlTextReader(new StringReader(newNodestring));
                                    XmlNode newNode = messageDoc.ReadNode(textReader);
                                    newNode["TextValue"].InnerText = field.NewValue;
                                    ItemsList[0].AppendChild(newNode);

                                    countList += 1;
                                    if (ItemsList[0].Attributes["count"] != null) ItemsList[0].Attributes["count"].Value = countList.ToString();

                                }

                            }

                        }






                    }

                }

                #endregion

                #region Save Items

                if (!string.IsNullOrEmpty(messageDoc.InnerXml))
                {
                    if (documentOut != null)
                    {
                        System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                        result = enc.GetBytes(messageDoc.InnerXml);
                        documentOut.EditableFields = result;
                        documentOutRepository.Update(documentOut);
                        documentOutRepository.SubmitChanges();
                    }
                }
                #endregion
            }

                return result;
           
        }

        public static IEnumerable<T> Shim<T>(System.Collections.IEnumerable enumerable)
        {
            foreach (object current in enumerable)
            {
                yield return (T)current;
            }
        }


        private string getBetween(string strSource, string strStart, string strEnd)
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


        public HttpResponseMessage GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(int tenant, string documentTypeId, bool isfilter, int mytenant)
        {
            try
            {
                Authentication();

                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                List<DocumentTypeTemplateList> documentTypeTemplateLists = documentTypeTemplateQuery.GetDocumentTypeTemplateFromLibraryByTenant(tenant, documentTypeId, mytenant, isfilter);

                return Request.CreateResponse(HttpStatusCode.OK, documentTypeTemplateLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

     

        public HttpResponseMessage GetDocumentTypeTemplatesFromLibrary(string objecttableid, int tenant, bool isfilter, string transportModeId, string shipmentLevelCode)
        {
            try
            {
                Authentication();
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                List<DocumentTypeTemplateList> documentTypeTemplateLists = documentTypeTemplateQuery.GetDocumentTypeTemplatesByObjectTableId(objecttableid, tenant, transportModeId, shipmentLevelCode, isfilter);
           
                return Request.CreateResponse(HttpStatusCode.OK, documentTypeTemplateLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetCopyDocumentTypeAndDocumentTypTemplate(string docmentTypeTemplateId, int tenant)
        {
            try
            {
                DocumentTypePM documentType = null;
                string CopyDocumentTypeCode = "";
                DocumentTypeCopyRepository theDocumentTypeCopyRepository = new DocumentTypeCopyRepository(tenant);
                DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(tenant);

                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);

                DocumentTypeTemplate documentTypeTemplate = documentTypeTemplateRepository.GetSingleDocumentTypeTemplateByTenant(docmentTypeTemplateId, 0);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);

                DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);

                if (documentTypeTemplate != null && documentTypeTemplate.DocumentType != null)
                {
                    documentType = documentTypeQuery.GetSinglePMByCodeAndTenant(documentTypeTemplate.DocumentType.Code, 0);
                }

                if (documentType != null)
                {
                    DocumentType itemDocumentType = new DocumentType()
                    {
                        Id = IdCounter.GetNumber("DocumentType", tenant).ToString(),
                        Tenant = tenant,
                        Name = documentType.Name,
                        Code = documentType.Code,
                        Notes = documentType.Notes,
                        IsAir = documentType.IsAir,
                        IsOcean = documentType.IsOcean,
                        IsInland = documentType.IsInland,
                        IsDocIn = documentType.IsDocIn,
                        IsDocOut = documentType.IsDocOut,
                        ObjectTableId = documentType.ObjectTableId,
                        Subject = documentType.Subject,
                        TemplateFormatCode = documentType.TemplateFormatCode,
                        InActive = false,
                        IsMaster = documentType.IsMaster,
                        IsDirect = documentType.IsDirect,
                        IsHouse = documentType.IsHouse,
                        SearchFields = documentType.SearchFields,
                        CustomControl = documentType.CustomControl,
                        CustomerRoleId = documentType.CustomerRoleId,
                        AgentRoleId = documentType.AgentRoleId,
                        IsCustomerView = documentType.IsCustomerView,
                        IsAgentView = documentType.IsAgentView,
                        IsReadOnly = documentType.IsReadOnly,
                        LimitedPrintCopyId = documentType.LimitedPrintCopyId,
                        IsDocumentOneTimePrintLimited = documentType.IsDocumentOneTimePrintLimited,
                        IsCopiedAtSignup = true,
                        IsEnabledForCustomers = true,
                        CountryCode = documentType.CountryCode,
                        IsSystemAdditionalPrintingFields = documentType.IsSystemAdditionalPrintingFields,
                        PrintingFieldsScreenCode = documentType.PrintingFieldsScreenCode,
                        DocumentTypeCategoryCode = documentType.DocumentTypeCategoryCode,


                    };


                    CopyDocumentTypeCode = itemDocumentType.Code;

                    foreach (DocumentTypeCopyPM copy in documentType.DocumentTypeCopies)
                    {


                        DocumentTypeCopy newCopy = new DocumentTypeCopy()
                        {
                            Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                            Code = copy.Code,
                            Name = copy.Name,
                            Tenant = tenant,
                            IndexOrder = copy.IndexOrder,
                            IsSelectedByDefault = copy.IsSelectedByDefault,
                            DocumentTypeId = itemDocumentType.Id,

                        };
                        theDocumentTypeCopyRepository.Add(newCopy);

                    }


                    IQueryable<DocumentTypeCustomField> zeroCustomFields = documentTypeCustomFieldRepository.GetDocumentTypeCusotmFieldsByDocumentTypeId(itemDocumentType.Id, 0);
                    foreach (DocumentTypeCustomField customField in zeroCustomFields)
                    {
                        DocumentTypeCustomField newCustomField = new DocumentTypeCustomField()
                        {
                            DocumentTypeId = itemDocumentType.Id,
                            DefaultValue = customField.DefaultValue,
                            FieldCode = customField.FieldCode,
                            FieldDataTypeCode = customField.FieldDataTypeCode,
                            Id = IdCounter.GetNumber("DocumentTypeCustomField", tenant).ToString(),
                            InActive = false,
                            IndexOrder = customField.IndexOrder,
                            IsRequired = customField.IsRequired,
                            MultiLine = customField.MultiLine,
                            Name = customField.Name,
                            Tenant = tenant,
                        };
                        documentTypeCustomFieldRepository.Add(newCustomField);
                    }

                    documentTypeRepository.Add(itemDocumentType);

                    DocumentTypeTemplate itemDocumentTypeTemplate = new DocumentTypeTemplate()
                    {
                        Description = documentTypeTemplate.Description,
                        DocumentTypeId = itemDocumentType.Id,
                        Id = IdCounter.GetNumber("DocumentTypeTemplate", tenant).ToString(),
                        LastUpdateDate = documentTypeTemplate.LastUpdateDate,
                        LastUpdatedByUserId = documentTypeTemplate.LastUpdatedByUserId,
                        TemplateBody = documentTypeTemplate.TemplateBody,
                        TemplateBodyHtml = documentTypeTemplate.TemplateBodyHtml,
                        TemplateBodyjson = documentTypeTemplate.TemplateBodyjson,
                        TemplateType = documentTypeTemplate.TemplateType,
                        Tenant = tenant,
                        InActive = false,
                        EditorTool = documentTypeTemplate.EditorTool,
                        HorizontalShift = documentTypeTemplate.HorizontalShift,
                        VerticalShift = documentTypeTemplate.VerticalShift,
                        Subject = documentTypeTemplate.Subject,
                        CountryCode = documentTypeTemplate.CountryCode,
                        InternalRemarks = documentTypeTemplate.InternalRemarks,
                        Language = documentTypeTemplate.Language,
                        OriginalTemplateId = documentTypeTemplate.Id,
                        IsCopiedAtSignup = true,
                        IsEnabledForCustomers = true,

                    };
                    documentTypeTemplateRepository.Add(itemDocumentTypeTemplate);



                    if (itemDocumentTypeTemplate.TemplateType == "M")
                    {
                        itemDocumentType.DocumentTypeDefaultHTMLTemplateId = itemDocumentTypeTemplate.Id;
                        itemDocumentType.TemplateFormatCode = "M";

                        itemDocumentType.DocumentTypeDefaultEditorTool = itemDocumentTypeTemplate.EditorTool;
                    }
                    else
                    {
                        itemDocumentType.DocumentTypeDefaultReportTemplateId = itemDocumentTypeTemplate.Id;
                        itemDocumentType.DocumentTypeDefaultEditorTool = itemDocumentTypeTemplate.EditorTool;
                        itemDocumentType.TemplateFormatCode = "P";

                    }



                    documentTypeRepository.SubmitChanges();
                    theDocumentTypeCopyRepository.SubmitChanges();
                    documentTypeCustomFieldRepository.SubmitChanges();
                    documentTypeTemplateRepository.SubmitChanges();

                    TableLastUpdateClass.UpdateTableHistory(tenant, "DocumentType");

                }
                return Request.CreateResponse(HttpStatusCode.OK, CopyDocumentTypeCode);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetSingleDocumentTypeTemplate(string id, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                DocumentTypeTemplatePM documentTypeTemplateLists = documentTypeTemplateQuery.GetSinglePM(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, documentTypeTemplateLists);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }




        public HttpResponseMessage GetDocumentTypeTemplatesByDocumentTypeIdForAutomations(string documentTypeId,string editorToolCode, int tenant)
        {
            try
            {
                Authentication();
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                List<DocumentTypeTemplatePM> result = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeIdForAutomation(documentTypeId, editorToolCode, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }





        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("DocumentTypeTemplate", "READ", authToken.Tenant);
        }



        public HttpResponseMessage GetTemplateBodyByDocumentTemplateId(string documentTypetemplateId, int tenant)
        {
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);

            byte[] data = documentTypeTemplateQuery.GetTemplateBodyByDocumentTypeTemplateId(documentTypetemplateId, tenant);
            byte[] jsonData = documentTypeTemplateQuery.GetTemplateBodyjsonByDocumentTypeTemplateId(documentTypetemplateId, tenant);
            string result = "";
            var response = Request.CreateResponse(HttpStatusCode.OK);
            if (data != null)
            {
                //result = System.Text.Encoding.UTF8.GetString(data);
                var report = new StiReport();
                if (jsonData != null)
                {
                    result = System.Text.Encoding.UTF8.GetString(jsonData);


                    //report.LoadFromJson(jsonString);
                }
                else
                {
                    report.Load(data);
                    result = report.SaveToJsonString();
                }
               // report.LoadFromJson
                
            }

            response.Headers.Add("ContentType", "application/json"); response.Headers.Add("ContentType", "UTF8");
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");
            return response;
        }

        public HttpResponseMessage GetTemplateBodyByDocumentTemplateId2(string documentTypetemplateId, int tenant)
        {
            DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);

            byte[] data = documentTypeTemplateQuery.GetTemplateBodyByDocumentTypeTemplateId(documentTypetemplateId, tenant);
            
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }


        public HttpResponseMessage GetDocumentTypeTemplatesDefultAttachments(string documentTypeTemplateId,string objectTableId , string entityId, string childEntityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("DocumentTypeTemplate", "READ", authToken.Tenant);

                DocumentTypeTemplateDefultAttachmentService documentTypeTemplateDefultAttachmentService = new DocumentTypeTemplateDefultAttachmentService();
                var attachments = documentTypeTemplateDefultAttachmentService.GetDefultAttachmentList(new DocumentTypeTemplateDefultAttachmentArgs() { DocumentTypeTemplateId = documentTypeTemplateId, EntityId = entityId, ObjectTableId = objectTableId, Tenant = authToken.Tenant , ChildEntityId = childEntityId });


                return Request.CreateResponse(HttpStatusCode.OK, attachments);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }






        //public HttpResponseMessage GetTemplateBodyhtmlOrJsonByDocumentTemplateId(string documentTyptemplateId, int tenant, bool isHtml)
        //{
        //    DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
        //    byte[] data = null;
        //    string result = "";
        //    if (isHtml)
        //    {
        //        data = documentTypeTemplateQuery.GetTemplateBodyHtmlByDocumentTypeTemplateId(documentTyptemplateId, tenant);
        //    }
        //    else
        //    {
        //        data = documentTypeTemplateQuery.GetTemplateBodyjsonByDocumentTypeTemplateId(documentTyptemplateId, tenant);
        //    }

        //    if (data != null)
        //    {
        //        result = System.Text.Encoding.UTF8.GetString(data);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

    }
}
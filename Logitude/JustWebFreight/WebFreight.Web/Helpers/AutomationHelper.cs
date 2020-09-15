using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class AutomationHelper
    {
        public void ExecuteEmailAutomation(EntityChange entityChange, List<Field> AutomationConditionFieldLists, Automation automation, EntityChangeAutomation entityChangesAutomation, List<EntityChangeAutomation> entityChangesAutomationsSsucceedList, string objectTableName = null)
        {

            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(automation.Tenant);
            List<AutomationResultEmailRecipientList> automationResultEmailRecipientLists = automationResultEmailRecipientQuery.GetAutomationResultEmailRecipientListsByAutomationId(automation.Id, automation.Tenant).Where(d=>!string.IsNullOrEmpty(d.RecipientValue)).ToList();
          
            var automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
            bool allActiveUsers = automatedBackup.IsAutomationResultEmailAllActiveUsers;

            //  #region Send Email Prosess

            if (automationResultEmailRecipientLists.Count > 0 || allActiveUsers)
            {
                if (!string.IsNullOrEmpty(automation.TemplateId))
                {
                    HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();
                    DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(entityChange.Tenant);
                    DocumentTypeTemplate template = documentTypeTemplateRepository.GetSingleDocumentTypeTemplateWithOutInClude(automation.TemplateId, automation.Tenant);


                    if (template != null)
                    {
                        #region Get TemplateHtml

                        string subject = template.Subject;
                        string from = template.From;
                        string replyTo = template.ReplyTo;
                        string cc = template.CC;
                        string bcc = template.BCC;
                        string userId = template.LastUpdatedByUserId;
                        if (!string.IsNullOrEmpty(entityChange.CreateByUserId)) userId = entityChange.CreateByUserId;
                        byte[] htmldata = null;
                        try
                        {

                            string html = htmlEditorHelper.GetEditorHtmlData("", entityChange.EntityId, entityChange.ObjectTableId, "", "", entityChange.Tenant, userId, true, template.Id, ref subject, ref from, ref replyTo, ref cc, ref bcc, "", template);

                            if (!string.IsNullOrEmpty(html))
                            {
                                var start = "<html><head><meta http- equiv='Content- Type' content= 'text/html; charset = iso-8859-1' > <style type='text/css' style= 'display: none; '></style></head><body>";
                                var end = "</body></html>";
                                html = start + html + end;
                                html = htmlEditorHelper.GetLogoHtmlString(html);

                            }
                            else html = "";

                            htmldata = Encoding.UTF8.GetBytes(html);

                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.HandleException(ex, DateTime.Now, automation.Tenant, "", "EntityChangeWorkerRole", "", null);
                        }

                        #endregion

                        #region  Automation Email Recipient

                        List<string> contactIds = new List<string>();
                        List<string> notifyBackContactIds = new List<string>();
                        List<string> notifyBackPartners = new List<string>();
                        string Emails = "";
                        string NotifyBackEmails = "";

                        if (allActiveUsers)
                        {
                            UserQuery userQuery = new UserQuery(entityChange.Tenant);
                            contactIds = userQuery.GetUserIdsByTenant(entityChange.Tenant);
                        }


                        foreach (AutomationResultEmailRecipientList automationResultEmail in automationResultEmailRecipientLists)
                        {
                            if (automationResultEmail.IsNotifyBack)
                            {
                                if (automationResultEmail.RecipientType == "Fixed") {
                                    if (!notifyBackContactIds.Contains(automationResultEmail.RecipientValue)) notifyBackContactIds.Add(automationResultEmail.RecipientValue);
                                }
                                else
                                {
                                    Field entityContactVariable = AutomationConditionFieldLists.Where(d => d.FieldCode == automationResultEmail.RecipientValue).FirstOrDefault();
                                    if (!notifyBackContactIds.Contains(entityContactVariable.Value)) notifyBackContactIds.Add(entityContactVariable.Value);
                                }
                            }
                            else if (automationResultEmail.RecipientType == "Fixed")
                            {
                                if (!contactIds.Contains(automationResultEmail.RecipientValue)) contactIds.Add(automationResultEmail.RecipientValue);
                            }
                            else
                            {
                                if (AutomationConditionFieldLists != null)
                                {
                                    Field entityContactVariable = AutomationConditionFieldLists.Where(d => d.FieldCode == automationResultEmail.RecipientValue).FirstOrDefault();
                                    if (entityContactVariable != null)
                                    {
                                        if (automationResultEmail.RecipientType == "Emails")
                                        {
                                            if (!Emails.Split(';').Contains(entityContactVariable.Value)) Emails += entityContactVariable.Value + ";";
                                        }
                                        else
                                        {
                                            if (!contactIds.Contains(entityContactVariable.Value)) contactIds.Add(entityContactVariable.Value);
                                            if (string.IsNullOrEmpty(entityContactVariable.Value))
                                            {
                                                if (!notifyBackPartners.Contains(entityContactVariable.PropertyName))
                                                    notifyBackPartners.Add(entityContactVariable.PropertyName);
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        if (contactIds.Count > 0)
                        {
                            ContactQuery contactQuery = new ContactQuery(automation.Tenant);
                            List<string> contactEmailLists = contactQuery.GetContactEmailsListsByIds(contactIds, automation.Tenant);
                            foreach (string contactEmail in contactEmailLists)
                            {
                                if (!string.IsNullOrEmpty(contactEmail))
                                {
                                    if (!Emails.Split(';').Contains(contactEmail)) Emails += contactEmail + ";";
                                }
                             
                            }

                        }

                        if (notifyBackContactIds.Count > 0 && notifyBackPartners.Count > 0)
                        {
                            ContactQuery contactQuery = new ContactQuery(automation.Tenant);
                            List<string> notifyBackContactEmailLists = contactQuery.GetContactEmailsListsByIds(notifyBackContactIds, automation.Tenant);
                            foreach (string contactEmail in notifyBackContactEmailLists)
                            {
                                if (!string.IsNullOrEmpty(contactEmail))
                                {
                                    if (!NotifyBackEmails.Split(';').Contains(contactEmail)) NotifyBackEmails += contactEmail + ";";
                                }

                            }

                        }

                        #endregion

                        if (!string.IsNullOrEmpty(Emails) && htmldata != null)
                        {
                            string communicationLog = AddAutomationToQueue(automation, entityChange, htmldata, Emails, from, replyTo, cc,bcc, subject);
                            entityChangesAutomation.ComunicationLogId = communicationLog;
                        }

                        if (!string.IsNullOrEmpty(NotifyBackEmails))
                        {
                            EmailCommunicationParams emailParams = BuildNotifyBackEmailCommunications(automation, notifyBackPartners, NotifyBackEmails);
                            Communications.AddEmailCommunicationLogQueue(emailParams, automation.Tenant);
                        }
                    }


                    entityChange.HasExecutedRecord = true;
                    entityChangesAutomation.IsConditionTrue = true;
                    entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(automation.Tenant);
                    entityChangesAutomationsSsucceedList.Add(entityChangesAutomation);


                }
            }

            //  #endregion
        }

        private EmailCommunicationParams BuildNotifyBackEmailCommunications(Automation automation, List<string> notifyBackPartners, string NotifyBackEmails)
        {
            string objectTableeName = GetObjectTableName(automation);
            string emailBody = GetAutomationNotifyBackEmailBody(automation.Name, objectTableeName, notifyBackPartners);
            StringBuilder HtmlTemplate = new StringBuilder();
            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append(emailBody);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("<br /><br />");

            string emailbody = HtmlTemplate.ToString();
            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                From = "no-replay@LogitudeWorld.com",
                To = NotifyBackEmails,
                CC = "",
                BCC = "",
                Subject = "Automation " + automation.Name + "Failed",
                EmailBody = emailbody,
                Tenant = automation.Tenant,
            };
            return emailParams;
        }

        private string GetObjectTableName(Automation automation)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(automation.Tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableById(automation.ObjectTableId, automation.Tenant);
            string objectTableeName = objectTable == null ? "" : objectTable.Name;
            return objectTableeName;
        }

        private string GetAutomationNotifyBackEmailBody(string automationName, string objectTableeName, List<string> notifyBackPartners)
        {
            string emailString = "";
            emailString += "Automation " + automationName + " failed to be sent to the following ";
            emailString += "partner" + (notifyBackPartners.Count > 1 ? "s " : " ") + string.Join(",", notifyBackPartners.ToArray());
            emailString += " since they are not defined in " + objectTableeName + " level ";
            return emailString;
        }

        public string AddAutomationToQueue(Automation automation , EntityChange  entityChange ,  byte[] htmlData, string toEmail,  string from, string replyTo, string cc, string bcc, string subject, string objectTableName = null)
        {
            string entityId = entityChange.EntityId;
            string objectTableId = entityChange.ObjectTableId;
            string userId = entityChange.CreateByUserId;
            int tenant = entityChange.Tenant;
            string automationId = automation.Id;
            string documentOutId = null;

            if (string.IsNullOrEmpty(objectTableName))
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableById(objectTableId, tenant);
                if (objectTable != null) objectTableName = objectTable.Name;
            }
  
            if(objectTableName == "Shipment")
            {
                #region Document Out
                DocumentOutRepository documentOutRepository = new DocumentOutRepository(automation.Tenant);
                documentOutId = documentOutRepository.GetDocumentOutIdByDocumentTypeIdAndEntityId(entityId, automation.DocumentTypeId, objectTableId, automation.Tenant);
                if (string.IsNullOrEmpty(documentOutId))
                {
                    DocumentHelper documentHelper = new DocumentHelper();
                    DocumentOutPM documentOutPM = documentHelper.CreateDocumentOut(automation.DocumentTypeId, entityId, "", "", objectTableId, tenant, userId);
                    if (documentOutPM != null) documentOutId = documentOutPM.Id;
                }

                if (string.IsNullOrEmpty(documentOutId)) documentOutId = null;

                #endregion
            }

            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "html",
                FileSize = Convert.ToInt32(htmlData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Folder = "others",
            };

            DocumentRepository documentRep = new DocumentRepository(tenant);
            documentRep.Add(document);
            documentRep.SubmitChanges();
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
                DocumentOutId = documentOutId,
                DocumentsFilingId = null,
                Subject = subject,
                Tenant = tenant,
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
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

            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(tenant);
            communicationLogRepository.Add(log);
            communicationLogRepository.SubmitChanges();

			//IQueueService queueservice = QueueServiceManager.GetQueueService("EmailQueue", tenant);
			DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
			queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } }, tenant);

            return log.Id;
        }

        public void CopyAutomationFromTenantZeroToMyTenant(int tenant, List<DocumentTypePM> tenantZeroDocumentTypePms)
        {
            List<AutomationDocumentTypeClass> myAutomationDocumentTypeClassLists = new List<AutomationDocumentTypeClass>();
            List<Automation> automationLists = GetAutomationsFromTenantZero(tenant);

            if (automationLists != null)
            {
                List<AutomationDocumentTypeClass> automationDocumentTypeLists = BuildAutomationDocumentTypeList(tenant, tenantZeroDocumentTypePms, automationLists);
                List<AutomationResultEmailRecipientPM> automationResultEmailRecipientPMList = GetAutomationResultEmailRecipientPMList(automationLists);
                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                AutomationService service = new AutomationService(MyContext, tenant);
                int OnCreateAutomationOrder = GetLastAutomationOrder(tenant, "OnCreate");
                int OnUpdateAutomationOrder = GetLastAutomationOrder(tenant, "OnUpdate");

                foreach (Automation automation in automationLists)
                {
                    AutomationPM newAutomationPM = CreateNewAutomation(tenant, automation);
                    newAutomationPM.AutomationResultEmailRecipientLists = automationResultEmailRecipientPMList.Where(d => d.AutomationsId == automation.Id).ToList();

                    AutomationDocumentTypeClass automationDocumentType = automationDocumentTypeLists.Where(d => d.AutomationCode == automation.Code).FirstOrDefault();
                    if (automationDocumentType != null)
                    {
                        newAutomationPM.DocumentTypeId = automationDocumentType.DocumentTypeId;
                        newAutomationPM.TemplateId = automationDocumentType.DocumentTypeTemplateId;
                    }

                   if(automation.Type == "OnCreate")
                    {
                        OnCreateAutomationOrder += 1;
                        automation.Order = OnCreateAutomationOrder;
                    }
                   else if (automation.Type == "OnUpdate")
                    {
                        OnUpdateAutomationOrder += 1;
                        automation.Order = OnUpdateAutomationOrder;
                    }
                    if (LogitudeSettings.DeploymentStage != "logboxwe1") {
                        newAutomationPM.Inactive = true;
                    }
                    service.Create(newAutomationPM);

                }
            }

        }

        private int GetLastAutomationOrder(int tenant , string type)
        {
            AutomationRepository automationRepository = new AutomationRepository(tenant);
            int order = automationRepository.GetAutomations(tenant).Where(d => d.Type == type).Count();
            return order;

        }

        private List<AutomationResultEmailRecipientPM> GetAutomationResultEmailRecipientPMList(List<Automation> automationLists)
        {
            List<string> automationIds = automationLists.GroupBy(d => d.Id).Select(d => d.First().Id).ToList();

            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(0);
           return automationResultEmailRecipientQuery.GetAutomationResultEmailRecipientPMsByAutomationIds(automationIds,0);
        }

        private AutomationPM  CreateNewAutomation(int tenant, Automation automation)
        {
            return new AutomationPM()
            {
                Tenant = tenant,
                Name = automation.Name,
                Code = automation.Code,
                Description = automation.Description,
                From = automation.From,
                FromEmail = automation.FromEmail,
                Inactive = automation.Inactive,
                Version = 1,
                Type = automation.Type,
                ResultCode = automation.ResultCode,
                Order = automation.Order,
                AutomationXML = automation.AutomationXML,
                ObjectTableId = automation.ObjectTableId,
            };
        }

        private List<AutomationDocumentTypeClass> BuildAutomationDocumentTypeList(int tenant, List<DocumentTypePM> tenantZeroDocumentTypePms, List<Automation> automationLists)
        {
            List<AutomationDocumentTypeClass> myAutomationDocumentTypeClassLists = new List<AutomationDocumentTypeClass>();

            if (automationLists.Count > 0)
            {
                DocumentTypeTemplateQuery documentTypeTemplateQuery = new DocumentTypeTemplateQuery(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(tenant);

                List<AutomationDocumentTypeClass> tenantZeroAutomationDocumentTypeClassLists = FillAutomationDocumentTypeList(automationLists, tenantZeroDocumentTypePms);
                List<string> tenantZeroDocumentTypeIdsUsedInAutomation = tenantZeroAutomationDocumentTypeClassLists.GroupBy(d => d.DocumentTypeId).Select(d => d.First().DocumentTypeId).ToList();
                List<string> tenantZeroDocumentTypeCodesUsedInAutomation = tenantZeroAutomationDocumentTypeClassLists.GroupBy(d => d.DocumentTypeCode).Select(d => d.First().DocumentTypeCode).ToList();
                List<string> tenantZeroDocumentTypeTemplateIdsUsedInAutomation = tenantZeroAutomationDocumentTypeClassLists.GroupBy(d => d.DocumentTypeTemplateId).Select(d => d.First().DocumentTypeTemplateId).ToList();

                List<DocumentTypePM> tenantZeroDocumentTypePmsUsedInAutomation = tenantZeroDocumentTypePms.Where(d => tenantZeroDocumentTypeIdsUsedInAutomation.Contains(d.Id)).ToList();
                List<DocumentTypeTemplatePM> tenantZeroDocumentTypeTemplatePmsUsedInAutomation = documentTypeTemplateQuery.GetDocumentTypeTemplatePMsByTenant(0).Where(d => tenantZeroDocumentTypeTemplateIdsUsedInAutomation.Contains(d.Id)).ToList();
                List<DocumentTypePM> myDocumentTypeListsUsedInAutomation = documentTypeQuery.GetDocumentTypePMsListsByCodes(tenantZeroDocumentTypeCodesUsedInAutomation, tenant).ToList();
                var ids = myDocumentTypeListsUsedInAutomation.Select(d => d.Id).ToList();
                List<DocumentTypeTemplatePM> myDocumentTypeTempaltesUsedInAutomation = documentTypeTemplateQuery.GetDocumentTypeTemplatesByDocumentTypeIds(ids, tenant).ToList();

                bool isChange = false;
                foreach (AutomationDocumentTypeClass automationDocumentTypeClass in tenantZeroAutomationDocumentTypeClassLists)
                {
                    #region DocumentType
                    DocumentType newDocType = null;
                    DocumentTypePM myDocType = myDocumentTypeListsUsedInAutomation.Where(d => d.Code == automationDocumentTypeClass.DocumentTypeCode).FirstOrDefault();
                    if (myDocType == null)
                    {
                        var docType = tenantZeroDocumentTypePmsUsedInAutomation.Where(d => d.Id == automationDocumentTypeClass.DocumentTypeTemplateId).FirstOrDefault();
                        if (docType != null)
                        {
                            newDocType = CreateNewDocumentType(docType, documentTypeRepository, tenant);

                            myDocType = new DocumentTypePM() { Code = newDocType.Code, Id = newDocType.Id, Tenant = newDocType.Tenant };
                            myDocumentTypeListsUsedInAutomation.Add(myDocType);
                        }
                        isChange = true;
                    }
                    #endregion

                    #region DocumentTypeTemplate
                    DocumentTypeTemplatePM myDocumentTypeTemplatePM = myDocumentTypeTempaltesUsedInAutomation.Where(d => d.OriginalTemplateId == automationDocumentTypeClass.DocumentTypeTemplateId).FirstOrDefault();
                    if (myDocumentTypeTemplatePM == null && myDocType != null)
                    {
                        isChange = true;
                        var tenantZeroDocumentTypeTemplate = tenantZeroDocumentTypeTemplatePmsUsedInAutomation.Where(d => d.Id == automationDocumentTypeClass.DocumentTypeTemplateId).FirstOrDefault();
                        DocumentTypeTemplate newtemplate = CreateNewDocumentTypeTemplate(documentTypeTemplateRepository, myDocType, tenantZeroDocumentTypeTemplate);
                        documentTypeTemplateRepository.Add(newtemplate);
                        myDocumentTypeTemplatePM = new DocumentTypeTemplatePM() { Id = newtemplate.Id, DocumentTypeId = newtemplate.DocumentTypeId, OriginalTemplateId = newtemplate.OriginalTemplateId,Tenant = newtemplate.Tenant };
                        myDocumentTypeTempaltesUsedInAutomation.Add(myDocumentTypeTemplatePM);

                        if (newDocType!=null && string.IsNullOrEmpty(newDocType.DocumentTypeDefaultEditorTool) )
                        {
                            newDocType.DocumentTypeDefaultEditorTool = newtemplate.Id;
                        }
                    }
                    #endregion
                    if (myDocType != null)
                    {
                        AutomationDocumentTypeClass automationDocumentType = CreateNewAutomationDocumentTypeClass(automationDocumentTypeClass, myDocType, myDocumentTypeTemplatePM);
                        myAutomationDocumentTypeClassLists.Add(automationDocumentType);
                    }

                }

                if (isChange)
                {
                    documentTypeRepository.SubmitChanges();
                    documentTypeTemplateRepository.SubmitChanges();

                }


                TableLastUpdateClass.UpdateTableHistory(tenant, "DocumentType");
                TableLastUpdateClass.UpdateTableHistory(tenant, "DocumentTypeTemplate");
            }

            return myAutomationDocumentTypeClassLists;
        }

        private static AutomationDocumentTypeClass CreateNewAutomationDocumentTypeClass( AutomationDocumentTypeClass automationDocumentTypeClass, DocumentTypePM myDocType, DocumentTypeTemplatePM myDocumentTypeTemplatePM)
        {

            AutomationDocumentTypeClass automationDocumentType = new AutomationDocumentTypeClass();
            automationDocumentType.AutomationCode = automationDocumentTypeClass.AutomationCode;
            automationDocumentType.DocumentTypeId = myDocType != null ? myDocType.Id : null;
            automationDocumentType.DocumentTypeCode = myDocType != null ? myDocType.Code : null;
            automationDocumentType.DocumentTypeTemplateId = myDocumentTypeTemplatePM != null ? myDocumentTypeTemplatePM.Id : null;
            automationDocumentType.Tenant = myDocType.Tenant;
            return automationDocumentType;
        }

        private static DocumentTypeTemplate CreateNewDocumentTypeTemplate( DocumentTypeTemplateRepository documentTypeTemplateRepository, DocumentTypePM newDocType, DocumentTypeTemplatePM tenantZeroDocumentTypeTemplate)
        {

            DocumentTypeTemplate newtemplate = new DocumentTypeTemplate()
            {
                Id = IdCounter.GetNumber("DocumentTypeTemplate", newDocType.Tenant).ToString(),
                Tenant = newDocType.Tenant,
                TemplateBody = tenantZeroDocumentTypeTemplate.TemplateBody,
                TemplateType = tenantZeroDocumentTypeTemplate.TemplateType,
                HorizontalShift = tenantZeroDocumentTypeTemplate.HorizontalShift,
                InActive = tenantZeroDocumentTypeTemplate.InActive,
                Description = tenantZeroDocumentTypeTemplate.Description,
                DocumentTypeId = newDocType.Id,
                EditorTool = tenantZeroDocumentTypeTemplate.EditorTool,
                CountryCode = tenantZeroDocumentTypeTemplate.CountryCode,
                Subject = tenantZeroDocumentTypeTemplate.Subject,
                Language = tenantZeroDocumentTypeTemplate.Language,
                OriginalTemplateId = tenantZeroDocumentTypeTemplate.Id,
                VerticalShift = tenantZeroDocumentTypeTemplate.VerticalShift,
                InternalRemarks = tenantZeroDocumentTypeTemplate.InternalRemarks,
                IsEnabledForCustomers = true,
                TemplateBodyHtml = tenantZeroDocumentTypeTemplate.TemplateBodyHtml,
                TemplateFooterHtml = tenantZeroDocumentTypeTemplate.TemplateFooterHtml,
                TemplateHeaderHtml = tenantZeroDocumentTypeTemplate.TemplateHeaderHtml,
                TemplateFooterHeight = tenantZeroDocumentTypeTemplate.TemplateFooterHeight,
                TemplateHeaderHeight = tenantZeroDocumentTypeTemplate.TemplateHeaderHeight,
                CC = tenantZeroDocumentTypeTemplate.CC,
                From = tenantZeroDocumentTypeTemplate.From,
                ReplyTo = tenantZeroDocumentTypeTemplate.ReplyTo,
            };

            documentTypeTemplateRepository.Add(newtemplate);

            return newtemplate;
        }

        private static DocumentType CreateNewDocumentType( DocumentTypePM docType , DocumentTypeRepository documentTypeRepository , int tenant)
        {
            DocumentType newDocType =  new DocumentType()
            {
                Id = IdCounter.GetNumber("DocumentType", tenant).ToString(),
                Code = docType.Code.Trim(),
                Name = docType.Name,
                IsOcean = docType.IsOcean,
                IsAir = docType.IsAir,
                IsInland = docType.IsInland,
                IsDocIn = docType.IsDocIn,
                IsDocOut = docType.IsDocOut,
                FollowUpTypeId = docType.FollowUpTypeId,
                Tenant = tenant,
                ObjectTableId = docType.ObjectTableId,
                SearchFields = docType.SearchFields,
                IsMaster = docType.IsMaster,
                IsDirect = docType.IsDirect,
                IsHouse = docType.IsHouse,
                TemplateFormatCode = docType.TemplateFormatCode,
                IsCustomerView = docType.IsCustomerView,
                IsAgentView = docType.IsAgentView,
                DocumentTypeCategoryCode = docType.DocumentTypeCategoryCode,
                CountryCode = docType.CountryCode,
                Subject = docType.Subject,
                IsEnabledForCustomers = true,
                Notes = docType.Notes,
                DocumentTypeDefaultHTMLTemplateId = docType.DocumentTypeDefaultHTMLTemplateId,
                DocumentTypeDefaultReportTemplateId = docType.DocumentTypeDefaultReportTemplateId,
                IsSystemAdditionalPrintingFields = docType.IsSystemAdditionalPrintingFields,
                PrintingFieldsScreenCode = docType.PrintingFieldsScreenCode,
                OnPrintPopulateDateFieldName = docType.OnPrintPopulateDateFieldName,
                OnSendPopulateDateFieldName = docType.OnSendPopulateDateFieldName,
                OnUploadPopulateDateFieldName = docType.OnUploadPopulateDateFieldName,
            };

            documentTypeRepository.Add(newDocType);

            return newDocType;
        }

        private List<AutomationDocumentTypeClass> FillAutomationDocumentTypeList(List<Automation> automationLists , List<DocumentTypePM> tenantZeroDocumentTypePms)
        {
            List<AutomationDocumentTypeClass> tenantZeroAutomationDocumentTypeClassLists = (from a in automationLists
                    select new AutomationDocumentTypeClass()
                    {
                        AutomationCode = a.Code,
                        DocumentTypeId = a.DocumentTypeId,
                        DocumentTypeTemplateId = a.TemplateId,
                        Tenant = a.Tenant,
                    }).ToList();


            foreach (AutomationDocumentTypeClass automationDocumentTypeClass in tenantZeroAutomationDocumentTypeClassLists)
            {
                automationDocumentTypeClass.DocumentTypeCode = tenantZeroDocumentTypePms.Where(d => d.Id == automationDocumentTypeClass.DocumentTypeId).Select(d => d.Code).FirstOrDefault();
            }

            return tenantZeroAutomationDocumentTypeClassLists;

        }

        private List<Automation>  GetAutomationsFromTenantZero(int tenant)
        {
            AutomationRepository automationRepository = new AutomationRepository(tenant);
            AutomationQuery automationQuery = new AutomationQuery(tenant);
            List<string> myAutomationListsCodes = automationQuery.GetAutomationCodeLists(tenant);
            List<Automation> automations = automationRepository.GetAutomations(0).Where(d => d.ResultCode == "EMAIL" && !d.Inactive && !string.IsNullOrEmpty(d.Code) && !myAutomationListsCodes.Contains(d.Code)).ToList();
            return automations;
        }


        public List<string> GetAutomationDocumentTypeIds(int tenant)
        {
            AutomationRepository automationRepository = new AutomationRepository(tenant);
            AutomationQuery automationQuery = new AutomationQuery(tenant);
            List<string> myAutomationListsCodes = automationQuery.GetAutomationCodeLists(tenant);
            List<string> automationdocumentTypeIds = automationRepository.GetAutomations(0).Where(d => d.ResultCode == "EMAIL" && !string.IsNullOrEmpty(d.Code) && !myAutomationListsCodes.Contains(d.Code)).Select(d => d.DocumentTypeId).ToList();
            return automationdocumentTypeIds;
        }




    }

    public class AutomationDocumentTypeClass
    {
        public string DocumentTypeId { get; set; }
        public string DocumentTypeTemplateId { get; set; }
        public string DocumentTypeCode { get; set; }
        public string AutomationCode { get; set; }
        public int Tenant { get; set; }
    }
}
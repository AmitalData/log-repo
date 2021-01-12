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
using WebFreight.Web.Helpers.AutomationModel;

namespace WebFreight.Web.Helpers
{
    public class AutomationHelper
    {
        public string ExecuteEmailAutomation(AutomationSendEmailArgs automationSendEmailArgs)
        {
            string comunicationLogId = string.Empty;
            List<Field> AutomationConditionFieldLists = automationSendEmailArgs.AutomationConditionFieldLists;
            Automation automation = automationSendEmailArgs.Automation;

            string objectTableName = automationSendEmailArgs.ObjectTableName;


            AutomationResultEmailRecipientQuery automationResultEmailRecipientQuery = new AutomationResultEmailRecipientQuery(automation.Tenant);
            List<AutomationResultEmailRecipientList> automationResultEmailRecipientLists = automationResultEmailRecipientQuery.GetAutomationResultEmailRecipientListsByAutomationId(automation.Id, automation.Tenant).Where(d=>!string.IsNullOrEmpty(d.RecipientValue)).ToList();
          
            var automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
            bool allActiveUsers = automatedBackup.IsAutomationResultEmailAllActiveUsers;
            
            //  #region Send Email Prosess

            if (automationResultEmailRecipientLists.Count > 0 || allActiveUsers)
            {
                #region  Automation Email Recipient
                List<string> contactIds = new List<string>();
                List<string> notifyBackContactIds = new List<string>();
                List<string> notifyBackPartners = new List<string>();
                string Emails = "";
                string NotifyBackEmails = "";

                if (allActiveUsers)
                {
                    UserQuery userQuery = new UserQuery(automationSendEmailArgs.Tenant);
                    contactIds = userQuery.GetUserIdsByTenant(automationSendEmailArgs.Tenant);
                }

                foreach (AutomationResultEmailRecipientList automationResultEmail in automationResultEmailRecipientLists)
                {
                    if (automationResultEmail.IsNotifyBack)
                    {
                        if (automationResultEmail.RecipientType == "Fixed")
                        {
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

                if (!string.IsNullOrEmpty(Emails))
                {
                    AutomationDocumentHelper automationDocumentHelper = new AutomationDocumentHelper(automationSendEmailArgs);
                    AutomationDocumentResult automationDocumentResult = automationDocumentHelper.GetAutomationDocumentResult();
                    automationDocumentResult.ToEmail = Emails;
                    automationDocumentResult.ObjectTableName = objectTableName;

                    string communicationLog = AddAutomationToQueue(automation, automationSendEmailArgs, automationDocumentResult);
                    comunicationLogId = communicationLog;
                }

                if (!string.IsNullOrEmpty(NotifyBackEmails))
                {
                    EmailCommunicationParams emailParams = BuildNotifyBackEmailCommunications(automation, notifyBackPartners, NotifyBackEmails);
                    Communications.AddEmailCommunicationLogQueue(emailParams, automation.Tenant);
                }
            }

            return comunicationLogId;
            //  #endregion
        }

        private EmailCommunicationParams BuildNotifyBackEmailCommunications(Automation automation, List<string> notifyBackPartners, string NotifyBackEmails)
        {
            string objectTableeName = GetObjectTableName(automation);
            string emailBody = GetAutomationNotifyBackEmailBody(automation.Name, objectTableeName, notifyBackPartners);
            StringBuilder HtmlTemplate = BuildHtmlTemplateWithBody(emailBody);

            string fromEmail = "no-reply@LogitudeWorld.com";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(automation.Tenant);
                fromEmail = tenantManagementQuery.GetSystemDomain(automation.Tenant);
                scope.Complete();
            }

            string emailbody = HtmlTemplate.ToString();
            EmailCommunicationParams emailParams = new EmailCommunicationParams()
            {
                From = fromEmail,
                To = NotifyBackEmails, CC = "", BCC = "",
                Subject = "Automation " + automation.Name + " Failed",
                EmailBody = emailbody,
                Tenant = automation.Tenant,
            };
            return emailParams;
        }
        private StringBuilder BuildHtmlTemplateWithBody(string emailBody)
        {
            StringBuilder HtmlTemplate = new StringBuilder();

            HtmlTemplate.Append("<div style='text-align:left;'>");
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append(emailBody);
            HtmlTemplate.Append("<br /><br />");
            HtmlTemplate.Append("<br /><br />");
            return HtmlTemplate;
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
            bool isMoreThanOnePartner = notifyBackPartners.Count > 1;
            emailString += "Automation " + automationName + " failed to be sent to the following ";
            emailString += "partner" + (isMoreThanOnePartner ? "s " : " ") + string.Join(",", notifyBackPartners.ToArray());
            if (isMoreThanOnePartner)
            {
                emailString = ReplaceLastOccurrence(emailString, ",", " and ");
            }
            emailString += " since "+ (isMoreThanOnePartner ? "they are" : "it is") +" not defined in " + objectTableeName + " level ";
            return emailString;
        }

        private string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        public string AddAutomationToQueue(Automation automation, AutomationSendEmailArgs automationSendEmailArgs, AutomationDocumentResult automationDocumentResult)
        {
            string entityId = automationSendEmailArgs.EntityId;
            string objectTableId = automationSendEmailArgs.ObjectTableId;
            string userId = automationSendEmailArgs.CreateByUserId;
            int tenant = automationSendEmailArgs.Tenant;
            string automationId = automation.Id;
            string documentOutId = null;

            if (string.IsNullOrEmpty(automationDocumentResult.ObjectTableName))
            {
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
                ObjectTable objectTable = objectTabelRepository.GetObjectTableById(objectTableId, tenant);
                if (objectTable != null) automationDocumentResult.ObjectTableName = objectTable.Name;
            }

            if (automationDocumentResult.ObjectTableName == "Shipment")
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

            ICommonDataContext context = CommonDataContext.GetContext(tenant);

            CommunicationLog log = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                InOut = "O",
                To = automationDocumentResult.ToEmail,
                CC = automationDocumentResult.HtmlEditorResolveResult.Cc,
                BCC = automationDocumentResult.HtmlEditorResolveResult.Bcc,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                DocumentId = automationDocumentResult.DocumentId,
                CreatedByUserId = userId,
                EntityId = entityId,
                ObjectTableId = objectTableId,
                DocumentOutId = documentOutId,
                DocumentsFilingId = null,
                Subject = automationDocumentResult.HtmlEditorResolveResult.Subject,
                Tenant = tenant,
                LastStatusDateUTC = DateTime.UtcNow,
                CreateDateUTC = DateTime.UtcNow,
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationLogTypeCode = "E",
                CommunicationStatusTypeCode = "W",

            };


            if (!string.IsNullOrEmpty(automationDocumentResult.HtmlEditorResolveResult.From) && !string.IsNullOrWhiteSpace(automationDocumentResult.HtmlEditorResolveResult.From))
            {
                log.From = automationDocumentResult.HtmlEditorResolveResult.From;
            }
            if (!string.IsNullOrEmpty(automationDocumentResult.HtmlEditorResolveResult.ReplyTo) && !string.IsNullOrWhiteSpace(automationDocumentResult.HtmlEditorResolveResult.ReplyTo))
            {
                log.ReplyToList = automationDocumentResult.HtmlEditorResolveResult.ReplyTo;
            }

            context.CommunicationLogs.Add(log);

            DocumentTypeTemplateDefultAttachmentService documentTypeTemplateDefultAttachmentService = new DocumentTypeTemplateDefultAttachmentService();
            var attachments = documentTypeTemplateDefultAttachmentService.GetDefultAttachmentList(new DocumentTypeTemplateDefultAttachmentArgs() { DocumentTypeTemplateId = automation.TemplateId, EntityId = entityId, ObjectTableId = objectTableId, Tenant = tenant });
            if (attachments.Count() > 0)
            {
                foreach (var item in attachments)
                {
                    CommunicationAttachment attachment = GetNewCommunicationAttachment(tenant, log, item.Id);
                    context.CommunicationAttachments.Add(attachment);

                }

            }

            AddReportTemplateDocOutAttachment(automationSendEmailArgs, context, log);

            if (!string.IsNullOrEmpty(automationSendEmailArgs.ExternalAttachmentDocumentId))
            {
                context.CommunicationAttachments.Add(GetNewCommunicationAttachment(tenant, log, automationSendEmailArgs.ExternalAttachmentDocumentId));
            }

            context.SaveChanges();

            //IQueueService queueservice = QueueServiceManager.GetQueueService("EmailQueue", tenant);
            DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
            queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } }, tenant);

            return log.Id;
        }

        private  void AddReportTemplateDocOutAttachment(AutomationSendEmailArgs automationSendEmailArgs, ICommonDataContext context, CommunicationLog log)
        {
            if (!string.IsNullOrEmpty(automationSendEmailArgs.ReportTemplateId))
            {
                ReportTemplateDocOutArgs reportTemplateDocOutArgs = new ReportTemplateDocOutArgs()
                {
                    Tenant = automationSendEmailArgs.Tenant,
                    ReportTemplateId = automationSendEmailArgs.ReportTemplateId,
                    DocumentTypeId = automationSendEmailArgs.Automation.DocumentTypeId,
                    EntityId = automationSendEmailArgs.EntityId, 
                    ObjectTableId = automationSendEmailArgs.ObjectTableId,
                };

                string documentId = new ReportTemplateDocOutService(reportTemplateDocOutArgs).GetDocOutDocumentId();
                if (!string.IsNullOrEmpty(documentId))
                {
                    CommunicationAttachment attachment = GetNewCommunicationAttachment(automationSendEmailArgs.Tenant, log, documentId);
                    context.CommunicationAttachments.Add(attachment);
                }
            }
        }

        private static CommunicationAttachment GetNewCommunicationAttachment(int tenant, CommunicationLog log, string documentId)
        {
            return new CommunicationAttachment()
            {
                Id = IdCounter.GetNumber("CommunicationAttachment", tenant).ToString(),
                CommunicationLogId = log.Id,
                DocumentId = documentId,
                Tenant = tenant,
            };
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
                        var docType = tenantZeroDocumentTypePmsUsedInAutomation.Where(d => d.Id == automationDocumentTypeClass.DocumentTypeId).FirstOrDefault();
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
                        myDocumentTypeTemplatePM = new DocumentTypeTemplatePM() { Id = newtemplate.Id, DocumentTypeId = newtemplate.DocumentTypeId, OriginalTemplateId = newtemplate.OriginalTemplateId, Tenant = newtemplate.Tenant };
                        myDocumentTypeTempaltesUsedInAutomation.Add(myDocumentTypeTemplatePM);

                        if (newDocType != null && string.IsNullOrEmpty(newDocType.DocumentTypeDefaultEditorTool))
                        {
                            newDocType.DocumentTypeDefaultEditorTool = newtemplate.EditorTool;
                        }
               

                        if (newDocType != null && string.IsNullOrEmpty(newDocType.DocumentTypeDefaultHTMLTemplateId))
                        {
                            newDocType.DocumentTypeDefaultHTMLTemplateId = newtemplate.Id;
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

        public List<string> GetAutomationDocumentTypeTemplateIds(int tenant)
        {
            AutomationRepository automationRepository = new AutomationRepository(tenant);
            AutomationQuery automationQuery = new AutomationQuery(tenant);
            List<string> myAutomationListsCodes = automationQuery.GetAutomationCodeLists(tenant);
            List<string> automationDocumentTypeTemplateIds = automationRepository.GetAutomations(0).Where(d => d.ResultCode == "EMAIL" && !string.IsNullOrEmpty(d.Code) && !myAutomationListsCodes.Contains(d.Code)).Select(d => d.TemplateId).ToList();
            return automationDocumentTypeTemplateIds;


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



    public class AutomationSendEmailArgs
    {
        public List<Field> AutomationConditionFieldLists { get; set; }
        public Automation Automation { get; set; }

        public  string ObjectTableName { get; set; }
        public string ExternalAttachmentDocumentId { get; set; }

        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }

        public string CreateByUserId { get; set; }
        public int Tenant { get; set; }
        public string ReportTemplateId { get; set; }
        

    }


}
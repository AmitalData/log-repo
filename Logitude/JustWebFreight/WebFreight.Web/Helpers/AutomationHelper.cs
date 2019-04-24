using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
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
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        string userId = template.LastUpdatedByUserId;
                        if (!string.IsNullOrEmpty(entityChange.CreateByUserId)) userId = entityChange.CreateByUserId;
                        byte[] htmldata = null;
                        try
                        {

                            string html = htmlEditorHelper.GetEditorHtmlData("", entityChange.EntityId, entityChange.ObjectTableId, "", "", entityChange.Tenant, userId, true, template.Id, ref subject, ref from, ref replyTo, ref cc, "", template);

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
                        string Emails = "";

                        if (allActiveUsers)
                        {
                            UserQuery userQuery = new UserQuery(entityChange.Tenant);
                            contactIds = userQuery.GetUserIdsByTenant(entityChange.Tenant);
                        }


                        foreach (AutomationResultEmailRecipientList automationResultEmail in automationResultEmailRecipientLists)
                        {
                            if (automationResultEmail.RecipientType == "Fixed")
                            {
                                if (!contactIds.Contains(automationResultEmail.RecipientValue)) contactIds.Add(automationResultEmail.RecipientValue);
                            }
                            else
                            {
                                if (AutomationConditionFieldLists != null)
                                {
                                    Field entityContactVariable = AutomationConditionFieldLists.Where(d => d.Id == automationResultEmail.RecipientValue).FirstOrDefault();
                                    if (entityContactVariable != null)
                                    {
                                        if (automationResultEmail.RecipientType == "Emails")
                                        {
                                            if (!Emails.Split(';').Contains(entityContactVariable.Value)) Emails += entityContactVariable.Value + ";";
                                        }
                                        else
                                        {
                                            if (!contactIds.Contains(entityContactVariable.Value)) contactIds.Add(entityContactVariable.Value);
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

                        #endregion

                        if (!string.IsNullOrEmpty(Emails) && htmldata != null)
                        {
                            string communicationLog = AddAutomationToQueue(automation, entityChange, htmldata, Emails, from, replyTo, cc, subject);
                            entityChangesAutomation.ComunicationLogId = communicationLog;
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

        public string AddAutomationToQueue(Automation automation , EntityChange  entityChange ,  byte[] htmlData, string toEmail,  string from, string replyTo, string cc,  string subject, string objectTableName = null)
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
                BCC = "",
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
			queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });

            return log.Id;
        }

        public void CopyAutomationFromTenantZeroToMyTenant(int tenant)
        {
            //AutomationQuery automationQuery = new AutomationQuery(tenant);
            //AutomationRepository automationRepository = new AutomationRepository(tenant);
            //List<string> automationListsCodes =  automationQuery.GetAutomationCodeLists(tenant);
            //List<Automation> automationLists = automationRepository.GetAutomations(0).Where(d =>d.ResultCode == "EMAIL" && !string.IsNullOrEmpty(d.Code)&& !automationListsCodes.Contains(d.Code)).ToList();

            //foreach (Automation automation in automationLists)
            //{
            //    var newautomation = new AutomationPM()
            //    {
            //        Tenant = tenant,
            //        Code = automation.Code,
            //        Description = automation.Description,
            //        From = automation.From,
            //        FromEmail = automation.FromEmail,
            //        Inactive = automation.Inactive,
            //        Version = 1,
            //        Type = automation.Type,
            //        ResultCode = automation.ResultCode,
            //        Order = automation.Order,
            //        AutomationXML = automation.AutomationXML,
            //        ObjectTableId = automation.ObjectTableId,
            //    };

            //    ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
            //    AutomationService service = new AutomationService(MyContext, tenant);
            //    service.Create(newautomation);
            //}

        }
    }
}
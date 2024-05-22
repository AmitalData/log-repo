using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Validators;
using System.Transactions;
using Logitude.CRM.Data.EntityKeys;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;
using WebFreight.Web.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System.Web;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure.Azure;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ActivityWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ActivityWcfService.svc or ActivityWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ActivityWcfService : IActivityWcfService
    {

        public Response Upsert(Logitude.CRM.BL.EntityPMs.ActivityPM entityPM, string email)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Activity", "UPDATE", entityPM.Tenant);

                using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
                {
                    entityPM.IsHybrid = true;
                    ICRMContext objectContext = CRMContext.GetContext(entityPM.Tenant);
                    ICommonDataContext commoncontext = CommonDataContext.GetContext(entityPM.Tenant);
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);

                    ActivityUpdateService service = new ActivityUpdateService(objectContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    UserRepository userrepository = new UserRepository(commoncontext);
                    ContactRepository contactRepository = new ContactRepository(commoncontext);
                    ObjectTableRepository objecttableRepository = new ObjectTableRepository(webFreightContext);
                    CardRepository cardsReporistory = new CardRepository(commoncontext);

                    ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);


                    Contact contact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);

                    if (contact == null)
                    {
                        response.HasError = true;
                        response.ErrorMessage = "email doesn't exist in the database,Upsert this entity before using it.";
                        return response;
                    }

                    User user = userrepository.GetSingleUser(contact.Id, contact.Tenant, false);

                    entityPM.OwnerId = contact.Id;
                    entityPM.UpdatedByUserId = contact.Id;
                    entityPM.CreatedByUserId = contact.Id;
                    entityPM.BranchId = user.BranchId;
                    entityPM.CompleteDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);


                    ClassLevelValidator validationClass = new ClassLevelValidator("Activity", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }


                    if (!string.IsNullOrEmpty(entityPM.SenderEmail))
                    {
                        Contact sendercontact = contactRepository.GetSingleContactByEmail(entityPM.SenderEmail, entityPM.Tenant);
                        if (sendercontact != null)
                        {
                            entityPM.SenderContactId = contact.Id;
                        }

                    }

                    foreach (ActivityInviteePM invitee in entityPM.ActivityInvitees)
                    {
                        invitee.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        if (!string.IsNullOrEmpty(invitee.Email))
                        {
                            Contact inviteecontact = contactRepository.GetSingleContactByEmail(invitee.Email, entityPM.Tenant);
                            if (inviteecontact != null)
                            {
                                invitee.ContactId = inviteecontact.Id;
                            }

                        }

                    }

                    foreach (ActivityEmailRecipientPM emailRecipient in entityPM.ActivityEmailRecipients)
                    {
                        emailRecipient.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        if (!string.IsNullOrEmpty(emailRecipient.Email))
                        {
                            Contact inviteecontact = contactRepository.GetSingleContactByEmail(emailRecipient.Email, entityPM.Tenant);
                            if (inviteecontact != null)
                            {
                                emailRecipient.ContactId = inviteecontact.Id;
                            }

                        }

                    }

                
                    //entityPM.SortingDate = entityPM.SendReceiveDate;
                    if (entityPM.ActivityTypeCode == "EI" || entityPM.ActivityTypeCode == "EO")
                    {
                        entityPM.IsMarkedCompleted = true;
                        entityPM.ActivityStatusCode = "C";
                        //entityPM.SendReceiveDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    }


                    ActivityPM entity = activityQueryService.GetSingleByOutlookId(entityPM.OutlookId, entityPM.Tenant);

                    if (entity == null)
                    {


                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                        if (entityPM.ActivityTypeCode != "EI")
                        {
                            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                        }
                        entityPM.DontSetNeedSynchronization = true;

                        service.Update(entityPM, true);

                        string activity = "New Activity from Outlook";
                        switch (entityPM.ActivityTypeCode)
                        {
                            case "EI":
                            case "EO":
                                activity = "E-mail Connection";
                                break;
                            case "TS":
                                activity = "Task- New from Outlook";
                                break;
                            case "AP":
                                activity = "Appointment - New from Outlook";
                                break;
                            case "CL":
                                activity = "Call - New from Outlook";
                                break;

                        }

                        // ActivityLog.SendTotangoContactActivity(email, "OUTLOOK COONECTION", activity, entityPM.Tenant, false, null);

                    }
                    else
                    {
                        entityPM.Id = entity.Id;

                        if (entity.ActivityStatusCode != "C" || (entity.MeetingSummary != entityPM.MeetingSummary))
                        {
                            entityPM.ConcurrencyGUID = entity.ConcurrencyGUID;
                            //entityPM.MeetingSummary = entity.MeetingSummary;

                            if (entity.ActivityStatusCode != "X")
                            {
                                entityPM.Id = entity.Id;
                                if (entityPM.ActivityTypeCode != "EI")
                                {
                                    entityPM.UpdateDate = DateTime.Now;
                                }
                                entityPM.DontSetNeedSynchronization = true;
                                entityPM.NeedSynchronization = false;
                                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                ActivityKeys activityKeys = new ActivityKeys() { Id = entity.Id };
                                ActivityInviteeQueryService queryService = new ActivityInviteeQueryService(objectContext);
                                List<ActivityInviteePM> activityInviteesList = queryService.GetMulti(activityKeys, true);
                                foreach (ActivityInviteePM invitee in activityInviteesList)
                                {
                                    invitee.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                    entityPM.ActivityInvitees.Add(invitee);
                                }


                                ActivityEmailRecipientQueryService emailRecipientQueryService = new ActivityEmailRecipientQueryService(objectContext);
                                List<ActivityEmailRecipientPM> recipients = emailRecipientQueryService.GetMulti(activityKeys, true);
                                foreach (ActivityEmailRecipientPM emailRecipient in recipients)
                                {
                                    emailRecipient.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                    entityPM.ActivityEmailRecipients.Add(emailRecipient);
                                }



                                service.Update(entityPM, true);



                            }
                        }

                    }
                    DocumentInWcfService documentService = new DocumentInWcfService();

                    if (string.IsNullOrEmpty(entityPM.ObjectTableName))
                    {
                        if (!string.IsNullOrEmpty(entityPM.OpportunityId))
                        {
                            entityPM.ObjectTableName = "Opportunity";
                        }
                        else if (!string.IsNullOrEmpty(entityPM.CustomerId))
                        {
                            entityPM.ObjectTableName = "Customer";
                        }
                        else if (!string.IsNullOrEmpty(entityPM.QuoteId))
                        {
                            entityPM.ObjectTableName = "Quote";
                        }
                    }

                    foreach (DocumentDataPM documentDataPM in entityPM.ActivityDocumentDatas)
                    {
                        //if (string.IsNullOrEmpty(documentDataPM.EntityId) && !string.IsNullOrEmpty(documentDataPM.ObjectTableName))
                        //{
                        //    documentDataPM.EntityId = entityPM.Id;
                        //    documentDataPM.ObjectTableName = "Activity";

                        //}


                        DocumentsFilingPM documentInPM = new DocumentsFilingPM()
                        {
                            // Id = IdCounter.GetNumber("Document", documentDataPM.Tenant).ToString(),
                            DirectionCode = "I",
                            Tenant = documentDataPM.Tenant,
                            Code = documentDataPM.Code,

                            DocumentId = documentDataPM.DocumentId,
                            EntityId = documentDataPM.EntityId,//entityPM.Id,
                            ChildEntityId = entityPM.Id,//documentDataPM.ChildEntityId,
                            ChildEntityReference = documentDataPM.ChildEntityReference,
                            DocumentTypeId = documentDataPM.DocumentTypeId,
                            ObjectTableId = entityPM.ObjectTableName,//"Activity",
                            ChildObjectTableId = "Activity",//documentDataPM.ChildObjectTableName,
                            CreatedByUserId = user.Id,
                            CreateDate = documentDataPM.ReceivedDate != null ? documentDataPM.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(documentDataPM.Tenant),
                            Notes = documentDataPM.Notes,
                            OwnerId = user.Id,
                            UpdatedByUserId = user.Id,
                            UpdateDate = documentDataPM.ReceivedDate != null ? documentDataPM.ReceivedDate.Value : TenantServerConfigration.GetCurrentDateTime(documentDataPM.Tenant),
                            Description = documentDataPM.Description,
                            ExternalEntityName = documentDataPM.ExternalEntityName,
                            ExternalEntityReference = documentDataPM.ExternalEntityReference,
                            EntityReference = documentDataPM.EntityReference,
                            FileExtension = documentDataPM.FileExtension,
                            FileSize = documentDataPM.FileData != null ? Convert.ToInt32(documentDataPM.FileData.Length) : documentDataPM.FileSize,
                            Folder = "docsin",
                            HasFile = true,
                            FileName = documentDataPM.FileName,
                            IsHybrid = true,
                            FileData = documentDataPM.FileData,
                            IsAttachment = true,
                        };




                        Response documentResponse = documentService.UpsertDocumentData(documentInPM);
                        if (documentResponse.HasError)
                        {
                            response = documentResponse;
                            break;
                        }
                    }




                    response.Result = entityPM.Id;

                    scope.Complete();


                }

                return response;

            }

            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }



        }

        public List<Logitude.CRM.BL.EntityPMs.ActivityPM> GetActivities(string email, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);//UPDATE//READ

                ICRMContext objectContext = CRMContext.GetContext(tenant);
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                ContactRepository contactRepository = new ContactRepository(commoncontext);
                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);
                ActivityQueryService activityService = new ActivityQueryService(objectContext);

                return activityService.GetNotSyncActivitis(contact.Id, tenant);
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }

        }

        public Response UpdateOutlookID(string activityId, string outlookId, int tenant)
        {

            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
                {

                    ICRMContext objectContext = CRMContext.GetContext(tenant);

                    ActivityUpdateService service = new ActivityUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
                    ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);

                    ActivityPM entityPM = activityQueryService.GetSingle(activityId, false, false);//activityRepository.GetSingle(activityId, tenant);
                    entityPM.OutlookId = outlookId;
                    entityPM.NeedSynchronization = false;
                    entityPM.DontSetNeedSynchronization = true;

                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    service.Update(entityPM, true);

                    scope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public Response SetAsSynchronized(string activityId, string email, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
                {

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                    ContactRepository contactRepository = new ContactRepository(commoncontext);
                    Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);

                    ICRMContext objectContext = CRMContext.GetContext(tenant);

                    ActivityUpdateService service = new ActivityUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
                    ActivityOwnerHistoryUpdateService activityOwnerHistoryUpdateService = new ActivityOwnerHistoryUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
                    ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);
                    ActivityOwnerHistoryQueryService activityOwnerHistoryQueryService = new ActivityOwnerHistoryQueryService(objectContext);

                    ActivityPM entityPM = activityQueryService.GetSingleByOwnerId(activityId, contact.Id, tenant);
                    if (entityPM != null)
                    {
                        entityPM.NeedSynchronization = false;
                        entityPM.DontSetNeedSynchronization = true;

                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        service.Update(entityPM, true);
                    }
                    else
                    {
                        List<ActivityOwnerHistoryPM> activityOwnerHistoryPMs = activityOwnerHistoryQueryService.GetActivityOwnerHistoryByActivityId(activityId, contact.Id, tenant);
                        foreach (ActivityOwnerHistoryPM pm in activityOwnerHistoryPMs)
                        {
                            pm.NeedSynchronization = false;
                            pm.ChangeSetOp = ChangeSetOperation.Update;
                            activityOwnerHistoryUpdateService.Update(pm, false);
                        }

                        objectContext.SaveChanges();

                    }

                    scope.Complete();

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public Response Delete(string activityId, int tenant)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
                {

                    ICRMContext objectContext = CRMContext.GetContext(tenant);

                    ActivityUpdateService service = new ActivityUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
                    ActivityQueryService activityQueryService = new ActivityQueryService(objectContext);


                    ActivityPM entityPM = activityQueryService.GetSingle(activityId, false, false);
                    if (entityPM != null)
                    {
                        entityPM.DontSetNeedSynchronization = true;
                        entityPM.NeedSynchronization = false;
                        entityPM.ActivityStatusCode = "X";
                        entityPM.IsOpen = false;
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        service.Update(entityPM, true);
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Activity not found";

                    }
                    scope.Complete();

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = new Response();
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public Response isOnline()
        {
            return new Response() { HasError = false };
        }


        public ActivityPM GetActivityPM(string id, int tenant, ref Response response)
        {
            try
            {
                ActivityPM entityPM = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Activity", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }


                ICRMContext crmContext = CRMContext.GetContext(tenant);
                ActivityQueryService activityQuery = new ActivityQueryService(crmContext);
                entityPM = activityQuery.GetSingle(id, true, false);

                return entityPM;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }

        public Response GetStorageContainerConnectionString(int tenant)
        {
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                response.Result = DocumentFileUploadHelper.GetTempStorageSasWrite(tenant);
                return response;
            }
            catch (Exception ex)
            {
                return UpdateResponseException(response, ex);
            }
        }

        private Response UpdateResponseException(Response response, Exception ex)
        {
            response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
            response.HasError = true;
            response.ErrorMessage = ex.Message;
            response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                response.ErrorMessage += Environment.NewLine + ex.StackTrace;
            }

            return response;
        }

        public Response UploadDocumentFileData(int tenant, string blobname, string DocumentId)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(blobname) || string.IsNullOrEmpty(DocumentId))
                return UpdateResponseException(response, new ArgumentException("blobname or DocumentId is null or empty"));

            SecurityUtility.AuthenticationOnTenant(tenant);
            response = DocumentFileUploadHelper.AddDocumentAndSendToInternalStorage(tenant, blobname, DocumentId);

            return response;
        }

        public Response UploadDocumentFileData(byte[] buffer, long fileSize, long sentBytes, string[] blockIdsList, int bufferNumber, int tenant, string FileNameWithExtention, string DocumentId)
        {
            Response response = new Response();

            try
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

                if (string.IsNullOrEmpty(FileNameWithExtention))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Invalid file name!";
                    return response;
                }

                string[] fileParams = FileNameWithExtention.Split('.');
                string finalFileName = FileNameWithExtention.Substring(0, FileNameWithExtention.LastIndexOf('.'));
                string fileextension = fileParams[fileParams.Length - 1];

                if (string.IsNullOrEmpty(fileextension))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Invalid file extension!";
                }

                if (string.IsNullOrEmpty(finalFileName))
                {
                    response.HasError = true;
                    response.ErrorMessage = "Invalid file name!";
                }

                if (response.HasError)
                {
                    return response;
                }


                if (sentBytes < fileSize)
                {
                    fileName = BuidDocument(tenant, FileNameWithExtention, fileSize, DocumentId, false);
                    string[] fileparams = fileName.Split('.');
                    //string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = fileparams[0],
                        FolderName = "docsin",
                        Extension = fileparams[1],
                        Tenant = tenant,
                        FileSize = fileSize,

                    };
                    storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);
                    //if (res.HasError)
                    //{
                    //    response.HasError = res.HasError;
                    //    response.ErrorMessage = res.ErrorMessage;
                    //}
                    //else
                    //{
                    //    response.Result = fileName.Split('.')[0].ToString();// "In Progress";
                    //}
                    response.Result = fileName.Split('.')[0].ToString();// "In Progress";

                }
                else if (sentBytes == fileSize)
                {
                    fileName = BuidDocument(tenant, FileNameWithExtention, fileSize, DocumentId, true);
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "docsin");
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    string[] fileparams = fileName.Split('.');
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = fileparams[0],
                        FolderName = "docsin",
                        Extension = fileparams[1],
                        Tenant = tenant,
                        FileSize = fileSize,

                    };

                    storageservice.WriteBlock(buffer, sentBytes, blockIdsList, bufferNumber, fileInfo);
                    //if (res.HasError)
                    //{
                    //    response.HasError = res.HasError;
                    //    response.ErrorMessage = res.ErrorMessage;
                    //}
                    //else
                    //{
                    //    response.Result = fileName.Split('.')[0].ToString(); //"Done";
                    //}

                    response.Result = fileName.Split('.')[0].ToString(); //"Done";
                }

                return response;
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }

                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }

        }
        string fileNameAndExtension;
        private static string fileName;
        string documentIdAndExtension;
        private string BuidDocument(int tenant, string FileNameWithExtention, long fileSize, string DocId, bool hasfile)
        {
            //try
            //{
            DocumentsFilingRepository externalDocumentRepository = new DocumentsFilingRepository(tenant);
            DocumentRepository docRepository = new DocumentRepository(tenant);

            //DocumentsFiling externalDocument = externalDocumentRepository.GetSingleDocumentsFiling(externalDocumentId, tenant);
            Document document = null;
            if (!string.IsNullOrEmpty(DocId))
            {
                document = docRepository.GetSingleDocument(tenant, DocId);
            }

            //string fileextension = FileNameWithExtention.Split('.')[1].ToString();
            //string finalFileName = FileNameWithExtention.Split('.')[0].ToString();// externalDocumentId + "." + fileextension;

            string[] fileParams = FileNameWithExtention.Split('.');
            string finalFileName = FileNameWithExtention.Substring(0, FileNameWithExtention.LastIndexOf('.'));
            string fileextension = fileParams[fileParams.Length - 1];


            string realFileName = null;
            if (!string.IsNullOrEmpty(fileName))
            {
                realFileName = fileName.Split('.')[0];
            }

            if (document == null)
            {
                document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = fileextension,
                    FileSize = Convert.ToInt32(fileSize),
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("Document", tenant).ToString(),//externalDocumentId,
                    HasFile = hasfile,
                    Folder = "docsin",
                    FileName = finalFileName,
                };
                docRepository.Add(document);
            }
            else
            {
                document.CreateDate = DateTime.Now;
                document.Extension = fileextension;
                document.FileSize = Convert.ToInt32(fileSize);
                document.Tenant = tenant;
                document.HasFile = hasfile;
                document.Folder = "docsin";
                document.FileName = finalFileName;
                document.IsEncrypted = true;
                docRepository.Update(document);
            }

            docRepository.SubmitChanges();
            fileNameAndExtension = document.Id + "." + document.Extension;
            //externalDocument.DocumentId = document.Id;
            //externalDocumentRepository.Update(externalDocument);
            //externalDocumentRepository.SubmitChanges();

            return fileNameAndExtension;

            //    }

            //    catch (Exception e)
            //    {
            //        string ip = "";
            //        if (HttpContext.Current != null && HttpContext.Current.Request != null)
            //        {
            //            ip = HttpContext.Current.Request.UserHostAddress;
            //        }
            //    }
            //    return documentIdAndExtension;
            //}
        }
    }
}

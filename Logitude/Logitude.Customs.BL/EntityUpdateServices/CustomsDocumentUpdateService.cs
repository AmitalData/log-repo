using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure;
using Microsoft.ServiceBus.Messaging;
using System.Transactions;
using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using System.IO;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Diagnostics;
using Logitude.Customs.Data.Repsitories;
using System.Configuration;
using System.Globalization;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsDocumentUpdateService : EntityUpdateService<CustomsDocument, CustomsDocumentPM, EntityPM>
    {
        protected override void OnCreating(CustomsDocumentPM entityPM, EntityPM entityParentPM)
        {
            
            //entityPM.DocumentInId = IdCounter.GetNumber("Customs.CustomsDocument", entityPM.Tenant);
            entityPM.DocumentVersion = 1;
            

        }

        //לאחר ממשק UD2LT - קישור מסמך לטיקט, אם התיק הינו תיק בלדרות יש לבצע העלאה של המסמך למכס - מסר קלוט צרופה
        public void AddPerfectCustomsDocumentMetaDataValues(CustomsDocumentPM entityPM)
        {
            if ( entityPM.CustomsDocumentMetaDataValues.Count == 0)
            {
                var customContext = CustomContext.GetContext(entityPM.Tenant);
                var customDocumentTypeMetaDataQuery = new CustomDocumentTypeMetaDataQueryService(customContext);
                var CustomDocumentTypeMetaData = customDocumentTypeMetaDataQuery.GetCustomDocumentTypeMetaDataByType(entityPM.DocumentTypeCode);
                entityPM.CustomsDocumentMetaDataValues = CustomDocumentTypeMetaData.Select(r => new CustomsDocumentMetaDataValuePM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    Tenant = entityPM.Tenant,
                    CustomsDocumentId = entityPM.CustomsDocId,
                    MetaDataTypeCode = r.MetaDataTypeCode,
                    MetaDataValue = null,
                }).ToList();

                AutoSetOriginalDocumentTrue(entityPM);
                this._AddPerfectCustomsDocumentMetaDataValues_IsMetaDataReady = true;
            }
        }
        protected override void UpdateComposition(CustomsDocumentPM entityPM)
        {
            AutoSetOriginalDocumentTrue(entityPM);

            CustomsDocumentMetaDataValueUpdateService customsDocumentMetaDataValueUpdateService = new CustomsDocumentMetaDataValueUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            customsDocumentMetaDataValueUpdateService.UpdateMulti(entityPM.CustomsDocumentMetaDataValues, entityPM.DeletedCustomsDocumentMetaDataValues, entityPM, false);

        }

        private void AutoSetOriginalDocumentTrue(CustomsDocumentPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(context);
            CustomDocumentTypePM docType = docTypeQuery.GetSingle(entityPM.DocumentTypeCode, false, false);

            foreach (CustomsDocumentMetaDataValuePM val in entityPM.CustomsDocumentMetaDataValues)
            {
                if (docType != null && docType.AutoSetOriginalDocumentTrue && val.MetaDataTypeCode == "87" && val.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    val.MetaDataValue = "True";
                }
            }
        }

        public const string SetCustomsRequestSheetStatus = "SetCustomsRequestSheetStatus";
        private void AddHybridTaskDocumentFilingChange(DocumentsFilingPM documentsFilingPM)//Bug 36694: Disconnecting document from the ticket  does not create trigger to UNF
        {
            var tenant = documentsFilingPM.Tenant;
            var loggedUserId = AuthenticationUtil.ResolveUserId(tenant);
            var table = ObjectTableQuery.GetObjectTableByCode("DocumentsFiling", 0);




            CommunicationsParams logParams = new CommunicationsParams()
            {
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = "externaltasksqueue" + tenant + 1,
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = loggedUserId,
                LoggingObjectTableId = table.Id,
                LoggingEntityId = documentsFilingPM.Id,
                Subject = "New Documents Filing Created",
                FolderName = "ExternalTasksQueue",
            };
            DocumentsFilingPM mappedPM = DocumentsFilingHybridMapping.MapEntityToHybrid(documentsFilingPM);

            string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(mappedPM);
            List<QueueTask> queue1Tasks = new List<QueueTask>();


            queue1Tasks.Add(new QueueTask()
            {
                Action = "DocumentsFiling.Upsert",
                Parameters = new List<Parameter>()
                                             {
                                                new Parameter{ Name = "DocumentMetaData", Order = 1, Value = xmlstring }
                                             }
            });

            logParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue1Tasks);
            Communications.AddCommunicationLog(logParams);
        }

        protected override void OnUpdating(CustomsDocumentPM entityPM, CustomsDocument entityPOCO)
        {
            try
            {
                if (entityPM.IsPartOfDeclaration != entityPOCO.IsPartOfDeclaration)
                {
                    string documentsFilingId = entityPM.DocumentsFilingId ?? EntityPOCO.DocumentsFilingId;
                    if (!string.IsNullOrWhiteSpace(documentsFilingId))
                    {
                        var documentsFilingQuery = new DocumentsFilingQuery(entityPM.Tenant);
                        var pm = documentsFilingQuery.GetSinglePM(documentsFilingId, entityPM.Tenant);
                        if (pm.ExternalEntityName == "CFIFILEM" && !string.IsNullOrWhiteSpace(pm.ExternalEntityReference))
                        {
                            var unifreightFUStatusTaskService = new UnifreightFUStatusTaskService();
                            unifreightFUStatusTaskService.DeleteINAFUStatus(entityPM.Tenant, pm.ExternalEntityReference);
                        }
                        AddHybridTaskDocumentFilingChange(pm); //Bug 36694: Disconnecting document from the ticket  does not create trigger to UNF
                    }

                }
                CustomsDocumentSendLog(entityPM, entityPOCO);


                if (entityPM.CurrentContextTag != SetCustomsRequestSheetStatus && String.IsNullOrWhiteSpace(entityPM.DocumentStatusCode) && !String.IsNullOrWhiteSpace(entityPOCO.DocumentStatusCode))
                {

                    // mohammad : i removed the exceptions to allow nulls because of the new version dev task 32398
                    //throw new Exception("Please call Amital!!!! Error in OnUpdating of document");
                }

                var forceDueLoadTest = (
                    !string.IsNullOrWhiteSpace(entityPM.DocumentRemarks) &&
                    entityPM.DocumentRemarks.Contains(LoadTestSendMessageToQueue) &&
                        entityPM.DocumentVersion == entityPOCO.DocumentVersion + 1);
                TrySendMessageToQueue(entityPM, forceDueLoadTest);
                base.OnUpdating(entityPM, entityPOCO);
                if (entityPM.DocumentStatusCode == "7")
                {
                    UpdateDeclarationCourierStatus(entityPM);
                }
                if(entityPM.DocumentTypeCode == "380" && string.IsNullOrEmpty(entityPM.DocumentStatusCode) && entityPM.ChangeSetOp == ChangeSetOperation.Update)
                {
                    UpdateDeclarationCourierStatus380(entityPM);
                }

            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<CustomsDocumentPM, CustomsDocument>("20180826HD315750.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
        }

        private void CustomsDocumentSendLog(CustomsDocumentPM entityPM, CustomsDocument entityPOCO)
        {
            /*
             *  CustomsDocumentStatusType CustomsDocumentStatusType1 = new CustomsDocumentStatusType() { Code = "1", LocalName = "נשלח", EnglishName = "Sent", SearchFields = "1", };
                CustomsDocumentStatusType CustomsDocumentStatusType2 = new CustomsDocumentStatusType() { Code = "2", LocalName = "נכשל", EnglishName = "Fail", SearchFields = "2", };
                CustomsDocumentStatusType CustomsDocumentStatusType3 = new CustomsDocumentStatusType() { Code = "3", LocalName = "נדרש", EnglishName = "Needed", SearchFields = "3", };
                CustomsDocumentStatusType CustomsDocumentStatusType4 = new CustomsDocumentStatusType() { Code = "4", LocalName = "אומת", EnglishName = "Verified", SearchFields = "4", };
                CustomsDocumentStatusType CustomsDocumentStatusType5 = new CustomsDocumentStatusType() { Code = "5", LocalName = "אומת בנוכחות הלקוח", EnglishName = "Verified With Customer Presents", SearchFields = "5", };
                CustomsDocumentStatusType CustomsDocumentStatusType6 = new CustomsDocumentStatusType() { Code = "6", LocalName = "נדחה אימות", EnglishName = "Verify Rejected", SearchFields = "6", };
                CustomsDocumentStatusType CustomsDocumentStatusType7 = new CustomsDocumentStatusType() { Code = "7", LocalName = "נשלח ללא תשובה", EnglishName = "Sent Without Answer", SearchFields = "7", };
             */
            try
            {
                if (entityPM.DocumentsFilingId != entityPOCO.DocumentsFilingId)
                {
                }
                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20180125.CustomsDocumentSendLogUntilDateyyyyMMdd"];
                if (string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    return;
                }

                DateTime stopLogAt = DateTime.MinValue; //new DateTime(2018, 02, 20);
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);

                if (DateTime.Now > stopLogAt)
                {
                    return;
                }



                bool pocoHaveCustomsREF = (!String.IsNullOrWhiteSpace(entityPOCO.CustomsDocId));
                bool pocoHaveSent = (entityPOCO.DocumentStatusCode == "1");
                if (!(pocoHaveCustomsREF || pocoHaveSent))
                {
                    return;
                }
                bool pmHaveCustomsREF = (!String.IsNullOrWhiteSpace(entityPM.CustomsDocId));
                bool pmHaveSent = (entityPM.DocumentStatusCode == "1");
                bool toLog = false;
                if (!pmHaveSent) { toLog = true; }
                if (!pmHaveCustomsREF) { toLog = true; }
                if (!toLog) return;
                string jsonPM = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPM);
                string jsonPOCO = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(EntityPOCO);


                var sb = new StringBuilder();
                sb
                    .AppendLine("**Stack:")
                    .AppendLine(Environment.StackTrace)
                    .AppendLine("**PM:New:")
                    .AppendLine(jsonPM)
                    .AppendLine("**POCO:old:")
                    .AppendLine(jsonPOCO);

                LogitudeSettings.HandleLogMe
                    //(mess, err, suffix, stopLogAt)
                    (sb.ToString(), false, "CustomsDocumentSendLog", stopLogAt);
            }
            catch (Exception)
            {


            }

        }

        public const string LoadTestSendMessageToQueue = "LoadTestSendMessageToQueue";
        public const string WhileAnalayzeCostomResponseSendDEC = "WhileAnalayzeCostomResponseSendDEC";
        public const int HugeFileSizeSendToDCA = 10 * 1000000;
        public const int MaxFileSizeDONOTSendToDCA = 200 * 1000000;

        protected override void OnUpdating(CustomsDocumentPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            
            CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQueryService = new CustomsDocumentMetaDataValueQueryService(context);
            CustomDocumentTypeMetaDataQueryService customDocumentTypeMetaDataQueryService = new CustomDocumentTypeMetaDataQueryService(context);
            //List<CustomsDocumentMetaDataValuePM> metadataValues = customsDocumentMetaDataValueQueryService.GetCustomDocumentMetaDataValues(entityPM.DocumentsFilingId, entityPM.Tenant);
            List<CustomDocumentTypeMetaDataPM> documentTypeMetaDatas = customDocumentTypeMetaDataQueryService.GetCustomDocumentTypeMetaDataByType(entityPM.DocumentTypeCode);
            List<CustomDocumentTypeMetaDataPM> requireddocumentTypeMetaDatas = documentTypeMetaDatas.Where(d => d.Mandatory).ToList();
           
          

            bool ready = true;
            if (requireddocumentTypeMetaDatas.Count == 0)
            {
                ready = false;
            }
            foreach (CustomDocumentTypeMetaDataPM documentTypeMetaData in requireddocumentTypeMetaDatas)
            {
                CustomsDocumentMetaDataValuePM value = entityPM.CustomsDocumentMetaDataValues.Where(d => d.MetaDataTypeCode == documentTypeMetaData.MetaDataTypeCode).FirstOrDefault();
                if (value == null)
                {
                    ready = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(value.MetaDataValue))
                    {
                        ready = false;
                    }
                }
            }
            if (_AddPerfectCustomsDocumentMetaDataValues_IsMetaDataReady)
            {
                if (requireddocumentTypeMetaDatas.Count == 0)
                {
                    ready = true;
                }
            }
                if (ready)
            {
                entityPM.IsMetaDataReady = true;

            }
            //if (true) //itzik + yaron  entityPM.DocumentVersion != EntityPOCO.DocumentVersion)
            //{
            //    entityPM.ExternalAttachmentId = entityPM.DocumentsFilingCode + "-" + entityPM.DocumentVersion;
            //}

            DocumentsFilingRepository documentInRep = new DocumentsFilingRepository(entityPM.Tenant);
            DocumentsFiling documentIn = documentInRep.GetSingleDocumentsFiling(entityPM.DocumentsFilingId, entityPM.Tenant);
            if (true && documentIn != null) // mohammad moved this from the previous place and took the document code from document in wi 14611
            {
                entityPM.ExternalAttachmentId = documentIn.Code + "-" + entityPM.DocumentVersion;//entityPM.DocumentsFilingCode + "-" + entityPM.DocumentVersion;
            }
            ObjectTableRepository objecttableRep = new ObjectTableRepository(0);
            ObjectTable objectTable = objecttableRep.GetObjectTableById(documentIn.ObjectTableId, 0);
#if true//cloudExc 11:22 ‎24/‎08/‎2016
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
            if (objectTable == null && !string.IsNullOrWhiteSpace(entityPM.DeclarationId))
            {
                objectTable = objecttableRep.GetObjectTableByName("Customs.Declaration", entityPM.Tenant, true);
                UpdateObjectTableIfDeclaration(entityPM, documentInRep, documentIn, objectTable, declarationQueryService);

            }
#endif
            if (objectTable != null)
            {
                if (objectTable.Name == "Customs.Declaration")
                {
#if false//cloudExc 11:22 ‎24/‎08/‎2016
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
#endif
                    if (String.IsNullOrWhiteSpace(entityPM.DeclarationId))
                    {
                        var declarationId = documentIn.EntityId;
                        if (String.IsNullOrWhiteSpace(declarationId))
                        {
                            UpdateObjectTableIfDeclaration(entityPM, documentInRep, documentIn, objectTable, declarationQueryService);
                        }


                        entityPM.DeclarationId = declarationId;
                    }
                    if (!string.IsNullOrWhiteSpace(entityPM.DeclarationId))
                    {
                        /// if (false)
                        {// //-------------------- mohammad task 33591 prevent concurrency error for client------------------------//
                         //DeclarationPM declaration = declarationQueryService.GetSingle(entityPM.DeclarationId, false, false);
                         //if (declaration != null)
                         //{
                         //    declaration.MarkAsChanged = true;
                         //    declaration.IsChanged = true;
                         //    declaration.ChangeSetOp = ChangeSetOperation.Update;
                         //    DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                         //    declarationUpdateService.Update(declaration, true);
                         //}


                            DeclarationRepository declarationRep = new DeclarationRepository(context);
                            Declaration declaration = declarationRep.GetSingle(entityPM.DeclarationId, entityPM.Tenant);
                            if (declaration != null)
                            {
                                if (!declaration.IsChanged)
                                {
                                    //declaration.IsChanged = true;
                                    //declarationRep.Update(declaration);
                                    //declarationRep.SubmitChanges();
                                    declarationRep.SetIsChangedAndSubmitChanges(declaration);
                                }
                            }


                            //-------------------------------------------------------------------------------------------------------------//
                        }

                    }
                    else
                    {
                        //to  throw  ??? :I should ask yaron - i asume not 
                    }
                }
            }
            entityPM.ExternalEntityReference = documentIn.ExternalEntityReference;
            entityPM.ExternalEntityName = documentIn.ExternalEntityName;
            // this.UpdateIsPartOfDeclaration(entityPM, documentIn);

        }

        private static void UpdateObjectTableIfDeclaration(CustomsDocumentPM entityPM, DocumentsFilingRepository documentInRep, DocumentsFiling documentIn, ObjectTable objectTable, DeclarationQueryService declarationQueryService)
        {

            if (documentIn.ExternalEntityName == "CFIFILEM" && !string.IsNullOrWhiteSpace(documentIn.ExternalEntityReference))
            {
                //var declarationQueryService = new DeclarationQueryService(customContext);
                var declarationId = declarationQueryService.GetIdByCustomFileNo(documentIn.ExternalEntityReference, entityPM.Tenant);
                documentIn.EntityId = declarationId;
                //ObjectTabelRepository objecttableRep = new ObjectTabelRepository(0);
                //ObjectTable objectTable = objecttableRep.GetObjectTableByName("Customs.Declaration", 0, true);
                documentIn.ObjectTableId = objectTable.Id;
                documentInRep.Update(documentIn);
                documentInRep.SubmitChanges();
            }


        }

        public const string AvoidSendToCustoms = "Logitude.Customs.BL.EntityUpdateServices.CustomsDocumentUpdateService.AvoidSendToCustoms";

        //protected override void AfterUpdating(CustomsDocumentPM entityPM, EntityPM entityParentPM)

        public bool IgnoreSendFailure = false;
        private bool _AddPerfectCustomsDocumentMetaDataValues_IsMetaDataReady;

        void TrySendMessageToQueue(CustomsDocumentPM entityPM, bool forceDueLoadTest = false)
        {
            var send = false;

            if (entityPM.DocumentStatusCode != "7" && entityPM.IsSendToQueue)
            {
                if (entityPM.IsMetaDataReady)
                {

                    send = true;
                    //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{

                    //scope.Complete();
                    //}
                }
            }

            if (!string.IsNullOrWhiteSpace(entityPM.CollateralId)
                //&& !string.IsNullOrWhiteSpace(entityPM.CustomsDocId)) 
                && string.IsNullOrWhiteSpace(entityPM.CustomsDocId))
            // Send automatically if from Collateral screen
            {
                send = true;
            }

            if (send || forceDueLoadTest)
            {
                if (SendMessageToQueue(entityPM, forceDueLoadTest))
                {
                    if (forceDueLoadTest)
                    {
                        LogMessagingUtil.Instance.AppendLine("forceDueLoadTest  >>> Sended");
                    }
                    entityPM.DocumentStatusCode = "7";
                }
                else
                {
                    if (forceDueLoadTest)
                    {
                        LogMessagingUtil.Instance.AppendLine("forceDueLoadTest  >>> Not Send !!!!!!!");
                    }
                }

            }
        }

        public bool SendMessageToQueue(CustomsDocumentPM entityPM, bool forceDueLoadTest = false)
        {
            bool send = false;
            bool sendWithCustomsDocId = false;
            try
            {


                if (AvoidSendToCustoms == entityPM.CurrentContextTag)
                {
                    return send;
                }
                if (!string.IsNullOrWhiteSpace(entityPM.CollateralId))
                {
                    sendWithCustomsDocId = true;
                }
                if (!String.IsNullOrWhiteSpace(entityPM.CustomsDocId) && !sendWithCustomsDocId)
                {
                    //ALREADY SEND TO MEHES AND RECIVE REF :entityPM.CustomsDocId
                    LogMessagingUtil.Instance.AppendLine("Customs Document already sent to Customs");
                    return send;
                }
                if (String.IsNullOrWhiteSpace(entityPM.DocumentTypeCode))
                {
                    LogMessagingUtil.Instance.AppendLine("DocumentTypeCode is null ,disconnect - dont sent 2 mehes ");
                    return send;
                }
                var currentCustomsDocumentsTicketId = entityPM.CurrentCustomsDocumentsTicketId;

                ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant); //this.currentContext 
                var customContext = MainContext as ICustomContext;
                var repo = new Logitude.Customs.Data.Repsitories.CustomsDocumentPointerRepository(entityPM.Tenant);
                var declarationId = entityPM.DeclarationId;
                var collateralId = entityPM.CollateralId;
                var customsDocumentQueryService = new CustomsDocumentQueryService(customContext);
                var concurCheck = customsDocumentQueryService.GetSingle(entityPM.DocumentsFilingId, false, false);
                if (concurCheck != null && !forceDueLoadTest)
                {
                    if (!String.IsNullOrWhiteSpace(concurCheck.CustomsDocId) && !sendWithCustomsDocId)
                    {
                        LogMessagingUtil.Instance.AppendLine("Customs Document already sent to Customs");
                        return send;
                    }
                }
                if (!string.IsNullOrWhiteSpace(collateralId))  ///&& String.IsNullOrWhiteSpace(declarationId))
                {
                    var customsCollateralQueryService = new CustomsCollateralQueryService(customContext);
                    var customsCollateralPM = customsCollateralQueryService.GetSingle(collateralId, false, false);
                    if (customsCollateralPM != null)
                    {
                        declarationId = customsCollateralPM.DeclarationId;
                    }

                }
                if (String.IsNullOrWhiteSpace(declarationId) && !string.IsNullOrWhiteSpace(entityPM.CustomsDocId))
                {

                    var fromDb = customsDocumentQueryService.GetSingleCustomsDocumentPMWithDeclarationId(entityPM.CustomsDocId, entityPM.Tenant);

                    if (fromDb != null)
                    {
                        declarationId = fromDb.DeclarationId;
                    }

                }
                if (String.IsNullOrWhiteSpace(declarationId))//move up ??
                {
                    if (entityPM.ExternalEntityName == "CFIFILEM" && !string.IsNullOrWhiteSpace(entityPM.ExternalEntityReference))
                    {
                        var declarationQueryService = new DeclarationQueryService(customContext);
                        declarationId = declarationQueryService.GetIdByCustomFileNo(entityPM.ExternalEntityReference, entityPM.Tenant);
                    }
                }
                //if (String.IsNullOrWhiteSpace(declarationId))
                //{
                //    throw new Exception("CustomsDocument must conncted to declaration "); ////task10676  yaron said it must be conncted to declaration !!

                //}
                var requestParams = new Logitude.CustomsMessaging.Common.RequestParams.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam()
                {
                    ///AppicationId = entityPM.DocumentsFilingId,
                    DocumentsFilingId = entityPM.DocumentsFilingId,
                    DeclaretionId = declarationId,
                    DocumentsTicketId = entityPM.CurrentCustomsDocumentsTicketId,
                    Tenant = entityPM.Tenant,
                    LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),//task10676 
                    LoggingEntityId = declarationId,
                    LoggingObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument"),//task10676 
                    LoggingEntityId2 = entityPM.DocumentsFilingId,
                };
                if (String.IsNullOrWhiteSpace(declarationId) && !String.IsNullOrWhiteSpace(entityPM.ClaimId))
                {
                    requestParams.LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Claim");
                    requestParams.LoggingEntityId = entityPM.ClaimId;//ObjectTableRepository.GetObjectTableByName("Customs.Declaration");


                }
                byte[] byteArray = null;
                var documentrepository = new DocumentRepository(commonContext);
                var document = documentrepository.GetSingleDocument(entityPM.Tenant, entityPM.DocumentsFilingId);
                var mySBQMessage = new SBQMessageService();

                requestParams.LoggingUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant); ;
                requestParams.RequestName = "CustomsDocument Request";
                requestParams.ResponseName = "שליחת צרופה " + entityPM.ExternalAttachmentId;
                requestParams.InterfaceTypeCode = "2715";
                ;
                if (Environment.MachineName.ToLower().Contains("itzik") && DateTime.Now < new DateTime(2016, 5, 10))
                {
                    AmitalDebuggerUtil.Break();
                    requestParams.ForcePersonalSign = true;
                }
                var my9mb = 9000000;
                //var my3mb = 3000000;
                
                
                //if (entityPM.FileSize.HasValue && entityPM.FileSize.GetValueOrDefault() > my3mb)
                //{
                //    SendDCA(requestParams);
                //}
                //else
                {
                    var hugeFile = false;
                    var repo1 = new GDMFILEVERRepository(requestParams.Tenant);
                    var list = repo1.GetList(requestParams.DocumentsFilingId);
                    if (list.Count > 0)
                    {
                        var lastVer = list.Max(r => r.VERSION);
                        var lastGDMFILEVER = list.First(r => r.VERSION == lastVer);
                        //9558452
                        //7000000
                        if (lastGDMFILEVER.FILESIZE > HugeFileSizeSendToDCA)
                        {
                            if (lastGDMFILEVER.FILESIZE > MaxFileSizeDONOTSendToDCA)
                            {
                                throw new Exception("המסמך מעל 200MB - לא תתאפשר שליחה");
                            }
                            hugeFile = true;

                        }
                    }
                    else
                    {

                        if (entityPM.FileSize.HasValue && entityPM.FileSize.GetValueOrDefault() > HugeFileSizeSendToDCA)
                        {
                            hugeFile = true;
                        }
                    }
                    if (hugeFile)
                    {
                        SendDCA(requestParams);
                    }
                }

                var _CustomsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(requestParams.Tenant);

                //var   listRequestInProgress = _CustomsRequestsSheetQueryService.GetRequestInProgress(
                //            requestParams.Tenant, requestParams.InterfaceTypeCode,
                //        ObjectTabelRepository.GetObjectTableByName("Customs.Declaration"), requestParams.DeclaretionId,
                //        ObjectTabelRepository.GetObjectTableByName("Customs.CustomsDocument"), requestParams.DocumentsFilingId, 
                //        null);
                //if (listRequestInProgress != null && listRequestInProgress.Count >0)
                //{
                //    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("2715 RequestInProgress stop create a new one !! ");
                //    return;
                //}

                try
                {
                    SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam>(requestParams
                        , false
                        );
                    send = true;
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("2715 RequestInProgress stop create a new one !! " + myCustomsRequestsSheetServiceException.Message);
                        return send;
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        
                    }
                    throw;
                }
            }
            catch (Exception e)
            {
                if(!IgnoreSendFailure)
                {
                    e.ChangeExceptionMessage(@"שליחת מסמך למכס נכשל" + Environment.NewLine);
                    throw e;
                }
            }
            return send;


        }

        private static void SendDCA(CustomsMessaging.Common.RequestParams.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams)
        {
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("2715 via DCABatch ,isBig  entityPM.FileSize.GetValueOrDefault() > my3mb ");
            requestParams.RequestName = "SendRequestVIA.DCABatch :CustomsDocument ";
            requestParams.RequestVIA = CustomsMessaging.Common.RequestParams.SendRequestVIA.DCABatch;
        }

        public void UpdateIsPartOfDeclaration(CustomsDocumentPM entityPM, DocumentsFiling documentIn)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsDocumentPointerQueryService CustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
            List<CustomsDocumentPointerPM> pointers = CustomsDocumentPointerQueryService.GetParentDocumentPointer(entityPM.DeclarationId, "Declaration", entityPM.Tenant);

            if (pointers != null)
            {
                if (pointers.Count > 0)
                {
                    entityPM.IsPartOfDeclaration = true;
                }
            }
        }

        private void UpdateDeclarationCourierStatus(CustomsDocumentPM entityPM)
        {

            if (!string.IsNullOrWhiteSpace(entityPM.DeclarationId) && entityPM.DocumentStatusCode == "7")
            {
                ICustomContext context = MainContext as CustomContext;
                DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(entityPM);
                if (connectedDeclarationPM != null && connectedDeclarationPM.IsCourierDeclaration)
                {
                    string status = "I";
                    
                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), connectedDeclarationPM.Tenant);
                        DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                        DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(connectedDeclarationPM.Id, true, false);
                        if (currentDeclarationCourierStatusPM == null)
                        {
                            currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                            {
                                DeclarationId = connectedDeclarationPM.Id,
                                Tenant = connectedDeclarationPM.Tenant,
                                IsClosedForFollowUp = false,
                                IsCourierMissingClassification = false,
                            };
                            currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        currentDeclarationCourierStatusPM.DocumentStatusCode = status;
                        declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                    }
                }
            }
        }



        private void UpdateDeclarationCourierStatus380(CustomsDocumentPM entityPM)
        {

            if (entityPM.DocumentTypeCode == "380")
            {
                ICustomContext context = MainContext as CustomContext;
                DeclarationPM connectedDeclarationPM = GetConnectedDeclarationPM(entityPM);
                if (connectedDeclarationPM != null && connectedDeclarationPM.IsCourierDeclaration)
                {
                    string status = "X";
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), connectedDeclarationPM.Tenant);
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                    DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(connectedDeclarationPM.Id, true, false);
                    if (currentDeclarationCourierStatusPM == null)
                    {
                        currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                        {
                            DeclarationId = connectedDeclarationPM.Id,
                            Tenant = connectedDeclarationPM.Tenant,
                            IsClosedForFollowUp = false,
                            IsCourierMissingClassification = false,
                        };
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    currentDeclarationCourierStatusPM.DocumentStatusCode = status;
                    currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }
            }
        }




        private DeclarationPM GetConnectedDeclarationPM(CustomsDocumentPM dirtyEntityPM)
        {
            DeclarationPM myDBEntity = null;
            var declarationQueryService = new Logitude.Customs.BL.EntityQueryServices.DeclarationQueryService(dirtyEntityPM.Tenant);
            if (dirtyEntityPM.DeclarationId != null)
            {
                myDBEntity = declarationQueryService.GetSingle(dirtyEntityPM.DeclarationId, false, false);
            }
            return myDBEntity ?? new DeclarationPM();
        }
    }
}

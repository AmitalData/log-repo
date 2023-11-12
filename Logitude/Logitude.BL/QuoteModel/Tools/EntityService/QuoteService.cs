using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityOtherServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.BL.QuoteModel.Tools.TraceEvents;
using Logitude.BL.QuoteModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using System.ComponentModel.DataAnnotations;
using Logitude.BL.QuoteModel.APIDataContract;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.BL.QuoteModel.Tools.Initializers;
using Logitude.BL.ExternalService;
using Logitude.BL.QuoteModel.Tools.Behaviours;
using Simplog.Data.InfrastructureModel;
using System.Reflection;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.CustomFields;
using Logitude.BL.AnalyticTableServices;
using Newtonsoft.Json;
using System.Text;
using Logitude.Server.Tools.QueueService;
using System.Transactions;
using Logitude.Server.Tools.StorageService;
using Logitude.BL.Interfaces;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public partial class QuoteService
    {
        private int tenant;
        private bool isNewEntity;
        private QuotePM entityPM;
        public Quote entityPoco { get; set; }
        public QuoteComputedField quoteComputedFieldEntityPOCO { get; set; }
        private Tenant loggedTenant;
        //private ContactPM loggedContact;
        //private IQuotesContext objectContext;
        private QuoteRepository entityRepository;
        private QuoteQuery quoteQuery;
        private QuoteComputedFieldRepository quoteComputedFieldRepository;
        private FollowUpRepository followUpRepository;
        private QuoteChargeRepository quoteChargeRepository;
        private QuotePriceStepsRepository quotePriceStepsRepository;
        private QuotePackageRepository quotePackageRepository;
        private QuoteTotalVATRepository quoteTotalVATRepository;
        private QuoteDocumentVersionRepository quoteDocumentVersionRepository;
        private ICommonDataContext myCommonContext;
        private AddressRepository addressRepository;
        private QuoteSetting iQuoteSetting;
        private bool isAdhoc;
        private bool isLCLQuote;
        private bool isFCLQuote;
        private bool isInlandDomestic;
        private QuoteServiceInitializer initializer;
        private QuoteFollowUpUpdateService quoteFollowUpUpdateService;
        public QuoteService(IQuotesContext objectContext, int tenant)
        {
            this.initializer = new QuoteServiceInitializer(objectContext, tenant, HttpContext.Current.User.Identity.Name);
            this.initializer.Initialize();

            this.tenant = initializer.Tenant;
            //this.objectContext = objectContext;
            this.myCommonContext = initializer.CommonContext; //CommonDataContext.GetContext(tenant);
            this.entityRepository = initializer.Repository; //new QuoteRepository(objectContext);
            this.quoteComputedFieldRepository = new QuoteComputedFieldRepository(tenant);
            this.quoteChargeRepository = new QuoteChargeRepository(objectContext);
            this.quotePriceStepsRepository = new QuotePriceStepsRepository(objectContext);
            this.followUpRepository = new FollowUpRepository(tenant);
            this.quotePackageRepository = new QuotePackageRepository(objectContext);
            this.quoteTotalVATRepository = new QuoteTotalVATRepository(objectContext);
            this.addressRepository = new AddressRepository(myCommonContext);
            this.loggedTenant = initializer.LoggedTenant; //TenantRepository.GetSingleTenant(tenant, false);
            this.quoteQuery = new QuoteQuery(this.entityRepository);
            //this.GetLoggedContact(HttpContext.Current.User.Identity.Name);
        }
        public QuoteService(IQuotesContext objectContext, int tenant, string email)
        {
            this.initializer = new QuoteServiceInitializer(objectContext, tenant, email);
            this.initializer.Initialize();

            this.tenant = initializer.Tenant;
            //this.objectContext = objectContext;
            this.myCommonContext = initializer.CommonContext; //CommonDataContext.GetContext(tenant);
            this.entityRepository = initializer.Repository; //new QuoteRepository(objectContext);
            this.quoteChargeRepository = new QuoteChargeRepository(objectContext);
            this.quoteComputedFieldRepository = new QuoteComputedFieldRepository(tenant);
            this.quotePriceStepsRepository = new QuotePriceStepsRepository(objectContext);
            this.followUpRepository = new FollowUpRepository(tenant);
            this.quotePackageRepository = new QuotePackageRepository(objectContext);
            this.quoteTotalVATRepository = new QuoteTotalVATRepository(objectContext);
            this.addressRepository = new AddressRepository(myCommonContext);
            this.loggedTenant = initializer.LoggedTenant; //TenantRepository.GetSingleTenant(tenant, false);
            this.quoteQuery = new QuoteQuery(this.entityRepository);
            //this.GetLoggedContact(email);
        }

        //private void GetLoggedContact(string serviceContextUser)
        //{
        //    //ContactQuery contactQuery = new ContactQuery(tenant);
        //    //this.loggedContact = contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);

        //    //if (this.loggedContact == null)
        //    //{
        //    //    loggedContact = contactQuery.GetContactByEmailOnly(serviceContextUser, tenant);
        //    //}

        //    //if (this.loggedContact == null)
        //    //{
        //    //    loggedContact = contactQuery.GetSinglePM(entityPM.CreatedByUserId, tenant);
        //    //}
        //}

        private List<QuoteChargePM> quoteChargesChangeSet;
        private List<QuoteFollowUpPM> quoteFollowUpsChangeSet;
        private List<QuotePackagePM> quotePackageChangeSet;
        private List<QuoteDocumentVersionPM> quoteDocumentVersionChangeSet;
        public void SetChangeSet(List<QuoteChargePM> quoteChargesChangeSet, List<QuoteFollowUpPM> quoteFollowUpsChangeSet, List<QuotePackagePM> quotePackageChangeSet, List<QuoteDocumentVersionPM> quoteDocumentVersionChangeSet)
        {
            this.quoteChargesChangeSet = quoteChargesChangeSet;
            this.quoteFollowUpsChangeSet = quoteFollowUpsChangeSet;
            this.quotePackageChangeSet = quotePackageChangeSet;
            this.quoteDocumentVersionChangeSet = quoteDocumentVersionChangeSet;
        }

        public void Create(QuotePM entityPM)
        {
            initializer.InitializeEntity(entityPM);
            this.entityPM = initializer.EntityPM;
            this.entityPoco = initializer.EntityPOCO;
            this.quoteComputedFieldEntityPOCO = initializer.QuoteComputedFieldPOCO;
            this.isNewEntity = initializer.IsNewEntity;

            this.GetQuoteSettings();
            this.InitializeComponent();
            initializer.HandleBehaviours();

            QuotetValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext);

            foreach (QuoteFollowUpPM itemPM in entityPM.FollowUps)
            {
                this.CreateQuoteFollowUp(itemPM);
            }

            foreach (QuoteChargePM itemPM in entityPM.QuoteCharges)
            {
                this.CreateQuoteChargeUp(itemPM);
            }

            foreach (QuotePackagePM itemPM in entityPM.QuotePackages)
            {
                this.CreateQuotePackage(itemPM);
            }

            this.UpdateTotalVats();
            QuoteTracing.Trace(entityPM, entityPoco, initializer.LoggedContactId, isNewEntity);

            this.SaveChildEntitiesCustomFields();

            EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = entityPoco, EntityPM = entityPM, OldEntityPM = new QuotePM(), AutomationType = "OnCreate", ObjectTableName = "Quote", Tenant = entityPM.Tenant, EntityId = entityPM.Id, EntityReference = entityPM.QuoteNumber});
            entityAutomationService.RunAutomation();
            QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
            quoteComputedFieldRepository.Add(quoteComputedFieldEntityPOCO);
            quoteComputedFieldRepository.SubmitChanges();
            followUpRepository.SubmitChanges();

            this.GetForeignFields(entityPM, entityPoco);

            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Quote", 0, true);
            ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "N", initializer.LoggedContactId);
            entityAutomationService.RunAutomationThatDependencyOnLastEntityUpdate();

            new QuoteAnalyticTableService(initializer.Context.GetActiveDbContext()).AddUpdate(entityPoco, tenant);
        }

        private void SaveChildEntitiesCustomFields()
        {
            new ChildEntitiesCustomFieldService().Update(new ChildEntitiesCustomFieldArgs()
            {
                Tenant = entityPM.Tenant,
                EntityId = entityPM.Id,
                ObjectTableName = "Quote",
                ChildObjectTableName = "QuotePackage",
                ChildEntities = entityPM.QuotePackages.Cast<object>().ToList()
            });

            new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = entityPM, ParentEntityId = entityPM.Id, ParentObjectTableName = "Quote", Tenant = tenant }).Update();

        }
        public class QuoteChangeTracking
        {
            public QuotePM ChangeTrackingPM { get; set; }
            public string EntityChangeFieldXml { get; set; }
            public List<NotifyPropertyChangeValues> NotifyPropertyChangeValuesLists { get; set; }
        }

        private void ComputeProfit()
        {
            if (entityPM.EstimateProfit != null && entityPM.ExchangeRate != null)
            {
                entityPM.EstimatedProfitInLocal = MethodHelper.Round(entityPM.EstimateProfit * entityPM.ExchangeRate, 2);
                entityPM.EstimatedProfitInProfit = MethodHelper.Round(entityPM.EstimatedProfitInLocal / entityPM.ProfitExchangeRate, 2);
            }
        }

        public void Update(QuotePM entityPM, bool mapComposition = false)
        {
            initializer.InitializeEntity(entityPM);
            this.entityPM = initializer.EntityPM;
            this.entityPoco = initializer.EntityPOCO;
            this.quoteComputedFieldEntityPOCO = initializer.QuoteComputedFieldPOCO;
            this.isNewEntity = initializer.IsNewEntity;

            if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
            {
                if (mapComposition)
                {
                    this.quoteChargesChangeSet = this.entityPM.QuoteCharges;
                    foreach (var itemPM in this.entityPM.QuoteCharges)
                    {
                        itemPM.QuoteChargePriceStepsChangeSet = itemPM.QuoteChargePriceSteps;
                    }

                    this.quotePackageChangeSet = this.entityPM.QuotePackages;
                    this.quoteFollowUpsChangeSet = this.entityPM.FollowUps;
                    this.quoteDocumentVersionChangeSet = this.entityPM.QuoteDocumentVersions;
                }

                this.InitializeComponent();

                initializer.HandleBehaviours();

                QuotetValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext);

                this.UpdateQuoteChargesCollection();
                this.UpdateQuoteFollowUpsCollection();
                this.UpdateQuotePackageCollection();
                if (FeatureToggleHelper.HasFeatureToggle("UQD", tenant))
                {
                    string communicationLogId = WriteEntityPMOnCommunicationLog(entityPM);
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("DocumentsExecutionQueue", entityPM.Tenant);
                    queueservice.Send(new Dictionary<string, string>() { { "Tenant", entityPM.Tenant.ToString() }, { "communicationLogId", communicationLogId } }, entityPM.Tenant, null, null);
                    entityPM.CommunicationLogId = communicationLogId;
                }
                else {
                    this.UpdateQuoteDocumentVersionCollection();
                }
                
                this.UpdateTotalVats();

                QuoteTracing.Trace(entityPM, entityPoco, initializer.LoggedContactId, isNewEntity);

                ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPoco.Tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Quote", 0, true);

                if (entityPM.QuoteTypeCode != "P")
                {
                    if (!entityPM.DontExportQuotationsToIntegratedSystem)
                    {
                        SentQuoteStatusMessageToUnifreight(objecttable.Id);
                    }

                    SendQuoteToIntegratedSystem(objecttable.Id);
                }

                this.SaveChildEntitiesCustomFields();

                EntityAutomationService entityAutomationService = new EntityAutomationService(new EntityAutomationArgs() { Poco = entityPoco, EntityPM = entityPM, OldEntityPM = new QuotePM(), AutomationType = "OnUpdate", ObjectTableName = "Quote", Tenant = entityPM.Tenant, EntityId = entityPM.Id, EntityAutomationMappingPMFields = new EntityAutomationQuoteMappingPMFields(), EntityReference = entityPM.QuoteNumber });
                entityAutomationService.RunAutomation();
                QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);

                entityRepository.Update(entityPoco);
                entityRepository.SubmitChanges();
                quoteComputedFieldRepository.Update(quoteComputedFieldEntityPOCO);
                quoteComputedFieldRepository.SubmitChanges();
                followUpRepository.SubmitChanges();

                this.GetForeignFields(entityPM, entityPoco);

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Quote");
                ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "U", initializer.LoggedContactId);
                new QuoteAnalyticTableService(initializer.Context.GetActiveDbContext()).AddUpdate(entityPoco, tenant);
            }

            else
            {
                // cancelled quotes
                // in case follow ups added
                // from client
                int indexComp = 0;
                if (quoteFollowUpsChangeSet != null)
                {
                    foreach (QuoteFollowUpPM item in quoteFollowUpsChangeSet)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            indexComp++;
                            item.Id = "QuoteFollowUpPM_" + indexComp;
                        }
                    }
                }
            }

            if (entityPM.IsCancelled)
            {
                List<FollowUp> allFollowupLists = this.followUpRepository.GetFollowUpsByQuoteId(entityPM.Id, entityPM.Tenant);
                foreach (FollowUp item in allFollowupLists)
                {
                    followUpRepository.Remove(item);
                }

                followUpRepository.SubmitChanges();

                entityPM.FollowUps = new List<QuoteFollowUpPM>();
            }

            this.quoteFollowUpUpdateService = new QuoteFollowUpUpdateService(entityPM, entityPM.Tenant);
            quoteFollowUpUpdateService.RefreshFollowUps();
        }
        private string WriteEntityPMOnCommunicationLog(QuotePM quotePM)
        {
            string jsonString = JsonConvert.SerializeObject(quotePM);
            byte[] xmlFile = Encoding.UTF8.GetBytes(jsonString);
            return Communications.AddCommunicationLog(new CommunicationsParams()
            {
                LoggingEntityId = quotePM.Id,
                Tenant = quotePM.Tenant,
                CommunicationLogTypeCode = "Q",
                Priority = 1,
                InOut = "O",
                Status = "W",
                Subject = "Update Quote Document",
                FolderName = "Other",
                ByteData = xmlFile
            });
        }
        public QuotePM DisconnectQuoteFromOpportunity(string quoteId)
        {
            if (string.IsNullOrEmpty(quoteId))
                return null;

            QuotePM entityPM = this.quoteQuery.GetSinglePM(quoteId, tenant);
            if (entityPM == null)
                return null;

            entityPM.OpportunityId = null;
            this.SetChangeSet(new List<QuoteChargePM>(), new List<QuoteFollowUpPM>(), new List<QuotePackagePM>(), new List<QuoteDocumentVersionPM>());
            this.Update(entityPM);

            return entityPM;
        }
        public byte[] BuildQuoteTemplatePdfDocument(string quoteId, int versionNumber, string quoteTemplateId, string updatedByUserId, int tenant, bool isGenerate) {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentRep = new DocumentRepository(commonContext);
            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);

            IQuotesContext objectContext = QuotesContext.GetContext(tenant);
            QuoteDocumentVersionRepository quoteDocumentVersionRep = new QuoteDocumentVersionRepository(objectContext);
            QuoteRepository quoteRep = new QuoteRepository(objectContext);
            QuoteQuery quoteQuery = new QuoteQuery(new QuoteRepository(objectContext));
            string documentTypeId = documentTypeRepository.GetDocumentTypeIdByCode("QUOTE", tenant);
            byte[] pdfData = null;
            if (!string.IsNullOrEmpty(documentTypeId))
            {

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    QuoteDocumentVersion version = quoteDocumentVersionRep.GetSingleQuoteDocumentVersion(quoteId, tenant, versionNumber);
                    QuotePM quotePM = quoteQuery.GetSinglePM(quoteId, tenant);
                    QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(tenant);

                    string defult = !isGenerate ? quotePM.QuoteTemplateId : null;
                    List<string> templateSectionsIds = quoteTemplateSectionQuery.GetQuoteTemplateSectionIdsByQuoteTemplateId(quoteTemplateId, quoteId, tenant, defult, quotePM.QuotationSections);
                    string sectionsIds = "";
                    foreach (string sectionId in templateSectionsIds)
                    {
                        sectionsIds += (sectionId + ",");
                    }

                    sectionsIds = sectionsIds.Remove(sectionsIds.Length - 1);

                    if (quotePM.QuoteTemplateId != quoteTemplateId || quotePM.QuotationSections != sectionsIds || quotePM.LastVersionNumber != version.VersionNumber)
                    {
                        quotePM.LastVersionNumber = version.VersionNumber;
                        quotePM.QuoteTemplateId = quoteTemplateId;
                        quotePM.QuotationSections = sectionsIds;


                    }

                    IQuoteTemplateReportHelper helper = ContainerAccessor.Container.Resolve(typeof(IQuoteTemplateReportHelper), "QuoteTemplateReportHelper", new ParameterOverride("", 1)) as IQuoteTemplateReportHelper;
                    pdfData = helper.BuildQuoteTemplatePdfReport(quoteId, quoteTemplateId, updatedByUserId, tenant, null, null, quotePM);
                    //Update QuoteHTMLDocumentId;
                    SetChangeSet(new List<QuoteChargePM>(), new List<QuoteFollowUpPM>(), new List<QuotePackagePM>(), new List<QuoteDocumentVersionPM>());
                    Update(quotePM);

                    Simplog.Data.CommonDataModel.EntityPOCOs.Document document = documentRep.GetSingleDocument(tenant, version.DocumentId);

                    document.FileSize = Convert.ToInt32(pdfData.Length);
                    document.Extension = "pdf";
                    document.CalculatedFileName = "Quotation-" + quotePM.QuoteNumber + "-" + version.VersionNumber;
                    document.IsEncrypted = true;
                    documentRep.Update(document);


                    version.VersionType = "G";
                    version.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    version.UpdatedByUserId = updatedByUserId;
                    version.QuoteTemplateId = quoteTemplateId;


                    DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(quoteId, documentTypeId, tenant);
                    documentout.IsBlobExist = true;
                    documentout.Issued = true;
                    documentout.DocumentsFiling.UpdatedByUserId = updatedByUserId;
                    documentout.DocumentsFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    documentOutRepository.Update(documentout);


                    commonContext.SaveChanges();

                    quoteDocumentVersionRep.Update(version);

                    objectContext.SaveChanges();


                    string filename = document.Id + "." + document.Extension;
                    string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = pdfData.Length,

                    };
                    storageservice.Write(pdfData, fileInfo);


                    scope.Complete();


                }

                return pdfData;
            }
            else
            {
                throw new Exception("Document Type with code 'QUOTE' is not found!");
            }
        }

        private void GetQuoteSettings()
        {
            QuoteSettingRepository iQuoteSettingRepository = new QuoteSettingRepository(initializer.Context);
            this.iQuoteSetting = iQuoteSettingRepository.GetSingleQuoteSetting(tenant);

            if (iQuoteSetting != null)
            {
                if (!entityPM.IsCopy)
                {
                    this.entityPM.IsSaleCurrencySameAsCost = iQuoteSetting.IsSaleAsCostCurrency;
                    this.entityPM.IsMultiCurrency = iQuoteSetting.IsMultiCurrency;
                }
            }
        }

        public void SentQuoteStatusMessageToUnifreight(string objectTableId)
        {
            if (CanSendQuoteToIntegratedSystem())
            {
                bool IsQuoteStageChange = false;
                if (entityPM.StageId != entityPoco.StageId || entityPM.IsCancelled)
                {
                    TraceEventRepository traceEventRepository = new TraceEventRepository(entityPM.Tenant);
                    QuoteStatus quoteStatus = new QuoteStatus();
                    quoteStatus.Stage = new APIDataContract.Stage();
                    quoteStatus.QuoteDeclineReason = new QuoteDeclineReason();
                    quoteStatus.QuoteCancelNote = "";
                    quoteStatus.QuoteNumber = entityPM.QuoteNumber;
                    quoteStatus.DueDate = entityPM.StageDueDate;

                    #region IsCancelled 
                    if (entityPM.IsCancelled)
                    {
                        EventTypeRepository eventTypeRepository = new EventTypeRepository(entityPM.Tenant);
                        string eventTypeId = eventTypeRepository.GetSingleEventTypeIdByCode("CLQT", entityPM.Tenant);
                        if (!string.IsNullOrEmpty(eventTypeId))
                        {
                            TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventTypeId, entityPM.Tenant);
                            if (traceEvent != null)
                            {
                                quoteStatus.IsQuoteCancel = true;
                                quoteStatus.QuoteCancelDate = traceEvent.EventDateTime;
                                quoteStatus.QuoteCancelNote = traceEvent.Notes;
                            }
                        }
                    }
                    #endregion

                    #region StatusChange
                    if (entityPoco.StageId != entityPM.StageId)
                    {
                        QuoteStageRepository quoteStageRepository = new QuoteStageRepository(entityPM.Tenant);
                        QuoteStage stage = quoteStageRepository.GetSingleQuoteStage(entityPM.StageId, entityPM.Tenant);
                        if (stage != null)
                        {
                            if (stage.Code == "QTAC" || stage.Code == "QTST" || stage.Code == "QTDC")
                            {
                                IsQuoteStageChange = true;
                                quoteStatus.Stage.Code = stage.Code;
                                quoteStatus.Stage.Id = stage.Id;
                                quoteStatus.Stage.Name = stage.Name;
                                quoteStatus.Stage.StageDate = entityPM.LastStageDate;
                                if (stage.Code == "QTAC")
                                {
                                    quoteStatus.QuoteAcceptNote = entityPM.EventNote;
                                }
                                    
                                if (stage.Code == "QTDC")
                                {
                                    QuoteClosingReasonRepository closingReasonRepository = new QuoteClosingReasonRepository(tenant);
                                    QuoteClosingReason myQuoteClosingReason = closingReasonRepository.GetSingleQuoteClosingReason(entityPM.QuoteClosingReasonId, tenant);
                                    if (myQuoteClosingReason != null)
                                    {
                                        quoteStatus.QuoteDeclineReason.Code = myQuoteClosingReason.Code;
                                        quoteStatus.QuoteDeclineReason.Name = myQuoteClosingReason.Name;
                                        quoteStatus.QuoteDeclineReason.Note = entityPM.EventNote;
                                    }
                                }
                            }
                        }

                    }
                    #endregion

                    #region Send Quote Status Message 
                    if (entityPM.IsCancelled || IsQuoteStageChange)
                    {
                        string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(quoteStatus);

                        CommunicationsParams tasklogParams = new CommunicationsParams()
                        {
                            Tenant = entityPM.Tenant,
                            CommunicationLogTypeCode = "Q",
                            QueueName = "externaltasksqueue" + entityPM.Tenant + 1,
                            Priority = 1,
                            InOut = "O",
                            Status = "W",
                            LoggingUserId = entityPM.UpdatedByUserId,
                            LoggingObjectTableId = objectTableId,
                            LoggingEntityId = entityPM.Id,
                            Subject = "Quote Status Change",
                            FolderName = "ExternalTasksQueue",
                        };

                        List<QueueTask> queueTasks = new List<QueueTask>();
                        queueTasks.Add(
                            new QueueTask()
                            {
                                Action = "QuoteStatusMessageSentToUnifreight",
                                Parameters = new List<Parameter>()
                                {
                                    new Parameter{ Name = "QuoteStatusMetaData", Order = 1,Value =  xmlstring},
                                }
                            });

                        tasklogParams.ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks);
                        Communications.AddCommunicationLog(tasklogParams);
                    }
                    #endregion
                }
            }
        }


        private void SendQuoteToIntegratedSystem(string objectTableId)
        {
            if (CanSendQuoteToIntegratedSystem())
            {
                Logitude.BL.QuoteModel.APIDataContract.ApiV1.QuoteQueryService quoteQueryService = new Logitude.BL.QuoteModel.APIDataContract.ApiV1.QuoteQueryService(entityPM.Tenant);
                Logitude.BL.QuoteModel.APIDataContract.ApiV1.Quote quote = quoteQueryService.GetQuoteById(entityPM.Id, entityPM.Tenant);
                string xmlstring = "";
                if (quote != null)
                {
                    xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(quote);
                }
                CommunicationsParams tasklogParams = GetQuotationDocumentCommunicationsParams(objectTableId);
                List<QueueTask> queue2Tasks = new List<QueueTask>();
                queue2Tasks.Add(new QueueTask()
                {
                    Action = "ExportQuotationsToIntegratedSystem",
                    Parameters = new List<Parameter>() {
                        new Parameter {
                            Name = "QuoteMetaData",
                            Order = 1,
                            Value = xmlstring },
                    }
                });

                tasklogParams.ByteData = LogitudeXmlSerializer.SerializeObject(queue2Tasks);
                Communications.AddCommunicationLog(tasklogParams);
            }
        }


        private bool CanSendQuoteToIntegratedSystem()
        {
            bool canSendQuote = false;

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            if (tenantPM.ExportQuotationsToIntegratedSystem)
            {
                QuoteStageRepository myQuoteStageRepository = new QuoteStageRepository(tenant);
                QuoteStage quoteStageSend = myQuoteStageRepository.GetQuoteStages(tenant).Where(d => d.Code == "QTST").FirstOrDefault();
                QuoteStage quoteStageAccepted = myQuoteStageRepository.GetQuoteStages(tenant).Where(d => d.Code == "QTAC").FirstOrDefault();

                bool quoteStatusChangedToSent = (quoteStageSend != null && quoteStageSend.Id == entityPM.StageId && this.entityPoco.StageId != quoteStageSend.Id);
                bool quoteStatusChangedAccept = (quoteStageAccepted != null && quoteStageAccepted.Id == entityPM.StageId && this.entityPoco.StageId != quoteStageAccepted.Id);

                if ((tenantPM?.TransferQuotationsToUnifreightTrigger == "OnSend" && quoteStatusChangedToSent)
                    || (tenantPM?.TransferQuotationsToUnifreightTrigger == "OnAccept" && quoteStatusChangedAccept))
                {
                    canSendQuote = true;
                }
            }

            return canSendQuote;
        }
        private CommunicationsParams GetQuotationDocumentCommunicationsParams(string objectTableId)
        {
            return new CommunicationsParams()
            {
                Tenant = entityPM.Tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = "externaltasksqueue" + entityPM.Tenant + 1,
                Priority = 1,
                InOut = "O",
                Status = "W",
                LoggingUserId = entityPM.UpdatedByUserId,
                LoggingObjectTableId = objectTableId,
                LoggingEntityId = entityPM.Id,
                Subject = "Quotation Document",
                FolderName = "ExternalTasksQueue",
            };
        }

        public void GetForeignFields(QuotePM quotepm, Quote quote)
        {
            quotepm.LastModified = quote.LastModified;

            string myIncotermCode = null;
            string myIncotermName = null;
            if (!string.IsNullOrEmpty(quote.IncotermId))
            {
                IncotermRepository incotermRepository = new IncotermRepository(this.myCommonContext);
                Incoterm incoterm = incotermRepository.GetSingleIncoterm(quote.IncotermId, quote.Tenant);
                if (incoterm != null)
                {
                    myIncotermCode = incoterm.Code;
                    myIncotermName = incoterm.Name;
                }
            }

            quotepm.IncotermCode = myIncotermCode;
            quotepm.IncotermName = myIncotermName;
        }

        private void InitializeComponent()
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            this.entityPM.MarkFollowUpsAsDone = false;

            this.isAdhoc = entityPM.QuoteTypeCode == "A" ? true : false;
            this.isInlandDomestic = (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I");

            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) && entityPM.TransportModeId == "A")
            {
                entityPM.ShipmentTypeId = "Air";
            }

            this.isLCLQuote = false;
            if (entityPM.TransportModeId.ToUpper() == "A")
            {
                this.isLCLQuote = true;
            }

            else if (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
            {
                this.isLCLQuote = true;
            }

            else if (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
            {
                this.isLCLQuote = true;
            }

            this.isFCLQuote = !this.isLCLQuote;

            this.InitializeVATs();

            if (isNewEntity)
            {
                entityPM.CreatedByUserId = initializer.LoggedContactId;
                entityPM.OpenDate = entityPM.IsHybrid ? entityPM.OpenDate : todayDateTime;
                entityPM.LastStageDate = todayDateTime;

                if (!entityPM.IsHybrid)
                {
                    entityPM.QuoteNumber = TableCounter.GetNumber(tenant, "QUOT", entityPM.DirectionId, entityPM.TransportModeId);
                }

                if (entityPM.IsCreatedFromTicket)
                {
                    entityPM.RequestDate = entityPM.TicketCreateDate;
                }
                else
                {
                    entityPM.RequestDate = entityPM.OpenDate;
                }

                this.InitializeStage();
                this.InitializeSaleCurrency();
                this.InitializeProfitCurrency();

                if (!entityPM.IsHybrid)
                {
                    if (this.entityPM.QuoteCharges.Count == 0)
                    {
                        this.GenerateDefaultCharges();
                    }

                    else
                    {
                        // Ayman
                        // if Quote is Copy from another
                        // and Quantites details are edited by user
                        // then we need to calculate in server
                        this.ComputeChargesAmounts();
                    }
                }
            }

            entityPM.UpdatedByUserId = initializer.LoggedContactId;
            entityPM.UpdateDate = todayDateTime;

            if (entityPM.Ratio == null)
            {
                entityPM.Ratio = (entityPM.TransportModeId == "A") ? 6 : 1;
            }

            if (string.IsNullOrEmpty(entityPM.RatingCode))
            {
                entityPM.RatingCode = "N";
            }

            InitializePartners();
            InitializeInlandDomestic();
            InitializePickupDelivery();
            SetCustomerDateFields(entityPM, entityPoco);
            ComputeChargesSaleFieldsInSaleCurrency();
            ComputeCountryForStatisticsId();

            if (!entityPM.IsHybrid)
            {
                InitializeSubject();
            }

            InitializeExpirationValues();
            InitializeAutomaticallyClose();
            InitializeQuoteConversionProcess();

            this.ComputeExpectedProfit();
            this.ComputeProfit();
            this.FillDefaultSubType();
        }

        private void InitializeQuoteConversionProcess()
        {
            if (entityPM.ConvertToLCL || entityPM.ConvertToFCL)
            {
                this.DeletePackagesAndCharges();                

                if (entityPM.ConvertToFCL)
                {
                    this.isLCLQuote = false;
                    entityPM.ShipmentTypeId = "FCLD";
                }
                else
                {
                    this.isLCLQuote = true;
                    entityPM.ShipmentTypeId = "LCLD";
                }
                this.GenerateDefaultCharges();
                this.RecalculateRatioAfterConversion();
            }

            if (entityPM.ConvertTransportMode)
            {
                this.DeletePackagesAndCharges();
                this.RecalculateRatioAfterConversion();
                this.GenerateDefaultCharges();
            }
        }

        private void DeletePackagesAndCharges()
        {
            entityPM.NumberOfPackages = null;
            entityPM.PackageType1Id = null;
            entityPM.PackageType1Quantity = null;
            entityPM.PackageType2Id = null;
            entityPM.PackageType2Quantity = null;
            entityPM.PackageType3Id = null;
            entityPM.PackageType3Quantity = null;
            entityPM.PackageType4Id = null;
            entityPM.PackageType4Quantity = null;
            entityPM.PackageType5Id = null;
            entityPM.PackageType5Quantity = null;
            entityPM.TEU = null;
            entityPM.NumberOfContainers = null;
            entityPM.GrossWeight = null;
            entityPM.ChargeableWeight = null;
            entityPM.VolumetricWeight = null;
            entityPM.Volume = null;
            entityPM.EstimateProfit = null;
            entityPM.EstimatedProfitInLocal = null;
            entityPM.EstimatedProfitInProfit = null;
            entityPM.EstimateProfitInSaleCurrency = null;

            foreach (QuotePackagePM pm in entityPM.QuotePackages)
            {
                this.DeleteQuotePackage(pm);
            }

            foreach (QuoteChargePM pm in entityPM.QuoteCharges)
            {
                this.DeleteQuoteChargeUp(pm);
            }

            entityPM.QuotePackages.Clear();
            entityPM.QuoteCharges.Clear();
        }
        private void RecalculateRatioAfterConversion()
        {
            entityPM.Ratio = MethodHelper.ComputeRatio(entityPM.DirectionId, entityPM.TransportModeId, entityPM.ShipmentTypeId, loggedTenant);
        }
        private void FillDefaultSubType()
        {
            if (string.IsNullOrEmpty(entityPM.ShipmentSubTypeId) || entityPM.ConvertToFCL || entityPM.ConvertToLCL || entityPM.ConvertTransportMode)
            {
                string code = null;
                if (entityPM.TransportModeId == "A")
                {
                    code = "Air";
                }

                else if (entityPM.TransportModeId == "I")
                {
                    if (entityPM.ShipmentTypeId == "FTL")
                    {
                        code = "FTL";
                    }

                    else
                    {
                        code = "LTL";
                    }
                }

                else if (entityPM.TransportModeId == "O")
                {
                    if (entityPM.ShipmentTypeId == "FCLD")
                    {
                        code = "FCL";
                    }

                    else
                    {
                        code = "LCL";
                    }
                }

                if (!string.IsNullOrEmpty(code))
                {
                    ShipmentSubTypeRepository subTypeRepository = new ShipmentSubTypeRepository(entityPM.Tenant);
                    ShipmentSubType subType = subTypeRepository.GetSingleShipmentSubTypeByCode(code, entityPM.Tenant);
                    if (subType != null)
                    {
                        entityPM.ShipmentSubTypeId = subType.Id;
                    }
                }
            }
        }

        private bool isEnableMultiPercentageVATTypes;
        private List<VatType> allVatTypes = new List<VatType>();
        private List<VatTypePercentagePM> allVatPercentages = new List<VatTypePercentagePM>();
        private void InitializeVATs()
        {
            this.allVatTypes = (from d in myCommonContext.VatTypes
                                where d.Tenant == this.tenant
                                select d).ToList();

            AccountingSetting accountingSetting = (from a in myCommonContext.AccountingSettings
                                                   where a.Id == this.tenant
                                                   select a).FirstOrDefault();

            if (accountingSetting != null)
            {
                this.isEnableMultiPercentageVATTypes = accountingSetting.EnableMultiPercentageVATTypes;
            }

            if (entityPM.QuoteTypeCode == "A")
            {
                VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                this.allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
            }
        }

        private void InitializeStage()
        {
            if (isNewEntity)
            {
                QuoteStage myStage = null;
                string myStageId = null;
                DateTime? stageDueDate = null;
                QuoteStageRepository stageRepository = new QuoteStageRepository(entityPM.Tenant);

                if (!string.IsNullOrEmpty(entityPM.StageId))
                {
                    myStage = stageRepository.GetSingleQuoteStage(entityPM.StageId, entityPM.Tenant);
                }

                else
                {
                    myStage = stageRepository.GetSingleQuoteStageByCode("QTCR", entityPM.Tenant);
                }

                if (myStage != null)
                {
                    myStageId = myStage.Id;

                    if (myStage.MaxDays != null)
                    {
                        DateTime? todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                        stageDueDate = todayDateTime.Value.Date.AddDays(Convert.ToDouble(myStage.MaxDays));
                    }
                }

                entityPM.StageId = myStageId;
                entityPM.StageDueDate = stageDueDate;
            }
        }
        private void InitializePartners()
        {
            if (string.IsNullOrEmpty(entityPM.ShipperId))
            {
                entityPM.ShipperName = null;
                entityPM.ShipperNote = null;
                entityPM.ShipperContactId = null;
                entityPM.ShipperReference1 = null;
                entityPM.ShipperReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                entityPM.ConsigneeName = null;
                entityPM.ConsigneeNote = null;
                entityPM.ConsigneeContactId = null;
                entityPM.ConsigneeReference1 = null;
                entityPM.ConsigneeReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.AgentId))
            {
                entityPM.AgentName = null;
                entityPM.AgentContactId = null;
                entityPM.AgentAddressId = null;
                entityPM.AgentReference1 = null;
                entityPM.AgentReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.NotifyId))
            {
                entityPM.NotifyName = null;
                entityPM.NotifyContactId = null;
                entityPM.NotifyAddressId = null;
                entityPM.NotifyNote = null;
                entityPM.NotifyReference1 = null;
                entityPM.NotifyReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.CustomerId))
            {
                entityPM.CustomerName = null;
                entityPM.CustomerContactId = null;
                entityPM.CustomerReference1 = null;
                entityPM.CustomerReference2 = null;
                entityPM.CustomerNote = null;
            }

            if (string.IsNullOrEmpty(entityPM.QuoteCustomerTypeCode))
            {
                if (entityPM.DirectionId == "I")
                {
                    entityPM.QuoteCustomerTypeCode = "CON";
                }

                else
                {
                    entityPM.QuoteCustomerTypeCode = "SHI";
                }
            }

            else
            {
                CardRepository cardRepository = new CardRepository(tenant);
                Card customer = cardRepository.GetSingleCard(entityPM.CustomerId, tenant);
                if (customer != null)
                {
                    entityPM.CustomerName = customer.EnglishName;
                }
            }
        }

        private void InitializeSaleCurrency()
        {
            if (isNewEntity)
            {
                if (string.IsNullOrEmpty(entityPM.SaleCurrencyId))
                {
                    entityPM.SaleCurrencyId = initializer.LoggedTenant.QuoteSaleCurrencyId;
                }

                if (!this.entityPM.IsCopyExchangeRates)
                {
                    if (string.IsNullOrEmpty(entityPM.SaleCurrencyId))
                    {
                        entityPM.ExchangeRate = null;
                    }

                    else if (entityPM.SaleCurrencyId == initializer.LoggedTenant.CurrencyId)
                    {
                        entityPM.ExchangeRate = 1;
                    }

                    else
                    {
                        RatesTableQuery myQuery = new RatesTableQuery(tenant);
                        LastRate lastRate = myQuery.GetLastRecordByValueDate(tenant, entityPM.SaleCurrencyId, initializer.LoggedTenant.CurrencyId, entityPM.OpenDate);
                        if (lastRate != null)
                        {
                            entityPM.ExchangeRate = MethodHelper.Round(lastRate.Rate, 5);
                        }
                    }
                }
            }
        }

        private void InitializeProfitCurrency()
        {
            if (isNewEntity)
            {
                if (string.IsNullOrEmpty(entityPM.ProfitCurrencyId))
                {
                    entityPM.ProfitCurrencyId = initializer.LoggedTenant.ProfitCurrencyId;
                }

                if (!this.entityPM.IsCopyExchangeRates)
                {
                    if (string.IsNullOrEmpty(entityPM.ProfitCurrencyId))
                    {
                        entityPM.ProfitExchangeRate = null;
                    }

                    else if (entityPM.ProfitCurrencyId == initializer.LoggedTenant.CurrencyId)
                    {
                        entityPM.ProfitExchangeRate = 1;
                    }

                    else
                    {
                        RatesTableQuery myQuery = new RatesTableQuery(tenant);
                        LastRate lastRate = myQuery.GetLastRecordByValueDate(tenant, entityPM.ProfitCurrencyId, initializer.LoggedTenant.CurrencyId, entityPM.OpenDate);
                        if (lastRate != null)
                        {
                            entityPM.ProfitExchangeRate = MethodHelper.Round(lastRate.Rate, 5);
                        }
                    }
                }
            }
        }

        private void InitializeInlandDomestic()
        {
            if (isInlandDomestic)
            {
                entityPM.IncludePickUp = false;
                entityPM.IncludeDelivery = false;

                //if (!string.IsNullOrEmpty(entityPM.ShipperId))
                //{
                //    if (string.IsNullOrEmpty(entityPM.FromPartnerId))
                //    {
                //        entityPM.FromPartnerId = entityPM.ShipperId;
                //    }

                //    if (string.IsNullOrEmpty(entityPM.FromPartnerAddressId))
                //    {
                //        Address adr = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, tenant);
                //        if (adr != null)
                //        {
                //            entityPM.FromPartnerAddressId = adr.Id;
                //        }
                //    }
                //}

                //if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
                //{
                //    if (string.IsNullOrEmpty(entityPM.ToPartnerId))
                //    {
                //        entityPM.ToPartnerId = entityPM.ConsigneeId;
                //    }

                //    if (string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
                //    {
                //        Address adr = addressRepository.GetMainAddressByCardId(entityPM.ConsigneeId, tenant);
                //        if (adr != null)
                //        {
                //            entityPM.ToPartnerAddressId = adr.Id;
                //        }
                //    }
                //}
            }
        }
        private void InitializePickupDelivery()
        {
            if (!isInlandDomestic)
            {
                if (entityPM.IncludePickUp)
                {
                    if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
                    {
                        entityPM.FromAddressCity = null;
                        entityPM.FromAddressZipCode = null;
                        entityPM.FromAddressCountryId = null;
                    }
                }

                else
                {
                    entityPM.FromAddressCity = null;
                    entityPM.FromAddressZipCode = null;
                    entityPM.FromAddressCountryId = null;
                    entityPM.PickUpAddressId = null;
                    entityPM.PickupLocation = null;
                }


                if (entityPM.IncludeDelivery)
                {
                    if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                    {
                        entityPM.ToAddressCity = null;
                        entityPM.ToAddressZipCode = null;
                        entityPM.ToAddressCountryId = null;
                    }
                }

                else
                {
                    entityPM.ToAddressCity = null;
                    entityPM.ToAddressZipCode = null;
                    entityPM.ToAddressCountryId = null;
                    entityPM.DeliveryAddressId = null;
                    entityPM.DeliveryLocation = null;
                }
            }
        }
        private void InitializeSubject()
        {
            bool isAutomaticUpdate = true;

            if (initializer.LoggedTenant.IsQuoteSubjectEdited && entityPM.IsSubjectEdited)
            {
                isAutomaticUpdate = false;
            }

            if (isAutomaticUpdate)
            {
                QuoteSubjectService iSubjectService = new QuoteSubjectService(entityPM);
                entityPM.Subject = iSubjectService.GetSubject();
            }
        }
        private void InitializeExpirationValues()
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

            if (entityPM.ExpirationDate != null && entityPM.ExpirationDays == null)
            {
                entityPM.ExpirationDays = (Convert.ToDateTime(entityPM.ExpirationDate) - todayDateTime.Date).Days;
            }

            if (entityPM.ExpirationDays != null && entityPM.ExpirationDate == null)
            {
                entityPM.ExpirationDate = todayDateTime.AddDays(Convert.ToDouble(entityPM.ExpirationDays));
            }
        }
        private void InitializeAutomaticallyClose()
        {
            if (entityPM.IsAutomaticallyClosed)
            {
                if (isNewEntity)
                {
                    if (entityPM.AutomaticallyCloseDate != null)
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        if (entityPM.AutomaticallyCloseDate.Value.Date <= todayDateTime)
                        {
                            this.CloseEntityAutomatically();
                        }
                    }
                }

                else if (!entityPoco.IsAutomaticallyClosed)
                {
                    if (entityPM.AutomaticallyCloseDate != null)
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        if (entityPM.AutomaticallyCloseDate.Value.Date <= todayDateTime)
                        {
                            this.CloseEntityAutomatically();
                        }
                    }
                }
            }
        }
        private void CloseEntityAutomatically()
        {
            var quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            var quoteClosing = quoteClosingReasonRepository.GetSingleQuoteClosingReasonByCode("XQ", tenant);
            entityPM.QuoteClosingReasonId = quoteClosing.Id;
            entityPM.IsClosed = true;
            entityPM.QuoteClosingReasonCode = "XQ";
            entityPM.ActionType = "Decline";
        }

        private void UpdateQuoteChargesCollection()
        {
            if (quoteChargesChangeSet != null)
            {
                foreach (QuoteChargePM itemPM in quoteChargesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateQuoteChargeUp(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateQuoteChargeUp(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteQuoteChargeUp(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateQuoteFollowUpsCollection()
        {
            if (quoteFollowUpsChangeSet != null)
            {
                List<FollowUp> doneFollowUps = new List<FollowUp>();

                foreach (QuoteFollowUpPM itemPM in quoteFollowUpsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateQuoteFollowUp(itemPM);
                                followUpRepository.SubmitChanges();

                                EventTracer.CreateTraceEvent(new EventTracerArgs()
                                {
                                    Tenant = tenant,
                                    EventTypeCode = "QFCR",
                                    UserId = initializer.LoggedContactId,
                                    EntityId = this.entityPM.Id,
                                    ObjectTableName = "Quote",
                                    Notes = itemPM.EventTypeFollowUpName,
                                });

                                if (itemPM.Done)
                                {
                                    itemPM.Deleted = true;
                                    FollowUp follow = followUpRepository.GetSingleFollowUp(itemPM.Id, tenant);
                                    doneFollowUps.Add(follow);

                                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                                    {
                                        Tenant = tenant,
                                        EventTypeCode = "QFCM",
                                        UserId = initializer.LoggedContactId,
                                        EntityId = this.entityPM.Id,
                                        ObjectTableName = "Quote",
                                        Notes = itemPM.EventTypeFollowUpName,
                                    });
                                }

                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteQuoteFollowUp(itemPM);
                                followUpRepository.SubmitChanges();
                                break;
                            }

                        default: { break; }
                    }
                }

                if (doneFollowUps.Count > 0)
                {
                    foreach (FollowUp itemPoco in doneFollowUps)
                    {
                        followUpRepository.Remove(itemPoco);
                        followUpRepository.SubmitChanges();

                        QuoteTracing.TraceQuoteOnCreateDoneFollowUp(entityPM, entityPoco, itemPoco, initializer.LoggedContactId);
                    }
                }

                foreach (QuoteFollowUpPM itemPM in quoteFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Update))
                {
                    if (itemPM.Done)
                    {
                        itemPM.Deleted = true;
                        entityPM.MarkFollowUpsAsDone = true;

                        QuoteTracing.TraceQuoteOnUpdateDoneFollowUp(entityPM, entityPoco, itemPM, initializer.LoggedContactId);

                        this.DeleteQuoteFollowUp(itemPM);
                        followUpRepository.SubmitChanges();

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "QFCM",
                            UserId = initializer.LoggedContactId,
                            EntityId = this.entityPM.Id,
                            ObjectTableName = "Quote",
                            Notes = itemPM.EventTypeFollowUpName,
                        });
                    }

                    else
                    {
                        this.UpdateQuoteFollowUp(itemPM);
                    }
                }

                this.ComputeNumerOfFollowUps();
            }
        }
        private void UpdateQuotePackageCollection()
        {
            if (quotePackageChangeSet != null)
            {
                foreach (QuotePackagePM itemPM in quotePackageChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateQuotePackage(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateQuotePackage(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteQuotePackage(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        public void UpdateQuoteDocuments(QuotePM quotePM) 
        {
            this.entityPM = quotePM;
            this.tenant = quotePM.Tenant;
            this.quoteDocumentVersionChangeSet = quotePM.QuoteDocumentVersions;
            this.UpdateQuoteDocumentVersionCollection();
        }
        private void UpdateQuoteDocumentVersionCollection()
        {
            if (quoteDocumentVersionChangeSet != null)
            {
                foreach (QuoteDocumentVersionPM itemPM in quoteDocumentVersionChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateQuoteDocumentVersion(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateQuoteDocumentVersion(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                //this.DeleteQuotePackage(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateQuoteChargeUp(QuoteChargePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("QuoteCharge", tenant).ToString();
            itemPM.QuoteId = entityPM.Id;
            itemPM.Tenant = tenant;

            QuoteCharge itemPoco = new QuoteCharge()
            {
                Id = itemPM.Id,
            };

            if (!string.IsNullOrEmpty(itemPM.TariffId))
            {
                this.UpdateTariffUsedDate(itemPM.TariffId);
            }

            this.ComputeQuoteChargesVATAmounts(itemPM);
            QuoteMapping.MapQuoteCharge(itemPM, itemPoco, true, this.entityPM);
            quoteChargeRepository.Add(itemPoco);

            if (itemPM.IsChargeBySteps)
            {
                if (itemPM.QuoteChargePriceSteps != null)
                {
                    foreach (QuotePriceStepsPM insideItemPM in itemPM.QuoteChargePriceSteps)
                    {
                        insideItemPM.QuoteId = entityPM.Id;
                        insideItemPM.QuoteChargeId = itemPM.Id;
                        this.CreateQuotePriceSteps(insideItemPM);
                    }
                }
            }
        }
        private void UpdateQuoteChargeUp(QuoteChargePM itemPM)
        {
            QuoteCharge itemPoco = quoteChargeRepository.GetSingleQuoteReceivable(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                if (!string.IsNullOrEmpty(itemPM.TariffId) && string.IsNullOrEmpty(itemPoco.TariffId))
                {
                    this.UpdateTariffUsedDate(itemPM.TariffId);
                }

                this.ComputeQuoteChargesVATAmounts(itemPM);
                QuoteMapping.MapQuoteCharge(itemPM, itemPoco, false, this.entityPM);
                quoteChargeRepository.Update(itemPoco);

                if (itemPM.QuoteChargePriceStepsChangeSet != null)
                {
                    if (!itemPM.IsChargeBySteps)
                    {
                        foreach (QuotePriceStepsPM insideItemPM in itemPM.QuoteChargePriceStepsChangeSet)
                        {
                            this.DeleteQuotePriceSteps(insideItemPM);
                        }
                    }

                    else
                    {
                        foreach (QuotePriceStepsPM insideItemPM in itemPM.QuoteChargePriceStepsChangeSet)
                        {
                            switch (insideItemPM.ChangeSetOp)
                            {
                                case ChangeSetOperation.Insert:
                                    {
                                        insideItemPM.QuoteId = entityPM.Id;
                                        insideItemPM.QuoteChargeId = itemPM.Id;
                                        this.CreateQuotePriceSteps(insideItemPM);
                                        break;
                                    }

                                case ChangeSetOperation.Update:
                                    {
                                        this.UpdateQuotePriceSteps(insideItemPM);
                                        break;
                                    }

                                case ChangeSetOperation.Delete:
                                    {
                                        this.DeleteQuotePriceSteps(insideItemPM);
                                        break;
                                    }

                                default: { break; }
                            }
                        }
                    }
                }
            }
        }
        private void DeleteQuoteChargeUp(QuoteChargePM itemPM)
        {
            QuoteCharge itemPoco = quoteChargeRepository.GetSingleQuoteReceivable(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                List<QuotePriceSteps> list = quotePriceStepsRepository.GetQuotePriceStepPMsByQuoteCharge(itemPoco.QuoteId, itemPoco.Id, itemPoco.Tenant).ToList();
                if (list != null)
                {
                    foreach (QuotePriceSteps insideItemPoco in list)
                    {
                        quotePriceStepsRepository.Remove(insideItemPoco);
                    }
                }

                quoteChargeRepository.Remove(itemPoco);
            }
        }

        private void CreateQuotePriceSteps(QuotePriceStepsPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("QuotePriceSteps", tenant).ToString();
            itemPM.Tenant = tenant;

            QuotePriceSteps itemPoco = new QuotePriceSteps()
            {
                Id = itemPM.Id,
                Tenant = tenant,
                QuoteId = itemPM.QuoteId,
                QuoteChargeId = itemPM.QuoteChargeId,
            };

            QuoteMapping.MapQuotePrice(itemPM, itemPoco, true);
            quotePriceStepsRepository.Add(itemPoco);
        }
        private void UpdateQuotePriceSteps(QuotePriceStepsPM itemPM)
        {
            QuotePriceSteps itemPoco = quotePriceStepsRepository.GetSingleQuotePriceStep(itemPM.Id);
            QuoteMapping.MapQuotePrice(itemPM, itemPoco, false);
            quotePriceStepsRepository.Update(itemPoco);
        }
        private void DeleteQuotePriceSteps(QuotePriceStepsPM itemPM)
        {
            QuotePriceSteps itemPoco = quotePriceStepsRepository.GetSingleQuotePriceStep(itemPM.Id);

            if (itemPoco != null)
            {
                quotePriceStepsRepository.Remove(itemPoco);
            }
        }

        private void CreateQuoteFollowUp(QuoteFollowUpPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("FollowUp", tenant).ToString();
            itemPM.QuoteId = entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.IsNew = false;

            if (itemPM.LegType != null)
            {
                if (itemPM.LegType.Contains("PickUp") || itemPM.LegType.Contains("Delivery"))
                {
                    itemPM.LegType = itemPM.LegType + entityPM.QuoteNumber + "/" + "1";
                }
            }

            FollowUp itemPoco = new FollowUp()
            {
                Id = itemPM.Id,
            };

            QuoteMapping.MapFollowUp(itemPM, itemPoco, true);
            followUpRepository.Add(itemPoco);
        }
        private void UpdateQuoteFollowUp(QuoteFollowUpPM itemPM)
        {
            FollowUp itemPoco = followUpRepository.GetSingleFollowUp(itemPM.Id, tenant);
            QuoteMapping.MapFollowUp(itemPM, itemPoco, false);
            followUpRepository.Update(itemPoco);
        }
        private void DeleteQuoteFollowUp(QuoteFollowUpPM itemPM)
        {
            FollowUp itemPoco = followUpRepository.GetSingleFollowUp(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                followUpRepository.Remove(itemPoco);
            }
        }

        private void CreateQuotePackage(QuotePackagePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("QuotePackage", tenant).ToString();
            itemPM.QuoteId = entityPM.Id;
            itemPM.Tenant = tenant;

            QuotePackage itemPoco = new QuotePackage()
            {
                Id = itemPM.Id,
            };

            QuoteMapping.MapQuotePackage(itemPM, itemPoco, true);
            quotePackageRepository.Add(itemPoco);
        }
        private void UpdateQuotePackage(QuotePackagePM itemPM)
        {
            QuotePackage itemPoco = quotePackageRepository.GetSingleQuotePackage(itemPM.Id, tenant);
            QuoteMapping.MapQuotePackage(itemPM, itemPoco, false);
            quotePackageRepository.Update(itemPoco);
        }
        private void DeleteQuotePackage(QuotePackagePM itemPM)
        {
            if (itemPM.Id != null)
            {
                QuotePackage itemPoco = quotePackageRepository.GetSingleQuotePackage(itemPM.Id, tenant);
                if (itemPoco != null)
                {
                    quotePackageRepository.Remove(itemPoco);
                }
            }
        }

        public void CreateQuoteDocumentVersion(QuoteDocumentVersionPM itemPM)
        {
            QuoteDocumentVersion itemPoco = new QuoteDocumentVersion();

            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);
            QuoteDocumentVersion lastVersion = quoteDocumentVersionRepository.GetQuoteDocumentVersionsByQuoteId(itemPM.QuoteId, itemPM.Tenant).OrderBy(s => s.VersionNumber).ToList().LastOrDefault();
            if (lastVersion != null)
            {
                itemPM.VersionNumber = lastVersion.VersionNumber + 1;
            }
            else
            {
                itemPM.VersionNumber = 1;
            }


            itemPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPM.UpdateDate = itemPM.CreateDate;

            QuoteTemplateEntityService quoteTemplateService = new QuoteTemplateEntityService();
            byte[] pdfData = new byte[] { };
            if (itemPM.VersionType == "G")
            {
                pdfData = quoteTemplateService.GetQuoteTemplatePdfReport(itemPM.QuoteId, itemPM.QuoteTemplateId, itemPM.CreatedByUserId, itemPM.Tenant, null, null, itemPM.VersionNumber);
            }

            DocumentRepository documentRep = new DocumentRepository(myCommonContext);
            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "pdf",
                FileSize = Convert.ToInt32(pdfData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", itemPM.Tenant).ToString(),
                Folder = "quotetemplatesectionfiles",
                CalculatedFileName = "Quotation-" + entityPM.QuoteNumber + "-" + itemPM.VersionNumber,
                HasFile = true,
            };
            documentRep.Add(document);
            documentRep.SubmitChanges();

            itemPM.DocumentId = document.Id;


            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(pdfData, fileInfo);



            this.entityPM.LastVersionNumber = itemPM.VersionNumber;
            this.entityPM.QuoteTemplateId = itemPM.QuoteTemplateId;

            QuoteDocumentVersionMapping.MappingQuoteDocumentVersion(itemPM, itemPoco, true);
            quoteDocumentVersionRepository.Add(itemPoco);
            quoteDocumentVersionRepository.SubmitChanges();

            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(myCommonContext);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(myCommonContext);
            DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(myCommonContext);
            DocumentOutCopyRepository documentOutCopyRepository = new DocumentOutCopyRepository(myCommonContext);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(myCommonContext);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode("QUOTE", tenant);
            if (documentType != null)
            {
                DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(itemPM.QuoteId, documentType.Id, tenant);
                if (documentout == null)
                {
                    ObjectTable table = tableRepository.GetObjectTableByName("Quote", 0, true);

                    DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentType.Id, EntityId = this.entityPM.Id, Tenant = tenant, ObjectTableId = table.Id, ChildEntityId = null, ChildEntityReference = null, DirectionCode = "O" };
                    newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
                    newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                    newDocumentFiling.CreatedByUserId = initializer.LoggedContactId;
                    newDocumentFiling.OwnerId = initializer.LoggedContactId;
                    newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.UpdatedByUserId = initializer.LoggedContactId;
                    newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.HasCopies = true;
                    newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
                    newDocumentFiling.DocumentId = document.Id;
                    newDocumentFiling.SecurityId = newDocumentFiling.Id + StringHelper.GetRandomString(10);
                    documentsFilingRepository.Add(newDocumentFiling);


                    documentout = new DocumentOut()
                    {
                        Id = newDocumentFiling.Id,
                        EmailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId,
                        DocumentTemplateId = documentType.DocumentTypeDefaultReportTemplateId,
                        Tenant = tenant,
                        Issued = true,
                        IsBlobExist = true,
                    };

                    documentOutRepository.Add(documentout);

                    DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, tenant);
                    if (documentTypeCopy == null)
                    {
                        documentTypeCopy = new DocumentTypeCopy()
                        {
                            Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                            DocumentTypeId = documentType.Id,
                            Code = documentType.Code,
                            Name = documentType.Name,
                            Tenant = tenant,
                        };

                        documentTypeCopyRepository.Add(documentTypeCopy);
                    }

                    DocumentOutCopy docoutCopy = new DocumentOutCopy()
                    {
                        Id = document.Id,
                        DocumentId = document.Id,
                        DocumentOutId = documentout.Id,
                        DocumentTypeCopyId = documentTypeCopy.Id,
                        Tenant = tenant,
                    };

                    documentOutCopyRepository.Add(docoutCopy);
                }
                else
                {
                    DocumentOutCopy docoutCopy = (from a in myCommonContext.DocumentOutCopies
                                                  where a.DocumentOutId == documentout.Id && a.Tenant == tenant
                                                  select a).FirstOrDefault();
                    if (docoutCopy != null)
                    {
                        docoutCopy.DocumentId = document.Id;
                    }
                    else
                    {
                        DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, tenant);
                        if (documentTypeCopy == null)
                        {
                            documentTypeCopy = new DocumentTypeCopy()
                            {
                                Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                                DocumentTypeId = documentType.Id,
                                Code = documentType.Code,
                                Name = documentType.Name,
                                Tenant = tenant,
                            };

                            documentTypeCopyRepository.Add(documentTypeCopy);
                        }

                        docoutCopy = new DocumentOutCopy()
                        {
                            Id = document.Id,
                            DocumentId = document.Id,
                            DocumentOutId = documentout.Id,
                            DocumentTypeCopyId = documentTypeCopy.Id,
                            Tenant = tenant,
                        };

                        documentOutCopyRepository.Add(docoutCopy);
                    }


                    DocumentsFiling documentsFiling = (from a in myCommonContext.DocumentsFilings
                                                       where a.Id == documentout.Id && a.Tenant == tenant
                                                       select a).FirstOrDefault();
                    if (documentsFiling != null)
                    {
                        if (documentsFiling.DocumentId != document.Id)
                        {
                            documentsFiling.DocumentId = document.Id;
                            documentsFilingRepository.Update(documentsFiling);
                        }
                    }
                }

                myCommonContext.SaveChanges();
            }
            else
            {
                throw new Exception("Document Type with code 'QUOTE' is not found!");
            }
        }
        public void UpdateQuoteDocumentVersion(QuoteDocumentVersionPM itemPM)
        {
            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);
            QuoteDocumentVersion itemPoco = quoteDocumentVersionRepository.GetSingleQuoteDocumentVersion(itemPM.QuoteId, itemPM.Tenant, itemPM.VersionNumber);

            string entityName = "QuoteDocumentVersion" + itemPM.QuoteId + itemPM.Tenant + itemPM.VersionNumber;
            string entityPmName = "QuoteDocumentVersionPM" + itemPM.QuoteId + itemPM.Tenant + itemPM.VersionNumber;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            itemPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            if (itemPM.IsSent && !itemPoco.IsSent)
            {
                itemPM.SendDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            }
            QuoteDocumentVersionMapping.MappingQuoteDocumentVersion(itemPM, itemPoco, isNewEntity);
            quoteDocumentVersionRepository.Update(itemPoco);
            quoteDocumentVersionRepository.SubmitChanges();
        }

        private void SetCustomerDateFields(QuotePM entityPM, Quote entityPOCO)
        {
            int myTenant = entityPM.Tenant;
            DateTime myDate = TenantServerConfigration.GetCurrentDateTime(myTenant);

            if (isNewEntity)
            {
                if (!string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    CustomerRepository customerRepository = new CustomerRepository(myTenant);
                    Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, myTenant, false);
                    if (customer != null)
                    {
                        customer.LastQuoteDate = myDate;
                        customer.LastInteractionDate = myDate;
                        customerRepository.Update(customer);
                        customerRepository.SubmitChanges();
                    }
                }
            }

            else
            {
                if (entityPM.CustomerId != entityPOCO.CustomerId)
                {
                    CustomerRepository customerRepository = new CustomerRepository(myTenant);

                    if (!string.IsNullOrEmpty(entityPM.CustomerId))
                    {
                        Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, myTenant, false);
                        if (customer != null)
                        {
                            customer.LastQuoteDate = myDate;
                            customer.LastInteractionDate = myDate;
                            customerRepository.Update(customer);
                            customerRepository.SubmitChanges();
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.CustomerId))
                    {
                        QuoteRepository myQuoteRepository = new QuoteRepository(myTenant);
                        IQueryable<Quote> oldCustomerEntities = myQuoteRepository.GetQuotesByCustomerId(entityPOCO.CustomerId, myTenant);

                        if (oldCustomerEntities != null)
                        {
                            if (oldCustomerEntities.Count() > 0)
                            {
                                DateTime? oldestDate = oldCustomerEntities.OrderByDescending(d => d.OpenDate).FirstOrDefault().OpenDate;
                                if (oldestDate != null)
                                {
                                    Customer customer = customerRepository.GetSingleCustomer(entityPOCO.CustomerId, myTenant, false);
                                    if (customer != null)
                                    {
                                        customer.LastQuoteDate = oldestDate;

                                        customer.LastInteractionDate = customerRepository.ComputeLastInteractionDate(customer, oldestDate);

                                        customerRepository.Update(customer);
                                        customerRepository.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateTotalVats()
        {
            List<QuoteTotalVAT> dbTotalVats = quoteTotalVATRepository.GetTotalVATs(entityPM.Id, entityPM.Tenant).ToList();

            foreach (QuoteTotalVAT item in dbTotalVats)
            {
                quoteTotalVATRepository.Remove(item);
            }

            if (entityPM.QuoteTypeCode == "A")
            {
                if (entityPM.IsChargesByVAT)
                {
                    List<QuoteChargePM> myDataLines = this.entityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null && d.IsAllIN == false).ToList();

                    if (myDataLines.Count > 0)
                    {
                        double? myProfitCurrencyRate = null;
                        RatesTableQuery myQuery = new RatesTableQuery(tenant);
                        LastRate lastRate = myQuery.GetLastRecordByValueDate(tenant, initializer.LoggedTenant.ProfitCurrencyId, initializer.LoggedTenant.CurrencyId, entityPM.UpdateDate);
                        if (lastRate != null)
                        {
                            myProfitCurrencyRate = MethodHelper.Round(lastRate.Rate, 5);
                        }

                        List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == this.tenant select d).ToList();

                        List<QuoteTotalsClass> group_Source = new List<QuoteTotalsClass>();

                        foreach (QuoteChargePM item in myDataLines)
                        {
                            #region
                            VatType lineVatType = this.allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                            if (lineVatType != null)
                            {
                                if (!lineVatType.IsMultiPercentage)
                                {
                                    QuoteTotalsClass newItem = new QuoteTotalsClass()
                                    {
                                        Id = item.VatTypeId,
                                        VatTypeId = item.VatTypeId,
                                        VatTypePercentage = item.VatPercentage,
                                        QuoteCurrencyAmount = item.SaleAmountInSaleCurrency,
                                        LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                        ExternalVATCard = lineVatType.ReceivablesExternalId,
                                        ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                                    };

                                    if (item.IsRegionalTax)
                                    {
                                        newItem.QuoteCurrencyAmount = item.SaleAmountInSaleCurrency + item.SaleAmountInSaleCurrency * (entityPM.RegionalTaxPercentage / 100);
                                        newItem.LocalCurrencyAmount = item.SaleTotalAmountLocal + item.SaleTotalAmountLocal * (entityPM.RegionalTaxPercentage / 100);
                                        
                                        QuoteTotalsClass newRegionalTaxItem = new QuoteTotalsClass()
                                        {
                                            Id = entityPM.RegionalTaxId,
                                            VatTypeId = entityPM.RegionalTaxId,
                                            VatTypePercentage = entityPM.RegionalTaxPercentage,
                                            QuoteCurrencyAmount = item.SaleAmountInSaleCurrency,
                                            LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                            ExternalVATCard = newItem.ExternalVATCard,
                                            ExternalTAXItemId = newItem.ExternalTAXItemId,
                                        };

                                        group_Source.Add(newRegionalTaxItem);
                                    }

                                    group_Source.Add(newItem);
                                }

                                else
                                {
                                    List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();
                                    foreach (VATTypesGroup itemGroup in myVatGroups)
                                    {
                                        QuoteTotalsClass newItem = new QuoteTotalsClass()
                                        {
                                            Id = itemGroup.SingleVATTypeId,
                                            VatTypeId = itemGroup.SingleVATTypeId,
                                            QuoteCurrencyAmount = item.SaleAmountInSaleCurrency,
                                            LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                        };

                                        VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (vatType != null)
                                        {
                                            newItem.ExternalVATCard = vatType.ReceivablesExternalId;
                                            newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                        }

                                        VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (myPercentagePM != null)
                                        {
                                            newItem.VatTypePercentage = myPercentagePM.Percentage;
                                        }

                                        group_Source.Add(newItem);
                                    }
                                }
                            }
                            #endregion
                        }

                        List<QuoteTotalsClass> group_data =
                            (from items in group_Source
                             group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVATCard, items.ExternalTAXItemId } into g
                             select new QuoteTotalsClass()
                             {
                                 Id = g.Key.VatTypeId,
                                 VatTypeId = g.Key.VatTypeId,
                                 VatTypePercentage = g.Key.VatTypePercentage,
                                 ExternalVATCard = g.Key.ExternalVATCard,
                                 ExternalTAXItemId = g.Key.ExternalTAXItemId,
                                 QuoteCurrencyAmount = g.Sum(s => s.QuoteCurrencyAmount),
                                 LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                             }).ToList();

                        foreach (var item in group_data)
                        {
                            if (entityPM.SaleCurrencyId == initializer.LoggedTenant.ProfitCurrencyId)
                            {
                                item.ProfitCurrencyAmount = item.QuoteCurrencyAmount;
                            }

                            else if (entityPM.SaleCurrencyId == initializer.LoggedTenant.CurrencyId)
                            {
                                item.ProfitCurrencyAmount = item.LocalCurrencyAmount;
                            }

                            else
                            {
                                item.ProfitCurrencyAmount = MethodHelper.Roundd(item.LocalCurrencyAmount / myProfitCurrencyRate, 2);
                            }

                            QuoteTotalVAT record = new QuoteTotalVAT()
                            {
                                Id = IdCounter.GetNumber("QuoteTotalVAT", entityPM.Tenant).ToString(),
                                Tenant = entityPM.Tenant,
                                QuoteId = entityPM.Id,
                                VatTypeId = item.Id,
                                VatPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                                LocalCurrencyVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                                QuoteCurrencyVatableAmount = MethodHelper.Roundd(item.QuoteCurrencyAmount, 2),
                                ProfitCurrencyVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                                ExternalVATCard = item.ExternalVATCard,
                                ExternalTAXItemId = item.ExternalTAXItemId
                            };

                            record.LocalCurrencyVATAmount = MethodHelper.Round((record.LocalCurrencyVatableAmount * record.VatPercent / 100), 2);
                            record.QuoteCurrencyVATAmount = MethodHelper.Round((record.QuoteCurrencyVatableAmount * record.VatPercent / 100), 2);
                            record.ProfitCurrencyVATAmount = MethodHelper.Round((record.ProfitCurrencyVatableAmount * record.VatPercent / 100), 2);
                            quoteTotalVATRepository.Add(record);
                        }
                    }
                }
            }
        }

        private void ComputeNumerOfFollowUps()
        {
            List<FollowUp> followUps = followUpRepository.GetFollowUpsByQuoteId(entityPM.Id, tenant);
            followUps = followUps.Where(d => !d.Done).ToList();

            if (followUps.Count > 0)
            {
                entityPM.NumberOfFollowUps = followUps.Count;
            }

            else
            {
                entityPM.NumberOfFollowUps = null;
            }
        }

        private void ComputeCountryForStatisticsId()
        {
            string fromPortId = entityPM.FromPortId;
            string toPortId = entityPM.ToPortId;

            //Import
            if (entityPM.DirectionId == "I")
            {
                PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, fromPortId, true);
                if (port != null)
                {
                    entityPM.CountryForStatisticsId = port.CountryId;
                }
            }

            //Export
            else if (entityPM.DirectionId == "E")
            {
                bool assigned = false;
                if (entityPM.IncludeDelivery)
                {
                    if (!string.IsNullOrEmpty(entityPM.ToAddressCountryId))
                    {
                        entityPM.CountryForStatisticsId = entityPM.ToAddressCountryId;
                        assigned = true;
                    }
                }

                if (!assigned)
                {
                    PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, toPortId, true);
                    if (port != null)
                    {
                        entityPM.CountryForStatisticsId = port.CountryId;
                    }
                }
            }

            //Domestic
            else if (entityPM.DirectionId == "D")
            {
                if (entityPM.TransportModeId == "I")
                {
                    if (!string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
                    {
                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        Address toAddress = addressRepository.GetSingleAddress(entityPM.ToPartnerAddressId, entityPM.Tenant);
                        if (toAddress != null)
                        {
                            entityPM.CountryForStatisticsId = toAddress.CountryId;
                        }
                    }
                }

                else
                {
                    PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, toPortId, true);
                    if (port != null)
                    {
                        entityPM.CountryForStatisticsId = port.CountryId;
                    }
                }
            }

            //Drop
            else if (entityPM.DirectionId == "R")
            {
                PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, toPortId, true);
                if (port != null)
                {
                    entityPM.CountryForStatisticsId = port.CountryId;
                }
            }
        }

        private void ComputeExpectedProfit()
        {
            //if (entityPM.EstimateProfit != null)
            //{
            //    double? myProfitAmount = 0;

            //    double? myCostAmountLocal = MethodHelper.Round(entityPM.QuoteCharges.Where(d => d.CostTotalAmountLocal != null).Sum(s => s.CostTotalAmountLocal), 2);
            //    double? mySaleAmountLocal = MethodHelper.Round(entityPM.QuoteCharges.Where(d => d.IsAllIN == false && d.SaleTotalAmountLocal != null).Sum(s => s.SaleTotalAmountLocal), 2);
            //    double? mySaleProfitLocal = MethodHelper.Round(mySaleAmountLocal - myCostAmountLocal, 2);

            //    if (entityPM.ExchangeRate != null && entityPM.ExchangeRate != 0)
            //    {
            //        myProfitAmount = MethodHelper.Round(mySaleProfitLocal / entityPM.ExchangeRate, 2);
            //    }

            //    if (entityPM.EstimateProfit != myProfitAmount)
            //    {
            //        throw new Exception("Wrong Estimate Profit");
            //    }
            //}
        }

        private void UpdateTariffUsedDate(string tariffId)
        {
            TariffRepository tariffRepository = new TariffRepository(tenant);
            Tariff tariff = tariffRepository.GetSingle(tariffId, tenant);
            if (tariff != null)
            {
                tariff.LastUsedDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                tariffRepository.Update(tariff);
                tariffRepository.SubmitChanges();
            }
        }

        private void ComputeQuoteChargesVATAmounts(QuoteChargePM quoteCharge)
        {
            this.ResetVATAmounts(quoteCharge);
                       
            VatType lineVatType = this.allVatTypes.Where(d => d.Id == quoteCharge.VatTypeId).FirstOrDefault();
            if (lineVatType == null) return;

            double? VATPercentage = this.GetQuoteChargeVATPercentage(quoteCharge, lineVatType);
            if (VATPercentage == null) return;

            quoteCharge.VATAmountInLocalCurrency = MethodHelper.Round((quoteCharge.SaleTotalAmountLocal * VATPercentage / 100), 2);
            quoteCharge.VATAmountInQuoteSaleCurrency = MethodHelper.Round((quoteCharge.SaleAmountInSaleCurrency * VATPercentage / 100), 2);
            quoteCharge.VATAmountInLineSaleCurrency = MethodHelper.Round((quoteCharge.SaleTotalAmount * VATPercentage / 100), 2);
            quoteCharge.SaleTotalAmountLocalIncludingVAT = quoteCharge.SaleTotalAmountLocal + quoteCharge.VATAmountInLocalCurrency;
            quoteCharge.SaleAmountInSaleCurrencyIncludingVAT = quoteCharge.SaleAmountInSaleCurrency + quoteCharge.VATAmountInQuoteSaleCurrency;
            quoteCharge.SaleTotalAmountIncludingVAT = quoteCharge.SaleTotalAmount + quoteCharge.VATAmountInLineSaleCurrency;

        }
        private void ResetVATAmounts(QuoteChargePM quoteCharge)
        {
            quoteCharge.VATAmountInLocalCurrency = null;
            quoteCharge.VATAmountInQuoteSaleCurrency = null;
            quoteCharge.VATAmountInLineSaleCurrency = null;
            quoteCharge.SaleTotalAmountLocalIncludingVAT = null;
            quoteCharge.SaleAmountInSaleCurrencyIncludingVAT = null;
            quoteCharge.SaleTotalAmountIncludingVAT = null;
        }
        private double? GetQuoteChargeVATPercentage(QuoteChargePM quoteCharge, VatType lineVatType)
        {
            double? VATPercentage = quoteCharge.VatPercentage;
            if (lineVatType.IsMultiPercentage)
            {
                List<VATTypesGroup> myVatGroups = (from d in myCommonContext.VATTypesGroups where d.Tenant == this.tenant && d.GroupVATTypeId == lineVatType.Id select d).ToList();
                VATPercentage = this.ComputeMultiVATPercentages(myVatGroups);                
            }

            if (this.IsQuoteHasRegionalTax(quoteCharge))
            {
                VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == entityPM.RegionalTaxId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    VATPercentage += myPercentagePM.Percentage;
                }
            }

            return VATPercentage;
        }
        private double? ComputeMultiVATPercentages(List<VATTypesGroup> myVatGroups)
        {
            double? multiVATPercentages = null;
            foreach (VATTypesGroup itemGroup in myVatGroups)
            {
                VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                if (myPercentagePM != null)
                {
                    if (multiVATPercentages == null)
                    {
                        multiVATPercentages = myPercentagePM.Percentage;
                    }

                    else
                    {
                        multiVATPercentages += myPercentagePM.Percentage;
                    }
                }
            }

            return multiVATPercentages;
        }
        private bool IsQuoteHasRegionalTax(QuoteChargePM quoteCharge)
        {
            if (!quoteCharge.IsRegionalTax)
            {
                return false;
            }

            else if (string.IsNullOrEmpty(entityPM.RegionalTaxId))
            {
                return false;
            }

            return true;
        }
    }    
    public class QuoteTotalsClass
    {
        [Key]
        public string Id { get; set; }
        public string VatTypeId { get; set; }
        public double? VatTypePercentage { get; set; }
        public string RowLabel { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? QuoteCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string ExternalVATCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public string VatTypeCell { get; set; }
    }
}
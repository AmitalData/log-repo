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

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public partial class QuoteService
    {
        private int tenant;
        private bool isNewEntity;
        private QuotePM entityPM;
        public Quote entityPoco { get; set; }
        private Tenant loggedTenant;
        private ContactPM loggedContact;
        private IQuotesContext objectContext;
        private QuoteRepository entityRepository;
        private FollowUpRepository followUpRepository;
        private QuoteChargeRepository quoteChargeRepository;
        private QuotePriceStepsRepository quotePriceStepsRepository;
        private QuotePackageRepository quotePackageRepository;
        private QuoteTotalVATRepository quoteTotalVATRepository;
        private QuoteDocumentVersionRepository quoteDocumentVersionRepository;
        private ICommonDataContext myCommonContext;
        private AddressRepository addressRepository;
        private bool isAdhoc;
        private bool isLCLQuote;
        private bool isFCLQuote;
        private bool isInlandDomestic;
        public QuoteService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.entityRepository = new QuoteRepository(objectContext);
            this.quoteChargeRepository = new QuoteChargeRepository(objectContext);
            this.quotePriceStepsRepository = new QuotePriceStepsRepository(objectContext);
            this.followUpRepository = new FollowUpRepository(tenant);
            this.quotePackageRepository = new QuotePackageRepository(objectContext);
            this.quoteTotalVATRepository = new QuoteTotalVATRepository(objectContext);
            this.addressRepository = new AddressRepository(myCommonContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, false);
            this.GetLoggedContact(HttpContext.Current.User.Identity.Name);
        }
        public QuoteService(IQuotesContext objectContext, int tenant, string email)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.entityRepository = new QuoteRepository(objectContext);
            this.quoteChargeRepository = new QuoteChargeRepository(objectContext);
            this.quotePriceStepsRepository = new QuotePriceStepsRepository(objectContext);
            this.followUpRepository = new FollowUpRepository(tenant);
            this.quotePackageRepository = new QuotePackageRepository(objectContext);
            this.quoteTotalVATRepository = new QuoteTotalVATRepository(objectContext);
            this.addressRepository = new AddressRepository(myCommonContext);
            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, false);
            this.GetLoggedContact(email);
        }

        private void GetLoggedContact(string serviceContextUser)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(serviceContextUser, tenant);
            }

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetSinglePM(entityPM.CreatedByUserId, tenant);
            }
        }

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
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("Quote", tenant).ToString();
            this.entityPoco = new Quote() { Id = this.entityPM.Id };

            if (!entityPM.IsCopy)
            {
                QuoteSettingRepository iQuoteSettingRepository = new QuoteSettingRepository(objectContext);
                QuoteSetting iQuoteSetting = iQuoteSettingRepository.GetSingleQuoteSetting(tenant);
                if (iQuoteSetting != null)
                {
                    this.entityPM.IsSaleCurrencySameAsCost = iQuoteSetting.IsSaleAsCostCurrency;
                }
            }

            this.InitializeComponent();

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

            QuoteTracing.Trace(entityPM, entityPoco, loggedContact.Id, isNewEntity);
            QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);

            entityRepository.Add(entityPoco);
            entityRepository.SubmitChanges();
            followUpRepository.SubmitChanges();

            this.GetForeignFields(entityPM, entityPoco);

            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Quote", 0, true);
            ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "N", loggedContact.Id);
        }
        public void Update(QuotePM entityPM, bool mapComposition = false)
        {
            //if(entityPM.TotalPerContainer && entityPM.IsSaleCurrencySameAsCost)
            //{
            //   throw new Exception("Total per container cannot be chosen with Same as cost currency");
            //}

            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPoco = entityRepository.GetSingleQuote(entityPM.Id, tenant);
            
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

                QuotetValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext);

                this.UpdateQuoteChargesCollection();
                this.UpdateQuoteFollowUpsCollection();
                this.UpdateQuotePackageCollection();
                this.UpdateQuoteDocumentVersionCollection();
                this.UpdateTotalVats();

                QuoteTracing.Trace(entityPM, entityPoco, loggedContact.Id, isNewEntity);

                ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPoco.Tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Quote", 0, true);

                if (!entityPM.DontExportQuotationsToIntegratedSystem)
                {
                    SentQuoteStatusMessageToUnifreight(objecttable.Id);
                }

                QuoteMapping.MapEntity(entityPM, entityPoco, isNewEntity);

                entityRepository.Update(entityPoco);
                entityRepository.SubmitChanges();
                followUpRepository.SubmitChanges();

                this.GetForeignFields(entityPM, entityPoco);

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Quote");
                ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "U", loggedContact.Id);
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
        }

        public void SentQuoteStatusMessageToUnifreight(string objectTableId)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            bool IsQuoteStageChange = false;

                if (tenantPM.ExportQuotationsToIntegratedSystem)
                {
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
                                    if (stage.Code == "QTAC") quoteStatus.QuoteAcceptNote = entityPM.EventNote;
                                    if (stage.Code == "QTDC")
                                    {
                                        QuoteClosingReasonRepository closingReasonRepository = new QuoteClosingReasonRepository(tenant);
                                        QuoteClosingReason myQuoteClosingReason = closingReasonRepository.GetSingleQuoteClosingReason(entityPM.QuoteClosingReasonCode);
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
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.OpenDate = entityPM.IsHybrid ? entityPM.OpenDate : todayDateTime;
                entityPM.LastStageDate = todayDateTime;

                if (!entityPM.IsHybrid)
                {
                    entityPM.QuoteNumber = TableCounter.GetNumber(tenant, "QUOT", entityPM.DirectionId, entityPM.TransportModeId);
                }

                this.InitializeStage();
                this.InitializeSaleCurrency();
                this.InitializeSalesman();

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
            
            entityPM.UpdatedByUserId = loggedContact.Id;
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

            if (!entityPM.IsHybrid)
            {
                InitializeSubject();
            }

            InitializeExpirationValues();
            InitializeAutomaticallyClose();

            if (entityPM.ConvertToLCL || entityPM.ConvertToFCL)
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
                entityPM.NumberOfPackages = null;
                entityPM.GrossWeight = null;
                entityPM.ChargeableWeight = null;
                entityPM.VolumetricWeight = null;
                entityPM.Volume = null;

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
                if(customer != null)
                {
                    entityPM.CustomerName = customer.EnglishName;                    
                }
            }

            //if (entityPM.QuoteCustomerTypeCode == "SHI")
            //{
            //    if (string.IsNullOrEmpty(entityPM.CustomerId))
            //    {
            //        entityPM.CustomerId = entityPM.ShipperId;
            //    }

            //    entityPM.CustomerName = entityPM.ShipperName;
            //    entityPM.CustomerNote = entityPM.ShipperNote;
            //    entityPM.CustomerContactId = entityPM.ShipperContactId;
            //    entityPM.CustomerReference1 = entityPM.ShipperReference1;
            //    entityPM.CustomerReference2 = entityPM.ShipperReference2;
            //}

            //else if (entityPM.QuoteCustomerTypeCode == "CON")
            //{
            //    if (string.IsNullOrEmpty(entityPM.CustomerId))
            //    {
            //        entityPM.CustomerId = entityPM.ConsigneeId;
            //    }

            //    entityPM.CustomerName = entityPM.ConsigneeName;
            //    entityPM.CustomerNote = entityPM.ConsigneeNote;
            //    entityPM.CustomerContactId = entityPM.ConsigneeContactId;
            //    entityPM.CustomerReference1 = entityPM.ConsigneeReference1;
            //    entityPM.CustomerReference2 = entityPM.ConsigneeReference2;
            //}

            //else
            //{
            //    if (!string.IsNullOrEmpty(entityPM.CustomerId))
            //    {
            //        if (entityPM.CustomerId == entityPM.AgentId)
            //        {
            //            entityPM.CustomerName = entityPM.AgentName;
            //            entityPM.CustomerContactId = entityPM.AgentContactId;
            //            entityPM.CustomerReference1 = entityPM.AgentReference1;
            //            entityPM.CustomerReference2 = entityPM.AgentReference2;
            //        }

            //        if (entityPM.CustomerId == entityPM.NotifyId)
            //        {
            //            entityPM.CustomerName = entityPM.NotifyName;
            //            entityPM.CustomerContactId = entityPM.NotifyContactId;
            //            entityPM.CustomerReference1 = null;
            //            entityPM.CustomerReference2 = null;
            //        }
            //    }
            //}
        }
        private void InitializeSalesman()
        {
            if (string.IsNullOrEmpty(entityPM.SalesmanUserId))
            {
                if (!string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    CustomerRepository customerRepository = new CustomerRepository(tenant);
                    Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, tenant, false);
                    if (customer != null)
                    {
                        if (!string.IsNullOrEmpty(customer.SalesmanUserId))
                        {
                            if (entityPM.SalesmanUserId != customer.SalesmanUserId)
                            {
                                entityPM.SalesmanUserId = customer.SalesmanUserId;
                            }
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(entityPM.SalesmanUserId))
            {
                if (entityPM.SalesmanUserId != entityPM.CreatedByUserId)
                {
                    entityPM.SalesmanUserId = entityPM.CreatedByUserId;
                }
            }

            UserRepository userRepository = new UserRepository(tenant);
            User user = userRepository.GetSingleUser(entityPM.SalesmanUserId, tenant, false);
            if (user != null)
            {
                if (entityPM.BusinessUnitId != user.BusinessUnitId)
                {
                    entityPM.BusinessUnitId = user.BusinessUnitId;
                }
            }
        }
        private void InitializeSaleCurrency()
        {
            if (isNewEntity)
            {
                if (string.IsNullOrEmpty(entityPM.SaleCurrencyId))
                {
                    entityPM.SaleCurrencyId = loggedTenant.QuoteSaleCurrencyId;
                }

                if (string.IsNullOrEmpty(entityPM.SaleCurrencyId))
                {
                    entityPM.ExchangeRate = null;
                }

                else if (entityPM.SaleCurrencyId == loggedTenant.CurrencyId)
                {
                    entityPM.ExchangeRate = 1;
                }

                else
                {
                    RatesTableQuery myQuery = new RatesTableQuery(tenant);
                    LastRate lastRate = myQuery.GetLastRecordByValueDate(tenant, entityPM.SaleCurrencyId, loggedTenant.CurrencyId, entityPM.OpenDate);
                    if (lastRate != null)
                    {
                        entityPM.ExchangeRate = MethodHelper.Round(lastRate.Rate, 5);
                    }
                }
            }
        }
        private void InitializeInlandDomestic()
        {
            if (isInlandDomestic)
            {
                entityPM.FromPortId = null;
                entityPM.ToPortId = null;
                entityPM.IncludePickUp = false;
                entityPM.IncludeDelivery = false;

                if (!string.IsNullOrEmpty(entityPM.ShipperId))
                {
                    if (string.IsNullOrEmpty(entityPM.FromPartnerId))
                    {
                        entityPM.FromPartnerId = entityPM.ShipperId;
                    }

                    if (string.IsNullOrEmpty(entityPM.FromPartnerAddressId))
                    {
                        Address adr = addressRepository.GetMainAddressByCardId(entityPM.ShipperId, tenant);
                        if (adr != null)
                        {
                            entityPM.FromPartnerAddressId = adr.Id;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
                {
                    if (string.IsNullOrEmpty(entityPM.ToPartnerId))
                    {
                        entityPM.ToPartnerId = entityPM.ConsigneeId;
                    }

                    if (string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
                    {
                        Address adr = addressRepository.GetMainAddressByCardId(entityPM.ConsigneeId, tenant);
                        if (adr != null)
                        {
                            entityPM.ToPartnerAddressId = adr.Id;
                        }
                    }
                }
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

            if (loggedTenant.IsQuoteSubjectEdited && entityPM.IsSubjectEdited)
            {
                isAutomaticUpdate = false;
            }

            if (isAutomaticUpdate)
            {
                QuoteQuery entityQuery = new QuoteQuery(entityRepository);

                string mySubject = entityQuery.GetQuoteAutomaticSubject(entityPM);

                entityPM.Subject = mySubject;
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
                        if (entityPM.AutomaticallyCloseDate.Value.Date == todayDateTime)
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
                        if (entityPM.AutomaticallyCloseDate.Value.Date == todayDateTime)
                        {
                            this.CloseEntityAutomatically();
                        }
                    }
                }
            }
        }
        private void CloseEntityAutomatically()
        {
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
                                    UserId = this.loggedContact.Id,
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
                                        UserId = this.loggedContact.Id,
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

                        QuoteTracing.TraceQuoteOnCreateDoneFollowUp(entityPM, entityPoco, itemPoco, loggedContact.Id);
                    }
                }

                foreach (QuoteFollowUpPM itemPM in quoteFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Update))
                {
                    if (itemPM.Done)
                    {
                        itemPM.Deleted = true;
                        entityPM.MarkFollowUpsAsDone = true;

                        QuoteTracing.TraceQuoteOnUpdateDoneFollowUp(entityPM, entityPoco, itemPM, loggedContact.Id);

                        this.DeleteQuoteFollowUp(itemPM);
                        followUpRepository.SubmitChanges();

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "QFCM",
                            UserId = this.loggedContact.Id,
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
                pdfData = quoteTemplateService.GetQuoteTemplatePdfReport(itemPM.QuoteId, itemPM.QuoteTemplateId, itemPM.CreatedByUserId, itemPM.Tenant, null);
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

            //string localPath = "quotetemplatesectionfiles/" + document.Id + "." + document.Extension;
            //var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
            //var blobfile = blobContainer.GetBlockBlobReference(localPath);
            //using (Stream blobstream = blobfile.OpenWrite())
            //{
            //    blobstream.Write(pdfData, 0, (int)pdfData.Length);
            //}

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

            //QuoteRepository quoteRep = new QuoteRepository(objectContext);
            //Quote quote = quoteRep.GetSingleQuote(itemPM.QuoteId, itemPM.Tenant);
            //quote.LastVersionNumber = itemPM.VersionNumber;
            //quote.QuoteTemplateId = itemPM.QuoteTemplateId;

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
                    newDocumentFiling.CreatedByUserId = loggedContact.Id;
                    newDocumentFiling.OwnerId = loggedContact.Id;
                    newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.UpdatedByUserId = loggedContact.Id;
                    newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.HasCopies = true;
                    newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;

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
                    List<QuoteChargePM> myDataLines = this.entityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.VatTypeId != null).ToList();
                    if (myDataLines.Count > 0)
                    {
                        double? myProfitCurrencyRate = null;
                        RatesTableQuery myQuery = new RatesTableQuery(tenant);
                        LastRate lastRate = myQuery.GetLastRecordByValueDate(tenant, loggedTenant.ProfitCurrencyId, loggedTenant.CurrencyId, entityPM.UpdateDate);
                        if (lastRate != null)
                        {
                            myProfitCurrencyRate = MethodHelper.Round(lastRate.Rate, 5);
                        }

                        List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups
                                                            where d.Tenant == this.tenant
                                                            select d).ToList();

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
                                        QuoteCurrencyAmount = item.SaleTotalAmount,
                                        LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                        ExternalVATCard = lineVatType.ExternalVATCard,
                                        ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                                    };

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
                                            QuoteCurrencyAmount = item.SaleTotalAmount,
                                            LocalCurrencyAmount = item.SaleTotalAmountLocal,
                                        };

                                        VatType vatType = this.allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                        if (vatType != null)
                                        {
                                            newItem.ExternalVATCard = vatType.ExternalVATCard;
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
                                 //ProfitCurrencyAmount = g.Sum(s => s.SaleTotalAmount),
                             }).ToList();                       

                        foreach (var item in group_data)
                        {
                            if (entityPM.SaleCurrencyId == loggedTenant.ProfitCurrencyId)
                            {
                                item.ProfitCurrencyAmount = item.QuoteCurrencyAmount;
                            }

                            else if (entityPM.SaleCurrencyId == loggedTenant.CurrencyId)
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
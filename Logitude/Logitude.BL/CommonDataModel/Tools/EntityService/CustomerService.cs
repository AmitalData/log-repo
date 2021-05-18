using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.EmailAlerts;
using Logitude.BL.CommonDataModel.Tools.HybridMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Social.BL.Helpers;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Logitude.Accounting.Def.EntityUpdateServicesExt;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Microsoft.Practices.Unity;
using Logitude.BL.DataContracts;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerService
    {
        bool isNewEntity;
        private int tenant;
        public Customer entityPOCO { get; set; }
        public string OverrideLoggingUserId { get; private set; }

        private Card entityCard;
        private CustomerPM entityPM;
        private Contact loggedContact;
        private ICommonDataContext objectContext;
        private CustomerRepository entityRepository;
        private AddressRepository addressRepository;
        private CardRepository cardRepository;
        private ContactRepository contactRepository;
        private CardContactRepository cardContactRepository;
        private CustomerProductRepository productRepository;
        private CustomerSalesNoteRepository salesNoteRepository;
        private CustomerProductLocationRepository productLocationRepository;
        private CustomerCompetitorRepository competitorsRepository;
        private CustomerCompetitorProductRepository competitorProductsRepository;
        private CustomerAdditionalServiceRepository servicesRepository;
        private CustomerSalesmanByProductRepository customerSalesmanByProductRepository;
        private CustomerAccountManagerByProductRepository customerAccountManagerByProductRepository;
        private CustomerCustomsAgentByProductRepository customerCustomsAgentByProductRepository;
        private CustomerForwarderByProductRepository customerForwarderByProductRepository;
        private CustomerMediatorByProductRepository customerMediatorByProductRepository;
        private CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository;
        private ProductItemRepository productItemRepository;
        private HTSCodeRepository hTSCodeRepository;
        private CardService cardService;
        ContactService contactService;
        HybridPartnerPM CurrentHybridPartner;

        public CustomerService(ICommonDataContext objectContext, CustomerPM entityPM)
        {
            this.Initialization(objectContext, entityPM);
            this.GetLoggedContact();
            this.SetHybridPartner(this.tenant);
        }
        public CustomerService(ICommonDataContext objectContext, CustomerPM entityPM, string loggedContactId)
        {
            this.Initialization(objectContext, entityPM);
            this.loggedContact = contactRepository.GetSingleContact(loggedContactId, tenant);
            this.SetHybridPartner(this.tenant);
        }
        private void Initialization(ICommonDataContext objectContext, CustomerPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CustomerRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.cardContactRepository = new CardContactRepository(objectContext);
            this.productRepository = new CustomerProductRepository(objectContext);
            this.productLocationRepository = new CustomerProductLocationRepository(objectContext);
            this.competitorsRepository = new CustomerCompetitorRepository(objectContext);
            this.competitorProductsRepository = new CustomerCompetitorProductRepository(objectContext);
            this.servicesRepository = new CustomerAdditionalServiceRepository(objectContext);
            this.salesNoteRepository = new CustomerSalesNoteRepository(objectContext);
            this.customerSalesmanByProductRepository = new CustomerSalesmanByProductRepository(objectContext);
            this.customerAccountManagerByProductRepository = new CustomerAccountManagerByProductRepository(objectContext);
            this.customerCustomsAgentByProductRepository = new CustomerCustomsAgentByProductRepository(objectContext);
            this.customerForwarderByProductRepository = new CustomerForwarderByProductRepository(objectContext);
            this.customerMediatorByProductRepository = new CustomerMediatorByProductRepository(objectContext);
            this.cardExternalCodeByCurrencyRepository = new CardExternalCodeByCurrencyRepository(objectContext);
            this.productItemRepository = new ProductItemRepository(objectContext);            
            this.hTSCodeRepository = new HTSCodeRepository(objectContext);
            this.contactService = new ContactService(objectContext, tenant);
            cardService = new CardService(objectContext, tenant);
        }
        private void GetLoggedContact()
        {
            if (!string.IsNullOrWhiteSpace(this.OverrideLoggingUserId))
            {
                this.loggedContact = contactRepository.GetSingleContact(this.OverrideLoggingUserId, tenant);
            }
            else
            {
                string email = HttpContext.Current.User.Identity.Name;
                AuthenticationUtil.ResolveLoggingUserId(tenant);
                this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }

            if (this.loggedContact == null)
            {
                this.loggedContact = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant);
            }
        }

        private void SetHybridPartner(int myTenant)
        {
            HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(myTenant);
            CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(myTenant);
        }

        private List<CustomerSalesNotePM> salesNotesChangeSet;
        private List<CustomerProductPM> productsChangeSet;
        private List<CustomerCompetitorPM> competitorsChangeSet;
        private List<CustomerAdditionalServicePM> servicesChangeSet;
        private List<CustomerSalesmanByProductPM> customerSalesmanByProductsChangeSet;
        private List<CustomerAccountManagerByProductPM> customerAccountManagerByProductChangeSet;
        private List<CustomerCustomsAgentByProductPM> customerCustomsAgentByProductChangeSet;
        private List<CustomerForwarderByProductPM> customerForwarderByProductChangeSet;
        private List<CustomerMediatorByProductPM> customerMediatorByProductChangeSet;
        private List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencyChangeSet;
        private List<ProductItemPM> productItemsChangeSet;

        public void SetChangeSet(List<CustomerSalesNotePM> salesNotesChangeSet, List<CustomerProductPM> productsChangeSet, List<CustomerCompetitorPM> competitorsChangeSet, List<CustomerAdditionalServicePM> servicesChangeSet, List<CustomerSalesmanByProductPM> customerSalesmanByProductsChangeSet, List<CustomerAccountManagerByProductPM> customerAccountManagerByProductChangeSet, List<CustomerCustomsAgentByProductPM> customerCustomsAgentByProductChangeSet, List<CustomerForwarderByProductPM> customerForwarderByProductChangeSet, List<CustomerMediatorByProductPM> customerMediatorByProductChangeSet, List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrencyChangeSet, List<ProductItemPM> productItemsChangeSet)
        {
            this.salesNotesChangeSet = salesNotesChangeSet;
            this.productsChangeSet = productsChangeSet;
            this.competitorsChangeSet = competitorsChangeSet;
            this.servicesChangeSet = servicesChangeSet;
            this.customerSalesmanByProductsChangeSet = customerSalesmanByProductsChangeSet;
            this.customerAccountManagerByProductChangeSet = customerAccountManagerByProductChangeSet;
            this.customerCustomsAgentByProductChangeSet = customerCustomsAgentByProductChangeSet;
            this.customerForwarderByProductChangeSet = customerForwarderByProductChangeSet;
            this.customerMediatorByProductChangeSet = customerMediatorByProductChangeSet;
            this.cardExternalCodeByCurrencyChangeSet = cardExternalCodeByCurrencyChangeSet;
            this.productItemsChangeSet = productItemsChangeSet;
        }

        public void Create()
        {
            this.isNewEntity = true;
            this.entityPM.Id = string.IsNullOrEmpty(this.entityPM.Id) || this.entityPM.IsHybrid ? IdCounter.GetNumber("Card", tenant).ToString() : this.entityPM.Id;

            this.entityCard = new Card()
            {
                Id = entityPM.Id,
                Tenant = tenant,
                SharedLogisticsInvitationStatusCode = 1,
                UploadingUniqueKey = entityPM.UploadingUniqueKey,
            };

            this.entityPOCO = new Customer()
            {
                Id = entityPM.Id,
                Tenant = tenant,
            };

            this.InitializeComponent();

            CustomerValidating.Validate(entityPM, isNewEntity, this.objectContext);

            RankRepository rankRep = new RankRepository(objectContext);
            Rank rank = rankRep.GetSingleRankByCode("1", tenant);

            if (rank != null)
            {
                entityPM.RankId = rank.Id;
            }

            if (!entityPM.IsHybrid)
            {
                CustomerTracing myTracingClass = new CustomerTracing(entityPM, entityPOCO, loggedContact.Id, isNewEntity);
                myTracingClass.Trace();
                myTracingClass.TraceProducts(entityPM.CustomerProducts, isNewEntity);
                myTracingClass.TraceCompetitors(entityPM.CustomerCompetitors, isNewEntity);
                myTracingClass.TraceAdditionalServices(entityPM.CustomerAdditionalServices, isNewEntity);
                myTracingClass.TraceProductItems(entityPM.CustomerProductItems, isNewEntity);
            }

            foreach (CustomerProductPM item in entityPM.CustomerProducts)
            {
                this.CreateCustomerProduct(item);
            }

            foreach (CustomerCompetitorPM item in entityPM.CustomerCompetitors)
            {
                this.CreateCustomerCompetitor(item);
            }

            foreach (CustomerAdditionalServicePM item in entityPM.CustomerAdditionalServices)
            {
                this.CreateCustomerAdditionalService(item);
            }

            foreach (CustomerSalesmanByProductPM item in entityPM.CustomerSalesmanByProducts)
            {
                this.CreateCustomerSalesmanByProduct(item);
            }

            foreach (CustomerAccountManagerByProductPM item in entityPM.CustomerAccountManagerByProducts)
            {
                this.CreateCustomerAccountManagerByProduct(item);
            }

            foreach (CustomerCustomsAgentByProductPM item in entityPM.CustomerCustomsAgentByProducts)
            {
                this.CreateCustomerCustomsAgentByProduct(item);
            }

            foreach (CustomerForwarderByProductPM item in entityPM.CustomerForwarderByProducts)
            {
                this.CreateCustomerForwarderByProduct(item);
            }

            foreach (CustomerMediatorByProductPM item in entityPM.CustomerMediatorByProducts)
            {
                this.CreateCustomerMediatorByProduct(item);
            }

            foreach (CardExternalCodeByCurrencyPM item in entityPM.CardExternalCodeByCurrencies)
            {
                this.CreateCardExternalCodeByCurrency(item);
            }

            foreach (CustomerSalesNotePM itemPM in entityPM.SalesNotes)
            {
                this.CreateCustomerSalesNote(itemPM);
            }

            foreach (AddressPM itemPM in entityPM.Addresses)
            {
                this.CreateAddress(itemPM);
            }

            foreach (ContactPM itemPM in entityPM.Contacts)
            {
                this.CreateContact(itemPM);
            }

            CustomerMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);

            cardRepository.Add(entityCard);
            entityRepository.Add(entityPOCO);
            entityRepository.SubmitChanges();

            foreach (ContactPM itemPM in entityPM.Contacts)
            {
                this.UpdateContactSearchField(itemPM);
            }

            if (!entityPM.IsHybrid)
            {
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Customer", 0, true);

                if (entityPOCO.IsCustomer)
                {
                    ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "N", loggedContact.Id);
                }
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Customer");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");

             string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
        }

        public void Update()
        {

            this.isNewEntity = false;

            this.entityPOCO = entityRepository.GetSingleCustomer(entityPM.Id, tenant, false);
            this.entityCard = cardRepository.GetSingleCard(entityPM.Id, entityPM.Tenant);

            this.InitializeComponent();

            CustomerValidating.Validate(entityPM, isNewEntity, this.objectContext);

            if (entityPM.IsHybrid)
            {
                if (this.entityPOCO == null)
                {
                    this.entityPOCO = entityRepository.GetSingleCustomerByVat(entityPM.Id, entityPM.Tenant, false);
                }

                if (entityPM.Code != this.entityCard.Code)
                {
                    DocumentsFilingService docmentsService = new DocumentsFilingService(this.objectContext, entityPM.Tenant);
                    ObjectTableQuery tablesQuery = new ObjectTableQuery(entityPM.Tenant);
                    ObjectTablePM table = tablesQuery.GetObjectTableByName("Customer", 0);

                    DocumentsFilingQuery docsQuery = new DocumentsFilingQuery(entityPM.Tenant);
                    var docsList = docsQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityPM.Id, null, table.Id, "I", entityPM.Tenant);
                    foreach (var doc in docsList)
                    {
                        docmentsService.AddToTasksQueue(doc, false, loggedContact.Id);
                    }
                }

                this.entityCard.Code = entityPM.Code;
                this.entityPOCO.Card.Code = entityPM.Code;
            }

            if (CacheManager.CacheWrapper != null)
            {
                string entityName = "Card" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "CardPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }

                entityName = "Customer" + entityPM.Id + entityPM.Tenant;
                entityPmName = "CustomerPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }


                entityPmName = "BasicCustomerPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }

            this.UpdateProductsCollection();
            this.UpdateServicesCollection();
            this.UpdateCompetitorsCollection();
            this.UpdateSalesNotesCollection();
            this.UpdateCustomerSalesmanByProductCollection();
            this.UpdateCustomerAccountManagerCollection();
            this.UpdateCustomerCustomsAgentCollection();
            this.UpdateForwarderCollection();
            this.UpdateCustomerMediatorByProductCollection();
            this.UpdateCardExternalCodeByCurrencyCollection();
            this.UpdateProductItemsCollection();

            this.UpdateGLAccount(entityPM, entityPOCO);
           
            //var tenantQuery = new TenantQuery(entityPM.Tenant);
            //var tenantPM = tenantQuery.GetSinglePM(entityPM.Tenant);
            if (!entityPM.IsHybrid && !entityPM.IsLogBox)
            {
                CustomerTracing myTracingClass = new CustomerTracing(entityPM, entityPOCO, loggedContact.Id, isNewEntity);
                myTracingClass.Trace();

                if (productsChangeSet != null)
                {
                    myTracingClass.TraceProducts(productsChangeSet, isNewEntity);
                }

                if (competitorsChangeSet != null)
                {
                    myTracingClass.TraceCompetitors(competitorsChangeSet, isNewEntity);
                }

                if (servicesChangeSet != null)
                {
                    myTracingClass.TraceAdditionalServices(servicesChangeSet, isNewEntity);
                }

                if (productItemsChangeSet != null)
                {
                    myTracingClass.TraceProductItems(productItemsChangeSet, isNewEntity);
                }
            }

            if (entityPM.SetReady && entityPOCO.CustomerStatusCode != "WAC")
            {
                try
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    AzureLog.SaveLogsInStorage("Before adding customer to activation queue (Id:" + entityPM.Id + ",Tenant:" + entityPM.Tenant + ")", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
                    using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
                    {
                        QueueClient client = ServiceBusQueueHelper.CreateCustomerQueue(entityPM.Tenant);
                        BrokeredMessage message = new BrokeredMessage();
                        message.Properties["CustomerId"] = entityPM.Id;
                        message.Properties["Tenant"] = tenant;

                        client.Send(message);
                        scope.Complete();

                        AzureLog.SaveLogsInStorage("After customer added to activation queue (Id:" + entityPM.Id + ",Tenant:" + entityPM.Tenant + ")", "L", DateTime.Now, "", "", 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
                    }

                    AddCustomerToQueue();
                }
                catch (Exception ex)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    AzureLog.SaveLogsInStorage("Error While Adding Customer to activation queue (Id:" + entityPM.Id + ",Tenant:" + entityPM.Tenant + ")", "E", DateTime.Now, ex.Message, ex.StackTrace, 0, loggedContact.Id, loggedContact.EnglishName, currentIP);
                    throw ex;
                }
            }

            if (entityPOCO.CustomerStatusCode != "ACT" && !entityPM.IsHybrid)
            {
                if (entityPM.SetActivated || entityPM.SetReady || entityPM.SavedForActivation)
                {
                    AddressQuery addressQuery = new AddressQuery(entityPOCO.Tenant);
                    AddressPM mainAddress = addressQuery.GetAddressPMByTypeAndCard(entityPOCO.Id, "M", entityPOCO.Tenant);
                    if (mainAddress != null)
                    {
                        this.MapAddressCustomer(mainAddress, entityPM);
                        AddressService addressService = new AddressService(objectContext, entityPOCO.Tenant);
                        mainAddress.IsHybrid = true;
                        addressService.Update(mainAddress);
                    }
                }
            }
            if ((entityPM.LogBoxActivated != entityPOCO.LogBoxActivated) || (entityPM.IsPrivateLabelCustomer != entityPOCO.IsPrivateLabelCustomer))
            {
                AddLogboxCustomerToQueue();
            }
            CustomerMapping.MapEntity(entityPM, entityPOCO, isNewEntity, entityCard);

            cardRepository.Update(entityCard);
            entityRepository.Update(entityPOCO);
            entityRepository.SubmitChanges();
            cardRepository.SubmitChanges();
            cardService.HandleGLAccountCardData(entityCard.Id, entityCard.GLAccountId, entityPM.Tenant);
            if (!entityPM.IsHybrid)
            {
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPOCO.Tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Customer", 0, true);

                if (entityPOCO.IsCustomer)
                {
                    ActivityLogger.AddAcitivityLog(entityPM.Id, objecttable.Id, entityPM.Tenant, "U", loggedContact.Id);
                }
            }

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Customer");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            { 
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
        }
       
        private void UpdateGLAccount(CustomerPM entityPM, Customer entityPOCO)
        {
            if ((entityPOCO.Card != null && entityPOCO.Card.EnglishName != entityPM.EnglishName) || (entityPOCO.Card != null && entityPOCO.Card.LocalName != entityPM.LocalName))
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                {
                    FullAccountingHelper fullAccountingHelper = new FullAccountingHelper();
                    GLAccountData data = new GLAccountData();
                    data.Tenant = entityPM.Tenant;
                    data.EnglishName = entityPM.EnglishName;
                    data.LocalName = entityPM.LocalName;
                    data.EntityId = entityPM.Id;
                    fullAccountingHelper.UpdateGLAccount(data);
                }
            }
        }

        private void CreateGLAccount()
        {

        }

        private void AddCustomerToQueue()
        {
            if (LogitudeSettings.EnableHybridQueue && (CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                    ObjectTablePM table = tablesQuery.GetObjectTableByName("Customer", tenant);
                    CommunicationsParams logParams = new CommunicationsParams()
                    {
                        Tenant = tenant,
                        CommunicationLogTypeCode = "Q",
                        QueueName = "externaltasksqueue" + tenant + 1,
                        Priority = 1,
                        InOut = "O",
                        Status = "W",
                        LoggingUserId = loggedContact.Id,
                        LoggingObjectTableId = table.Id,
                        LoggingEntityId = entityPM.Id,
                        Subject = "Customer ready for activation",
                        FolderName = "ExternalTasksQueue",
                    };

                    CustomerPM mappedpm = CustomerHybridMapping.MapEntityToHybrid(entityPM);
                    string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(mappedpm);
                    List<QueueTask> tasks = new List<QueueTask>();

                    tasks.Add(new QueueTask() { Action = "Customer.ReadyForActivation", Parameters = new List<Parameter>() { new Parameter { Order = 1, Value = xmlstring } } });

                    logParams.ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                    Communications.AddCommunicationLog(logParams);

                    scope.Complete();
                }
            }
        }

        private void AddLogboxCustomerToQueue()
        {
            if (LogitudeSettings.EnableHybridQueue && (CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                    ObjectTablePM table = tablesQuery.GetObjectTableByName("Customer", tenant);
                    CommunicationsParams logParams = new CommunicationsParams()
                    {
                        Tenant = tenant,
                        CommunicationLogTypeCode = "Q",
                        QueueName = "externaltasksqueue" + tenant + 1,
                        Priority = 1,
                        InOut = "O",
                        Status = "W",
                        LoggingUserId = loggedContact.Id,
                        LoggingObjectTableId = table.Id,
                        LoggingEntityId = entityPM.Id,
                        Subject = "LogBox Customer Status",
                        FolderName = "ExternalTasksQueue",
                    };

                    CustomerPM mappedpm = CustomerHybridMapping.MapEntityToHybrid(entityPM);
                    string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(mappedpm);
                    List<QueueTask> tasks = new List<QueueTask>();
                    if (entityPM.IsPrivateLabelCustomer == true || entityPOCO.IsPrivateLabelCustomer == true)
                    {
                        tasks.Add(new QueueTask() { Action = "Customer.PrivateLabel", Parameters = new List<Parameter>() { new Parameter { Order = 1, Value = entityPM.Code }, new Parameter { Order = 3, Value = entityPM.IsPrivateLabelCustomer.ToString() }, new Parameter { Order = 4, Value = entityPM.CustomerTenant.ToString() } } });
                    }
                    else
                    {
                        tasks.Add(new QueueTask() { Action = "Customer.LogBoxActivated", Parameters = new List<Parameter>() { new Parameter { Order = 1, Value = entityPM.Code }, new Parameter { Order = 2, Value = entityPM.LogBoxActivated.ToString() } } });
                    }


                    logParams.ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                    Communications.AddCommunicationLog(logParams);

                    scope.Complete();
                }
            }
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = (entityPM.IsHybrid && entityPM.CreateDate != null) ? entityPM.CreateDate : TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = (entityPM.IsHybrid && entityPM.CreateDate != null) ? entityPM.CreateDate : TenantServerConfigration.GetCurrentDateTime(tenant);

                if (string.IsNullOrEmpty(entityPM.CreatedByUserId))
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                    entityPM.UpdatedByUserId = loggedContact.Id;
                }

                if (!entityPM.IsHybrid && (string.IsNullOrEmpty(entityPM.Code) || entityPM.Code == "new"))
                {
                    entityPM.Code = CodeCounter.GetNumber("Customer", tenant).ToString();
                }

                this.CreatePotentialMainAddress();
                this.CreateGLAccount();
            }

            else
            {
                entityPM.UpdateDate = (entityPM.IsHybrid && entityPM.UpdateDate != null) ? entityPM.UpdateDate : TenantServerConfigration.GetCurrentDateTime(tenant);
                if (loggedContact != null)
                    entityPM.UpdatedByUserId = loggedContact.Id;
            }

            this.SetCustomerStatus();
            this.InitializeCardFields();
            this.ComputeContactFields();
        }

        private void CreatePotentialMainAddress()
        {
            if (isNewEntity)
            {
                if (entityPM.CustomerStatusCode == "POT")
                {
                    if (entityPM.IsLocalLanguage)
                    {
                        AddressPM localAddress = new AddressPM()
                        {
                            Id = IdCounter.GetNumber("Address", tenant).ToString(),
                            Tenant = tenant,
                            AddressTypeId = "L",
                            Description = "Local Address",
                            Name = entityPM.LocalName,
                            Address1 = entityPM.Address1_Potential,
                            Address2 = entityPM.Address2_Potential,
                            City = entityPM.City_Potential,
                            CountryId = entityPM.CountryId_Potential,
                            StateId = entityPM.StateId_Potential,
                            ZipCode = entityPM.ZipCode_Potential,
                            FaxNumber = entityPM.FaxNumber_Potential,
                            PhoneNumber = entityPM.PhoneNumber_Potential,
                            ATTN = entityPM.ATTN_Potential,
                            SearchFields = entityPM.Address1_Potential + "," + entityPM.Address2_Potential
                        };

                        entityPM.Addresses.Add(localAddress);

                        AddressPM mainAddress = new AddressPM()
                        {
                            Id = IdCounter.GetNumber("Address", tenant).ToString(),
                            Tenant = tenant,
                            AddressTypeId = "M",
                            Description = "Main Address",
                            Name = entityPM.EnglishName,
                            CountryId = entityPM.CountryId_Potential,
                            StateId = entityPM.StateId_Potential,
                        };

                        entityPM.Addresses.Add(mainAddress);
                    }

                    else
                    {
                        AddressPM address = new AddressPM()
                        {
                            Id = IdCounter.GetNumber("Address", tenant).ToString(),
                            Tenant = tenant,
                            AddressTypeId = "M",
                            Description = "Main Address",
                            Name = entityPM.EnglishName,
                            Address1 = entityPM.Address1_Potential,
                            Address2 = entityPM.Address2_Potential,
                            City = entityPM.City_Potential,
                            CountryId = entityPM.CountryId_Potential,
                            StateId = entityPM.StateId_Potential,
                            ZipCode = entityPM.ZipCode_Potential,
                            FaxNumber = entityPM.FaxNumber_Potential,
                            PhoneNumber = entityPM.PhoneNumber_Potential,
                            ATTN = entityPM.ATTN_Potential,
                            SearchFields = entityPM.Address1_Potential + "," + entityPM.Address2_Potential
                        };

                        entityPM.Addresses.Add(address);
                    }
                }
            }
        }

        private void InitializeCardFields()
        {
            if (isNewEntity)
            {
                if (entityPM.CustomerStatusCode == "POT")
                {
                    entityPM.CityName = entityPM.City_Potential;
                    entityPM.CountryId = entityPM.CountryId_Potential;
                    entityCard.Address1 = entityPM.Address1_Potential;
                    entityCard.Address2 = entityPM.Address2_Potential;
                    entityCard.Phone = entityPM.PhoneNumber;
                    entityCard.ZipCode = entityPM.ZipCode_Potential;

                    if (entityPM.CountryId_Potential != null)
                    {
                        Country country = CountryRepository.GetSingleCountry(entityPM.CountryId_Potential, tenant, true);
                        if (country != null)
                        {
                            entityPM.CountryCode = country.Code;
                            entityPM.CountryName = country.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPM.StateId_Potential))
                    {
                        StateRepository stateRepository = new StateRepository(objectContext);
                        State state = stateRepository.GetSingleState(entityPM.StateId_Potential, entityPM.Tenant);
                        if (state != null)
                        {
                            entityCard.StateName = state.EnglishName;
                        }
                    }
                }

                else
                {
                    if (entityPM.Addresses != null)
                    {
                        AddressPM myAddress = entityPM.Addresses.Where(a => a.AddressTypeId == "M").FirstOrDefault();

                        if (myAddress != null)
                        {
                            entityPM.CityName = myAddress.City;
                            entityPM.CountryId = myAddress.CountryId;
                            entityCard.Address1 = myAddress.Address1;
                            entityCard.Address2 = myAddress.Address2;
                            entityCard.Phone = myAddress.PhoneNumber;
                            entityCard.ZipCode = myAddress.ZipCode;

                            if (myAddress.CountryId != null)
                            {
                                Country country = CountryRepository.GetSingleCountry(myAddress.CountryId, tenant, true);
                                if (country != null)
                                {
                                    entityPM.CountryCode = country.Code;
                                    entityPM.CountryName = country.EnglishName;
                                }
                            }

                            if (!string.IsNullOrEmpty(myAddress.StateId))
                            {
                                StateRepository stateRepository = new StateRepository(objectContext);
                                State state = stateRepository.GetSingleState(myAddress.StateId, entityPM.Tenant);
                                if (state != null)
                                {
                                    entityCard.StateName = state.EnglishName;
                                }
                            }
                        }
                    }
                }

                entityCard.CityName = entityPM.CityName;
                entityCard.CountryId = entityPM.CountryId;
                entityCard.CountryCode = entityPM.CountryCode;
                entityCard.CountryName = entityPM.CountryName;
            }

            else
            {
                entityPM.CityName = entityCard.CityName;
                entityPM.CountryId = entityCard.CountryId;
                entityPM.CountryCode = entityCard.CountryCode;
                entityPM.CountryName = entityCard.CountryName;
            }
        }

        private void ComputeContactFields()
        {
            if (!string.IsNullOrEmpty(entityPM.PrimaryContactId))
            {
                ContactRepository contactRepository = new ContactRepository(objectContext);
                Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                if (contact != null)
                {
                    entityPM.PrimaryContactName = contact.EnglishName;
                    entityPM.PrimaryContactEmail = contact.Email;
                    entityPM.PrimaryContactPhone = contact.BusinessPhone;
                }
            }
        }

        private void MapAddressCustomer(AddressPM address, CustomerPM customer)
        {
            address.Address1 = customer.Address1_Potential;
            address.Address2 = customer.Address2_Potential;
            address.ZipCode = customer.ZipCode_Potential;
            address.City = customer.City_Potential;
            address.ATTN = customer.ATTN_Potential;
            address.CountryId = customer.CountryId_Potential;
            address.StateId = customer.StateId_Potential;
            address.PhoneNumber = customer.PhoneNumber_Potential;
            address.FaxNumber = customer.FaxNumber_Potential;
        }

        private void UpdateCardExternalCodeByCurrencyCollection()
        {
            if (cardExternalCodeByCurrencyChangeSet != null)
            {
                foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrencyChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCardExternalCodeByCurrency(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void SetCustomerStatus()
        {
            if (entityPM.SetActivated)
            {
                entityPM.CustomerStatusCode = "ACT";
                entityPM.CustomerStatusName = "Active";
                entityPM.PartnerTypeId = "CS";
                entityPM.ActivationDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ActivatedByUserId = loggedContact.Id;
            }

            else if (entityPM.SetReady)
            {
                entityPM.CustomerStatusCode = "WAC";
                entityPM.CustomerStatusName = "Waiting for Activation";
                entityPM.ReadyForActivationDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ActivationRequestDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ActivationRequestedByUserId = loggedContact.Id;

                CustomerEmailAlert customerEmailAlert = new CustomerEmailAlert();
                customerEmailAlert.SendEmailAlert(entityPM, entityPM.Tenant, "GCAC", false);
            }

            else if (entityPM.SetInActive)
            {
                entityPM.BeforeDeactiveStatusCode = entityPM.CustomerStatusCode;
                entityPM.CustomerStatusCode = "INA";
                entityPM.CustomerStatusName = "Inactive";
                entityPM.InactiveDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.SetAsInactiveByUserId = loggedContact.Id;
                entityPM.InActive = true;
            }

            else if (entityPM.SetReActivated)
            {
                entityPM.CustomerStatusCode = entityPM.BeforeDeactiveStatusCode;
                CustomerStatusRepository rep = new CustomerStatusRepository(tenant);
                CustomerStatus status = rep.GetSingleCustomerStatus(entityPM.CustomerStatusCode);
                entityPM.CustomerStatusName = status.Name;
                entityPM.ActivationDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.ActivatedByUserId = loggedContact.Id;
                entityPM.InActive = false;
            }

            else if (entityPM.SetAsPotential)
            {
                entityPM.PartnerTypeId = "PO";
                entityPM.CustomerStatusCode = "POT";
                entityPM.CustomerStatusName = "Potential";
                entityPM.LastShipmentDate = null;
                entityPM.FirstShipmentDate = null;
            }
        }

        private void UpdateCustomerMediatorByProductCollection()
        {
            if (customerMediatorByProductChangeSet != null)
            {
                foreach (CustomerMediatorByProductPM itemPM in customerMediatorByProductChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerMediatorByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerMediatorByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerMediatorByProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdateForwarderCollection()
        {
            if (customerForwarderByProductChangeSet != null)
            {
                foreach (CustomerForwarderByProductPM itemPM in customerForwarderByProductChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerForwarderByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerForwarderByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerForwarderByProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdateCustomerAccountManagerCollection()
        {
            if (customerAccountManagerByProductChangeSet != null)
            {
                foreach (CustomerAccountManagerByProductPM itemPM in customerAccountManagerByProductChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerAccountManagerByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerAccountManagerByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerAccountManagerByProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdateCustomerCustomsAgentCollection()
        {
            if (customerCustomsAgentByProductChangeSet != null)
            {
                foreach (CustomerCustomsAgentByProductPM itemPM in customerCustomsAgentByProductChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerCustomsAgentByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerCustomsAgentByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerCustomsAgentByProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdateCustomerSalesmanByProductCollection()
        {
            if (customerSalesmanByProductsChangeSet != null)
            {
                foreach (CustomerSalesmanByProductPM itemPM in customerSalesmanByProductsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerSalesmanByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerSalesmanByProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerSalesmanByProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdateProductsCollection()
        {
            if (productsChangeSet != null)
            {
                foreach (CustomerProductPM itemPM in productsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerProduct(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerProduct(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateServicesCollection()
        {
            if (servicesChangeSet != null)
            {
                foreach (CustomerAdditionalServicePM itemPM in servicesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerAdditionalService(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerAdditionalService(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerAdditionalService(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateSalesNotesCollection()
        {
            if (salesNotesChangeSet != null)
            {
                foreach (CustomerSalesNotePM itemPM in salesNotesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerSalesNote(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerSalesNote(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerSalesNote(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateCompetitorsCollection()
        {
            if (productsChangeSet != null)
            {
                foreach (CustomerCompetitorPM itemPM in competitorsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerCompetitor(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerCompetitor(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerCompetitor(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateProductItemsCollection()
        {
            if (productItemsChangeSet != null)
            {
                foreach (ProductItemPM itemPM in productItemsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerProductItem(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerProductItem(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerProductItem(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateCustomerProduct(CustomerProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerProduct itemPoco = new CustomerProduct()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant
            };

            CustomerProductMapping.MapEntity(itemPM, itemPoco, true);
            productRepository.Add(itemPoco);

            if (itemPM.ProductLocations != null)
            {
                foreach (CustomerProductLocationPM locationPM in itemPM.ProductLocations)
                {
                    this.CreateCustomerProductLocation(locationPM);
                }
            }

            string myBodyText = "Potential details created: " + itemPM.ProductTypeName;
            string myEntityDescription = entityPM.Notes;
            AutomaticPosting.CreatePost(itemPM.CustomerId, "Customer", loggedContact.Id, myEntityDescription, myBodyText, true, itemPM.Tenant);
        }
        private void UpdateCustomerProduct(CustomerProductPM itemPM)
        {
            CustomerProduct itemPoco = productRepository.GetSingleCustomerProduct(itemPM.CustomerId, itemPM.ProductTypeCode, tenant);
            CustomerProductMapping.MapEntity(itemPM, itemPoco, false);

            if (itemPM.LocationsChangeSet != null)
            {
                foreach (CustomerProductLocationPM locationPM in itemPM.LocationsChangeSet)
                {
                    switch (locationPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerProductLocation(locationPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerProductLocation(locationPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerProductLocation(locationPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }

            productRepository.Update(itemPoco);

            string myBodyText = "Potential details updated: " + itemPM.ProductTypeName;
            string myEntityDescription = entityPM.Notes;
            AutomaticPosting.CreatePost(itemPM.CustomerId, "Customer", loggedContact.Id, myEntityDescription, myBodyText, true, itemPM.Tenant);
        }
        private void DeleteCustomerProduct(CustomerProductPM itemPM)
        {
            CustomerProduct itemPoco = productRepository.GetSingleCustomerProduct(itemPM.CustomerId, itemPM.ProductTypeCode, tenant);

            if (itemPoco != null)
            {
                List<CustomerProductLocation> locations = productLocationRepository.GetCustomerProductLocations(itemPM.CustomerId, itemPM.ProductTypeCode, tenant).ToList();

                foreach (CustomerProductLocation location in locations)
                {
                    productLocationRepository.Remove(location);
                }

                productRepository.Remove(itemPoco);

                string myBodyText = "Potential details deleted: " + itemPM.ProductTypeName;
                string myEntityDescription = entityPM.Notes;
                AutomaticPosting.CreatePost(itemPM.CustomerId, "Customer", loggedContact.Id, myEntityDescription, myBodyText, true, itemPM.Tenant);
            }
        }

        private void CreateCustomerSalesNote(CustomerSalesNotePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CustomerSalesNote", tenant).ToString();
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.CreatedByUserId = loggedContact.Id;
            itemPM.UpdatedByUserId = loggedContact.Id;
            itemPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            itemPM.UpdateDate = itemPM.CreateDate;

            CustomerSalesNote itemPoco = new CustomerSalesNote()
            {
                Id = itemPM.Id,
                CustomerId = itemPM.CustomerId,
                Tenant = tenant,
            };

            CustomerSalesNoteMapping.MapEntity(itemPM, itemPoco, true);
            salesNoteRepository.Add(itemPoco);

            if (itemPM.PostToFollowers)
            {

                string myBodyText = "Sales Notes updated: " + Environment.NewLine + itemPM.Notes;
                AutomaticPosting.CreatePost(entityPM.Id, "Customer", itemPM.CreatedByUserId, entityPM.EnglishName, myBodyText, true, itemPM.Tenant);
                itemPM.PostToFollowers = false;
            }
        }
        private void UpdateCustomerSalesNote(CustomerSalesNotePM itemPM)
        {
            itemPM.UpdatedByUserId = loggedContact.Id;
            itemPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            CustomerSalesNote itemPoco = salesNoteRepository.GetSingleCustomerSalesNote(itemPM.Id, tenant);
            CustomerSalesNoteMapping.MapEntity(itemPM, itemPoco, false);
        }
        private void DeleteCustomerSalesNote(CustomerSalesNotePM itemPM)
        {
            CustomerSalesNote itemPoco = salesNoteRepository.GetSingleCustomerSalesNote(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                salesNoteRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerCompetitor(CustomerCompetitorPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerCompetitor itemPoco = new CustomerCompetitor()
            {
                CustomerId = itemPM.CustomerId,
                CompetitorId = itemPM.CompetitorId,
                Tenant = tenant
            };

            CustomerCompetitorMapping.MapEntity(itemPM, itemPoco, true);
            competitorsRepository.Add(itemPoco);

            if (itemPM.CustomerCompetitorProducts != null)
            {
                foreach (CustomerCompetitorProductPM product in itemPM.CustomerCompetitorProducts)
                {
                    this.CreateCustomerCompetitorProduct(product);
                }
            }
        }
        private void UpdateCustomerCompetitor(CustomerCompetitorPM itemPM)
        {
            CustomerCompetitor itemPoco = competitorsRepository.GetSingleCustomerCompetitor(itemPM.CustomerId, itemPM.CompetitorId, tenant);
            CustomerCompetitorMapping.MapEntity(itemPM, itemPoco, false);

            if (itemPM.CustomerCompetitorProducts != null)
            {
                foreach (CustomerCompetitorProductPM product in itemPM.CustomerCompetitorProducts)
                {
                    switch (product.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerCompetitorProduct(product);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerCompetitorProduct(product);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerCompetitorProduct(product);
                                break;
                            }

                        default: { break; }
                    }
                }
            }

            competitorsRepository.Update(itemPoco);
        }
        private void DeleteCustomerCompetitor(CustomerCompetitorPM itemPM)
        {
            CustomerCompetitor itemPoco = competitorsRepository.GetSingleCustomerCompetitor(itemPM.CustomerId, itemPM.CompetitorId, tenant);

            if (itemPoco != null)
            {
                List<CustomerCompetitorProduct> products = competitorProductsRepository.GetCustomerCompetitorProducts(itemPM.CustomerId, itemPM.CompetitorId, tenant).ToList();

                foreach (CustomerCompetitorProduct product in products)
                {
                    competitorProductsRepository.Remove(product);
                }

                competitorsRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerProductLocation(CustomerProductLocationPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerProductLocation itemPoco = new CustomerProductLocation()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant
            };

            CustomerProductLocationMapping.MapEntity(itemPM, itemPoco, true);
            productLocationRepository.Add(itemPoco);
        }
        private void UpdateCustomerProductLocation(CustomerProductLocationPM itemPM)
        {
            CustomerProductLocation itemPoco = productLocationRepository.GetSingleCustomerProductLocation(itemPM.CustomerId, itemPM.ProductTypeCode, itemPM.CountryId, tenant);

            if (itemPoco != null)
            {
                CustomerProductLocationMapping.MapEntity(itemPM, itemPoco, false);
                productLocationRepository.Update(itemPoco);
            }
        }
        private void DeleteCustomerProductLocation(CustomerProductLocationPM itemPM)
        {
            CustomerProductLocation itemPoco = productLocationRepository.GetSingleCustomerProductLocation(itemPM.CustomerId, itemPM.ProductTypeCode, itemPM.CountryId, tenant);

            if (itemPoco != null)
            {
                productLocationRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerCompetitorProduct(CustomerCompetitorProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerCompetitorProduct itemPoco = new CustomerCompetitorProduct()
            {
                CustomerId = itemPM.CustomerId,
                CompetitorId = itemPM.CompetitorId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant
            };

            CustomerCompetitorProductMapping.MapEntity(itemPM, itemPoco, true);
            competitorProductsRepository.Add(itemPoco);
        }
        private void UpdateCustomerCompetitorProduct(CustomerCompetitorProductPM itemPM)
        {
            CustomerCompetitorProduct itemPoco = competitorProductsRepository.GetSingleCustomerCompetitorProduct(itemPM.CustomerId, itemPM.CompetitorId, itemPM.ProductTypeCode, tenant);

            if (itemPoco != null)
            {
                CustomerCompetitorProductMapping.MapEntity(itemPM, itemPoco, false);
                competitorProductsRepository.Update(itemPoco);
            }
        }
        private void DeleteCustomerCompetitorProduct(CustomerCompetitorProductPM itemPM)
        {
            CustomerCompetitorProduct itemPoco = competitorProductsRepository.GetSingleCustomerCompetitorProduct(itemPM.CustomerId, itemPM.CompetitorId, itemPM.ProductTypeCode, tenant);

            if (itemPoco != null)
            {
                competitorProductsRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerAdditionalService(CustomerAdditionalServicePM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerAdditionalService itemPoco = new CustomerAdditionalService()
            {
                CustomerId = itemPM.CustomerId,
                AdditionalServiceId = itemPM.AdditionalServiceId,
                Tenant = tenant
            };

            CustomerAdditionalServiceMapping.MapEntity(itemPM, itemPoco, true);
            servicesRepository.Add(itemPoco);
        }
        private void UpdateCustomerAdditionalService(CustomerAdditionalServicePM itemPM)
        {
            CustomerAdditionalService itemPoco = servicesRepository.GetSingleAdditionalService(itemPM.CustomerId, itemPM.AdditionalServiceId, tenant);

            if (itemPoco != null)
            {
                CustomerAdditionalServiceMapping.MapEntity(itemPM, itemPoco, false);
                servicesRepository.Update(itemPoco);
            }
        }
        private void DeleteCustomerAdditionalService(CustomerAdditionalServicePM itemPM)
        {
            CustomerAdditionalService itemPoco = servicesRepository.GetSingleAdditionalService(itemPM.CustomerId, itemPM.AdditionalServiceId, tenant);

            if (itemPoco != null)
            {
                servicesRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerSalesmanByProduct(CustomerSalesmanByProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerSalesmanByProduct itemPoco = new CustomerSalesmanByProduct()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant,
                SalesmanUserId = itemPM.SalesmanUserId,
            };

            CustomerSalesmanByProductMapping.MapEntity(itemPM, itemPoco, true);
            customerSalesmanByProductRepository.Add(itemPoco);
        }
        private void UpdateCustomerSalesmanByProduct(CustomerSalesmanByProductPM itemPM)
        {
            CustomerSalesmanByProduct itemPoco = customerSalesmanByProductRepository.GetSingleCustomerSalesmanByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            CustomerSalesmanByProductMapping.MapEntity(itemPM, itemPoco, false);

            customerSalesmanByProductRepository.Update(itemPoco);
        }
        private void DeleteCustomerSalesmanByProduct(CustomerSalesmanByProductPM itemPM)
        {
            CustomerSalesmanByProduct itemPoco = customerSalesmanByProductRepository.GetSingleCustomerSalesmanByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            if (itemPoco != null)
            {
                customerSalesmanByProductRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerAccountManagerByProduct(CustomerAccountManagerByProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerAccountManagerByProduct itemPoco = new CustomerAccountManagerByProduct()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant,
                AccountManagerId = itemPM.AccountManagerId,
            };

            CustomerAccountManagerByProductMapping.MapEntity(itemPM, itemPoco, true);
            customerAccountManagerByProductRepository.Add(itemPoco);
        }
        private void UpdateCustomerAccountManagerByProduct(CustomerAccountManagerByProductPM itemPM)
        {
            CustomerAccountManagerByProduct itemPoco = customerAccountManagerByProductRepository.GetSingleCustomerAccountManagerByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            CustomerAccountManagerByProductMapping.MapEntity(itemPM, itemPoco, false);

            customerAccountManagerByProductRepository.Update(itemPoco);
        }
        private void DeleteCustomerAccountManagerByProduct(CustomerAccountManagerByProductPM itemPM)
        {
            CustomerAccountManagerByProduct itemPoco = customerAccountManagerByProductRepository.GetSingleCustomerAccountManagerByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            if (itemPoco != null)
            {
                customerAccountManagerByProductRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerCustomsAgentByProduct(CustomerCustomsAgentByProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerCustomsAgentByProduct itemPoco = new CustomerCustomsAgentByProduct()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant,
                CustomsAgentId = itemPM.CustomsAgentId,
            };

            CustomerCustomsAgentByProductMapping.MapEntity(itemPM, itemPoco, true);
            customerCustomsAgentByProductRepository.Add(itemPoco);
        }
        private void UpdateCustomerCustomsAgentByProduct(CustomerCustomsAgentByProductPM itemPM)
        {
            CustomerCustomsAgentByProduct itemPoco = customerCustomsAgentByProductRepository.GetSingleCustomerCustomsAgentByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            CustomerCustomsAgentByProductMapping.MapEntity(itemPM, itemPoco, false);

            customerCustomsAgentByProductRepository.Update(itemPoco);
        }
        private void DeleteCustomerCustomsAgentByProduct(CustomerCustomsAgentByProductPM itemPM)
        {
            CustomerCustomsAgentByProduct itemPoco = customerCustomsAgentByProductRepository.GetSingleCustomerCustomsAgentByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            if (itemPoco != null)
            {
                customerCustomsAgentByProductRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerForwarderByProduct(CustomerForwarderByProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerForwarderByProduct itemPoco = new CustomerForwarderByProduct()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant,
                ForwarderId = itemPM.ForwarderId,
            };

            CustomerForwarderByProductMapping.MapEntity(itemPM, itemPoco, true);
            customerForwarderByProductRepository.Add(itemPoco);
        }
        private void UpdateCustomerForwarderByProduct(CustomerForwarderByProductPM itemPM)
        {
            CustomerForwarderByProduct itemPoco = customerForwarderByProductRepository.GetSingleCustomerForwarderByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            CustomerForwarderByProductMapping.MapEntity(itemPM, itemPoco, false);

            customerForwarderByProductRepository.Update(itemPoco);
        }
        private void DeleteCustomerForwarderByProduct(CustomerForwarderByProductPM itemPM)
        {
            CustomerForwarderByProduct itemPoco = customerForwarderByProductRepository.GetSingleCustomerForwarderByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            if (itemPoco != null)
            {
                customerForwarderByProductRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerMediatorByProduct(CustomerMediatorByProductPM itemPM)
        {
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            CustomerMediatorByProduct itemPoco = new CustomerMediatorByProduct()
            {
                CustomerId = itemPM.CustomerId,
                ProductTypeCode = itemPM.ProductTypeCode,
                Tenant = tenant,
                MediatorId = itemPM.MediatorId,
            };

            CustomerMediatorByProductMapping.MapEntity(itemPM, itemPoco, true);
            customerMediatorByProductRepository.Add(itemPoco);
        }
        private void UpdateCustomerMediatorByProduct(CustomerMediatorByProductPM itemPM)
        {
            CustomerMediatorByProduct itemPoco = customerMediatorByProductRepository.GetSingleCustomerMediatorByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            CustomerMediatorByProductMapping.MapEntity(itemPM, itemPoco, false);

            customerMediatorByProductRepository.Update(itemPoco);
        }
        private void DeleteCustomerMediatorByProduct(CustomerMediatorByProductPM itemPM)
        {
            CustomerMediatorByProduct itemPoco = customerMediatorByProductRepository.GetSingleCustomerMediatorByProduct(itemPM.ProductTypeCode, itemPM.CustomerId, tenant);
            if (itemPoco != null)
            {
                customerMediatorByProductRepository.Remove(itemPoco);
            }
        }

        private void CreateCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CardExternalCodeByCurrency", tenant).ToString();
            itemPM.CardId = this.entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.CurrencyId = itemPM.CurrencyId;
            itemPM.ExternalRecievableTableId = itemPM.ExternalRecievableTableId;
            itemPM.ExternalPayableTableId = itemPM.ExternalPayableTableId;
            CardExternalCodeByCurrency itemPoco = new CardExternalCodeByCurrency()
            {
                Id = itemPM.Id,
                CardId = itemPM.CardId,
                CurrencyId = itemPM.CurrencyId,
                ExternalRecievableTableId = itemPM.ExternalRecievableTableId,
                ExternalPayableTableId = itemPM.ExternalPayableTableId,
                Tenant = tenant,
            };

            CardExternalCodeByCurrencyMapping.MapEntity(itemPM, itemPoco, true);
            cardExternalCodeByCurrencyRepository.Add(itemPoco);
        }
        private void UpdateCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            CardExternalCodeByCurrency itemPoco = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrency(itemPM.Id, tenant);
            CardExternalCodeByCurrencyMapping.MapEntity(itemPM, itemPoco, false);

            cardExternalCodeByCurrencyRepository.Update(itemPoco);
        }
        private void DeleteCardExternalCodeByCurrency(CardExternalCodeByCurrencyPM itemPM)
        {
            CardExternalCodeByCurrency itemPoco = cardExternalCodeByCurrencyRepository.GetSingleCardExternalCodeByCurrency(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                cardExternalCodeByCurrencyRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerProductItem(ProductItemPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ProductItem", tenant);
            itemPM.CustomerId = this.entityPM.Id;
            itemPM.Tenant = tenant;

            ProductItem itemPoco = new ProductItem()
            {
                CustomerId = itemPM.CustomerId,
                Tenant = tenant
            };

            ProductItemMapping.MapEntity(itemPM, itemPoco, true);
            productItemRepository.Add(itemPoco);

            if (itemPM.HTSCodes != null)
            {
                foreach (HTSCodePM hTSCodePM in itemPM.HTSCodes)
                {
                    this.CreateCustomerProductItemHTSCode(hTSCodePM, itemPM.Id);
                }
            }
        }
        private void UpdateCustomerProductItem(ProductItemPM itemPM)
        {
            ProductItem itemPoco = productItemRepository.GetSingleProductItem(itemPM.Id, tenant);
            ProductItemMapping.MapEntity(itemPM, itemPoco, false);

            if (itemPM.HTSCodeChangeSet != null)
            {
                foreach (HTSCodePM hTSCodePM in itemPM.HTSCodeChangeSet)
                {
                    switch (hTSCodePM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCustomerProductItemHTSCode(hTSCodePM, itemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCustomerProductItemHTSCode(hTSCodePM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCustomerProductItemHTSCode(hTSCodePM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }

            productItemRepository.Update(itemPoco);
        }
        private void DeleteCustomerProductItem(ProductItemPM itemPM)
        {
            ProductItem itemPoco = productItemRepository.GetSingleProductItem(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                List<HTSCode> hTSCodes = hTSCodeRepository.GetHTSCodes(itemPM.Id,tenant).ToList();

                foreach (HTSCode hTSCode in hTSCodes)
                {
                    hTSCodeRepository.Remove(hTSCode);
                }

                productItemRepository.Remove(itemPoco);
            }
        }

        private void CreateCustomerProductItemHTSCode(HTSCodePM itemPM,string itemId)
        {
            itemPM.Id = IdCounter.GetNumber("HTSCode", tenant);
            itemPM.ItemId = itemId;
            itemPM.Tenant = tenant;

            HTSCode itemPoco = new HTSCode()
            {
                ItemId = itemPM.ItemId,
                Tenant = tenant
            };

            HTSCodeMapping.MapEntity(itemPM, itemPoco, true);
            hTSCodeRepository.Add(itemPoco);
        }
        private void UpdateCustomerProductItemHTSCode(HTSCodePM itemPM)
        {
            HTSCode itemPoco = hTSCodeRepository.GetSingleHTSCode(itemPM.Id,tenant);

            if (itemPoco != null)
            {
                HTSCodeMapping.MapEntity(itemPM, itemPoco, false);
                hTSCodeRepository.Update(itemPoco);
            }
        }
        private void DeleteCustomerProductItemHTSCode(HTSCodePM itemPM)
        {
            HTSCode itemPoco = hTSCodeRepository.GetSingleHTSCode(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                hTSCodeRepository.Remove(itemPoco);
            }
        }

        private void CreateAddress(AddressPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("Address", tenant).ToString();
            itemPM.CardId = this.entityPOCO.Id;

            Address newAddress = new Address()
            {
                Id = itemPM.Id,
                CardId = itemPM.CardId,
                Tenant = tenant
            };

            AddressMapping.MapEntity(itemPM, newAddress, isNewEntity);
            addressRepository.Add(newAddress);
        }
        private void CreateContact(ContactPM itemContactPM)
        {
            if (itemContactPM.Email != null)
            {
                itemContactPM.Email = itemContactPM.Email.ToLower();
            }

            itemContactPM.CardId = this.entityPM.Id;
            itemContactPM.CompanyName = this.entityPM.EnglishName;

            if (itemContactPM.IsCreatedWithPartner)
            {
                if (!string.IsNullOrEmpty(itemContactPM.Email))
                {
                    itemContactPM.Id = entityPM.ExistedContactId;
                }

                if (string.IsNullOrEmpty(itemContactPM.Id))
                {
                    #region
                    itemContactPM.Id = IdCounter.GetNumber("Contact", tenant).ToString();

                    Contact newContact = new Contact()
                    {
                        Id = itemContactPM.Id,
                        Tenant = tenant,
                        UserType = "R",
                    };

                    Random rnd = new Random();
                    newContact.IndexColor = rnd.Next(1, 20);

                    ContactMapping.MapEntity(itemContactPM, newContact, isNewEntity);
                    contactRepository.Add(newContact);

                    ContactTenant newContactTenant = new ContactTenant()
                    {
                        Id = IdCounter.GetNumber("ContactTenant", tenant).ToString(),
                        TenantId = entityPM.Tenant,
                        ContactId = itemContactPM.Id,
                    };

                    ContactTenantRepository contactTenantRepository = new ContactTenantRepository(objectContext);
                    contactTenantRepository.Add(newContactTenant);

                    if (!entityPM.IsHybrid)
                    {
                        ContactTracing.Trace(itemContactPM, newContact, isNewEntity);
                    }

                    #region global db region
                    if (!string.IsNullOrEmpty(itemContactPM.Email))
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IGlobalContext globalContext = GlobalContext.GetContext();

                            GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);

                            bool globalContactExists = (from a in globalContactRep.GetGlobalContactByTenant(tenant)
                                                        where a.Email == itemContactPM.Email
                                                        select a).Any();
                            if (!globalContactExists)
                            {

                                GlobalContact conflictcontact = globalContactRep.GetSingleGlobalContact(entityPM.Id);
                                if (conflictcontact != null)
                                {
                                    globalContactRep.Remove(conflictcontact);
                                }

                                GlobalContact gcontact = new GlobalContact() { Email = itemContactPM.Email, Id = itemContactPM.Id, GlobalTenantId = tenant, };

                                globalContactRep.Add(gcontact);
                                globalContactRep.SubmitChanges();
                                scope.Complete();
                            }
                        }
                    }
                    #endregion

                    #region if has customer id
                    if (!string.IsNullOrEmpty(itemContactPM.CustomerId))
                    {
                        CardContact cardContact = new CardContact()
                        {
                            CardId = itemContactPM.CustomerId,
                            ContactId = itemContactPM.Id,
                            Id = IdCounter.GetNumber("CardContact", entityPM.Tenant).ToString(),
                            Tenant = tenant,
                        };

                        cardContactRepository.Add(cardContact);
                    }
                    #endregion

                    #endregion
                }

                if (isNewEntity)
                {
                    if (itemContactPM.SetAsPrimaryForCard)
                    {
                        entityPM.PrimaryContactId = itemContactPM.Id;
                        entityPM.PrimaryContactPhone = itemContactPM.BusinessPhone;
                        entityPM.PrimaryContactName = itemContactPM.EnglishName;
                    }
                }

                CardContact newCardContact = new CardContact()
                {
                    CardId = entityPM.Id,
                    ContactId = itemContactPM.Id,
                    Id = IdCounter.GetNumber("CardContact", tenant).ToString(),
                    Tenant = tenant
                };

                cardContactRepository.Add(newCardContact);
            }
        }
        private void UpdateContactSearchField(ContactPM itemContactPM)
        {
            if (itemContactPM != null)
            {
                if (itemContactPM.Id != null)
                {
                    string myResult = null;

                    IQueryable<Card> allCards = objectContext.CardContacts.Where(d => d.ContactId == itemContactPM.Id).Select(s => s.Card);

                    if (allCards.Count() > 0)
                    {
                        List<string> names = (from d in allCards where d.EnglishName != null group d by d.EnglishName into g select g.Key).ToList();
                        foreach (string name in names)
                        {
                            if (myResult == null)
                            {
                                myResult = name;
                            }

                            else
                            {
                                myResult += "," + name;
                            }
                        }
                    }

                    if (myResult != null)
                    {
                        if (myResult.Length > 1000)
                        {
                            myResult = myResult.Substring(0, 1000);
                        }
                    }


                    Contact myContact = contactRepository.GetSingleContact(itemContactPM.Id, itemContactPM.Tenant);
                    if (myContact != null)
                    {
                        myContact.CompanyName = myResult;
                        itemContactPM.CompanyName = myResult;

                        ContactMapping.BuildSearchFields(itemContactPM, myContact);

                        contactRepository.Update(myContact);
                        contactRepository.SubmitChanges();
                    }
                }
            }
        }

        public void Submit()
        {
            contactRepository.SubmitChanges();
            cardRepository.SubmitChanges();
            cardContactRepository.SubmitChanges();
            entityRepository.SubmitChanges();
            addressRepository.SubmitChanges();
            customerSalesmanByProductRepository.SubmitChanges();
            customerAccountManagerByProductRepository.SubmitChanges();
            customerCustomsAgentByProductRepository.SubmitChanges();
            customerForwarderByProductRepository.SubmitChanges();
            customerMediatorByProductRepository.SubmitChanges();
            cardExternalCodeByCurrencyRepository.SubmitChanges();
        }
        //ITZIK 
        public static void CreateNew(ICommonDataContext objectContext, CustomerPM entityPM, string LoggingUserId)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CustomerService service = new CustomerService(objectContext, entityPM);
            service.OverrideLoggingUserId = LoggingUserId;
            service.Create();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Customer");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");
        }

        public static bool DoesCustomerVatNumberExist(string code, int tenant)
        {
            CustomerRepository customerRepository = new CustomerRepository(tenant);
            return (customerRepository.GetCustomers(tenant).Where(d => d.Card.VatNumber == code && d.Tenant == tenant)).Any();
        }

        public static void UpdateByContext(ICommonDataContext objectContext, CustomerPM entityPM, string LoggingUserId) // moran 3.11.16 - call 273521
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            CustomerService service = new CustomerService(objectContext, entityPM);
            service.SetChangeSet(new List<CustomerSalesNotePM>(), new List<CustomerProductPM>(), new List<CustomerCompetitorPM>(), new List<CustomerAdditionalServicePM>(), new List<CustomerSalesmanByProductPM>(), new List<CustomerAccountManagerByProductPM>(), new List<CustomerCustomsAgentByProductPM>(), new List<CustomerForwarderByProductPM>(), new List<CustomerMediatorByProductPM>(), new List<CardExternalCodeByCurrencyPM>(), new List<ProductItemPM>());
            service.OverrideLoggingUserId = LoggingUserId;
            service.Update();

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Customer");
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");
        }
    }
}

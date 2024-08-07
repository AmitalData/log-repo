using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityOtherServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.ExternalService;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.BL.Workfkow;
using Logitude.BL.Workfkow.Constants;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.CToolWorkflows;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.BL.Security;
using Logitude.BL.AnalyticTableServices;
using System.Data.SqlClient;
using Logitude.BL.Workflow;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System.Net.Http;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ShipmentService
    {
        private int tenant;
        private bool isNewEntity;
        private bool calculateProfit;
        private bool calculatePayables;
        private bool calculateReceivables;
        private bool isEntityStatusUpdated;
        public Shipment entityPoco { get; set; }
        private ShipmentPM entityPM;
        private ShipmentMasterData entityMasterData;
        private IShipmentsContext objectContext;
        private ICommonDataContext myCommonContext;
        private ShipmentRepository entityRepository;
        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        private ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository;
        private ShipmentPickUpDeliveryPackageRepository shipmentPickUpDeliveryPackageRepository;
        private FollowUpRepository followUpRepository;
        private ShipmentOrderPackageRepository shipmentOrderPackageRepository;
        private ShipmentPackageRepository shipmentPackageRepository;
        private InsideShipmentPackageRepository insideShipmentPackageRepository;
        private ShipmentAWBPrintOnlyRepository shipmentAWBPrintOnlyRepository;
        private ShipmentReceivableRepository shipmentReceivableRepository;
        private ShipmentPayableRepository shipmentPayableRepository;
        private ShipmentPackageItemRepository shipmentPackageItemRepository;
        private ShipmentPackageHarmonizeRepository shipmentPackageHarmonizeRepository;
        private PickUpDeliveryPackageHarmonizeRepository pickUpDeliveryPackageHarmonizeRepository;
        private ShipmentCarrierStatusRepository shipmentCarrierStatusRepository;
        private AWBOCIRepository aWBOCIRepository;
        private ShipmentCommodityRepository shipmentCommodityRepository;
        private TenantSettingQuery tenantSettingQuery;
        private CardRepository cardRepository;
        private AddressRepository myAddressRepository;
        private PortRepository myPortRepository;
        private ShipmentAdditionalCloudDataRepository shipmentAdditionalCloudDataRepository;
        private ShipmentAdditionalCloudData shipmentAdditionalCloudData;
        private ShipmentTracing shipmentTracing;
        private Tenant loggedTenant;
        private ContactPM loggedContact;
        private ShipmentAssemblyRepository shipmentAssemblyRepository;
        private ShipmentReferanceRepository shipmentReferanceRepository;
        private ShipmentStoragePricingRepository shipmentStoragePricingRepository;
        private ShipmentProductItemRepository shipmentProductItemRepository;
        private ShipmentUnassignedFieldRepository shipmentUnassignedFieldRepository;
        private PayableProratedAmountRepository payableProratedAmountRepository;
        List<ShipmentPM> housesList = new List<ShipmentPM>();
        private ShipmentBehaviourFacade shipmentBehaviourFacade;
        private ShipmentContainerStatusRepository shipmentContainerStatusRepository;
        HybridPartnerPM CurrentHybridPartner;
        public ShipmentComputedFields UpdatedShipmentComputedFields;
        public ShipmentDigitalField shipmentDigitalFields;
        private ShipmentServiceInitializer initializer;
        string UpdateByEmail;
        private ComputingPartnerRepository computingPartnerRepository;
        private ComputingPartnerTableRepository computingPartnerTableRepository;
        private ComputingPartnerTranslationRepository computingPartnerTranslationRepository;
        public ShipmentDocsField ShipmentDocsFieldFromWorkerRole;
        public bool isFromEventTrace;

        private List<FieldChange> FieldChanges = new List<FieldChange>();
        private AuditLogRepository AuditLogRepository;

        public ShipmentService(int tenant)
        {
            FieldChanges = new List<FieldChange>();

            this.objectContext = ShipmentsContext.GetContext(tenant);
            this.myCommonContext = CommonDataContext.GetContext(tenant);
            this.myAddressRepository = new AddressRepository(this.myCommonContext);
        }
        public ShipmentService(IShipmentsContext objectContext, ShipmentPM entityPM, string serviceContextUser)
        {
            FieldChanges = new List<FieldChange>();
            AuditLogRepository = new AuditLogRepository(tenant);

            UpdateByEmail = serviceContextUser;
            this.initializer = new ShipmentServiceInitializer(objectContext, entityPM, serviceContextUser);
            this.initializer.Initialize();
            this.tenant = initializer.Tenant;
            this.entityPM = initializer.EntityPM;
            this.entityPoco = initializer.EntityPOCO;
            this.loggedTenant = initializer.LoggedTenant;
            this.loggedContact = initializer.LoggedContact;
            this.entityRepository = initializer.Repository;
            this.objectContext = initializer.ShipmentContext;
            this.myCommonContext = initializer.CommonContext;
            this.entityMasterData = initializer.EntityMasterData;
            this.shipmentMasterDataRepository = initializer.MasterDataRepository;
            this.shipmentPackageRepository = initializer.ShipmentPackageRepository;
            this.shipmentContainerStatusRepository = initializer.ShipmentContainerStatusRepository;
            this.insideShipmentPackageRepository = initializer.InsideShipmentPackageRepository;
            this.shipmentPackageItemRepository = initializer.ShipmentPackageItemRepository;
            this.shipmentPackageHarmonizeRepository = initializer.ShipmentPackageHarmonizeRepository;
            this.shipmentOrderPackageRepository = initializer.ShipmentOrderPackageRepository;

            this.shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(objectContext);
            this.shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(objectContext);
            this.shipmentAWBPrintOnlyRepository = new ShipmentAWBPrintOnlyRepository(objectContext);
            this.shipmentReceivableRepository = new ShipmentReceivableRepository(objectContext);
            this.shipmentPayableRepository = new ShipmentPayableRepository(objectContext);
            this.pickUpDeliveryPackageHarmonizeRepository = new PickUpDeliveryPackageHarmonizeRepository(objectContext);
            this.shipmentCarrierStatusRepository = new ShipmentCarrierStatusRepository(objectContext);
            this.followUpRepository = new FollowUpRepository(tenant);
            this.cardRepository = new CardRepository(myCommonContext);
            this.myAddressRepository = new AddressRepository(myCommonContext);
            this.myPortRepository = new PortRepository(myCommonContext);
            this.aWBOCIRepository = new AWBOCIRepository(objectContext);
            this.shipmentCommodityRepository = new ShipmentCommodityRepository(objectContext);
            this.shipmentAdditionalCloudDataRepository = new ShipmentAdditionalCloudDataRepository(objectContext);
            this.shipmentAssemblyRepository = new ShipmentAssemblyRepository(objectContext);
            this.shipmentStoragePricingRepository = new ShipmentStoragePricingRepository(objectContext);
            this.shipmentProductItemRepository = new ShipmentProductItemRepository(objectContext);
            this.shipmentUnassignedFieldRepository = new ShipmentUnassignedFieldRepository(objectContext);
            this.payableProratedAmountRepository = new PayableProratedAmountRepository(objectContext);
            this.shipmentReferanceRepository = new ShipmentReferanceRepository(objectContext);

            this.computingPartnerRepository = new ComputingPartnerRepository(myCommonContext);
            this.computingPartnerTableRepository = new ComputingPartnerTableRepository(myCommonContext);
            this.computingPartnerTranslationRepository = new ComputingPartnerTranslationRepository(myCommonContext);
            this.SetHybridPartner(this.tenant);
        }

        private void SetHybridPartner(int myTenant)
        {
            HybridPartnerQuery HybridPartnerQuery = new HybridPartnerQuery(myTenant);
            CurrentHybridPartner = HybridPartnerQuery.GetSinglePMByPartnerTenant(myTenant);
        }


        public void SetChangeSet(List<ShipmentPackagePM> shipmentPackagesChangeSet, List<ShipmentOrderPackagePM> shipmentOrderPackagesChangeSet, List<ShipmentPickUpPM> shipmentPickUpsChangeSet, List<ShipmentDeliveryPM> shipmentDeliveriesChangeSet, List<ShipmentReceivablePM> shipmentReceivablesChangeSet, List<ShipmentPayablePM> shipmentPayablesChangeSet, List<ShipmentFollowUpPM> shipmentFollowUpsChangeSet, List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnliesChangeSet, List<ConsoleShipmentPM> shipmentConsoleShipmentsChangeSet, List<ShipmentCarrierStatusPM> shipmentCarrierStatusesChangeSet, List<AWBOCIPM> aWBOCIPMChangeSet, List<ShipmentCommodityPM> shipmentCommoditiesChangeSet, List<ShipmentAssemblyPM> shipmentAssembliesChangeSet, List<ShipmentStoragePricingPM> shipmentStoragePricingsChangeSet, List<ShipmentProductItemPM> shipmentProductItemsChangeSet, List<ShipmentUnassignedFieldPM> shipmentUnassignedFieldChangeSet)
        {
            // this was for the old silverlight system
            this.initializer.ShipmentPackagesChangeSet = shipmentPackagesChangeSet;
            this.initializer.ShipmentOrderPackagesChangeSet = shipmentOrderPackagesChangeSet;
            this.initializer.ShipmentPickUpsChangeSet = shipmentPickUpsChangeSet;
            this.initializer.ShipmentDeliveriesChangeSet = shipmentDeliveriesChangeSet;
            this.initializer.ShipmentReceivablesChangeSet = shipmentReceivablesChangeSet;
            this.initializer.ShipmentPayablesChangeSet = shipmentPayablesChangeSet;
            this.initializer.ShipmentFollowUpsChangeSet = shipmentFollowUpsChangeSet;
            this.initializer.ShipmentAWBPrintOnliesChangeSet = shipmentAWBPrintOnliesChangeSet;
            this.initializer.ShipmentConsoleShipmentsChangeSet = shipmentConsoleShipmentsChangeSet;
            this.initializer.ShipmentCarrierStatusesChangeSet = shipmentCarrierStatusesChangeSet;
            this.initializer.AWBOCIPMChangeSet = aWBOCIPMChangeSet;
            this.initializer.ShipmentCommoditiesChangeSet = shipmentCommoditiesChangeSet;
            this.initializer.ShipmentAssembliesChangeSet = shipmentAssembliesChangeSet;
            this.initializer.ShipmentStoragePricingsChangeSet = shipmentStoragePricingsChangeSet;
            this.initializer.ShipmentProductItemsChangeSet = shipmentProductItemsChangeSet;
            this.initializer.ShipmentUnassignedFieldChangeSet = shipmentUnassignedFieldChangeSet;
        }

        public void Create()
        {
            if (entityPM.IsCustomShipment)
            {
                this.CreateCustomShipment();
                return;
            }

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                //used when create master from house, to check if house already connected to another master
                Shipment houseShipment = null;
                if (!string.IsNullOrEmpty(entityPM.MasterCreatedFromHouseId))
                {
                    houseShipment = entityRepository.GetSingleShipment(entityPM.MasterCreatedFromHouseId, tenant);
                    if (houseShipment != null && !string.IsNullOrEmpty(houseShipment.MasterShipmentDataId))
                    {
                        throw new ApplicationException("House shipment already connected to a Master, in order to connect to another please disconnect it first");
                    }
                }

                this.isNewEntity = true;
                this.calculateProfit = false;
                this.calculatePayables = false;
                this.calculateReceivables = false;

                this.initializer.HandleBehaviours();

                this.entityMasterData = this.initializer.EntityMasterData;

                this.InitializeComponent();

                if (!loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                {
                    this.initializer.HandleValidators();

                    ShipmentValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext, loggedTenant);
                    ShipmentValidating.ValidateFutureRoutingDates(entityPM, entityPM.ShipmentPickUps, entityPM.ShipmentDeliveries);
                }

                foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
                {
                    this.CreateShipmentPackage(itemPM);
                }

                foreach (ShipmentPickUpPM itemPM in entityPM.ShipmentPickUps)
                {
                    this.CreateShipmentPickUp(itemPM);
                }

                foreach (ShipmentDeliveryPM itemPM in entityPM.ShipmentDeliveries)
                {
                    this.CreateShipmentDelivery(itemPM);
                }

                foreach (ShipmentFollowUpPM itemPM in entityPM.FollowUps)
                {
                    this.CreateShipmentFollowUp(itemPM);
                }

                foreach (ShipmentPayablePM itemPM in entityPM.ShipmentPayables)
                {
                    this.CreateShipmentPayable(itemPM);
                }

                foreach (ShipmentReceivablePM itemPM in entityPM.ShipmentReceivables)
                {
                    this.CreateShipmentReceivable(itemPM);
                }

                foreach (ShipmentAWBPrintOnlyPM itemPM in entityPM.ShipmentAWBPrintOnlies)
                {
                    this.CreateShipmentAWBPrintOnly(itemPM);
                }

                foreach (ShipmentCarrierStatusPM itemPM in entityPM.ShipmentCarrierStatuses)
                {
                    this.CreateShipmentCarrierStatus(itemPM);
                }

                foreach (AWBOCIPM itemPM in entityPM.AWBOCIPMs)
                {
                    this.CreateAWBOCI(itemPM);
                }

                foreach (ShipmentCommodityPM itemPM in entityPM.ShipmentCommodities)
                {
                    this.CreateShipmentCommodity(itemPM);
                }

                foreach (ShipmentAssemblyPM itemPM in entityPM.ShipmentAssemblies)
                {
                    this.CreateShipmentAssembly(itemPM);
                }

                foreach (ShipmentStoragePricingPM itemPM in entityPM.ShipmentStoragePricings)
                {
                    this.CreateShipmentStoragePricing(itemPM);
                }

                foreach (ShipmentProductItemPM itemPM in entityPM.ShipmentProductItems)
                {
                    this.CreateShipmentProductItem(itemPM);
                }
                foreach (ShipmentUnassignedFieldPM itemPM in entityPM.ShipmentUnassignedFields)
                {
                    this.CreateShipmentUnassignedField(itemPM);
                }

                this.initializer.HandleComposition();
                this.initializer.HandleStandalone();

                entityPM.CalculateProfit = calculateProfit;
                entityPM.CalculatePayables = calculatePayables;
                entityPM.CalculateReceivables = calculateReceivables;

                if (!entityPM.IsHybrid && !loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                {
                    shipmentTracing.BeginTracing();
                }
                this.ComputeIsAssemblyField();
                this.ComputeFinalDestination();
                this.ComputeIsHTSMissingField();
                this.ComputeHasUnassignedField();
                this.SaveChildEntitiesCustomFields();

                RunAutomation("OnCreate");

                ShipmentMapping.MapEntity(entityPM, entityPoco, entityMasterData, isNewEntity, entityPM.ShipmentPackages, objectContext, FieldChanges);
                this.ComputeAgentComputed(entityPM, entityPoco);
                this.ComputeETAAndETDHouseFields();


                entityRepository.Add(entityPoco);
                entityRepository.SubmitChanges();

                entityPM.IsConnectToMasterShipment = entityMasterData != null ? true : false;
                shipmentBehaviourFacade = new ShipmentBehaviourFacade(entityPM, objectContext, UpdatedShipmentComputedFields, isNewEntity);
                shipmentBehaviourFacade.Handle(FieldChanges);
                shipmentBehaviourFacade.HandleShipmentDigitalFields(shipmentDigitalFields);

                shipmentBehaviourFacade.Save(); // Abed to make automation change to condation work fine

                new ShipmentAnalyticTableService(objectContext.GetActiveDbContext()).AddUpdate(entityPoco, tenant);

                if (!string.IsNullOrEmpty(entityPM.MasterCreatedFromHouseId))
                {
                    if (houseShipment != null)
                    {
                        houseShipment.MasterShipmentDataId = entityPM.Id;
                        houseShipment.ComputedShipmentNumber = entityPM.ShipmentNumber;
                        houseShipment.AgentComputed = houseShipment.AgentId == null ? entityPM.AgentId : houseShipment.AgentId;

                        entityRepository.Update(houseShipment);
                        entityRepository.SubmitChanges();

                        this.RunRegistryDateProcedure(houseShipment.Id);
                        this.RunFirstApprovalDateProcedure(houseShipment.Id);

                        calculateProfit = true;
                        calculatePayables = true;
                        calculateReceivables = true;
                    }
                }

                foreach (ConsoleShipmentPM itemPM in entityPM.ShipmentConsoleShipments)
                {
                    itemPM.MasterShipmentDataId = this.entityPM.Id;
                    this.CreateConsoleShipment(itemPM);
                }

                shipmentAdditionalCloudDataRepository.SubmitChanges();
                followUpRepository.SubmitChanges();
                shipmentPickUpDeliveryRepository.SubmitChanges();

                this.InitializeBookingData();

                GetForeignFields();
                BuildActivityLog();
                BuildImportersQueue();
                RunStoredProcedures();
                BuildAgentSharedManifest();

                SendAutomaticallyOceanOnsightsRequest();
                if (UpdateByEmail != "system@tenant" + entityPM.Tenant + ".com" &&
                    this.entityPM != null && !this.entityPM.FromCTool)
                {
                    EntityChangesMessageProducer.ProduceShipmentCreateMessage(entityPoco, entityPM);
                }

                RunAutomationThatDependencyOnLastEntityUpdate();

                AuditLog auditLog = null;
                if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
                {
                    auditLog = AddShipmentAuditLogChanges(entityPoco);
                    AuditLogRepository.Add(auditLog);
                    AuditLogRepository.SubmitChanges();
                }

                new WorkflowEntityQueueMessage()
                {
                    Entity = WorkflowEntities.Shipment,
                    EntityId = entityPM.Id,
                    AuditLogId = auditLog?.Id,
                    Tenant = entityPM.Tenant,
                    Type = QueueMessagesTypes.Create,
                    IsCustom = false
                }.Produce();

                new TaskDoneQueueMessage()
                {
                    Entity = WorkflowEntities.Shipment,
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    Type = QueueMessagesTypes.Create
                }.Produce();

                scope.Complete();

            }			
		}

		public void CreateCustomShipment()
		{
			using (TransactionScope scope = TransactionFactory.GetTransaction())
			{
				this.isNewEntity = true;

				this.InitializeComponent();

				ShipmentValidating.ValidateCustomShipment(entityPM, true);

				// generate shipment number
				this.entityPM.ShipmentNumber = TableCounter.GetNumber(tenant, "SHIP", entityPM.DirectionId, entityPM.TransportModeId);

				// todo: add AIR to AddShipmentTypes and execute it
				entityPM.ShipmentTypeId = null;

				// todo: get default values
				this.entityPM.SalesmanUserId = "1-421340";
				this.entityPM.ReferantUserId = "1-421335";

				this.entityPM.CreatedByUserId = loggedContact.Id;
				this.entityPM.UpdatedByUserId = loggedContact.Id;

				ShipmentMapping.MapEntity(entityPM, entityPoco, entityMasterData, isNewEntity, entityPM.ShipmentPackages, objectContext, FieldChanges);

				entityRepository.Add(entityPoco);
				entityRepository.SubmitChanges();

				entityPM.IsConnectToMasterShipment = entityMasterData != null ? true : false;
				shipmentBehaviourFacade = new ShipmentBehaviourFacade(entityPM, objectContext, UpdatedShipmentComputedFields, isNewEntity);
				shipmentBehaviourFacade.Handle(FieldChanges);
				shipmentBehaviourFacade.HandleShipmentDigitalFields(shipmentDigitalFields);
				shipmentBehaviourFacade.Save(); // Abed to make automation change to condation work fine

				AuditLog auditLog = null;
				if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
				{
					auditLog = AddShipmentAuditLogChanges(entityPoco);
					AuditLogRepository.Add(auditLog);
					AuditLogRepository.SubmitChanges();
				}

				new WorkflowEntityQueueMessage()
				{
					Entity = WorkflowEntities.Shipment,
					EntityId = entityPM.Id,
					AuditLogId = auditLog?.Id,
					Tenant = entityPM.Tenant,
					Type = QueueMessagesTypes.Create,
					IsCustom = false
				}.Produce();

				new TaskDoneQueueMessage()
				{
					Entity = WorkflowEntities.Shipment,
					EntityId = entityPM.Id,
					Tenant = entityPM.Tenant,
					Type = QueueMessagesTypes.Create
				}.Produce();

				CreateDeclaration(entityPM, "NEW");

				scope.Complete();
			}
		}
		public  void CreateDeclaration(ShipmentPM shipmentPM, string mode)
		{
			int tenant = shipmentPM.Tenant;
			var myAmitalCustom = new LogitudeCustomsFile();
			CustomsSettingRepository customsSettingQueryService = new CustomsSettingRepository(tenant);
			var customsSettingPM = customsSettingQueryService.GetSettingByTenant(tenant);
			
			myAmitalCustom.CustomFileNo = shipmentPM.ShipmentNumber;
            myAmitalCustom.DeclarationOfficeCode = shipmentPM.DeclarationOfficeCode;
			myAmitalCustom.CustomerId = shipmentPM.CustomerId;
            myAmitalCustom.AgentId = customsSettingPM?.CustomsAgentId;
            myAmitalCustom.CreatedByUserId = shipmentPM.CreatedByUserId;
			myAmitalCustom.DepartmentId = shipmentPM.DepartmentId;
			myAmitalCustom.TransportModeId = shipmentPM.TransportModeId;
            myAmitalCustom.ReferentUserId = shipmentPM.ReferantUserId;
			myAmitalCustom.SystemConnection = "N";
            //DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
            var objDefult = "";//defaultValueQueryService.GetDefault("ISRAEL", "CGG_PAYHAND_FIL", "NON", "NON", tenant);//ביטול הזרמת ח.פ להצהרה
			if (!string.IsNullOrEmpty(objDefult)) {
			   CardRepository cardRep = new CardRepository(tenant);
			   Card card = cardRep.GetSingleCard(shipmentPM.CustomerId, tenant);
               if (card != null && !string.IsNullOrEmpty(card.VatNumber))
               {
                   if (card.VatNumber != null) { 
                      myAmitalCustom.ImporterId = card.Id;                     
                   }
			   }
            }
			myAmitalCustom.Direction = "I";
			myAmitalCustom.Mode = mode;
			myAmitalCustom.Tenant = shipmentPM.Tenant.ToString();
            
			var respnse = APIConnectionHelper.Instance.PostViaWebAPI<Response, LogitudeCustomsFile>("/api/Declarartion/UpdateDeclarationInU2L", myAmitalCustom);
		}
				
		private void InsertInShipmnetUpdateLog(int StartOrEnd, string errorMessage = null)
        {
            string mySubError = errorMessage;
            if (errorMessage != null && errorMessage.Length > 4000)
            {
                mySubError = errorMessage.Substring(0, 3999);
            }
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    string strConnString = entityRepository.context.GetConnection().ConnectionString;
                    string query = "INSERT INTO ShipmentUpdateLog (MessageID, EntityID, Tenant,LogDateTime,StartOrEnd,ErrorMessage) " +
                                        "VALUES (@MessageID, @EntityID, @Tenant, @LogDateTime,@StartOrEnd,@ErrorMessage) ";

                    using (SqlConnection cn = new SqlConnection(strConnString))
                    {
                        SqlCommand cmd = new SqlCommand(query, cn);

                        cmd.Parameters.Add("@MessageID", SqlDbType.Int).Value = DbQueueService.MessageID == null ? (object)DBNull.Value : DbQueueService.MessageID;
                        cmd.Parameters.Add("@EntityId", SqlDbType.VarChar, 50).Value = entityPM.Id.ToString();
                        cmd.Parameters.Add("@Tenant", SqlDbType.Int).Value = tenant;
                        cmd.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@StartOrEnd", SqlDbType.VarChar, 50).Value = StartOrEnd;
                        if (errorMessage == null)
                        {
                            cmd.Parameters.AddWithValue("@ErrorMessage", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ErrorMessage", mySubError);
                        }
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 5;
                        cn.Open();
                        var output = cmd.ExecuteNonQuery();
                        cn.Close();
                    }

                }
                catch (Exception ex)
                {
                }
                scope.Complete();
            }
        }
        private void AddVIRExternalTaskQueue()
        {
            if (entityPM.IsHybrid && entityPM.ExternalStatuses == "VIR")
            {
                ExternalTasksQueueService externalTasksQueueService = new ExternalTasksQueueService(entityPM.Tenant, "User ID Link Received");
                externalTasksQueueService.AddVIRExternalTaskQueue(entityPM);
            }
        }

        string CustomerChanged = "false";
        public void Update(bool mapComposition = false, bool isFromUpdateTool = false, bool isPatchUpdate = false)
        {
            if (entityPM.IsCustomShipment)
            {
                if (!entityPM.IsHybrid && !loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                {
                    shipmentTracing = new ShipmentTracing(entityPM, entityPoco, entityMasterData, loggedContact.Id, isNewEntity);
                    shipmentTracing.BeginTracing();
                }
                this.UpdateCustomShipment();
                return;
            }

            //bool isPatchUpdate = false;
            InsertInShipmnetUpdateLog(0);
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    initializer.IsMappingComposition = mapComposition;
                    initializer.IsUpdateFromUpdateTool = isFromUpdateTool;

                    #region
                    this.isNewEntity = false;
                    this.calculateProfit = false;
                    this.calculatePayables = false;
                    this.calculateReceivables = false;
                    Shipment shipmentPocoCopy = null;
                    ShipmentPM shipmentPMCopy = null;

                    if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
                    {

                        #region

                    string myOldCustomerId = "";
                        string oldEntityStatusId = entityPoco.StatusId;
                        if (entityPM.CustomerId != entityPoco.CustomerId)
                        {
                            myOldCustomerId = entityPoco.CustomerId;
                            CustomerChanged = "true";
                        }
                        this.OldCustomerId = myOldCustomerId;

                        this.initializer.HandleBehaviours();

                        this.entityMasterData = this.initializer.EntityMasterData;

                        this.InitializeComponent();

                        if (!loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                        {
                            this.initializer.HandleValidators();

                            ShipmentValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext, loggedTenant, isPatchUpdate);
                            ShipmentValidating.ValidateFutureRoutingDates(entityPM, initializer.ShipmentPickUpsChangeSet, initializer.ShipmentDeliveriesChangeSet);
                        }

                        if (entityPM.ShipmentDirectionConverted && entityPM.ShipmentConvertedNewNumber)
                        {
                            this.ChangePickupDliveryNumbersOnShipmentDirectionConverted();
                        }

                        this.UpdateShipmentProductItems();
                        this.ComputeIsHTSMissingField();
                        this.UpdateShipmentPackagesCollection();
                        this.UpdateShipmentPickUpsCollection();
                        this.UpdateShipmentDeliveriesCollection();
                        this.UpdateShipmentPayablesCollection();
                        this.UpdateShipmentReceivablesCollection();
                        this.UpdateShipmentAWBPrintOnliesCollection();
                        this.UpdateShipmentConsoleShipmentsCollection();
                        this.UpdateShipmentFollowUpsCollection("InSert");
                        this.UpdateShipmentCarrierStatusesCollection();
                        this.UpdateShipmentAWBOCIsCollection();
                        this.UpdateShipmentCommoditiesCollection();
                        this.UpdateShipmentAssembliesCollection();
                        this.UpdateShipmentStoragePricingsCollection();
                        this.UpdateShipmentProductItemsCollection();
                        this.UpdateShipmentUnassignedFieldsCollection();
                        this.initializer.HandleComposition();
                        this.initializer.HandleStandalone();
                        this.InitializeBookingData();
                        this.RemoveDeletedItemsFromEntityPM();

                        if (entityPM.WarehouseStorageFreeDays != entityPoco.WarehouseStorageFreeDays)
                        {
                            calculatePayables = true;
                            calculateReceivables = true;
                        }

                        if (initializer.IsUpdatingProfitFromConversion)
                        {
                            entityPM.CalculateProfit = true;
                            entityPM.CalculatePayables = true;
                            entityPM.CalculateReceivables = true;
                        }

                        else
                        {
                            entityPM.CalculateProfit = calculateProfit;
                            entityPM.CalculatePayables = calculatePayables;
                            entityPM.CalculateReceivables = calculateReceivables;
                        }

                        if (!entityPM.IsHybrid && !loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                        {
                            shipmentTracing.BeginTracing(isFromEventTrace);
                            isEntityStatusUpdated = oldEntityStatusId != entityPM.StatusId ? true : false;
                        }

                        if(!entityPM.ShipmentUpdatedFromContainer)
                            ShipmentContainersEntityBehaviour.UpdateConatinarStatus(this.entityPM, isEntityStatusUpdated, objectContext);

                        if (string.IsNullOrEmpty(entityPM.CustomFileId) && !string.IsNullOrEmpty(entityPoco.CustomFileId))
                        {
                            entityPM.CustomFilePocoId = entityPoco.CustomFileId;
                        }

                        List<ShipmentPackagePM> myPackagesList = new List<ShipmentPackagePM>();
                        if (initializer.ShipmentPackagesChangeSet != null)
                        {
                            myPackagesList = initializer.ShipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                        }

                        this.BuildShipmentExternalUpdate();
                        this.ComputeIsAssemblyField();
                        this.ComputeFinalDestination();
                        this.CheckUpdatingMasterHouses();
                        this.ComputeIsHTSMissingField();
                        this.ComputeHasUnassignedField();
                        this.ComputeNumberOfTransshipments();

                        entityPM.IsConnectToMasterShipment = entityMasterData != null ? true : false;
                        shipmentBehaviourFacade = new ShipmentBehaviourFacade(entityPM, objectContext, UpdatedShipmentComputedFields, isNewEntity);
                        shipmentBehaviourFacade.Handle(FieldChanges);
                        shipmentBehaviourFacade.HandleShipmentDigitalFields(shipmentDigitalFields);

                        if (ShipmentDocsFieldFromWorkerRole != null)
                        {
                            shipmentBehaviourFacade.HandleShipmentDocsFields(ShipmentDocsFieldFromWorkerRole);                        
                        }

                        if (shipmentBehaviourFacade.ReceivablePricingUpdated_CrossDoc)
                        {
                            ShipmentReceivablePM storageReceivable = entityPM.ShipmentReceivables.Where(d => d.ChargesTypeCode == "ISTOR" && d.MeasurementCode == "STFE" && string.IsNullOrEmpty(d.ARInvoiceId)).FirstOrDefault();
                            if (storageReceivable != null)
                            {
                                if (storageReceivable.ChangeSetOp == ChangeSetOperation.Insert)
                                {
                                    this.CreateShipmentReceivable(storageReceivable);
                                }

                                else if (storageReceivable.ChangeSetOp == ChangeSetOperation.Update)
                                {
                                    this.UpdateShipmentReceivable(storageReceivable);
                                }

                                else if (storageReceivable.ChangeSetOp == ChangeSetOperation.Delete)
                                {
                                    this.DeleteShipmentReceivable(storageReceivable);
                                }
                            }

                            foreach (ShipmentStoragePricingPM pricingPM in entityPM.ShipmentStoragePricings.Where(d => d.ChangeSetOp == ChangeSetOperation.Update))
                            {
                                this.UpdateShipmentStoragePricing(pricingPM);
                            }
                        }

                        if (shipmentBehaviourFacade.DatesUpdated_CrossDoc)
                        {
                            shipmentTracing.TraceTerminalData();
                        }
                        this.SaveChildEntitiesCustomFields();


                        RunAutomation("OnUpdate", BuildShipmentChangeTracking());

                        shipmentBehaviourFacade.Save(); // Abed to make automation change to condation work fine
                        this.UpdateShipmentFollowUpsCollection();
                        UpdateStandaloneShipments();

                        shipmentPocoCopy = CloneObjectService.Clone(entityPoco);
                        shipmentPMCopy = CloneObjectService.Clone(entityPM);
                        ShipmentMapping.MapEntity(entityPM, entityPoco, entityMasterData, isNewEntity, myPackagesList, objectContext, FieldChanges);
                        this.ComputeAgentComputed(entityPM, entityPoco);

                        entityRepository.Update(entityPoco);
                        ////
                        entityRepository.SubmitChanges();

                        shipmentAdditionalCloudDataRepository.SubmitChanges();
                        followUpRepository.SubmitChanges();
                        shipmentPickUpDeliveryRepository.SubmitChanges();
                        new ShipmentAnalyticTableService(objectContext.GetActiveDbContext()).AddUpdate(entityPoco, tenant);

                        if (entityPM.IsStandalonePickupDelivery)
                        {
                            this.CopyForwarderShipmentPackagesFromStandalone();
                        }

                        UpdateMasterHouses();
                        RunStoredProcedures();
                        GetForeignFields();
                        BuildActivityLog();
                        BuildImportersQueue();
                        SendAutomaticallyOceanOnsightsRequest();
                        UpdatePayablesLinesVatAmounts();
                        RemoveDeletedPackagesItemsFromEntityPM();
                        RunAutomationThatDependencyOnLastEntityUpdate();
                        #endregion
                    }

                    else
                    {
                        #region
                        // cancelled shipments
                        // in case follow ups added
                        // from client
                        int indexComp = 0;
                        if (initializer.ShipmentFollowUpsChangeSet != null)
                        {
                            foreach (ShipmentFollowUpPM item in initializer.ShipmentFollowUpsChangeSet)
                            {
                                if (string.IsNullOrEmpty(item.Id))
                                {
                                    indexComp++;
                                    item.Id = "ShipmentFollowUpPM_" + indexComp;
                                }
                            }
                        }
                        #endregion
                    }

                    if (entityPM.IsCancelled)
                    {
                        List<FollowUp> allFollowupLists = this.followUpRepository.GetFollowUpsByShipmentId(entityPM.Id, entityPM.Tenant);
                        foreach (FollowUp item in allFollowupLists)
                        {
                            followUpRepository.Remove(item);
                        }

                        followUpRepository.SubmitChanges();

                        entityPM.FollowUps = new List<ShipmentFollowUpPM>();
                    }

                    this.RefreshFollowUpDate();

                    this.UpdateExtendedTasksDueDate();

                    if (this.entityPM != null && !this.entityPM.FromCTool)
                    {
                        EntityChangesMessageProducer.ProduceShipmentUpdateMessage(shipmentPocoCopy, shipmentPMCopy);
                    }

                    AuditLog auditLog = null;
                    if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
                    {
                        auditLog = AddShipmentAuditLogChanges(entityPoco);
                        AuditLogRepository.Add(auditLog);
                        AuditLogRepository.SubmitChanges();
                    }

                    new WorkflowEntityQueueMessage()
                    {
                        Entity = WorkflowEntities.Shipment,
                        EntityId = entityPM.Id,
                        AuditLogId = auditLog?.Id,
                        Tenant = entityPM.Tenant,
                        Type = QueueMessagesTypes.Update,
                        IsCustom = false
                    }.Produce();

                    new TaskDoneQueueMessage()
                    {
                        Entity = WorkflowEntities.Shipment,
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        Type = QueueMessagesTypes.Update
                    }.Produce();

                    scope.Complete();

                    #endregion
                }
                InsertInShipmnetUpdateLog(1);
			}
            catch (Exception ex)
            {

                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                }

                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                InsertInShipmnetUpdateLog(-1, errorMessage);
                throw ex;
            }

        }
        public void UpdateCustomShipment()
        {
            InsertInShipmnetUpdateLog(0);
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (entityPM.CustomerId != entityPoco.CustomerId)
                    {
                        // todo: get default values (if value from client is not changed?)
                        this.entityPM.DepartmentId = "1-10140";
                        this.entityPM.SalesmanUserId = "1-421340";
                        this.entityPM.ReferantUserId = "1-421335";
                    }

                    initializer.IsMappingComposition = true;
                    this.initializer.HandleBehaviours();

                    ShipmentValidating.ValidateCustomShipment(entityPM, false);
                    ValidateShipmentReferancesCollection();

                    ShipmentMapping.MapEntity(entityPM, entityPoco, entityMasterData, isNewEntity, new List<ShipmentPackagePM> { }, objectContext, FieldChanges);

                    entityRepository.Update(entityPoco);
                    this.UpdateShipmentPackageCustom();
                    ////
                    entityRepository.SubmitChanges();

                    this.UpdateShipmentReferancesCollection();

                    AuditLog auditLog = null;
                    if (entityPM != null && FeatureToggleHelper.HasFeatureToggle("ADL", entityPM.Tenant))
                    {
                        auditLog = AddShipmentAuditLogChanges(entityPoco);
                        AuditLogRepository.Add(auditLog);
                        AuditLogRepository.SubmitChanges();
                    }

                    new WorkflowEntityQueueMessage()
                    {
                        Entity = WorkflowEntities.Shipment,
                        EntityId = entityPM.Id,
                        AuditLogId = auditLog?.Id,
                        Tenant = entityPM.Tenant,
                        Type = QueueMessagesTypes.Update,
                        IsCustom = false
                    }.Produce();

                    new TaskDoneQueueMessage()
                    {
                        Entity = WorkflowEntities.Shipment,
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        Type = QueueMessagesTypes.Update
                    }.Produce();

                    if(!entityPM.IsHybrid)
					UpdateDeclaration(entityPM, "UPDATE");

					scope.Complete();
                }
                InsertInShipmnetUpdateLog(1);
            }
            catch (Exception ex)
            {

                string errorMessage = ex.Message + Environment.NewLine;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                }

                errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                InsertInShipmnetUpdateLog(-1, errorMessage);
                throw ex;
            }

        }

		public void UpdateDeclaration(ShipmentPM shipmentPM, string mode)
		{
			int tenant = shipmentPM.Tenant;
			ICustomContext customContext = CustomContext.GetContext(tenant);
			IWarehouseContext warehouseContext = WarehouseContext.GetContext(tenant);

			CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
			DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
			WarehouseEntryRepository warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
			DeclarationReferantDataRepository declarationReferantDataRepository = new DeclarationReferantDataRepository(tenant);

		
			string decId = declarationQueryService.GetIdByCustomFileNo(shipmentPM.ShipmentNumber, tenant);
			DeclarationReferantData declarationReferantDataPM = declarationReferantDataRepository.GetSingle(decId, tenant);

			var myAmitalCustom = new LogitudeCustomsFile();

			myAmitalCustom.CustomFileNo = shipmentPM.ShipmentNumber;
			myAmitalCustom.Id = decId;
			myAmitalCustom.TransportModeId = shipmentPM.TransportModeId;
			myAmitalCustom.DepartmentId = shipmentPM.DepartmentId;
			myAmitalCustom.CustomerId = shipmentPM.CustomerId;
			if (shipmentPM.TransportModeId == "A")
			{
				myAmitalCustom.CargoTypeCode = "1";
				myAmitalCustom.SecondCargoID = declarationReferantDataPM != null ? declarationReferantDataPM.CarrierCode + "-" + declarationReferantDataPM.Mawb : null;
				myAmitalCustom.ThirdCargoID = shipmentPM.House;
			}
			if (shipmentPM.TransportModeId == "O")
			{
				myAmitalCustom.CargoTypeCode = "11";
                myAmitalCustom.ManifestNumber = shipmentPM.IskaNumber?.Length >= 7 ? shipmentPM.IskaNumber.Substring(1, 7): shipmentPM.IskaNumber;
				myAmitalCustom.SecondCargoID = shipmentPM.IskaNumber?.Length >= 7 ? shipmentPM.IskaNumber.Substring(7) : "";
            }
			if (shipmentPM.TransportModeId == "L")
			{
				myAmitalCustom.CargoTypeCode = "20";
				myAmitalCustom.ManifestNumber = shipmentPM.IskaNumber;
			}
			myAmitalCustom.UnloadDate = declarationReferantDataPM?.ArrivalDate?.ToString();
			myAmitalCustom.ManifestDate = shipmentPM.HAWBDate != null ? shipmentPM.HAWBDate?.ToString() : declarationReferantDataPM.MawbDate?.ToString();
			myAmitalCustom.PackageTypeCode = declarationReferantDataPM?.PackageTypeCode;
			myAmitalCustom.PackageMeasureQualifierCode = "2";
			myAmitalCustom.PackageQuantity = shipmentPM.NumberOfPackages?.ToString();
			myAmitalCustom.GrossMassMeasure = shipmentPM.GrossWeight?.ToString();
			//myAmitalCustom.GrossMassMeasureTypeCode = "KGM";
			myAmitalCustom.CargoDescription = shipmentPM.DescriptionOfGoods;

			myAmitalCustom.Direction = "I";
			myAmitalCustom.SystemConnection = "N";
			myAmitalCustom.Mode = mode;
			myAmitalCustom.Tenant = shipmentPM.Tenant.ToString();

			#region declaration referant data fields
            myAmitalCustom.MAWB = shipmentPM.Mawb;
			myAmitalCustom.MawbDate = shipmentPM.MawbDate?.ToString();
			myAmitalCustom.EstimatedArrivalDate = shipmentPM.EstimatedArrivalDate?.ToString();
			myAmitalCustom.PackageTypeCode = shipmentPM.PackageTypeCode;
			myAmitalCustom.ArrivalDate = shipmentPM.ArrivalDate?.ToString();
			myAmitalCustom.Commodity = shipmentPM.Commodity;
			myAmitalCustom.Vessel = shipmentPM.Vessel;
			myAmitalCustom.FlightVoyageNumber = shipmentPM.FlightVoyageNumber;
			myAmitalCustom.CarrierCode = shipmentPM.CarrierCode;
			#endregion

			var respnse = APIConnectionHelper.Instance.PostViaWebAPI<Response, LogitudeCustomsFile>("/api/Declarartion/UpdateDeclarationInU2L", myAmitalCustom);
		}
		private AuditLog AddShipmentAuditLogChanges(Shipment entityPoco)
        {
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPoco.Tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Shipment", 0, true);
            AuditLog auditLog = new AuditLog()
            {
                Id = IdCounter.GetNumber("AuditLog", entityPoco.Tenant).ToString(),
                Tenant = entityPoco.Tenant,
                UpdateDate = entityPoco.LastUpdateDate,
                UpdatedByUserId = entityPoco.UpdatedByUserId,
                EntityId = entityPoco.Id,
                ObjectTableId = objecttable.Id,
                ChangesJson = JsonConvert.SerializeObject(FieldChanges)
            };

            return auditLog;
        }

        private void RunAutomationThatDependencyOnLastEntityUpdate()
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            foreach (MainEntityChangeService mainEntityChangeService in mainEntityChangeServices)
            {
                ExecuteAutomationThatDependencyOnLastEntityUpdate(shipmentQuery, mainEntityChangeService);
            }
        }

        private void ExecuteAutomationThatDependencyOnLastEntityUpdate(ShipmentQuery shipmentQuery, MainEntityChangeService mainEntityChangeService)
        {
            if (!mainEntityChangeService.CheckIfUserDefinedAutomationDependencyOnLastEntityUpdate()) return;
            var shipmentPM = GetShipmentPMForDependencyAutomation(shipmentQuery, mainEntityChangeService);
            List<TraceEventPM> shipmentTraceEventPMs = shipmentPM.EventList;
            shipmentQuery.MapEventsListForAPI(shipmentPM);
            mainEntityChangeService.ExecuteAutomationThatDependencyOnLastEntityUpdate(shipmentPM, shipmentPM.ShipmentNumber);
            shipmentPM.EventList = shipmentTraceEventPMs;
        }

        private ShipmentPM GetShipmentPMForDependencyAutomation(ShipmentQuery shipmentQuery, MainEntityChangeService mainEntityChangeService)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("EHA", entityPM.Tenant))
            {
                return !mainEntityChangeService.IsChild ? entityPM : ShipmentMapping.MapShipmentPMToShipmentPMForAutomation(entityPM, mainEntityChangeService.entityChangeArgs.EntityPM as ShipmentPM, new ShipmentPM());
            }

            if (!mainEntityChangeService.IsChild)
            {
                return entityPM;
            }

            ShipmentPM shipmentPM = mainEntityChangeService.entityChangeArgs.EntityPM as ShipmentPM;
            shipmentPM = shipmentQuery.GetSinglePM(shipmentPM.Id, shipmentPM.Tenant);
            ShipmentMapping.MapMasterDetailsForShipment(entityPM, shipmentPM);

            return shipmentPM;
        }

        private void UpdatePayablesLinesVatAmounts()
        {
            if (initializer.ShipmentPayablesChangeSet != null && initializer.ShipmentPayablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.None).Any())
            {
                var allPayablesIds = (from d in entityPM.ShipmentPayables select d.Id).ToList();
                var allPayables = shipmentPayableRepository.GetShipmentPayablesFromIdList(allPayablesIds, tenant);
                PayablesLinesVatAmounts payablesLinesVatAmounts = new PayablesLinesVatAmounts(allPayables, initializer.Tenant, shipmentPayableRepository);
                payablesLinesVatAmounts.UpdateAllPayablesVatAmount();
            }
        }

        private void UpdateStandaloneShipments()
        {
            if (IsUpdatingStandaloneShipments())
            {
                List<Shipment> standaloneShipments = this.GetStandaloneShipments();
                this.RunStandaloneShipmentBehaviour(standaloneShipments);
            }
        }

        private bool IsUpdatingStandaloneShipments()
        {
            if (this.entityPM.IsHybrid)
                return false;
            if (this.entityPM.DirectionId != this.entityPoco.DirectionId)
                return true;
            if (this.entityPM.ShipmentLevelCode != this.entityPoco.ShipmentLevelCode)
                return true;
            if (this.entityPM.ShipmentTypeId != this.entityPoco.ShipmentTypeId)
                return true;

            return false;
        }

        private List<Shipment> GetStandaloneShipments()
        {
            List<Shipment> standaloneShipments = entityRepository.GetStandaloneShipments(entityPM.Id, tenant);
            return standaloneShipments;
        }
        private void RunStandaloneShipmentBehaviour(List<Shipment> standaloneShipments)
        {
            StandaloneShipmentBehaviour standaloneShipmentBehaviour = new StandaloneShipmentBehaviour();
            standaloneShipmentBehaviour.UpdateStandaloneShipmentsOfParentShipment(this.initializer, standaloneShipments);
        }

        private void RemoveDeletedItemsFromEntityPM()
        {
            if (initializer.ShipmentPickUpsChangeSet != null)
            {
                this.entityPM.ShipmentPickUps = initializer.ShipmentPickUpsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }

            if (initializer.ShipmentDeliveriesChangeSet != null)
            {
                this.entityPM.ShipmentDeliveries = initializer.ShipmentDeliveriesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }

            if (initializer.ShipmentPayablesChangeSet != null)
            {
                this.entityPM.ShipmentPayables = initializer.ShipmentPayablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }

            if (initializer.ShipmentReceivablesChangeSet != null)
            {
                this.entityPM.ShipmentReceivables = initializer.ShipmentReceivablesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }

        }

        private void RemoveDeletedPackagesItemsFromEntityPM()
        {
            if (initializer.ShipmentPickUpsChangeSet != null)
            {
                this.RemoveDeletedItemsFromPickUpPackages();
            }

            if (initializer.ShipmentDeliveriesChangeSet != null)
            {
                this.RemoveDeletedItemsFromDeliveryPackages();
            }

            if (initializer.ShipmentPackagesChangeSet != null)
            {
                this.entityPM.ShipmentPackages = initializer.ShipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }
        }

        private void RemoveDeletedItemsFromPickUpPackages()
        {
            foreach (ShipmentPickUpPM shipmentPickUpPM in this.entityPM.ShipmentPickUps)
            {
                this.MapShipmentPickUpPackages(shipmentPickUpPM);
            }
        }
        private void MapShipmentPickUpPackages(ShipmentPickUpPM shipmentPickUpPM)
        {
            if (!(shipmentPickUpPM.ShipmentPickUpPackagesChangeSet != null && shipmentPickUpPM.ShipmentPickUpPackagesChangeSet.Count > 0))
            {
                return;
            }

            shipmentPickUpPM.ShipmentPickUpDeliveryPackages = shipmentPickUpPM.ShipmentPickUpPackagesChangeSet
                                                              .Where(package => package.ChangeSetOp != ChangeSetOperation.Delete).ToList();
        }

        private void RemoveDeletedItemsFromDeliveryPackages()
        {
            foreach (ShipmentDeliveryPM shipmentDeliveryPM in this.entityPM.ShipmentDeliveries)
            {
                MapShipmentDeliveryPackages(shipmentDeliveryPM);
            }
        }

        private void MapShipmentDeliveryPackages(ShipmentDeliveryPM shipmentDeliveryPM)
        {
            if (!(shipmentDeliveryPM.ShipmentDeliveryPackagesChangeSet != null && shipmentDeliveryPM.ShipmentDeliveryPackagesChangeSet.Count > 0))
            {
                return;
            }

            shipmentDeliveryPM.ShipmentPickUpDeliveryPackages = shipmentDeliveryPM.ShipmentDeliveryPackagesChangeSet
                                                              .Where(package => package.ChangeSetOp != ChangeSetOperation.Delete).ToList();
        }

        private void UpdateMasterHouses()
        {
            MasterHousesBehaviour MasterHousesBehaviour = new MasterHousesBehaviour(this.initializer, this.allHouses, this.isNewEntity);
            MasterHousesBehaviour.ApplyUpdatingMasterHouses();
        }

        private void ComputeAgentComputed(ShipmentPM entityPM, Shipment entityPoco)
        {
            if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
            {
                entityPoco.AgentComputed = entityPM.AgentId;
            }
            else if (entityPM.ShipmentLevelCode == "H")
            {
                if (!string.IsNullOrEmpty(entityPM.AgentId))
                {
                    entityPoco.AgentComputed = entityPM.AgentId;
                }
                else
                {
                    if (!string.IsNullOrEmpty(entityPM.MasterShipmentDataId))
                    {
                        ShipmentRepository shipmentrepo = new ShipmentRepository(entityPM.Tenant);
                        entityPoco.AgentComputed = shipmentrepo.GetSingleShipment(entityPM.MasterShipmentDataId, entityPM.Tenant).AgentId;
                    }
                    else
                    {
                        entityPoco.AgentComputed = null;
                    }

                }
            }
        }

        private void RefreshFollowUpDate()
        {
            IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);
            FollowUpRepository followUpsRepository = new FollowUpRepository(myFreightContext);
            List<FollowUp> allFollowupLists = null;
            List<FollowUp> followupLists = null;
            List<string> houseIds = new List<string>();

            if (entityPM.ShipmentLevelCode == "C")
            {
                if (allHouses.Count > 0)
                {
                    foreach (Shipment item in allHouses)
                    {
                        houseIds.Add(item.Id);
                    }
                }
            }

            if (entityPM.IsRefreshShipmentFollowUps)
            {
                allFollowupLists = followUpsRepository.GetFollowUpsByShipmentId(entityPM.Id, entityPM.Tenant);
                followupLists = allFollowupLists.Where(d => !string.IsNullOrEmpty(d.DateFieldName)).ToList();
                if (houseIds.Count > 0)
                {
                    List<FollowUp> houseFollowupLists = followUpsRepository.GetFollowUpsThatHaveDateFileName(entityPM.Tenant).Where(d => houseIds.Contains(d.ShipmentId)).ToList();
                    foreach (FollowUp houseFollowup in houseFollowupLists)
                    {
                        followupLists.Add(houseFollowup);
                    }
                }
            }
            else
            {
                followupLists = followUpsRepository.GetFollowUpsThatHaveDateFileName(entityPM.Tenant).Where(d => houseIds.Contains(d.ShipmentId) || d.ShipmentId == entityPM.Id).ToList();
            }


            if (followupLists.Count > 0)
            {
                bool isAnyOneChange = false;
                foreach (FollowUp follow in followupLists)
                {
                    bool isChange = false;

                    if (!string.IsNullOrEmpty(follow.DateFieldName))
                    {
                        PropertyInfo propInfo = entityPM.GetType().GetProperty(follow.DateFieldName);
                        if (propInfo != null)
                        {
                            object fieldValue = propInfo.GetValue(entityPM);

                            if (fieldValue != null)
                            {
                                DateTime? fieldValuedate = (DateTime?)fieldValue;

                                if (follow.DateEscalationActionTimeIndicatorCode != "IM" && follow.DateEscalationTime != 0)
                                {
                                    int dateEscalationTime = follow.DateEscalationActionTimeIndicatorCode == "AF" ? follow.DateEscalationTime : follow.DateEscalationTime * -1;
                                    fieldValuedate = fieldValuedate.Value.AddDays(dateEscalationTime);

                                }

                                if (follow.Date != fieldValuedate)
                                {
                                    follow.Date = fieldValuedate;

                                    if (!entityPM.IsRefreshShipmentFollowUps)
                                    {
                                        ShipmentFollowUpPM followUpPM = entityPM.FollowUps.Where(d => d.Id == follow.Id).FirstOrDefault();
                                        if (followUpPM != null) followUpPM.Date = follow.Date;
                                    }

                                    isChange = true;
                                    isAnyOneChange = true;
                                }



                            }

                            if (isChange) followUpsRepository.Update(follow);
                        }
                    }
                }
                if (isAnyOneChange) followUpsRepository.SubmitChanges();

            }

            if (entityPM.IsRefreshShipmentFollowUps && allFollowupLists.Count > 0) RefreshShipmentFollowUps(allFollowupLists);

        }

        private void UpdateExtendedTasksDueDate()
        {
            ActivityRepository activityRepository = new ActivityRepository(tenant);
            List<Activity> extendedActivities = activityRepository.GetExtendedActivitiesByShipmentId(entityPM.Id, tenant).ToList();

            bool changeFound = false;
            foreach (Activity item in extendedActivities)
            {
                if (!string.IsNullOrEmpty(item.DueDateDateField))
                {
                    PropertyInfo propInfo = entityPM.GetType().GetProperty(item.DueDateDateField);
                    if (propInfo != null)
                    {
                        object fieldValue = propInfo.GetValue(entityPM);
                        if (fieldValue != null)
                        {
                            DateTime? fieldValuedate = (DateTime?)fieldValue;
                            fieldValuedate = fieldValuedate.Value.AddHours((double)item.DueDateOffset);

                            if (item.DueDate != fieldValuedate)
                            {
                                item.DueDate = fieldValuedate;
                            }
                        }

                        else
                        {
                            item.DueDate = null;
                        }

                        activityRepository.Update(item);

                        ContactRepository contactRepository = new ContactRepository(tenant);
                        Contact contact = contactRepository.GetContactByUserTypeAndTenant("S", tenant);
                        if (contact != null)
                        {
                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "UPAV",
                                UserId = contact.Id,
                                EntityId = item.Id,
                                ObjectTableName = "Activity",
                                Entity = item,

                            });
                        }

                        changeFound = true;
                    }
                }
            }

            if (changeFound)
            {
                activityRepository.SubmitChanges();
            }
        }

        private void RefreshShipmentFollowUps(List<FollowUp> followupList = null)
        {
            entityPM.IsRefreshShipmentFollowUps = false;
            entityPM.IsRefreshFollowUp = true;
            IWebFreightContext myFreightContext = WebFreightContext.GetContext(tenant);
            FollowUpRepository followUpsRepository = new FollowUpRepository(myFreightContext);

            if (entityPM.FollowUps.Count != 0)
            {
                entityPM.FollowUps.Clear();
            }

            if (followupList == null)
            {
                followupList = followUpsRepository.GetFollowUpsByShipmentId(entityPM.Id, entityPM.Tenant);
            }
            foreach (FollowUp follow in followupList)
            {
                ShipmentFollowUpPM followUpPM = new ShipmentFollowUpPM()
                {
                    Tenant = follow.Tenant,
                    Date = follow.Date,
                    Done = follow.Done,
                    DoneDateTime = follow.DoneDateTime,
                    DoneNote = follow.DoneNote,
                    ExternalDocumentId = follow.DocumentsFilingId,
                    Id = follow.Id,
                    InternalDocumentId = follow.InternalDocumentId,
                    IsNew = follow.IsNew,
                    JobId = follow.JobId,
                    LegType = follow.LegType,
                    Notes = follow.Notes,
                    ShipmentId = follow.ShipmentId,
                    EventTypeId = follow.EventTypeId,
                    EventTypeFollowUpName = follow.EventType.FollowUpEnglishName,
                    ManualActivatedFollowUp = follow.EventType.ManualActivatedFollowUp,
                    OwnerUserId = follow.OwnerUserId,
                    OwnerUserName = follow.OwnerUser.Contact.EnglishName,
                    Area = follow.Area,
                    DocumentTypeId = follow.DocumentTypeId,
                    AutomationId = follow.AutomationId,
                    DateEscalationActionTimeIndicatorCode = follow.DateEscalationActionTimeIndicatorCode,
                    DateEscalationTime = follow.DateEscalationTime,
                    DateFieldName = follow.DateFieldName,

                };
                entityPM.FollowUps.Add(followUpPM);
            }
        }

        public void Delete()
        {
            this.entityPoco = entityRepository.GetSingleShipment(entityPM.Id, tenant);
            this.entityMasterData = (entityPM.ShipmentLevelCode == "H") ? shipmentMasterDataRepository.GetSingleMasterData(entityPM.MasterShipmentDataId) : shipmentMasterDataRepository.GetSingleMasterData(entityPoco.MasterShipmentDataId);

            if (entityMasterData != null)
            {
                shipmentMasterDataRepository.Remove(entityMasterData);
            }

            foreach (ConsoleShipmentPM pm in entityPM.ShipmentConsoleShipments)
            {
                this.DeleteConsoleShipment(pm);
            }

            foreach (ShipmentOrderPackagePM pm in entityPM.ShipmentOrderPackages)
            {
                this.DeleteShipmentOrderPackage(pm);
            }

            foreach (ShipmentPackagePM pm in entityPM.ShipmentPackages)
            {
                this.DeleteShipmentPackage(pm);
            }

            foreach (ShipmentPickUpPM pm in entityPM.ShipmentPickUps)
            {
                this.DeleteShipmentPickUp(pm);
            }

            foreach (ShipmentDeliveryPM pm in entityPM.ShipmentDeliveries)
            {
                this.DeleteShipmentDelivery(pm);
            }

            foreach (ShipmentPayablePM pm in entityPM.ShipmentPayables)
            {
                this.DeleteShipmentPayable(pm);
            }

            foreach (ShipmentReceivablePM pm in entityPM.ShipmentReceivables)
            {
                this.DeleteShipmentReceivable(pm);
            }

            foreach (ShipmentAWBPrintOnlyPM pm in entityPM.ShipmentAWBPrintOnlies)
            {
                this.DeleteShipmentAWBPrintOnly(pm);
            }

            foreach (ShipmentFollowUpPM pm in entityPM.FollowUps)
            {
                this.DeleteShipmentFollowUp(pm);
            }

            foreach (ShipmentCarrierStatusPM pm in entityPM.ShipmentCarrierStatuses)
            {
                this.DeleteShipmentCarrierStatus(pm);
            }

            foreach (AWBOCIPM pm in entityPM.AWBOCIPMs)
            {
                this.DeleteAWBOCI(pm);
            }

            foreach (ShipmentCommodityPM item in entityPM.ShipmentCommodities)
            {
                this.DeleteShipmentCommodity(item);
            }

            foreach (ShipmentAssemblyPM pm in entityPM.ShipmentAssemblies)
            {
                this.DeleteShipmentAssembly(pm);
            }

            foreach (ShipmentStoragePricingPM pm in entityPM.ShipmentStoragePricings)
            {
                this.DeleteShipmentStoragePricing(pm);
            }

            foreach (ShipmentProductItemPM pm in entityPM.ShipmentProductItems)
            {
                this.DeleteShipmentProductItem(pm);
            }

            foreach (ShipmentUnassignedFieldPM pm in entityPM.ShipmentUnassignedFields)
            {
                this.DeleteShipmentUnassignedField(pm);
            }

            entityRepository.Remove(this.entityPoco);
            entityRepository.SubmitChanges();
        }

        private void ComputeETAAndETDHouseFields()
        {
            if (entityPM.ShipmentLevelCode == "H" && !String.IsNullOrEmpty(entityPM.MasterShipmentDataId) && isNewEntity)
            {
                var shipment = (from d in objectContext.Shipments
                                where d.Tenant == tenant
                                && d.Id == entityPM.MasterShipmentDataId
                                select d).FirstOrDefault();

                entityPoco.NextETA = shipment.NextETA;
                entityPM.NextETA = shipment.NextETA;
                entityPoco.NextETD = shipment.NextETD;
                entityPM.NextETD = shipment.NextETD;
            }
        }

        private string OldCustomerId;
        private void GetForeignFields()
        {
            string myIncotermCode = null;
            string myIncotermName = null;
            if (!string.IsNullOrEmpty(this.entityPoco.IncotermId))
            {
                IncotermRepository myIncotermRepository = new IncotermRepository(this.myCommonContext);
                Incoterm myIncoterm = myIncotermRepository.GetSingleIncoterm(this.entityPoco.IncotermId, tenant);
                if (myIncoterm != null)
                {
                    myIncotermCode = myIncoterm.Code;
                    myIncotermName = myIncoterm.Name;
                }
            }

            entityPM.IncotermCode = myIncotermCode;
            entityPM.IncotermName = myIncotermName;

            string myShipmentType = "";
            if (entityPoco.ShipmentTypeId != null)
            {
                ShipmentTypeRepository shipmentTypeRepository = new ShipmentTypeRepository(entityPoco.Tenant);
                ShipmentType shipmentType = shipmentTypeRepository.GetSingleShipmentType(entityPoco.ShipmentTypeId);
                if (shipmentType != null)
                {
                    myShipmentType = shipmentType.Name;
                }
            }

            if (entityPoco.ShipmentLevelCode != null)
            {
                ShipmentLevelRepository shipmentLevelRepository = new ShipmentLevelRepository(entityPoco.Tenant);
                ShipmentLevel shipmentLevel = shipmentLevelRepository.GetSingleShipmentLevel(entityPoco.ShipmentLevelCode);
                if (shipmentLevel != null)
                {
                    myShipmentType = string.IsNullOrEmpty(myShipmentType) ? shipmentLevel.Name : myShipmentType + " " + shipmentLevel.Name;
                }
            }

            entityPM.ShipmentType = myShipmentType;
            entityPM.ShipmentTypeViewField = myShipmentType;

            entityPM.FHLStatusName = entityPoco.FHLStatus != null ? entityPoco.FHLStatus.Name : null;
            entityPM.FWBStatusName = entityPoco.ShipmentMasterData != null ? (entityPoco.ShipmentMasterData.FWBStatus != null ? entityPoco.ShipmentMasterData.FWBStatus.Name : null) : null;

            this.GetForeignFields_Status();
        }
        private void GetForeignFields_Status()
        {
            entityPM.StatusName = null;
            entityPM.StatusLocation = null;

            if (entityPoco.ShipmentLevelCode == "H" && entityPoco.MasterShipmentDataId != null)
            {
                var iConsoleStatus = (from d in objectContext.ShipmentMasterDatas
                                      where d.Id == entityPoco.MasterShipmentDataId
                                      select new
                                      {
                                          StatusId = d.StatusId,
                                          StatusLocation = d.StatusLocation,
                                      }).FirstOrDefault();

                if (iConsoleStatus != null)
                {
                    string statusName = null;
                    string statusCode = null;
                    string iHighestStatusId = EntityStatusHelper.GetHighestStatusId(entityPoco.StatusId, iConsoleStatus.StatusId, entityPoco.Tenant, ref statusName, ref statusCode);
                    entityPM.StatusName = statusName;
                    entityPM.StatusCode = statusCode;

                    if (iHighestStatusId == entityPoco.StatusId)
                    {
                        entityPM.StatusLocation = entityPoco.StatusLocation;
                    }

                    else
                    {
                        entityPM.StatusLocation = iConsoleStatus.StatusLocation;
                    }
                }
            }

            else
            {
                if (entityPoco.StatusId != null)
                {
                    EntityStatus iEntityStatus = EntityStatusRepository.GetSingleEntityStatus(entityPoco.StatusId, entityPoco.Tenant, true);
                    if (iEntityStatus != null)
                    {
                        entityPM.StatusName = iEntityStatus.Name;
                        entityPM.StatusLocation = entityPoco.StatusLocation;
                    }
                }
            }
        }

        private void BuildActivityLog()
        {
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(entityPoco.Tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName("Shipment", 0, true);

            if (isNewEntity)
            {
                ActivityLogger.AddAcitivityLog(entityPoco.Id, objecttable.Id, entityPoco.Tenant, "N", entityPoco.UpdatedByUserId);
            }

            else
            {
                if (initializer.LoggedContactEmail.Contains("system@tenant"))
                {
                    EntityLastUpdateRepository entityLastUpdateRepository = new EntityLastUpdateRepository(entityPoco.Tenant);
                    EntityLastUpdate update = entityLastUpdateRepository.GetSingleEntityLastUpdateByEntityId(entityPoco.Id);
                    if (update != null)
                    {
                        update.EntityGUID = entityPoco.ConcurrencyGUID;
                        entityLastUpdateRepository.Update(update);
                    }

                    else
                    {
                        update = new EntityLastUpdate()
                        {
                            EntityGUID = entityPoco.ConcurrencyGUID,
                            Tenant = entityPoco.Tenant,
                            ObjectTableId = objecttable.Id,
                            EntityId = entityPoco.Id,
                            UpdateDate = entityPoco.LastUpdateDate,
                            UpdatedByUserId = entityPoco.UpdatedByUserId,
                            Id = IdCounter.GetNumber("Shipment", entityPoco.Tenant),
                        };

                        entityLastUpdateRepository.Add(update);
                    }

                    entityLastUpdateRepository.SubmitChanges();
                }

                else
                {
                    ActivityLogger.AddAcitivityLog(entityPoco.Id, objecttable.Id, entityPoco.Tenant, "U", entityPoco.UpdatedByUserId);
                }
            }
        }
        private void BuildImportersQueue()
        {
            if (this.isNewEntity)
            {
                try
                {

                    //&& !entityPM.IsCancelled // for LogBox
                    if (IsShipmentMatchLogBoxConditions(loggedTenant, entityPM, this.isNewEntity))
                    {
                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, entityPM.CustomerId);

                        PrivateLabelShipmentService privateLabelShipmentService = new PrivateLabelShipmentService(entityPM, customerTenantAccessInfo);
                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0 && privateLabelShipmentService.IsShipmentsAllowedForLogBox())
                        {
                            var ImporterTenant = customerTenantAccessInfo.CustomerTenant;
                            IQueueService queueservice = new DbQueueService();
                            if (IsShipmentMatchDigitalQueueConditions(entityPM))
                            {
                                queueservice.InitializeQueue("ImportersDigitalShipmentQueue", 0);
                            }
                            else
                            {
                                queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            }
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", customerTenantAccessInfo.CustomerTenant.ToString() }, { "CustomerId", entityPM.CustomerId } }, tenant, null, entityPM.CustomerId);
                        }
                    }

                    OpenForwarderShipmentQueue();
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
            }

            else
            {
                try
                {

                    //&& !entityPM.IsCancelled // for LogBox
                    if (IsShipmentMatchLogBoxConditions(loggedTenant, entityPM, this.isNewEntity))
                    {
                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, entityPM.CustomerId);

                        PrivateLabelShipmentService privateLabelShipmentService = new PrivateLabelShipmentService(entityPM, customerTenantAccessInfo);
                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0 && privateLabelShipmentService.IsShipmentsAllowedForLogBox())
                        {
                            var ImporterTenant = customerTenantAccessInfo.CustomerTenant;
                            IQueueService queueservice = new DbQueueService();
                            if (IsShipmentMatchDigitalQueueConditions(entityPM))
                            {
                                queueservice.InitializeQueue("ImportersDigitalShipmentQueue", 0);
                            }
                            else
                            {
                                queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            }
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", customerTenantAccessInfo.CustomerTenant.ToString() }, { "CustomerId", !string.IsNullOrEmpty(OldCustomerId) ? OldCustomerId : entityPM.CustomerId }, { "CustomerChanged", CustomerChanged } }, tenant, null, entityPM.CustomerId);
                        }
                        else if (!string.IsNullOrEmpty(entityPoco.CustomerShipmentNumber) && !string.IsNullOrEmpty(OldCustomerId) && CustomerChanged == "true")
                        {
                            IQueueService queueservice = new DbQueueService();
                            if (IsShipmentMatchDigitalQueueConditions(entityPM))
                            {
                                queueservice.InitializeQueue("ImportersDigitalShipmentQueue", 0);
                            }
                            else
                            {
                                queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            }
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", entityPoco.CustomerTenantNumber.ToString() }, { "CustomerId", OldCustomerId }, { "CustomerChanged", CustomerChanged } }, tenant, null, entityPM.CustomerId);
                        }
                    }
                    OpenForwarderShipmentQueue();

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
            }
        }

        private void OpenForwarderShipmentQueue()
        {
            EntityStatusRepository entityStatusRep = new EntityStatusRepository(tenant);
            EntityStatus myStatus = entityStatusRep.GetSingleEntityStatusByCode("INPS", tenant);//"in progress"
            if (loggedTenant.LogBoxTenantSetting.IsDocumentsArchive && !entityPM.DontAddToForwarderQueue && (myStatus != null && (entityPM.StatusId == myStatus.Id && string.IsNullOrEmpty(entityPM.ForwarderShipmentNumber)) || entityPM.SendUpdatesToAgentEnabled))
            {
                if (IsPrivateLabelTenant(entityPM.Tenant))
                {
                    IQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("ForwarderShipmentQueue", 0);
                    queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, }, tenant);
                }

            }
        }

        private bool IsPrivateLabelTenant(int tenant)
        {
            TenantQuery TenantQuery = new TenantQuery(entityPM.Tenant);
            TenantPM CurrentTenant = TenantQuery.GetSingleTenantPM(entityPM.Tenant, false);
            return (!string.IsNullOrEmpty(CurrentTenant.PrivateLabelId));
        }

        private bool IsImporterTenantHasExportFeatureForExportShipments(int ImporterTenant, ShipmentPM entityPM)
        {
            if ((entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R") && !FeatureToggleHelper.HasFeatureToggle("LEX", ImporterTenant, entityPM.Tenant))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsShipmentMatchDigitalQueueConditions(ShipmentPM entityPM)
        {
            return entityPM.IsHybrid && (entityPM.CreatedFromDigital || (entityPM.IsImporterApprovalRequired == true && !string.IsNullOrEmpty(entityPM.DeclarationXMLData)));
        }

        private bool IsShipmentMatchLogBoxConditions(Tenant loggedTenant, ShipmentPM entityPM, bool isNewEntity)
        {
            if (!entityPM.DontAddToImportersQueue
                && IsLogBoxQueueEnabled(loggedTenant, entityPM)
                && !loggedTenant.LogBoxTenantSetting.IsDocumentsArchive
                && (isNewEntity == true ? !entityPM.IsCancelled : true)
                && IsImportShipmentsAllowedForLogBox(loggedTenant, entityPM))

            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsLogBoxQueueEnabled(Tenant loggedTenant, ShipmentPM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.CustomerShipmentNumber) && entityPM.CustomerTenantNumber != null)
                return true;
            else
            {
                CustomerRepository customerRepository = new CustomerRepository(tenant);
                Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, tenant, true);
                if (customer != null && (customer.LogBoxActivated || customer.IsPrivateLabelCustomer))
                    return true;
                if(!string.IsNullOrEmpty(entityPoco.CustomerShipmentNumber) && !string.IsNullOrEmpty(OldCustomerId) && CustomerChanged == "true")
                    return true;
            }

            return false;
        }

        private bool IsImportShipmentsAllowedForLogBox(Tenant loggedTenant, ShipmentPM entityPM)
        {
            if (entityPM.DirectionId.ToUpper() == "I")
                return false;
            return true;
        }

        private bool IsExportShipmentsAllowedForLogBox(Tenant loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareExportFile == true)// && FeatureToggleHelper.HasFeatureToggle("LEX", loggedTenant.Id)
            {
                return (entityPM.DirectionId.ToUpper() == "E" || entityPM.DirectionId.ToUpper() == "R");
            }
            else
            {
                return false;
            }
        }

        private void RunStoredProcedures()
        {
            RunStoredProcedureClass.UpdateForeignPartnerCountryCode(entityPM.Id, entityPM.Tenant);
            RunStoredProcedureClass.UpdateShipmentStatus(entityPM.Id, entityPM.Tenant);
            RunStoredProcedureClass.UpdateShipmentOperationalDate(entityPM.Id, entityPM.Tenant);

            //RunStoredProcedureClass.UpdateShipmentFinalArrivalDate(entityPM.Id, entityPM.Tenant);

            if (!string.IsNullOrEmpty(entityPM.CustomFilePocoId))
            {
                RunStoredProcedureClass.UpdateCustomConnectToShipment(entityPM.CustomFilePocoId, entityPM.Tenant);
            }

            bool isReloadingConsoles = false;

            if (initializer.IsProratingChanged)
            {
                this.entityPM.CalculateProfit = true;
                this.entityPM.CalculateReceivables = true;
            }

            if (entityPM.CalculatePayables)
            {
                if (this.entityPM.ShipmentLevelCode != "D")
                {
                    UpdateShipmentProfitClass.UpdatePayables(entityPM.Id, entityPM.Tenant, false, null);

                    // Ayman: Please don't remove
                    if (this.entityPM.ShipmentLevelCode == "C")
                    {
                        isReloadingConsoles = true;
                    }

                    ShipmentPayableQuery shipmentPayableQuery = new ShipmentPayableQuery(this.shipmentPayableRepository);
                    entityPM.ShipmentPayables = shipmentPayableQuery.GetShipmentPayablePMsByShipment(entityPM.Id, entityPM.Tenant);
                }
            }

            if (entityPM.CalculateReceivables)
            {
                if (this.entityPM.ShipmentLevelCode != "D")
                {
                    UpdateShipmentProfitClass.UpdateReceivables(entityPM.Id, entityPM.Tenant, false);

                    // Ayman: Please don't remove
                    if (this.entityPM.ShipmentLevelCode == "C")
                    {
                        isReloadingConsoles = true;
                    }

                    ShipmentReceivableQuery shipmentReceivableQuery = new ShipmentReceivableQuery(this.shipmentReceivableRepository);
                    entityPM.ShipmentReceivables = shipmentReceivableQuery.GetShipmentReceivablePMsByShipmentId(entityPM.Id, entityPM.Tenant);
                }
            }

            if (initializer.IsUpdatingRegistryDate)
            {
                this.RunRegistryDateProcedure(this.entityPM.Id);
            }

            if (initializer.IsUpdatingFirstApprovalDate)
            {
                this.RunFirstApprovalDateProcedure(this.entityPM.Id);
            }

            if (entityPM.CalculateProfit && !entityPM.IsHybrid)
            {
                UpdateShipmentProfitClass.UpdateProfit(entityPM.Id, entityPM.Tenant);
            }

            // Ayman: Please don't remove
            IShipmentsContext updatedEntityContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository updatedEntityRepository = new ShipmentRepository(updatedEntityContext);
            Shipment updatedPOCO = updatedEntityRepository.GetSingleShipment(entityPM.Id, entityPM.Tenant);

            if (updatedPOCO != null)
            {
                entityPM.OpenPayablesInLocalCurrency = updatedPOCO.OpenPayablesInLocalCurrency;
                entityPM.OpenPayablesInProfitCurrency = updatedPOCO.OpenPayablesInProfitCurrency;
                entityPM.AccountedPayablesInLocalCurrency = updatedPOCO.AccountedPayablesInLocalCurrency;
                entityPM.AccountedPayablesInProfitCurrency = updatedPOCO.AccountedPayablesInProfitCurrency;
                entityPM.OpenReceivablesInLocalCurrency = updatedPOCO.OpenReceivablesInLocalCurrency;
                entityPM.OpenReceivablesInProfitCurrency = updatedPOCO.OpenReceivablesInProfitCurrency;
                entityPM.AccountedReceivablesInLocalCurrency = updatedPOCO.AccountedReceivablesInLocalCurrency;
                entityPM.AccountedReceivablesInProfitCurrency = updatedPOCO.AccountedReceivablesInProfitCurrency;
                entityPM.ProfitInLocalCurrency = updatedPOCO.ProfitInLocalCurrency;
                entityPM.ProfitInProfitCurrency = updatedPOCO.ProfitInProfitCurrency;
                entityPM.ShipmentPayableStatusCode = updatedPOCO.ShipmentPayableStatusCode;
                entityPM.ShipmentReceivableStatusCode = updatedPOCO.ShipmentReceivableStatusCode;
                entityPM.ARInvoiceIssued = updatedPOCO.ARInvoiceIssued;
                entityPM.CreditNoteIssued = updatedPOCO.CreditNoteIssued;
                entityPM.OperationalDate = updatedPOCO.OperationalDate;
                entityPM.RegistryDate = updatedPOCO.RegistryDate;
                entityPM.FirstARInvoiceApprovalDate = updatedPOCO.FirstARInvoiceApprovalDate;

                entityPM.HousesOpenPayablesInLocal = updatedPOCO.HousesOpenPayablesInLocal;
                entityPM.HousesOpenPayablesInProfit = updatedPOCO.HousesOpenPayablesInProfit;
                entityPM.HousesACCTPayablesInLocal = updatedPOCO.HousesACCTPayablesInLocal;
                entityPM.HousesACCTPayablesInProfit = updatedPOCO.HousesACCTPayablesInProfit;
                entityPM.HousesOpenReceivablesInLocal = updatedPOCO.HousesOpenReceivablesInLocal;
                entityPM.HousesOpenReceivablesInProfit = updatedPOCO.HousesOpenReceivablesInProfit;
                entityPM.HousesACCTReceivablesInLocal = updatedPOCO.HousesACCTReceivablesInLocal;
                entityPM.HousesACCTReceivablesInProfit = updatedPOCO.HousesACCTReceivablesInProfit;
            }

            if (isReloadingConsoles)
            {
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(updatedEntityContext);
                shipmentConsoleShipmentQuery.BuildConsoleShipments(entityPM);
            }
        }

        //private void UpdateShipmentOrderPackagesCollection()
        //{
        //    if (initializer.ShipmentOrderPackagesChangeSet != null)
        //    {
        //        foreach (ShipmentOrderPackagePM itemPM in initializer.ShipmentOrderPackagesChangeSet)
        //        {
        //            switch (itemPM.ChangeSetOp)
        //            {
        //                case ChangeSetOperation.Insert:
        //                    {
        //                        this.CreateShipmentOrderPackage(itemPM);
        //                        break;
        //                    }

        //                case ChangeSetOperation.Update:
        //                    {
        //                        this.UpdateShipmentOrderPackage(itemPM);
        //                        break;
        //                    }

        //                case ChangeSetOperation.Delete:
        //                    {
        //                        this.DeleteShipmentOrderPackage(itemPM);
        //                        break;
        //                    }

        //                default: { break; }
        //            }
        //        }
        //    }
        //}
        private void UpdateShipmentPackagesCollection()
        {
            if (initializer.ShipmentPackagesChangeSet != null)
            {
                foreach (ShipmentPackagePM itemPM in initializer.ShipmentPackagesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPackage(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPackage(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPackage(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentPackageCustom()
        {
            foreach (ShipmentPackagePM itemPM in entityPM.ShipmentPackages)
            {
                switch (itemPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            this.CreateShipmentPackage(itemPM);
                            break;
                        }

                    case ChangeSetOperation.Update:
                        {
                            this.UpdateShipmentPackage(itemPM);
                            break;
                        }

                    case ChangeSetOperation.Delete:
                        {
                            this.DeleteShipmentPackage(itemPM);
                            break;
                        }

                    default: { break; }
                }
            }
        }
        private void UpdateShipmentPickUpsCollection()
        {
            if (initializer.ShipmentPickUpsChangeSet != null)
            {
                foreach (ShipmentPickUpPM itemPM in initializer.ShipmentPickUpsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPickUp(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPickUp(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPickUp(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentDeliveriesCollection()
        {
            if (initializer.ShipmentDeliveriesChangeSet != null)
            {
                foreach (ShipmentDeliveryPM itemPM in initializer.ShipmentDeliveriesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentDelivery(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentDelivery(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentDelivery(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentPayablesCollection()
        {
            if (initializer.ShipmentPayablesChangeSet != null)
            {
                foreach (ShipmentPayablePM itemPM in initializer.ShipmentPayablesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPayable(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPayable(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPayable(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentReceivablesCollection()
        {
            if (initializer.ShipmentReceivablesChangeSet != null)
            {
                foreach (ShipmentReceivablePM itemPM in initializer.ShipmentReceivablesChangeSet)
                {
                    this.CheckReceivableStatus(itemPM);

                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentReceivable(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentReceivable(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentReceivable(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentAWBPrintOnliesCollection()
        {
            if (initializer.ShipmentAWBPrintOnliesChangeSet != null)
            {
                foreach (ShipmentAWBPrintOnlyPM itemPM in initializer.ShipmentAWBPrintOnliesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentAWBPrintOnly(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentAWBPrintOnly(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentAWBPrintOnly(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentConsoleShipmentsCollection()
        {
            if (initializer.ShipmentConsoleShipmentsChangeSet != null)
            {
                foreach (ConsoleShipmentPM itemPM in initializer.ShipmentConsoleShipmentsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateConsoleShipment(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteConsoleShipment(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentFollowUpsCollection(string changeSet = "Update")
        {
            if (initializer.ShipmentFollowUpsChangeSet != null)
            {
                bool isChange = false;

                if (changeSet == "InSert")
                {
                    #region Insert FollowUp
                    List<FollowUp> doneFollowUps = new List<FollowUp>();
                    foreach (ShipmentFollowUpPM itemPM in initializer.ShipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert))
                    {
                        isChange = true;
                        if (itemPM.Done) itemPM.Deleted = true;
                        this.CreateShipmentFollowUp(itemPM);
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "SFCR",
                            UserId = this.loggedContact.Id,
                            EntityId = this.entityPM.Id,
                            ObjectTableName = "Shipment",
                            Notes = itemPM.EventTypeFollowUpName,
                            Entity = entityPM,
                        });

                    }
                    if (isChange) followUpRepository.SubmitChanges();
                    #endregion

                    #region Done FollowUp
                    foreach (ShipmentFollowUpPM itemPM in initializer.ShipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert && d.Done))
                    {
                        FollowUp follow = followUpRepository.GetSingleFollowUp(itemPM.Id, tenant);
                        doneFollowUps.Add(follow);

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "SFCM",
                            UserId = this.loggedContact.Id,
                            EntityId = this.entityPM.Id,
                            ObjectTableName = "Shipment",
                            Notes = itemPM.EventTypeFollowUpName,
                            Entity = entityPM,

                        });

                    }
                    if (doneFollowUps.Count > 0)
                    {
                        foreach (FollowUp itemPoco in doneFollowUps)
                        {
                            if (!entityPM.IsHybrid)
                            {
                                shipmentTracing.TraceShipmentOnCreateDoneFollowUp(itemPoco);
                            }

                            followUpRepository.Remove(itemPoco);

                        }

                        followUpRepository.SubmitChanges();
                    }

                    #endregion
                }
                else
                {
                    #region Delete & Update FollowUp
                    isChange = false;
                    foreach (ShipmentFollowUpPM itemPM in initializer.ShipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Delete))
                    {
                        this.DeleteShipmentFollowUp(itemPM);
                        isChange = true;
                    }

                    foreach (ShipmentFollowUpPM itemPM in initializer.ShipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Update))
                    {
                        if (itemPM.Done)
                        {
                            itemPM.Deleted = true;
                            entityPM.MarkFollowUpsAsDone = true;

                            if (!entityPM.IsHybrid)
                            {
                                shipmentTracing.TraceShipmentOnUpdateDoneFollowUp(itemPM);
                            }

                            this.DeleteShipmentFollowUp(itemPM);
                            followUpRepository.SubmitChanges();

                            EventTracer.CreateTraceEvent(new EventTracerArgs()
                            {
                                Tenant = tenant,
                                EventTypeCode = "SFCM",
                                UserId = this.loggedContact.Id,
                                EntityId = this.entityPM.Id,
                                ObjectTableName = "Shipment",
                                Notes = itemPM.EventTypeFollowUpName,
                                Entity = entityPM,
                            });
                        }

                        else
                        {
                            this.UpdateShipmentFollowUp(itemPM);
                        }

                        isChange = true;
                    }

                    if (isChange) followUpRepository.SubmitChanges();
                    #endregion

                    this.ComputeNumerOfFollowUps();
                }

            }
        }
        private void UpdateShipmentCarrierStatusesCollection()
        {
            if (initializer.ShipmentCarrierStatusesChangeSet != null)
            {
                foreach (ShipmentCarrierStatusPM itemPM in initializer.ShipmentCarrierStatusesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentCarrierStatus(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentCarrierStatus(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentCarrierStatus(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentAWBOCIsCollection()
        {
            if (initializer.AWBOCIPMChangeSet != null)
            {
                foreach (AWBOCIPM itemPM in initializer.AWBOCIPMChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateAWBOCI(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateAWBOCI(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteAWBOCI(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentCommoditiesCollection()
        {
            if (initializer.ShipmentCommoditiesChangeSet != null)
            {
                foreach (ShipmentCommodityPM itemPM in initializer.ShipmentCommoditiesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentCommodity(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentCommodity(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentCommodity(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentAssembliesCollection()
        {
            if (initializer.ShipmentAssembliesChangeSet != null)
            {
                foreach (ShipmentAssemblyPM itemPM in initializer.ShipmentAssembliesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentAssembly(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentAssembly(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentAssembly(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentReferancesCollection()
        {
            if (initializer.ShipmentReferanceChangeSet != null && initializer.ShipmentReferanceChangeSet.Count > 0)
            {
                foreach (ShipmentReferancePM itemPM in initializer.ShipmentReferanceChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentReferance(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentReferance(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentReferance(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
                this.shipmentReferanceRepository.SubmitChanges();
            }
        }
        private void ValidateShipmentReferancesCollection()
        {
            if (initializer.ShipmentReferanceChangeSet != null && initializer.ShipmentReferanceChangeSet.Count > 0)
            {
                foreach (ShipmentReferancePM itemPM in initializer.ShipmentReferanceChangeSet)
                {
                    if (itemPM.ChangeSetOp == ChangeSetOperation.Insert || itemPM.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        if (string.IsNullOrEmpty(itemPM.ReferenceType) && !string.IsNullOrEmpty(itemPM.ReferenceValue))
                        {
                            throw new ApplicationException(TranslateTextsClass.Translate("ShipmentReferance.O.MissingReferenceType", tenant));
                        }
                        else if (!string.IsNullOrEmpty(itemPM.ReferenceType) && string.IsNullOrEmpty(itemPM.ReferenceValue))
                        {
                            throw new ApplicationException(TranslateTextsClass.Translate("ShipmentReferance.O.MissingReferenceValue", tenant));
                        }
                        else if (itemPM.ChangeSetOp == ChangeSetOperation.Insert && string.IsNullOrEmpty(itemPM.ReferenceType) && string.IsNullOrEmpty(itemPM.ReferenceValue))
                        {
                            initializer.ShipmentReferanceChangeSet.Remove(itemPM);
                        }
                    }
                }
                this.shipmentReferanceRepository.SubmitChanges();
            }
        }

        private void UpdateShipmentStoragePricingsCollection()
        {
            if (initializer.ShipmentStoragePricingsChangeSet != null)
            {
                foreach (ShipmentStoragePricingPM itemPM in initializer.ShipmentStoragePricingsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentStoragePricing(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentStoragePricing(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentStoragePricing(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentProductItemsCollection()
        {
            if (initializer.ShipmentProductItemsChangeSet != null)
            {
                foreach (ShipmentProductItemPM itemPM in initializer.ShipmentProductItemsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentProductItem(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentProductItem(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentProductItem(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentUnassignedFieldsCollection()
        {
            if (initializer.ShipmentUnassignedFieldChangeSet != null)
            {
                foreach (ShipmentUnassignedFieldPM itemPM in initializer.ShipmentUnassignedFieldChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentUnassignedField(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentUnassignedField(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentUnassignedField(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void BuildShipmentExternalUpdate()
        {
            if (!string.IsNullOrEmpty(entityPM.AgentSharedManifestRef))
            {
                if (entityPM.ShipmentLevelCode != "H")
                {
                    if (entityMasterData == null)
                    {
                        entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPM.Id);
                    }
                    if (entityMasterData != null)
                    {
                        if (CheckIfChangeLastATADate())
                        {
                            AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(tenant);
                            AgentSharedManifestPM agentSharedManifestPM = agentSharedManifestQuery.GetSinglePM(entityPM.AgentSharedManifestRef, tenant);
                            List<Status> statusLists = new List<Status>();

                            if (agentSharedManifestPM != null)
                            {
                                if (!string.IsNullOrEmpty(agentSharedManifestPM.ManifestXML))
                                {
                                    ManifestSL manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(agentSharedManifestPM.ManifestXML);

                                    if (manifestSL != null)
                                    {
                                        EntityExternalUpdate entityExternalUpdate = new EntityExternalUpdate();
                                        entityExternalUpdate.Master = entityPM.Master;
                                        entityExternalUpdate.House = entityPM.House;
                                        entityExternalUpdate.ShipmentNumber = manifestSL.ShipmentNumber;
                                        entityExternalUpdate.Tenant = manifestSL.SourceAgentTenant;
                                        entityExternalUpdate.Entity = "Shipment";

                                        if (!string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
                                        {
                                            statusLists.Add(GetLegShipmentExternalStatus(entityPM.MainCarriageToPortCode, entityPM.MainCarriageToPortCountryCode, entityPM.MainCarriageATA));
                                        }

                                        if (!string.IsNullOrEmpty(entityPM.Transshipment1ToPortCode))
                                        {
                                            statusLists.Add(GetLegShipmentExternalStatus(entityPM.Transshipment1ToPortCode, entityPM.Transshipment1ToPortCountryCode, entityPM.Transshipment1ATA));
                                        }

                                        if (!string.IsNullOrEmpty(entityPM.Transshipment2ToPortCode))
                                        {
                                            statusLists.Add(GetLegShipmentExternalStatus(entityPM.Transshipment2ToPortCode, entityPM.Transshipment2ToPortCountryCode, entityPM.Transshipment2ATA));
                                        }

                                        if (!string.IsNullOrEmpty(entityPM.Transshipment3ToPortCode))
                                        {
                                            statusLists.Add(GetLegShipmentExternalStatus(entityPM.Transshipment3ToPortCode, entityPM.Transshipment3ToPortCountryCode, entityPM.Transshipment3ATA));
                                        }

                                        entityExternalUpdate.Statuses = statusLists;

                                        //TenantRepository tenantRepository = new TenantRepository(tenant);
                                        //Tenant sourceAgentTenantPOCO = tenantRepository.GetSingleTenant(tenant);
                                        // Tenant destinationAgentTenantPOCO = tenantRepository.GetSingleTenant(manifestSL.SourceAgentTenant);


                                        using (TransactionScope scope = TransactionFactory.GetTransaction())
                                        {
                                            byte[] entityExternalUpdateXML = LogitudeXmlSerializer.SerializeObject(entityExternalUpdate);
                                            ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                                            ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);
                                            CommunicationsParams logParams = new CommunicationsParams()
                                            {
                                                Tenant = tenant,
                                                From = "Agent",
                                                To = "Agent",
                                                CommunicationLogTypeCode = "Q",
                                                QueueName = "AgentsSharedLogisticsQueue",
                                                Priority = 1,
                                                InOut = "O",
                                                Status = "W",
                                                LoggingUserId = loggedContact.Id,
                                                LoggingObjectTableId = table.Id,
                                                LoggingEntityId = entityPM.Id,
                                                Subject = "Status Update",
                                                FolderName = "AgentsSharedLogisticsQueue",
                                                ByteData = entityExternalUpdateXML,
                                            };
                                            Communications.AddCommunicationLog(logParams);
                                            scope.Complete();
                                        }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        private void BuildAgentSharedManifest()
        {
            if (entityPM.IsCreatedFromAgentSharedManifest)
            {
                AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(tenant);
                AgentSharedManifestPM agentSharedManifestPM = agentSharedManifestQuery.GetSinglePM(this.entityPM.AgentSharedManifestRef.Split('/')[0], tenant);
                if (agentSharedManifestPM != null)
                {
                    if (!string.IsNullOrEmpty(agentSharedManifestPM.ManifestXML))
                    {
                        ObjectTableQuery tablesQuery = new ObjectTableQuery(tenant);
                        ObjectTablePM table = tablesQuery.GetObjectTableByName("Shipment", 0);
                        ManifestSL manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(agentSharedManifestPM.ManifestXML);
                        if (manifestSL != null)
                        {
                            AddAgentManifestCommunicationsLog(agentSharedManifestPM, manifestSL, table.Id);

                            if (this.entityPM.ShipmentLevelCode != "H")
                            {
                                this.AddEventSharedFromAgent(manifestSL.AgentName, table.Id);
                            }

                        }


                    }
                }
            }

        }
        private bool CheckIfChangeLastATADate()
        {
            bool isChangeLastATADate = false;
            if (entityPM != null && entityMasterData != null)
            {
                if (!string.IsNullOrEmpty(entityPM.Transshipment3ToPortCode))
                {
                    if (entityPM.Transshipment3ATA != entityMasterData.Transshipment3ATA) isChangeLastATADate = true;
                }

                else if (!string.IsNullOrEmpty(entityPM.Transshipment2ToPortCode))
                {
                    if (entityPM.Transshipment2ATA != entityMasterData.Transshipment2ATA) isChangeLastATADate = true;
                }

                else if (!string.IsNullOrEmpty(entityPM.Transshipment1ToPortCode))
                {
                    if (entityPM.Transshipment1ATA != entityMasterData.Transshipment1ATA) isChangeLastATADate = true;
                }

                else if (!string.IsNullOrEmpty(entityPM.MainCarriageToPortCode))
                {
                    if (entityPM.MainCarriageATA != entityMasterData.MainCarriageATA) isChangeLastATADate = true;
                }
            }
            return isChangeLastATADate;
        }
        private Status GetLegShipmentExternalStatus(string portCode, string portCountry, DateTime? date)
        {
            Status status = new Status()
            {
                Code = "ARR",
                Name = "Arrived",
                Date = date,
                PortCode = portCode,
                PortCountry = portCountry,
            };
            return status;
        }
        private void AddAgentManifestCommunicationsLog(AgentSharedManifestPM agentSharedManifestPM, ManifestSL manifestSL, string tableId)
        {
            if (manifestSL != null)
            {
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant sourceAgentTenantPOCO = tenantRepository.GetSingleTenant(manifestSL.SourceAgentTenant);
                Tenant destinationAgentTenantPOCO = tenantRepository.GetSingleTenant(manifestSL.DestinationAgentTenant);

                if (sourceAgentTenantPOCO != null && sourceAgentTenantPOCO != null)
                {
                    if (this.entityPM.ShipmentLevelCode != "H")
                    {

                        using (TransactionScope scope1 = TransactionFactory.GetTransaction())
                        {
                            byte[] manifestXML = Encoding.UTF8.GetBytes(agentSharedManifestPM.ManifestXML);

                            CommunicationsParams logParams = new CommunicationsParams()
                            {
                                Tenant = tenant,
                                From = sourceAgentTenantPOCO.Company + " " + sourceAgentTenantPOCO.Id,
                                To = destinationAgentTenantPOCO.Company + " " + destinationAgentTenantPOCO.Id,
                                CommunicationLogTypeCode = "Q",
                                Priority = 1,
                                InOut = "I",
                                Status = "D",
                                LoggingUserId = loggedContact.Id,
                                LoggingObjectTableId = tableId,
                                LoggingEntityId = entityPM.Id,
                                Subject = "Shared Manifest",
                                FolderName = "AgentsSharedLogisticsQueue",
                                ByteData = manifestXML,
                            };

                            Communications.AddCommunicationLog(logParams);


                            scope1.Complete();
                        };
                    }


                    if (!string.IsNullOrEmpty(this.entityPM.AgentSharedManifestRef))
                    {

                        AgentSharedDocumentQuery agentSharedDocumentQuery = new AgentSharedDocumentQuery(tenant);
                        List<AgentSharedDocumentPM> agentSharedDocumentLists = agentSharedDocumentQuery.GetAgentSharedDocumentPMsByAgentSharedManifestRef(this.entityPM.AgentSharedManifestRef, tenant).ToList();

                        if (agentSharedDocumentLists.Count > 0)
                        {
                            foreach (AgentSharedDocumentPM agentSharedDocument in agentSharedDocumentLists)
                            {

                                IQueueService queueservice = new DbQueueService();
                                queueservice.InitializeQueue("AgentsSharedDocumentQueue", tenant);
                                queueservice.Send(new Dictionary<string, string>() { { "EntityId", agentSharedDocument.Id }, { "Tenant", sourceAgentTenantPOCO.Id.ToString() }, { "AgentTenant", agentSharedDocument.Tenant.ToString() } }, tenant, null, null, null, null);


                                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                                //{
                                //    byte[] documentXML = null;

                                //    if (!string.IsNullOrEmpty(agentSharedDocument.DocumentXML))
                                //    {
                                //        DocumentSL documentSL = LogitudeXmlSerializer.DeserializeObject<DocumentSL>(agentSharedDocument.DocumentXML);
                                //        documentXML = LogitudeXmlSerializer.SerializeObject(documentSL);
                                //    }

                                //    CommunicationsParams logParams = new CommunicationsParams()
                                //    {
                                //        From = sourceAgentTenantPOCO.Company + " " + sourceAgentTenantPOCO.Id,
                                //        To = destinationAgentTenantPOCO.Company + " " + destinationAgentTenantPOCO.Id,
                                //        Tenant = sourceAgentTenantPOCO.Id,
                                //        CommunicationLogTypeCode = "Q",
                                //        QueueName = "AgentsSharedDocumentQueue",
                                //        Priority = 1,
                                //        InOut = "O",
                                //        Status = "W",
                                //        LoggingUserId = loggedContact.Id,
                                //        LoggingObjectTableId = tableId,
                                //        LoggingEntityId = entityId,
                                //        Subject = "Shared Documents",
                                //        FolderName = "AgentsSharedDocumentQueue",
                                //        ByteData = documentXML,
                                //    };

                                //    logParams.QueueParameters = new Dictionary<string, string>() { { "EntityId", agentSharedDocument.Id },
                                //        { "Tenant", sourceAgentTenantPOCO.Id.ToString() }, { "AgentTenant", agentSharedDocument.Tenant.ToString() } };

                                //    Communications.AddCommunicationLog(logParams);

                                //    scope.Complete();
                                //}
                            }
                        }
                    }

                }
            }
        }
        private void AddEventSharedFromAgent(string agentName, string objectTableId)
        {
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            EventTypePM eventType = eventTypeQuery.GetSinglePMByCode("CFSM", tenant);

            if (eventType != null)
            {
                string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                UserRepository userRepository = new UserRepository(entityPM.Tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                if (loggedUser == null)
                {
                    loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, "system@tenant" + entityPM.Tenant + ".com", entityPM.Tenant, true);
                }

                TraceEvent newTraceEvent = new TraceEvent();
                newTraceEvent.Id = Guid.NewGuid().ToString();
                newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                newTraceEvent.ObjectTableId = objectTableId;
                newTraceEvent.Tenant = tenant;
                newTraceEvent.UserId = loggedUser != null ? loggedUser.Id : "";
                newTraceEvent.EventTypeId = eventType.Id;
                newTraceEvent.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                newTraceEvent.EntityId = entityPM.Id;
                newTraceEvent.Deleted = false;
                newTraceEvent.IsAddedManually = true;
                newTraceEvent.PartnerName = agentName;
                TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
                traceEventRep.Add(newTraceEvent);
                traceEventRep.SubmitChanges();
            }

        }
        public string SetReceivableLineStatus(ShipmentReceivablePM receivable)
        {
            var status = receivable.ShipmentReceivableLineStatusCode;
            if (receivable.ShipmentReceivableLineStatusCode == "APPD" || receivable.ShipmentReceivableLineStatusCode == "ACCT" || receivable.ShipmentReceivableLineStatusCode == "DRFT")
            {

            }

            else
            {
                if (receivable.MeasurementCode == "STFE" && receivable.ChargesTypeCode == "ISTOR")
                {
                    // import storage charge has no quantity or price
                    status = "OAMT";
                }

                else
                {
                    if (receivable.Quantity != null && receivable.UnitPrice != null)
                    {
                        status = "OAMT";
                    }

                    else
                    {
                        status = "EMPT";
                    }
                }
            }
            return status;
        }


        private List<MainEntityChangeService> mainEntityChangeServices = new List<MainEntityChangeService>();
        private void RunAutomation(string type, ShipmentChangeTracking shipmentChangeTracking = null)
        {
            if (entityPM != null)
            {
                string tableName = entityPM.ShipmentLevelCode == "C" ? "Master" : entityPM.ShipmentLevelCode == "H" ? "Shipment" : "MasterAndHouse";
                string objectTableName = tableName;
                string otherObjectTableName = "";

                if (tableName == "MasterAndHouse")
                {
                    objectTableName = "Master";
                    otherObjectTableName = "Shipment";
                }

                MapMainCarriageLegsForAutomation(); //temp Solution
                GeneralEntityChangeService generalEntityChangeService = new GeneralEntityChangeService();
                object externalEntity = (entityPM.ShipmentLevelCode != "H" && entityMasterData != null) ? this.entityPM : null;

                if (type == "OnCreate")
                {
                    bool isHaveAutomation = generalEntityChangeService.CheckIfEntityHaveAutomation(tableName, "OnCreate", entityPM.Tenant);
                    if (isHaveAutomation)
                    {
                        var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { ExternalEntity = externalEntity, EntityPM = entityPM, ProcessType = "OnCreate", ObjectTableName = objectTableName, EntityId = entityPM.Id, Tenant = entityPM.Tenant, StartDate = DateTime.Now, OtherObjectTableName = otherObjectTableName, DontExecuteAutomationThatDependencyOnLastEntityUpdate = true, EntityReference = entityPM.ShipmentNumber, LoggedUserEmail = UpdateByEmail });
                        mainEntityChangeService.AddEntityChange();
                        mainEntityChangeServices.Add(mainEntityChangeService);
                    }
                }

                else if (!entityPM.IsUpdateByAutomation && type == "OnUpdate")
                {
                    bool isHaveAutomation = generalEntityChangeService.CheckIfEntityHaveAutomation(tableName, "OnUpdate", entityPM.Tenant);
                    if (isHaveAutomation)
                    {
                        var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { ExternalEntity = externalEntity, EntityPM = entityPM, ProcessType = "OnUpdate", EntityChangeFieldXml = shipmentChangeTracking.EntityChangeFieldXml, OldEntityPM = shipmentChangeTracking.ChangeTrackingPM, ObjectTableName = objectTableName, EntityId = entityPM.Id, Tenant = entityPM.Tenant, StartDate = DateTime.Now, OtherObjectTableName = otherObjectTableName, DontExecuteAutomationThatDependencyOnLastEntityUpdate = true, EntityReference = entityPM.ShipmentNumber, LoggedUserEmail = UpdateByEmail });
                        mainEntityChangeService.AddEntityChange();
                        mainEntityChangeServices.Add(mainEntityChangeService);

                    }

                    #region Houses

                    if (entityPM.ShipmentLevelCode == "C")
                    {
                        isHaveAutomation = generalEntityChangeService.CheckIfEntityHaveAutomation("Shipment", "OnUpdate", entityPM.Tenant);
                        if (isHaveAutomation)
                        {
                            RunAutomationForHouses(shipmentChangeTracking);
                        }
                    }
                    #endregion
                }
            }
        }

        private void RunAutomationForHouses(ShipmentChangeTracking shipmentChangeTracking)
        {
            bool haveAutomatiomMasterHouseSetFieldValueFeatureToggle = FeatureToggleHelper.HasFeatureToggle("AHS", tenant);
            string masterChangeFields = "MainCarriageCarrierId,MainCarriageETD,MainCarriageATD,MainCarriageFinalDestinationETA,MainCarriageFinalDestinationATA,FinalDistenationPortId,StatusId,CutoffDate";
            List<NotifyPropertyChangeValues> changedProperties = shipmentChangeTracking.NotifyPropertyChangeValuesLists.Where(d => masterChangeFields.Split(',').Contains(d.PropertyName)).ToList();
            shipmentChangeTracking.EntityChangeFieldXml = EntityPMChangeTrackingHelper.GetChangesDetectedXml(changedProperties);
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            List<ShipmentPM> oldHousesPMs = shipmentQuery.GetShipmentPMsByMasterIdAndTenantForAutomation(entityPM.Id, tenant);
            List<ShipmentPM> newHousesPMs = haveAutomatiomMasterHouseSetFieldValueFeatureToggle ? shipmentQuery.GetShipmentPMsByMasterId(entityPM.Id, tenant) : new List<ShipmentPM>();
            bool IsHouseUpdated = false;
            foreach (ShipmentPM oldHousePM in oldHousesPMs)
            {
                ShipmentPM shipmentPm = haveAutomatiomMasterHouseSetFieldValueFeatureToggle ? newHousesPMs.Where(newHouse => newHouse.Id == oldHousePM.Id).FirstOrDefault() : new ShipmentPM();
                ShipmentPM newHousePM = RunAutomationForSingleHouse(shipmentChangeTracking, shipmentPm, oldHousePM);
                IsHouseUpdated = IsHouseUpdated ? IsHouseUpdated : IsUpdateHouseShipmentPM(newHousePM, haveAutomatiomMasterHouseSetFieldValueFeatureToggle);
            }

            if (IsHouseUpdated)
            {
                allHouses = entityRepository.GetHouseShipmentsForMaster(entityPM.Id, tenant);
            }
        }

        private ShipmentPM RunAutomationForSingleHouse(ShipmentChangeTracking shipmentChangeTracking, ShipmentPM shipmentPm, ShipmentPM oldHousePM)
        {
            if (shipmentPm == null) return shipmentPm;

            oldHousePM.StatusId = shipmentChangeTracking.ChangeTrackingPM.StatusId;
            shipmentPm = ShipmentMapping.MapShipmentPMToShipmentPMForAutomation(entityPM, oldHousePM, shipmentPm);
            var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() { ExternalEntity = this.entityPM, EntityPM = shipmentPm, OldEntityPM = oldHousePM, ProcessType = "OnUpdate", EntityChangeFieldXml = shipmentChangeTracking.EntityChangeFieldXml, ObjectTableName = "Shipment", EntityId = shipmentPm.Id, Tenant = shipmentPm.Tenant, StartDate = DateTime.Now, DontExecuteAutomationThatDependencyOnLastEntityUpdate = true, LoggedUserEmail = UpdateByEmail });
            mainEntityChangeService.IsChild = true;
            mainEntityChangeService.AddEntityChange();
            mainEntityChangeServices.Add(mainEntityChangeService);

            return shipmentPm;
        }

        private bool IsUpdateHouseShipmentPM(ShipmentPM housePM, bool haveAutomatiomMasterHouseSetFieldValueFeatureToggle)
        {
            if (!haveAutomatiomMasterHouseSetFieldValueFeatureToggle) return false;
            if (housePM == null) return false;
            if (!housePM.IsUpdatedByAutomationSetValueResult) return false;

            using (TransactionScope scopee = TransactionFactory.GetTransaction())
            {
                housePM.IsUpdateByAutomation = true;
                ShipmentService shipmentService = new ShipmentService(objectContext, housePM, SecurityUtility.GetAuthenticatedUser());
                shipmentService.Update(true);
                scopee.Complete();
                return true;
            }
        }

        private void SendAutomaticallyOceanOnsightsRequest()
        {
            if (IsAutomaticallyOceanOnsightsRequest())
            {
                // Shipment
                ContainerStatusesHelper myHelper = new ContainerStatusesHelper(this.initializer.EntityPM.Id, null, false, this.initializer.Tenant, objectContext);
                if (myHelper.Validate() && myHelper.IsLogitudeOceanInsightsRequestExistForShipment())
                {
                    myHelper.SendContainerStatusRequest();
                }
            }
        }

        private bool IsAutomaticallyOceanOnsightsRequest()
        {
            if (!FeatureToggleHelper.HasFeatureToggle("OIC", this.initializer.Tenant) || !FeatureToggleHelper.HasFeatureToggle("AOI", this.initializer.Tenant))
                return false;

            if (initializer.EntityPM.TransportModeId != "O")
                return false;

            if (initializer.EntityPM.ShipmentTypeId.ToLower() != "fcl" && initializer.EntityPM.ShipmentTypeId.ToLower() != "fcld")
                return false;

            if (string.IsNullOrEmpty(this.initializer.EntityPM.Master))
                return false;

            if (!this.initializer.IsFirstFourDigitsOfMasterNumberAreLetters())
                return false;

            return true;
        }

        private void MapMainCarriageLegsForAutomation()
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(entityRepository);
            shipmentQuery.MapMainCarriageLegsForAPI(entityPM);
        }

        private ShipmentChangeTracking BuildShipmentChangeTracking()
        {
            ShipmentMapping.ComputeMainCarriageFinalDestinationDates(this.entityMasterData, this.entityPM, true);
            ShipmentChangeTracking shipmentChangeTracking = new ShipmentChangeTracking() { ChangeTrackingPM = new ShipmentPM() };
            ShipmentQuery query = new ShipmentQuery(tenant);
            query.MapShipmentToShipmentPMForAutomation(new AutomationShipmentMappingArgs { ShipmentPM = shipmentChangeTracking.ChangeTrackingPM, Shipment = this.entityPoco, ShipmentMasterDataList = null, MasterData = this.entityMasterData, ShipmentPMBeforeNewMapping = this.entityPM });
            ShipmentQuery.MapFieldsBeforeTrackingChangedForAutomation(entityPM, initializer);
            shipmentChangeTracking.ChangeTrackingPM.StatusId = entityPM.OldStatusValue;
            if (entityPM.ShipmentLevelCode == "H") shipmentChangeTracking.ChangeTrackingPM.StatusId = entityPM.StatusId;
            shipmentChangeTracking.NotifyPropertyChangeValuesLists = ShipmentMapping.BuildChangedProperties(entityPM, shipmentChangeTracking.ChangeTrackingPM);
            shipmentChangeTracking.EntityChangeFieldXml = EntityPMChangeTrackingHelper.GetChangesDetectedXml(shipmentChangeTracking.NotifyPropertyChangeValuesLists);

            return shipmentChangeTracking;
        }

        private void InitializeComponent()
        {
            entityPM.CalculateProfit = false;
            entityPM.CalculatePayables = false;
            entityPM.CalculateReceivables = false;
            entityPM.MarkFollowUpsAsDone = false;
            entityPM.IsAddingStackEvents = false;
            entityPM.IsRemovingStackEvents = false;
            entityPM.CalculateStatus = false;

            this.ComputeShipmentStatus();

            if (isNewEntity)
            {
                #region
                shipmentAdditionalCloudData = new ShipmentAdditionalCloudData();
                shipmentAdditionalCloudData.Id = entityPM.Id;
                shipmentAdditionalCloudData.Tenant = entityPM.Tenant;
                shipmentAdditionalCloudData.IsImporterApprovalRequried = entityPM.IsImporterApprovalRequired;
                shipmentAdditionalCloudData.DeclarationXmlData = entityPM.DeclarationXMLData;
                shipmentAdditionalCloudData.DeclarationWCOXml = entityPM.DeclarationWCOXml;
                shipmentAdditionalCloudData.IsUserIDNumberRequired = entityPM.IsUserIDNumberRequired;
                if (!string.IsNullOrEmpty(entityPM.DeclarationWCOXml))
                {
                    shipmentAdditionalCloudData.DeclarationXmlData = null;
                }
                shipmentAdditionalCloudData.VersionApproved = entityPM.VersionApproved;
                shipmentAdditionalCloudData.ApproveDateTime = entityPM.ApproveDateTime;
                if (entityPM.UpdateSendUpdatesToAgentEnabledField)
                {
                    shipmentAdditionalCloudData.SendUpdatesToAgentEnabled = entityPM.SendUpdatesToAgentEnabled;
                }

                if (!string.IsNullOrEmpty(entityPM.ApprovedBy))
                {
                    shipmentAdditionalCloudData.ApprovedByUserName = entityPM.ApprovedBy;
                }
                if (!string.IsNullOrEmpty(entityPM.DocumentsApprovedByUserName))
                {
                    shipmentAdditionalCloudData.DocumentsApprovedByUserName = entityPM.DocumentsApprovedByUserName;
                }
                shipmentAdditionalCloudData.ShipmentAddtionalDataXML = ShipmentAdditionalDataService.SerializeShipmentAdditionalXmlData(entityPM.ShipmentAdditionalData);

                if (!string.IsNullOrEmpty(entityPM.PaymentRequestXML) && shipmentAdditionalCloudData.PaymentRequestXML != entityPM.PaymentRequestXML)
                {
                    shipmentAdditionalCloudData.IsPaymentRequired = true;
                    shipmentAdditionalCloudData.PaymentRequestXML = entityPM.PaymentRequestXML;
                    shipmentAdditionalCloudData.PaymentDateTime = entityPM.PaymentDateTime;
                    AddPaymentReceivedToQueue();
                }

                shipmentAdditionalCloudDataRepository.Add(shipmentAdditionalCloudData);
                //shipmentAdditionalCloudDataRepository.SubmitChanges();
                if (string.IsNullOrEmpty(entityPM.FreightPrepaidCollectId) || string.IsNullOrEmpty(entityPM.OtherPrepaidCollectId))
                {
                    string prepaidCollectId = null;
                    string otherPrepaidCollectId = null;

                    switch (entityPM.DirectionId)
                    {
                        case "E":
                        case "R":
                            {
                                prepaidCollectId = loggedTenant.ExportFreightPrepaidCollectId;
                                otherPrepaidCollectId = loggedTenant.ExportOtherPrepaidCollectId;
                                break;
                            }

                        case "I":
                        case "C":
                            {
                                prepaidCollectId = loggedTenant.ImportFreightPrepaidCollectId;
                                otherPrepaidCollectId = loggedTenant.ImportOtherPrepaidCollectId;
                                break;
                            }

                        case "D":
                            {
                                prepaidCollectId = "P";
                                otherPrepaidCollectId = "P";
                                break;
                            }
                    }

                    if (string.IsNullOrEmpty(entityPM.FreightPrepaidCollectId))
                    {
                        entityPM.FreightPrepaidCollectId = prepaidCollectId;
                    }

                    if (string.IsNullOrEmpty(entityPM.OtherPrepaidCollectId))
                    {
                        entityPM.OtherPrepaidCollectId = otherPrepaidCollectId;
                    }
                }

                this.InitializeOCIs();
                this.InitializeIssuingCarrier();

                if (!entityPM.IsHybrid)
                {
                    this.InitializeHouseField();
                }

                entityPM.BasicFreightId = entityPM.FreightPrepaidCollectId;
                entityPM.DestinationPortChargesId = entityPM.OtherPrepaidCollectId;
                entityPM.DestinationHaulageChargesId = entityPM.OtherPrepaidCollectId;
                entityPM.AdditionalChargesId = entityPM.OtherPrepaidCollectId;

                if (entityPM.FreightPrepaidCollectId == "P")
                {
                    if (entityPM.ShipperId != null)
                    {
                        entityPM.FreightPayerId = entityPM.ShipperId;
                        entityPM.FreightPayerAddressId = entityPM.ShipperAddressId;

                        if (entityPM.FreightPayerAddressId == null)
                        {
                            Address address = myAddressRepository.GetSingleAddressByCardIdAndTypeId(entityPM.ShipperId, "M", entityPM.Tenant);
                            if (address != null)
                            {
                                entityPM.FreightPayerAddressId = address.Id;
                            }
                        }
                    }
                }

                else if (entityPM.FreightPrepaidCollectId == "C")
                {
                    if (entityPM.AgentId != null)
                    {
                        entityPM.FreightPayerId = entityPM.AgentId;
                        entityPM.FreightPayerAddressId = entityPM.AgentAddressId;

                        if (entityPM.FreightPayerAddressId == null)
                        {
                            Address address = myAddressRepository.GetSingleAddressByCardIdAndTypeId(entityPM.AgentId, "M", entityPM.Tenant);
                            if (address != null)
                            {
                                entityPM.FreightPayerAddressId = address.Id;
                            }
                        }
                    }
                }
                IntializeWarehouseStorageFreeDays();
                #endregion
            }

            else
            {
                #region
                if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
                {
                    //if (entityPM.ConvertShipmentToLCL || entityPM.ConvertShipmentToFCL)
                    //{
                    //    foreach (ShipmentPackagePM pm in entityPM.ShipmentPackages)
                    //    {
                    //        this.DeleteShipmentPackage(pm);
                    //    }

                    //    foreach (ShipmentOrderPackagePM pm in entityPM.ShipmentOrderPackages)
                    //    {
                    //        this.DeleteShipmentOrderPackage(pm);
                    //    }

                    //    entityPM.BookingVolume = null;
                    //    entityPM.BookingNumberOfPackages = null;
                    //    entityPM.OrderChargeableWeight = null;
                    //    entityPM.OrderGrossWeight = null;
                    //    entityPM.OrderVolumetricWeight = null;
                    //    entityPM.TEU = null;
                    //    entityPM.NumberOfPackages = null;
                    //    entityPM.NumberOfContainers = null;
                    //    entityPM.GrossWeight = null;
                    //    entityPM.ChargeableWeight = null;
                    //    entityPM.VolumetricWeight = null;
                    //    entityPM.Volume = null;

                    //    if (entityPM.ConvertShipmentToLCL)
                    //    {
                    //        entityPM.ShipmentTypeId = "LCLD";
                    //    }

                    //    else if (entityPM.ConvertShipmentToFCL)
                    //    {
                    //        entityPM.ShipmentTypeId = "FCLD";
                    //    }
                    //}

                    if (entityPM.ConvertFromDirectToHouse)
                    {
                        #region
                        entityPM.ShipmentLevelCode = "H";
                        entityPM.MasterShipmentDataId = null;
                        entityPoco.ComputedShipmentNumber = null;
                        entityPM.ComputedShipmentNumber = null;
                        entityPoco.MasterShipmentDataId = null;

                        if (entityMasterData == null)
                        {
                            entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPM.Id);
                        }

                        if (entityMasterData != null)
                        {
                            shipmentMasterDataRepository.Remove(entityMasterData);
                        }

                        if (!entityPM.IsHybrid)
                        {
                            this.InitializeHouseField();
                        }
                        #endregion
                    }

                    else if (entityPM.ConvertFromHouseToDirect)
                    {
                        #region
                        entityPM.ShipmentLevelCode = "D";

                        if (entityPM.IsHybrid)
                        {
                            entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPM.Id);
                            if (entityMasterData == null)
                            {
                                entityMasterData = new ShipmentMasterData() { Id = entityPM.Id, Tenant = tenant, MasterShipmentNumber = entityPM.ShipmentNumber, };
                                shipmentMasterDataRepository.Add(entityMasterData);
                            }

                            entityPM.MasterShipmentDataId = entityPM.Id;
                            entityPoco.MasterShipmentDataId = entityPM.Id;
                        }

                        else
                        {
                            entityMasterData = new ShipmentMasterData() { Id = entityPM.Id, Tenant = tenant, MasterShipmentNumber = entityPM.ShipmentNumber, };
                            entityPM.MasterShipmentDataId = entityPM.Id;
                            entityPoco.ComputedShipmentNumber = entityPM.ShipmentNumber;
                            entityPM.ComputedShipmentNumber = entityPM.ShipmentNumber;
                            entityPoco.MasterShipmentDataId = entityPM.Id;

                            shipmentMasterDataRepository.Add(entityMasterData);

                        }

                        if (!string.IsNullOrEmpty(entityPM.House))
                        {
                            if (entityPM.DirectionId == "E" || entityPM.DirectionId == "D" || entityPM.DirectionId == "R")
                            {
                                TenantSettingRepository tenantSettingsRepository = new TenantSettingRepository(tenant);
                                tenantSettingQuery = new TenantSettingQuery(tenantSettingsRepository);
                                string settingsCode = "HAWBCounter" + entityPM.TransportModeId + "_E_D";
                                TenantSettingPM tenantSettingPM = tenantSettingQuery.GetTenantSettingsByCode("Shipment", settingsCode, tenant);

                                if (tenantSettingPM != null)
                                {
                                    if (tenantSettingPM.SettingValue == "Stock")
                                    {
                                        ReturnFBLStock();
                                    }
                                    else if (tenantSettingPM.DontIncludeDirects)
                                    {
                                        entityPM.House = null;
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    else
                    {
                        TenantSettingRepository tenantSettingsRepository = new TenantSettingRepository(tenant);
                        tenantSettingQuery = new TenantSettingQuery(tenantSettingsRepository);
                        string settingsCode = "HAWBCounter" + entityPM.TransportModeId + "_E_D";
                        TenantSettingPM tenantSettingPM = tenantSettingQuery.GetTenantSettingsByCode("Shipment", settingsCode, tenant);

                        if (tenantSettingPM != null && tenantSettingPM.SettingValue == "Stock")
                        {
                            this.InitializeFBLStock();
                        }
                    }
                }

                bool IsImporterApprovalRequiredOldValue = false;
                if (entityPM.IsHybrid || loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                {
                    shipmentAdditionalCloudData = shipmentAdditionalCloudDataRepository.GetSingleShipmentAdditionalCloudData(entityPM.Id, entityPM.Tenant);
                    if (shipmentAdditionalCloudData != null)
                    {
                        IsImporterApprovalRequiredOldValue = shipmentAdditionalCloudData.IsImporterApprovalRequried;
                        if (loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                        {
                            shipmentAdditionalCloudData.IsImporterApprovalRequried = entityPM.IsImporterApprovalRequired;
                        }
                        if (entityPM.UpdateSendUpdatesToAgentEnabledField)
                        {
                            shipmentAdditionalCloudData.SendUpdatesToAgentEnabled = entityPM.SendUpdatesToAgentEnabled;
                        }
                        if (entityPM.ApproveDateTime != null)
                        {
                            shipmentAdditionalCloudData.ApproveDateTime = entityPM.ApproveDateTime;
                        }

                        //if (!string.IsNullOrEmpty(entityPM.DeclarationXMLData))
                        //{
                        //    shipmentAdditionalCloudData.DeclarationXmlData = entityPM.DeclarationXMLData;
                        //}
                        if (!string.IsNullOrEmpty(entityPM.ApprovedBy))
                        {
                            shipmentAdditionalCloudData.ApprovedByUserName = entityPM.ApprovedBy;
                        }
                        if (!string.IsNullOrEmpty(entityPM.DocumentsApprovedByUserName))
                        {
                            shipmentAdditionalCloudData.DocumentsApprovedByUserName = entityPM.DocumentsApprovedByUserName;
                        }

                        if (entityPM.DeclarationXMLData != shipmentAdditionalCloudData.DeclarationXmlData && !string.IsNullOrEmpty(entityPM.DeclarationXMLData))
                        {
                            //var tempShipmentAdditionalCloudData = shipmentAdditionalCloudDataRepository.GetSingleShipmentAdditionalCloudData(entityPM.Id, entityPM.Tenant);
                            if (loggedTenant.LogBoxTenantSetting.IsDocumentsArchive && (!IsImporterApprovalRequiredOldValue && entityPM.IsImporterApprovalRequired))
                            {
                                AddImporterApprovalReceivedQueue();
                            }
                            if (!loggedTenant.LogBoxTenantSetting.IsDocumentsArchive && (!IsImporterApprovalRequiredOldValue && entityPM.IsImporterApprovalRequired))
                            {
                                AddImporterApprovalReceivedQueueForCargoTracking();
                            }
                            shipmentAdditionalCloudData.DeclarationXmlData = entityPM.DeclarationXMLData;
                            shipmentAdditionalCloudData.IsImporterApprovalRequried = entityPM.IsImporterApprovalRequired;
                            ClearApprovalDenialFields();
                        }
                        else if (entityPM.IsShipmentAdditionalCloudDataChange)
                        {
                            shipmentAdditionalCloudData.DeclarationXmlData = entityPM.DeclarationXMLData;
                            shipmentAdditionalCloudData.ApprovedByUserName = null;
                            shipmentAdditionalCloudData.DenyReason = null;
                            shipmentAdditionalCloudData.ApproveDateTime = null;
                            //if (loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                            //{
                            shipmentAdditionalCloudData.IsImporterApprovalRequried = entityPM.IsImporterApprovalRequired;
                            //}

                        }

                        if (entityPM.DeclarationWCOXml != shipmentAdditionalCloudData.DeclarationWCOXml && !string.IsNullOrEmpty(entityPM.DeclarationWCOXml))
                        {
                            shipmentAdditionalCloudData.DeclarationWCOXml = entityPM.DeclarationWCOXml;
                            shipmentAdditionalCloudData.DeclarationXmlData = null;
                        }


                        if (!string.IsNullOrEmpty(entityPM.VersionApproved))
                        {
                            shipmentAdditionalCloudData.VersionApproved = entityPM.VersionApproved;
                        }
                        if (entityPM.ShipmentAdditionalData != null)
                        {
                            shipmentAdditionalCloudData.ShipmentAddtionalDataXML = ShipmentAdditionalDataService.SerializeShipmentAdditionalXmlData(entityPM.ShipmentAdditionalData);
                        }

                        if (!string.IsNullOrEmpty(entityPM.PaymentRequestXML) && shipmentAdditionalCloudData.PaymentRequestXML != entityPM.PaymentRequestXML)
                        {
                            shipmentAdditionalCloudData.IsPaymentRequired = true;
                            shipmentAdditionalCloudData.PaymentRequestXML = entityPM.PaymentRequestXML;
                            shipmentAdditionalCloudData.PaymentDateTime = entityPM.PaymentDateTime;
                            AddPaymentReceivedToQueue();
                        }
                        AddVIRExternalTaskQueue();
                        if (!string.IsNullOrEmpty(entityPM.UserIdNumberXMLData))
                        {
                            shipmentAdditionalCloudData.IsUserIDNumberRequired = entityPM.IsUserIDNumberRequired;
                            shipmentAdditionalCloudData.UserIdNumberXMLData = entityPM.UserIdNumberXMLData;
                        }
                        shipmentAdditionalCloudDataRepository.Update(shipmentAdditionalCloudData);
                    }
                }
                #endregion
            }

            shipmentTracing = new ShipmentTracing(entityPM, entityPoco, entityMasterData, loggedContact.Id, isNewEntity);

            if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
            {
                this.InitializeFCL_LCL();
                this.InitializeClosingFields();
                this.InitializeStatus();
                this.InitializeInlandDomestic();
                this.InitializeAWBFields();

                if (!loggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
                {
                    this.InitializeMAWBStack();
                }
                this.InitializeNumberOfInsidePackages();
                this.InitializeCarrierPrefix();
                this.InitializeKnownConsignor();
                this.InitializePrintingFields();
                this.ComputeTEU();
                this.FillDefaultSubType();

                ShipmentFinalArrivalDateBehaviour behaviour = new ShipmentFinalArrivalDateBehaviour(this.entityPM, this.objectContext, isNewEntity);
                behaviour.Handle();
                this.initializer.IsUpdatingHousesFinalArrivalDate = behaviour.IsUpdatingHouses;
            }
        }

        private void AddImporterApprovalReceivedQueueForCargoTracking()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("CargoTrackingImporterApprovalReceivedQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, }, tenant, null, null);
        }

        private void ClearApprovalDenialFields()
        {
            if (!shipmentAdditionalCloudData.IsImporterApprovalRequried)
            {
                shipmentAdditionalCloudData.ApprovedByUserName = null;
                shipmentAdditionalCloudData.ApproveDateTime = null;
                shipmentAdditionalCloudData.DenyReason = null;
                shipmentAdditionalCloudData.VersionApproved = null;
            }
        }

        private void FillDefaultSubType()
        {
            if (string.IsNullOrEmpty(entityPM.ShipmentSubTypeId) || initializer.IsUpdatingSubType)
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

                    else if (entityPM.ShipmentTypeId == "LTL")
                    {
                        code = "LTL";
                    }

                    else if (entityPM.ShipmentTypeId == "MyGI")
                    {
                        code = "MyGI";
                    }
                }

                else if (entityPM.TransportModeId == "O")
                {
                    if (entityPM.ShipmentTypeId == "FCLD")
                    {
                        code = "FCL";
                    }

                    else if (entityPM.ShipmentTypeId == "LCLD")
                    {
                        code = "LCL";
                    }

                    else if (entityPM.ShipmentTypeId == "MyGO")
                    {
                        code = "MyGO";
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

        private void IntializeWarehouseStorageFreeDays()
        {
            Card consigneeCard = CardRepository.GetSingleCard(entityPM.ConsigneeId, tenant, true);
            entityPM.WarehouseStorageFreeDays = consigneeCard != null ? consigneeCard.StorageFreeDays : null;
        }

        private void AddImporterApprovalReceivedQueue()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ImporterApprovalReceivedQueue", 0);
            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, }, tenant, null, null);
        }

        private void AddPaymentReceivedToQueue()
        {
            if (LogitudeSettings.EnableHybridQueue && (CurrentHybridPartner != null && !CurrentHybridPartner.IsExternalPartner) )
            {
                //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                //{
                ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
                DocumentRepository documentrepository = new DocumentRepository(commonContext);
                ObjectTableRepository objecttableRep = new ObjectTableRepository(entityPM.Tenant);
                ObjectTable objectTable = null;

                objectTable = objecttableRep.GetObjectTableByName("Shipment", 0, true);

                List<QueueTask> tasks = new List<QueueTask>();
                tasks.Add(new QueueTask()
                {
                    Action = "StatusUpdate",
                    Parameters = new List<Logitude.Server.Tools.Parameter>() {
                new Logitude.Server.Tools.Parameter { Name = "ShipmentNumber", Value = entityPM.ShipmentNumber},
                new Logitude.Server.Tools.Parameter { Name = "Code", Value = "VPR"},
                new Logitude.Server.Tools.Parameter { Name = "Direction", Value = entityPM.DirectionId},

                }
                });




                var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = ByteData.Length,
                    Tenant = Convert.ToInt32(entityPM.Tenant),
                    Id = IdCounter.GetNumber("Document", entityPM.Tenant),
                    HasFile = true,
                    Folder = "ExternalTasksQueue",
                };
                documentrepository.Add(document);
                documentrepository.SubmitChanges();
                var commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", entityPM.Tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                    InOut = "O",
                    ObjectTableId = (objectTable != null && !string.IsNullOrEmpty(objectTable.Id)) ? objectTable.Id : null,
                    Subject = "Status Update",
                    Tenant = entityPM.Tenant,
                    CommunicationLogTypeCode = "Q",
                    CommunicationStatusTypeCode = "W",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                    DocumentId = document.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    LastStatusDateUTC = DateTime.UtcNow,
                    QueueName = "externaltasksqueue" + entityPM.Tenant + 1,
                    Priority = 1,
                    EntityReference = entityPM.ShipmentNumber

                };

                communicationLogRepository.Add(commLog);
                communicationLogRepository.SubmitChanges();
                string filename = document.Id + "." + document.Extension;
                string filePath = "tenant" + commLog.Tenant + "/" + StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = entityPM.Tenant,
                    FileSize = ByteData.Length,

                };

                storageservice.Write(ByteData, fileInfo);
                Communications.SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, commLog.Tenant);

                //    scope.Complete();
                //}
            }
        }

        private void InitializeFCL_LCL()
        {
            if (this.isNewEntity)
            {
                foreach (ShipmentPackagePM itemPM in this.entityPM.ShipmentPackages)
                {
                    if (itemPM.TemperatureUnitCode == null)
                    {
                        itemPM.TemperatureUnitCode = loggedTenant.TemperatureUnitCode;

                        if (itemPM.TemperatureUnitCode == null)
                        {
                            itemPM.TemperatureUnitCode = "CEL";
                        }
                    }

                    if (itemPM.FlashPointTemperatureUnitCode == null)
                    {
                        itemPM.FlashPointTemperatureUnitCode = loggedTenant.TemperatureUnitCode;

                        if (itemPM.FlashPointTemperatureUnitCode == null)
                        {
                            itemPM.FlashPointTemperatureUnitCode = "CEL";
                        }
                    }
                }
            }
        }

        private void ReturnFBLStock(bool throwNotFoundException = false)
        {
            FBLStockRepository fBLStockRepository = new FBLStockRepository(tenant);
            FBLStock stack = fBLStockRepository.GetSingleFBLStockByNumber(int.Parse(entityPM.FBLStockNumber), tenant);
            if (stack != null)
            {
                entityPM.FBLStockNumber = null;
                entityPM.FBLReturnedToStock = false;

                entityPM.FBLIsFromStock = false;

                if (!entityPM.FBLReturnedToStockWithCancel)
                {
                    entityPM.House = null;
                }


                //entityPM.IsAddingStackEvents = true;

                stack.Notes = "Returned from shipment";
                stack.IsUsed = false;
                stack.InsertionDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                fBLStockRepository.Update(stack);
                fBLStockRepository.SubmitChanges();
            }
            else if (throwNotFoundException)
            {
                string msg = TranslateTextsClass.Translate("Shipment.M.ThisFBLStockNotExists", tenant);
                throw new ApplicationException(msg);
            }
        }
        private void InitializeStatus()
        {
            if (this.isNewEntity)
            {
                if (entityPM.StatusId == null)
                {
                    EntityStatusRepository entityStatusRep = new EntityStatusRepository(tenant);
                    EntityStatus myStatus = entityStatusRep.GetSingleEntityStatusByCode("SHOR", tenant);
                    entityPM.StatusId = myStatus.Id;
                }
            }
        }
        private void InitializeBookingData()
        {
            if (!string.IsNullOrEmpty(entityPM.BookingId))
            {
                BookingRepository bookingRepository = new BookingRepository(tenant);
                Booking booking = bookingRepository.GetSingle(entityPM.BookingId, tenant);

                if (isNewEntity)
                {
                    if (booking != null)
                    {
                        booking.ShipmentId = entityPM.Id;
                        booking.BookingStatusCode = "AWB";
                    }
                }

                else
                {
                    if (booking != null)
                    {
                        booking.ShipmentId = null;
                        booking.BookingStatusCode = "CNF";
                    }
                }

                bookingRepository.Update(booking);
                bookingRepository.SubmitChanges();
            }
        }
        private void InitializeOCIs()
        {
            if (isNewEntity && entityPM.IsCopyFromShipment)
            {
                if (!string.IsNullOrEmpty(entityPM.CopyFromShipmentId))
                {
                    if (entityPM.AWBOCIPMs.Count == 0)
                    {
                        List<AWBOCI> list = aWBOCIRepository.GetAWBOCIsbyShipmentId(entityPM.CopyFromShipmentId, tenant).ToList();
                        foreach (AWBOCI item in list)
                        {
                            AWBOCI newOCI = new AWBOCI()
                            {
                                Id = IdCounter.GetNumber("AWBOCI", tenant).ToString(),
                                ShipmentId = entityPM.Id,
                                Tenant = entityPM.Tenant,
                                CountryId = item.CountryId,
                                AWBInformationCode = item.AWBInformationCode,
                                AWBCustomsInformationCode = item.AWBCustomsInformationCode,
                                SupplementaryCustomsInfo = item.SupplementaryCustomsInfo,
                            };

                            aWBOCIRepository.Add(newOCI);
                        }
                    }
                }
            }
        }
        private void InitializeCarrierPrefix()
        {
            if (entityPM.TransportModeId == "A")
            {
                if (string.IsNullOrEmpty(entityPM.AirlinePrefix))
                {
                    if (!string.IsNullOrEmpty(entityPM.InterlineId))
                    {
                        //AirlineRepository airlineRepository = new AirlineRepository(tenant);
                        //Airline myAirline = airlineRepository.GetSingleAirline(entityPM.InterlineId, tenant);
                        //if (myAirline != null)
                        //{
                        //    if (!string.IsNullOrEmpty(myAirline.Prefix))
                        //    {
                        //        entityPM.AirlinePrefix = myAirline.Prefix.PadLeft(3, '0');
                        //    }
                        //}
                    }

                    else if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
                    {
                        AirlineRepository airlineRepository = new AirlineRepository(tenant);
                        Airline myAirline = airlineRepository.GetSingleAirline(entityPM.MainCarriageCarrierId, tenant);
                        if (myAirline != null)
                        {
                            if (!string.IsNullOrEmpty(myAirline.Prefix))
                            {
                                entityPM.AirlinePrefix = myAirline.Prefix.PadLeft(3, '0');
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierPrefix))
                    {
                        Card card = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, entityPM.Tenant, true);

                        if (card != null)
                        {
                            entityPM.MainCarriageCarrierPrefix = card.Code;
                        }
                    }
                }
            }
        }
        private void InitializeInlandDomestic()
        {
            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                if (isNewEntity)
                {
                    //if (string.IsNullOrEmpty(entityPM.MainCarriageFromPartnerId))
                    //{
                    //    entityPM.MainCarriageFromPartnerId = entityPM.ShipperId;
                    //}

                    //if (string.IsNullOrEmpty(entityPM.MainCarriageFromAddressId))
                    //{
                    //    entityPM.MainCarriageFromAddressId = entityPM.ShipperAddressId;
                    //}

                    //if (string.IsNullOrEmpty(entityPM.MainCarriageToPartnerId))
                    //{
                    //    entityPM.MainCarriageToPartnerId = entityPM.ConsigneeId;
                    //}

                    //if (string.IsNullOrEmpty(entityPM.MainCarriageToAddressId))
                    //{
                    //    entityPM.MainCarriageToAddressId = entityPM.ConsigneeAddressId;
                    //}
                }

                //entityPM.FromPortId = null;
                //entityPM.ToPortId = null;
                //entityPM.MainCarriageFromPortId = null;
                //entityPM.MainCarriageToPortId = null;
                //entityPM.MainCarriageFinalDestinationPortId = null;
                entityPM.ShipmentLevelCode = "D";
            }
        }
        private void InitializeAWBFields()
        {
            if (string.IsNullOrEmpty(entityPM.AWBDeclaredValueForCarriage))
            {
                entityPM.AWBDeclaredValueForCarriage = "NVD";
            }

            if (string.IsNullOrEmpty(entityPM.AWBDeclaredValueForCustoms))
            {
                entityPM.AWBDeclaredValueForCustoms = "NCV";
            }

            if (string.IsNullOrEmpty(entityPM.AWBInsurrenceValue))
            {
                entityPM.AWBInsurrenceValue = "XXX";
            }

            if (string.IsNullOrEmpty(entityPM.AWBSignature))
            {
                if (!string.IsNullOrEmpty(entityPM.BranchId))
                {
                    Branch myBranch = (from d in myCommonContext.Branches
                                       where d.Tenant == tenant
                                       && d.Id == entityPM.BranchId
                                       select d).FirstOrDefault();

                    if (myBranch != null)
                    {
                        entityPM.AWBSignature = myBranch.Signature;
                    }
                }


                if (string.IsNullOrEmpty(entityPM.AWBSignature))
                {
                    entityPM.AWBSignature = loggedTenant.Signature;
                }
            }

            if (string.IsNullOrEmpty(entityPM.AWBChargesCodeCode))
            {
                entityPM.AWBChargesCodeCode = "PC";
                if (entityPM.FreightPrepaidCollectId == "P" && entityPM.OtherPrepaidCollectId == "P")
                {
                    entityPM.AWBChargesCodeCode = "PP";
                }

                else if (entityPM.FreightPrepaidCollectId == "C" && entityPM.OtherPrepaidCollectId == "C")
                {
                    entityPM.AWBChargesCodeCode = "CC";
                }
            }

            PortPM portFrom = PortQuery.GetSinglePort(tenant, entityPM.MainCarriageFromPortId, true);
            PortPM portTo = PortQuery.GetSinglePort(tenant, entityPM.MainCarriageFinalDestinationPortId, true);

            // Place
            if (string.IsNullOrEmpty(entityPM.AWBPlace))
            {
                string fromPortName = "";
                string fromCountryName = "";

                if (portFrom != null)
                {
                    fromPortName = portFrom.EnglishName;
                    fromCountryName = portFrom.CountryName;
                }

                else
                {
                    fromPortName = entityPM.MainCarriageFromPortName;
                    fromCountryName = entityPM.MainCarriageFromPortCountryName;
                }

                string result = string.IsNullOrEmpty(fromCountryName) ? fromPortName : fromPortName + " " + fromCountryName;
                entityPM.AWBPlace = result;
            }

            if (entityPM.IsMultipleCommodities)
            {
                entityPM.RateClassCode = null;
                entityPM.AWBChargeRate = null;
            }

            else
            {
                if (entityPM.TransportModeId == "A")
                {
                    this.ComputeFreightChargesFromFreightPayableLine();

                    if (string.IsNullOrEmpty(entityPM.RateClassCode))
                    {
                        entityPM.RateClassCode = "Q";
                    }

                    if (entityPM.ShipmentCommodities.Count == 0)
                    {
                        entityPM.ShipmentCommodities.Add(new ShipmentCommodityPM()
                        {
                            Id = IdCounter.GetNumber("ShipmentCommodity", tenant).ToString(),
                            Tenant = tenant,
                            ShipmentId = entityPM.Id,
                            ChargeableWeight = entityPM.ChargeableWeight,
                            ChargeAmount = entityPM.AWBChargeAmount,
                            ChargeRate = entityPM.AWBChargeRate,
                            CommodityNumber = entityPM.AWBCommodityItemNumber,
                            GrossWeight = entityPM.GrossWeight,
                            DescriptionOfGoods = entityPM.DescriptionOfGoods,
                            NumberOfPackages = entityPM.NumberOfPackages,
                            RateClassCode = entityPM.RateClassCode,
                            Volume = entityPM.Volume,
                            VolumetricWeight = entityPM.VolumetricWeight,
                            IsFirstLine = true,
                        });
                    }

                    else
                    {
                        ShipmentCommodityPM myShipmentCommodity = entityPM.ShipmentCommodities.Where(d => d.IsFirstLine).FirstOrDefault();

                        if (myShipmentCommodity != null)
                        {
                            bool isUpdatingSingleCommodity = false;

                            if (myShipmentCommodity.ChargeableWeight != entityPM.ChargeableWeight)
                            {
                                myShipmentCommodity.ChargeableWeight = entityPM.ChargeableWeight;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.ChargeAmount != entityPM.AWBChargeAmount)
                            {
                                myShipmentCommodity.ChargeAmount = entityPM.AWBChargeAmount;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.ChargeRate != entityPM.AWBChargeRate)
                            {
                                myShipmentCommodity.ChargeRate = entityPM.AWBChargeRate;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.CommodityNumber != entityPM.AWBCommodityItemNumber)
                            {
                                myShipmentCommodity.CommodityNumber = entityPM.AWBCommodityItemNumber;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.GrossWeight != entityPM.GrossWeight)
                            {
                                myShipmentCommodity.GrossWeight = entityPM.GrossWeight;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.DescriptionOfGoods != entityPM.DescriptionOfGoods)
                            {
                                myShipmentCommodity.DescriptionOfGoods = entityPM.DescriptionOfGoods;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.NumberOfPackages != entityPM.NumberOfPackages)
                            {
                                myShipmentCommodity.NumberOfPackages = entityPM.NumberOfPackages;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.RateClassCode != entityPM.RateClassCode)
                            {
                                myShipmentCommodity.RateClassCode = entityPM.RateClassCode;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.Volume != entityPM.Volume)
                            {
                                myShipmentCommodity.Volume = entityPM.Volume;
                                isUpdatingSingleCommodity = true;
                            }

                            if (myShipmentCommodity.VolumetricWeight != entityPM.VolumetricWeight)
                            {
                                myShipmentCommodity.VolumetricWeight = entityPM.VolumetricWeight;
                                isUpdatingSingleCommodity = true;
                            }

                            if (isUpdatingSingleCommodity)
                            {
                                this.UpdateShipmentCommodity(myShipmentCommodity);
                            }
                        }
                    }
                }
            }
        }

        private void ComputeFreightChargesFromFreightPayableLine()
        {
            if (entityPM.AWBChargeRate == null)
            {
                var airFreightCode = "AFT";
                var airFreightCharge = entityPM.ShipmentPayables.Where(a => a.ChargesTypeCode == airFreightCode).FirstOrDefault();
                if (airFreightCharge != null && ValidateCurrencyOfShipmentAWBPrintOnlies(airFreightCharge))
                {
                    this.SetAWBFreightChargeFields(airFreightCharge);
                }
            }
        }

        private bool ValidateCurrencyOfShipmentAWBPrintOnlies(ShipmentPayablePM airFreightCharge)
        {
            var isValid = true;
            if (entityPM.ShipmentAWBPrintOnlies != null)
            {
                foreach (var item in entityPM.ShipmentAWBPrintOnlies)
                {
                    if (item.CurrencyId != airFreightCharge.CurrencyId)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }

        private void SetAWBFreightChargeFields(ShipmentPayablePM airFreightCharge)
        {
            entityPM.AWBChargeRate = airFreightCharge.UnitPrice;
            entityPM.AWBCurrencyId = airFreightCharge.CurrencyId;
            entityPM.AWBChargeAmount = this.SetAWBChargeAmount();
            if (!string.IsNullOrEmpty(airFreightCharge.PrepaidCollectId))
            {
                entityPM.FreightPrepaidCollectId = airFreightCharge.PrepaidCollectId;
                entityPM.AWBChargesCodeCode = this.SetAWBChargesCodeCode();
                SetAWBFrieghtAmountCollectAndPrepaid();
            }
        }

        public string SetAWBChargesCodeCode()
        {
            if (entityPM.FreightPrepaidCollectId == "P" && entityPM.OtherPrepaidCollectId == "P")
            {
                return "PP";
            }

            else if (entityPM.FreightPrepaidCollectId == "C" && entityPM.OtherPrepaidCollectId == "C")
            {
                return "CC";
            }

            else
            {
                return "PC";
            }
        }
        public void SetAWBFrieghtAmountCollectAndPrepaid()
        {
            var computedAmount = entityPM.AWBChargeAmount;
            var totaAmount = entityPM.AWBFreightAmountPrepaid + entityPM.AWBFreightAmountCollect;

            var recomputeAmounts = true;
            var isPrepaidHasAmount = (entityPM.AWBFreightAmountPrepaid != 0 && entityPM.AWBFreightAmountPrepaid != null);
            var isCollectHasAmount = (entityPM.AWBFreightAmountCollect != 0 && entityPM.AWBFreightAmountCollect != null);

            if (!string.IsNullOrEmpty(entityPM.FreightPrepaidCollectId))
            {
                if (isPrepaidHasAmount && isCollectHasAmount && (computedAmount == totaAmount))
                {
                    recomputeAmounts = false;
                }
            }

            if (recomputeAmounts)
            {
                RecomputeAWBFrieghtAmountCollectAndPrepaid(computedAmount);
            }
        }
        public void RecomputeAWBFrieghtAmountCollectAndPrepaid(double? computedAmount)
        {

            if (entityPM.FreightPrepaidCollectId == "P")
            {
                entityPM.AWBFreightAmountCollect = 0;
                entityPM.AWBFreightAmountPrepaid = computedAmount == null ? 0 : computedAmount;
            }

            else if (entityPM.FreightPrepaidCollectId == "C")
            {
                entityPM.AWBFreightAmountPrepaid = 0;
                entityPM.AWBFreightAmountCollect = computedAmount == null ? 0 : computedAmount;
            }
        }

        public double? SetAWBChargeAmount()
        {
            double? myResult = null;
            var myRateClassCode = entityPM.RateClassCode;
            var myChargeRate = entityPM.AWBChargeRate;
            var chargeAmount = entityPM.ChargeableWeight;
            var groupCode = this.GetRateClassGroupCode(myRateClassCode);
            if (entityPM.RateClassCode == "K")
            {
                chargeAmount = entityPM.ChargeableWeightInKG;
            }

            if (groupCode == "M")
            {
                myResult = myChargeRate;
            }
            else if (groupCode == "R")
            {
                myResult = myChargeRate * chargeAmount;
            }
            return myResult;
        }

        public string GetRateClassGroupCode(string rateClassCode)
        {
            var code = "";
            switch (rateClassCode)
            {
                case "M":
                case "B":
                    {
                        code = "M";
                        break;
                    }

                case "R":
                case "X":
                case "Y":
                    {
                        code = "S";
                        break;
                    }

                case "C":
                case "E":
                case "K":
                case "N":
                case "P":
                case "Q":
                case "U":
                case "S":
                    {
                        code = "R";
                        break;
                    }

                default: { break; }
            }
            return code;
        }

        private void InitializeHouseField()
        {
            if (string.IsNullOrEmpty(entityPM.House))
            {
                if (entityPM.DirectionId == "E" || entityPM.DirectionId == "D" || entityPM.DirectionId == "R")
                {
                    TenantSettingRepository tenantSettingsRepository = new TenantSettingRepository(tenant);
                    tenantSettingQuery = new TenantSettingQuery(tenantSettingsRepository);
                    string settingsCode = "HAWBCounter" + entityPM.TransportModeId + "_E_D";
                    TenantSettingPM settings = tenantSettingQuery.GetTenantSettingsByCode("Shipment", settingsCode, tenant);

                    if (settings != null)
                    {
                        if (settings.SettingValue == "Stock")
                        {
                            this.InitializeFBLStock();
                        }
                        else
                        {
                            bool getHouseField = false;

                            if (entityPM.ShipmentLevelCode == "H")
                            {
                                getHouseField = true;
                            }

                            else if (entityPM.ShipmentLevelCode == "D")
                            {
                                if (!settings.DontIncludeDirects)
                                {
                                    getHouseField = true;
                                }
                            }

                            if (getHouseField)
                            {
                                string house = !string.IsNullOrEmpty(entityPM.House) ? entityPM.House : "";

                                if (settings.SettingValue == "Shipment")
                                {
                                    string shipPrefix = TableCounter.GetCounterPrefix(tenant, "SHIP", entityPM.DirectionId, entityPM.TransportModeId);

                                    house = entityPM.ShipmentNumber;

                                    if (!String.IsNullOrEmpty(shipPrefix))
                                    {
                                        if (!string.IsNullOrEmpty(house))
                                        {
                                            if (house.Contains(shipPrefix))
                                            {
                                                house = house.Remove(0, shipPrefix.Length);
                                            }
                                        }
                                    }
                                }

                                if (settings.SettingValue == "Counter")
                                {
                                    house = TableCounter.GetNumber(tenant, "HAWB", entityPM.TransportModeId, null);
                                }

                                if (!String.IsNullOrEmpty(house))
                                {
                                    house = house.ToString().PadLeft(settings.Size, '0');
                                }

                                if (settings.SettingValue != "None")
                                {
                                    if (!String.IsNullOrEmpty(settings.Prefix))
                                    {
                                        house = settings.Prefix + house;
                                    }
                                }

                                entityPM.House = house;
                            }
                        }
                    }
                }
            }
        }
        private void InitializeMAWBStack()
        {
            if (entityPM.TransportModeId == "A")
            {
                if (entityMasterData != null)
                {
                    ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                    MAWBStackRepository mawStackRepository = new MAWBStackRepository(commonContext);

                    if (string.IsNullOrEmpty(entityPM.BookingId))
                    {
                        if (!string.IsNullOrEmpty(entityPM.Master))
                        {
                            if (!Regex.IsMatch(entityPM.Master, "^[0-9]*$"))
                            {
                                throw new ApplicationException("Master Field must be all digits");
                            }

                            else
                            {
                                if (entityPM.CarrierIsLimitedLength && entityPM.Master.Length != 8)
                                {
                                    throw new ApplicationException("Master Field length must be 8 digits");
                                }
                                else
                                {
                                    if (entityPM.CarrierIsCheckDigit)
                                    {
                                        string myPrefix = entityPM.Master.Substring(0, 7);
                                        string myCheckDegit = entityPM.Master.Substring(7, 1);

                                        int myPrefixInteger = 0;

                                        int.TryParse(myPrefix, out myPrefixInteger);

                                        int myMod = myPrefixInteger % 7;

                                        if (myMod >= 7)
                                        {
                                            myMod = myMod % 7;
                                        }

                                        if (myMod.ToString() != myCheckDegit)
                                        {
                                            throw new ApplicationException("Master Field invalid check digit");
                                        }
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(entityPM.MAWBStackAirlineId))
                            {
                                if (entityPM.MAWBTakenFromStack == false && entityPM.MainCarriageIsFromStack == false)
                                {
                                    string myAirlineId = entityPM.MainCarriageCarrierId;
                                    if (!string.IsNullOrEmpty(entityPM.InterlineId))
                                    {
                                        myAirlineId = entityPM.InterlineId;
                                    }

                                    if (!string.IsNullOrEmpty(myAirlineId))
                                    {
                                        MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(long.Parse(entityPM.Master), entityPM.Tenant, myAirlineId);
                                        if (stack != null)
                                        {
                                            string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackFoundInStack", entityPM.Tenant);
                                            throw new ApplicationException(msg);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (entityPM.MAWBTakenFromStack)
                    {
                        if (entityPM.MAWBStackNumber != null)
                        {
                            this.SetMAWBAirline();

                            MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(int.Parse(entityPM.MAWBStackNumber), tenant, entityPM.MAWBStackAirlineId);
                            if (stack != null)
                            {
                                entityPM.MAWBStackNumber = null;
                                entityPM.MAWBStackAirlineId = null;
                                entityPM.MAWBTakenFromStack = false;

                                entityPM.MainCarriageIsFromStack = true;
                                entityPM.Master = stack.Number.ToString().PadLeft(8, '0');

                                entityPM.StackAirlineId = stack.AirlineId;
                                entityPM.IsRemovingStackEvents = true;

                                stack.IsUsed = true;
                                mawStackRepository.Update(stack);
                                mawStackRepository.SubmitChanges();
                            }

                            else
                            {
                                string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackNotExists", tenant);
                                throw new ApplicationException(msg);
                            }
                        }
                    }

                    if (entityPM.MAWBReturnedToStack)
                    {
                        if (entityPM.MAWBStackNumber != null)
                        {
                            this.SetMAWBAirline();

                            MAWBStack stack = mawStackRepository.GetSingleMAWBStackByNumberAirline(int.Parse(entityPM.MAWBStackNumber), tenant, entityPM.MAWBStackAirlineId);
                            if (stack != null)
                            {
                                entityPM.MAWBStackNumber = null;
                                entityPM.MAWBStackAirlineId = null;
                                entityPM.MAWBReturnedToStack = false;

                                entityPM.MainCarriageIsFromStack = false;

                                if (!entityPM.MAWBReturnedToStackWithCancel)
                                {
                                    entityPM.Master = null;
                                }

                                entityPM.StackAirlineId = stack.AirlineId;
                                entityPM.IsAddingStackEvents = true;

                                stack.Notes = "Returned from shipment";
                                stack.IsUsed = false;
                                stack.InsertionDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                mawStackRepository.Update(stack);
                                mawStackRepository.SubmitChanges();
                            }

                            else
                            {
                                string msg = TranslateTextsClass.Translate("Shipment.M.ThisAirlineMAWBStackNotExists", tenant);
                                throw new ApplicationException(msg);
                            }
                        }
                    }

                }
            }
        }
        private void InitializeFBLStock()
        {
            //ocean export shipments (Direct & House) :
            if (entityPM.TransportModeId == "O" && (entityPM.DirectionId == "E" || entityPM.DirectionId == "D") && entityPM.ShipmentLevelCode != "C")
            {
                FBLStockRepository fBLStockRepository = new FBLStockRepository(this.objectContext);

                if (entityPM.FBLTakenFromStock == false && entityPM.FBLIsFromStock == false && !string.IsNullOrEmpty(entityPM.House))
                {

                    FBLStock stack = fBLStockRepository.GetSingleFBLStockByNumber(long.Parse(entityPM.House), entityPM.Tenant);
                    if (stack != null)
                    {
                        string msg = TranslateTextsClass.Translate("Shipment.M.ThisFBLStockFoundInStack", entityPM.Tenant);
                        throw new ApplicationException(msg);
                    }

                }


                if (entityPM.FBLTakenFromStock)
                {
                    if (entityPM.FBLStockNumber != null)
                    {

                        FBLStock stack = fBLStockRepository.GetSingleFBLStockByNumber(int.Parse(entityPM.FBLStockNumber), tenant);
                        if (stack != null)
                        {
                            entityPM.FBLStockNumber = null;
                            entityPM.FBLTakenFromStock = false;

                            entityPM.FBLIsFromStock = true;
                            entityPM.House = stack.Number.ToString();


                            //entityPM.IsRemovingStackEvents = true;

                            stack.IsUsed = true;
                            fBLStockRepository.Update(stack);
                            fBLStockRepository.SubmitChanges();
                        }

                        else
                        {
                            string msg = TranslateTextsClass.Translate("Shipment.M.ThisFBLStockNotExists", tenant);
                            throw new ApplicationException(msg);
                        }
                    }
                }

                if (entityPM.FBLReturnedToStock)
                {
                    if (entityPM.FBLStockNumber != null)
                    {
                        this.ReturnFBLStock(true);
                    }
                }

                if (entityPM.IsCancelled && entityPM.FBLIsFromStock && !string.IsNullOrEmpty(entityPM.House))
                {
                    entityPM.FBLStockNumber = entityPM.House;
                    this.ReturnFBLStock(false);
                }

            }

        }
        private void SetMAWBAirline()
        {
            entityPM.MAWBStackAirlineId = entityPM.MainCarriageCarrierId;

            if (!string.IsNullOrEmpty(entityPM.InterlineId))
            {
                entityPM.MAWBStackAirlineId = entityPM.InterlineId;
            }
        }
        private void InitializeIssuingCarrier()
        {
            if (isNewEntity)
            {
                if (entityPM.TransportModeId == "A" && (entityPM.DirectionId == "E" || entityPM.DirectionId == "R"))
                {
                    if (!entityPM.ViaColoader)
                    {
                        #region
                        if (!string.IsNullOrEmpty(entityPM.BookingId))
                        {
                            BookingRepository bookingRepository = new BookingRepository(tenant);
                            Booking booking = bookingRepository.GetSingle(entityPM.BookingId, tenant);

                            if (booking != null)
                            {
                                entityPM.IssuingCarrierAgentId = booking.IssuingCarrierAgentId;
                                entityPM.IssuingCarrierAddressId = booking.IssuingCarrierAddressId;
                                entityPM.IssuingCarrierIATACode = booking.IssuingCarrierIATACode;
                                entityPM.CASSCode = booking.CASSCode;
                            }
                        }

                        if (string.IsNullOrEmpty(entityPM.IssuingCarrierAgentId))
                        {
                            entityPM.IssuingCarrierAgentId = loggedTenant.AgentId;
                        }

                        if (string.IsNullOrEmpty(entityPM.IssuingCarrierAddressId))
                        {
                            if (!string.IsNullOrEmpty(entityPM.IssuingCarrierAgentId))
                            {
                                Address myAddress = myAddressRepository.GetMainAddressByCardId(entityPM.IssuingCarrierAgentId, tenant);

                                if (myAddress != null)
                                {
                                    entityPM.IssuingCarrierAddressId = myAddress.Id;
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(entityPM.IssuingCarrierIATACode))
                        {
                            entityPM.IssuingCarrierIATACode = loggedTenant.IATA;
                        }

                        if (string.IsNullOrEmpty(entityPM.CASSCode))
                        {
                            entityPM.CASSCode = loggedTenant.CASSCode;
                        }

                        if (string.IsNullOrEmpty(entityPM.RegulatedAgentRANumber))
                        {
                            entityPM.RegulatedAgentRANumber = loggedTenant.RegulatedAgentNumber;
                        }
                        #endregion
                    }

                    else
                    {
                        #region
                        if (!string.IsNullOrEmpty(entityPM.IssuingCarrierAgentId))
                        {
                            if (string.IsNullOrEmpty(entityPM.IssuingCarrierAddressId))
                            {
                                Address myAddress = myAddressRepository.GetMainAddressByCardId(entityPM.IssuingCarrierAgentId, tenant);

                                if (myAddress != null)
                                {
                                    entityPM.IssuingCarrierAddressId = myAddress.Id;
                                }
                            }

                            AgentRepository myRepository = new AgentRepository(myCommonContext);
                            Agent myAgent = myRepository.GetSingleAgent(tenant, entityPM.IssuingCarrierAgentId);
                            if (myAgent != null)
                            {
                                if (string.IsNullOrEmpty(entityPM.CASSCode))
                                {
                                    entityPM.CASSCode = myAgent.CASSCode;
                                }

                                if (string.IsNullOrEmpty(entityPM.IssuingCarrierIATACode))
                                {
                                    entityPM.IssuingCarrierIATACode = myAgent.IATACode;
                                }

                                if (string.IsNullOrEmpty(entityPM.RegulatedAgentRANumber))
                                {
                                    entityPM.RegulatedAgentRANumber = myAgent.RegulatedAgentCode;
                                }
                            }

                            if (string.IsNullOrEmpty(entityPM.ColoaderId))
                            {
                                entityPM.ColoaderId = entityPM.IssuingCarrierAgentId;
                            }

                            if (string.IsNullOrEmpty(entityPM.ColoaderAddressId))
                            {
                                entityPM.ColoaderAddressId = entityPM.IssuingCarrierAddressId;
                            }

                            if (string.IsNullOrEmpty(entityPM.ColoaderRANumber))
                            {
                                if (myAgent != null)
                                {
                                    entityPM.ColoaderRANumber = myAgent.RegulatedAgentCode;
                                }
                            }
                        }
                        #endregion
                    }
                }

                else
                {
                    entityPM.IssuingCarrierAgentId = null;
                    entityPM.IssuingCarrierAddressId = null;
                }
            }
        }
        private void InitializeKnownConsignor()
        {
            if (string.IsNullOrEmpty(entityPM.ShipperId))
            {
                entityPM.KnownConsignorNumber = null;
            }

            else
            {
                if (string.IsNullOrEmpty(entityPM.KnownConsignorNumber))
                {
                    CustomerRepository myCustomerRepository = new CustomerRepository(tenant);
                    Customer myCustomer = myCustomerRepository.GetSingleCustomer(entityPM.ShipperId, tenant, true);
                    if (myCustomer != null)
                    {
                        entityPM.KnownConsignorNumber = myCustomer.KnownConsignor;
                    }
                }
            }
        }
        private void InitializePrintingFields()
        {
            if (loggedTenant.RegulatedAgentRegimeActivated)
            {
                if (!entityPM.AWBPrintingRANumberEdited)
                {
                    #region
                    string myField = null;

                    if (entityPM.IsKnownCargo)
                    {
                        if (!string.IsNullOrEmpty(entityPM.ColoaderRANumber))
                        {
                            myField = entityPM.ColoaderRANumber;
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(entityPM.RegulatedAgentRANumber) && !string.IsNullOrEmpty(entityPM.KnownConsignorNumber))
                            {
                                myField = entityPM.RegulatedAgentRANumber;
                            }
                        }
                    }

                    entityPM.AWBPrintingRANumber = myField;
                    #endregion
                }

                if (!entityPM.AdditionalHandlingInfoEdited)
                {
                    #region
                    string myCode = null;
                    string myField = null;

                    if (string.IsNullOrEmpty(entityPM.KnownConsignorNumber))
                    {
                        myCode = "UNK";
                    }

                    else
                    {
                        if (entityPM.IsKnownCargo)
                        {
                            myCode = "KCKC";
                        }

                        else
                        {
                            myCode = "UKKC";
                        }
                    }

                    AWBAdditionalHandlingInfoRepository myRepository = new AWBAdditionalHandlingInfoRepository(tenant);
                    AWBAdditionalHandlingInfo myHandlingInfo = myRepository.GetSingleAWBAdditionalHandlingInfoByCode(myCode, tenant);
                    if (myHandlingInfo != null)
                    {
                        myField = myHandlingInfo.PrintDescription;
                    }

                    entityPM.AdditionalHandlingInfo = myField;
                    if (string.IsNullOrEmpty(entityPM.AWBHandlingInformation))
                    {
                        entityPM.AWBHandlingInformation = myField;
                    }
                    #endregion
                }

                if (!entityPM.AWBPrintingSecurityStatusEdited)
                {
                    #region
                    string myFieldCode = null;

                    if (entityPM.IsKnownCargo)
                    {
                        if (!string.IsNullOrEmpty(entityPM.ColoaderRANumber))
                        {
                            if (!string.IsNullOrEmpty(entityPM.RegulatedAgentRANumber))
                            {
                                myFieldCode = "SPX";
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(entityPM.RegulatedAgentRANumber) && !string.IsNullOrEmpty(entityPM.KnownConsignorNumber))
                            {
                                myFieldCode = "SPX";
                            }
                        }
                    }

                    string myFieldId = null;
                    if (!string.IsNullOrEmpty(myFieldCode))
                    {
                        AWBSpecialHandlingCodeRepository myRepository = new AWBSpecialHandlingCodeRepository(tenant);
                        AWBSpecialHandlingCode myField = myRepository.GetSingleAWBHandlingCodeByCode(myFieldCode);
                        if (myField != null)
                        {
                            myFieldId = myField.Id;
                        }
                    }

                    entityPM.AWBPrintingSecurityStatusId = myFieldId;
                    #endregion
                }
            }
        }
        private void InitializeNumberOfInsidePackages()
        {
            if (MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId))
            {
                entityPM.NumberOfInsidePackages = 0;
                entityPM.NumberOfInsidePackagesDetails = "";

                if (isNewEntity)
                {
                    foreach (ShipmentPackagePM item in entityPM.ShipmentPackages)
                    {
                        item.NumberOfInsidePackages = 0;
                        item.NumberOfInsidePackagesDetails = "";
                    }
                }

                else
                {
                    if (initializer.ShipmentPackagesChangeSet != null)
                    {
                        foreach (ShipmentPackagePM item in initializer.ShipmentPackagesChangeSet)
                        {
                            item.NumberOfInsidePackages = 0;
                            item.NumberOfInsidePackagesDetails = "";
                        }
                    }
                }
            }

            else
            {
                if (isNewEntity)
                {
                    List<ShipmentPackagePM> allPackages = entityPM.ShipmentPackages.ToList();
                    List<InsideShipmentPackagePM> insidePackages = new List<InsideShipmentPackagePM>();

                    foreach (ShipmentPackagePM item in allPackages)
                    {
                        List<InsideShipmentPackagePM> list = item.InsideShipmentPackages.ToList();

                        if (list.Count > 0)
                        {
                            insidePackages.AddRange(list);
                        }

                        NumberOfInsidePackagesResult myLineResult = this.GetNumberOfInsidePackagesResult(list);
                        item.NumberOfInsidePackages = myLineResult.NumberOfInsidePackages;
                        item.NumberOfInsidePackagesDetails = myLineResult.NumberOfInsidePackagesDetails;
                    }

                    NumberOfInsidePackagesResult myShipmentResult = this.GetNumberOfInsidePackagesResult(insidePackages);
                    entityPM.NumberOfInsidePackages = myShipmentResult.NumberOfInsidePackages;
                    entityPM.NumberOfInsidePackagesDetails = myShipmentResult.NumberOfInsidePackagesDetails;
                }

                else
                {
                    if (initializer.ShipmentPackagesChangeSet != null)
                    {
                        List<ShipmentPackagePM> allPackages = initializer.ShipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                        List<InsideShipmentPackagePM> insidePackages = new List<InsideShipmentPackagePM>();

                        foreach (ShipmentPackagePM item in allPackages)
                        {
                            List<InsideShipmentPackagePM> list = new List<InsideShipmentPackagePM>();

                            switch (item.ChangeSetOp)
                            {
                                case ChangeSetOperation.Insert:
                                case ChangeSetOperation.None:
                                    {
                                        list = item.InsideShipmentPackages.ToList();
                                        break;
                                    }

                                case ChangeSetOperation.Update:
                                    {
                                        list = item.InsideShipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                                        break;
                                    }
                            }

                            if (list.Count > 0)
                            {
                                insidePackages.AddRange(list);
                            }

                            NumberOfInsidePackagesResult myLineResult = this.GetNumberOfInsidePackagesResult(list);
                            item.NumberOfInsidePackages = myLineResult.NumberOfInsidePackages;
                            item.NumberOfInsidePackagesDetails = myLineResult.NumberOfInsidePackagesDetails;

                            if (item.ChangeSetOp == ChangeSetOperation.None)
                            {
                                ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(item.Id, tenant);
                                itemPoco.NumberOfInsidePackages = item.NumberOfInsidePackages;
                                itemPoco.NumberOfInsidePackagesDetails = item.NumberOfInsidePackagesDetails;
                                shipmentPackageRepository.Update(itemPoco);
                            }
                        }

                        NumberOfInsidePackagesResult myShipmentResult = this.GetNumberOfInsidePackagesResult(insidePackages);
                        entityPM.NumberOfInsidePackages = myShipmentResult.NumberOfInsidePackages;
                        entityPM.NumberOfInsidePackagesDetails = myShipmentResult.NumberOfInsidePackagesDetails;
                    }
                }
            }
        }
        private NumberOfInsidePackagesResult GetNumberOfInsidePackagesResult(List<InsideShipmentPackagePM> insidePackages)
        {
            NumberOfInsidePackagesResult myResult = new NumberOfInsidePackagesResult()
            {
                NumberOfInsidePackages = 0,
                NumberOfInsidePackagesDetails = "",
            };

            if (insidePackages.Count > 0)
            {
                int? myNumberOfInsidePackages = 0;
                string myNumberOfInsidePackagesDetails = "";

                myNumberOfInsidePackages = insidePackages.Sum(s => s.Quantity);

                List<InsideShipmentPackagePM> list1 = insidePackages.Where(d => d.PackageTypeId != null).ToList();
                List<InsideShipmentPackagePM> list2 = insidePackages.Where(d => d.PackageTypeId == null).ToList();

                List<NumberOfInsidePackagesHelper> dataList = (from a in list1
                                                               group a by a.PackageTypeId into g
                                                               select new NumberOfInsidePackagesHelper
                                                               {
                                                                   PackageTypeId = g.Key,
                                                                   Count = g.Sum(s => s.Quantity)
                                                               }).ToList();

                foreach (NumberOfInsidePackagesHelper item in dataList)
                {
                    if (item.Count == null)
                    {
                        item.Count = 0;
                    }

                    PackageType myPackageType = PackageTypeRepository.GetSinglePackageType(item.PackageTypeId, tenant, true);
                    if (myPackageType != null)
                    {
                        item.PackageTypeName = myPackageType.EnglishName;
                    }
                }

                foreach (NumberOfInsidePackagesHelper item in dataList.OrderBy(o => o.PackageTypeName))
                {
                    string myString = item.Count + " " + item.PackageTypeName;

                    if (!string.IsNullOrEmpty(myString))
                    {
                        myNumberOfInsidePackagesDetails = string.IsNullOrEmpty(myNumberOfInsidePackagesDetails) ? myString : myNumberOfInsidePackagesDetails + ", " + myString;
                    }
                }

                if (list2.Count > 0)
                {
                    string myString = list2.Sum(s => s.Quantity) + " " + "others";

                    if (!string.IsNullOrEmpty(myString))
                    {
                        myNumberOfInsidePackagesDetails = string.IsNullOrEmpty(myNumberOfInsidePackagesDetails) ? myString : myNumberOfInsidePackagesDetails + ", " + myString;
                    }
                }

                if (list1 != null && list1.Count > 0)
                {
                    foreach (InsideShipmentPackagePM item in list1)
                    {
                        var itemVehicleDetails = "";
                        itemVehicleDetails += string.IsNullOrEmpty(item.Make) ? "" : item.Make;
                        itemVehicleDetails += string.IsNullOrEmpty(item.Model) ? "" : "/ " + item.Model;
                        itemVehicleDetails += string.IsNullOrEmpty(item.Year) ? "" : "/ " + item.Year;
                        itemVehicleDetails += string.IsNullOrEmpty(item.Color) ? "" : "/ " + item.Color;
                        itemVehicleDetails += string.IsNullOrEmpty(item.ChassisNumber) ? "" : "/ " + item.ChassisNumber;
                        itemVehicleDetails += string.IsNullOrEmpty(item.RegistrationNumber) ? "" : "/ " + item.RegistrationNumber;
                        itemVehicleDetails += string.IsNullOrEmpty(item.CountryCode) ? "" : "/ " + item.CountryCode;
                        myNumberOfInsidePackagesDetails = string.IsNullOrEmpty(myNumberOfInsidePackagesDetails) ? itemVehicleDetails : myNumberOfInsidePackagesDetails + "\n " + itemVehicleDetails;
                    }
                }

                if (myNumberOfInsidePackagesDetails.Length > 500)
                {
                    myNumberOfInsidePackagesDetails = myNumberOfInsidePackagesDetails.Substring(0, 500);
                }

                myResult.NumberOfInsidePackages = myNumberOfInsidePackages == null ? 0 : myNumberOfInsidePackages.Value;
                myResult.NumberOfInsidePackagesDetails = string.IsNullOrEmpty(myNumberOfInsidePackagesDetails) ? null : myNumberOfInsidePackagesDetails;
            }

            return myResult;
        }

        private void InitializeClosingFields()
        {
            if (!this.entityPM.IsHybrid)
            {
                if (this.entityPM.ShipmentLevelCode != "H")
                {
                    if (this.entityPM.IsOperationalClosed != this.entityPoco.IsOperationalClosed)
                    {
                        if (this.entityPM.IsOperationalClosed)
                        {
                            this.entityPM.OperationalCloseDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            this.entityPM.OperationalClosedByUserId = this.entityPM.UpdatedByUserId;
                            if (this.entityPM.FirstOperationalCloseDate == null)
                            {
                                this.entityPM.FirstOperationalCloseDate = this.entityPM.OperationalCloseDate;
                            }
                        }

                        else
                        {
                            this.entityPM.OperationalCloseDate = null;
                            this.entityPM.OperationalClosedByUserId = null;


                        }
                    }

                    if (this.entityPM.IsAccountingClosed != this.entityPoco.IsAccountingClosed)
                    {
                        if (this.entityPM.IsAccountingClosed)
                        {
                            entityPM.AccountingCloseDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            this.entityPM.AccountingClosedByUserId = this.entityPM.UpdatedByUserId;
                            if (this.entityPM.FirstAccountingCloseDate == null)
                            {
                                this.entityPM.FirstAccountingCloseDate = this.entityPM.AccountingCloseDate;
                            }
                        }

                        else
                        {
                            entityPM.AccountingCloseDate = null;
                            this.entityPM.AccountingClosedByUserId = null;
                        }
                    }
                }

                bool isConnectedHouse = false;
                if (this.entityPM.ShipmentLevelCode == "H" && this.entityPM.MasterShipmentDataId != null)
                {
                    isConnectedHouse = true;
                }

                if (!isConnectedHouse)
                {
                    if (this.entityPM.IsCancelled != this.entityPoco.IsCancelled)
                    {
                        if (entityPM.IsCancelled)
                        {
                            entityPM.CancelledDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            entityPM.BookingId = null;
                        }

                        else
                        {
                            entityPM.CancelledDate = null;
                        }
                    }
                }
            }
        }

        List<Shipment> allHouses = new List<Shipment>();
        private void CheckUpdatingMasterHouses()
        {
            if (!this.isNewEntity)
            {
                if (entityPM.ShipmentLevelCode == "C")
                {
                    this.allHouses = entityRepository.GetHouseShipmentsForMaster(entityPM.Id, tenant);

                    if (entityPM.IsCancelled != entityPoco.IsCancelled)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    else if (entityPM.IsAccountingClosed != entityPoco.IsAccountingClosed)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    else if (entityPM.IsOperationalClosed != entityPoco.IsOperationalClosed)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    else if (entityPM.MainCarriageFromPortId != entityMasterData.MainCarriageFromPortId)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    else if (entityPM.MainCarriageFinalDestinationPortId != entityMasterData.MainCarriageFinalDestinationPortId)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    else if (entityPM.PreCarriageFromPortId != entityMasterData.PreCarriageFromPortId)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    else if (entityPM.OnCarriageToPortId != entityMasterData.OnCarriageToPortId)
                    {
                        this.initializer.IsUpdatingHouses = true;
                    }
                    //else if (entityPM.ShipmentConsoleShipments != null && entityPM.ShipmentConsoleShipments.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert).Any())
                    //{
                    //    this.initializer.IsUpdatingHouses = true;
                    //}
                }
            }
        }

        //private void CreateShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        //{
        //    itemPM.Id = IdCounter.GetNumber("ShipmentOrderPackage", tenant).ToString();
        //    itemPM.ShipmentId = entityPM.Id;
        //    itemPM.Tenant = tenant;

        //    ShipmentOrderPackage itemPoco = new ShipmentOrderPackage()
        //    {
        //        Id = itemPM.Id,
        //    };

        //    ShipmentMapping.MapOrderPackage(itemPM, itemPoco, true);
        //    shipmentOrderPackageRepository.Add(itemPoco);
        //}
        //private void UpdateShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        //{
        //    ShipmentOrderPackage itemPoco = shipmentOrderPackageRepository.GetSingleShipmentOrderPackage(itemPM.Id);

        //    if (itemPoco != null)
        //    {
        //        ShipmentMapping.MapOrderPackage(itemPM, itemPoco, false);
        //        shipmentOrderPackageRepository.Update(itemPoco);
        //    }
        //}
        private void DeleteShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            ShipmentOrderPackage itemPoco = shipmentOrderPackageRepository.GetSingleShipmentOrderPackage(itemPM.Id);

            if (itemPoco != null)
            {
                shipmentOrderPackageRepository.Remove(itemPoco);
            }
        }

        public Dictionary<string, string> DummyIdGuidPackages { get; set; }
        private void CreateShipmentPackage(ShipmentPackagePM itemPM, string shipmentId = null)
        {
            if (entityPM.IsStandalonePickupDelivery)
            {
                initializer.IsPackageCreatedFromStandaloneShipment = true;
                initializer.StandalonePackage = itemPM;
            }

            itemPM.Id = IdCounter.GetNumber("ShipmentPackage", tenant).ToString();
            itemPM.ShipmentId = shipmentId == null ? entityPM.Id : shipmentId;
            itemPM.Tenant = tenant;

            if (initializer.IsFCLEntity && itemPM.Quantity == null)
            {
                itemPM.Quantity = 1;
            }

            ShipmentPackage itemPoco = new ShipmentPackage()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                ShipmentId = itemPM.ShipmentId,
            };

            this.UpdateShipmentDeliveryFromPackage(itemPM, itemPoco);

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TracePackage(itemPM, itemPoco, entityPM);
            }

            ShipmentMapping.MapPcakge(itemPM, itemPoco, true, this.loggedTenant);
            shipmentPackageRepository.Add(itemPoco);

            if (!string.IsNullOrEmpty(itemPM.OriginalShipmentPackageId))
            {
                ShipmentPackage originPackage = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.OriginalShipmentPackageId, tenant);
                if (originPackage != null)
                {
                    originPackage.ContainerNumber = itemPM.ContainerNumber;
                }
            }

            if (itemPM.InsideShipmentPackages != null)
            {
                foreach (InsideShipmentPackagePM insideItemPM in itemPM.InsideShipmentPackages)
                {
                    this.CreateInsideShipmentPackage(insideItemPM, itemPM.Id, itemPM.ContainerNumber);
                }
            }

            if (itemPM.ShipmentPackageItems != null)
            {
                int lineNumber = 0;
                foreach (ShipmentPackageItemPM packageItemPM in itemPM.ShipmentPackageItems)
                {
                    lineNumber += 1;
                    packageItemPM.LineNumber = lineNumber;
                    this.CreateShipmentPackageItem(packageItemPM, itemPM.Id);
                }
            }

            if (itemPM.ShipmentPackageHarmonizes != null)
            {
                foreach (ShipmentPackageHarmonizePM itemHarmonizePM in itemPM.ShipmentPackageHarmonizes)
                {
                    this.CreateShipmentPackageHarmonize(itemHarmonizePM, itemPM.Id);
                }
            }

            calculateProfit = true;
            calculatePayables = true;
            calculateReceivables = true;

            if (itemPM.DummyIdGuid != null)
            {
                if (DummyIdGuidPackages == null)
                {
                    DummyIdGuidPackages = new Dictionary<string, string>();
                }

                DummyIdGuidPackages.Add(itemPM.Id, itemPM.DummyIdGuid);
            }
        }
        private void UpdateShipmentPackage(ShipmentPackagePM itemPM)
        {
            ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.Id, tenant);

            this.UpdateShipmentDeliveryFromPackage(itemPM, itemPoco);

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TracePackage(itemPM, itemPoco, entityPM);
            }

            ShipmentMapping.MapPcakge(itemPM, itemPoco, false, this.loggedTenant);
            shipmentPackageRepository.Update(itemPoco);

            if (!string.IsNullOrEmpty(itemPoco.OriginalShipmentPackageId))
            {
                ShipmentPackage originPackage = shipmentPackageRepository.GetSingleShipmentPackage(itemPoco.OriginalShipmentPackageId, tenant);
                if (originPackage != null)
                {
                    originPackage.ContainerNumber = itemPoco.ContainerNumber;
                    shipmentPackageRepository.Update(originPackage);
                }
            }

            if (itemPM.InsideShipmentPackages != null)
            {
                foreach (InsideShipmentPackagePM insideItemPM in itemPM.InsideShipmentPackages)
                {
                    if (!string.IsNullOrEmpty(insideItemPM.OriginalShipmentPackageId))
                    {
                        ShipmentPackage originPackage = shipmentPackageRepository.GetSingleShipmentPackage(insideItemPM.OriginalShipmentPackageId, tenant);
                        if (originPackage != null)
                        {
                            originPackage.ContainerNumber = itemPoco.ContainerNumber;
                            shipmentPackageRepository.Update(originPackage);
                        }
                    }
                }
            }

            if (itemPM.InsideShipmentPackagesChangeSet != null)
            {
                foreach (InsideShipmentPackagePM insideItemPM in itemPM.InsideShipmentPackagesChangeSet)
                {
                    switch (insideItemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateInsideShipmentPackage(insideItemPM, itemPM.Id, itemPM.ContainerNumber);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateInsideShipmentPackage(insideItemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteInsideShipmentPackage(insideItemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }

            if (itemPM.ShipmentPackageItemsChangeSet != null)
            {
                int lineNumber = 0;

                if (itemPM.ShipmentPackageItemsChangeSet.Count > 0)
                {
                    lineNumber = itemPM.ShipmentPackageItemsChangeSet.Max(m => m.LineNumber);
                }

                foreach (ShipmentPackageItemPM packageItemPM in itemPM.ShipmentPackageItemsChangeSet)
                {
                    switch (packageItemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                lineNumber += 1;
                                packageItemPM.LineNumber = lineNumber;
                                this.CreateShipmentPackageItem(packageItemPM, itemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPackageItem(packageItemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPackageItem(packageItemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }

            if (itemPM.ShipmentPackageHarmonizesChangeSet != null)
            {
                foreach (ShipmentPackageHarmonizePM harmonizeItemPM in itemPM.ShipmentPackageHarmonizesChangeSet)
                {
                    switch (harmonizeItemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPackageHarmonize(harmonizeItemPM, itemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPackageHarmonize(harmonizeItemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPackageHarmonize(harmonizeItemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
            if (!(IsInlandDomesticShipment(entityPM) && !entityPM.IsStandalonePickupDelivery))
            {
                UpdateConnectedPackages(itemPM);
            }

            calculateProfit = true;
            calculatePayables = true;
            calculateReceivables = true;
        }
        private void DeleteShipmentPackage(ShipmentPackagePM itemPM)
        {
            ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                if (!entityPM.IsHybrid)
                {
                    shipmentTracing.TraceDeletedPackage(itemPM, itemPoco, entityPM);
                }

                List<ShipmentContainerStatus> shipmentContainerStatuses = shipmentContainerStatusRepository.GetShipmentContainerStatusByContainerId(itemPoco.Id, itemPoco.Tenant).ToList();
                if (shipmentContainerStatuses != null)
                {
                    foreach (ShipmentContainerStatus item in shipmentContainerStatuses)
                    {
                        shipmentContainerStatusRepository.Remove(item);
                    }
                }

                List<InsideShipmentPackage> list = insideShipmentPackageRepository.GetInsidePackagesByShipmentPackageId(itemPoco.Id, itemPoco.Tenant).ToList();
                if (list != null)
                {
                    foreach (InsideShipmentPackage item in list)
                    {
                        insideShipmentPackageRepository.Remove(item);
                    }
                }

                List<ShipmentPackageItem> list2 = shipmentPackageItemRepository.GetShipmentPackageItemsbyPackageId(itemPoco.Id, itemPoco.Tenant).ToList();
                if (list2 != null)
                {
                    foreach (ShipmentPackageItem item in list2)
                    {
                        shipmentPackageItemRepository.Remove(item);
                    }
                }

                List<ShipmentPackageHarmonize> list3 = shipmentPackageHarmonizeRepository.GetShipmentPackageHarmonizesByShipmentPackageId(itemPoco.Id, itemPoco.Tenant).ToList();
                if (list3 != null)
                {
                    foreach (ShipmentPackageHarmonize item in list3)
                    {
                        shipmentPackageHarmonizeRepository.Remove(item);
                    }
                }

                shipmentPackageRepository.Remove(itemPoco);
            }

            calculateProfit = true;
            calculatePayables = true;
            calculateReceivables = true;
        }

        private void CreateInsideShipmentPackage(InsideShipmentPackagePM insideItemPM, string shipmentPackageId, string shipmentPackageContainerNumber)
        {
            insideItemPM.Id = IdCounter.GetNumber("InsideShipmentPackage", tenant).ToString();
            insideItemPM.ShipmentPackageId = shipmentPackageId;
            insideItemPM.Tenant = tenant;

            InsideShipmentPackage insideItemPoco = new InsideShipmentPackage()
            {
                Id = insideItemPM.Id,
            };

            ShipmentMapping.MapInsideShipmentPackage(insideItemPM, insideItemPoco, true);
            insideShipmentPackageRepository.Add(insideItemPoco);

            if (insideItemPM.InsidePackageHarmonizes != null)
            {
                foreach (ShipmentPackageHarmonizePM itemHarmonizePM in insideItemPM.InsidePackageHarmonizes)
                {
                    this.CreateInsidePackageHarmonize(itemHarmonizePM, shipmentPackageId, insideItemPM.Id);
                }
            }

            if (!string.IsNullOrEmpty(insideItemPM.OriginalShipmentPackageId))
            {
                ShipmentPackage originPackage = shipmentPackageRepository.GetSingleShipmentPackage(insideItemPM.OriginalShipmentPackageId, tenant);
                if (originPackage != null)
                {
                    originPackage.ContainerNumber = shipmentPackageContainerNumber;
                }
            }
        }
        private void UpdateInsideShipmentPackage(InsideShipmentPackagePM insideItemPM)
        {
            InsideShipmentPackage insideItemPoco = insideShipmentPackageRepository.GetSingleInsideShipmentPackage(insideItemPM.Id, tenant);

            if (insideItemPoco != null)
            {
                ShipmentMapping.MapInsideShipmentPackage(insideItemPM, insideItemPoco, true);
                insideShipmentPackageRepository.Update(insideItemPoco);
            }

            if (insideItemPM.InsidePackageHarmonizesChangeSet != null)
            {
                foreach (ShipmentPackageHarmonizePM itemHarmonizePM in insideItemPM.InsidePackageHarmonizesChangeSet)
                {
                    switch (itemHarmonizePM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateInsidePackageHarmonize(itemHarmonizePM, insideItemPM.ShipmentPackageId, insideItemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateInsidePackageHarmonize(itemHarmonizePM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteInsidePackageHarmonize(itemHarmonizePM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void DeleteInsideShipmentPackage(InsideShipmentPackagePM insideItemPM)
        {
            InsideShipmentPackage insideItemPoco = insideShipmentPackageRepository.GetSingleInsideShipmentPackage(insideItemPM.Id, tenant);

            ShipmentPackageHarmonizeQuery shipmentPackageHarmonizeQuery = new ShipmentPackageHarmonizeQuery(shipmentPackageHarmonizeRepository);
            List<ShipmentPackageHarmonizePM> packageHarmonize = shipmentPackageHarmonizeQuery.GetInsideShipmentPackageHarmonizes(insideItemPM.Id, tenant);
            foreach (ShipmentPackageHarmonizePM harmonizeItemPM in packageHarmonize)
            {
                ShipmentPackageHarmonize harmonizeItem = shipmentPackageHarmonizeRepository.GetSingleShipmentPackageHarmonize(harmonizeItemPM.Id, tenant);
                shipmentPackageHarmonizeRepository.Remove(harmonizeItem);
            }

            if (insideItemPoco != null)
            {
                insideShipmentPackageRepository.Remove(insideItemPoco);
            }
        }

        private void CreateShipmentPackageItem(ShipmentPackageItemPM packageItemPM, string shipmentPackageId)
        {
            packageItemPM.PackageId = shipmentPackageId;
            packageItemPM.Tenant = tenant;

            ShipmentPackageItem poco = new ShipmentPackageItem()
            {
                PackageId = packageItemPM.PackageId,
                LineNumber = packageItemPM.LineNumber,
                Tenant = packageItemPM.Tenant,
                Description = packageItemPM.Description,
                Quantity = packageItemPM.Quantity,
                GoodsValue = packageItemPM.GoodsValue,
            };

            shipmentPackageItemRepository.Add(poco);
        }
        private void UpdateShipmentPackageItem(ShipmentPackageItemPM packageItemPM)
        {
            ShipmentPackageItem poco = shipmentPackageItemRepository.GetSingleShipmentPackageItem(packageItemPM.PackageId, packageItemPM.LineNumber, tenant);

            if (poco != null)
            {
                poco.Quantity = packageItemPM.Quantity;
                poco.Description = packageItemPM.Description;
                poco.GoodsValue = packageItemPM.GoodsValue;

                shipmentPackageItemRepository.Update(poco);
            }
        }
        private void DeleteShipmentPackageItem(ShipmentPackageItemPM packageItemPM)
        {
            ShipmentPackageItem poco = shipmentPackageItemRepository.GetSingleShipmentPackageItem(packageItemPM.PackageId, packageItemPM.LineNumber, tenant);

            if (poco != null)
            {
                shipmentPackageItemRepository.Remove(poco);
            }
        }

        private void CreateShipmentPackageHarmonize(ShipmentPackageHarmonizePM itemPM, string shipmentPackageId)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentPackageHarmonize", tenant).ToString();
            itemPM.PackageId = shipmentPackageId;
            itemPM.Tenant = tenant;

            ShipmentPackageHarmonize itemPoco = new ShipmentPackageHarmonize()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                PackageId = itemPM.PackageId,
                Harmonize = itemPM.Harmonize,
            };

            shipmentPackageHarmonizeRepository.Add(itemPoco);
        }
        private void UpdateShipmentPackageHarmonize(ShipmentPackageHarmonizePM itemPM)
        {
            ShipmentPackageHarmonize itemPoco = shipmentPackageHarmonizeRepository.GetSingleShipmentPackageHarmonize(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                itemPoco.Harmonize = itemPM.Harmonize;
                shipmentPackageHarmonizeRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentPackageHarmonize(ShipmentPackageHarmonizePM itemPM)
        {
            ShipmentPackageHarmonize itemPoco = shipmentPackageHarmonizeRepository.GetSingleShipmentPackageHarmonize(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                shipmentPackageHarmonizeRepository.Remove(itemPoco);
            }
        }

        private void CreatePickUpDeliveryPackageHarmonize(PickUpDeliveryPackageHarmonizePM itemPM, string shipmentPackageId)
        {
            itemPM.Id = IdCounter.GetNumber("PickUpDeliveryPackageHarmonize", tenant).ToString();
            itemPM.PackageId = shipmentPackageId;
            itemPM.Tenant = tenant;

            PickUpDeliveryPackageHarmonize itemPoco = new PickUpDeliveryPackageHarmonize()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                PackageId = itemPM.PackageId,
                Harmonize = itemPM.Harmonize,
            };

            pickUpDeliveryPackageHarmonizeRepository.Add(itemPoco);
        }
        private void UpdatePickUpDeliveryPackageHarmonize(PickUpDeliveryPackageHarmonizePM itemPM)
        {
            PickUpDeliveryPackageHarmonize itemPoco = pickUpDeliveryPackageHarmonizeRepository.GetSinglePickUpDeliveryPackageHarmonize(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                itemPoco.Harmonize = itemPM.Harmonize;
                pickUpDeliveryPackageHarmonizeRepository.Update(itemPoco);
            }
        }
        private void DeletePickUpDeliveryPackageHarmonize(PickUpDeliveryPackageHarmonizePM itemPM)
        {
            PickUpDeliveryPackageHarmonize itemPoco = pickUpDeliveryPackageHarmonizeRepository.GetSinglePickUpDeliveryPackageHarmonize(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                pickUpDeliveryPackageHarmonizeRepository.Remove(itemPoco);
            }
        }

        private void CreateInsidePackageHarmonize(ShipmentPackageHarmonizePM itemPM, string shipmentPackageId, string insidePackageId)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentPackageHarmonize", tenant).ToString();
            itemPM.PackageId = shipmentPackageId;
            itemPM.InsidePackageId = insidePackageId;
            itemPM.Tenant = tenant;

            ShipmentPackageHarmonize itemPoco = new ShipmentPackageHarmonize()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                PackageId = itemPM.PackageId,
                InsidePackageId = itemPM.InsidePackageId,
                Harmonize = itemPM.Harmonize,
            };

            shipmentPackageHarmonizeRepository.Add(itemPoco);
        }
        private void UpdateInsidePackageHarmonize(ShipmentPackageHarmonizePM itemPM)
        {
            ShipmentPackageHarmonize itemPoco = shipmentPackageHarmonizeRepository.GetSingleShipmentPackageHarmonize(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                itemPoco.Harmonize = itemPM.Harmonize;
                shipmentPackageHarmonizeRepository.Update(itemPoco);
            }
        }
        private void DeleteInsidePackageHarmonize(ShipmentPackageHarmonizePM itemPM)
        {
            ShipmentPackageHarmonize itemPoco = shipmentPackageHarmonizeRepository.GetSingleShipmentPackageHarmonize(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                shipmentPackageHarmonizeRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentPickUp(ShipmentPickUpPM itemPM)
        {
            AddressValidating.ValidatePickUp(itemPM);

            itemPM.Id = IdCounter.GetNumber("ShipmentPickUpDelivery", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            if (!string.IsNullOrEmpty(itemPM.ParentPickUpDeliveryId))
            {
                ShipmentPickUpDelivery parent = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, itemPM.ParentPickUpDeliveryId);
                if (parent != null)
                {
                    if (parent.ChildPickUpIndex == null)
                    {
                        parent.ChildPickUpIndex = 2;
                    }

                    else
                    {
                        parent.ChildPickUpIndex += 1;
                    }

                    itemPM.PickUpDeliveryNumber = parent.PickUpDeliveryNumber + "/" + parent.ChildPickUpIndex;
                    shipmentPickUpDeliveryRepository.Update(parent);
                }
            }

            else
            {
                entityPM.ShipmentPickUpIndex += 1;
                itemPM.PickUpDeliveryNumber = entityPM.ShipmentNumber + "/" + entityPM.ShipmentPickUpIndex;
            }

            ShipmentPickUpDelivery itemPoco = new ShipmentPickUpDelivery()
            {
                Id = itemPM.Id,
            };

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TracePickUp(itemPM, itemPoco, entityPM, entityPoco);
            }

            ShipmentPickUpDeliveryValidator.ValidatePickup(itemPM, entityPM);
            ShipmentMapping.MapPickUp(itemPM, itemPoco, myCommonContext, true);
            shipmentPickUpDeliveryRepository.Add(itemPoco);

            if (itemPM.ShipmentPickUpDeliveryPackages != null)
            {
                foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in itemPM.ShipmentPickUpDeliveryPackages)
                {
                    this.CreateShipmentPickUpDeliveryPackage(insideItemPM, itemPM.Id);
                }
            }
        }
        private void UpdateShipmentPickUp(ShipmentPickUpPM itemPM)
        {
            AddressValidating.ValidatePickUp(itemPM);

            ShipmentPickUpDelivery itemPoco = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, itemPM.Id);

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TracePickUp(itemPM, itemPoco, entityPM, entityPoco);
            }

            if (string.IsNullOrEmpty(itemPoco.StandaloneShipmentId) && !string.IsNullOrEmpty(itemPM.StandaloneShipmentId))
            {
                itemPM.IsConnectedToStandalone = true;
            }

            ShipmentPickUpDeliveryValidator.ValidatePickup(itemPM, entityPM);
            ShipmentMapping.MapPickUp(itemPM, itemPoco, myCommonContext, true);
            shipmentPickUpDeliveryRepository.Update(itemPoco);

            if (itemPM.ShipmentPickUpPackagesChangeSet != null)
            {
                foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in itemPM.ShipmentPickUpPackagesChangeSet)
                {
                    switch (insideItemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPickUpDeliveryPackage(insideItemPM, itemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPickUpDeliveryPackage(insideItemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPickUpDeliveryPackage(insideItemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void DeleteShipmentPickUp(ShipmentPickUpPM itemPM)
        {
            ShipmentPickUpDelivery itemPoco = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, itemPM.Id);

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TraceDeletedPickUp(itemPM, itemPoco, entityPM, entityPoco);
            }

            ShipmentPickUpDeliveryPackageQuery shipmentPickUpDeliveryPackageQuery = new ShipmentPickUpDeliveryPackageQuery(shipmentPickUpDeliveryPackageRepository);

            List<ShipmentPickUpDeliveryPackagePM> PickUpDeliveryPackage = shipmentPickUpDeliveryPackageQuery.GetShipmentPickUpDeliveryPackages(itemPM.Id, tenant);
            foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in PickUpDeliveryPackage)
            {
                ShipmentPickUpDeliveryPackage insideItemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(insideItemPM.Id);
                shipmentPickUpDeliveryPackageRepository.Remove(insideItemPoco);
            }

            shipmentPickUpDeliveryRepository.Remove(itemPoco);
        }

        private void CreateShipmentDelivery(ShipmentDeliveryPM itemPM)
        {
            AddressValidating.ValidateDelivery(itemPM);

            itemPM.Id = IdCounter.GetNumber("ShipmentPickUpDelivery", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.ShipmentNumber = entityPM.ShipmentNumber;
            itemPM.Tenant = tenant;

            if (itemPM.PickUpDeliveryTypeCode == "EMPT")
            {
                entityPM.ShipmentContainerReturnIndex += 1;
                itemPM.PickUpDeliveryNumber = entityPM.ShipmentNumber + "/" + entityPM.ShipmentContainerReturnIndex;
            }

            else
            {
                if (!string.IsNullOrEmpty(itemPM.ParentPickUpDeliveryId))
                {
                    ShipmentPickUpDelivery parent = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, itemPM.ParentPickUpDeliveryId);
                    if (parent != null)
                    {
                        if (parent.ChildDeliveryIndex == null)
                        {
                            parent.ChildDeliveryIndex = 2;
                        }

                        else
                        {
                            parent.ChildDeliveryIndex += 1;
                        }

                        itemPM.PickUpDeliveryNumber = parent.PickUpDeliveryNumber + "/" + parent.ChildDeliveryIndex;
                        shipmentPickUpDeliveryRepository.Update(parent);
                    }
                }

                else
                {
                    entityPM.ShipmentDeliveryIndex += 1;
                    itemPM.PickUpDeliveryNumber = entityPM.ShipmentNumber + "/" + entityPM.ShipmentDeliveryIndex;
                }
            }

            this.UpdateShipmentPackageFromDelivery(itemPM);

            ShipmentPickUpDelivery itemPoco = new ShipmentPickUpDelivery()
            {
                Id = itemPM.Id,
            };

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TraceDelivery(itemPM, itemPoco, entityPM, entityPoco);
            }

            ShipmentPickUpDeliveryValidator.ValidateDelivery(itemPM, entityPM);
            ShipmentMapping.MapDelivery(itemPM, itemPoco, myCommonContext, true);
            shipmentPickUpDeliveryRepository.Add(itemPoco);

            if (itemPM.ShipmentPickUpDeliveryPackages != null)
            {
                foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in itemPM.ShipmentPickUpDeliveryPackages)
                {
                    this.CreateShipmentPickUpDeliveryPackage(insideItemPM, itemPM.Id);
                }
            }
        }
        private void UpdateShipmentDelivery(ShipmentDeliveryPM itemPM)
        {
            AddressValidating.ValidateDelivery(itemPM);

            ShipmentPickUpDelivery itemPoco = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, itemPM.Id);

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TraceDelivery(itemPM, itemPoco, entityPM, entityPoco);
            }

            if (string.IsNullOrEmpty(itemPoco.StandaloneShipmentId) && !string.IsNullOrEmpty(itemPM.StandaloneShipmentId))
            {
                itemPM.IsConnectedToStandalone = true;
            }

            this.UpdateShipmentPackageFromDelivery(itemPM);

            ShipmentPickUpDeliveryValidator.ValidateDelivery(itemPM, entityPM);
            ShipmentMapping.MapDelivery(itemPM, itemPoco, myCommonContext, false);
            shipmentPickUpDeliveryRepository.Update(itemPoco);

            if (itemPM.ShipmentDeliveryPackagesChangeSet != null)
            {
                foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in itemPM.ShipmentDeliveryPackagesChangeSet)
                {
                    switch (insideItemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPickUpDeliveryPackage(insideItemPM, itemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPickUpDeliveryPackage(insideItemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPickUpDeliveryPackage(insideItemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void DeleteShipmentDelivery(ShipmentDeliveryPM itemPM)
        {
            ShipmentPickUpDelivery itemPoco = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, itemPM.Id);

            if (itemPoco != null)
            {
                if (!entityPM.IsHybrid)
                {
                    shipmentTracing.TraceDeletedDelivery(itemPM, itemPoco, entityPM, entityPoco);
                }

                ShipmentPickUpDeliveryPackageQuery shipmentPickUpDeliveryPackageQuery = new ShipmentPickUpDeliveryPackageQuery(shipmentPickUpDeliveryPackageRepository);
                PickUpDeliveryPackageHarmonizeQuery pickUpDeliveryPackageHarmonizeQuery = new PickUpDeliveryPackageHarmonizeQuery(pickUpDeliveryPackageHarmonizeRepository);

                List<ShipmentPickUpDeliveryPackagePM> PickUpDeliveryPackage = shipmentPickUpDeliveryPackageQuery.GetShipmentPickUpDeliveryPackages(itemPM.Id, tenant);
                foreach (ShipmentPickUpDeliveryPackagePM insideItemPM in PickUpDeliveryPackage)
                {
                    ShipmentPickUpDeliveryPackage insideItemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(insideItemPM.Id);
                    shipmentPickUpDeliveryPackageRepository.Remove(insideItemPoco);

                    List<PickUpDeliveryPackageHarmonizePM> PickUpDeliveryPackageHarmonize = pickUpDeliveryPackageHarmonizeQuery.GetPickUpDeliveryPackageHarmonizes(insideItemPM.Id, tenant);
                    foreach (PickUpDeliveryPackageHarmonizePM harmonizeItemPM in PickUpDeliveryPackageHarmonize)
                    {
                        PickUpDeliveryPackageHarmonize harmonizeItem = pickUpDeliveryPackageHarmonizeRepository.GetSinglePickUpDeliveryPackageHarmonize(harmonizeItemPM.Id, tenant);
                        pickUpDeliveryPackageHarmonizeRepository.Remove(harmonizeItem);
                    }
                }

                shipmentPickUpDeliveryRepository.Remove(itemPoco);
            }
        }
        private void UpdateShipmentPackageFromDelivery(ShipmentDeliveryPM itemPM)
        {
            if (itemPM != null)
            {
                bool isUpdating = false;

                if (itemPM.ConnectedPackageId != null)
                {
                    isUpdating = true;
                }

                if (itemPM.AllConnectedPackagesId != null)
                {
                    if (itemPM.AllConnectedPackagesId.Count > 0)
                    {
                        isUpdating = true;
                    }
                }

                if (isUpdating)
                {
                    string myFromField = null;
                    string myToField = null;

                    switch (itemPM.PickUpDeliveryFromTypeCode)
                    {
                        #region From
                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(itemPM.FromPortId))
                                {
                                    Port myPort = myPortRepository.GetSinglePort(itemPM.FromPortId, entityPM.Tenant);
                                    if (myPort != null)
                                    {
                                        myFromField = myPort.Code;
                                    }
                                }

                                break;
                            }

                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(itemPM.FromAddressId))
                                {
                                    Address address = myAddressRepository.GetSingleAddress(itemPM.FromAddressId, entityPM.Tenant);
                                    if (address != null)
                                    {
                                        myFromField = address.City;
                                    }
                                }

                                break;
                            }

                        default:
                            {
                                myFromField = itemPM.FromAddressCity;
                                break;
                            }
                            #endregion
                    }
                    switch (itemPM.PickUpDeliveryToTypeCode)
                    {
                        #region To
                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(itemPM.ToPortId))
                                {
                                    Port myPort = myPortRepository.GetSinglePort(itemPM.ToPortId, entityPM.Tenant);
                                    if (myPort != null)
                                    {
                                        myToField = myPort.Code;
                                    }
                                }

                                break;
                            }

                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(itemPM.ToAddressId))
                                {
                                    Address address = myAddressRepository.GetSingleAddress(itemPM.ToAddressId, entityPM.Tenant);
                                    if (address != null)
                                    {
                                        myToField = address.City;
                                    }
                                }

                                break;
                            }

                        default:
                            {
                                myToField = itemPM.ToAddressCity;
                                break;
                            }
                            #endregion

                    }

                    if (itemPM.ConnectedPackageId != null)
                    {
                        #region
                        ShipmentPackagePM myPackage = this.entityPM.ShipmentPackages.Where(d => d.Id == itemPM.ConnectedPackageId).FirstOrDefault();
                        if (myPackage != null)
                        {
                            if (itemPM.PickUpDeliveryTypeCode == "EMPT")
                            {
                                myPackage.IsEmptyContainerReturnFU = true;
                                myPackage.EmptyContainerReturnId = itemPM.Id;
                                myPackage.EmptyContainerReturnFrom = myFromField;
                                myPackage.EmptyContainerReturnTo = myToField;
                                myPackage.EmptyContainerReturnETD = itemPM.ETD;
                                myPackage.EmptyContainerReturnATD = itemPM.ATD;
                                myPackage.EmptyContainerReturnETA = itemPM.ETA;
                                myPackage.EmptyContainerReturnATA = itemPM.ATA;
                                myPackage.ECRTransportModeCode = itemPM.TransportModeCode;
                            }

                            else
                            {
                                myPackage.IsDeliveryFU = true;
                                myPackage.DeliveryId = itemPM.Id;
                                myPackage.DeliveryFrom = myFromField;
                                myPackage.DeliveryTo = myToField;
                                myPackage.DeliveryETD = itemPM.ETD;
                                myPackage.DeliveryATD = itemPM.ATD;
                                myPackage.DeliveryETA = itemPM.ETA;
                                myPackage.DeliveryATA = itemPM.ATA;
                                myPackage.DeliveryTransportModeCode = itemPM.TransportModeCode;
                            }

                            ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(myPackage.Id, tenant);
                            ShipmentMapping.MapPcakge(myPackage, itemPoco, false, this.loggedTenant);
                            shipmentPackageRepository.Update(itemPoco);
                        }
                        #endregion
                    }

                    if (itemPM.AllConnectedPackagesId != null)
                    {
                        foreach (string id in itemPM.AllConnectedPackagesId)
                        {
                            ShipmentPackagePM myPackage = this.entityPM.ShipmentPackages.Where(d => d.Id == id).FirstOrDefault();
                            if (myPackage != null)
                            {
                                myPackage.IsDeliveryFU = true;
                                myPackage.DeliveryId = itemPM.Id;
                                myPackage.DeliveryFrom = myFromField;
                                myPackage.DeliveryTo = myToField;
                                myPackage.DeliveryETD = itemPM.ETD;
                                myPackage.DeliveryATD = itemPM.ATD;
                                myPackage.DeliveryETA = itemPM.ETA;
                                myPackage.DeliveryATA = itemPM.ATA;
                                myPackage.DeliveryTransportModeCode = itemPM.TransportModeCode;

                                ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(myPackage.Id, tenant);
                                ShipmentMapping.MapPcakge(myPackage, itemPoco, false, this.loggedTenant);
                                shipmentPackageRepository.Update(itemPoco);
                            }
                        }
                    }
                }
            }
        }
        private void UpdateShipmentDeliveryFromPackage(ShipmentPackagePM itemPM, ShipmentPackage itemPoco)
        {
            if (itemPM != null && itemPoco != null)
            {
                if (itemPM.DeliveryId != null || itemPM.EmptyContainerReturnId != null)
                {
                    if (itemPM.DeliveryId != null)
                    {
                        ShipmentDeliveryPM myDelivery = this.entityPM.ShipmentDeliveries.Where(d => d.Id == itemPM.DeliveryId).FirstOrDefault();
                        if (myDelivery != null)
                        {
                            myDelivery.ETD = itemPM.DeliveryETD;
                            myDelivery.ATD = itemPM.DeliveryATD;
                            myDelivery.ETA = itemPM.DeliveryETA;
                            myDelivery.ATA = itemPM.DeliveryATA;
                            myDelivery.TransportModeCode = itemPM.DeliveryTransportModeCode;

                            if (myDelivery.ShipmentPickUpDeliveryPackages.Count > 0)
                            {
                                ShipmentPickUpDeliveryPackagePM DeliveryPackagePM = myDelivery.ShipmentPickUpDeliveryPackages.Where(d => d.OriginalShipmentPackageId == itemPoco.Id).FirstOrDefault();
                                if (DeliveryPackagePM != null)
                                {
                                    DeliveryPackagePM.ContainerNumber = itemPM.ContainerNumber;
                                    DeliveryPackagePM.Description = itemPM.Description;
                                    DeliveryPackagePM.PackageTypeId = itemPM.PackageTypeId;
                                    DeliveryPackagePM.Quantity = itemPM.Quantity;
                                    DeliveryPackagePM.Volume = itemPM.Volume;
                                    DeliveryPackagePM.Weight = itemPM.Weight;
                                    DeliveryPackagePM.ShipperSeal = itemPM.ShipperSeal;
                                    DeliveryPackagePM.Width = itemPM.Width;
                                    DeliveryPackagePM.Height = itemPM.Height;
                                    DeliveryPackagePM.Length = itemPM.Length;

                                    ShipmentPickUpDeliveryPackage DeliveryPackage = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(DeliveryPackagePM.Id);
                                    if (DeliveryPackage != null)
                                    {
                                        ShipmentMapping.MapPickUpDeliveryPackage(DeliveryPackagePM, DeliveryPackage, false);
                                        shipmentPickUpDeliveryPackageRepository.Update(DeliveryPackage);
                                    }
                                }
                            }

                            ShipmentPickUpDelivery entityPOCO = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, myDelivery.Id);
                            ShipmentMapping.MapDelivery(myDelivery, entityPOCO, myCommonContext, false);
                            shipmentPickUpDeliveryRepository.Update(entityPOCO);
                        }
                    }

                    if (itemPM.EmptyContainerReturnId != null)
                    {
                        bool isFieldsUpdated = false;
                        if (itemPM.EmptyContainerReturnETD != itemPoco.EmptyContainerReturnETD)
                        {
                            isFieldsUpdated = true;
                        }

                        else if (itemPM.EmptyContainerReturnATD != itemPoco.EmptyContainerReturnATD)
                        {
                            isFieldsUpdated = true;
                        }

                        else if (itemPM.EmptyContainerReturnETA != itemPoco.EmptyContainerReturnETA)
                        {
                            isFieldsUpdated = true;
                        }

                        else if (itemPM.EmptyContainerReturnATA != itemPoco.EmptyContainerReturnATA)
                        {
                            isFieldsUpdated = true;
                        }

                        else if (itemPM.ECRTransportModeCode != itemPoco.ECRTransportModeCode)
                        {
                            isFieldsUpdated = true;
                        }

                        if (isFieldsUpdated)
                        {
                            ShipmentDeliveryPM myDelivery = this.entityPM.ShipmentDeliveries.Where(d => d.Id == itemPM.EmptyContainerReturnId).FirstOrDefault();
                            if (myDelivery != null)
                            {
                                myDelivery.ETD = itemPM.EmptyContainerReturnETD;
                                myDelivery.ATD = itemPM.EmptyContainerReturnATD;
                                myDelivery.ETA = itemPM.EmptyContainerReturnETA;
                                myDelivery.ATA = itemPM.EmptyContainerReturnATA;
                                myDelivery.TransportModeCode = itemPM.ECRTransportModeCode;

                                ShipmentPickUpDelivery entityPOCO = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, myDelivery.Id);
                                ShipmentMapping.MapDelivery(myDelivery, entityPOCO, myCommonContext, false);
                                shipmentPickUpDeliveryRepository.Update(entityPOCO);
                            }
                        }
                    }
                }
            }
        }

        private void CreateShipmentPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM pickUpDeliveryPackagePM, string pickUpDeliveryId)
        {
            pickUpDeliveryPackagePM.Id = IdCounter.GetNumber("ShipmentPickUpDeliveryPackage", tenant).ToString();
            pickUpDeliveryPackagePM.ShipmentPickUpDeliveryId = pickUpDeliveryId;
            pickUpDeliveryPackagePM.Tenant = tenant;

            ShipmentPickUpDeliveryPackage insideItemPoco = new ShipmentPickUpDeliveryPackage()
            {
                Id = pickUpDeliveryPackagePM.Id,
            };

            ShipmentMapping.MapPickUpDeliveryPackage(pickUpDeliveryPackagePM, insideItemPoco, true);
            shipmentPickUpDeliveryPackageRepository.Add(insideItemPoco);

            if (pickUpDeliveryPackagePM.PickUpDeliveryPackageHarmonizes != null)
            {
                foreach (PickUpDeliveryPackageHarmonizePM itemHarmonizePM in pickUpDeliveryPackagePM.PickUpDeliveryPackageHarmonizes)
                {
                    this.CreatePickUpDeliveryPackageHarmonize(itemHarmonizePM, pickUpDeliveryPackagePM.Id);
                }
            }
            UpdateShipmentConcectedPackagesByContainerEntityId(pickUpDeliveryPackagePM);
        }
        private void UpdateShipmentPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM pickUpDeliveryPackagePM)
        {
            ShipmentPickUpDeliveryPackage insideItemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(pickUpDeliveryPackagePM.Id);
            ShipmentMapping.MapPickUpDeliveryPackage(pickUpDeliveryPackagePM, insideItemPoco, false);
            shipmentPickUpDeliveryPackageRepository.Update(insideItemPoco);

            if (pickUpDeliveryPackagePM.PickUpDeliveryPackageHarmonizesChangeSet != null)
            {
                foreach (PickUpDeliveryPackageHarmonizePM itemHarmonizePM in pickUpDeliveryPackagePM.PickUpDeliveryPackageHarmonizesChangeSet)
                {
                    switch (itemHarmonizePM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreatePickUpDeliveryPackageHarmonize(itemHarmonizePM, pickUpDeliveryPackagePM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdatePickUpDeliveryPackageHarmonize(itemHarmonizePM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeletePickUpDeliveryPackageHarmonize(itemHarmonizePM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
            UpdateShipmentConcectedPackagesByContainerEntityId(pickUpDeliveryPackagePM);
        }
        private void DeleteShipmentPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM pickUpDeliveryPackagePM)
        {
            ShipmentPickUpDeliveryPackage insideItemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(pickUpDeliveryPackagePM.Id);

            PickUpDeliveryPackageHarmonizeQuery pickUpDeliveryPackageHarmonizeQuery = new PickUpDeliveryPackageHarmonizeQuery(pickUpDeliveryPackageHarmonizeRepository);
            List<PickUpDeliveryPackageHarmonizePM> PickUpDeliveryPackageHarmonize = pickUpDeliveryPackageHarmonizeQuery.GetPickUpDeliveryPackageHarmonizes(pickUpDeliveryPackagePM.Id, tenant);
            foreach (PickUpDeliveryPackageHarmonizePM harmonizeItemPM in PickUpDeliveryPackageHarmonize)
            {
                PickUpDeliveryPackageHarmonize harmonizeItem = pickUpDeliveryPackageHarmonizeRepository.GetSinglePickUpDeliveryPackageHarmonize(harmonizeItemPM.Id, tenant);
                pickUpDeliveryPackageHarmonizeRepository.Remove(harmonizeItem);
            }

            shipmentPickUpDeliveryPackageRepository.Remove(insideItemPoco);
        }

        private void CreateShipmentPayable(ShipmentPayablePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentPayable", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentPayable itemPoco = new ShipmentPayable()
            {
                Id = itemPM.Id,
            };

            if (!string.IsNullOrEmpty(itemPM.TariffId))
            {
                this.UpdateTariffUsedDate(itemPM.TariffId);
            }

            ShipmentMapping.MapPayable(itemPM, itemPoco, loggedContact.Id, loggedTenant, true);
            shipmentPayableRepository.Add(itemPoco);

            if (itemPM.ChildShipmentPayables != null)
            {
                foreach (ShipmentPayablePM childItemPM in itemPM.ChildShipmentPayables)
                {
                    this.CreateChildPayable(childItemPM, itemPM.Id);
                }
            }

            calculateProfit = true;
            calculatePayables = true;
        }
        private void UpdateShipmentPayable(ShipmentPayablePM itemPM)
        {
            ShipmentPayable itemPoco = shipmentPayableRepository.GetSingleShipmentPayable(itemPM.Id);
            if (itemPoco != null)
            {
                if (!string.IsNullOrEmpty(itemPM.TariffId) && string.IsNullOrEmpty(itemPoco.TariffId))
                {
                    this.UpdateTariffUsedDate(itemPM.TariffId);
                }

                ShipmentMapping.MapPayable(itemPM, itemPoco, loggedContact.Id, loggedTenant, false);

                if (itemPM.ChildShipmentPayablesChangeSet != null)
                {
                    foreach (ShipmentPayablePM childItemPM in itemPM.ChildShipmentPayablesChangeSet)
                    {
                        switch (childItemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateChildPayable(childItemPM, itemPM.Id);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateChildPayable(childItemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteChildPayable(childItemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }

                shipmentPayableRepository.Update(itemPoco);
                calculateProfit = true;
                calculatePayables = true;
            }
        }
        private void DeleteShipmentPayable(ShipmentPayablePM itemPM)
        {
            ShipmentPayable itemPoco = shipmentPayableRepository.GetSingleShipmentPayable(itemPM.Id);
            if (itemPoco != null)
            {
                this.ValidatePayableConnectedInvoice(itemPM);
                List<ShipmentPayable> childPayables = shipmentPayableRepository.GetChildPayablesByParentPayable(itemPM.Id, tenant);

                foreach (ShipmentPayable insideItem in childPayables)
                {
                    shipmentPayableRepository.Remove(insideItem);
                }

                shipmentPayableRepository.Remove(itemPoco);
                calculateProfit = true;
                calculatePayables = true;
            }
        }
        private void ValidatePayableConnectedInvoice(ShipmentPayablePM payablePM)
        {
            APInvoiceLineRepository invoiceLineRepository = new APInvoiceLineRepository(tenant);
            if (invoiceLineRepository.IsPayableConnectedToInvoiceLines(payablePM.Id, tenant))
            {
                throw new ApplicationException("Can't delete payable " + payablePM.ChargesTypeName + " since it is connected to invoice");
            }
        }
        private void CreateChildPayable(ShipmentPayablePM childPayablePM, string parentId)
        {
            childPayablePM.Id = IdCounter.GetNumber("ShipmentPayable", tenant).ToString();
            childPayablePM.ShipmentPayableAmountTypeCode = "ACCU";
            childPayablePM.ShipmentPayableParentId = parentId;
            childPayablePM.Tenant = tenant;

            ShipmentPayable childPayablePoco = new ShipmentPayable()
            {
                Id = childPayablePM.Id,
            };

            ShipmentMapping.MapPayable(childPayablePM, childPayablePoco, loggedContact.Id, loggedTenant, true);
            shipmentPayableRepository.Add(childPayablePoco);
        }
        private void UpdateChildPayable(ShipmentPayablePM childPayablePM)
        {
            ShipmentPayable itemPOCO = shipmentPayableRepository.GetSingleShipmentPayable(childPayablePM.Id);
            if (itemPOCO != null)
            {
                ShipmentMapping.MapPayable(childPayablePM, itemPOCO, loggedContact.Id, loggedTenant, false);
                shipmentPayableRepository.Update(itemPOCO);
            }
        }
        private void DeleteChildPayable(ShipmentPayablePM childPayablePM)
        {
            ShipmentPayable itemPOCO = shipmentPayableRepository.GetSingleShipmentPayable(childPayablePM.Id);
            if (itemPOCO != null)
            {
                shipmentPayableRepository.Remove(itemPOCO);
            }
        }

        private void CreateShipmentReceivable(ShipmentReceivablePM itemPM)
        {
            this.CheckReceivableStatus(itemPM);

            // Ayman: we need this for the:IsBackToBack
            //if (string.IsNullOrEmpty(itemPM.Id))
            //{
            //    itemPM.Id = IdCounter.GetNumber("ShipmentReceivable", tenant).ToString();
            //}

            itemPM.Id = IdCounter.GetNumber("ShipmentReceivable", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentReceivable itemPoco = new ShipmentReceivable()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapReceivable(itemPM, itemPoco, loggedContact.Id, loggedTenant, true);
            shipmentReceivableRepository.Add(itemPoco);

            if (itemPM.ChildShipmentReceivables != null)
            {
                foreach (ShipmentReceivablePM childItemPM in itemPM.ChildShipmentReceivables)
                {
                    this.CreateChildReceivable(childItemPM, itemPM.Id);
                }
            }

            calculateProfit = true;
            calculateReceivables = true;
        }
        private void UpdateShipmentReceivable(ShipmentReceivablePM itemPM)
        {
            ShipmentReceivable itemPoco = shipmentReceivableRepository.GetSingleShipmentReceivable(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                ShipmentMapping.MapReceivable(itemPM, itemPoco, loggedContact.Id, loggedTenant, false);

                if (itemPM.ChildShipmentReceivablesChangeSet != null)
                {
                    foreach (ShipmentReceivablePM childItemPM in itemPM.ChildShipmentReceivablesChangeSet)
                    {
                        switch (childItemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateChildReceivable(childItemPM, itemPM.Id);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateChildReceivable(childItemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteChildReceivable(childItemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }

                shipmentReceivableRepository.Update(itemPoco);
                calculateProfit = true;
                calculateReceivables = true;
            }
        }
        private void DeleteShipmentReceivable(ShipmentReceivablePM itemPM)
        {
            ShipmentReceivable itemPoco = shipmentReceivableRepository.GetSingleShipmentReceivable(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                if (itemPoco.ShipmentReceivableLineStatusCode.ToUpper() == "ACCT")
                {
                    string msg = TranslateTextsClass.Translate("Shipment.M.CantDeleteThisReceivable", tenant);
                    throw new ApplicationException(msg);
                }

                else
                {
                    List<ShipmentReceivable> childReceivables = shipmentReceivableRepository.GetChildReceivableByParentPayable(itemPM.Id, tenant);

                    foreach (ShipmentReceivable insideItem in childReceivables)
                    {
                        shipmentReceivableRepository.Remove(insideItem);
                    }

                    shipmentReceivableRepository.Remove(itemPoco);
                    calculateProfit = true;
                    calculateReceivables = true;
                }
            }
        }
        private void CheckReceivableStatus(ShipmentReceivablePM itemPM)
        {
            if (itemPM != null)
            {
                if (string.IsNullOrEmpty(itemPM.ARInvoiceId))
                {
                    if (itemPM.ChangeSetOp != ChangeSetOperation.Delete)
                    {
                        string myStatusCode = null;

                        if (itemPM.MeasurementCode == "STFE" && itemPM.ChargesTypeCode == "ISTOR")
                        {
                            // import storage charge has no quantity or price
                            myStatusCode = "OAMT";
                        }

                        else
                        {
                            if (itemPM.Quantity == null || itemPM.UnitPrice == null)
                            {
                                myStatusCode = "EMPT";
                            }

                            else
                            {
                                myStatusCode = "OAMT";
                            }
                        }

                        if (itemPM.ShipmentReceivableLineStatusCode != myStatusCode)
                        {
                            itemPM.ShipmentReceivableLineStatusCode = myStatusCode;

                            if (itemPM.ChangeSetOp == ChangeSetOperation.None)
                            {
                                itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }
                    }
                }
            }
        }
        private void CreateChildReceivable(ShipmentReceivablePM childItemPM, string parentId)
        {
            childItemPM.Id = IdCounter.GetNumber("ShipmentReceivable", tenant).ToString();
            childItemPM.ShipmentReceivableParentId = parentId;
            childItemPM.Tenant = tenant;

            ShipmentReceivable childPoco = new ShipmentReceivable()
            {
                Id = childItemPM.Id,
            };

            ShipmentMapping.MapReceivable(childItemPM, childPoco, loggedContact.Id, loggedTenant, true);
            shipmentReceivableRepository.Add(childPoco);
        }
        private void UpdateChildReceivable(ShipmentReceivablePM childItemPM)
        {
            ShipmentReceivable itemPOCO = shipmentReceivableRepository.GetSingleShipmentReceivable(childItemPM.Id, tenant);
            if (itemPOCO != null)
            {
                ShipmentMapping.MapReceivable(childItemPM, itemPOCO, loggedContact.Id, loggedTenant, true);
                shipmentReceivableRepository.Update(itemPOCO);
            }
        }
        private void DeleteChildReceivable(ShipmentReceivablePM childItemPM)
        {
            ShipmentReceivable childPoco = shipmentReceivableRepository.GetSingleShipmentReceivable(childItemPM.Id, tenant);
            if (childPoco != null)
            {
                shipmentReceivableRepository.Remove(childPoco);
            }
        }

        private void CreateShipmentAWBPrintOnly(ShipmentAWBPrintOnlyPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentAWBPrintOnly", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentAWBPrintOnly itemPoco = new ShipmentAWBPrintOnly()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapAWBPrintOnly(itemPM, itemPoco, true);
            shipmentAWBPrintOnlyRepository.Add(itemPoco);
        }
        private void UpdateShipmentAWBPrintOnly(ShipmentAWBPrintOnlyPM itemPM)
        {
            ShipmentAWBPrintOnly itemPoco = shipmentAWBPrintOnlyRepository.GetSingleShipmentAWBPrintOnly(itemPM.Id);
            ShipmentMapping.MapAWBPrintOnly(itemPM, itemPoco, false);
            shipmentAWBPrintOnlyRepository.Update(itemPoco);
        }
        private void DeleteShipmentAWBPrintOnly(ShipmentAWBPrintOnlyPM itemPM)
        {
            ShipmentAWBPrintOnly itemPoco = shipmentAWBPrintOnlyRepository.GetSingleShipmentAWBPrintOnly(itemPM.Id);
            if (itemPoco != null)
            {
                shipmentAWBPrintOnlyRepository.Remove(itemPoco);
            }
        }

        private void CreateConsoleShipment(ConsoleShipmentPM itemPM)
        {
            Shipment houseShipment = entityRepository.GetSingleShipment(itemPM.Id, tenant);

            if (houseShipment != null)
            {
                houseShipment.MasterShipmentDataId = itemPM.MasterShipmentDataId;
                houseShipment.ComputedShipmentNumber = itemPM.ShipmentNumber;
                houseShipment.AgentComputed = houseShipment.AgentId == null ? entityPM.AgentId : houseShipment.AgentId;
                this.UpdateHouseRoutingFieldsWhenConnectedToMaster(houseShipment);
                entityRepository.Update(houseShipment);
                entityRepository.SubmitChanges();

                this.RunRegistryDateProcedure(houseShipment.Id);
                this.RunFirstApprovalDateProcedure(houseShipment.Id);

                calculateProfit = true;
                calculatePayables = true;
                calculateReceivables = true;

                initializer.ConnectedHousesIds.Add(houseShipment.Id);
            }
        }
        private void DeleteConsoleShipment(ConsoleShipmentPM itemPM)
        {
            Shipment houseShipment = entityRepository.GetSingleShipment(itemPM.Id, tenant);

            if (houseShipment != null)
            {
                houseShipment.MasterShipmentDataId = null;
                houseShipment.ComputedShipmentNumber = null;
                houseShipment.OperationalDate = houseShipment.CreateDateTime;
                houseShipment.AgentComputed = houseShipment.AgentId;
                entityRepository.Update(houseShipment);
                entityRepository.SubmitChanges();
                RunStoredProcedureClass.UpdateShipmentStatus(itemPM.Id, tenant);
                //this.RunRegistryDateProcedure(houseShipment.Id);

                calculateProfit = true;
                calculatePayables = true;
                calculateReceivables = true;

                List<PayableProratedAmount> proratedAmounts = payableProratedAmountRepository.GetPayableProratedAmountsByShipmentId(houseShipment.Id, tenant);
                foreach (PayableProratedAmount item in proratedAmounts)
                {
                    payableProratedAmountRepository.Remove(item);
                }

                List<ShipmentPayable> myPayables = shipmentPayableRepository.GetConsoleChildPayables(houseShipment.Id, tenant);
                foreach (ShipmentPayable item in myPayables)
                {
                    shipmentPayableRepository.Remove(item);
                }

                List<ShipmentReceivable> myReceivable = shipmentReceivableRepository.GetConsoleChildReceivables(houseShipment.Id, tenant);
                foreach (ShipmentReceivable item in myReceivable)
                {
                    shipmentReceivableRepository.Remove(item);
                }

                shipmentPayableRepository.SubmitChanges();
                shipmentReceivableRepository.SubmitChanges();

                //if (entityPM != null && !entityPM.IsHybrid)
                //{
                //    UpdateShipmentProfitClass.UpdateProfitFunction(houseShipment.Id, tenant, false);
                //}

                initializer.DeletedHousesIds.Add(houseShipment.Id);
            }
        }

        private void CreateShipmentFollowUp(ShipmentFollowUpPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("FollowUp", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.IsNew = false;

            FollowUp itemPoco = new FollowUp()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapFollowUp(itemPM, itemPoco, true);
            followUpRepository.Add(itemPoco);
        }
        private void UpdateShipmentFollowUp(ShipmentFollowUpPM itemPM)
        {
            FollowUp itemPoco = followUpRepository.GetSingleFollowUp(itemPM.Id, tenant);
            ShipmentMapping.MapFollowUp(itemPM, itemPoco, false);
            followUpRepository.Update(itemPoco);
        }
        private void DeleteShipmentFollowUp(ShipmentFollowUpPM itemPM)
        {
            FollowUp itemPoco = followUpRepository.GetSingleFollowUp(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                followUpRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentCarrierStatus(ShipmentCarrierStatusPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentCarrierStatus", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentCarrierStatus itemPoco = new ShipmentCarrierStatus()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapShipmentCarrierStatus(itemPM, itemPoco, true);
            shipmentCarrierStatusRepository.Add(itemPoco);
        }
        private void UpdateShipmentCarrierStatus(ShipmentCarrierStatusPM itemPM)
        {
            ShipmentCarrierStatus itemPoco = shipmentCarrierStatusRepository.GetSingleShipmentCarrierStatus(itemPM.Id, itemPM.Tenant);
            ShipmentMapping.MapShipmentCarrierStatus(itemPM, itemPoco, false);
            shipmentCarrierStatusRepository.Update(itemPoco);
        }
        private void DeleteShipmentCarrierStatus(ShipmentCarrierStatusPM itemPM)
        {
            ShipmentCarrierStatus itemPoco = shipmentCarrierStatusRepository.GetSingleShipmentCarrierStatus(itemPM.Id, itemPM.Tenant);
            shipmentCarrierStatusRepository.Remove(itemPoco);
        }

        private void CreateAWBOCI(AWBOCIPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("AWBOCI", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            AWBOCI itemPoco = new AWBOCI()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapAWBOCI(itemPM, itemPoco, true);
            aWBOCIRepository.Add(itemPoco);
        }
        private void UpdateAWBOCI(AWBOCIPM itemPM)
        {
            AWBOCI itemPoco = aWBOCIRepository.GetSingleAWBOCI(itemPM.Id, itemPM.Tenant);
            ShipmentMapping.MapAWBOCI(itemPM, itemPoco, false);
            aWBOCIRepository.Update(itemPoco);
        }
        private void DeleteAWBOCI(AWBOCIPM itemPM)
        {
            AWBOCI itemPoco = aWBOCIRepository.GetSingleAWBOCI(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                aWBOCIRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentCommodity(ShipmentCommodityPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentCommodity", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentCommodity itemPoco = new ShipmentCommodity()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                ShipmentId = itemPM.ShipmentId,
            };

            ShipmentMapping.MapCommodity(itemPM, itemPoco, true);
            shipmentCommodityRepository.Add(itemPoco);

            if (itemPM.CommodityPackages != null)
            {
                foreach (CommodityPackagePM insideItemPM in itemPM.CommodityPackages)
                {
                    insideItemPM.CommodityId = itemPM.Id;
                    this.CreateCommodityPackage(insideItemPM);
                }
            }

            calculateProfit = true;
            calculatePayables = true;
            calculateReceivables = true;
        }
        private void UpdateShipmentCommodity(ShipmentCommodityPM itemPM)
        {
            ShipmentCommodity itemPoco = shipmentCommodityRepository.GetSingleCommodity(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                ShipmentMapping.MapCommodity(itemPM, itemPoco, false);

                if (itemPM.CommodityPackages != null)
                {
                    foreach (CommodityPackagePM insideItemPM in itemPM.CommodityPackages)
                    {
                        switch (insideItemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    insideItemPM.CommodityId = itemPM.Id;
                                    this.CreateCommodityPackage(insideItemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateCommodityPackage(insideItemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteCommodityPackage(insideItemPM);
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
            }

            calculateProfit = true;
            calculatePayables = true;
            calculateReceivables = true;
        }
        private void DeleteShipmentCommodity(ShipmentCommodityPM itemPM)
        {
            ShipmentCommodity itemPoco = shipmentCommodityRepository.GetSingleCommodity(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                List<ShipmentPackage> list = shipmentPackageRepository.GetShipmentPackagesByCommodityId(entityPM.Id, itemPoco.Id, itemPoco.Tenant).ToList();
                if (list != null)
                {
                    //foreach (ShipmentPackage package in list)
                    //{
                    //    shipmentPackageRepository.Remove(package);
                    //}
                }

                shipmentCommodityRepository.Remove(itemPoco);
            }

            calculateProfit = true;
            calculatePayables = true;
            calculateReceivables = true;
        }

        private void CreateCommodityPackage(CommodityPackagePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentPackage", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentPackage itemPoco = new ShipmentPackage()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                ShipmentId = itemPM.ShipmentId,
            };

            ShipmentMapping.MapCommodityPackage(itemPM, itemPoco, true);
            shipmentPackageRepository.Add(itemPoco);
        }
        private void UpdateCommodityPackage(CommodityPackagePM itemPM)
        {
            ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                ShipmentMapping.MapCommodityPackage(itemPM, itemPoco, false);
            }
        }
        private void DeleteCommodityPackage(CommodityPackagePM itemPM)
        {
            ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                shipmentPackageRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentAssembly(ShipmentAssemblyPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentAssembly", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentAssembly itemPoco = new ShipmentAssembly()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapAssembly(itemPM, itemPoco, true, loggedContact.Id);
            shipmentAssemblyRepository.Add(itemPoco);
        }
        private void UpdateShipmentAssembly(ShipmentAssemblyPM itemPM)
        {
            ShipmentAssembly itemPoco = shipmentAssemblyRepository.GetSingleShipmentAssembly(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                ShipmentMapping.MapAssembly(itemPM, itemPoco, false, loggedContact.Id);
                shipmentAssemblyRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentAssembly(ShipmentAssemblyPM itemPM)
        {
            ShipmentAssembly itemPoco = shipmentAssemblyRepository.GetSingleShipmentAssembly(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                shipmentAssemblyRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentStoragePricing(ShipmentStoragePricingPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentStoragePricing", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentStoragePricing itemPoco = new ShipmentStoragePricing()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapShipmentStoragePricing(itemPM, itemPoco, true);
            shipmentStoragePricingRepository.Add(itemPoco);
        }
        private void UpdateShipmentStoragePricing(ShipmentStoragePricingPM itemPM)
        {
            ShipmentStoragePricing itemPoco = shipmentStoragePricingRepository.GetSingleShipmentStoragePricing(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                ShipmentMapping.MapShipmentStoragePricing(itemPM, itemPoco, false);
                shipmentStoragePricingRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentStoragePricing(ShipmentStoragePricingPM itemPM)
        {
            ShipmentStoragePricing itemPoco = shipmentStoragePricingRepository.GetSingleShipmentStoragePricing(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                shipmentStoragePricingRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentReferance(ShipmentReferancePM itemPM)
        {
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentReferance itemPoco = new ShipmentReferance()
            {
                ShipmentId = itemPM.ShipmentId,
            };

            ShipmentMapping.MapShipmentReferance(itemPM, itemPoco, true);
            shipmentReferanceRepository.Add(itemPoco);
        }
        private void UpdateShipmentReferance(ShipmentReferancePM itemPM)
        {
            ShipmentReferance itemPoco = shipmentReferanceRepository.GetSingleShipmentReferance(itemPM.ShipmentId, itemPM.Tenant, itemPM.LineNumber);
            if (itemPoco != null)
            {
                ShipmentMapping.MapShipmentReferance(itemPM, itemPoco, false);
                shipmentReferanceRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentReferance(ShipmentReferancePM itemPM)
        {
            ShipmentReferance itemPoco = shipmentReferanceRepository.GetSingleShipmentReferance(itemPM.ShipmentId, itemPM.Tenant, itemPM.LineNumber);
            if (itemPoco != null)
            {
                shipmentReferanceRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentProductItem(ShipmentProductItemPM itemPM)
        {
            if (!itemPM.IsEmptyLine)
            {
                itemPM.Id = IdCounter.GetNumber("ShipmentProductItem", tenant).ToString();
                itemPM.ShipmentId = entityPM.Id;
                itemPM.Tenant = tenant;

                ShipmentProductItem itemPoco = new ShipmentProductItem()
                {
                    Id = itemPM.Id,
                };

                ShipmentMapping.MapProductItem(itemPM, itemPoco, true);
                shipmentProductItemRepository.Add(itemPoco);
            }
        }
        private void UpdateShipmentProductItem(ShipmentProductItemPM itemPM)
        {
            ShipmentProductItem itemPoco = shipmentProductItemRepository.GetSingleShipmentProductItem(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                ShipmentMapping.MapProductItem(itemPM, itemPoco, false);
                shipmentProductItemRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentProductItem(ShipmentProductItemPM itemPM)
        {
            ShipmentProductItem itemPoco = shipmentProductItemRepository.GetSingleShipmentProductItem(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                shipmentProductItemRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentUnassignedField(ShipmentUnassignedFieldPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentUnassignedField", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentUnassignedField itemPoco = new ShipmentUnassignedField()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapShipmentUnassignedField(itemPM, itemPoco, true);
            shipmentUnassignedFieldRepository.Add(itemPoco);
        }
        private void UpdateShipmentUnassignedField(ShipmentUnassignedFieldPM itemPM)
        {
            ShipmentUnassignedField itemPoco = shipmentUnassignedFieldRepository.GetSingleShipmentUnassignedField(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                if (!string.IsNullOrEmpty(itemPM.ReplacedDataId))
                {
                    this.HandleUnassignedComputingPartnerTranslation(itemPM);
                }

                ShipmentMapping.MapShipmentUnassignedField(itemPM, itemPoco, false);
                shipmentUnassignedFieldRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentUnassignedField(ShipmentUnassignedFieldPM itemPM)
        {
            ShipmentUnassignedField itemPoco = shipmentUnassignedFieldRepository.GetSingleShipmentUnassignedField(itemPM.Id, itemPM.Tenant);
            if (itemPoco != null)
            {
                shipmentUnassignedFieldRepository.Remove(itemPoco);
            }
        }
        private void HandleUnassignedComputingPartnerTranslation(ShipmentUnassignedFieldPM shipmentUnassignedField)
        {
            Card card = cardRepository.GetSingleCard(shipmentUnassignedField.ReplacedDataId, tenant);
            ComputingPartner partner = this.GetComputingPartner(shipmentUnassignedField.ComputingPartnrCode);

            if (partner == null || card == null)
            {
                return;
            }

            ComputingPartnerTable computingPartnerTable = computingPartnerTableRepository.GetSingleComputingPartnerTable(tenant, shipmentUnassignedField.ObjectTableId, partner.Id);
            if (computingPartnerTable == null)
            {
                return;
            }

            ComputingPartnerTranslation computingPartnerTranslation = computingPartnerTranslationRepository.GetSingleTranslationByOurCode(partner.Id, shipmentUnassignedField.ObjectTableId, card.Code, tenant);
            if (computingPartnerTranslation == null)
            {
                this.CreateNewComputingPartnerTranslation(partner.Id, shipmentUnassignedField.ObjectTableId, card.Code, shipmentUnassignedField.ReceivedCode);
            }

            else
            {
                this.UpdateComputingPartnerTranslation(computingPartnerTranslation, shipmentUnassignedField.ReceivedCode);
            }

            computingPartnerTranslationRepository.SubmitChanges();
        }
        private ComputingPartner GetComputingPartner(string computingPartnrCode)
        {
            ComputingPartner partner = computingPartnerRepository.GetSingleComputingPartnerByCode(computingPartnrCode, tenant);

            if (partner == null)
            {
                partner = computingPartnerRepository.GetSingleComputingPartnerByCode(computingPartnrCode, 0);
            }

            return partner;
        }
        private void CreateNewComputingPartnerTranslation(string partnerId, string tableId, string ourCode, string partnerCode)
        {
            ComputingPartnerTranslation computingPartnerTranslation = new ComputingPartnerTranslation()
            {
                Id = IdCounter.GetNumber("ComputingPartnerTranslation", tenant),
                Tenant = tenant,
                ComputingPartnerId = partnerId,
                ObjectTableId = tableId,
                OurCode = ourCode,
                PartnerCode = partnerCode,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreatedByUserId = loggedContact.Id,
                UpdatedByUserId = loggedContact.Id,
                SearchFields = ourCode + "," + partnerCode,
            };

            computingPartnerTranslationRepository.Add(computingPartnerTranslation);
        }
        private void UpdateComputingPartnerTranslation(ComputingPartnerTranslation computingPartnerTranslation, string partnerCode)
        {
            computingPartnerTranslation.PartnerCode = partnerCode;
            computingPartnerTranslation.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            computingPartnerTranslation.UpdatedByUserId = loggedContact.Id;
            computingPartnerTranslation.SearchFields = computingPartnerTranslation.OurCode + "," + partnerCode;
            computingPartnerTranslationRepository.Update(computingPartnerTranslation);
        }

        private void ComputeShipmentStatus()
        {
            if (isNewEntity)
            {
                entityPM.CalculateStatus = true;
            }

            else
            {
                entityPM.CalculateStatus = true;
            }
        }
        private void ComputeNumerOfFollowUps()
        {
            List<FollowUp> followUps = followUpRepository.GetFollowUpsByShipmentId(entityPM.Id, tenant);
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
        private void ComputeTEU()
        {
            if (entityPM.TransportModeId == "A")
            {
                entityPM.TEU = null;
            }

            else
            {
                double? myResult = null;
                List<ShipmentPackagePM> myPackages = entityPM.ShipmentPackages.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                if (myPackages.Count > 0)
                {
                    myResult = 0;

                    foreach (ShipmentPackagePM item in myPackages)
                    {
                        PackageType myPackageType = PackageTypeRepository.GetSinglePackageType(item.PackageTypeId, tenant, true);
                        if (myPackageType != null)
                        {
                            myResult += myPackageType.TEU;
                        }
                    }
                }

                entityPM.TEU = myResult;
            }
        }
        private void RunRegistryDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentRegistryDate(myShipmentId, entityPM.Tenant);
        }
        private void RunFirstApprovalDateProcedure(string myShipmentId)
        {
            RunStoredProcedureClass.UpdateShipmentFirstApprovalDate(myShipmentId, entityPM.Tenant);
        }

        private void ComputeIsAssemblyField()
        {
            List<ShipmentAssemblyPM> myList = this.entityPM.ShipmentAssemblies.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

            if (myList.Count == 0)
            {
                entityPM.IsAssembly = false;
            }

            else
            {
                entityPM.IsAssembly = true;
            }
        }

        private void ComputeHasUnassignedField()
        {
            List<ShipmentUnassignedFieldPM> myList = this.entityPM.ShipmentUnassignedFields.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

            entityPM.HasUnassignedData = false;

            if (myList.Count > 0 && myList.Where(s => string.IsNullOrEmpty(s.ReplacedDataId)).Any())
            {
                entityPM.HasUnassignedData = true;
            }
        }
        private void ComputeFinalDestination()
        {
            this.ComputeFrom();
            this.ComputeTo();

            List<ShipmentDeliveryPM> deliveries = this.entityPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).ToList();
            List<ShipmentPickUpPM> pickups = this.entityPM.ShipmentPickUps.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete && d.PickUpDeliveryTypeCode == "PICK").OrderBy(s => s.PickUpDeliveryNumber).ToList();

            ShipmentDeliveryPM myLastDelivery = deliveries.FirstOrDefault();
            ShipmentPickUpPM myFirstPickup = pickups.FirstOrDefault();

            this.CountryForStatisticsId(myLastDelivery);
            this.ComputeOrigin(myFirstPickup);

            if (myFirstPickup != null)
            {
                this.entityPM.FirstPickupETA = myFirstPickup.ETA;
                this.entityPM.FirstPickupETD = myFirstPickup.ETD;
                this.entityPM.FirstPickupATD = myFirstPickup.ATD;
            }

            else
            {
                this.entityPM.FirstPickupETA = null;
                this.entityPM.FirstPickupETD = null;
                this.entityPM.FirstPickupATD = null;
            }

            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                if (!string.IsNullOrEmpty(entityPM.MainCarriageToAddressId))
                {
                    Address myPartnerAddress = myAddressRepository.GetSingleAddress(entityPM.MainCarriageToAddressId, tenant);
                    if (myPartnerAddress != null)
                    {
                        this.entityPM.LastFinalDestination = myPartnerAddress.City;
                    }
                }
            }

            else
            {
                if (myLastDelivery != null)
                {
                    switch (myLastDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToAddressId))
                                {
                                    Address myPartnerAddress = myAddressRepository.GetSingleAddress(myLastDelivery.ToAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        this.entityPM.LastFinalDestination = myPartnerAddress.City;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                {
                                    Port myPort = myPortRepository.GetSinglePort(tenant, myLastDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        this.entityPM.LastFinalDestination = myPort.EnglishName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myLastDelivery.ToAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    this.entityPM.LastFinalDestination = myCity;
                                }

                                break;
                            }
                    }
                }

                else if (this.entityPM.DirectionId == "I" && !string.IsNullOrEmpty(this.entityPM.WarehouseLegWarehouseId))
                {
                    Card warehouse = cardRepository.GetSingleCard(this.entityPM.WarehouseLegWarehouseId, tenant);
                    if (warehouse != null)
                    {
                        this.entityPM.LastFinalDestination = warehouse.EnglishName;
                    }
                }

                else if (entityPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(this.entityPM.OnForwardingToPortId))
                {
                    Port onCarriageToPort = myPortRepository.GetSinglePort(tenant, this.entityPM.OnForwardingToPortId);
                    if (onCarriageToPort != null)
                    {
                        this.entityPM.LastFinalDestination = onCarriageToPort.EnglishName;
                    }
                }

                else if (!string.IsNullOrEmpty(this.entityPM.OnCarriageToPortId))
                {
                    Port onCarriageToPort = myPortRepository.GetSinglePort(tenant, this.entityPM.OnCarriageToPortId);
                    if (onCarriageToPort != null)
                    {
                        this.entityPM.LastFinalDestination = onCarriageToPort.EnglishName;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(this.entityPM.Transshipment3ToPortId))
                    {
                        Port transshipment3ToPort = myPortRepository.GetSinglePort(tenant, this.entityPM.Transshipment3ToPortId);
                        if (transshipment3ToPort != null)
                        {
                            this.entityPM.LastFinalDestination = transshipment3ToPort.EnglishName;
                        }
                    }

                    else if (!string.IsNullOrEmpty(this.entityPM.Transshipment2ToPortId))
                    {
                        Port transshipment2ToPort = myPortRepository.GetSinglePort(tenant, this.entityPM.Transshipment2ToPortId);
                        if (transshipment2ToPort != null)
                        {
                            this.entityPM.LastFinalDestination = transshipment2ToPort.EnglishName;
                        }
                    }

                    else if (!string.IsNullOrEmpty(this.entityPM.Transshipment1ToPortId))
                    {
                        Port transshipment1ToPort = myPortRepository.GetSinglePort(tenant, this.entityPM.Transshipment1ToPortId);
                        if (transshipment1ToPort != null)
                        {
                            this.entityPM.LastFinalDestination = transshipment1ToPort.EnglishName;
                        }
                    }

                    else if (!string.IsNullOrEmpty(this.entityPM.MainCarriageToPortId))
                    {
                        Port mainCarriageToPort = myPortRepository.GetSinglePort(tenant, this.entityPM.MainCarriageToPortId);
                        if (mainCarriageToPort != null)
                        {
                            this.entityPM.LastFinalDestination = mainCarriageToPort.EnglishName;
                        }
                    }
                }
            }

            this.ComputeFirstPickupFullAddress(myFirstPickup);
            this.ComputeLastDeliveryFullAddress(myLastDelivery);
        }

        private void CountryForStatisticsId(ShipmentDeliveryPM myLastDelivery)
        {
            string fromPortId = entityPM.MainCarriageFromPortId;
            string toPortId = entityPM.MainCarriageToPortId;

            if (entityPM.ShipmentLevelCode == "H")
            {
                fromPortId = entityPM.FromPortId;
                toPortId = entityPM.ToPortId;
            }

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
                bool Assigned = false;
                if (myLastDelivery != null)
                {
                    switch (myLastDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPartnerCardId))
                                {
                                    AddressRepository addressRepository = new AddressRepository(CommonDataContext.GetContext(entityPM.Tenant));
                                    Address toAddress = addressRepository.GetSingleAddress(myLastDelivery.ToAddressId, entityPM.Tenant);
                                    if (toAddress != null)
                                    {
                                        entityPM.CountryForStatisticsId = toAddress.CountryId;
                                        Assigned = true;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {

                                if (!string.IsNullOrEmpty(myLastDelivery.ToAddressCountryId))
                                {
                                    entityPM.CountryForStatisticsId = myLastDelivery.ToAddressCountryId;
                                    Assigned = true;
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                {
                                    PortPM myPort = PortQuery.GetSinglePort(entityPM.Tenant, myLastDelivery.ToPortId, true);
                                    if (myPort != null)
                                    {
                                        entityPM.CountryForStatisticsId = myPort.CountryId;
                                        Assigned = true;
                                    }
                                }

                                break;
                            }
                    }
                }

                if (!Assigned)
                {
                    if (!string.IsNullOrEmpty(entityPM.Transshipment3ToPortId))
                    {
                        PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.Transshipment3ToPortId, true);
                        if (port != null)
                        {
                            entityPM.CountryForStatisticsId = port.CountryId;
                        }
                    }

                    else if (!string.IsNullOrEmpty(entityPM.Transshipment2ToPortId))
                    {
                        PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.Transshipment2ToPortId, true);
                        if (port != null)
                        {
                            entityPM.CountryForStatisticsId = port.CountryId;
                        }
                    }

                    else if (!string.IsNullOrEmpty(entityPM.Transshipment1ToPortId))
                    {
                        PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, entityPM.Transshipment1ToPortId, true);
                        if (port != null)
                        {
                            entityPM.CountryForStatisticsId = port.CountryId;
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

            }

            //Domestic
            else if (entityPM.DirectionId == "D")
            {
                if (entityPM.TransportModeId == "I")
                {
                    if (!string.IsNullOrEmpty(entityPM.MainCarriageToAddressId))
                    {
                        AddressRepository addressRepository = new AddressRepository(entityPM.Tenant);
                        Address toAddress = addressRepository.GetSingleAddress(entityPM.MainCarriageToAddressId, entityPM.Tenant);
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

            //Customs
            else if (entityPM.DirectionId == "C")
            {
                PortPM port = PortQuery.GetSinglePort(entityPM.Tenant, fromPortId, true);
                if (port != null)
                {
                    entityPM.CountryForStatisticsId = port.CountryId;
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

        private void ComputeFrom()
        {
            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                if (!string.IsNullOrEmpty(entityPM.MainCarriageFromAddressId))
                {
                    Address myPartnerAddress = myAddressRepository.GetSingleAddress(entityPM.MainCarriageFromAddressId, tenant);
                    if (myPartnerAddress != null)
                    {
                        this.entityPM.From = myPartnerAddress.City;
                    }
                }
            }

            else
            {
                string fromId = null;
                if (entityPM.ShipmentLevelCode == "H")
                {
                    fromId = entityPM.FromPortId;
                }

                else
                {
                    fromId = entityPM.MainCarriageFromPortId;
                }

                PortPM portFrom = PortQuery.GetSinglePort(tenant, fromId, true);
                if (portFrom != null)
                {
                    entityPM.From = portFrom.EnglishName;
                }
            }
        }
        private void ComputeTo()
        {
            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                if (!string.IsNullOrEmpty(entityPM.MainCarriageToAddressId))
                {
                    Address myPartnerAddress = myAddressRepository.GetSingleAddress(entityPM.MainCarriageToAddressId, tenant);
                    if (myPartnerAddress != null)
                    {
                        this.entityPM.To = myPartnerAddress.City;
                    }
                }
            }

            else
            {
                string toId = null;
                if (entityPM.ShipmentLevelCode == "H")
                {
                    toId = entityPM.ToPortId;
                }

                else
                {
                    if (!string.IsNullOrEmpty(entityPM.Transshipment3ToPortId))
                    {
                        toId = this.entityPM.Transshipment3ToPortId;
                    }

                    else if (!string.IsNullOrEmpty(entityPM.Transshipment2ToPortId))
                    {
                        toId = this.entityPM.Transshipment2ToPortId;
                    }

                    else if (!string.IsNullOrEmpty(entityPM.Transshipment1ToPortId))
                    {
                        toId = this.entityPM.Transshipment1ToPortId;
                    }

                    else if (!string.IsNullOrEmpty(entityPM.MainCarriageToPortId))
                    {
                        toId = this.entityPM.MainCarriageToPortId;
                    }
                }

                PortPM portTo = PortQuery.GetSinglePort(tenant, toId, true);
                if (portTo != null)
                {
                    entityPM.To = portTo.EnglishName;
                }
            }
        }
        private void ComputeOrigin(ShipmentPickUpPM myFirstPickup)
        {
            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                if (!string.IsNullOrEmpty(entityPM.MainCarriageFromAddressId))
                {
                    Address myPartnerAddress = myAddressRepository.GetSingleAddress(entityPM.MainCarriageFromAddressId, tenant);
                    if (myPartnerAddress != null)
                    {
                        this.entityPM.Origin = myPartnerAddress.City;
                    }
                }
            }

            else
            {
                this.entityPM.FirstPickupLocation = null;
                if (myFirstPickup != null)
                {
                    switch (myFirstPickup.PickUpDeliveryFromTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myFirstPickup.FromAddressId))
                                {
                                    Address myPartnerAddress = myAddressRepository.GetSingleAddress(myFirstPickup.FromAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        this.entityPM.Origin = myPartnerAddress.City;
                                        this.entityPM.FirstPickupLocation = myPartnerAddress.City;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myFirstPickup.FromPortId))
                                {
                                    Port myPort = myPortRepository.GetSinglePort(tenant, myFirstPickup.FromPortId);
                                    if (myPort != null)
                                    {
                                        this.entityPM.Origin = myPort.EnglishName;
                                        this.entityPM.FirstPickupLocation = myPort.EnglishName;

                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myFirstPickup.FromAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    this.entityPM.Origin = myCity;
                                    this.entityPM.FirstPickupLocation = myCity;

                                }

                                break;
                            }
                    }
                }

                else if (this.entityPM.DirectionId == "E" && !string.IsNullOrEmpty(this.entityPM.WarehouseLegWarehouseId))
                {
                    Card warehouse = cardRepository.GetSingleCard(this.entityPM.WarehouseLegWarehouseId, tenant);
                    if (warehouse != null)
                    {
                        this.entityPM.Origin = warehouse.EnglishName;
                    }
                }

                else if (entityPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(entityPM.PreForwardingFromPortId))
                {
                    Port myPort = myPortRepository.GetSinglePort(tenant, entityPM.PreForwardingFromPortId);
                    if (myPort != null)
                    {
                        this.entityPM.Origin = myPort.EnglishName;
                    }
                }

                else if (!string.IsNullOrEmpty(entityPM.PreCarriageFromPortId))
                {
                    Port myPort = myPortRepository.GetSinglePort(tenant, entityPM.PreCarriageFromPortId);
                    if (myPort != null)
                    {
                        this.entityPM.Origin = myPort.EnglishName;
                    }
                }

                else
                {
                    Port myPort = myPortRepository.GetSinglePort(tenant, entityPM.MainCarriageFromPortId);
                    if (myPort != null)
                    {
                        this.entityPM.Origin = myPort.EnglishName;
                    }
                }
            }
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
        private void UpdateHouseRoutingFieldsWhenConnectedToMaster(Shipment houseShipment)
        {
            if (!string.IsNullOrEmpty(entityPM.PreCarriageFromPortId) && !string.IsNullOrEmpty(entityPM.PreCarriageToPortId)
                && !string.IsNullOrEmpty(houseShipment.PreForwardingFromPortId) && !string.IsNullOrEmpty(houseShipment.PreForwardingToPortId))
            {
                houseShipment.PreForwardingToPortId = entityPM.PreCarriageFromPortId;
            }

            if (!string.IsNullOrEmpty(entityPM.OnCarriageFromPortId) && !string.IsNullOrEmpty(entityPM.OnCarriageToPortId)
                && !string.IsNullOrEmpty(houseShipment.OnForwardingFromPortId) && !string.IsNullOrEmpty(houseShipment.OnForwardingToPortId))
            {
                houseShipment.OnForwardingFromPortId = entityPM.OnCarriageToPortId;
            }
        }
        private void UpdateShipmentProductItems()
        {
            if (entityPM.IsProductItemsUpdated)
            {
                HTSCodeQuery hTSCodeQuery = new HTSCodeQuery(tenant);
                foreach (ShipmentProductItemPM productItem in this.entityPM.ShipmentProductItems.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
                {
                    productItem.ChangeSetOp = ChangeSetOperation.Update;
                    HTSCodePM hTSCodePM = hTSCodeQuery.GetSingleHTSCodeByProductItemAndCountry(productItem.ProductItemId, entityPM.ToCountryId, tenant);
                    if (hTSCodePM != null)
                    {
                        productItem.HTSCode = hTSCodePM.Code;
                        productItem.ApprovedByCustomer = hTSCodePM.ApprovedByCustomer;
                        productItem.VATPercentage = hTSCodePM.VATPercentage;
                        productItem.DutiesPercentage = hTSCodePM.DutiesPercentage;
                        productItem.OtherDuties = hTSCodePM.OtherDuties;
                        productItem.Remarks = hTSCodePM.Remarks;
                    }
                    else
                    {
                        productItem.HTSCode = null;
                        productItem.ApprovedByCustomer = false;
                        productItem.VATPercentage = null;
                        productItem.DutiesPercentage = null;
                        productItem.OtherDuties = null;
                        productItem.Remarks = null;
                    }
                }

                entityPM.IsProductItemsUpdated = false;
            }
        }
        private void ComputeIsHTSMissingField()
        {
            List<ShipmentProductItemPM> shipmentProductItem = this.entityPM.ShipmentProductItems.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

            entityPM.IsHTSMissing = false;

            if (shipmentProductItem.Count > 0 && shipmentProductItem.Where(d => string.IsNullOrEmpty(d.HTSCode)).Any())
            {
                entityPM.IsHTSMissing = true;
            }
        }
        private void CopyForwarderShipmentPackagesFromStandalone()
        {
            if (initializer.IsPackageCreatedFromStandaloneShipment)
            {
                string forwarderShipmentId = shipmentPickUpDeliveryRepository.GetSingleShipmentPIdByStandaloneShipmentId(entityPM.Id, tenant);
                if (!string.IsNullOrEmpty(forwarderShipmentId))
                {
                    ShipmentPM forwarderShipment = this.GetShipmentPM(forwarderShipmentId);

                    if (forwarderShipment != null)
                    {
                        this.ValidateUpdatingInsertStandAloneShipmentPackages(initializer.StandalonePackage.ContainerNumber, initializer.StandalonePackage.ContainerEntityId, forwarderShipmentId);
                        this.CreateOrUpdateForwarderShipmentPackage(forwarderShipment);
                        this.UpdateForwarderShipmentPickUpDelivery(forwarderShipment);
                        this.UpdateForwarderShipment(forwarderShipment);

                    }
                }
            }
        }
        private void ValidateUpdatingInsertStandAloneShipmentPackages(string containerNumber, string ContainerEntityId, string forwarderShipmentId)
        {
            List<ShipmentPackage> shipmentPackages = this.initializer.ShipmentPackageRepository.GetShipmentPackagesForShipmentTenant(forwarderShipmentId, tenant).ToList();
            if (shipmentPackages != null)
            {
                bool isContainerNumberExist = shipmentPackages.Any(d => d.ContainerNumber == containerNumber && d.ContainerEntityId != ContainerEntityId);
                if (isContainerNumberExist)
                {
                    throw new ApplicationException("Cannot have 2 containers with same number");
                }
            }
        }

        private void UpdateForwarderShipment(ShipmentPM forwarderShipment)
        {
            ShipmentService shipmentService = new ShipmentService(objectContext, forwarderShipment, "");
            shipmentService.Update(true);
        }
        private void UpdateForwarderShipmentPickUpDelivery(ShipmentPM forwarderShipment)
        {
            ShipmentPickUpDelivery shipmentPickUpDelivery = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDeliveryByStandaloneShipmentId(entityPM.Id, tenant);
            if (shipmentPickUpDelivery != null)
            {
                if (shipmentPickUpDelivery.PickUpDeliveryTypeCode == "PICK")
                {
                    ShipmentPickUpPM forwarderShipmentPickup = forwarderShipment.ShipmentPickUps.Where(d => d.Id == shipmentPickUpDelivery.Id).FirstOrDefault();
                    this.CreatePickUpPackage(forwarderShipmentPickup);
                }

                else
                {
                    ShipmentDeliveryPM forwarderShipmentDelivery = forwarderShipment.ShipmentDeliveries.Where(d => d.Id == shipmentPickUpDelivery.Id).FirstOrDefault();
                    this.CreateDliveryPackage(forwarderShipmentDelivery);
                }

                forwarderShipment.StandaloneShipmentId = shipmentPickUpDelivery.StandaloneShipmentId;
            }
        }
        private void CreatePickUpPackage(ShipmentPickUpPM forwarderShipmentPickup)
        {
            ShipmentPickUpDeliveryPackagePM package = new ShipmentPickUpDeliveryPackagePM()
            {
                Tenant = tenant,
                ContainerNumber = initializer.StandalonePackage.ContainerNumber,
                Description = initializer.StandalonePackage.Description,
                PackageTypeId = initializer.StandalonePackage.PackageTypeId,
                ContainerEntityId = initializer.StandalonePackage.ContainerEntityId,
                Quantity = initializer.StandalonePackage.Quantity,
                Volume = initializer.StandalonePackage.Volume,
                Weight = initializer.StandalonePackage.Weight,
                ChangeSetOp = ChangeSetOperation.Insert,
            };

            this.CreateShipmentPickUpDeliveryPackage(package, forwarderShipmentPickup.Id);
        }
        private void CreateDliveryPackage(ShipmentDeliveryPM forwarderShipmentDelivery)
        {
            ShipmentPickUpDeliveryPackagePM package = new ShipmentPickUpDeliveryPackagePM()
            {
                Tenant = tenant,
                ContainerNumber = initializer.StandalonePackage.ContainerNumber,
                Description = initializer.StandalonePackage.Description,
                PackageTypeId = initializer.StandalonePackage.PackageTypeId,
                Quantity = initializer.StandalonePackage.Quantity,
                ContainerEntityId = initializer.StandalonePackage.ContainerEntityId,
                Volume = initializer.StandalonePackage.Volume,
                Weight = initializer.StandalonePackage.Weight,
                ChangeSetOp = ChangeSetOperation.Insert,
            };

            this.CreateShipmentPickUpDeliveryPackage(package, forwarderShipmentDelivery.Id);
        }
        private void CreateOrUpdateForwarderShipmentPackage(ShipmentPM forwarderShipment)
        {
            ShipmentPackagePM forwarderShipmentPackage = forwarderShipment.ShipmentPackages.Where(d => d.ContainerEntityId == initializer.StandalonePackage.ContainerEntityId).FirstOrDefault();

            if (forwarderShipmentPackage != null)
            {
                this.MapShipmentPackageFromStandAlonePackage(forwarderShipmentPackage, initializer.StandalonePackage);
            }
            else
            {
                this.CreateForwarderShipmentPackage(forwarderShipment);
            }
        }

        private void MapShipmentPackageFromStandAlonePackage(ShipmentPackagePM forwarderShipmentPackage, ShipmentPackagePM standAloneShipmentPackage)
        {
            forwarderShipmentPackage.ChangeSetOp = ChangeSetOperation.Update;
            forwarderShipmentPackage.Description = initializer.StandalonePackage.Description;
            forwarderShipmentPackage.PackageTypeId = initializer.StandalonePackage.PackageTypeId;
            forwarderShipmentPackage.Quantity = initializer.StandalonePackage.Quantity;
            forwarderShipmentPackage.Volume = initializer.StandalonePackage.Volume;
            forwarderShipmentPackage.Weight = initializer.StandalonePackage.Weight;
            forwarderShipmentPackage.ContainerNumber = initializer.StandalonePackage.ContainerNumber;
        }

        private void CreateForwarderShipmentPackage(ShipmentPM forwarderShipment)
        {
            ShipmentPackagePM forwarderShipmentPackage = new ShipmentPackagePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = tenant,
                Description = initializer.StandalonePackage.Description,
                PackageTypeId = initializer.StandalonePackage.PackageTypeId,
                Quantity = initializer.StandalonePackage.Quantity,
                Volume = initializer.StandalonePackage.Volume,
                Weight = initializer.StandalonePackage.Weight,
                ContainerNumber = initializer.StandalonePackage.ContainerNumber,
                ShipmentId = forwarderShipment.Id,
            };
            forwarderShipment.ShipmentPackages.Add(forwarderShipmentPackage);
        }
        private ShipmentPM GetShipmentPM(string forwarderShipmentId)
        {
            ShipmentQuery shipmenQuery = new ShipmentQuery(initializer.Repository);
            return shipmenQuery.GetSinglePM(forwarderShipmentId, tenant);
        }

        private void UpdateConnectedPackages(ShipmentPackagePM itemPM)
        {
            List<ShipmentPackage> shipmentPackages = this.shipmentPackageRepository.GetShipmentsPackagesByContainerIdAndTenant(itemPM.ShipmentId, itemPM.ContainerEntityId, tenant);
            if (shipmentPackages != null)
            {
                foreach (ShipmentPackage shipmentPackage in shipmentPackages)
                {
                    if (shipmentPackage != null)
                        UpdateConnectedStanadAloneShipmentPackages(itemPM, shipmentPackage);
                }
            }
            List<ShipmentPickUpDeliveryPackage> shipmentPickUpDeliveryPackages = this.shipmentPickUpDeliveryPackageRepository.GetShipmentPickUpDeliveryPackagesByContainerIdAndTenant(itemPM.ContainerEntityId, tenant);
            if (shipmentPickUpDeliveryPackages != null)
            {
                foreach (ShipmentPickUpDeliveryPackage shipmentPickUpDeliveryPackage in shipmentPickUpDeliveryPackages)
                {
                    if (shipmentPickUpDeliveryPackage != null)
                        UpdateConnectedPickupDeliveryPackages(itemPM, shipmentPickUpDeliveryPackage);
                }
            }
        }
        private void UpdateConnectedStanadAloneShipmentPackages(ShipmentPackagePM itemPM, ShipmentPackage shipmentPackage)
        {
            shipmentPackage.ContainerNumber = itemPM.ContainerNumber;
            shipmentPackage.Description = itemPM.Description;
            shipmentPackage.PackageTypeId = itemPM.PackageTypeId;
            shipmentPackage.Quantity = itemPM.Quantity;
            shipmentPackage.Volume = itemPM.Volume;
            shipmentPackage.Weight = itemPM.Weight;
            shipmentPackage.ShipperSeal = itemPM.ShipperSeal;
            this.shipmentPackageRepository.Update(shipmentPackage);
        }
        private void UpdateConnectedPickupDeliveryPackages(ShipmentPackagePM itemPM, ShipmentPickUpDeliveryPackage shipmentPickUpDeliveryPackage)
        {
            shipmentPickUpDeliveryPackage.ContainerNumber = itemPM.ContainerNumber;
            shipmentPickUpDeliveryPackage.Description = itemPM.Description;
            shipmentPickUpDeliveryPackage.PackageTypeId = itemPM.PackageTypeId;
            shipmentPickUpDeliveryPackage.Quantity = itemPM.Quantity;
            shipmentPickUpDeliveryPackage.Volume = itemPM.Volume;
            shipmentPickUpDeliveryPackage.Weight = itemPM.Weight;
            shipmentPickUpDeliveryPackage.ShipperSeal = itemPM.ShipperSeal;
            this.shipmentPickUpDeliveryPackageRepository.Update(shipmentPickUpDeliveryPackage);
        }
        private bool IsInlandDomesticShipment(ShipmentPM entityPM)
        {
            return entityPM.DirectionId == "D" && entityPM.TransportModeId == "I";
        }
        private void UpdateShipmentConcectedPackagesByContainerEntityId(ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagePM)
        {
            ShipmentPackagePM shipmentPackagePM = this.entityPM.ShipmentPackages.Find(d => !string.IsNullOrEmpty(d.ContainerEntityId) && d.ContainerEntityId == shipmentPickUpDeliveryPackagePM.ContainerEntityId);
            if (shipmentPackagePM == null)
            {
                return;
            }
            if (this.IsConnectedForwaderShipmentPackageFieldsUpdated(shipmentPackagePM, shipmentPickUpDeliveryPackagePM))
            {
                this.MapUpdatedConnectedForwaderShipmentPackage(shipmentPackagePM, shipmentPickUpDeliveryPackagePM);
                ShipmentValidating.ValidateContainerNumbers(this.entityPM);
                this.UpdateShipmentPackage(shipmentPackagePM);
            }
        }
        private void MapUpdatedConnectedForwaderShipmentPackage(ShipmentPackagePM shipmentPackagePM, ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagePM)
        {
            shipmentPackagePM.ContainerNumber = shipmentPickUpDeliveryPackagePM.ContainerNumber;
            shipmentPackagePM.Description = shipmentPickUpDeliveryPackagePM.Description;
            shipmentPackagePM.PackageTypeId = shipmentPickUpDeliveryPackagePM.PackageTypeId;
            shipmentPackagePM.Quantity = shipmentPickUpDeliveryPackagePM.Quantity;
            shipmentPackagePM.Volume = shipmentPickUpDeliveryPackagePM.Volume;
            shipmentPackagePM.Weight = shipmentPickUpDeliveryPackagePM.Weight;
            shipmentPackagePM.ShipperSeal = shipmentPickUpDeliveryPackagePM.ShipperSeal;
        }
        private bool IsConnectedForwaderShipmentPackageFieldsUpdated(ShipmentPackagePM shipmentPackagePM, ShipmentPickUpDeliveryPackagePM shipmentPickUpDeliveryPackagePM)
        {
            if (shipmentPickUpDeliveryPackagePM.ChangeSetOp == ChangeSetOperation.Update)
            {
                return true;
            }
            bool isUpdated = false;
            isUpdated = shipmentPackagePM.ContainerNumber != shipmentPickUpDeliveryPackagePM.ContainerNumber ? true : isUpdated;
            isUpdated = shipmentPackagePM.Description != shipmentPickUpDeliveryPackagePM.Description ? true : isUpdated;
            isUpdated = shipmentPackagePM.PackageTypeId != shipmentPickUpDeliveryPackagePM.PackageTypeId ? true : isUpdated;
            isUpdated = shipmentPackagePM.Quantity != shipmentPickUpDeliveryPackagePM.Quantity ? true : isUpdated;
            isUpdated = shipmentPackagePM.Volume != shipmentPickUpDeliveryPackagePM.Volume ? true : isUpdated;
            isUpdated = shipmentPackagePM.Weight != shipmentPickUpDeliveryPackagePM.Weight ? true : isUpdated;
            isUpdated = shipmentPackagePM.ShipperSeal != shipmentPickUpDeliveryPackagePM.ShipperSeal ? true : isUpdated;
            return isUpdated;
        }

        private void ChangePickupDliveryNumbersOnShipmentDirectionConverted()
        {
            if (entityPM.ShipmentPickUps.Count > 0)
            {
                this.ChangePickupsNumbers();
            }

            if (entityPM.ShipmentDeliveries.Count > 0)
            {
                this.ChangeDeliveriesNumbers();
            }
        }
        private void ChangePickupsNumbers()
        {
            foreach (ShipmentPickUpPM pickUp in entityPM.ShipmentPickUps.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
            {
                pickUp.PickUpDeliveryNumber = this.GetNewPickupDeliveryNumber(pickUp.PickUpDeliveryNumber, !string.IsNullOrEmpty(pickUp.ParentPickUpDeliveryId));
                pickUp.ChangeSetOp = ChangeSetOperation.Update;
            }
        }
        private void ChangeDeliveriesNumbers()
        {
            foreach (ShipmentDeliveryPM delivery in entityPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete))
            {
                delivery.PickUpDeliveryNumber = this.GetNewPickupDeliveryNumber(delivery.PickUpDeliveryNumber, !string.IsNullOrEmpty(delivery.ParentPickUpDeliveryId));
                delivery.ChangeSetOp = ChangeSetOperation.Update;
            }
        }
        private string GetNewPickupDeliveryNumber(string oldNumber, bool isChild)
        {
            string newNumber = oldNumber;
            int slashesCount = oldNumber.Replace(entityPM.ShipmentNumber, "").Count(t => t == '/');

            if (slashesCount <= 2)
            {
                string[] numberArray = oldNumber.Replace(entityPM.ShipmentNumber, "").Split('/');
                newNumber = entityPM.ShipmentNumber + "/" + Convert.ToInt32(numberArray[1]);

                if (isChild && numberArray.Length > 2)
                {
                    newNumber = entityPM.ShipmentNumber + "/" + Convert.ToInt32(numberArray[1]) + "/" + Convert.ToInt32(numberArray[2]);
                }
            }

            else
            {
                string actualickupdeliveryNumber = oldNumber.Replace(entityPM.OldShipmentNumber + "/", "");
                string[] numberArray = actualickupdeliveryNumber.Split('/');
                newNumber = entityPM.ShipmentNumber + "/" + Convert.ToInt32(numberArray[0]);

                if (isChild && numberArray.Length > 1)
                {
                    newNumber = entityPM.ShipmentNumber + "/" + Convert.ToInt32(numberArray[0]) + "/" + Convert.ToInt32(numberArray[1]);
                }
            }

            return newNumber;
        }

        private void ComputeFirstPickupFullAddress(ShipmentPickUpPM firstShipmentPickup)
        {
            string myResult = null;

            if (firstShipmentPickup != null)
            {
                myResult = this.GetFirstPickupFullAddressFromPickUp(firstShipmentPickup);
            }

            else
            {
                myResult = this.GetPickUpAddressFromMainCarriage();
            }

            this.entityPM.FirstPickupFullAddress = this.TrimLengthTo1000(myResult);
        }
        private void ComputeLastDeliveryFullAddress(ShipmentDeliveryPM lastShipmentDelivery)
        {
            string myResult = null;

            if (lastShipmentDelivery != null)
            {
                myResult = this.GeteLastDeliveryFullAddressFromDelivery(lastShipmentDelivery);
            }

            else
            {
                myResult = this.GetDeliveryAddressFromMainCarriage();
            }

            this.entityPM.LastDeliveryFullAddress = this.TrimLengthTo1000(myResult);
        }
        private string GetFirstPickupFullAddressFromPickUp(ShipmentPickUpPM firstShipmentPickup)
        {
            if (firstShipmentPickup.PickUpDeliveryFromTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(firstShipmentPickup.FromAddressId);
            }

            else if (firstShipmentPickup.PickUpDeliveryFromTypeCode == "PORT")
            {
                return firstShipmentPickup.FromAddress;
            }

            else if (firstShipmentPickup.PickUpDeliveryFromTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(firstShipmentPickup.FromAddressCountryId, firstShipmentPickup.FromAddressCity, firstShipmentPickup.FromAddressZipCode);
            }

            return null;
        }
        private string GeteLastDeliveryFullAddressFromDelivery(ShipmentDeliveryPM lastShipmentDelivery)
        {
            if (lastShipmentDelivery.PickUpDeliveryToTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(lastShipmentDelivery.ToAddressId);
            }

            else if (lastShipmentDelivery.PickUpDeliveryToTypeCode == "PORT")
            {
                return lastShipmentDelivery.ToAddress;
            }

            else if (lastShipmentDelivery.PickUpDeliveryToTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(lastShipmentDelivery.ToAddressCountryId, lastShipmentDelivery.ToAddressCity, lastShipmentDelivery.ToAddressZipCode);
            }

            return null;
        }
        private string GetPickUpAddressFromMainCarriage()
        {
            string myResult = null;

            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                myResult = this.GetPickupFromAddressForInlandDomestic();
            }

            else
            {
                myResult = this.GetMainCarriagePortName(entityPM.MainCarriageFromPortId, entityPM.MainCarriageFromPortName);
            }

            return myResult;
        }
        private string GetDeliveryAddressFromMainCarriage()
        {
            string myResult = null;

            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                myResult = this.GetDeliveryToAddressForInlandDomestic();
            }

            else
            {
                myResult = this.GetMainCarriagePortName(entityPM.MainCarriageFinalDestinationPortId, entityPM.MainCarriageFinalDestinationPortName);
            }

            return myResult;
        }
        private string GetPickupFromAddressForInlandDomestic()
        {
            if (entityPM.InlandDomesticFromTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(entityPM.MainCarriageFromAddressId);
            }

            else if (entityPM.InlandDomesticFromTypeCode == "PORT")
            {
                return entityPM.MainCarriageFromPortAddress;
            }

            else if (entityPM.InlandDomesticFromTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(entityPM.InlandDomesticFromCountryId, entityPM.InlandDomesticFromCity, entityPM.InlandDomesticFromZipCode);
            }

            return null;
        }
        private string GetDeliveryToAddressForInlandDomestic()
        {
            if (entityPM.InlandDomesticToTypeCode == "PART")
            {
                return GetFullAddressByPartnerId(entityPM.MainCarriageToAddressId);
            }

            else if (entityPM.InlandDomesticToTypeCode == "PORT")
            {
                return entityPM.MainCarriageToPortAddress;
            }

            else if (entityPM.InlandDomesticToTypeCode == "CASL")
            {
                return this.GetFullAddressByCASLAddress(entityPM.InlandDomesticToCountryId, entityPM.InlandDomesticToCity, entityPM.InlandDomesticToZipCode);
            }

            return null;
        }
        private string GetFullAddressByPartnerId(string addressId)
        {
            if (string.IsNullOrEmpty(addressId))
            {
                return null;
            }

            Address partnerAddress = myAddressRepository.GetSingleAddress(addressId, tenant);
            return this.GetAddress(partnerAddress);
        }
        private string GetFullAddressByCASLAddress(string countryId, string city, string zipCode)
        {
            string myResult = "";

            if (!string.IsNullOrEmpty(city))
            {
                myResult = city;
            }

            if (!string.IsNullOrEmpty(zipCode))
            {
                myResult = myResult + " " + zipCode;
            }

            if (!string.IsNullOrEmpty(countryId))
            {
                Country country = CountryRepository.GetSingleCountry(countryId, tenant, false);
                if (country != null)
                {
                    myResult = myResult + Environment.NewLine + country.EnglishName;
                }
            }

            return myResult;
        }
        private string GetAddress(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                    }

                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(address.ATTN))
                {
                    resultAddress = resultAddress + "Contact: " + address.ATTN;
                }

                if (!string.IsNullOrEmpty(address.ATTN))
                {
                    resultAddress = resultAddress + Environment.NewLine + "Contact: " + address.ATTN;
                }

                if (!string.IsNullOrEmpty(address.PhoneNumber))
                {
                    resultAddress = resultAddress + Environment.NewLine + "Phone: " + address.PhoneNumber;
                }
            }

            return resultAddress;
        }
        private string GetMainCarriagePortName(string portId, string portName)
        {
            string myResult = portName;

            if (string.IsNullOrEmpty(myResult))
            {
                Port port = myPortRepository.GetSinglePort(portId, tenant);
                myResult = port?.EnglishName;
            }

            return myResult;
        }
        private string TrimLengthTo1000(string fieldValue)
        {
            string myResult = fieldValue;

            if (!string.IsNullOrEmpty(fieldValue))
            {
                if (fieldValue.Length > 1000)
                {
                    myResult = fieldValue.Substring(0, 1000);
                }
            }

            return myResult;
        }
        private void SaveChildEntitiesCustomFields()
        {
            new ShipmentChildEntitiesCustomFieldServices(entityPM, initializer).Save();
            new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = entityPM, ParentEntityId = entityPM.Id, ParentObjectTableName = "Shipment", Tenant = tenant }).Update();
        }
        private void ComputeNumberOfTransshipments()
        {
            if (!string.IsNullOrEmpty(entityPM.Transshipment3FromPortId))
            {
                entityPM.NumberOfTransshipments = 3;
            }

            else if (!string.IsNullOrEmpty(entityPM.Transshipment2FromPortId))
            {
                entityPM.NumberOfTransshipments = 2;
            }

            else if (!string.IsNullOrEmpty(entityPM.Transshipment1FromPortId))
            {
                entityPM.NumberOfTransshipments = 1;
            }

            else
            {
                entityPM.NumberOfTransshipments = null;
            }
        }
    }

    public class NumberOfInsidePackagesHelper
    {
        public string PackageTypeId { get; set; }
        public string PackageTypeName { get; set; }
        public int? Count { get; set; }
    }
    public class NumberOfInsidePackagesResult
    {
        public int NumberOfInsidePackages { get; set; }
        public string NumberOfInsidePackagesDetails { get; set; }
    }
    public class ShipmentChangeTracking
    {
        public ShipmentPM ChangeTrackingPM { get; set; }
        public string EntityChangeFieldXml { get; set; }
        public List<NotifyPropertyChangeValues> NotifyPropertyChangeValuesLists { get; set; }
    }
}

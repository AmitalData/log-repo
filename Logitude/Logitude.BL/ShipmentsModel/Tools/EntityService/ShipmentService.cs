using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EmailAlerts;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.BL.ShipmentsModel.Tools.Validating;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.SystemLogs;
using System.Web;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.BL.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using System.Text;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Data.InfrastructureModel;
using System.Reflection;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.Server.Tools.StorageService;
using Simplog.Server.Infrastructure.Azure;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ShipmentService
    {
        private int tenant;
        private bool isNewEntity;
        private bool calculateProfit;
        private bool calculatePayables;
        private bool calculateReceivables;
        private bool isUpdatingRegistryDate;
        private string serviceContextUser;
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
        private ShipmentComputedFieldsRepository shipmentComputedFieldsRepository;
        private ShipmentAdditionalCloudDataRepository shipmentAdditionalCloudDataRepository;
        private ShipmentAdditionalCloudData shipmentAdditionalCloudData;
        private DocumentsFilingRepository documentsFilingRepository;
        private DocumentRepository documentRepository;
        private DocumentsFilingQuery documentsFilingQuery;
        private DocumentTypeRepository DocTypeReposioty;
        private ShipmentTracing shipmentTracing;
        private Tenant loggedTenant;
        private ContactPM loggedContact;
        private ShipmentAssemblyRepository shipmentAssemblyRepository;
        List<ShipmentPM> housesList = new List<ShipmentPM>();
        private ShipmentComputedFields entityComputedFields;
        private bool IsLCLEntity;
        private bool IsFCLEntity;
        public ShipmentService(IShipmentsContext objectContext, ShipmentPM entityPM, string serviceContextUser)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.serviceContextUser = serviceContextUser;
            this.myCommonContext = CommonDataContext.GetContext(entityPM.Tenant);

            this.entityRepository = new ShipmentRepository(objectContext);
            this.shipmentMasterDataRepository = new ShipmentMasterDataRepository(objectContext);
            this.shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(objectContext);
            this.shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(objectContext);
            this.shipmentOrderPackageRepository = new ShipmentOrderPackageRepository(objectContext);
            this.shipmentPackageRepository = new ShipmentPackageRepository(objectContext);
            this.insideShipmentPackageRepository = new InsideShipmentPackageRepository(objectContext);
            this.shipmentAWBPrintOnlyRepository = new ShipmentAWBPrintOnlyRepository(objectContext);
            this.shipmentReceivableRepository = new ShipmentReceivableRepository(objectContext);
            this.shipmentPayableRepository = new ShipmentPayableRepository(objectContext);
            this.shipmentPackageItemRepository = new ShipmentPackageItemRepository(objectContext);
            this.shipmentPackageHarmonizeRepository = new ShipmentPackageHarmonizeRepository(objectContext);
            this.pickUpDeliveryPackageHarmonizeRepository = new PickUpDeliveryPackageHarmonizeRepository(objectContext);
            this.shipmentCarrierStatusRepository = new ShipmentCarrierStatusRepository(objectContext);
            this.followUpRepository = new FollowUpRepository(tenant);
            this.cardRepository = new CardRepository(myCommonContext);
            this.myAddressRepository = new AddressRepository(myCommonContext);
            this.myPortRepository = new PortRepository(myCommonContext);
            this.aWBOCIRepository = new AWBOCIRepository(objectContext);
            this.shipmentComputedFieldsRepository = new ShipmentComputedFieldsRepository(objectContext);
            this.documentsFilingRepository = new DocumentsFilingRepository(myCommonContext);
            this.shipmentCommodityRepository = new ShipmentCommodityRepository(objectContext);
            this.shipmentAdditionalCloudDataRepository = new ShipmentAdditionalCloudDataRepository(objectContext);
            this.shipmentAssemblyRepository = new ShipmentAssemblyRepository(objectContext);
            this.GetLoggedData();
        }
        private void GetLoggedData()
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            this.loggedContact = contactQuery.GetContactByNameAndTenant(serviceContextUser, tenant, true);

            if (this.loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(serviceContextUser, tenant);
            }

            this.loggedTenant = TenantRepository.GetSingleTenant(tenant, true);
        }

        private List<ShipmentPackagePM> shipmentPackagesChangeSet;
        private List<ShipmentOrderPackagePM> shipmentOrderPackagesChangeSet;
        private List<ShipmentPickUpPM> shipmentPickUpsChangeSet;
        private List<ShipmentDeliveryPM> shipmentDeliveriesChangeSet;
        private List<ShipmentReceivablePM> shipmentReceivablesChangeSet;
        private List<ShipmentPayablePM> shipmentPayablesChangeSet;
        private List<ShipmentFollowUpPM> shipmentFollowUpsChangeSet;
        private List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnliesChangeSet;
        private List<ConsoleShipmentPM> shipmentConsoleShipmentsChangeSet;
        private List<ShipmentCarrierStatusPM> shipmentCarrierStatusesChangeSet;
        private List<AWBOCIPM> aWBOCIPMChangeSet;
        private List<ShipmentCommodityPM> shipmentCommoditiesChangeSet;
        private List<ShipmentAssemblyPM> shipmentAssembliesChangeSet;
        public void SetChangeSet(List<ShipmentPackagePM> shipmentPackagesChangeSet, List<ShipmentOrderPackagePM> shipmentOrderPackagesChangeSet, List<ShipmentPickUpPM> shipmentPickUpsChangeSet, List<ShipmentDeliveryPM> shipmentDeliveriesChangeSet, List<ShipmentReceivablePM> shipmentReceivablesChangeSet, List<ShipmentPayablePM> shipmentPayablesChangeSet, List<ShipmentFollowUpPM> shipmentFollowUpsChangeSet, List<ShipmentAWBPrintOnlyPM> shipmentAWBPrintOnliesChangeSet, List<ConsoleShipmentPM> shipmentConsoleShipmentsChangeSet, List<ShipmentCarrierStatusPM> shipmentCarrierStatusesChangeSet, List<AWBOCIPM> aWBOCIPMChangeSet, List<ShipmentCommodityPM> shipmentCommoditiesChangeSet, List<ShipmentAssemblyPM> shipmentAssembliesChangeSet)
        {
            this.shipmentPackagesChangeSet = shipmentPackagesChangeSet;
            this.shipmentOrderPackagesChangeSet = shipmentOrderPackagesChangeSet;
            this.shipmentPickUpsChangeSet = shipmentPickUpsChangeSet;
            this.shipmentDeliveriesChangeSet = shipmentDeliveriesChangeSet;
            this.shipmentReceivablesChangeSet = shipmentReceivablesChangeSet;
            this.shipmentPayablesChangeSet = shipmentPayablesChangeSet;
            this.shipmentFollowUpsChangeSet = shipmentFollowUpsChangeSet;
            this.shipmentAWBPrintOnliesChangeSet = shipmentAWBPrintOnliesChangeSet;
            this.shipmentConsoleShipmentsChangeSet = shipmentConsoleShipmentsChangeSet;
            this.shipmentCarrierStatusesChangeSet = shipmentCarrierStatusesChangeSet;
            this.aWBOCIPMChangeSet = aWBOCIPMChangeSet;
            this.shipmentCommoditiesChangeSet = shipmentCommoditiesChangeSet;
            this.shipmentAssembliesChangeSet = shipmentAssembliesChangeSet;
        }

        public void Create()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.isNewEntity = true;
                this.calculateProfit = false;
                this.calculatePayables = false;
                this.calculateReceivables = false;
                this.entityPM.Id = IdCounter.GetNumber("Shipment", tenant).ToString();
                this.entityPM.SecurityKey = Guid.NewGuid().ToString("N");
                this.entityPoco = new Shipment() { Id = this.entityPM.Id, SecurityKey = this.entityPM.SecurityKey };

                if (this.entityPM.ShipmentLevelCode == "C")
                {
                    this.entityPM.ProrateReceivables = loggedTenant.ProrateMasterReceivables;
                }

                this.InitializeComponent();

                //this.HandleBackToBack();
                if (!loggedTenant.IsDocumentsArchive)
                {
                    ShipmentValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext, loggedTenant);
                    ShipmentValidating.ValidateRoutingDates(entityPM, entityPM.ShipmentPickUps, entityPM.ShipmentDeliveries);
                }

                foreach (ShipmentOrderPackagePM itemPM in entityPM.ShipmentOrderPackages)
                {
                    this.CreateShipmentOrderPackage(itemPM);
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

                entityPM.CalculateProfit = calculateProfit;
                entityPM.CalculatePayables = calculatePayables;
                entityPM.CalculateReceivables = calculateReceivables;

                if (!entityPM.IsHybrid && !loggedTenant.IsDocumentsArchive)
                {
                    shipmentTracing.BeginTracing();
                }

                this.ComputeIsAssemblyField();
                this.ComputeFinalDestination();

                ShipmentMapping.MapEntity(entityPM, entityPoco, entityMasterData, isNewEntity, entityPM.ShipmentPackages, objectContext);
                this.ComputeAgentComputed(entityPM, entityPoco);
                entityRepository.Add(entityPoco);
                entityRepository.SubmitChanges();

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
                RunAutomation("OnCreate");

                scope.Complete();
            }
        }

        string CustomerChanged = "false";
        private bool isProrateReceivablesPM = false;
        private bool isProrateReceivablesPOCO = false;
        public void Update(bool mapComposition = false)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                #region
                if (mapComposition)
                {
                    foreach (var itemPM in this.entityPM.ShipmentPackages)
                    {
                        itemPM.InsideShipmentPackagesChangeSet = itemPM.InsideShipmentPackages;
                        itemPM.ShipmentPackageItemsChangeSet = itemPM.ShipmentPackageItems;
                        itemPM.ShipmentPackageHarmonizesChangeSet = itemPM.ShipmentPackageHarmonizes;
                    }

                    foreach (var itemPM in this.entityPM.ShipmentPickUps)
                    {
                        itemPM.ShipmentPickUpPackagesChangeSet = itemPM.ShipmentPickUpDeliveryPackages;

                        foreach (var item in itemPM.ShipmentPickUpDeliveryPackages)
                        {
                            item.PickUpDeliveryPackageHarmonizesChangeSet = item.PickUpDeliveryPackageHarmonizes;
                        }
                    }

                    foreach (var itemPM in this.entityPM.ShipmentDeliveries)
                    {
                        itemPM.ShipmentDeliveryPackagesChangeSet = itemPM.ShipmentPickUpDeliveryPackages;
                    }

                    this.shipmentPackagesChangeSet = this.entityPM.ShipmentPackages;
                    this.shipmentOrderPackagesChangeSet = this.entityPM.ShipmentOrderPackages;
                    this.shipmentPickUpsChangeSet = this.entityPM.ShipmentPickUps;
                    this.shipmentDeliveriesChangeSet = this.entityPM.ShipmentDeliveries;
                    this.shipmentReceivablesChangeSet = this.entityPM.ShipmentReceivables;
                    this.shipmentPayablesChangeSet = this.entityPM.ShipmentPayables;
                    this.shipmentFollowUpsChangeSet = this.entityPM.FollowUps;
                    this.shipmentAWBPrintOnliesChangeSet = this.entityPM.ShipmentAWBPrintOnlies;
                    this.shipmentConsoleShipmentsChangeSet = this.entityPM.ShipmentConsoleShipments;
                    this.shipmentCarrierStatusesChangeSet = this.entityPM.ShipmentCarrierStatuses;
                    this.aWBOCIPMChangeSet = this.entityPM.AWBOCIPMs;
                    this.shipmentCommoditiesChangeSet = this.entityPM.ShipmentCommodities;
                    this.shipmentAssembliesChangeSet = this.entityPM.ShipmentAssemblies;
                }

                this.isNewEntity = false;
                this.calculateProfit = false;
                this.calculatePayables = false;
                this.calculateReceivables = false;
                this.entityPoco = entityRepository.GetSingleShipment(entityPM.Id, tenant);
                entityPM.OldStatusValue = entityPoco.StatusId;

                this.entityMasterData = (entityPM.ShipmentLevelCode == "H") ? shipmentMasterDataRepository.GetSingleMasterData(entityPM.MasterShipmentDataId) : shipmentMasterDataRepository.GetSingleMasterData(entityPoco.MasterShipmentDataId);

                if (this.entityPM.ShipmentLevelCode == "C")
                {
                    this.isProrateReceivablesPM = this.entityPM.ProrateReceivables;
                    this.isProrateReceivablesPOCO = this.entityMasterData.ProrateReceivables;
                }

                if (entityPM.IsHybrid)//31-Mar fix for old consoles in hybrid without masterdata id
                {
                    if (entityMasterData == null && entityPoco.ShipmentLevelCode != "H" && !entityPM.ConvertFromDirectToHouse && !entityPM.ConvertFromHouseToDirect)
                    {
                        entityMasterData = new ShipmentMasterData();
                        entityMasterData.Id = entityPM.Id;
                        entityPM.MasterShipmentDataId = entityPM.Id;
                        entityMasterData.MasterShipmentNumber = entityPM.ShipmentNumber;
                        shipmentMasterDataRepository.Add(entityMasterData);
                    }
                }

                if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
                {
                    #region
                    string myOldCustomerId = "";
                    if (entityPM.CustomerId != entityPoco.CustomerId)
                    {
                        myOldCustomerId = entityPoco.CustomerId;
                        CustomerChanged = "true";
                    }
                    this.OldCustomerId = myOldCustomerId;

                    this.InitializeComponent();

                    //this.HandleBackToBack();

                    if (!loggedTenant.IsDocumentsArchive)
                    {
                        ShipmentValidating.Validate(entityPM, entityPoco, isNewEntity, myCommonContext, loggedTenant);
                        ShipmentValidating.ValidateRoutingDates(entityPM, this.shipmentPickUpsChangeSet, this.shipmentDeliveriesChangeSet);
                    }

                    this.UpdateShipmentOrderPackagesCollection();
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
                    this.InitializeBookingData();

                    entityPM.CalculateProfit = calculateProfit;
                    entityPM.CalculatePayables = calculatePayables;
                    entityPM.CalculateReceivables = calculateReceivables;

                    if (!entityPM.IsHybrid && !loggedTenant.IsDocumentsArchive)
                    {
                        shipmentTracing.BeginTracing();
                    }

                    if (string.IsNullOrEmpty(entityPM.CustomFileId) && !string.IsNullOrEmpty(entityPoco.CustomFileId))
                    {
                        entityPM.CustomFilePocoId = entityPoco.CustomFileId;
                    }

                    List<ShipmentPackagePM> myPackagesList = new List<ShipmentPackagePM>();
                    if (shipmentPackagesChangeSet != null)
                    {
                        myPackagesList = shipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
                    }

                    this.BuildShipmentExternalUpdate();
                    this.ComputeIsAssemblyField();
                    this.ComputeFinalDestination();
                    this.CheckUpdatingMasterHouses();

                    RunAutomation("OnUpdate");

                    this.UpdateShipmentFollowUpsCollection();

                    UpdateShipmentComputedFields();


                    ShipmentMapping.MapEntity(entityPM, entityPoco, entityMasterData, isNewEntity, myPackagesList, objectContext);
                    this.ComputeAgentComputed(entityPM, entityPoco);
                    entityRepository.Update(entityPoco);
                    entityRepository.SubmitChanges();
                    shipmentAdditionalCloudDataRepository.SubmitChanges();
                    followUpRepository.SubmitChanges();
                    shipmentPickUpDeliveryRepository.SubmitChanges();
                    
                    this.ApplyUpdatingMasterHouses();

                    RunStoredProcedures();
                    GetForeignFields();
                    BuildActivityLog();
                    BuildImportersQueue();
                    #endregion
                }

                else
                {
                    #region
                    // cancelled shipments
                    // in case follow ups added
                    // from client
                    int indexComp = 0;
                    if (shipmentFollowUpsChangeSet != null)
                    {
                        foreach (ShipmentFollowUpPM item in shipmentFollowUpsChangeSet)
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

                UpdateWareHouseEntry();
                scope.Complete();
                #endregion
            }

            if (this.deletedHousesIds.Count > 0)
            {
                foreach (string myShipmentId in this.deletedHousesIds)
                {
                    this.RunRegistryDateProcedure(myShipmentId);

                    RunStoredProcedureClass.UpdateShipmentFinalArrivalDate(myShipmentId, tenant);

                    if (entityPM != null && !entityPM.IsHybrid)
                    {
                        UpdateShipmentProfitClass.UpdateProfitFunction(myShipmentId, tenant, false);
                    }
                }
            }
        }

        private void UpdateShipmentComputedFields()
        {
            if (entityComputedFields != null)
            {
                shipmentComputedFieldsRepository.Update(entityComputedFields);
            }

            entityPM.IsShipmentComputedFieldChange = false;

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

        private void UpdateWareHouseEntry()
        {
            List<WarehouseEntry> warehouseEntryLists = null;
            string fromPortId = String.Empty;
            string toPortId = String.Empty;

            if (entityPM != null)
            {
                WarehouseEntryRepository warehouseEntryRepository = new WarehouseEntryRepository(entityPM.Tenant);

                IQueryable<WarehouseEntry> allWarehouseEntryList = warehouseEntryRepository.GetWarehouseEntriesByshipmentId(entityPM.Id, entityPM.Tenant);

                if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
                {
                    warehouseEntryLists = allWarehouseEntryList.Where(d => d.FromAddressId != this.entityPM.MainCarriageFromAddressId || d.ToAddressId != this.entityPM.MainCarriageToAddressId || d.FromPartnerId != this.entityPM.MainCarriageFromPartnerId || d.ToAddressId != this.entityPM.MainCarriageToPartnerId).ToList();
                }
                else
                {
                    fromPortId = !string.IsNullOrEmpty(this.entityPM.MainCarriageFromPortId) ? this.entityPM.MainCarriageFromPortId : this.entityPM.FromPortId;
                    toPortId = this.entityPM.ShipmentLevelCode == "H" ? this.entityPM.MainCarriageFinalDestinationPortId : this.entityPM.FinalDistenationPortId;
                    warehouseEntryLists = allWarehouseEntryList.Where(d => d.FromPortId != fromPortId || d.ToPortId != toPortId).ToList();
                }

                if (warehouseEntryLists != null && warehouseEntryLists.Count > 0)
                {
                    foreach (WarehouseEntry warehouseEntry in warehouseEntryLists)
                    {
                        if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
                        {
                            warehouseEntry.FromAddressId = this.entityPM.MainCarriageFromAddressId;
                            warehouseEntry.ToAddressId = this.entityPM.MainCarriageToAddressId;
                            warehouseEntry.FromPartnerId = this.entityPM.MainCarriageFromPartnerId;
                            warehouseEntry.ToPartnerId = this.entityPM.MainCarriageToPartnerId;
                        }
                        else
                        {
                            warehouseEntry.FromPortId = fromPortId;
                            warehouseEntry.ToPortId = toPortId;
                        }

                        warehouseEntryRepository.Update(warehouseEntry);
                    }

                    warehouseEntryRepository.SubmitChanges();
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
                    Note = follow.Notes,
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

            entityRepository.Remove(this.entityPoco);
            entityRepository.SubmitChanges();
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

            entityPM.StatusName = null;
            entityPM.StatusWeight = 0;
            if (entityPoco.StatusId != null)
            {
                EntityStatus iEntityStatus = EntityStatusRepository.GetSingleEntityStatus(entityPoco.StatusId, entityPoco.Tenant, true);
                if (iEntityStatus != null)
                {
                    entityPM.StatusName = iEntityStatus.Name;
                    entityPM.StatusWeight = iEntityStatus.StatusWeight;
                }
            }

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
                if (serviceContextUser.Contains("system@tenant"))
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
                    if (IsShipmentMatchLogBoxConditions(loggedTenant,entityPM))
                    {
                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, entityPM.CustomerId);

                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0)
                        {
                            var ImporterTenant = customerTenantAccessInfo.CustomerTenant;
                            IQueueService queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", customerTenantAccessInfo.CustomerTenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() }, { "CustomerId", entityPM.CustomerId } }, null, entityPM.CustomerId);
                        }
                    }
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
                    if (IsShipmentMatchLogBoxConditions(loggedTenant, entityPM))
                    {
                        CustomerTenantAccessQuery customerTenantAccessQuery = new CustomerTenantAccessQuery(tenant);
                        CustomerTenantAccessInfo customerTenantAccessInfo = customerTenantAccessQuery.GetCustomerTenantAccessInfo(tenant, entityPM.CustomerId);

                        if (customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0)
                        {
                            var ImporterTenant = customerTenantAccessInfo.CustomerTenant;
                            IQueueService queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", customerTenantAccessInfo.CustomerTenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() }, { "CustomerId", !string.IsNullOrEmpty(OldCustomerId) ? OldCustomerId : entityPM.CustomerId }, { "CustomerChanged", CustomerChanged } }, null, entityPM.CustomerId);
                        }
                        else if ((customerTenantAccessInfo == null || customerTenantAccessInfo.HasAccess == false) && !string.IsNullOrEmpty(entityPoco.CustomerShipmentNumber) && !string.IsNullOrEmpty(OldCustomerId))
                        {
                            //var ImporterTenant = customerTenantAccessInfo.CustomerTenant;
                            IQueueService queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", entityPoco.CustomerTenantNumber.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() }, { "CustomerId", !string.IsNullOrEmpty(OldCustomerId) ? OldCustomerId : entityPM.CustomerId }, { "CustomerChanged", CustomerChanged } }, null, entityPM.CustomerId);
                        }
                        else if (!string.IsNullOrEmpty(OldCustomerId) && customerTenantAccessInfo != null && customerTenantAccessInfo.HasAccess && customerTenantAccessInfo.CustomerTenant != 0)
                        {
                            var ImporterTenant = entityPM.CustomerTenantNumber;
                            IQueueService queueservice = new DbQueueService();
                            queueservice.InitializeQueue("ImportersShipmentQueue", 0);
                            queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "ImporterTenant", customerTenantAccessInfo.CustomerTenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() }, { "CustomerId", !string.IsNullOrEmpty(OldCustomerId) ? OldCustomerId : entityPM.CustomerId }, { "CustomerChanged", CustomerChanged } }, null, entityPM.CustomerId);
                        }
                    }
                    if (loggedTenant.IsDocumentsArchive && !entityPM.DontAddToForwarderQueue && ((entityPM.StatusName.ToLower() == "in progress" && string.IsNullOrEmpty(entityPM.ForwarderShipmentNumber)) || entityPM.SendUpdatesToAgentEnabled))
                    {
                        IQueueService queueservice = new DbQueueService();
                        queueservice.InitializeQueue("ForwarderShipmentQueue", 0);
                        queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", entityPM.Id }, { "Tenant", tenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() } });
                    }
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

        private bool IsShipmentMatchLogBoxConditions(Tenant loggedTenant,ShipmentPM entityPM)
        {
            if (!entityPM.DontAddToImportersQueue && !loggedTenant.IsDocumentsArchive && !entityPM.IsCancelled && loggedTenant.IsCustomerTenantShare && (entityPM.DirectionId.ToUpper() == "C" || IsImportShipmentsAllowedForLogBox(loggedTenant,entityPM) || IsExportShipmentsAllowedForLogBox(loggedTenant, entityPM)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsImportShipmentsAllowedForLogBox(Tenant loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareImportFile == true)
            {
                return (entityPM.DirectionId.ToUpper() == "I");
            }
            else
            {
                return false;
            }
        }

        private bool IsExportShipmentsAllowedForLogBox(Tenant loggedTenant, ShipmentPM entityPM)
        {
            if (loggedTenant.CustomerTenantShareExportFile == true && FeatureToggleHelper.HasFeatureToggle("LEX", loggedTenant.Id))
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
            RunStoredProcedureClass.UpdateShipmentFinalArrivalDate(entityPM.Id, entityPM.Tenant);

            if (!string.IsNullOrEmpty(entityPM.CustomFilePocoId))
            {
                RunStoredProcedureClass.UpdateCustomConnectToShipment(entityPM.CustomFilePocoId, entityPM.Tenant);
            }

            bool isReloadingConsoles = false;

            if (this.entityPM.ShipmentLevelCode == "C")
            {
                if (this.isProrateReceivablesPM != this.isProrateReceivablesPOCO)
                {
                    this.entityPM.CalculateProfit = true;
                    this.entityPM.CalculateReceivables = true;
                    this.isUpdatingRegistryDate = true;
                }
            }

            if (entityPM.CalculatePayables)
            {
                if (this.entityPM.ShipmentLevelCode != "D")
                {
                    UpdateShipmentProfitClass.UpdatePayables(entityPM.Id, entityPM.Tenant, false);

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

            if (this.isUpdatingRegistryDate)
            {
                this.RunRegistryDateProcedure(this.entityPM.Id);
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
                entityPM.FinalArrivalDate = updatedPOCO.FinalArrivalDate;
                entityPM.EstimatedFinalArrivalDate = updatedPOCO.EstimatedFinalArrivalDate;
                entityPM.ActualFinalArrivalDate = updatedPOCO.ActualFinalArrivalDate;
                entityPM.RegistryDate = updatedPOCO.RegistryDate;
            }

            if (isReloadingConsoles)
            {
                ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(updatedEntityContext);
                shipmentConsoleShipmentQuery.BuildConsoleShipments(entityPM);
            }
        }

        //private string GetForeignPartnerCountryCode(ShipmentPM entityPM)
        //{
        //    CardRepository cardRepository = new CardRepository(entityPM.Tenant);
        //    Card card = null;
        //    string foreignPartnerCountryCode = "";
        //    if (entityPM.DirectionId == "I" || entityPM.DirectionId == "C")
        //    {
        //        card = cardRepository.GetSingleCardWithoutInclude(entityPM.ShipperId, entityPM.Tenant);
        //        foreignPartnerCountryCode = card != null ? card.CountryCode : "";
        //    }
        //    else
        //    {
        //        card = cardRepository.GetSingleCardWithoutInclude(entityPM.ConsigneeId, entityPM.Tenant);
        //        foreignPartnerCountryCode = card != null ? card.CountryCode : "";
        //    }

        //    return foreignPartnerCountryCode;
        //}

        private void InitializeShipmentDocument()
        {
            if (loggedTenant.IsDocumentsArchive == true)
            {

                try
                {
                    if (myCommonContext == null)
                    {
                        myCommonContext = CommonDataContext.GetContext(entityPM.Tenant);
                    }

                    documentsFilingRepository = new DocumentsFilingRepository(myCommonContext);
                    documentRepository = new DocumentRepository(myCommonContext);
                    documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);
                    DocTypeReposioty = new DocumentTypeRepository(myCommonContext);
                    ObjectTableRepository tableRep = new ObjectTableRepository(entityPM.Tenant);
                    ObjectTable table = tableRep.GetObjectTableByName("Shipment", 0, true);
                    DocumentType invoiceDocType = DocTypeReposioty.GetSingleDocumentTypeByCode("380", tenant);
                    DocumentType CustomsInvoiceDocType = DocTypeReposioty.GetSingleDocumentTypeByCode("CINV", tenant);
                    DocumentType DeclerationDocType = DocTypeReposioty.GetSingleDocumentTypeByCode("DEC", tenant);

                    string loggedUserEmail = AuthenticationUtil.GetAuthenticatedUser();
                    UserRepository userRepository = new UserRepository(entityPM.Tenant);
                    User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, entityPM.Tenant, true);
                    if (loggedUser == null)
                    {
                        loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, "system@tenant" + entityPM.Tenant + ".com", entityPM.Tenant, true);
                    }
                    DocumentsFilingService service = new DocumentsFilingService(myCommonContext, entityPM.Tenant);
                    DocumentsFilingPM newDocument;
                    // After New Customs’s Production
                    //DocumentsFilingPM newDocument = new DocumentsFilingPM()
                    //{
                    //    Description = "Supplier Invoice",
                    //    DocumentTypeId = invoiceDocType.Id,
                    //    Tenant = entityPM.Tenant,
                    //    DirectionCode = "I",
                    //    EntityId = entityPM.Id,
                    //    EntityNumber = entityPM.ShipmentNumber,
                    //    ObjectTableId = table.Id,
                    //};

                    //newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", newDocument.Tenant).ToString();
                    //newDocument.CreatedByUserId = loggedUser.Id;
                    //newDocument.OwnerId = loggedUser.Id;
                    //newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);
                    //newDocument.UpdatedByUserId = loggedUser.Id;
                    //newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant); 
                    //service.Create(newDocument, null);
                    if (CustomsInvoiceDocType != null)
                    {
                        newDocument = new DocumentsFilingPM()
                        {
                            Description = "Customs Invoice",
                            DocumentTypeId = CustomsInvoiceDocType.Id,
                            Tenant = entityPM.Tenant,
                            DirectionCode = "I",
                            EntityId = entityPM.Id,
                            EntityNumber = entityPM.ShipmentNumber,
                            ObjectTableId = table.Id,
                        };

                        newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", newDocument.Tenant).ToString();
                        newDocument.CreatedByUserId = loggedUser.Id;
                        newDocument.OwnerId = loggedUser.Id;
                        newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);
                        newDocument.UpdatedByUserId = loggedUser.Id;
                        newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);
                        service.Create(newDocument, null);
                    }
                    if (DeclerationDocType != null)
                    {
                        newDocument = new DocumentsFilingPM()
                        {
                            Description = "Declaration Form",
                            DocumentTypeId = DeclerationDocType.Id,
                            Tenant = entityPM.Tenant,
                            DirectionCode = "I",
                            EntityId = entityPM.Id,
                            EntityNumber = entityPM.ShipmentNumber,
                            ObjectTableId = table.Id,
                        };

                        newDocument.Code = CodeCounter.GetNumber("DocumentsFiling", newDocument.Tenant).ToString();
                        newDocument.CreatedByUserId = loggedUser.Id;
                        newDocument.OwnerId = loggedUser.Id;
                        newDocument.CreateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);
                        newDocument.UpdatedByUserId = loggedUser.Id;
                        newDocument.UpdateDate = TenantServerConfigration.GetCurrentDateTime(newDocument.Tenant);
                        service.Create(newDocument, null);
                    }

                    //After New Customs’s Production
                    //if (entityPM.TransportModeId == "A")
                    //{
                    //    DocumentType airDocType = DocTypeReposioty.GetSingleDocumentTypeByCode("714", tenant);

                    //    DocumentsFilingPM Air = new DocumentsFilingPM()
                    //    {
                    //        Description = "HAWB",
                    //        DocumentTypeId = airDocType.Id,
                    //        Tenant = entityPM.Tenant,
                    //        DirectionCode = "I",
                    //        EntityId = entityPM.Id,
                    //        EntityNumber = entityPM.ShipmentNumber,
                    //        ObjectTableId = table.Id,
                    //    };

                    //    Air.Code = CodeCounter.GetNumber("DocumentsFiling", Air.Tenant).ToString();
                    //    Air.CreatedByUserId = loggedUser.Id;
                    //    Air.OwnerId = loggedUser.Id;
                    //    Air.CreateDate = TenantServerConfigration.GetCurrentDateTime(Air.Tenant);
                    //    Air.UpdatedByUserId = loggedUser.Id;
                    //    Air.UpdateDate = TenantServerConfigration.GetCurrentDateTime(Air.Tenant);


                    //    service.Create(Air, null);
                    //}
                    //else if (entityPM.TransportModeId == "O")
                    //{
                    //    DocumentType OceanDocType = DocTypeReposioty.GetSingleDocumentTypeByCode("706", tenant);

                    //    DocumentsFilingPM Ocean = new DocumentsFilingPM()
                    //    {
                    //        Description = "Sea WayBill",
                    //        DocumentTypeId = OceanDocType.Id,
                    //        Tenant = entityPM.Tenant,
                    //        DirectionCode = "I",
                    //        EntityId = entityPM.Id,
                    //        EntityNumber = entityPM.ShipmentNumber,
                    //        ObjectTableId = table.Id,
                    //    };

                    //    Ocean.Code = CodeCounter.GetNumber("DocumentsFiling", Ocean.Tenant).ToString();
                    //    Ocean.CreatedByUserId = loggedUser.Id;
                    //    Ocean.OwnerId = loggedUser.Id;
                    //    Ocean.CreateDate = TenantServerConfigration.GetCurrentDateTime(Ocean.Tenant);
                    //    Ocean.UpdatedByUserId = loggedUser.Id;
                    //    Ocean.UpdateDate = TenantServerConfigration.GetCurrentDateTime(Ocean.Tenant);


                    //    service.Create(Ocean, null);
                    //}

                    myCommonContext.SaveChanges();
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    string Error = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            //ve.PropertyName, ve.ErrorMessage);

                            Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                        }
                    }


                    string authenticateduser = "";

                    try
                    {
                        authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
                    }

                    catch
                    {
                        authenticateduser = "UnKnown";
                    }
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
                    ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                    throw new Exception(Error);
                }


            }

        }

        private void UpdateShipmentOrderPackagesCollection()
        {
            if (shipmentOrderPackagesChangeSet != null)
            {
                foreach (ShipmentOrderPackagePM itemPM in shipmentOrderPackagesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentOrderPackage(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentOrderPackage(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentOrderPackage(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void UpdateShipmentPackagesCollection()
        {
            if (shipmentPackagesChangeSet != null)
            {
                foreach (ShipmentPackagePM itemPM in shipmentPackagesChangeSet)
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
        private void UpdateShipmentPickUpsCollection()
        {
            if (shipmentPickUpsChangeSet != null)
            {
                foreach (ShipmentPickUpPM itemPM in shipmentPickUpsChangeSet)
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
            if (shipmentDeliveriesChangeSet != null)
            {
                foreach (ShipmentDeliveryPM itemPM in shipmentDeliveriesChangeSet)
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
            if (shipmentPayablesChangeSet != null)
            {
                foreach (ShipmentPayablePM itemPM in shipmentPayablesChangeSet)
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
            if (shipmentReceivablesChangeSet != null)
            {
                foreach (ShipmentReceivablePM itemPM in shipmentReceivablesChangeSet)
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
            if (shipmentAWBPrintOnliesChangeSet != null)
            {
                foreach (ShipmentAWBPrintOnlyPM itemPM in shipmentAWBPrintOnliesChangeSet)
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
            if (shipmentConsoleShipmentsChangeSet != null)
            {
                foreach (ConsoleShipmentPM itemPM in shipmentConsoleShipmentsChangeSet)
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
            if (shipmentFollowUpsChangeSet != null)
            {
                bool isChange = false;

                if (changeSet == "InSert")
                {
                    #region Insert FollowUp
                    List<FollowUp> doneFollowUps = new List<FollowUp>();
                    foreach (ShipmentFollowUpPM itemPM in shipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert))
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
                        });

                    }
                    if (isChange) followUpRepository.SubmitChanges();
                    #endregion

                    #region Done FollowUp
                    foreach (ShipmentFollowUpPM itemPM in shipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Insert && d.Done))
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
                    foreach (ShipmentFollowUpPM itemPM in shipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Delete))
                    {
                        this.DeleteShipmentFollowUp(itemPM);
                        isChange = true;
                    }

                    foreach (ShipmentFollowUpPM itemPM in shipmentFollowUpsChangeSet.Where(d => d.ChangeSetOp == ChangeSetOperation.Update))
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
            if (shipmentCarrierStatusesChangeSet != null)
            {
                foreach (ShipmentCarrierStatusPM itemPM in shipmentCarrierStatusesChangeSet)
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
            if (aWBOCIPMChangeSet != null)
            {
                foreach (AWBOCIPM itemPM in aWBOCIPMChangeSet)
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
            if (shipmentCommoditiesChangeSet != null)
            {
                foreach (ShipmentCommodityPM itemPM in shipmentCommoditiesChangeSet)
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
            if (shipmentAssembliesChangeSet != null)
            {
                foreach (ShipmentAssemblyPM itemPM in shipmentAssembliesChangeSet)
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
                                queueservice.Send(new Dictionary<string, string>() { { "EntityId", agentSharedDocument.Id }, { "Tenant", sourceAgentTenantPOCO.Id.ToString() }, { "AgentTenant", agentSharedDocument.Tenant.ToString() } }, null, null, null, null);


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
            EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("CFSM", tenant);

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
        private void HandleBackToBack()
        {
            foreach (ShipmentPayablePM item in this.entityPM.ShipmentPayables.Where(d => d.IsBackToBack))
            {
                ShipmentReceivablePM myBackToBackReceivable = null;

                if (item.Id != null && item.ReceivableId != null)
                {
                    myBackToBackReceivable = entityPM.ShipmentReceivables.Where(d => d.Id == item.ReceivableId).FirstOrDefault();
                }
                switch (item.ChangeSetOp)
                {
                    case ChangeSetOperation.Delete:
                        {
                            if (myBackToBackReceivable != null)
                            {
                                if (myBackToBackReceivable.ARInvoiceId == null)
                                {
                                    myBackToBackReceivable.ChangeSetOp = ChangeSetOperation.Delete;
                                }
                            }
                            break;
                        }

                    case ChangeSetOperation.Update:
                        {
                            if (myBackToBackReceivable != null)
                            {
                                if (myBackToBackReceivable.ARInvoiceId == null)
                                {
                                    myBackToBackReceivable.ChangeSetOp = ChangeSetOperation.Update;
                                    // map payable to receivable
                                    this.MapReceivable(item, myBackToBackReceivable);
                                }
                            }

                            break;
                        }

                    case ChangeSetOperation.Insert:
                        {
                            if (myBackToBackReceivable == null)
                            {
                                // create new
                                myBackToBackReceivable = new ShipmentReceivablePM();
                                myBackToBackReceivable.ChangeSetOp = ChangeSetOperation.Insert;
                                myBackToBackReceivable.Id = IdCounter.GetNumber("ShipmentReceivable", tenant).ToString();
                                myBackToBackReceivable.IsBackToBack = true;

                                item.ReceivableId = myBackToBackReceivable.Id;
                                item.IsBackToBack = true;
                                // map payable to receivable
                                this.MapReceivable(item, myBackToBackReceivable);
                                this.entityPM.ShipmentReceivables.Add(myBackToBackReceivable);
                            }

                            break;
                        }
                }
            }
        }
        private void MapReceivable(ShipmentPayablePM payable, ShipmentReceivablePM receivable)
        {
            receivable.Tenant = payable.Tenant;
            receivable.ShipmentId = payable.ShipmentId;
            receivable.ShipmentNumber = payable.ShipmentNumber;
            receivable.ChargesTypeId = payable.ChargesTypeId;
            receivable.ChargesTypeName = payable.ChargesTypeName;
            receivable.MeasurementId = payable.MeasurementId;
            receivable.Quantity = payable.Quantity;
            receivable.CurrencyId = payable.CurrencyId;
            receivable.CurrencyCode = payable.CurrencyCode;
            receivable.UnitPrice = payable.UnitPrice;
            receivable.TotalAmount = payable.ExpectedAmount;
            receivable.Rate = payable.Rate;
            receivable.TotalAmountLocal = payable.ExpectedAmountLocal;
            receivable.Notes = payable.Notes;
            receivable.UpdateByUserId = payable.UpdateByUserId;
            receivable.UpdateDate = payable.UpdateDate;
            receivable.PrepaidCollectId = payable.PrepaidCollectId;
            receivable.AWBPrint = payable.AWBPrint;
            receivable.DueTypeCode = payable.DueTypeCode;
            receivable.AmountInProfitCurrency = payable.ExpectedAmountInProfitCurrency;
            receivable.ProfitCurrencyExchangeRate = payable.ProfitCurrencyExchangeRate;
            receivable.CreateDate = payable.CreateDate;
            receivable.CreatedByUserId = payable.CreatedByUserId;
            receivable.IATACodeId = payable.IATACodeId;
            receivable.IsFromQuote = payable.IsFromQuote;
            receivable.QuoteChargeId = payable.QuoteChargeId;
            receivable.IsChargeBySteps = payable.IsChargeBySteps;
            receivable.VatTypeId = payable.VatTypeId;
            receivable.IsBackToBack = payable.IsBackToBack;
            receivable.ShipmentReceivableLineStatusCode = SetReceivableLineStatus(receivable.ShipmentReceivableLineStatusCode, receivable.Quantity, receivable.UnitPrice);
        }
        public string SetReceivableLineStatus(string code, double? quantity, double? unitPrice)
        {
            var status = code;
            if (code == "APPD" || code == "ACCT" || code == "DRFT")
            {

            }

            else
            {
                if (quantity != null && unitPrice != null)
                {
                    status = "OAMT";
                }

                else
                {
                    status = "EMPT";
                }
            }
            return status;
        }

        private void RunAutomation(string type)
        {
            if (entityPM != null)
            {
                DateTime? dateBefore = DateTime.Now;
                string tableName = entityPM.ShipmentLevelCode == "C" ? "Master" : entityPM.ShipmentLevelCode == "H" ? "Shipment" : "MasterAndHouse";
                EntityChangeHelper entityChangeHelper = new EntityChangeHelper();
                if (type == "OnCreate")
                {
                    bool isHaveAutomation = entityChangeHelper.CheckIfEntityHaveAutomation(tableName, "OnCreate", entityPM.Tenant);
                    if (isHaveAutomation)
                    {
                        entityChangeHelper.AddEntityChange(entityPM, null, "OnCreate", "", tableName, dateBefore);
                    }
                }

                else if (!entityPM.IsUpdateByAutomation && type == "OnUpdate")
                {
                    ShipmentPM changeTrackingPM = new ShipmentPM();
                    ShipmentQuery query = new ShipmentQuery(tenant);
                    query.MapShipmentToShipmentPMForAutomation(changeTrackingPM, this.entityPoco, null, this.entityMasterData);
                    changeTrackingPM.StatusId = entityPM.OldStatusValue;

                    if (entityPM.ShipmentLevelCode == "H")
                    {
                        changeTrackingPM.StatusId = entityPM.StatusId;
                    }

                    List<NotifyPropertyChangeValues> notifyPropertyChangeValuesLists = ShipmentMapping.BuildChangedProperties(entityPM, changeTrackingPM);
                    string entityChangeFieldXml = EntityPMChangeTrackingHelper.GetChangesDetectedXml(notifyPropertyChangeValuesLists);
                    bool isHaveAutomation = entityChangeHelper.CheckIfEntityHaveAutomation(tableName, "OnUpdate", entityPM.Tenant);
                    if (isHaveAutomation)
                    {
                        entityChangeHelper.AddEntityChange(entityPM, changeTrackingPM, "OnUpdate", entityChangeFieldXml, tableName, dateBefore);
                    }

                    #region Houses

                    if (entityPM.ShipmentLevelCode == "C" && notifyPropertyChangeValuesLists != null && notifyPropertyChangeValuesLists.Count > 0)
                    {
                        isHaveAutomation = entityChangeHelper.CheckIfEntityHaveAutomation("Shipment", "OnUpdate", entityPM.Tenant);
                        if (isHaveAutomation)
                        {
                            string fields = "MainCarriageCarrierId,MainCarriageETD,MainCarriageATD,MainCarriageETA,MainCarriageATA,FinalDistenationPortId";
                            List<NotifyPropertyChangeValues> changedProperties = notifyPropertyChangeValuesLists.Where(d => fields.Split(',').Contains(d.PropertyName)).ToList();
                            if (changedProperties.Count > 0)
                            {
                                entityChangeFieldXml = EntityPMChangeTrackingHelper.GetChangesDetectedXml(changedProperties);
                                ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                                List<ShipmentPM> housesList = shipmentQuery.GetShipmentPMsByMasterIdAndTenantForAutomation(entityPM.Id, tenant);
                                foreach (ShipmentPM oldHousePM in housesList)
                                {
                                    oldHousePM.StatusId = changeTrackingPM.StatusId;
                                    dateBefore = DateTime.Now;
                                    ShipmentPM shipmentPm = ShipmentMapping.MapShipmentPMToShipmentPMForAutomation(entityPM, oldHousePM);
                                    var changeHelper = new EntityChangeHelper();
                                    changeHelper.AddEntityChange(shipmentPm, oldHousePM, "OnUpdate", entityChangeFieldXml, "Shipment", dateBefore);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
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
            this.UpdateCustomerWorkingDates();            

            if (isNewEntity)
            {
                #region

                if (string.IsNullOrEmpty(entityPM.CreatedByUserId))
                {
                    entityPM.CreatedByUserId = loggedContact.Id;
                }

                entityPM.FWBStatusCode = "NSEN";
                entityPM.FHLStatusCode = "NSEN";
                entityPM.CargonautFHLStatusCode = "NSEN";
                entityPM.CargonautFWBStatusCode = "NSEN";
                entityPM.ShipmentReceivableStatusCode = "NORE";
                entityPM.ShipmentPayableStatusCode = "NOPA";

                entityPM.INTTRASIStatusCode = "NSEN";

                if (entityPM.ProfitExchangeRate == null)
                {
                    if (!string.IsNullOrEmpty(entityPM.ProfitCurrencyId))
                    {
                        if (entityPM.ProfitCurrencyId == loggedTenant.CurrencyId)
                        {
                            entityPM.ProfitExchangeRate = 1;
                        }

                        else
                        {
                            RatesTableQuery lastRateQuery = new RatesTableQuery(tenant);
                            LastRate lastRate = lastRateQuery.GetLastRecordByValueDate(tenant, entityPM.ProfitCurrencyId, loggedTenant.CurrencyId, entityPM.CreateDateTime);
                            if (lastRate != null)
                            {
                                entityPM.ProfitExchangeRate = lastRate.Rate;
                            }
                        }
                    }
                }

                //if (!string.IsNullOrEmpty(entityPM.CustomerId))
                //{
                //    this.UpdateCustomerWorkingDates();
                //}

                if (!entityPM.IsHybrid)
                {
                    Dictionary<string, string> counterAdditionalParameters = new Dictionary<string, string>() { { "[B]", "" } };
                    if (!string.IsNullOrEmpty(entityPM.BranchId))
                    {
                        Branch myBranch = (from d in myCommonContext.Branches
                                           where d.Tenant == tenant
                                           && d.Id == entityPM.BranchId
                                           select d).FirstOrDefault();

                        if (myBranch != null && !string.IsNullOrEmpty(myBranch.CounterCode))
                        {
                            counterAdditionalParameters["[B]"] = myBranch.CounterCode;
                        }
                    }

                    entityPM.CreateDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);

                    if (entityPM.ShipmentLevelCode == "C")
                    {
                        if (entityPM.ShipmentNumber == null)
                            entityPM.ShipmentNumber = TableCounter.GetNumber(tenant, "MAST", entityPM.DirectionId, entityPM.TransportModeId, counterAdditionalParameters);
                    }

                    else
                    {
                        if (entityPM.DirectionId.ToUpper() == "C")
                        {
                            if (entityPM.ShipmentNumber == null)
                                entityPM.ShipmentNumber = TableCounter.GetNumber(tenant, "SHIP", "I", entityPM.TransportModeId, counterAdditionalParameters);
                        }

                        else
                        {
                            if (entityPM.ShipmentNumber == null)
                                entityPM.ShipmentNumber = TableCounter.GetNumber(tenant, "SHIP", entityPM.DirectionId, entityPM.TransportModeId, counterAdditionalParameters);
                        }
                    }
                }

                if (entityPM.ShipmentLevelCode != "H")
                {
                    entityMasterData = new ShipmentMasterData();
                    entityMasterData.Id = entityPM.Id;
                    entityPM.MasterShipmentDataId = entityPM.Id;
                    entityMasterData.MasterShipmentNumber = entityPM.ShipmentNumber;
                    shipmentMasterDataRepository.Add(entityMasterData);
                }

                else
                {
                    if (entityPM.MasterShipmentDataId != null)
                    {
                        // this case is when create house from master sceen
                        // need to get the master, some fields need to be calculated from the master
                        // but we dont want to map the master it self

                        entityMasterData = shipmentMasterDataRepository.GetSingleMasterData(entityPM.MasterShipmentDataId);
                        if (entityMasterData != null)
                        {
                            if (entityMasterData.ProrateReceivables)
                            {
                                this.isUpdatingRegistryDate = true;
                            }
                        }
                    }
                }

                entityComputedFields = new ShipmentComputedFields();
                entityComputedFields.Id = entityPM.Id;
                entityComputedFields.Tenant = entityPM.Tenant;

                if (entityPM.IsOperationalClosed)
                {
                    entityComputedFields.IsMissingDocuments = false;
                    entityComputedFields.IsRequestedDocuments = false;
                }

                else
                {
                    entityComputedFields.IsMissingDocuments = true;
                }


                if (entityPM.CustomsClearanceDate != null)
                {
                    entityComputedFields.IsDigitalSignRequired = false;
                }

                if (entityPM.IsShipmentComputedFieldChange)
                {
                    entityComputedFields.IsDepositionRequired = entityPM.IsDepositionRequired;
                }

                entityComputedFields.LastDocumentDateTime = null;// new DateTime(1900, 1, 1);
                shipmentComputedFieldsRepository.Add(entityComputedFields);


                entityPM.IsDepositionRequired = entityComputedFields.IsDepositionRequired;
                entityPM.IsRequestedDocuments = entityComputedFields.IsRequestedDocuments;
                entityPM.IsDigitalSignRequired = entityComputedFields.IsDigitalSignRequired;

                shipmentAdditionalCloudData = new ShipmentAdditionalCloudData();
                shipmentAdditionalCloudData.Id = entityPM.Id;
                shipmentAdditionalCloudData.Tenant = entityPM.Tenant;
                shipmentAdditionalCloudData.IsImporterApprovalRequried = entityPM.IsImporterApprovalRequired;
                shipmentAdditionalCloudData.DeclarationXmlData = entityPM.DeclarationXMLData;
                shipmentAdditionalCloudData.DeclarationWCOXml = entityPM.DeclarationWCOXml;
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
                shipmentAdditionalCloudData.ShipmentAddtionalDataXML = entityPM.ShipmentAddtionalDataXML;

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

                this.InitializeSalesman();
                this.InitializeAccountManager();
                this.InitializeAgentAddress();
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
                #endregion
            }

            else
            {
                #region
                if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
                {
                    if (entityPM.IsHybrid && !string.IsNullOrEmpty(entityPM.QuoteId) && entityPM.QuoteId != entityPoco.QuoteId)
                    {
                        this.UpdateQuoteUsage();
                    }

                    if (entityPM.ConvertShipmentToLCL || entityPM.ConvertShipmentToFCL)
                    {
                        foreach (ShipmentPackagePM pm in entityPM.ShipmentPackages)
                        {
                            this.DeleteShipmentPackage(pm);
                        }

                        foreach (ShipmentOrderPackagePM pm in entityPM.ShipmentOrderPackages)
                        {
                            this.DeleteShipmentOrderPackage(pm);
                        }

                        entityPM.TEU = null;
                        entityPM.NumberOfPackages = null;
                        entityPM.NumberOfContainers = null;
                        entityPM.GrossWeight = null;
                        entityPM.ChargeableWeight = null;
                        entityPM.VolumetricWeight = null;
                        entityPM.Volume = null;

                        if (entityPM.ConvertShipmentToLCL)
                        {
                            entityPM.ShipmentTypeId = "LCLD";
                        }

                        else if(entityPM.ConvertShipmentToFCL)
                        {
                            entityPM.ShipmentTypeId = "FCLD";
                        }
                    }
                    
                    if (entityPM.ConvertFromDirectToHouse)
                    {
                        #region
                        entityPM.ShipmentLevelCode = "H";
                        entityPM.MasterShipmentDataId = null;
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

                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(tenant);
                entityComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(entityPM.Id, entityPM.Tenant);

                if (entityComputedFields != null)
                {

                    if (entityPM.IsShipmentComputedFieldChange)
                    {
                        entityComputedFields.IsDepositionRequired = entityPM.IsDepositionRequired;
                        entityComputedFields.IsRequestedDocuments = entityPM.IsRequestedDocuments;
                        entityComputedFields.IsDigitalSignRequired = entityPM.IsDigitalSignRequired;
                        entityComputedFields.IsMissingDocuments = entityPM.IsMissingDocuments;
                        entityComputedFields.DocumentsSearchFields = entityPM.DocumentsSearchFields;
                        entityComputedFields.MissingDocumentsCount = entityPM.MissingDocumentsCount;
                        entityComputedFields.MissingDocumentsNames = entityPM.MissingDocumentsNames;
                        entityComputedFields.RequestedDocumentsCount = entityPM.RequestedDocumentsCount;
                        entityComputedFields.NumberOfHouses = entityPM.NumberOfHouses;
                        entityComputedFields.ImporterDepositionRequestDetails = entityPM.ImporterDepositionRequestDetails;
                    }


                    if (entityPM.IsOperationalClosed)
                    {
                        entityComputedFields.IsMissingDocuments = false;
                        entityComputedFields.IsRequestedDocuments = false;
                        entityComputedFields.IsDigitalSignRequired = false;
                        entityComputedFields.MissingDocumentsCount = 0;
                        entityComputedFields.MissingDocumentsNames = "";
   
                    }

                    else
                    {
                        var OTId = objectTableRepository.GetObjectTableIdByName("Shipment");
                        entityComputedFields.MissingDocumentsCount = documentsFilingQuery.GetMissingDocCountForEntity(entityPM.Id, OTId, tenant, entityPM.IsOperationalClosed);
                        entityComputedFields.MissingDocumentsNames = documentsFilingQuery.GetMissingDocsNamesForEntity(entityPM.Id, OTId, tenant, entityPM.IsOperationalClosed);
                        if (entityComputedFields.MissingDocumentsCount == 0)
                        {
                            entityComputedFields.IsMissingDocuments = false;
                        }

                        else
                        {
                            entityComputedFields.IsMissingDocuments = true;
                        }
                    }
                    if (entityPM.CustomsClearanceDate != null)
                    {
                        entityComputedFields.IsMissingDocuments = false;
                        entityComputedFields.IsRequestedDocuments = false;
                        entityComputedFields.IsDigitalSignRequired = false;
                    }


                    entityPM.IsDepositionRequired = entityComputedFields.IsDepositionRequired;
                    entityPM.IsRequestedDocuments = entityComputedFields.IsRequestedDocuments;
                    entityPM.IsDigitalSignRequired = entityComputedFields.IsDigitalSignRequired;

                }


                if (entityPM.IsHybrid || loggedTenant.IsDocumentsArchive)
                {
                    shipmentAdditionalCloudData = shipmentAdditionalCloudDataRepository.GetSingleShipmentAdditionalCloudData(entityPM.Id, entityPM.Tenant);
                    shipmentAdditionalCloudData.IsImporterApprovalRequried = entityPM.IsImporterApprovalRequired;
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


                    if (entityPM.DeclarationXMLData != shipmentAdditionalCloudData.DeclarationXmlData && !string.IsNullOrEmpty(entityPM.DeclarationXMLData) && entityPM.CustomsClearanceDate == null)
                    {
                        shipmentAdditionalCloudData.DeclarationXmlData = entityPM.DeclarationXMLData;
                        shipmentAdditionalCloudData.IsImporterApprovalRequried = true;
                        shipmentAdditionalCloudData.ApprovedByUserName = null;
                        shipmentAdditionalCloudData.ApproveDateTime = null;
                        shipmentAdditionalCloudData.DenyReason = null;
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
                    if (!string.IsNullOrEmpty(entityPM.ShipmentAddtionalDataXML))
                    {
                        shipmentAdditionalCloudData.ShipmentAddtionalDataXML = entityPM.ShipmentAddtionalDataXML;
                    }

                    if (!string.IsNullOrEmpty(entityPM.PaymentRequestXML) && shipmentAdditionalCloudData.PaymentRequestXML != entityPM.PaymentRequestXML)
                    {
                        shipmentAdditionalCloudData.IsPaymentRequired = true;
                        shipmentAdditionalCloudData.PaymentRequestXML = entityPM.PaymentRequestXML;
                        shipmentAdditionalCloudData.PaymentDateTime = entityPM.PaymentDateTime;
                        AddPaymentReceivedToQueue();
                    }

                    shipmentAdditionalCloudDataRepository.Update(shipmentAdditionalCloudData);
                    //shipmentAdditionalCloudDataRepository.SubmitChanges();
                }
                #endregion
            }

            shipmentTracing = new ShipmentTracing(entityPM, entityPoco, entityMasterData, loggedContact.Id, isNewEntity);

            if (!entityPoco.IsCancelled || !entityPM.IsCancelled)
            {
                entityPM.UpdatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserName = loggedContact.EnglishName;
                entityPM.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                if (entityPM.IsUpdatedByChampAnalyzer)
                {
                    entityPM.UpdatedByPartner = "Airline Transmission";
                }

                else if (entityPM.IsUpdatedByGLSHKAnalyzer)
                {
                    entityPM.UpdatedByPartner = "Airline Transmission";
                }

                else if (entityPM.IsUpdatedByINTTRAAnalyzer)
                {
                    entityPM.UpdatedByPartner = "INTTRA";
                }

                else
                {
                    entityPM.UpdatedByPartner = loggedContact.EnglishName;
                }

                this.InitializeFCL_LCL();
                this.InitializeClosingFields();
                this.InitializeStatus();
                this.InitializePartners();
                this.InitializeInlandDomestic();
                this.InitializeAWBFields();
                this.InitializeMAWBStack();

                this.InitializeNumberOfInsidePackages();
                this.InitializeAccountManager();
                this.InitializeCarrierPrefix();
                this.InitializeKnownConsignor();
                this.InitializePrintingFields();
                this.ComputeTEU();
                this.UpdateQuoteUsage();
                //this.InitializeBookingData();
            }

          
        }

        private void AddPaymentReceivedToQueue()
        {
            if (LogitudeSettings.EnableHybridQueue)
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
            var isFCL = false;
            string myShipmentTypeId = this.entityPM.ShipmentTypeId;
            string myTransportModeId = this.entityPM.TransportModeId;

            if (myTransportModeId != null)
            {
                myTransportModeId = myTransportModeId.ToUpper();
            }

            if (myShipmentTypeId != null)
            {
                myShipmentTypeId = myShipmentTypeId.ToUpper();
            }

            if (myTransportModeId == "O" && (myShipmentTypeId == "FCLD" || myShipmentTypeId == "MYGO"))
            {
                isFCL = true;
            }

            if (myTransportModeId == "I" && (myShipmentTypeId == "FTL" || myShipmentTypeId == "MYGI"))
            {
                isFCL = true;
            }

            this.IsFCLEntity = isFCL;
            this.IsLCLEntity = !isFCL;

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
        private void InitializePartners()
        {
            if (string.IsNullOrEmpty(entityPM.CustomerId))
            {
                entityPM.CustomerName = null;
                entityPM.CustomerNote = null;
                entityPM.CustomerContactId = null;
                entityPM.CustomerAddressId = null;
                entityPM.CustomerReference1 = null;
                entityPM.CustomerReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.ShipperId))
            {
                //entityPM.ShipperName = null;
                entityPM.ShipperNote = null;
                entityPM.ShipperContactId = null;
                entityPM.ShipperAddressId = null;
                entityPM.ShipperMainAddressId = null;
                entityPM.ShipperAddressText = null;
                entityPM.ShipperReference1 = null;
                entityPM.ShipperReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                //entityPM.ConsigneeName = null;
                entityPM.ConsigneeNote = null;
                entityPM.ConsigneeContactId = null;
                entityPM.ConsigneeAddressId = null;
                entityPM.ConsigneeMainAddressId = null;
                entityPM.ConsigneeAddressText = null;
                entityPM.ConsigneeReference1 = null;
                entityPM.ConsigneeReference2 = null;
            }

            if (string.IsNullOrEmpty(entityPM.AgentId))
            {
                entityPM.AgentName = null;
                entityPM.AgentNote = null;
                entityPM.AgentContactId = null;
                entityPM.AgentAddressId = null;
                entityPM.AgentAddressText = null;
                entityPM.AgentReference1 = null;
                entityPM.AgentReference2 = null;
            }

            if (entityPM.ShipmentLevelCode == "C")
            {
                entityPM.CustomerId = null;
                entityPM.CustomerName = null;
                entityPM.CustomerReference1 = null;
                entityPM.CustomerReference2 = null;
                entityPM.CustomerAddressId = null;
                entityPM.CustomerContactId = null;
                entityPM.ShipmentCustomerTypeCode = null;
            }

            else
            {
                if (string.IsNullOrEmpty(entityPM.ShipmentCustomerTypeCode))
                {
                    if (entityPM.DirectionId == "I")
                    {
                        entityPM.ShipmentCustomerTypeCode = "CON";
                    }

                    else
                    {
                        entityPM.ShipmentCustomerTypeCode = "SHI";
                    }
                }

                if (entityPM.ShipmentCustomerTypeCode == "SHI")
                {
                    if (!entityPM.IsImporterShipment)
                    {
                        if (string.IsNullOrEmpty(entityPM.CustomerId))
                        {
                            entityPM.CustomerId = entityPM.ShipperId;
                        }

                        entityPM.CustomerName = entityPM.ShipperName;
                        entityPM.CustomerNote = entityPM.ShipperNote;
                        entityPM.CustomerContactId = entityPM.ShipperContactId;
                        entityPM.CustomerAddressId = entityPM.ShipperAddressId;
                        entityPM.CustomerReference1 = entityPM.ShipperReference1;
                        entityPM.CustomerReference2 = entityPM.ShipperReference2;
                    }

                }

                else if (entityPM.ShipmentCustomerTypeCode == "CON")
                {
                    if (string.IsNullOrEmpty(entityPM.CustomerId))
                    {
                        entityPM.CustomerId = entityPM.ConsigneeId;
                    }

                    entityPM.CustomerName = entityPM.ConsigneeName;
                    entityPM.CustomerNote = entityPM.ConsigneeNote;
                    entityPM.CustomerContactId = entityPM.ConsigneeContactId;
                    entityPM.CustomerAddressId = entityPM.ConsigneeAddressId;
                    entityPM.CustomerReference1 = entityPM.ConsigneeReference1;
                    entityPM.CustomerReference2 = entityPM.ConsigneeReference2;
                }

                else
                {
                    if (!string.IsNullOrEmpty(entityPM.CustomerId))
                    {
                        if (entityPM.CustomerId == entityPM.AgentId)
                        {
                            entityPM.CustomerName = entityPM.AgentName;
                            entityPM.CustomerNote = entityPM.AgentNote;
                            entityPM.CustomerContactId = entityPM.AgentContactId;
                            entityPM.CustomerAddressId = entityPM.AgentAddressId;
                            entityPM.CustomerReference1 = entityPM.AgentReference1;
                            entityPM.CustomerReference2 = entityPM.AgentReference2;
                        }

                        else if (entityPM.CustomerId == entityPM.IssuingCarrierAgentId)
                        {
                            entityPM.CustomerName = entityPM.IssuingCarrierAgentName;
                            entityPM.CustomerNote = entityPM.IssuingCarrierAgentNote;
                            entityPM.CustomerContactId = null;
                            entityPM.CustomerAddressId = entityPM.IssuingCarrierAddressId;
                            entityPM.CustomerReference1 = null;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.CustomAgentExportId)
                        {
                            entityPM.CustomerName = entityPM.CustomAgentExportName;
                            entityPM.CustomerNote = entityPM.CustomAgentExportNote;
                            entityPM.CustomerContactId = entityPM.CustomAgentExportContactId;
                            entityPM.CustomerAddressId = entityPM.CustomAgentExportAddressId;
                            entityPM.CustomerReference1 = entityPM.CustomAgentExportReference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.CustomAgentImportId)
                        {
                            entityPM.CustomerName = entityPM.CustomAgentImportName;
                            entityPM.CustomerNote = entityPM.CustomAgentImportNote;
                            entityPM.CustomerContactId = entityPM.CustomAgentImportContactId;
                            entityPM.CustomerAddressId = entityPM.CustomAgentImportAddressId;
                            entityPM.CustomerReference1 = entityPM.CustomAgentImportReference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.Notify1Id)
                        {
                            entityPM.CustomerName = entityPM.Notify1Name;
                            entityPM.CustomerNote = entityPM.Notify1Note;
                            entityPM.CustomerContactId = entityPM.Notify1ContactId;
                            entityPM.CustomerAddressId = entityPM.Notify1AddressId;
                            entityPM.CustomerReference1 = entityPM.Notify1Reference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.Notify2Id)
                        {
                            entityPM.CustomerName = entityPM.Notify2Name;
                            entityPM.CustomerNote = entityPM.Notify2Note;
                            entityPM.CustomerContactId = entityPM.Notify2ContactId;
                            entityPM.CustomerAddressId = entityPM.Notify2AddressId;
                            entityPM.CustomerReference1 = entityPM.Notify2Reference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.ShipperNotExporterId)
                        {
                            entityPM.CustomerName = entityPM.ShipperNotExporterName;
                            entityPM.CustomerNote = entityPM.ShipperNotExporterNote;
                            entityPM.CustomerContactId = entityPM.ShipperNotExporterContactId;
                            entityPM.CustomerAddressId = entityPM.ShipperNotExporterAddressId;
                            entityPM.CustomerReference1 = entityPM.ShipperNotExporterReference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.ConsigneeNotImporterId)
                        {
                            entityPM.CustomerName = entityPM.ConsigneeNotImporterName;
                            entityPM.CustomerNote = entityPM.ConsigneeNotImporterNote;
                            entityPM.CustomerContactId = entityPM.ConsigneeNotImporterContactId;
                            entityPM.CustomerAddressId = entityPM.ConsigneeNotImporterAddressId;
                            entityPM.CustomerReference1 = entityPM.ConsigneeNotImporterReference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.FreightForwarderId)
                        {
                            entityPM.CustomerName = entityPM.FreightForwarderName;
                            entityPM.CustomerNote = entityPM.FreightForwarderNote;
                            entityPM.CustomerContactId = entityPM.FreightForwarderContactId;
                            entityPM.CustomerAddressId = entityPM.FreightForwarderAddressId;
                            entityPM.CustomerReference1 = entityPM.FreightForwarderReference;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.ColoaderId)
                        {
                            entityPM.CustomerName = entityPM.ColoaderName;
                            entityPM.CustomerNote = entityPM.ColoaderNote;
                            entityPM.CustomerContactId = entityPM.ColoaderContactId;
                            entityPM.CustomerAddressId = entityPM.ColoaderAddressId;
                            entityPM.CustomerReference1 = entityPM.ColoaderReference1;
                            entityPM.CustomerReference2 = null;
                        }

                        else if (entityPM.CustomerId == entityPM.CustomClearancePointId)
                        {
                            entityPM.CustomerName = entityPM.CustomClearancePointName;
                            entityPM.CustomerNote = entityPM.CustomClearancePointNote;
                            entityPM.CustomerContactId = entityPM.CustomClearancePointContactId;
                            entityPM.CustomerAddressId = entityPM.CustomClearancePointAddressId;
                            entityPM.CustomerReference1 = entityPM.CustomClearancePointReference1;
                            entityPM.CustomerReference2 = null;
                        }
                    }
                }
            }
        }
        private void InitializeAgentAddress()
        {
            if (!string.IsNullOrEmpty(entityPM.AgentId))
            {
                if (string.IsNullOrEmpty(entityPM.AgentName))
                {
                    Card agent = CardRepository.GetSingleCard(entityPM.AgentId, entityPM.Tenant, true);
                    if (agent != null)
                    {
                        entityPM.AgentName = agent.EnglishName;
                        entityPM.AgentNote = agent.Notes;
                    }
                }

                if (string.IsNullOrEmpty(entityPM.AgentAddressId))
                {
                    Address address = myAddressRepository.GetSingleAddressByCardIdAndTypeId(entityPM.AgentId, "M", entityPM.Tenant);
                    if (address != null)
                    {
                        entityPM.AgentAddressId = address.Id;
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
                    if (string.IsNullOrEmpty(entityPM.MainCarriageFromPartnerId))
                    {
                        entityPM.MainCarriageFromPartnerId = entityPM.ShipperId;
                    }

                    if (string.IsNullOrEmpty(entityPM.MainCarriageFromAddressId))
                    {
                        entityPM.MainCarriageFromAddressId = entityPM.ShipperAddressId;
                    }

                    if (string.IsNullOrEmpty(entityPM.MainCarriageToPartnerId))
                    {
                        entityPM.MainCarriageToPartnerId = entityPM.ConsigneeId;
                    }

                    if (string.IsNullOrEmpty(entityPM.MainCarriageToAddressId))
                    {
                        entityPM.MainCarriageToAddressId = entityPM.ConsigneeAddressId;
                    }
                }

                entityPM.FromPortId = null;
                entityPM.ToPortId = null;
                entityPM.MainCarriageFromPortId = null;
                entityPM.MainCarriageToPortId = null;
                entityPM.MainCarriageFinalDestinationPortId = null;
                entityPM.ShipmentLevelCode = "D";
            }
        }
        private void InitializeSalesman()
        {
            if (string.IsNullOrEmpty(entityPM.SalesmanUserId))
            {
                CustomerRepository customerRepository = new CustomerRepository(tenant);
                Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, tenant, true);

                if (customer != null)
                {
                    entityPM.SalesmanUserId = customer.SalesmanUserId;
                }

                if (string.IsNullOrEmpty(entityPM.SalesmanUserId))
                {
                    entityPM.SalesmanUserId = entityPM.CreatedByUserId;
                }
            }
        }
        private void InitializeAccountManager()
        {
            if (!entityPM.IsHybrid)
            {
                CustomerRepository customerRepository = new CustomerRepository(tenant);
                Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, tenant, true);

                if (customer != null && string.IsNullOrEmpty(entityPM.AccountManagerUserId))
                {
                    entityPM.AccountManagerUserId = customer.AccountManagerUserId;
                }
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
                        ShipmentCommodityPM myShipmentCommodity = entityPM.ShipmentCommodities.OrderBy(d => d.Id).FirstOrDefault();

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
                    if (shipmentPackagesChangeSet != null)
                    {
                        foreach (ShipmentPackagePM item in shipmentPackagesChangeSet)
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
                    if (shipmentPackagesChangeSet != null)
                    {
                        List<ShipmentPackagePM> allPackages = shipmentPackagesChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
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

                            if (this.entityPM.FirstAccountingCloseDate == null)
                            {
                                this.entityPM.FirstAccountingCloseDate = this.entityPM.AccountingCloseDate;
                            }
                        }

                        else
                        {
                            entityPM.AccountingCloseDate = null;
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

        private bool isUpdatingHouses = false;
        List<Shipment> allHouses = new List<Shipment>();
        private void CheckUpdatingMasterHouses()
        {
            if (this.isNewEntity)
            {
                if (entityPM.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(entityPM.MasterShipmentDataId))
                {
                    ShipmentComputedFields entityMasterComputedFields = shipmentComputedFieldsRepository.GetSingleShipmentComputedFields(entityPM.MasterShipmentDataId, entityPM.Tenant);
                    if (entityMasterComputedFields != null)
                    {
                        entityMasterComputedFields.NumberOfHouses += 1;
                        shipmentComputedFieldsRepository.Update(entityMasterComputedFields);
                    }
                }
            }

            else
            {
                if (entityPM.ShipmentLevelCode == "C")
                {
                    this.allHouses = entityRepository.GetHouseShipmentsForMaster(entityPM.Id, tenant);

                    if (entityPM.IsCancelled != entityPoco.IsCancelled)
                    {
                        isUpdatingHouses = true;
                    }
                    else if (entityPM.IsAccountingClosed != entityPoco.IsAccountingClosed)
                    {
                        isUpdatingHouses = true;
                    }
                    else if (entityPM.IsOperationalClosed != entityPoco.IsOperationalClosed)
                    {
                        isUpdatingHouses = true;
                    }
                    else if (entityPM.MainCarriageFromPortId != entityMasterData.MainCarriageFromPortId)
                    {
                        isUpdatingHouses = true;
                    }
                    else if (entityPM.MainCarriageFinalDestinationPortId != entityMasterData.MainCarriageFinalDestinationPortId)
                    {
                        isUpdatingHouses = true;
                    }

                    if (entityComputedFields != null && shipmentConsoleShipmentsChangeSet != null)
                    {
                        entityComputedFields.NumberOfHouses = shipmentConsoleShipmentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Count();
                    }
                }
            }
        }
        private void ApplyUpdatingMasterHouses()
        {
            if (!this.isNewEntity)
            {
                if (entityPM.ShipmentLevelCode == "C")
                {
                    if (allHouses.Count > 0)
                    {
                        foreach (Shipment item in allHouses)
                        {
                            item.NextETA = this.entityPoco.NextETA;
                            item.NextETD = this.entityPoco.NextETD;
                            item.NextLeg = this.entityPoco.NextLeg;
                            item.NextLegCode = this.entityPoco.NextLegCode;
                            if(string.IsNullOrEmpty(item.AgentId))
                            {
                                item.AgentComputed = entityPM.AgentId;
                            }
                            entityRepository.Update(item);
                        }

                        entityRepository.SubmitChanges();
                    }

                    if (isUpdatingHouses)
                    {
                        if (shipmentConsoleShipmentsChangeSet != null)
                        {
                            List<string> ids = shipmentConsoleShipmentsChangeSet.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).Select(s => s.Id).ToList();

                            if (ids.Count > 0)
                            {
                                ShipmentQuery iShipmentQuery = new ShipmentQuery(this.entityRepository);

                                foreach (string id in ids)
                                {
                                    ShipmentPM iHousePM = iShipmentQuery.GetSinglePMWithoutComposition(id, this.tenant);

                                    if (iHousePM != null)
                                    {
                                        iHousePM.IsCancelled = this.entityPM.IsCancelled;
                                        iHousePM.CancelledDate = this.entityPM.CancelledDate;
                                        iHousePM.IsOperationalClosed = this.entityPM.IsOperationalClosed;
                                        iHousePM.OperationalClosedByUserId = this.entityPM.OperationalClosedByUserId;
                                        iHousePM.OperationalCloseDate = this.entityPM.OperationalCloseDate;
                                        iHousePM.FirstOperationalCloseDate = this.entityPM.FirstOperationalCloseDate;
                                        iHousePM.IsAccountingClosed = this.entityPM.IsAccountingClosed;
                                        iHousePM.AccountingCloseDate = this.entityPM.AccountingCloseDate;
                                        iHousePM.FirstAccountingCloseDate = this.entityPM.FirstAccountingCloseDate;

                                        if (iHousePM.FromPortId != this.entityPM.MainCarriageFromPortId)
                                        {
                                            iHousePM.FromPortId = this.entityPM.MainCarriageFromPortId;

                                            if (iHousePM.PreCarriageFromPortId != null && iHousePM.PreCarriageToPortId != null)
                                            {
                                                iHousePM.PreCarriageToPortId = iHousePM.FromPortId;
                                            }
                                        }

                                        if (iHousePM.ToPortId != this.entityPM.MainCarriageFinalDestinationPortId)
                                        {
                                            iHousePM.ToPortId = this.entityPM.MainCarriageFinalDestinationPortId;

                                            if (iHousePM.OnCarriageFromPortId != null && iHousePM.OnCarriageToPortId != null)
                                            {
                                                iHousePM.OnCarriageFromPortId = iHousePM.ToPortId;
                                            }
                                        }

                                        ShipmentService iShipmentService = new ShipmentService(this.objectContext, iHousePM, this.serviceContextUser);
                                        iShipmentService.Update();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ShipmentOrderPackage", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            ShipmentOrderPackage itemPoco = new ShipmentOrderPackage()
            {
                Id = itemPM.Id,
            };

            ShipmentMapping.MapOrderPackage(itemPM, itemPoco, true);
            shipmentOrderPackageRepository.Add(itemPoco);
        }
        private void UpdateShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            ShipmentOrderPackage itemPoco = shipmentOrderPackageRepository.GetSingleShipmentOrderPackage(itemPM.Id);

            if (itemPoco != null)
            {
                ShipmentMapping.MapOrderPackage(itemPM, itemPoco, false);
                shipmentOrderPackageRepository.Update(itemPoco);
            }
        }
        private void DeleteShipmentOrderPackage(ShipmentOrderPackagePM itemPM)
        {
            ShipmentOrderPackage itemPoco = shipmentOrderPackageRepository.GetSingleShipmentOrderPackage(itemPM.Id);

            if (itemPoco != null)
            {
                shipmentOrderPackageRepository.Remove(itemPoco);
            }
        }

        private void CreateShipmentPackage(ShipmentPackagePM itemPM)
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

            this.UpdateShipmentDeliveryFromPackage(itemPM, itemPoco);

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
        }
        private void UpdateShipmentPackage(ShipmentPackagePM itemPM)
        {
            ShipmentPackage itemPoco = shipmentPackageRepository.GetSingleShipmentPackage(itemPM.Id, tenant);

            this.UpdateShipmentDeliveryFromPackage(itemPM, itemPoco);

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
                foreach (ShipmentPackageHarmonizePM itemHarmonizePM in itemPM.ShipmentPackageHarmonizesChangeSet)
                {
                    switch (itemHarmonizePM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateShipmentPackageHarmonize(itemHarmonizePM, itemPM.Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateShipmentPackageHarmonize(itemHarmonizePM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteShipmentPackageHarmonize(itemHarmonizePM);
                                break;
                            }

                        default: { break; }
                    }
                }
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
        }
        private void DeleteInsideShipmentPackage(InsideShipmentPackagePM insideItemPM)
        {
            InsideShipmentPackage insideItemPoco = insideShipmentPackageRepository.GetSingleInsideShipmentPackage(insideItemPM.Id, tenant);

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

        private void CreateShipmentPickUp(ShipmentPickUpPM itemPM)
        {
            AddressValidating.ValidatePickUp(itemPM);

            itemPM.Id = IdCounter.GetNumber("ShipmentPickUpDelivery", tenant).ToString();
            itemPM.ShipmentId = entityPM.Id;
            itemPM.Tenant = tenant;

            entityPM.ShipmentPickUpIndex += 1;
            itemPM.PickUpDeliveryNumber = entityPM.ShipmentNumber + "/" + entityPM.ShipmentPickUpIndex;

            ShipmentPickUpDelivery itemPoco = new ShipmentPickUpDelivery()
            {
                Id = itemPM.Id,
            };

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TracePickUp(itemPM, itemPoco);
            }

            ShipmentMapping.MapPickUp(itemPM, itemPoco, true);
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
                shipmentTracing.TracePickUp(itemPM, itemPoco);
            }

            ShipmentMapping.MapPickUp(itemPM, itemPoco, true);
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
                shipmentTracing.TraceDeletedPickUp(itemPM, itemPoco);
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
            itemPM.Tenant = tenant;

            if (itemPM.PickUpDeliveryTypeCode == "EMPT")
            {
                entityPM.ShipmentContainerReturnIndex += 1;
                itemPM.PickUpDeliveryNumber = entityPM.ShipmentNumber + "/" + entityPM.ShipmentContainerReturnIndex;
            }

            else
            {
                entityPM.ShipmentDeliveryIndex += 1;
                itemPM.PickUpDeliveryNumber = entityPM.ShipmentNumber + "/" + entityPM.ShipmentDeliveryIndex;
            }

            this.UpdateShipmentPackageFromDelivery(itemPM);

            ShipmentPickUpDelivery itemPoco = new ShipmentPickUpDelivery()
            {
                Id = itemPM.Id,
            };

            if (!entityPM.IsHybrid)
            {
                shipmentTracing.TraceDelivery(itemPM, itemPoco);
            }

            ShipmentMapping.MapDelivery(itemPM, itemPoco, true);
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
                shipmentTracing.TraceDelivery(itemPM, itemPoco);
            }

            this.UpdateShipmentPackageFromDelivery(itemPM);

            ShipmentMapping.MapDelivery(itemPM, itemPoco, false);
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
                    shipmentTracing.TraceDeletedDelivery(itemPM, itemPoco);
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
                            ShipmentMapping.MapDelivery(myDelivery, entityPOCO, false);
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
                                ShipmentMapping.MapDelivery(myDelivery, entityPOCO, false);
                                shipmentPickUpDeliveryRepository.Update(entityPOCO);
                            }
                        }
                    }
                }
            }
        }

        private void CreateShipmentPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM insideItemPM, string pickUpDeliveryId)
        {
            insideItemPM.Id = IdCounter.GetNumber("ShipmentPickUpDeliveryPackage", tenant).ToString();
            insideItemPM.ShipmentPickUpDeliveryId = pickUpDeliveryId;
            insideItemPM.Tenant = tenant;

            ShipmentPickUpDeliveryPackage insideItemPoco = new ShipmentPickUpDeliveryPackage()
            {
                Id = insideItemPM.Id,
            };

            ShipmentMapping.MapPickUpDeliveryPackage(insideItemPM, insideItemPoco, true);
            shipmentPickUpDeliveryPackageRepository.Add(insideItemPoco);

            if (insideItemPM.PickUpDeliveryPackageHarmonizes != null)
            {
                foreach (PickUpDeliveryPackageHarmonizePM itemHarmonizePM in insideItemPM.PickUpDeliveryPackageHarmonizes)
                {
                    this.CreatePickUpDeliveryPackageHarmonize(itemHarmonizePM, insideItemPM.Id);
                }
            }
        }
        private void UpdateShipmentPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM insideItemPM)
        {
            ShipmentPickUpDeliveryPackage insideItemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(insideItemPM.Id);
            ShipmentMapping.MapPickUpDeliveryPackage(insideItemPM, insideItemPoco, false);
            shipmentPickUpDeliveryPackageRepository.Update(insideItemPoco);

            if (insideItemPM.PickUpDeliveryPackageHarmonizesChangeSet != null)
            {
                foreach (PickUpDeliveryPackageHarmonizePM itemHarmonizePM in insideItemPM.PickUpDeliveryPackageHarmonizesChangeSet)
                {
                    switch (itemHarmonizePM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreatePickUpDeliveryPackageHarmonize(itemHarmonizePM, insideItemPM.Id);
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
        }
        private void DeleteShipmentPickUpDeliveryPackage(ShipmentPickUpDeliveryPackagePM insideItemPM)
        {
            ShipmentPickUpDeliveryPackage insideItemPoco = shipmentPickUpDeliveryPackageRepository.GetSingleShipmentPickUpDeliveryPackage(insideItemPM.Id);

            PickUpDeliveryPackageHarmonizeQuery pickUpDeliveryPackageHarmonizeQuery = new PickUpDeliveryPackageHarmonizeQuery(pickUpDeliveryPackageHarmonizeRepository);
            List<PickUpDeliveryPackageHarmonizePM> PickUpDeliveryPackageHarmonize = pickUpDeliveryPackageHarmonizeQuery.GetPickUpDeliveryPackageHarmonizes(insideItemPM.Id, tenant);
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
            if (string.IsNullOrEmpty(itemPM.Id))
            {
                itemPM.Id = IdCounter.GetNumber("ShipmentReceivable", tenant).ToString();
            }

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

                        if (itemPM.Quantity == null || itemPM.UnitPrice == null)
                        {
                            myStatusCode = "EMPT";
                        }

                        else
                        {
                            myStatusCode = "OAMT";
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

        private List<string> deletedHousesIds = new List<string>();
        private void CreateConsoleShipment(ConsoleShipmentPM itemPM)
        {
            Shipment houseShipment = entityRepository.GetSingleShipment(itemPM.Id, tenant);

            if (houseShipment != null)
            {
                houseShipment.MasterShipmentDataId = itemPM.MasterShipmentDataId;
                entityRepository.Update(houseShipment);
                entityRepository.SubmitChanges();

                this.RunRegistryDateProcedure(houseShipment.Id);

                calculateProfit = true;
                calculatePayables = true;
                calculateReceivables = true;
            }
        }
        private void DeleteConsoleShipment(ConsoleShipmentPM itemPM)
        {
            Shipment houseShipment = entityRepository.GetSingleShipment(itemPM.Id, tenant);

            if (houseShipment != null)
            {
                houseShipment.MasterShipmentDataId = null;
                houseShipment.OperationalDate = houseShipment.CreateDateTime;
                entityRepository.Update(houseShipment);
                entityRepository.SubmitChanges();

                //this.RunRegistryDateProcedure(houseShipment.Id);

                calculateProfit = true;
                calculatePayables = true;
                calculateReceivables = true;

                //RunStoredProcedureClass.UpdateShipmentFinalArrivalDate(houseShipment.Id, entityPM.Tenant);

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

                this.deletedHousesIds.Add(houseShipment.Id);
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
                    foreach (ShipmentPackage package in list)
                    {
                        shipmentPackageRepository.Remove(package);
                    }
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

        private void UpdateQuoteUsage()
        {
            bool isUpdatingUsage = false;

            if (isNewEntity)
            {
                if (!string.IsNullOrEmpty(entityPM.QuoteId))
                {
                    isUpdatingUsage = true;
                }
            }
            else if (!string.IsNullOrEmpty(entityPM.QuoteId) && string.IsNullOrEmpty(entityPoco.QuoteId))
            {
                isUpdatingUsage = true;
            }

            if (isUpdatingUsage)
            {
                QuoteRepository quoteRepository = new QuoteRepository(tenant);
                Quote quote = quoteRepository.GetSingleQuote(entityPM.QuoteId, tenant);

                if (quote != null)
                {
                    if (quote.UsageCount == null)
                    {
                        quote.UsageCount = 1;
                    }

                    else
                    {
                        quote.UsageCount += 1;
                    }

                    quote.LastUsageDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                    quoteRepository.Update(quote);
                    quoteRepository.SubmitChanges();
                }
            }
        }
        private void UpdateCustomerWorkingDates()
        {
            if (isNewEntity)
            {
                if (!string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
                    Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);

                    CustomerRepository customerRepository = new CustomerRepository(tenant);
                    Customer customer = customerRepository.GetSingleCustomerWithCardOnly(entityPM.CustomerId, tenant, false); // islam: no includes even for the card!

                    if (customer != null)
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                        if (customer.StartWorkingDate == null && !entityPM.IsHybrid && !currentTenant.IsHybrid)
                        {
                            customer.StartWorkingDate = todayDate;
                        }

                        if (customer.FirstShipmentDate == null)
                        {
                            customer.FirstShipmentDate = currentTenant.IsHybrid ? entityPM.CreateDateTime : todayDate;

                            CustomerQuery customerQuery = new CustomerQuery(customerRepository);
                            CustomerPM customerpm = new CustomerPM()
                            {
                                Id = customer.Id,
                                Code = customer.Card.Code,
                                EnglishName = customer.Card.EnglishName,
                                VatNumber = customer.Card.VatNumber,
                                SalesmanUserId = customer.Card.SalesmanUserId,
                                CreatedByUserId = customer.Card.CreatedByUserId,
                                RankId = customer.RankId,
                                AccountManagerUserId = customer.AccountManagerUserId,
                                RegionId = customer.RegionId,

                                CustomerSizeId = customer.CustomerSizeId,
                                LastCallDate = customer.LastCallDate,
                                LastMeetingDate = customer.LastMeetingDate,
                                LastOpportunityDate = customer.LastOpportunityDate,
                                FirstInvoiceDate = customer.FirstInvoiceDate,
                                FirstShipmentDate = customer.FirstShipmentDate,
                                LastShipmentDate = customer.LastShipmentDate,
                                StartWorkingDate = customer.StartWorkingDate,
                                StartWorkingManuallySet = customer.StartWorkingManuallySet,
                                LastQuoteDate = customer.LastQuoteDate,
                                LastInteractionDate = customer.LastInteractionDate,
                            };
                            //CustomerMapping.GetMappedPMFromPoco(customer);//customerQuery.GetSinglePM(customer.Id, entityPM.Tenant);
                            if ((DateTime.Today.Date - customer.FirstShipmentDate.Value.Date).Days <= 10)
                            {
                                CustomerEmailAlert customerEmailAlert = new CustomerEmailAlert();
                                customerEmailAlert.SendEmailAlert(customerpm, entityPM.Tenant, "GCFS", false);
                            }
                        }

                        if (currentTenant.IsHybrid)
                        {
                            if (entityPM.CreateDateTime > customer.LastShipmentDate)
                            {
                                customer.LastShipmentDate = entityPM.CreateDateTime;
                            }
                        }
                        else
                        {
                            customer.LastShipmentDate = todayDate;
                        }

                        customerRepository.Update(customer);
                        customerRepository.SubmitChanges();
                    }
                }
            }

            else
            {
                if (!entityPM.IsHybrid)
                {
                    if (this.entityPM.CustomerId != this.entityPoco.CustomerId)
                    {
                        CustomerRepository customerRepository = new CustomerRepository(tenant);

                        if (!string.IsNullOrEmpty(this.entityPM.CustomerId))
                        {
                            Customer customer = customerRepository.GetSingleCustomerWithCardOnly(entityPM.CustomerId, tenant, false);
                            if (customer != null)
                            {
                                customer.LastShipmentDate = this.entityPM.CreateDateTime;
                                customerRepository.Update(customer);
                                customerRepository.SubmitChanges();
                            }
                        }

                        if (!string.IsNullOrEmpty(this.entityPoco.CustomerId))
                        {
                            Customer customer = customerRepository.GetSingleCustomerWithCardOnly(this.entityPoco.CustomerId, tenant, false);
                            if (customer != null)
                            {
                                Shipment shipment = this.objectContext.Shipments.Where(d => d.CustomerId == customer.Id && d.Id != this.entityPoco.Id).OrderByDescending(s => s.CreateDateTime).FirstOrDefault();
                                if (shipment != null)
                                {
                                    customer.LastShipmentDate = shipment.CreateDateTime;
                                    customerRepository.Update(customer);
                                    customerRepository.SubmitChanges();
                                }
                            }
                        }
                    }
                }
            }
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
            }

            else
            {
                this.entityPM.FirstPickupETA = null;
                this.entityPM.FirstPickupETD = null;
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
            else if (entityPM.DirectionId == "E" )
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
}

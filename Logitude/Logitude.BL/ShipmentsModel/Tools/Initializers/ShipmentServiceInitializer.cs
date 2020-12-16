using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Initializers
{
    public class ShipmentServiceInitializer: IServiceInitializer
    {
        public int Tenant { get; private set; }
        public bool IsNewEntity { get; private set; }
        public Shipment EntityPOCO { get; private set; }
        public ShipmentPM EntityPM { get; private set; }
        public ShipmentMasterData EntityMasterData { get; private set; }
        public IShipmentsContext ShipmentContext { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        public ShipmentRepository Repository { get; private set; }
        public ShipmentMasterDataRepository MasterDataRepository { get; private set; }
        public CardRepository CardRepository { get; private set; }
        public AddressRepository AddressRepository { get; private set; }
        public ContactRepository ContactRepository { get; private set; }

        public Tenant LoggedTenant { get; private set; }
        public ContactPM LoggedContact { get; private set; }
        public string LoggedContactId { get; private set; }
        public string LoggedContactEmail { get; private set; }
        public string LoggedContactName { get; private set; }

        public DateTime TodayDate { get; private set; }
        public DateTime TodayDateTime { get; private set; }
        public bool IsLCLEntity { get; private set; }
        public bool IsFCLEntity { get; private set; }
        public bool IsProratingChanged { get; private set; }
        public bool IsUpdatingRegistryDate { get; private set; }
        public bool IsUpdatingFirstApprovalDate { get; private set; }
        public bool IsMappingComposition { get; internal set; }

        public List<ShipmentPackagePM> ShipmentPackagesChangeSet;
        public List<ShipmentOrderPackagePM> ShipmentOrderPackagesChangeSet;
        public List<ShipmentPickUpPM> ShipmentPickUpsChangeSet;
        public List<ShipmentDeliveryPM> ShipmentDeliveriesChangeSet;
        public List<ShipmentReceivablePM> ShipmentReceivablesChangeSet;
        public List<ShipmentPayablePM> ShipmentPayablesChangeSet;
        public List<ShipmentFollowUpPM> ShipmentFollowUpsChangeSet;
        public List<ShipmentAWBPrintOnlyPM> ShipmentAWBPrintOnliesChangeSet;
        public List<ConsoleShipmentPM> ShipmentConsoleShipmentsChangeSet;
        public List<ShipmentCarrierStatusPM> ShipmentCarrierStatusesChangeSet;
        public List<AWBOCIPM> AWBOCIPMChangeSet;
        public List<ShipmentCommodityPM> ShipmentCommoditiesChangeSet;
        public List<ShipmentAssemblyPM> ShipmentAssembliesChangeSet;
        public List<ShipmentStoragePricingPM> ShipmentStoragePricingsChangeSet;

        public Customer Customer { get; private set; }

        public ShipmentServiceInitializer(IShipmentsContext ShipmentContext, ShipmentPM entityPM, string loggedEmail)
        {
            if (string.IsNullOrEmpty(loggedEmail))
            {
                loggedEmail = AuthenticationUtil.GetAuthenticatedUser();
            }

            this.EntityPM = entityPM;
            this.Tenant = entityPM.Tenant;
            this.IsNewEntity = entityPM.Id == null ? true : false;
            this.ShipmentContext = ShipmentContext;
            this.LoggedContactEmail = loggedEmail;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.Repository = new ShipmentRepository(ShipmentContext);
            this.MasterDataRepository = new ShipmentMasterDataRepository(ShipmentContext);

            this.CardRepository = new CardRepository(this.CommonContext);
            this.AddressRepository = new AddressRepository(this.CommonContext);
            this.ContactRepository = new ContactRepository(this.CommonContext);

            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Date;
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
            InitializeEntity();
            InitializeShipmentNumber();
            InitializeMasterEntity();
            InitializeFlags();
        }

        private void InitializeLoggedTenant()
        {
            LoggedTenant = TenantRepository.GetSingleTenant(Tenant, true);
            LoggedTenant.LogBoxTenantSetting = LogBoxTenantSettingRepository.GetSingleLBTenantSetting(Tenant);
        }
        private void InitializeLoggedContact()
        {
            ContactQuery contactQuery = new ContactQuery(Tenant);
            LoggedContact = contactQuery.GetContactByNameAndTenant(LoggedContactEmail, Tenant, true);

            if (LoggedContact == null)
            {
                LoggedContact = contactQuery.GetContactByEmailOnly(LoggedContactEmail, Tenant);
            }

            if (LoggedContact != null)
            {
                this.LoggedContactId = LoggedContact.Id;
                this.LoggedContactName = LoggedContact.EnglishName;
            }
        }
        private void InitializeEntity()
        {
            if (this.IsNewEntity)
            {
                EntityPM.Id = IdCounter.GetNumber("Shipment", Tenant).ToString();
                EntityPM.SecurityKey = Guid.NewGuid().ToString("N");

                EntityPOCO = new Shipment()
                {
                    Id = EntityPM.Id,
                    SecurityKey = EntityPM.SecurityKey,
                    Tenant = EntityPM.Tenant,
                };
            }

            else
            {
                EntityPOCO = Repository.GetSingleShipment(EntityPM.Id, Tenant);
            }
        }
        private void InitializeShipmentNumber()
        {
            // Ayman: We need this here before InitializeMasterEntity

            if (IsNewEntity)
            {
                if (!EntityPM.IsHybrid)
                {
                    this.GetCounterShipmentNumber();
                }
            }

            else
            {
                if (!EntityPOCO.IsCancelled || !EntityPM.IsCancelled)
                {
                    if (EntityPM.ShipmentDirectionConverted)
                    {
                        if (EntityPM.ShipmentConvertedNewNumber)
                        {
                            this.GetCounterShipmentNumber();
                        }
                    }
                }
            }
        }

        private void GetCounterShipmentNumber()
        {
            bool isTakenCounter = false;

            if (IsNewEntity && EntityPM.ShipmentNumber == null)
            {
                isTakenCounter = true;
            }

            else if (EntityPM.ShipmentDirectionConverted && EntityPM.ShipmentConvertedNewNumber)
            {
                isTakenCounter = true;

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = Tenant,
                    EventTypeCode = "SNOC",
                    UserId = LoggedContactId,
                    EntityId = EntityPM.Id,
                    ObjectTableName = "Shipment",
                    Notes = "Old Number: " + EntityPM.ShipmentNumber,
                    Entity = EntityPM,
                });
            }

            if (isTakenCounter)
            {
                Dictionary<string, string> counterAdditionalParameters = new Dictionary<string, string>() { { "[B]", "" } };
                if (!string.IsNullOrEmpty(EntityPM.BranchId))
                {
                    BranchRepository branchRepository = new BranchRepository(CommonContext);
                    Branch myBranch = branchRepository.GetSingleBranch(EntityPM.BranchId, EntityPM.Tenant);

                    if (myBranch != null && !string.IsNullOrEmpty(myBranch.CounterCode))
                    {
                        counterAdditionalParameters["[B]"] = myBranch.CounterCode;
                    }
                }

                if (EntityPM.ShipmentLevelCode == "C")
                {
                    EntityPM.ShipmentNumber = TableCounter.GetNumber(Tenant, "MAST", EntityPM.DirectionId, EntityPM.TransportModeId, counterAdditionalParameters);
                }

                else
                {
                    if (EntityPM.DirectionId.ToUpper() == "C")
                    {
                        EntityPM.ShipmentNumber = TableCounter.GetNumber(Tenant, "SHIP", "I", EntityPM.TransportModeId, counterAdditionalParameters);
                    }

                    else
                    {
                        EntityPM.ShipmentNumber = TableCounter.GetNumber(Tenant, "SHIP", EntityPM.DirectionId, EntityPM.TransportModeId, counterAdditionalParameters);
                    }
                }
            }
        }

        private void InitializeMasterEntity()
        {
            if (this.IsNewEntity)
            {
                if (EntityPM.ShipmentLevelCode != "H")
                {
                    EntityMasterData = new ShipmentMasterData();
                    EntityMasterData.Id = EntityPM.Id;
                    EntityMasterData.MasterShipmentNumber = EntityPM.ShipmentNumber;
                    EntityPM.MasterShipmentDataId = EntityPM.Id;
                    MasterDataRepository.Add(EntityMasterData);
                }

                else if (EntityPM.ShipmentLevelCode == "H" && EntityPM.MasterShipmentDataId != null)
                {
                    // this case is when create house from master sceen
                    // need to get the master, some fields need to be calculated from the master
                    // but we dont want to map the master it self
                    EntityMasterData = MasterDataRepository.GetSingleMasterData(EntityPM.MasterShipmentDataId);

                    if (EntityMasterData != null)
                    {
                        EntityPM.ComputedShipmentNumber = EntityMasterData.MasterShipmentNumber;

                        if (EntityMasterData.ProrateReceivables)
                        {
                            IsUpdatingRegistryDate = true;
                            IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }
            }

            else
            {
                string masterDataId = null;

                if (EntityPM.ShipmentLevelCode == "H")
                {
                    masterDataId = EntityPM.MasterShipmentDataId;
                }

                else
                {
                    masterDataId = EntityPOCO.MasterShipmentDataId;
                }

                if (masterDataId != null)
                {
                    EntityMasterData = MasterDataRepository.GetSingleMasterData(masterDataId);
                }

                if (EntityPM.IsHybrid)
                {
                    if (EntityMasterData == null)
                    {
                        if (EntityPM.ShipmentLevelCode != "H")
                        {
                            if (!EntityPM.ConvertFromDirectToHouse && !EntityPM.ConvertFromHouseToDirect)
                            {
                                EntityMasterData = new ShipmentMasterData();
                                EntityMasterData.Id = EntityPM.Id;
                                EntityPM.MasterShipmentDataId = EntityPM.Id;
                                EntityMasterData.MasterShipmentNumber = EntityPM.ShipmentNumber;
                                MasterDataRepository.Add(EntityMasterData);
                            }
                        }
                    }
                }
            }
        }
        private void InitializeFlags()
        {
            this.IsFCLEntity = MethodHelper.IsFCLEntity(EntityPM.TransportModeId, EntityPM.ShipmentTypeId);
            this.IsLCLEntity = !this.IsFCLEntity;

            if (EntityMasterData != null)
            {
                if (IsNewEntity)
                {
                    if (EntityPM.ShipmentLevelCode == "H")
                    {
                        if (EntityMasterData.ProrateReceivables)
                        {
                            IsUpdatingRegistryDate = true;
                            IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }

                else
                {
                    if (EntityPM.ShipmentLevelCode == "C")
                    {
                        if (EntityPM.ProrateReceivables != EntityMasterData.ProrateReceivables)
                        {
                            IsProratingChanged = true;
                            IsUpdatingRegistryDate = true;
                            IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }
            }
        }

        public void HandleBehaviours()
        {
            List<IServiceBehaviour> serviceBehaviours = new List<IServiceBehaviour>();

            serviceBehaviours.Add(new MapCompositionBehaviour());
            serviceBehaviours.Add(new ShipmentFieldsBehaviour());
            serviceBehaviours.Add(new ShipmentPartnersBehaviour());
            serviceBehaviours.Add(new ShipmentCustomerBehaviour());
            serviceBehaviours.Add(new ShipmentCustomerUsersBehaviour());
            serviceBehaviours.Add(new ShipmentCustomerWorkingDaysBehaviour());
            serviceBehaviours.Add(new ShipmentQuoteBehaviour());

            //serviceBehaviours.Add(new ShipmentNumberCounterBehaviour());           

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }

        internal void SetCustomer(Customer customer)
        {
            this.Customer = customer;
        }
    }
}

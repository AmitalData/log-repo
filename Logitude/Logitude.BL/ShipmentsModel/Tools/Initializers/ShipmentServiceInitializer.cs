using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.CompositionBehaviours;
using Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators;
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
        public ShipmentMasterData EntityMasterData { get; set; } //private set;
        public IShipmentsContext ShipmentContext { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        public ShipmentRepository Repository { get; private set; }
        public ShipmentMasterDataRepository MasterDataRepository { get; private set; }
        public CardRepository CardRepository { get; private set; }
        public AddressRepository AddressRepository { get; private set; }
        public ContactRepository ContactRepository { get; private set; }
        public ShipmentPackageRepository ShipmentPackageRepository { get; private set; }
        public ShipmentContainerStatusRepository ShipmentContainerStatusRepository { get; private set; }
        public InsideShipmentPackageRepository InsideShipmentPackageRepository { get; private set; }
        public ShipmentPackageItemRepository ShipmentPackageItemRepository { get; private set; }
        public ShipmentPackageHarmonizeRepository ShipmentPackageHarmonizeRepository { get; private set; }
        public ShipmentOrderPackageRepository ShipmentOrderPackageRepository { get; private set; }

        public Tenant LoggedTenant { get; private set; }
        public ContactPM LoggedContact { get; private set; }
        public string LoggedContactId { get; private set; }
        public string LoggedContactEmail { get; private set; }
        public string LoggedContactName { get; private set; }

        public DateTime TodayDate { get; private set; }
        public DateTime TodayDateTime { get; private set; }
        public bool IsLCLEntity { get; private set; }
        public bool IsFCLEntity { get; private set; }
        public bool IsProratingChanged { get; set; } //private set;
        public bool IsUpdatingRegistryDate { get; set; } //private set;
        public bool IsUpdatingFirstApprovalDate { get; set; } //private set;
        public bool IsMappingComposition { get; internal set; }
        public bool IsUpdatingSubType { get; set; }
        public bool IsUpdatingProfitFromConversion { get; set; }
        public bool IsUpdatingHouses { get; set; }
        public bool IsUpdatingHousesFinalArrivalDate { get; set; }
        public List<string> DeletedHousesIds { get; set; }
        public List<string> ConnectedHousesIds { get; set; }

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
            this.ShipmentPackageRepository = new ShipmentPackageRepository(ShipmentContext);
            this.ShipmentContainerStatusRepository = new ShipmentContainerStatusRepository(ShipmentContext);
            this.InsideShipmentPackageRepository = new InsideShipmentPackageRepository(ShipmentContext);
            this.ShipmentPackageItemRepository = new ShipmentPackageItemRepository(ShipmentContext);
            this.ShipmentPackageHarmonizeRepository = new ShipmentPackageHarmonizeRepository(ShipmentContext);
            this.ShipmentOrderPackageRepository = new ShipmentOrderPackageRepository(ShipmentContext);

            this.CardRepository = new CardRepository(this.CommonContext);
            this.AddressRepository = new AddressRepository(this.CommonContext);
            this.ContactRepository = new ContactRepository(this.CommonContext);

            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Date;

            this.DeletedHousesIds = new List<string>();
            this.ConnectedHousesIds = new List<string>();
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
            InitializeEntity();
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
        private void InitializeFlags()
        {
            this.IsFCLEntity = MethodHelper.IsFCLEntity(EntityPM.TransportModeId, EntityPM.ShipmentTypeId);
            this.IsLCLEntity = !this.IsFCLEntity;
        }

        public void HandleBehaviours()
        {
            List<IServiceBehaviour> serviceBehaviours = new List<IServiceBehaviour>();

            serviceBehaviours.Add(new MapCompositionBehaviour());
            serviceBehaviours.Add(new ShipmentFieldsBehaviour());
            serviceBehaviours.Add(new ShipmentNumberBehaviour());
            serviceBehaviours.Add(new ShipmentMasterEntityBehaviour());
            serviceBehaviours.Add(new ShipmentPartnersBehaviour());
            serviceBehaviours.Add(new ShipmentCustomerBehaviour());
            serviceBehaviours.Add(new ShipmentCustomerUsersBehaviour());
            serviceBehaviours.Add(new ShipmentCustomerWorkingDaysBehaviour());
            serviceBehaviours.Add(new ShipmentQuoteBehaviour());
            serviceBehaviours.Add(new ShipmentConversionBehaviour());
            serviceBehaviours.Add(new ShipmentOperationalDateBehaviour());
            serviceBehaviours.Add(new UpdateDocumentFilingBehaviour());

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }

        public void HandleComposition()
        {
            List<IServiceBehaviour> behaviours = new List<IServiceBehaviour>();

            behaviours.Add(new OrderPackagesBehaviour());

            foreach (IServiceBehaviour behaviour in behaviours)
            {
                behaviour.Handle(this);
            }
        }

        public void HandleValidators()
        {
            List<IServiceValidator> validators = new List<IServiceValidator>();

            if (!LoggedTenant.LogBoxTenantSetting.IsDocumentsArchive)
            {
                validators.Add(new ShipmentMasterIsUsedValidator());
            }

            foreach (IServiceValidator behaviour in validators)
            {
                behaviour.Validate(this);
            }
        }

        internal void SetCustomer(Customer customer)
        {
            this.Customer = customer;
        }
    }
}

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
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
        public Tenant LoggedTenant { get; private set; }
        public ContactPM LoggedContact { get; private set; }
        public string LoggedContactId { get; private set; }
        public string LoggedContactEmail { get; private set; }
        public DateTime? TodayDate { get; private set; }
        public DateTime? TodayDateTime { get; private set; }
        public bool IsLCLEntity { get; private set; }
        public bool IsFCLEntity { get; private set; }
        public bool IsUpdatingRegistryDate { get; private set; }
        public bool IsUpdatingFirstApprovalDate { get; private set; }

        private List<IServiceBehaviour> serviceBehaviours;
        public ShipmentServiceInitializer(IShipmentsContext ShipmentContext, ShipmentPM entityPM, string serviceContextUser)
        {
            this.EntityPM = entityPM;
            this.Tenant = entityPM.Tenant;
            this.IsNewEntity = entityPM.Id == null ? true : false;
            this.ShipmentContext = ShipmentContext;
            this.LoggedContactEmail = serviceContextUser;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.Repository = new ShipmentRepository(ShipmentContext);
            this.MasterDataRepository = new ShipmentMasterDataRepository(ShipmentContext);

            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Value.Date;
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
            InitializeEntity();
            InitializeEntityMasterData();
            InitializeFlags();

            InitializeServiceBehaviours();
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
                    SecurityKey = EntityPM.SecurityKey
                };
            }

            else
            {
                EntityPOCO = Repository.GetSingleShipment(EntityPM.Id, Tenant);
            }
        }
        private void InitializeEntityMasterData()
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
                            IsUpdatingRegistryDate = true;
                            IsUpdatingFirstApprovalDate = true;
                        }
                    }
                }
            }
        }
        private void InitializeServiceBehaviours()
        {
            serviceBehaviours = new List<IServiceBehaviour>();

            serviceBehaviours.Add(new ShipmentFieldsBehaviour());
            serviceBehaviours.Add(new ShipmentQuoteUsageBehaviour());
            serviceBehaviours.Add(new ShipmentNumberCounterBehaviour());           

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }
    }
}

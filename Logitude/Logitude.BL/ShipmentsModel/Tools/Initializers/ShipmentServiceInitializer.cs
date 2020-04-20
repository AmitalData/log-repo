using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
        public IShipmentsContext ShipmentContext { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        public Tenant LoggedTenant { get; private set; }
        public ContactPM LoggedContact { get; private set; }
        public bool IsLCLEntity { get; private set; }
        public bool IsFCLEntity { get; private set; }
        private ShipmentRepository entityRepository;
        private List<IServiceBehaviour> serviceBehaviours;
        public ShipmentServiceInitializer(ShipmentPM entityPM)
        {
            this.EntityPM = entityPM;
            this.Tenant = entityPM.Tenant;
            this.IsNewEntity = entityPM.Id == null ? true : false;
            this.ShipmentContext = ShipmentsContext.GetContext(Tenant);
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.entityRepository = new ShipmentRepository(ShipmentContext);
            this.IsLCLEntity = MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId);
            this.IsFCLEntity = !this.IsLCLEntity;
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
            InitializeCurrentEntity();
            InitializeServiceBehaviours();
        }

        private void InitializeLoggedTenant()
        {
            LoggedTenant = TenantRepository.GetSingleTenant(Tenant, true);
            LoggedTenant.LogBoxTenantSetting = LogBoxTenantSettingRepository.GetSingleLBTenantSetting(Tenant);
        }
        private void InitializeLoggedContact()
        {
            ContactPM loggedContact = null;
            string loggedContactEmail = null;

            if (loggedContactEmail != null)
            {
                ContactQuery contactQuery = new ContactQuery(Tenant);
                loggedContact = contactQuery.GetContactByNameAndTenant(loggedContactEmail, Tenant, true);

                if (loggedContact == null)
                {
                    loggedContact = contactQuery.GetContactByEmailOnly(loggedContactEmail, Tenant);
                }
            }

            else
            {
                loggedContact = LoggedContactResolver.GetLoggedContact(Tenant);
            }

            LoggedContact = loggedContact;
        }
        private void InitializeCurrentEntity()
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
                EntityPOCO = entityRepository.GetSingleShipment(EntityPM.Id, Tenant);
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

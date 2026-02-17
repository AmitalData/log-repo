using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CompetitorService
    {
        bool isNewEntity;
        private int tenant;
        public Competitor Poco { get; set; }
        private CompetitorPM entityPM;
        private ICommonDataContext objectContext;
        private CompetitorRepository entityRepository;
        private AddressRepository addressRepository;
        public CompetitorService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CompetitorRepository(objectContext);
            this.addressRepository = new AddressRepository(tenant);
        }

        public void Create(CompetitorPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.Id = IdCounter.GetNumber("Competitor", tenant).ToString();
            this.Poco = new Competitor() { Id = entityPM.Id, Tenant = tenant };

            this.InitializeComponent();

            CompetitorTracing.Trace(entityPM, Poco, isNewEntity);
            CompetitorMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CompetitorPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCompetitor(entityPM.Id, entityPM.Tenant);

            this.InitializeComponent();

            CompetitorTracing.Trace(entityPM, Poco, isNewEntity);
            CompetitorMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            if (isNewEntity)
            {
                Address address = new Address()
                {
                    Id = IdCounter.GetNumber("Address", tenant).ToString(),
                    Tenant = tenant,
                    AddressTypeId = "M",
                    Description = "Competitor",
                    Name = "Competitor",

                    Address1 = entityPM.Address1,
                    Address2 = entityPM.Address2,
                    City = entityPM.City,
                    CountryId = entityPM.CountryId,
                    StateId = entityPM.StateId,
                    ZipCode = entityPM.ZipCode,
                    FaxNumber = entityPM.FaxNumber,
                    PhoneNumber = entityPM.PhoneNumber,
                    SearchFields = entityPM.Address1 + "," + entityPM.Address2
                };

                addressRepository.Add(address);
                addressRepository.SubmitChanges();
                entityPM.AddressId = address.Id;
            }

            else
            {
                Address address = addressRepository.GetSingleAddress(entityPM.AddressId, tenant);
                if (address != null)
                {
                    address.Address1 = entityPM.Address1;
                    address.Address2 = entityPM.Address2;
                    address.City = entityPM.City;
                    address.CountryId = entityPM.CountryId;
                    address.StateId = entityPM.StateId;
                    address.ZipCode = entityPM.ZipCode;
                    address.FaxNumber = entityPM.FaxNumber;
                    address.PhoneNumber = entityPM.PhoneNumber;
                    address.SearchFields = entityPM.Address1 + "," + entityPM.Address2;
                    addressRepository.Update(address);
                    addressRepository.SubmitChanges();
                }
            }
        }
    }
}

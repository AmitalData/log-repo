using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Behaviours;
using Logitude.BL.QuoteModel.Tools.Behaviours.QuoteBehaviours;
using Logitude.CRM.Data;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.Initializers
{
    public class QuoteServiceInitializer : IServiceInitializer
    {
        public int Tenant { get; private set; }
        public bool IsNewEntity { get; private set; }
        public Quote EntityPOCO { get; private set; }
        public QuotePM EntityPM { get; private set; }
        public IQuotesContext Context { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        public IShipmentsContext ShipmentContext { get; private set; }
        public ICRMContext CRMcontext { get; private set; }
        public QuoteRepository Repository { get; private set; }
        public DateTime? TodayDate { get; private set; }
        public DateTime? TodayDateTime { get; private set; }
        public Tenant LoggedTenant { get; private set; }
        public QuoteComputedFieldRepository quoteComputedFieldRepository { get; private set; }
        public QuoteComputedField quoteComputedFieldPOCO { get; private set; }
        public QuoteComputedFieldPM quoteComputedFieldPM { get; private set; }
        public AddressRepository addressRepository { get; private set; }
        public CountryRepository countryRepository { get; private set; }
        public string LoggedContactId { get; private set; }
        private string loggedEmail;

        public QuoteServiceInitializer(IQuotesContext objectContext, int tenant, string loggedContactEmail)
        {            
            this.Tenant = tenant;
            this.Context = objectContext;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.ShipmentContext = ShipmentsContext.GetContext(Tenant);
            this.CRMcontext = CRMContext.GetContext(Tenant);
            this.countryRepository = new CountryRepository(Tenant);
            this.Repository = new QuoteRepository(Context);
            this.quoteComputedFieldRepository = new QuoteComputedFieldRepository(Tenant);
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Value.Date;
            this.loggedEmail = loggedContactEmail;
            this.addressRepository = new AddressRepository(Tenant);
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
        }

        public void InitializeEntity(QuotePM entityPM)
        {
            this.EntityPM = entityPM;
            this.IsNewEntity = entityPM.Id == null ? true : false;

            if (this.IsNewEntity)
            {
                EntityPM.Id = IdCounter.GetNumber("Quote", Tenant).ToString();

                EntityPOCO = new Quote()
                {
                    Id = EntityPM.Id,
                };
            }

            else
            {
                EntityPOCO = Repository.GetSingleQuote(EntityPM.Id, Tenant);
            }
            this.InitializeQuoteComputedFieldsEntity();
        }

        public void InitializeQuoteComputedFieldsEntity()
        {
            if (this.IsNewEntity)
            {

                this.quoteComputedFieldPOCO = new QuoteComputedField()
                {
                    Id = EntityPM.Id,
                    Tenant = EntityPM.Tenant
                };

            }
            else  
            {
                this.quoteComputedFieldPOCO = quoteComputedFieldRepository.GetSingleQuoteComputedField(EntityPM.Id);
            }
        }

        private void InitializeLoggedTenant()
        {
            LoggedTenant = TenantRepository.GetSingleTenant(Tenant, true);
        }

        private void InitializeLoggedContact()
        {
            ContactQuery contactQuery = new ContactQuery(Tenant);
            ContactPM loggedContact = contactQuery.GetContactByNameAndTenant(loggedEmail, Tenant, true);

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(loggedEmail, Tenant);
            }

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetSinglePM(EntityPM.CreatedByUserId, Tenant);
            }

            this.LoggedContactId = loggedContact.Id;
        }



        public void HandleBehaviours()
        {
            List<IServiceBehaviour> serviceBehaviours = new List<IServiceBehaviour>();

            serviceBehaviours.Add(new QuoteFieldsBehaviour());
            serviceBehaviours.Add(new QuoteSalesmanBehavior());
            serviceBehaviours.Add(new UpdatequoteComputedFieldBehaviour());

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }


    }
}

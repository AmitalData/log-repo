using Amital.QuoteOPM.BL.Tools.Behaviours;
using Amital.QuoteOPM.BL.Tools.Behaviours.QuoteBehaviours;
using Amital.QuoteOPM.Data;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.CRM.Data;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
//using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.Tools.Initializers
{
    public class QuoteOPServiceInitializer : IServiceInitializer
    {
        public int Tenant { get; private set; }
        public bool IsNewEntity { get; private set; }
        public QuoteOP EntityPOCO { get; private set; }
        public QuoteOPPM EntityPM { get; private set; }
        public IQuoteOPMContext Context { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        //public IShipmentsContext ShipmentContext { get; private set; }
        //public ICRMContext CRMcontext { get; private set; }
        public QuoteOPRepository Repository { get; private set; }
        public DateTime? TodayDate { get; private set; }
        public DateTime? TodayDateTime { get; private set; }
        public Tenant LoggedTenant { get; private set; }
        public QuoteOPComputedFieldRepository QuoteComputedFieldRepository { get; private set; }
        public QuoteOPComputedField QuoteComputedFieldPOCO { get; private set; }
        public QuoteOPComputedFieldPM QuoteComputedFieldPM { get; private set; }
        public AddressRepository AddressRepository { get; private set; }
        public CountryRepository CountryRepository { get; private set; }
        public string LoggedContactId { get; private set; }
        private string loggedEmail;

        public QuoteOPServiceInitializer(IQuoteOPMContext objectContext, int tenant, string loggedContactEmail)
        {            
            this.Tenant = tenant;
            this.Context = objectContext;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            //this.ShipmentContext = ShipmentsContext.GetContext(Tenant);
            //this.CRMcontext = CRMContext.GetContext(Tenant);
            this.CountryRepository = new CountryRepository(Tenant);
            this.Repository = new  QuoteOPRepository(Context);
            this.QuoteComputedFieldRepository = new QuoteOPComputedFieldRepository(Tenant);
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Value.Date;
            this.loggedEmail = loggedContactEmail;
            this.AddressRepository = new AddressRepository(Tenant);
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
        }

        public void InitializeEntity(QuoteOPPM entityPM)
        {
            this.EntityPM = entityPM;
            this.IsNewEntity = entityPM.Id == null ? true : false;

            if (this.IsNewEntity)
            {
                EntityPM.Id = IdCounter.GetNumber("QuoteOP", Tenant).ToString();

                EntityPOCO = new QuoteOP()
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

                this.QuoteComputedFieldPOCO = new QuoteOPComputedField()
                {
                    Id = EntityPM.Id,
                    Tenant = EntityPM.Tenant
                };
                this.QuoteComputedFieldPM = new QuoteOPComputedFieldPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Id = EntityPM.Id,
                    Tenant = EntityPM.Tenant
                };
            }
            else  
            {
                this.QuoteComputedFieldPOCO = QuoteComputedFieldRepository.GetSingleQuoteComputedField(EntityPM.Id);
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

            serviceBehaviours.Add(new QuoteOPFieldsBehaviour());
            serviceBehaviours.Add(new QuoteOPSalesmanBehavior());
            serviceBehaviours.Add(new UpdateQuoteOPComputedFieldBehaviour());

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }


    }
}

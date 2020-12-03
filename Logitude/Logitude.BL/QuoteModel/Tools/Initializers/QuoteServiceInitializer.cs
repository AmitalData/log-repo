using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Behaviours.QuoteBehaviours;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
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
        public QuoteRepository Repository { get; private set; }
        public DateTime? TodayDate { get; private set; }
        public DateTime? TodayDateTime { get; private set; }
        public Tenant LoggedTenant { get; private set; }
        public string LoggedContactId { get; private set; }
        private string loggedEmail;
        public QuoteServiceInitializer(IQuotesContext objectContext, int tenant, string loggedContactEmail)
        {            
            this.Tenant = tenant;
            this.Context = objectContext;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.Repository = new QuoteRepository(Context);
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Value.Date;
            this.loggedEmail = loggedContactEmail;
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

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }


    }
}

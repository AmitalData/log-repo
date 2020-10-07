using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.Initializers
{
    public class ARInvoiceServiceInitializer
    {
        public int Tenant { get; private set; }
        public bool IsNewEntity { get; private set; }
        public ARInvoice EntityPOCO { get; private set; }
        public ARInvoicePM EntityPM { get; private set; }
        public IInvoiceContext Context { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        public ARInvoiceRepository Repository { get; private set; }
        public Tenant LoggedTenant { get; private set; }
        public AccountingSystem AccountingSystem { get; private set; }
        public AccountingSetting AccountingSetting { get; private set; }
        public string LoggedContactId { get; private set; }
        public string loggedContactName { get; private set; }
        public DateTime? TodayDate { get; private set; }
        public DateTime? TodayDateTime { get; private set; }
        public ARInvoiceServiceInitializer(IInvoiceContext objectContext, ARInvoicePM entityPM)
        {
            this.EntityPM = entityPM;
            this.Tenant = entityPM.Tenant;
            this.IsNewEntity = entityPM.Id == null ? true : false;
            this.Context = objectContext;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.Repository = new ARInvoiceRepository(Context);
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Value.Date;
        }

        public void Initialize()
        {
            //InitializeLoggedTenant();
            InitializeLoggedContact();
            //InitializeAccountingSetting();
            //InitializeAccountingSystem();
        }

        private void InitializeLoggedContact()
        {
            ContactPM loggedContact = null;

            if (EntityPM.IsFromConsolidationBatch)
            {
                ContactRepository contactRepository = new ContactRepository(CommonContext);
                ContactQuery contactQuery = new ContactQuery(contactRepository);
                loggedContact = contactQuery.GetSinglePM(EntityPM.UpdatedByUserId, Tenant);

                if (loggedContact == null)
                {
                    loggedContact = contactQuery.GetSinglePM(EntityPM.UpdatedByUserId, 0);
                }
            }

            else
            {
                loggedContact = LoggedContactResolver.GetLoggedContact(Tenant);
            }

            if (loggedContact != null)
            {
                LoggedContactId = loggedContact.Id;
                loggedContactName = loggedContact.EnglishName;
            }
        }

        public void HandleBehaviours()
        {
            List<IServiceBehaviour> serviceBehaviours = new List<IServiceBehaviour>();
            {

            }
        }
    }
}

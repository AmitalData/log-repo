using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Transactions;
    using Simplog.Global.Data.GlobalModel;
    using Simplog.Global.Data.GlobalModel.Repositories;
    using Logitude.BL.CommonDataModel.EntityPMs;
    using Simplog.Server.Infrastructure.Helpers;

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public class PreLoginDomainService : DomainService
    {
        private IGlobalContext objectContext;
        public IGlobalContext ObjectContext
        {
            get
            {
                return objectContext;
            }
            set
            {
                objectContext = value;
            }
        }

        PasswordResetRequestRepository passwordResetRequestsRepository;
        public PasswordResetRequestRepository PasswordResetRequestsRepository
        {
            get { return passwordResetRequestsRepository; }
            set { passwordResetRequestsRepository = value; }
        }
        public PreLoginDomainService(IGlobalContext context)
        {
            PasswordResetRequestsRepository = new PasswordResetRequestRepository(context);
        }

        public PreLoginDomainService()
        {
            ObjectContext = GlobalContext.GetContext();
            PasswordResetRequestsRepository = new PasswordResetRequestRepository(ObjectContext);
        }

        public TenantPM GetTenantsById(int id)
        {
            TenantQuery tenantQuery = new TenantQuery(id);
            return tenantQuery.GetSinglePM(id);
        }

        public List<ContactPM> GetContactsByEmail(string email, int tenant)
        {
            
            //ContactRepository contactsRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(tenant);
            return contactQuery.GetContactsByEmail(email, tenant);
          
        }

        public IQueryable<ContactTenant> GetContactTenantsForContact(string contactId, int tenant)
        {
            //this.ChangeConnectionString(tenant);
            ContactTenantRepository contactTenantsRepository = new ContactTenantRepository(tenant);
            
            return contactTenantsRepository.GetContactTenants(tenant).Where(ct => ct.ContactId == contactId);
        }

        public List<TenantPM> GetTenantsForContactsByEmail(string email)
        {
            List<int> tenantIds = new List<int>();
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                GlobalContactRepository globalContactRep = new GlobalContactRepository();
                List<GlobalContact> globalContacts = globalContactRep.GetContactByEmail(email).ToList();

               
                foreach (GlobalContact contact in globalContacts)
                {
                    tenantIds.Add(contact.GlobalTenantId);
                }
                scope.Complete();
            }
            List<TenantPM> tenants = new List<TenantPM>();
            foreach (int tenant in tenantIds)
            {
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM tenantpm = tenantQuery.GetSinglePM(tenant);
                tenants.Add(tenantpm);
            }
            return tenants;
            //ContactRepository ContactsRepository = new ContactRepository();
            //ContactTenantRepository contactTenantsRepository = new ContactTenantRepository();
           
            //List<ContactTenant> contactTenants = contactTenantsRepository.GetContactTenantForContact(email).ToList();
          
          

            //List<Tenant> tenants = (from a in contactTenants
            //                       select a.Tenant).ToList();

            //return (from a in tenants
            //        select new TenantPM()
            //        {
            //            AddressId = a.AddressId,
            //            Company = a.Company,
            //            CurrencyId = a.CurrencyId,
            //            Direction = a.Direction,
            //            Email = a.Email,
            //            Format = a.Format,
            //            Id = a.Id,
            //            Language = a.Language,
            //            Website = a.Website,
            //            Version=a.Version,
            //            IsActive=a.IsActive,
            //        }).ToList();
           
        }

        #region PasswordResetRequests
        public IQueryable<PasswordResetRequest> GetPasswordResetRequests()
        {
            //this.ChangeConnectionString(tenant);
            //SecurityUtility.AuthenticationOnTenant(tenant);
            PasswordResetRequestsRepository = new PasswordResetRequestRepository();
            return PasswordResetRequestsRepository.GetPasswordResetRequests();
        }

        public PasswordResetRequest GetSinglePasswordResetRequest(string requestNumber, int tenant)
        {
            //this.ChangeConnectionString(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            PasswordResetRequestsRepository = new PasswordResetRequestRepository();
            return PasswordResetRequestsRepository.GetSinglePasswordResetRequest(requestNumber);
        }

        public void InsertPasswordResetRequest(PasswordResetRequest newEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = GlobalContext.GetContext();
            }

            PasswordResetRequestsRepository = new PasswordResetRequestRepository(ObjectContext);
            PasswordResetRequestsRepository.Add(newEntity);
        }

        public void UpdatePasswordResetRequest(PasswordResetRequest currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = GlobalContext.GetContext();
            }

            PasswordResetRequestsRepository = new PasswordResetRequestRepository(ObjectContext);

            PasswordResetRequestsRepository.Update(currentEntity);
        }

        public void DeletePasswordResetRequest(PasswordResetRequest entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = GlobalContext.GetContext();
            }

            PasswordResetRequestsRepository = new PasswordResetRequestRepository(ObjectContext);
            PasswordResetRequestsRepository.Remove(entity);
        }
        #endregion

        protected override bool PersistChangeSet()
        {
            ObjectContext.SaveChanges();
            return base.PersistChangeSet();
        }
    }
}



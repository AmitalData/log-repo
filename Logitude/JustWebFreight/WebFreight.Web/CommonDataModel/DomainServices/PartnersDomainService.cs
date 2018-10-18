using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using WebFreight.Web.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    // Implements application logic using the CommonDataContext context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public partial class PartnersDomainService : LogitudeDomainService 
    {
        private ICommonDataContext objectContext;
        public ICommonDataContext ObjectContext { get { return objectContext; } set { objectContext = value; } }

        private AgentRepository agentRepository;     
        private ContactTenantRoleRepository contactTenantRoleRepository;
        private RoleRepository roleRepository;
        private CustomAgentRepository customAgentRepository;
        private ShippingAgentRepository shippingAgentRepository;
        private ContactTenantRepository contactTenantRepository;
        private AirlineRepository airlineRepository;
        private TermsofUseRepository termsofUseRepository;
        private TermsofUseSignatureRepository termsofUseSignatureRepository;
        private ShippingLineRepository shippingLineRepository;
        private TruckerRepository truckerRepository;
        private GlobalZoneRepository globalZoneRepository;
        private CountryRepository countryRepository;
        private StateRepository stateRepository;
        private MAWBStackRepository mAWBStackRepository;
        private TarrifChargeRepository tarrifChargeRepository;
        private TarrifFromToRepository tarrifFromToRepository;
        private TarrifFromToTypeRepository tarrifFromToTypeRepository;
        private TarrifTypeRepository tarrifTypeRepository;
        private TarrifHeaderRepository tarrifHeaderRepository;
        private TarrifStepRepository tarrifStepRepository;
        private VendorRepository vendorRepository;
        private WarehouseRepository warehouseRepository;
        private ParticipantRepository participantRepository;

        public CardRepository CardRepository { get; set; }
        public CardContactRepository CardContactRepository { get; set; }
        public AddressRepository AddressRepository { get; set; }
        public ContactRepository ContactRepository { get; set; }
        public CustomerRepository CustomerRepository { get; set; }
        public CardExternalCodeByCurrencyRepository cardExternalCodeByCurrencyRepository { get; set; }

        private AddressQuery addressQuery;
        private AgentQuery agentQuery;
        private AirlineQuery airlineQuery;
        private CardContactQuery cardContactQuery;
        private CardQuery cardQuery;
        private CustomAgentQuery customAgentQuery;
        private CustomerQuery customerQuery;
        private ShippingAgentQuery shippingAgentQuery;
        private ShippingLineQuery shippingLineQuery;
        private TruckerQuery truckerQuery;
        private VendorQuery vendorQuery;
        private WarehouseQuery warehouseQuery;
        private MAWBStackQuery mawbStackQuery;
        private TarrifTypeQuery tarrifTypeQuery;
        private TarrifStepQuery tarrifStepQuery;
        private TarrifHeaderQuery tarrifHeaderQuery;
        private TarrifFromToTypeQuery tarrifFromToTypeQuery;
        private TermsofUseQuery termsofUseQuery;
        private TermsofUseSignatureQuery termsofUseSignatureQuery;
        private CardExternalCodeByCurrencyQuery cardExternalCodeByCurrencyQuery;
        private ParticipantQuery participantQuery;

        private string variable;

        public PartnersDomainService()
        {
            variable = Guid.NewGuid().ToString();
        }
        
        public CRMDataCounts GetDataCountsForCRM(int tenant,string customerid)
        {
            ShipmentRepository shipmentrep = new ShipmentRepository(tenant);
            QuoteRepository quoteRep = new QuoteRepository(tenant);

            CRMDataCounts datacounts = new CRMDataCounts();
            datacounts.AllQuotes = quoteRep.GetAllQuotesCountForCustomer(tenant, customerid);
            datacounts.OpenQuotes = quoteRep.GetOpenQuotesCountForCustomer(tenant,customerid);
            datacounts.AllShipments = shipmentrep.GetAllShipmentsCountForCustomer(tenant,customerid);
            datacounts.OpenShipments = shipmentrep.GetOpenShipmentsCountForCustomer(tenant, customerid);

            return datacounts;
        }

        public CRMMoneyInformation GetCRMMoneyInformation(int tenant, string customerid)
        {
            ShipmentRepository shipmentrep = new ShipmentRepository(tenant);
            ARInvoiceRepository invoiceRep = new ARInvoiceRepository(tenant);
            ARPaymentRepository paymentrep = new ARPaymentRepository(tenant);

            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(invoiceRep);
            CRMMoneyInformation moneyinfo = new CRMMoneyInformation();
            moneyinfo.ARPayments = paymentrep.GetARPaymentForCustomer(tenant, customerid);
            moneyinfo.InvoicesDue = arInvoiceQuery.GetInvoicesDueForCustomer(tenant, customerid);
            moneyinfo.OpenARInvoices = invoiceRep.GetOpenARInvoicesForCustomer(tenant, customerid);
            moneyinfo.OpenReceivables = shipmentrep.GetOpenReceivablesForCustomer(tenant, customerid);

            return moneyinfo;
        }







        protected override bool PersistChangeSet()
        {
            try
            {
                objectContext.SaveChanges();
            }
            catch (Exception ex)
            { 

            }
            return base.PersistChangeSet();
        }

        protected override bool ExecuteChangeSet()
        {
            return base.ExecuteChangeSet();
        }

        public override void Initialize(DomainServiceContext context)
        {
            base.Initialize(context);
        }

        public override System.Collections.IEnumerable Query(QueryDescription queryDescription, out IEnumerable<ValidationResult> validationErrors, out int totalCount)
        {
            return base.Query(queryDescription, out validationErrors, out totalCount);
        }

        protected override bool ValidateChangeSet()
        {
            return base.ValidateChangeSet();
        }
    }
}




using System;
using System.Windows.Forms;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.Helpers;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityLists;

namespace TestTenantConfiguration
{
    public partial class Form1 : Form
    {
        private int Tenant;

        public Form1()
        {
            CacheManager.CacheWrapper = new MockCacheWrapper();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateSignup();
            CreateAgent();
            CreateAddress();
            CreateRatesTables();

            UpdateTenant();

            //contactpasswords 
            //TODO contactpasswords
            //CREATE  Computingpartnertranslations
            //UPDATE  QuoteDomain
            //UPDATE  Tenantmanagements
        }



        private void CreateSignup()
        {
            SignUpInfoClass signUpInfo = CreateSignUpInfoInstance();
            String Password = SignUpClass.StartSignUp(signUpInfo);
            this.Tenant = signUpInfo.Tenant; 
        }

        private SignUpInfoClass CreateSignUpInfoInstance()
        {
            SignUpInfoClass signUpInfo = new SignUpInfoClass
            {
                Email = TenantEmailTextBox.Text,
                Company = TenantCompanyTextBox.Text,
                Name = TenantCompanyTextBox.Text,
                Phone = "050808080",
                PackageCode = "DVMT",
                CountryCode = "PS",
                CountryName = "State Of Palestin"
            };

            return signUpInfo;
        }

        private void CreateAgent()
        {
            AgentPM agentPM = CreateAgentInstance();
            string loggedContactId = GetContactIdByEmail(TenantEmailTextBox.Text);
            ICommonDataContext MyContext = CommonDataContext.GetContext(agentPM.Tenant);
            AgentService service = new AgentService(MyContext, agentPM, loggedContactId);
            service.Create(agentPM);
        }

        private AgentPM CreateAgentInstance()
        {
            AgentPM agentPM = new AgentPM
            {
                Code = "new",
                EnglishName = TenantCompanyTextBox.Text,
                PartnerTypeId = "AG",
                Tenant = this.Tenant,
                IsHybrid = true
            };
            return agentPM;
        }

        private string GetContactIdByEmail(String email)
        {
            ContactQuery contactQuery = new ContactQuery(this.Tenant);
            ContactPM contactPM = contactQuery.GetSingleContact(email, this.Tenant);
            return contactPM.Id;
        }

        private void CreateAddress()
        {
            AddressPM addressPM = CreateAddressInstance();
            ICommonDataContext MyContext = CommonDataContext.GetContext(addressPM.Tenant);
            AddressService service = new AddressService(MyContext, addressPM.Tenant);
            service.Create(addressPM);
        }

        private AddressPM CreateAddressInstance()
        {
            AddressPM addressPM = new AddressPM
            {
                Address1 = "address1",
                AddressTypeId = "M",
                CardId = GetAgentId(),
                City = "Ramallah",
                CountryCode = "PS",
                CountryEnglishName = "State Of Palestine",
                CountryId = GetCountryId("PS"),
                Description = TenantCompanyTextBox.Text,
                Name = TenantCompanyTextBox.Text,
                HasStates = false,
                IsStateRequired = false,
                Tenant = this.Tenant,
                IsHybrid = true
            };

            return addressPM;
        }

        private string GetCountryId(string code)
        {
            CountryQuery countryQuery = new CountryQuery(this.Tenant);
            CountryPM countryPM = countryQuery.GetSinglePMByCode(code, this.Tenant);
            return countryPM.Id;
        }

        private string GetAgentId() //since the tenant is new we will have 1 agent only so get their id
        {
            AgentQuery agentQuery = new AgentQuery(this.Tenant);
            IQueryable<AgentPM> agentPM = agentQuery.GetAgentPMsByTenant(this.Tenant);
            return agentPM.FirstOrDefault().Id;
        }

        private void CreateRatesTables()
        {
            RatesTablePM RatesPM = CreateRatesTablesInstance();
            IWebFreightContext MyContext = WebFreightContext.GetContext(RatesPM.Tenant);
            RatesTableService service = new RatesTableService(MyContext, RatesPM.Tenant);
            service.Create(RatesPM);
        }

        private RatesTablePM CreateRatesTablesInstance()
        {
            RatesTablePM RatesPM = new RatesTablePM
            {
                BaseCurrencyId = GetCurrencyId("USD" , 3.1),
                ForeignCurrencyId = GetCurrencyId("NIS",1),
                LogDateTime = TenantServerConfigration.GetCurrentDateTime(this.Tenant), 
                Rate = 3.8,
                Tenant = this.Tenant
            };
            RatesPM.ValueDate = RatesPM.ValueDate.Value.Date; //ask about it C:\Source\log-repo\Logitude\Logitude.BL\InfrastructureModel\Tools\DataMapping\RatesTableMapping.cs
            return RatesPM;
        }

        private string GetCurrencyId(string code , double rate)
        {
            string tenantCurrencyId = CopyCurrencyToTenant(code,rate);
            //CurrencyQuery currencyQuery = new CurrencyQuery(this.Tenant);
            //CurrencyPM currencyPM = currencyQuery.GetSingleCurrencyByCode(code, this.Tenant);
            //return currencyPM.Id;
            return tenantCurrencyId;
        }

        private string CopyCurrencyToTenant(string code , double CurrencyRate)
        {
            DateTime RateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
            string CurrencyId = GetCurrencyIdFromTenantZero(code);
            CommonDataDomainService commonDomain = new CommonDataDomainService();
            CurrencyList myResult = commonDomain.CopyCurrencyToTenant(CurrencyId, this.Tenant, CurrencyRate, RateDate);
            return myResult.Id;
        }

        private string GetCurrencyIdFromTenantZero(string code)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(this.Tenant);
            CurrencyPM currencyPM = currencyQuery.GetSingleCurrencyByCode(code, 0);
            return currencyPM.Id;
        }

        private void UpdateTenant()
        {
            TenantQuery tenantQuery = new TenantQuery(this.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(this.Tenant);
            tenantPM.AgentId = GetAgentId();
            //tenantPM.AddressId  = 


        }
    }
}

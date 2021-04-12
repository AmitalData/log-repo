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
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Simplog.Data.QuoteModel;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Drawing;
using System.Linq;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;

namespace TestTenantConfiguration
{
    public partial class Form1 : Form
    {
        private int Tenant;
        private string AgentId, AddressId;
        private string TenantEmail, TenantCompanyName;

        public Form1()
        {
            CacheManager.CacheWrapper = new MockCacheWrapper();
            FillAppSettings();
            ContainerAccessor.InitContainer();
            InitializeComponent();
        }

        private void FillAppSettings()
        {
            SettingRepository settingRepository = new SettingRepository();
            Setting setting = settingRepository.GetSingleSetting("1");
            LogitudeSettings.Id = setting.Id;
            LogitudeSettings.ChampEnv = setting.ChampEnv;
            LogitudeSettings.ChampURL = setting.ChampURL;
            LogitudeSettings.ChampTestAPIURL = setting.ChampTestAPIURL;
            LogitudeSettings.ChampTestAPIPassword = setting.ChampTestAPIPassword;
            LogitudeSettings.ChampProdAPIURL = setting.ChampProdAPIURL;
            LogitudeSettings.ChampProdAPIPassword = setting.ChampProdAPIPassword;
            LogitudeSettings.CustomerCareIP = setting.CustomerCareIP;
            LogitudeSettings.DeploymentStage = setting.DeploymentStage;
            LogitudeSettings.IsLogEnabled = setting.IsLogEnabled;
            LogitudeSettings.LogitudeURL = setting.LogitudeURL;
            LogitudeSettings.TotangoServiceId = setting.TotangoServiceId;
            LogitudeSettings.UsingAzure = setting.UsingAzure;
            LogitudeSettings.StorageAccountKey = setting.StorageAccountKey;
            LogitudeSettings.StorageAccountName = setting.StorageAccountName;
            LogitudeSettings.StorageType = setting.StorageType;
            LogitudeSettings.LogitudeCRMTenantNumber = setting.LogitudeCRMTenantNumber;
            LogitudeSettings.AutoSignupEmail = setting.AutoSignupEmail;
            LogitudeSettings.AutoSignupPassword = setting.AutoSignupPassword;
            LogitudeSettings.ForceHttps = setting.ForceHttps;
            LogitudeSettings.CheckConnectionURL = setting.CheckConnectionURL;
            LogitudeSettings.AndroidSharedAppMinimumVersion = setting.AndroidSharedAppMinimumVersion;
            LogitudeSettings.IOSSharedAppMinimumVersion = setting.IOSSharedAppMinimumVersion;
            LogitudeSettings.WorkEnvironment = setting.WorkEnvironment;
            LogitudeSettings.LogoCode = setting.LogoCode;
            LogitudeSettings.EnableHybridQueue = setting.EnableHybridQueue;
            LogitudeSettings.EmailAlertSignature = setting.EmailAlertSignature;
            LogitudeSettings.IOSAppLink = setting.IOSAppLink;
            LogitudeSettings.AndroidAppLink = setting.AndroidAppLink;
            LogitudeSettings.AndroidPodAppMinimumVersion = setting.AndroidPodAppMinimumVersion;
            LogitudeSettings.IOSPodAppMinimumVersion = setting.IOSPodAppMinimumVersion;
            LogitudeSettings.MinimumOutlookVersion = setting.MinimumOutlookVersion;
            LogitudeSettings.ABMProductId = setting.ABMProductId;
            LogitudeSettings.AzureFolderName = setting.AzureFolderName;
            LogitudeSettings.SignAppVersion = setting.SignAppVersion;
            LogitudeSettings.ReportsRunUsingWR = setting.ReportsRunUsingWR;
            LogitudeSettings.SMSServiceUserId = setting.SMSServiceUserId;
            LogitudeSettings.SMSServiceAuthToken = setting.SMSServiceAuthToken;
            LogitudeSettings.SMSServicePhoneNumber = setting.SMSServicePhoneNumber;
            LogitudeSettings.GLSHKEnv = setting.GLSHKEnv;
            LogitudeSettings.GLSHKURL = setting.GLSHKURL;
            LogitudeSettings.NotificationHubName = setting.NotificationHubName;
            LogitudeSettings.NotificationHubConnectionString = setting.NotificationHubConnectionString;
            LogitudeSettings.DomainName = setting.DomainName;
            LogitudeSettings.ProductName = setting.ProductName;
            LogitudeSettings.QueueServiceMode = setting.QueueServiceMode;
            LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;
            LogitudeSettings.DropboxAppKey = setting.DropboxAppKey;
            LogitudeSettings.DropboxAppSecret = setting.DropboxAppSecret;
            LogitudeSettings.OceanInsightsToken = setting.OceanInsightsToken;
            LogitudeSettings.CPUIntensiveWebServicesURL = setting.CPUIntensiveWebServicesURL;
        }

        #region Setup before creating tenant
        private void TenantEmailTextBox_TextChanged(object sender, EventArgs e)
        {
            this.TenantEmail = TenantEmailTextBox.Text;
        }
        private void TenantCompanyTextBox_TextChanged(object sender, EventArgs e)
        {
            this.TenantCompanyName = TenantCompanyTextBox.Text;
        }

        private ContactPM GetContactEmailOnly(string email)
        {
            ContactQuery contactQuery = new ContactQuery(this.Tenant);
            ContactPM contactPM = contactQuery.GetSingleByEmailWithoutTenant(email);
            return contactPM;
        }

        private void TenantEmailTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TenantEmailTextBox.Text))
            {
                this.TenantEmailValidation.Text = "Please Fill The Email!";
            }
            //else if (GetContactEmailOnly(TenantEmailTextBox.Text) != null)
            //{
            //    this.TenantEmailValidation.Text = "This Email Already Exist!";
            //}
            else if (!((TenantEmailTextBox.Text).Contains("@") && (TenantEmailTextBox.Text).Contains(".com")))
            {
                this.TenantEmailValidation.Text = "This Email Format is incorrect!";
            }
            else
            {
                this.TenantEmailValidation.Text = "";
            }
        }

        private void TenantCompanyTextBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TenantCompanyTextBox.Text))
            {
                this.TenantCompanyValidation.Text = "Please Fill The Company Name";
            }
            else
            {
                this.TenantCompanyValidation.Text = "";
            }
        }

        #endregion

        private void CreateTenant_Click(object sender, EventArgs e)
        {
            CreateSignup();
            CreateAgent();
            CreateAddress();
            CreateRatesTables();
            UpdateTenant();

            ResetPassword();

            CreateTranslationsInComputingPartners();
            UpdateQuoteSettings();
            UpdateAMANACTab();
            UpdateTrialStatus();
        }

        #region Create Tenant 

        #region SignUp 
        private void CreateSignup()
        {
            SignUpInfoClass signUpInfo = CreateSignUpInfoInstance();
            String Password = SignUpClass.StartSignUp(signUpInfo);
            this.Tenant = signUpInfo.Tenant;
            OldPassword.Text = Password;
        }

        private SignUpInfoClass CreateSignUpInfoInstance()
        {
            SignUpInfoClass signUpInfo = new SignUpInfoClass
            {
                Email = this.TenantEmail,
                Company = this.TenantCompanyName,
                Name = this.TenantCompanyName,
                Phone = "050808080",
                PackageCode = "DVMT",
                CountryCode = "PS",
                CountryName = "State Of Palestin"
            };
            return signUpInfo;
        }
        #endregion

        #region Agent
        private void CreateAgent()
        {
            AgentPM agentPM = CreateAgentInstance();
            string loggedContactId = GetContactIdByEmail(this.TenantEmail);
            ICommonDataContext MyContext = CommonDataContext.GetContext(agentPM.Tenant);
            AgentService service = new AgentService(MyContext, agentPM, loggedContactId);
            service.Create(agentPM);
            this.AgentId = agentPM.Id;
        }

        private AgentPM CreateAgentInstance()
        {
            AgentPM agentPM = new AgentPM
            {
                Code = "new",
                EnglishName = this.TenantCompanyName,
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
            return contactPM?.Id;
        }
        #endregion

        #region Address
        private void CreateAddress()
        {
            AddressPM addressPM = CreateAddressInstance();
            ICommonDataContext MyContext = CommonDataContext.GetContext(addressPM.Tenant);
            AddressService service = new AddressService(MyContext, addressPM.Tenant);
            service.Create(addressPM);
            this.AddressId = addressPM.Id;
        }

        private AddressPM CreateAddressInstance()
        {
            AddressPM addressPM = new AddressPM
            {
                Address1 = "address1",
                AddressTypeId = "M",
                CardId = this.AgentId,
                City = "Ramallah",
                CountryCode = "PS",
                CountryEnglishName = "State Of Palestine",
                CountryId = GetCountryId("PS"),
                Description = this.TenantCompanyName,
                Name = this.TenantCompanyName,
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
        #endregion

        #region Rates
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
                BaseCurrencyId = GetCurrencyIdFromTenant("USD"),
                ForeignCurrencyId = GetCurrencyIdFromTenant("NIS"),
                LogDateTime = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                Rate = 3.8,
                Tenant = this.Tenant
            };
            RatesPM.ValueDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
            return RatesPM;
        }

        private string GetCurrencyIdFromTenant(string code)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(this.Tenant);
            CurrencyPM currencyPM = currencyQuery.GetSingleCurrencyByCode(code, this.Tenant);
            if (currencyPM == null)
            {
                return CreateCurrencyInTenant(code);
            }
            return currencyPM.Id;
        }

        private string CreateCurrencyInTenant(string code)
        {
            CurrencyPM CurrencyInTenantZero = GetCurrencyFromTenantZero(code);
            CurrencyPM currencyPM = new CurrencyPM
            {
                Code = CurrencyInTenantZero.Code,
                EnglishName = CurrencyInTenantZero.EnglishName,
                AddedManually = CurrencyInTenantZero.AddedManually,
                InActive = CurrencyInTenantZero.InActive,
                LocalName = CurrencyInTenantZero.LocalName,
                SearchFields = CurrencyInTenantZero.SearchFields,
                Sign = CurrencyInTenantZero.Sign,
                Tenant = this.Tenant,
                IsHybrid = true
            };
            ICommonDataContext MyContext = CommonDataContext.GetContext(currencyPM.Tenant);
            CurrencyService service = new CurrencyService(MyContext, currencyPM.Tenant);
            service.Create(currencyPM);
            return currencyPM.Id;
        }

        private CurrencyPM GetCurrencyFromTenantZero(string code)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(this.Tenant);
            CurrencyPM currencyPM = currencyQuery.GetSingleCurrencyByCode(code, 0);
            return currencyPM;
        }
        #endregion

        private void UpdateTenant()
        {
            TenantQuery tenantQuery = new TenantQuery(this.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(this.Tenant);
            tenantPM.AgentId = this.AgentId;
            tenantPM.AddressId = this.AddressId;
            tenantPM.CurrencyId = GetCurrencyFromTenantZero("USD").Id;
            tenantPM.ProfitCurrencyId = GetCurrencyFromTenantZero("NIS").Id;
            tenantPM.ProfitCurrencyRate = 3.8;
            tenantPM.TimeZoneOffset = 3;
            tenantPM.FreightCurrencyId = GetCurrencyIdFromTenant("EUR");
            tenantPM.OtherChargesCurrencyId = GetCurrencyIdFromTenant("EUR");
            tenantPM.QuoteSaleCurrencyId = GetCurrencyIdFromTenant("EUR");
            tenantPM.CountryCode = "PS";
            tenantPM.CountryName = "State Of Palestine";
            tenantPM.PaymentTermId = GetPaymentTermIdByName("Net 30");
            tenantPM.PackageCode = "DVMT";


            //CommonDataDomainService domain = new CommonDataDomainService();
            //domain.UpdateTenantPM(tenantPM);
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenantPM.Id);
            TenantRepository tenantRepository = new TenantRepository(objectContext);
            Tenant entity = tenantRepository.GetSingleTenant(tenantPM.Id);
            TenantMapping.MapEntity(tenantPM, entity, false);
            tenantRepository.Update(entity);
            tenantRepository.SubmitChanges();

            //to make sure it's needed
            GlobalTenantRepository globalTenantRep = new GlobalTenantRepository();
            GlobalTenant gtenant = globalTenantRep.GetGlobalTenantsByTenant(tenantPM.Id);
            gtenant.CompanyName = tenantPM.Company;
            globalTenantRep.Update(gtenant);
            globalTenantRep.SubmitChanges();

            TenantManagementRepository tenantMngmentRep = new TenantManagementRepository();
            TenantManagement tenantMngment = tenantMngmentRep.GetSingleTenantManagement(tenantPM.Id);
            tenantMngment.Name = tenantPM.Company;
            tenantMngmentRep.Update(tenantMngment);
            tenantMngmentRep.SubmitChanges();
        }

        private string GetPaymentTermIdByName(string name)
        {
            PaymentTermQuery paymentTermQuery = new PaymentTermQuery(this.Tenant);
            IQueryable<PaymentTermPM> paymentTermQueryable = paymentTermQuery.GetPaymentTermsByCodeOrName(null, name, this.Tenant);
            return paymentTermQueryable.FirstOrDefault().Id;
        }
        #endregion

        private void ResetPassword()
        {
            string NewPassword = "!Cypress1";
            PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();
            bool succeeded = passwordChangeHelper.ChangePassword(this.TenantEmail, NewPassword);
            if (succeeded)
            {
                this.NewPasswordText.Text = NewPassword;
                Clipboard.SetText(NewPassword);
                this.ValidateCopy.Text = "New password was successfully copied to clipboard";
            }
        }

        #region Computing Partner Translation
        private void CreateTranslationsInComputingPartners()
        {
            ComputingPartnerTranslationPM entityPM = CreateComputingPartnerTranslationInstance();
            string loggedContactId = GetContactIdByEmail(this.TenantEmail);
            ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
            ComputingPartnerTranslationService service = new ComputingPartnerTranslationService(MyContext, entityPM.Tenant, loggedContactId);
            service.Create(entityPM);
        }

        private ComputingPartnerTranslationPM CreateComputingPartnerTranslationInstance()
        {
            ComputingPartnerTranslationPM entityPM = new ComputingPartnerTranslationPM
            {
                ComputingPartnerId = GetComputingPartnerIdByName("INTTRA"),
                ComputingPartnerName = "INTTRA",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CreatedByUserId = GetUserId(this.TenantEmail),
                ObjectTableId = GetObjectTableIdByName("PackageType"),
                ObjectTableName = "PackageType",
                OurCode = "40GP",
                PartnerCode = "CCCC",
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                UpdatedByUserId = GetUserId(this.TenantEmail),
                Tenant = this.Tenant
            };
            return entityPM;
        }

        private string GetUserId(string email)
        {
            UserQuery userQuery = new UserQuery(this.Tenant);
            UserPM userPM = userQuery.GetSinglePMByEmail(email, this.Tenant);
            return userPM.Id;
        }

        private string GetComputingPartnerIdByName(string name)
        {
            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(this.Tenant);
            ComputingPartnerPM computingPartnerPM = computingPartnerQuery.GetSinglePMByName(name, 0);
            return computingPartnerPM.Id;
        }

        private string GetObjectTableIdByName(string name)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(this.Tenant);
            ObjectTablePM objectTablePM = objectTableQuery.GetObjectTableByName(name, 0);
            return objectTablePM.Id;
        }
        #endregion

        #region Quote Domain
        private void UpdateQuoteSettings()
        {
            QuoteSettingPM entityPM = CreateQuoteDomainInstnace();
            IQuotesContext objectContext = QuotesContext.GetContext(entityPM.Tenant);
            QuoteSettingService myService = new QuoteSettingService(objectContext, entityPM.Tenant);
            QuoteSettingRepository myRepository = new QuoteSettingRepository(objectContext);
            QuoteSetting myPOCO = myRepository.GetSingleQuoteSetting(this.Tenant);
            if (myPOCO == null)
            {
                myService.Create(entityPM);
            }
            else if (entityPM.Id == null)
            {
                myService.Create(entityPM);
            }
            else
            {
                myService.Update(entityPM);
            }
        }

        private QuoteSettingPM CreateQuoteDomainInstnace()
        {
            QuoteSettingPM entityPM = new QuoteSettingPM
            {
                CopyShipper = true,
                CopyConsignee = false,
                CopyMainCarriage = true,
                CopyPickup = false,
                CopyDelivery = false,
                CopyChargesTypes = false,
                CopyChargesCost = false,
                CopyChargesSale = false,
                EditMainCarriage = false,
                CopyAgent = false,
                CopyNotify = false,
                IsSaleAsCostCurrency = false,
                CopyExchangeRates = false,
                AutomaticallyCloseDays = 30,
                IsMultiCurrency = false,
                QuoteExpirationDays = 30,
                Tenant = this.Tenant
            };
            return entityPM;
        }
        #endregion

        #region AMANAC Tab
        private void UpdateAMANACTab()
        {
            string loggedContactId = GetContactIdByEmail(this.TenantEmail);
            CustomsInterfaceSettingPM customsInterfaceSettingPM = CreateCustomsInterfaceSettingsInstance();
            ICommonDataContext MyContext = CommonDataContext.GetContext(customsInterfaceSettingPM.Tenant);
            CustomsInterfaceSettingService service = new CustomsInterfaceSettingService(MyContext, customsInterfaceSettingPM.Tenant, loggedContactId);
            service.Update(customsInterfaceSettingPM);
        }

        private CustomsInterfaceSettingPM CreateCustomsInterfaceSettingsInstance()
        {
            CustomsInterfaceSettingPM customsInterfaceSettingPM = new CustomsInterfaceSettingPM
            {
                ActivateCustomsManagementInShipments = true,
                LocalCustomsInterfaceCode = "AMC",
                ExportFromUSAInterfaceCode = "NO",
                ImportToUSAInterfaceCode = "NO",
                AMCAirStartDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                AMCOceanStartDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                Tenant = this.Tenant
            };
            return customsInterfaceSettingPM;
        }
        #endregion

        #region Trial
        private void UpdateTrialStatus()
        {
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(this.Tenant);
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(this.Tenant);
            tenantManagementPM.IsTrial = false;

            IGlobalContext objectContext = GlobalContext.GetContext();
            TenantManagementRepository entityRepository = new TenantManagementRepository(objectContext);
            TenantManagement entityPoco = entityRepository.GetSingleTenantManagement(tenantManagementPM.Id);

            TenantManagementMapping.MapEntity(tenantManagementPM, entityPoco, false);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }
        #endregion
    }
}

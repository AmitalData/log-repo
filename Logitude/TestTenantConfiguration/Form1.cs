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
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Logitude.CRM.BL.EntityPMs;
using System.Collections.Generic;
using Logitude.CRM.Data;
using Logitude.Server.Tools.Counters;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using System.Data.Entity.Validation;
using Logitude.CRM.Data.EntityKeys;
using Logitude.BL.Helpers;
using WebFreight.Web.Helpers.APIHelpers;

namespace TestTenantConfiguration
{
    public partial class Form1 : Form
    {
        private int Tenant;
        private string AgentId, AddressId, EmployeeId, ContactID;
        private string TenantEmail, TenantCompanyName, NewPassword , Title;
        private bool ValidateEmail = false, ValidateCompany = false;

        public Form1()
        {
            CacheManager.CacheWrapper = new MockCacheWrapper();
            FillAppSettings();
            ContainerAccessor.InitContainer();
            InitializeComponent();
            GetContactEmailOnly("");
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
			LogitudeSettings.WindWardSettings = setting.WindWardSettings;

		}

		#region Setup before creating tenant
		private void TenantEmailTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TenantEmailTextBox.Text != null)
            {
                this.TenantEmailValidation.Text = "";
            }
            this.TenantEmail = TenantEmailTextBox.Text;
        }
        private void TenantCompanyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TenantCompanyTextBox.Text != null)
            {
                this.TenantCompanyValidation.Text = "";
            }
            this.TenantCompanyName = TenantCompanyTextBox.Text;
        }

        private void TenantEmailTextBox_LostFocus(object sender, EventArgs e)
        {
            ValidateEmailTB();
        }

        private void ValidateEmailTB()
        {
            ValidateEmail = false;
            if (string.IsNullOrEmpty(TenantEmailTextBox.Text))
            {
                this.TenantEmailValidation.Text = "Please Fill The Email!";
            }
            else if (GetContactEmailOnly(TenantEmailTextBox.Text) != null)
            {
                this.TenantEmailValidation.Text = "This Email Already Exist!";
            }
            else if (!((TenantEmailTextBox.Text).Contains("@") && (TenantEmailTextBox.Text).Contains(".com")))
            {
                this.TenantEmailValidation.Text = "This Email Format is incorrect!";
            }
            else
            {
                this.TenantEmailValidation.Text = "";
                ValidateEmail = true;
            }
        }

        private void TenantCompanyTextBox_LostFocus(object sender, EventArgs e)
        {
            ValidateCompanyTB();
        }

        private void ValidateCompanyTB()
        {
            ValidateCompany = false;
            if (string.IsNullOrEmpty(TenantCompanyTextBox.Text))
            {
                this.TenantCompanyValidation.Text = "Please Fill The Company Name";
            }
            else
            {
                this.TenantCompanyValidation.Text = "";
                ValidateCompany = true;
            }
        }

        private ContactPM GetContactEmailOnly(string email)
        {
            ContactQuery contactQuery = new ContactQuery(this.Tenant);
            ContactPM contactPM = contactQuery.GetSingleByEmailWithoutTenant(email);
            return contactPM;
        }
        #endregion

        private void CreateTenantBtn_Click(object sender, EventArgs e)
        {
            if (ValidateEmail && ValidateCompany)
            {
                Thread thread = new Thread(() => StartCreateTenant());
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                ValidateEmailTB();
                ValidateCompanyTB();
            }
        }

        Stopwatch globalStopwatch;
        Label generalLabel;
        private void StartCreateTenant()
        {
            SetControlPropertyValue(Timerlbl, "Text", "Creating Tenant...");
            SetControlPropertyValue(Timerlbl, "ForeColor", Color.Black);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            globalStopwatch = stopWatch;
            generalLabel = this.Timerlbl;
            timer1.Enabled = true;
            timer1.Start();

            CreateTenantMethods();

            // for timer
            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            SetControlPropertyValue(Timerlbl, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue(Timerlbl, "ForeColor", Color.Green);
            SetControlPropertyValue(Timerlbl, "Text", "Done in " + ts.ToString(@"hh\:mm\:ss"));
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (globalStopwatch != null && generalLabel != null)
            {
                TimeSpan elapsedTime = globalStopwatch.Elapsed;
            }
        }

        private void CreateTenantMethods()
        {
            try
            {
                CreateTenantConfiguration();

                ////Maintenance settings
                ComputingPartnersPrepare();
                UpdateQuoteSettings();
                UpdateAMANACTab();
                UpdateTrialStatus();
                TicketPrepareData();

                ////Prepare data location , partners , shipment
                PrepareDataForTenant();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg = msg + ex.InnerException.Message;
                    msg = msg +"inner Exception : "+ ex.InnerException.InnerException;
                }
                MessageBox.Show(msg, this.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Create Tenant Configuration
        private ICommonDataContext MyContext;

        private void CreateTenantConfiguration()
        {
            Signup();
            TenantConstructor();
            CreateAgent();
            CreateAddress();
            CreateRatesTables();
            UpdateTenant();
            AcceptTerms();
            displayGettingStarted();
            ResetPassword();
        }

        private void TenantConstructor()
        {
            MyContext = CommonDataContext.GetContext(this.Tenant);
            this.ContactID = GetContactIdByEmail(this.TenantEmail, this.Tenant);
        }

        private string GetContactIdByEmail(string email, int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(this.Tenant);
            ContactPM contactPM = contactQuery.GetSingleContact(email, tenant);
            return contactPM?.Id;
        }

        #region SignUp 
        private void Signup()
        {
            this.Title = "SignUp";
            SignUpInfoClass signUpInfo = CreateSignUpInfoInstance();
            String Password = SignUpClass.StartSignUp(signUpInfo);
            this.Tenant = signUpInfo.Tenant;
            SetControlPropertyValue(TenantNumber, "Text", this.Tenant.ToString());
        }

        private SignUpInfoClass CreateSignUpInfoInstance()
        {
            SignUpInfoClass signUpInfo = new SignUpInfoClass
            {
                Email = this.TenantEmail,
                Company = this.TenantCompanyName,
                Name = "SpecflowTest",
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
            this.Title = "Agent";
            AgentPM agentPM = CreateAgentInstance();
            AgentService service = new AgentService(MyContext, agentPM, ContactID);
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
        #endregion

        #region Address
        private void CreateAddress()
        {
            this.Title = "Address";
            AddressPM addressPM = CreateAddressInstance();
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
            this.Title = "rate";
            RatesTablePM RatesPM = CreateRatesTablesInstance("USD", "NIS", TenantServerConfigration.GetCurrentDateTime(this.Tenant), 3.8);
            RatesTablePM RatesPM2 = CreateRatesTablesInstance("USD", "EUR", DateHelper.GetDate("2019:6:24:0:0:0"), 4);
            RatesTablePM RatesPM3 = CreateRatesTablesInstance("USD", "EUR", TenantServerConfigration.GetCurrentDateTime(this.Tenant), 3.8);
            IWebFreightContext MyContext = WebFreightContext.GetContext(RatesPM.Tenant);
            RatesTableService service = new RatesTableService(MyContext, RatesPM.Tenant);
            service.Create(RatesPM);
            service.Create(RatesPM2);
            service.Create(RatesPM3);
        }

        private RatesTablePM CreateRatesTablesInstance(string baseCurrency, string foreignCurrency, DateTime? date, double rate)
        {
            RatesTablePM RatesPM = new RatesTablePM
            {
                BaseCurrencyId = GetCurrencyIdFromTenant(baseCurrency),
                ForeignCurrencyId = GetCurrencyIdFromTenant(foreignCurrency),
                LogDateTime = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                Rate = rate,
                Tenant = this.Tenant
            };
            RatesPM.ValueDate = date;
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

        ICommonDataContext objectContext;
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
            CurrencyService service = new CurrencyService(MyContext, currencyPM.Tenant);
            service.Create(currencyPM);
            //objectContext = CommonDataContext.GetContext(this.Tenant);
            TableLastUpdateClass.UpdateTableHistory(currencyPM.Tenant, "Currency");
            MyContext.SaveChanges();

            return currencyPM.Id;
        }

        private CurrencyPM GetCurrencyFromTenantZero(string code)
        {
            CurrencyQuery currencyQuery = new CurrencyQuery(this.Tenant);
            CurrencyPM currencyPM = currencyQuery.GetSingleCurrencyByCode(code, 0);
            return currencyPM;
        }
        #endregion

        #region Update Tenant
        private void UpdateTenant()
        {
            this.Title = "update tenant";
            TenantQuery tenantQuery = new TenantQuery(this.Tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(this.Tenant);
            tenantPM.AgentId = this.AgentId;
            tenantPM.AddressId = this.AddressId;
            tenantPM.CurrencyId = GetCurrencyIdFromTenant("USD");
            tenantPM.ProfitCurrencyId = GetCurrencyIdFromTenant("NIS");
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

        #region Accept Terms
        private void AcceptTerms()
        {
            this.Title = "accept terms";
            TermsofUseSignaturePM MyTenant = CreateTermInstance(this.ContactID);
            TermsofUseSignaturePM CustomerCareTenant = CreateTermInstance(GetContactIdByEmail("ahmada@logitudeworld.com", 0));
            TermsofUseSignatureService service = new TermsofUseSignatureService(MyContext, MyTenant.Tenant);
            service.Create(MyTenant);
            service.Create(CustomerCareTenant);
        }

        private TermsofUseSignaturePM CreateTermInstance(string ContactId)
        {
            TermsofUseSignaturePM entityPM = new TermsofUseSignaturePM()
            {
                ContactId = ContactId,
                SignedDatetime = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                Tenant = this.Tenant,
                TermsofUseId = LastTermOfUseLastVersion()
            };
            return entityPM;
        }

        private int LastTermOfUseLastVersion()
        {
            TermsofUseQuery termsofUseQuery = new TermsofUseQuery(this.Tenant);
            TermsofUsePM termsofUsePM = termsofUseQuery.GetTermsofUseDefault(false);
            return termsofUsePM.VersionNumber;
        }
        #endregion

        #region Getting started 
        private void displayGettingStarted()
        {
            this.Title = "Display getting started";
            ContactQuery query = new ContactQuery(this.Tenant);
            ContactPM contact = query.GetSingleContact(this.TenantEmail, this.Tenant);
            contact.DisplayGettingStarted = false;
            contact.IsHybrid = true;
            contact.DontShowLocalLabels = true; //language
            ICommonDataContext MyContext = CommonDataContext.GetContext(this.Tenant);
            ContactService service = new ContactService(MyContext, this.Tenant);
            service.Update(contact);
        }
        #endregion
        
        #region Reset Password
        private void ResetPassword()
        {
            this.Title = "Reset password";
            this.NewPassword = "!Cypress1";
            PasswordChangeHelper passwordChangeHelper = new PasswordChangeHelper();
            bool succeeded = passwordChangeHelper.ChangePassword(this.TenantEmail, this.NewPassword);
            if (succeeded)
            {
                SetControlPropertyValue(NewPasswordText, "Text", this.NewPassword);
                //Clipboard.SetText(NewPassword);
                SetControlPropertyValue(ValidateCopy, "Text", "New password was successfully changed");
            }
        }
        #endregion
        
        #endregion

        #region Computing Partner Translation

        private void ComputingPartnersPrepare()
        {
            this.Title = "Computing partners";
            CreateNewTableForTranslation();
            CreateTranslationsInComputingPartners();
        }
        private void CreateTranslationsInComputingPartners()
        {
            ComputingPartnerTranslationPM translationFor40GP = CreateComputingPartnerTranslationInstance("PackageType", "40GP", "CCCC");
            ComputingPartnerTranslationPM translationForCT = CreateComputingPartnerTranslationInstance("PackageType", "CT", "CT");
            ComputingPartnerTranslationPM translationForPTP = CreateComputingPartnerTranslationInstance("MoveType", "PTP", "PortToPort");
            ComputingPartnerTranslationService service = new ComputingPartnerTranslationService(MyContext, this.Tenant, this.ContactID);
            service.Create(translationFor40GP);
            service.Create(translationForCT);
            service.Create(translationForPTP);
        }

        private void CreateNewTableForTranslation()
        {
            ComputingPartnerPM entityPM = CreatenewComputingPartnerTableInstance();
            ICommonDataContext MyContext = CommonDataContext.GetContext(this.Tenant);
            ComputingPartnerService service = new ComputingPartnerService(MyContext, entityPM, this.ContactID);
            service.Update(entityPM.PartnerTables);
        }

        private ComputingPartnerPM CreatenewComputingPartnerTableInstance()
        {
            List<ComputingPartnerTablePM> PartnerTablesList = new List<ComputingPartnerTablePM>();
            ComputingPartnerTablePM computingPartnerTable = new ComputingPartnerTablePM()
            {
                ChangeSetOp = ChangeSetOperation.None,
                ComputingPartnerId = GetComputingPartnerByName("INTTRA").Id,
                ComputingPartnerName = "INTTRA",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CreatedByUserId = GetComputingPartnerByName("INTTRA").CreatedByUserId,
                //CreatedByUserName = "Lana" , 
                HasPartnerList = false,
                MustUsePartnerList = false,
                Name = "Package type",
                ObjectTableId = GetObjectTableIdByName("PackageType"),
                ObjectTableName = "PackageType",
                Tenant = 0,
                TenantLevelTranslationBlocked = false,
                TransalationRequired = false,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                UpdatedByUserId = GetComputingPartnerByName("INTTRA").CreatedByUserId,
                //UpdatedByUserName = "Lana",
            };
            ComputingPartnerTablePM computingPartnerTable2 = new ComputingPartnerTablePM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                ComputingPartnerId = GetComputingPartnerByName("INTTRA").Id,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CreatedByUserId = GetUserId(this.TenantEmail),
                Name = "MoveType",
                ObjectTableId = GetObjectTableIdByName("MoveType"),
                ObjectTableName = "MoveType",
                Tenant = this.Tenant,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                UpdatedByUserId = GetUserId(this.TenantEmail),
                //UpdatedByUserName = "TeamR3",
            };
            PartnerTablesList.Add(computingPartnerTable);
            PartnerTablesList.Add(computingPartnerTable2);

            ComputingPartnerPM entityPM = new ComputingPartnerPM()
            {
                Code = "G-INTTRA",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CreatedByUserId = GetComputingPartnerByName("INTTRA").CreatedByUserId,
                //CreatedByUserName = ,
                Id = GetComputingPartnerByName("INTTRA").Id,
                Name = "INTTRA",
                SearchFields = "INTTRA",
                Tenant = 0,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                UpdatedByUserId = GetUserId(this.TenantEmail),
                InActive = false,
                LoggedTenantId = this.Tenant,
                PartnerTables = PartnerTablesList
            };
            return entityPM;
        }

        private ComputingPartnerTranslationPM CreateComputingPartnerTranslationInstance(string objectTable, string ourCode, string partnerCode)
        {
            ComputingPartnerTranslationPM entityPM = new ComputingPartnerTranslationPM
            {
                ComputingPartnerId = GetComputingPartnerByName("INTTRA").Id,
                ComputingPartnerName = "INTTRA",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CreatedByUserId = GetUserId(this.TenantEmail),
                ObjectTableId = GetObjectTableIdByName(objectTable),
                ObjectTableName = objectTable,
                OurCode = ourCode,
                PartnerCode = partnerCode,
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

        private ComputingPartnerPM GetComputingPartnerByName(string name)
        {
            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(this.Tenant);
            ComputingPartnerPM computingPartnerPM = computingPartnerQuery.GetSinglePMByName(name, 0);
            return computingPartnerPM;
        }

        private string GetObjectTableIdByName(string name)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(this.Tenant);
            ObjectTablePM objectTablePM = objectTableQuery.GetObjectTableByName(name, 0);
            return objectTablePM.Id;
        }
        #endregion

        #region Quote Settings
        private void UpdateQuoteSettings()
        {
            this.Title = "Quote Settings";
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
            this.Title = "AMANAC tab";
            CustomsInterfaceSettingPM customsInterfaceSettingPM = CreateCustomsInterfaceSettingsInstance();
            CustomsInterfaceSettingService service = new CustomsInterfaceSettingService(MyContext, customsInterfaceSettingPM.Tenant, this.ContactID);
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
            this.Title = "Trial";
            TenantManagementQuery tenantManagementQuery = new TenantManagementQuery(this.Tenant);
            TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(this.Tenant);
            tenantManagementPM.IsTrial = false;
            tenantManagementPM.SupportActivated = true;
            tenantManagementPM.SupportDomain = this.TenantEmail;

            IGlobalContext objectContext = GlobalContext.GetContext();
            TenantManagementRepository entityRepository = new TenantManagementRepository(objectContext);
            TenantManagement entityPoco = entityRepository.GetSingleTenantManagement(tenantManagementPM.Id);

            TenantManagementMapping.MapEntity(tenantManagementPM, entityPoco, false);
            entityRepository.Update(entityPoco);
            entityRepository.SubmitChanges();
        }
        #endregion

        #region Ticket
        IMapping<EmployeeGroupPM, EmployeeGroup> employeeGroupMapping;
        IRepository<EmployeeGroup> employeeGroupRepository;
        IMapping<EmployeeGroupLinePM, EmployeeGroupLine> employeeGroupLineMapping;
        IRepository<EmployeeGroupLine> employeeGroupLineRepository;
        IMapping<TicketClassificationPM, TicketClassification> ticketClassificationMapping;
        IRepository<TicketClassification> ticketClassificationRepository;
        private ICRMContext MainContext;
        private Dictionary<string, IContext> additionalContexts;

        private void TicketPrepareData()
        {
            this.Title = "Ticket";
            TicketConstructor();
            CreateEmployeeGroup();
            CreateClassifiactionInstance();
        }

        private void TicketConstructor()
        {
            ICRMContext MyContext = CRMContext.GetContext(this.Tenant);
            this.MainContext = MyContext as CRMContext;
            additionalContexts = new Dictionary<string, IContext>();
            employeeGroupMapping = new EmployeeGroupDataMapping();
            employeeGroupRepository = new EmployeeGroupRepository(this.MainContext);
            employeeGroupLineMapping = new EmployeeGroupLineDataMapping();
            employeeGroupLineRepository = new EmployeeGroupLineRepository(this.MainContext);
            ticketClassificationMapping = new TicketClassificationDataMapping();
            ticketClassificationRepository = new TicketClassificationRepository(this.MainContext);
        }

        private void CreateEmployeeGroup()
        {
            EmployeeGroupLinePM employeeGroupLine = new EmployeeGroupLinePM()
            {
                Tenant = this.Tenant,
                UserId = this.ContactID,
                IsDefaultOwner = true,
                ChangeSetOp = ChangeSetOperation.Insert
            };
            List<EmployeeGroupLinePM> employeeGroupLineList = new List<EmployeeGroupLinePM>();
            employeeGroupLineList.Add(employeeGroupLine);
            EmployeeGroupPM employeeGroupPM = new EmployeeGroupPM()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                CreatedByUserId = this.ContactID,
                ManagerUserId = this.ContactID,
                Name = "Support Group",
                Tenant = this.Tenant,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant),
                UpdatedByUserId = this.ContactID,
                EmployeeGroupLines = employeeGroupLineList
            };

            employeeGroupPM.Id = this.EmployeeId = IdCounter.GetNumber("EmployeeGroup", employeeGroupPM.Tenant);
            employeeGroupLine.Id = IdCounter.GetNumber("EmployeeGroupLine", employeeGroupLine.Tenant);
            employeeGroupPM.EmployeeGroupLines.First().EmployeeGroupId = employeeGroupPM.Id;


            EmployeeGroup employeeGroupPoco = new EmployeeGroup();
            employeeGroupMapping.CustomPMToPOCO(employeeGroupPM, employeeGroupPoco);
            employeeGroupMapping.PMToPOCO(employeeGroupPM, employeeGroupPoco);
            employeeGroupRepository.Add(employeeGroupPoco);

            EmployeeGroupLine employeeGroupLinePOCO = new EmployeeGroupLine();
            employeeGroupLineMapping.CustomPMToPOCO(employeeGroupLine, employeeGroupLinePOCO);
            employeeGroupLineMapping.PMToPOCO(employeeGroupLine, employeeGroupLinePOCO);
            employeeGroupLineRepository.Add(employeeGroupLinePOCO);

            SubmitChanges();
        }

        private void CreateClassifiactionInstance()
        {
            TicketClassificationPM entityPM = new TicketClassificationPM()
            {
                Tenant = this.Tenant,
                Id = this.Tenant.ToString(),
                Name = "test",
                SearchFields = "test",
                Inactive = false,
                EmployeeGroupId = this.EmployeeId,
                ManagerUserId = this.ContactID,
                EscalationNotify = this.TenantEmail,
                DefaultSeverityId = GetDefaultSeverityId("Medium")
            };

            EntityKeyFields entityKeys = GetKeys(entityPM);
            TicketClassification EntityPOCO = ticketClassificationRepository.GetSingle(entityKeys);
            TicketClassificationPM OldEntityPM = new TicketClassificationPM();
            TicketClassificationPM ChangeTrackingEntityPM = new TicketClassificationPM();
            ticketClassificationMapping.POCOToPM(OldEntityPM, EntityPOCO);
            ticketClassificationMapping.POCOToPM(ChangeTrackingEntityPM, EntityPOCO);
            ticketClassificationMapping.PMToOldPM(entityPM, ChangeTrackingEntityPM);

            ticketClassificationMapping.CustomPMToPOCO(entityPM, EntityPOCO);
            ticketClassificationMapping.PMToPOCO(entityPM, EntityPOCO);
            ticketClassificationRepository.Update(EntityPOCO);
            SubmitChanges();
        }

        protected virtual void SubmitChanges()
        {
            try
            {
                MainContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
            foreach (IContext context in additionalContexts.Values)
            {
                context.SaveChanges();
            }

        }

        private EntityKeyFields GetKeys(TicketClassificationPM entityPM)
        {
            TicketClassificationKeys entityKeys = new TicketClassificationKeys() { Id = entityPM.Id };
            return entityKeys;
        }

        private string GetDefaultSeverityId(string name)
        {
            TicketSeverityRepository ticketSeverityRepository = new TicketSeverityRepository(this.Tenant);
            TicketSeverity ticketSeverity = ticketSeverityRepository.GetSingleByName(name, this.Tenant);
            return ticketSeverity.Id;
        }
        #endregion

        private void PrepareDataForTenant()
        {
            this.Title = "Prepare Data";
            SetControlPropertyValue(Timerlbl, "Text", "Preparing Tenant Data ...");
            SetControlPropertyValue(Timerlbl, "ForeColor", Color.DodgerBlue);

            string LogitudeURL = System.Configuration.ConfigurationSettings.AppSettings.Get("LogitudeURL");
            Logitude.Base.Hooks.BeforeTestRun.PrepareTheData(this.TenantEmail, this.NewPassword, LogitudeURL);
            Logitude.ShipmentTests.Hooks.BeforeTestRun.SetupShipmentPreparationVariables();

            SetControlPropertyValue(ValidatePrepareData, "Text", "The Tenant is ready with the prepared data");
            SetControlPropertyValue(ValidatePrepareData, "ForeColor", Color.Green);
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
    }
}

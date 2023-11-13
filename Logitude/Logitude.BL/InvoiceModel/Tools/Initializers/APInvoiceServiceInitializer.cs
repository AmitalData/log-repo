using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.Behaviours;
using Logitude.BL.InvoiceModel.Tools.Behaviours.APInvoiceBehaviours;
using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.InvoiceModel.Tools.Initializers
{
    public class APInvoiceServiceInitializer : IServiceInitializer
    {
        public int Tenant { get; private set; }
        public bool IsNewEntity { get; private set; }
        public APInvoice EntityPOCO { get; private set; }
        public APInvoicePM EntityPM { get; private set; }
        public IInvoiceContext Context { get; private set; }
        public ICommonDataContext CommonContext { get; private set; }
        public APInvoiceRepository Repository { get; private set; }
        public Tenant LoggedTenant { get; private set; }
        public AccountingSystem AccountingSystem { get; private set; }
        public AccountingSetting AccountingSetting { get; private set; }
        public string LoggedContactId { get; private set; }
        public DateTime? TodayDate { get; private set; }
        public DateTime? TodayDateTime { get; private set; }

        public List<APInvoiceLinePM> ActiveLines { get; private set; }
        public List<VatType> AllVatTypes { get; private set; }
        public List<VatTypePercentagePM> AllVatPercentages { get; private set; }

        public APInvoiceServiceInitializerFlags Flags { get; private set; }

        public APInvoiceServiceInitializer(IInvoiceContext objectContext, APInvoicePM entityPM)
        {
            this.EntityPM = entityPM;
            this.Tenant = entityPM.Tenant;
            this.IsNewEntity = entityPM.Id == null ? true : false;
            this.Context = objectContext;
            this.CommonContext = CommonDataContext.GetContext(Tenant);
            this.Repository = new APInvoiceRepository(Context);
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            this.TodayDate = this.TodayDateTime.Value.Date;            
        }

        public void Initialize()
        {
            InitializeLoggedTenant();
            InitializeLoggedContact();
            InitializeAccountingSetting();
            InitializeAccountingSystem();

            InitializeEntity();
            InitializeFlags();
            InitializeVATs();
            InitializeActiveLines();
        }

        public void HandleBehaviours()
        {
            List<IServiceBehaviour> serviceBehaviours = new List<IServiceBehaviour>();

            serviceBehaviours.Add(new APInvoiceFieldsBehaviour());
            serviceBehaviours.Add(new APInvoiceAmountDueBehaviour());

            serviceBehaviours.Add(new APInvoiceLinesBehavior());
            serviceBehaviours.Add(new APInvoiceTotalVatsBehavior());

            foreach (IServiceBehaviour behaviour in serviceBehaviours)
            {
                behaviour.Handle(this);
            }
        }

        private void InitializeLoggedTenant()
        {
            LoggedTenant = TenantRepository.GetSingleTenant(Tenant, true);
        }
        private void InitializeLoggedContact()
        {
            if (EntityPM.CreatedFromAPI)
            {
                this.LoggedContactId = EntityPM.CreatedByUserId;
            }

            else
            {
                ContactRepository contactRepository = new ContactRepository(CommonContext);
                string email = "";
                if (SecurityUtility.IsWorkerRoleCall) {
                    email = SecurityUtility.GetAuthenticatedUser();
                } else {
                    email = HttpContext.Current.User.Identity.Name;
                }

                if (email != null)
                {
                    Contact loggedContact = contactRepository.GetSingleContactByEmail(email, Tenant);
                    this.LoggedContactId = loggedContact.Id;
                }

                else
                {
                    ContactPM loggedContact = new ContactQuery(contactRepository).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), Tenant, true);
                    this.LoggedContactId = loggedContact.Id;
                }
            }
        }
        private void InitializeAccountingSetting()
        {
            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(CommonContext);
            AccountingSetting = accountingSettingRepository.GetSingleAccountSetting(Tenant);            
        }
        private void InitializeAccountingSystem()
        {
            if (AccountingSetting != null)
            {
                AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(CommonContext);
                AccountingSystem = accountingSystemRepository.GetSingleAccountingSystem(AccountingSetting.AccountingSystemCode);
            }
        }
        private void InitializeEntity()
        {
            if (this.IsNewEntity)
            {
                EntityPM.Id = IdCounter.GetNumber("APInvoice", Tenant).ToString();

                EntityPOCO = new APInvoice()
                {
                    Id = EntityPM.Id,
                };
            }

            else
            {
                EntityPOCO = Repository.GetSingleAPInvoice(EntityPM.Id, Tenant);
            }
        }
        private void InitializeFlags()
        {
            this.Flags = new APInvoiceServiceInitializerFlags(this);
        }
        private void InitializeVATs()
        {
            VatTypeRepository vatTypeRepository = new VatTypeRepository(CommonContext);
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(CommonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);

            AllVatTypes = vatTypeRepository.GetVatTypes(Tenant).ToList();
            AllVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(Tenant, TodayDate);
        }
        private void InitializeActiveLines()
        {
            if (IsNewEntity)
            {
                ActiveLines = EntityPM.InvoiceLines.ToList();
            }
            else
            {
                ActiveLines = EntityPM.InvoiceLines.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
            }
        }
    }

    public class APInvoiceServiceInitializerFlags
    {
        public bool IsJournal { get; private set; }
        public bool IsExternal { get; private set; }
        public bool IsTaxItemManaged { get; private set; }
        public bool CanTransferToFTP { get; private set; }
        public bool CanTransferToDropbox { get; private set; }
        public bool TransferToFTPActivated { get; private set; }
        public bool TransferToDropboxActivated { get; private set; }
        public bool IsTransferEnabled { get; private set; }
        public bool IsSetVoided { get; private set; }
        public bool IsSetApproved { get; private set; }
        public bool IsAlreadyVoided { get; private set; }
        public bool IsInvoiceLinesChanged { get; private set; }
        public bool IsInvoiceTotalVATsChanged { get; private set; }

        private APInvoiceServiceInitializer initializer;
        public APInvoiceServiceInitializerFlags(APInvoiceServiceInitializer initializer)
        {
            this.initializer = initializer;

            this.SetEntityFlags();
            this.SetAccountingFlags();
        }

        private void SetEntityFlags()
        {
            IsSetVoided = initializer.EntityPM.SetVoided;
            IsSetApproved = initializer.EntityPM.SetApproved;
            IsAlreadyVoided = initializer.EntityPOCO.StatusCode == "VD" ? true : false;
            InitializeFlag_IsInvoiceLinesChanged();
            InitializeFlag_IsInvoiceTotalVATsChanged();
        }

        private void SetAccountingFlags()
        {
            if (initializer.AccountingSetting != null)
            {
                this.TransferToFTPActivated = initializer.AccountingSetting.TransferToFTPActivated;
                this.TransferToDropboxActivated = initializer.AccountingSetting.TransferToDropboxActivated;

                if (initializer.AccountingSystem != null)
                {
                    this.IsJournal = initializer.AccountingSystem.IsJournalMode;
                    this.IsExternal = initializer.AccountingSystem.IsExternalCodesFromTable;
                    this.IsTaxItemManaged = initializer.AccountingSystem.IsTaxItemManaged;
                    this.CanTransferToFTP = initializer.AccountingSystem.CanTransferToFTP;
                    this.CanTransferToDropbox = initializer.AccountingSystem.CanTransferToDropbox;

                    if (initializer.AccountingSetting.IsAPInvoicesTransferEnabled && initializer.AccountingSystem.AllowAPInvoicesTransfer)
                    {
                        this.IsTransferEnabled = true;
                    }
                }
            }
        }

        private void InitializeFlag_IsInvoiceLinesChanged()
        {
            if (initializer.IsNewEntity)
            {
                IsInvoiceLinesChanged = true;
            }

            else
            {
                if (initializer.EntityPM.InvoiceLines.Where
                    (
                    d =>                        
                    d.ChangeSetOp == ChangeSetOperation.Insert                
                    ||                
                    d.ChangeSetOp == ChangeSetOperation.Update
                    ||
                    d.ChangeSetOp == ChangeSetOperation.Delete
                    ).Any())                
                {
                    IsInvoiceLinesChanged = true;
                }
            }
        }
        private void InitializeFlag_IsInvoiceTotalVATsChanged()
        {
            if (initializer.IsNewEntity)
            {
                IsInvoiceTotalVATsChanged = true;
            }

            else
            {
                if (initializer.EntityPM.TotalVATs.Where
                    (
                    d => 
                    d.ChangeSetOp == ChangeSetOperation.Insert
                    ||
                    d.ChangeSetOp == ChangeSetOperation.Update
                    ||
                    d.ChangeSetOp == ChangeSetOperation.Delete
                    ).Any())
                {
                    IsInvoiceTotalVATsChanged = true;
                }
            }
        }
    }
}

using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CardService
    {
        bool isNewEntity;
        private int tenant;
        public Card Poco { get; set; }
        private Contact loggedContact;
        private AddressRepository addressRepository;
        private CardPM entityPM;
        private ICommonDataContext objectContext;
        private CardRepository entityRepository;
        private ContactRepository contactRepository;
        public CardService(ICommonDataContext objectContext, CardPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.GetLoggedContact();
        }
        public CardService(ICommonDataContext objectContext,int tenant)
        {

            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CardRepository(objectContext);
            this.addressRepository = new AddressRepository(objectContext);
            this.contactRepository = new ContactRepository(objectContext);
            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null)
            {
                string email = HttpContext.Current.User.Identity.Name;
                this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);

            }
        }

        public void Create(CardPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.Create();
            RunStoredProcedures();

        }
        public void Create()
        {
            this.isNewEntity = true;
            
            if (string.IsNullOrEmpty(entityPM.Id))
            {
                this.entityPM.Id = IdCounter.GetNumber("Card", tenant).ToString();
            }

            entityPM.Code = CodeCounter.GetNumber("Card", entityPM.Tenant).ToString();

            this.Initialize();

            this.Poco = new Card();
            this.Poco.Id = this.entityPM.Id;
            this.Poco.Code = entityPM.Code;

            CardValidating.Validate(entityPM);
            CardTracing.Trace(entityPM, Poco, isNewEntity);
            CardMapping.MapEntity(entityPM, Poco, isNewEntity);

            this.GetCountryName();

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            if (entityPM.Addresses != null)
            {
                foreach (AddressPM addressPM in entityPM.Addresses)
                {
                    addressPM.Id = IdCounter.GetNumber("Address", entityPM.Tenant).ToString();
                    addressPM.CardId = this.Poco.Id;

                    Address newAddress = new Address()
                    {
                        Id = addressPM.Id,
                        CardId = addressPM.CardId,
                        Tenant = tenant
                    };

                    AddressMapping.MapEntity(addressPM, newAddress, isNewEntity);
                    addressRepository.Add(newAddress);
                }
            }
        }
        public void Update(CardPM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.Update();
            RunStoredProcedures();
        }

        public void Update()
        {
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleCard(entityPM.Id , tenant);

            this.Initialize();

            CardValidating.Validate(entityPM);

            this.CacheEntity();
            this.GetCountryName();

            CardTracing.Trace(entityPM, Poco, isNewEntity);
            CardMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            //UpdateGLaccountCardsDara();
            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Card");

            if (entityPM.DisconectFromContact)
            {
                CardContactRepository CardContactRepository = new Simplog.Data.CommonDataModel.Repositories.CardContactRepository(objectContext);
                CardContact cardContact = CardContactRepository.GetCardContactByContactAndCard(entityPM.Id, entityPM.ContactId, entityPM.Tenant);
                CardContactRepository.Remove(cardContact);
                CardContactRepository.SubmitChanges();
            }
        }

        private GLAccountPM GetGLaccount(string accountId)
        {
            IGLAccountQueryServiceExt gLAccountQueryServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            return gLAccountQueryServiceExt.GetSingleGLAccountPM(accountId, tenant);
        }
        GLAccountPM cardGLaccount;
        public void CreateUpdateGLaccountCardsDara(Card card)
        {
            this.Poco = card;
            cardGLaccount = GetGLaccount(Poco.GLAccountId);
            if (cardGLaccount != null)
            {
                if (CheckIfGLAccountHasCardDataRecord())
                {
                    UpdategLAccountCardsDataPM();
                }
                else
                {
                    GLAccountCardsDataPM gLAccountCardsDataPM=  CreateGLAccountCardsData();
                    UpdateGLAccount(gLAccountCardsDataPM);
                }
            }
        }
        private void UpdategLAccountCardsDataPM()
        {
            MapGLAccountCardsDataFields(gLAccountCardsDataPM);
            gLAccountCardsDataPM.ChangeSetOp = ChangeSetOperation.Update;
            UpdateChanges(gLAccountCardsDataPM);
        }
        private GLAccountCardsDataPM CreateGLAccountCardsData( )
        {
            GLAccountCardsDataPM gLAccountCardsDataPM = new GLAccountCardsDataPM();

            MapGLAccountCardsDataFields(gLAccountCardsDataPM);
            gLAccountCardsDataPM.ChangeSetOp = ChangeSetOperation.Insert;
            UpdateChanges(gLAccountCardsDataPM);          
            return gLAccountCardsDataPM;
        }
        private void UpdateChanges(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            IGLAccountCardsDataUpdateServiceExt IGLAccountCardsDataUpdateServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountCardsDataUpdateServiceExt), "GLAccountCardsDataUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountCardsDataUpdateServiceExt;
            IGLAccountCardsDataUpdateServiceExt.Update(gLAccountCardsDataPM);
        }
        private GLAccountCardsDataPM MapGLAccountCardsDataFields(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            gLAccountCardsDataPM.CollectorUserId = Poco.CollectorId;
            gLAccountCardsDataPM.CreditLimit = GetCustomerCreditLimitAmount();
            gLAccountCardsDataPM.PaymentTermId = Poco.PaymentTermId;
            gLAccountCardsDataPM.Tenant = Poco.Tenant;
            gLAccountCardsDataPM.SalesmanUserId = Poco.SalesmanUserId;
            gLAccountCardsDataPM.Phone = Poco.Phone;
            gLAccountCardsDataPM.VatNumber = Poco.VatNumber;
            gLAccountCardsDataPM.TotalOpenShipments = GetCustomerOpenFilesAmount();
            return gLAccountCardsDataPM;
        }
        private CustomerPM GetCustomer()
        {
            CustomerQuery customerQuery = new CustomerQuery(tenant);
            return customerQuery.GetSinglePM(Poco.Id, tenant);
        }
        private double? GetCustomerCreditLimitAmount()
        {
            CustomerPM customer = GetCustomer();         
            if (customer != null)
            {
                return customer.CreditLimitAmount;
            }
            else return null;
        }


        private void UpdateGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            if (cardGLaccount.IsMultiCurrency == false)
            {
                SetGLAccountCardsDataForSingleCurrencyGLAccount(gLAccountCardsDataPM);             
            }
            else
            {
                SetGLAccountForMultiCurrencyGLAccount(gLAccountCardsDataPM);               
            }

        }
        private void SetGLAccountForMultiCurrencyGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            SetGLAccountCardsData(cardGLaccount, gLAccountCardsDataPM);
        }
        private void SetGLAccountCardsDataForSingleCurrencyGLAccount(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            GLAccountCurrencyPM gLAccountCurrencyPM = GetGLAccountCurrency();
            if (gLAccountCurrencyPM != null)
            {
                GLAccountPM mainAccount = GetGLaccount(gLAccountCurrencyPM.MainGLAccountId);
                SetGLAccountCardsData(mainAccount, gLAccountCardsDataPM);
            }
            else
            {
                SetGLAccountCardsData(cardGLaccount, gLAccountCardsDataPM);
            }
        }

        private void SetGLAccountCardsData(GLAccountPM accountPM, GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            IGLAccountUpdateServiceExt glaccountUpdate = ContainerAccessor.Container.Resolve(typeof(IGLAccountUpdateServiceExt), "GLAccountUpdateServiceExt", new ParameterOverride("", 1)) as IGLAccountUpdateServiceExt;
            accountPM.CardsDataId = gLAccountCardsDataPM.Id;
            accountPM.ChangeSetOp = ChangeSetOperation.Update;
            glaccountUpdate.Update(accountPM);
        }
        private decimal? GetCustomerOpenFilesAmount()
        {
            CustomerOpenFilesAmountQuery customerOpenFilesAmount = new CustomerOpenFilesAmountQuery(tenant);
            CustomerOpenFilesAmountPM customerOpenFilesAmountPM = customerOpenFilesAmount.GetSinglePMByCustomerId(Poco.Id, Poco.Tenant);
            return customerOpenFilesAmountPM != null ? customerOpenFilesAmountPM.TotalOpenFilesAmount : (decimal)0.0;
        }
       
        GLAccountCardsDataPM gLAccountCardsDataPM;
        private bool CheckIfGLAccountHasCardDataRecord()
        {      
                if (cardGLaccount.IsMultiCurrency == false)
                {
                    gLAccountCardsDataPM= GetGLAccountCardsDataForSingkeCurrencyGLAccount();                  
                }
                else
                {
                    gLAccountCardsDataPM = GetGLAccountCardsDataForMultiCurrencyGLAccount();               
                }
                if (gLAccountCardsDataPM != null) return true;
                else return false;
        }

       private  GLAccountCardsDataPM GetGLAccountCardsDataForSingkeCurrencyGLAccount()
        {
            GLAccountCurrencyPM gLAccountCurrencyPM = GetGLAccountCurrency();
            if (gLAccountCurrencyPM != null)
            {
                GLAccountPM mainAccount = GetGLaccount(gLAccountCurrencyPM.MainGLAccountId);

                gLAccountCardsDataPM = GetGLAccountCardsData(mainAccount);
            }
            else
            {
                gLAccountCardsDataPM = GetGLAccountCardsData(cardGLaccount);
            }
            return gLAccountCardsDataPM;
        }
        private GLAccountCardsDataPM GetGLAccountCardsDataForMultiCurrencyGLAccount()
        {
          return  GetGLAccountCardsData(cardGLaccount);
        }
        private GLAccountCardsDataPM GetGLAccountCardsData(GLAccountPM gLAccount)
        {
         IGLAccountCardsDataQueryServiceExt GLAccountCardsDataQueryService = ContainerAccessor.Container.Resolve(typeof(IGLAccountCardsDataQueryServiceExt), "GLAccountCardsDataQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountCardsDataQueryServiceExt;
            return GLAccountCardsDataQueryService.GetSingleGLAccountCardsData(gLAccount.CardsDataId, gLAccount.Tenant);
        }
        private GLAccountCurrencyPM GetGLAccountCurrency()
        {
            IGLAccountCurrencyQueryServiceExt gLAccountCurrencyQueryServiceExt = ContainerAccessor.Container.Resolve(typeof(IGLAccountCurrencyQueryServiceExt), "GLAccountCurrencyQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountCurrencyQueryServiceExt;
            return gLAccountCurrencyQueryServiceExt.GetEntityByGLAccountId(cardGLaccount.Id, cardGLaccount.Tenant);
        }
        private void RunStoredProcedures()
        {

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms != "oracle")
            {
                RunStoredProcedureClass.UpdateCardSearcsRecords(entityPM.Id, entityPM.Tenant);
            }
        }



        private void Initialize()
        {
            if (isNewEntity)
            {
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdateDate = entityPM.CreateDate;
                entityPM.CreatedByUserId = loggedContact.Id;
                entityPM.UpdatedByUserId = loggedContact.Id;
            }

            else
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                entityPM.UpdatedByUserId = loggedContact.Id;
            }
        }

        private void CacheEntity()
        {
            if (CacheManager.CacheWrapper != null)
            {
                string entityName = "Card" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "CardPM" + entityPM.Id + entityPM.Tenant;
                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }
        }
        private void GetCountryName()
        {
            if (entityPM.Addresses != null)
            {
                AddressPM add = entityPM.Addresses.Where(a => a.AddressTypeId == "M").FirstOrDefault();
                if (add != null)
                {
                    this.Poco.CityName = add.City;
                    this.Poco.Address1 = add.Address1;
                    this.Poco.Address2 = add.Address2;
                    this.Poco.Phone = add.PhoneNumber;
                    this.Poco.ZipCode = add.ZipCode;

                    if (add.CountryId != null)
                    {
                        Country country = CountryRepository.GetSingleCountry(add.CountryId, entityPM.Tenant, true);
                        if (country != null)
                        {
                            this.Poco.CountryName = country.EnglishName;
                        }
                    }

                    if (add.StateId != null)
                    {
                        StateRepository stateRepository = new StateRepository(objectContext);
                        State state = stateRepository.GetSingleState(add.StateId, entityPM.Tenant);
                        if (state != null)
                        {
                            this.Poco.StateName = state.EnglishName;
                        }
                    }
                }
            }
        }
    }
}

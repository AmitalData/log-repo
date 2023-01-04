using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AddressService
    {
        bool isNewEntity;
        private int tenant;
        public Address Poco { get; set; }
        private AddressPM entityPM;
        private ICommonDataContext objectContext;
        private AddressRepository entityRepository;
        private CardRepository cardRepository;
        public AddressService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AddressRepository(objectContext);
            this.cardRepository = new CardRepository(objectContext);
        }

        public void Create(AddressPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            InitializeComponent();

            AddressValidating.Validate(entityPM);

            this.entityPM.Id = IdCounter.GetNumber("Address", tenant).ToString();
            this.Poco = new Address();
            this.Poco.Id = this.entityPM.Id;
            
            if (!entityPM.IsHybrid)
            {
                AddressTracing.Trace(entityPM, Poco, isNewEntity);
            }

            else
            {
                Poco.ExternalId = entityPM.ExternalId;
            }

            AddressMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            if (!string.IsNullOrEmpty(entityPM.BranchId))
            {
                BranchRepository rep = new BranchRepository(tenant);
                Branch branch = rep.GetSingleBranch(entityPM.BranchId, tenant);

                if(branch != null)
                {
                    string entityName = "Branch" + branch.Id + branch.Tenant;
                    string entityPmName = "BranchPM" + branch.Id + branch.Tenant;
                    if (CacheManager.CacheWrapper.Get(entityName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityName);
                    }
                    if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(entityPmName);
                    }

                    branch.AddressId = Poco.Id;
                    rep.Update(branch);
                    rep.SubmitChanges();
                }
            }
        }

        public void Update(AddressPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleAddress(entityPM.Id , entityPM.Tenant);

            InitializeComponent();

            AddressValidating.Validate(entityPM);

            if (CacheManager.CacheWrapper != null)
            {
                string entityName = "Address" + entityPM.Id + entityPM.Tenant;
                string entityPmName = "AddressPM" + entityPM.Id + entityPM.Tenant;

                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityName);
                }

                if (CacheManager.CacheWrapper.Get(entityPmName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(entityPmName);
                }
            }

            if (!entityPM.IsHybrid)
            {
                AddressTracing.Trace(entityPM, Poco, isNewEntity);
            }

            AddressMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void InitializeComponent()
        {
            if (entityPM.AddressTypeId == "M")
            {
                Card myCard = cardRepository.GetSingleCard(entityPM.CardId, tenant);
                if (myCard != null)
                {
                    myCard.CityName = entityPM.City;
                    myCard.CountryId = entityPM.CountryId;
                    myCard.Address1 = entityPM.Address1;
                    myCard.Address2 = entityPM.Address2;
                    myCard.Phone = entityPM.PhoneNumber;
                    myCard.ZipCode = entityPM.ZipCode;

                    if (entityPM.CountryId != null)
                    {
                        Country country = CountryRepository.GetSingleCountry(entityPM.CountryId, tenant, true);
                        if (country != null)
                        {
                            myCard.CountryCode = country.Code;
                            myCard.CountryName = country.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPM.StateId))
                    {
                        StateRepository stateRepository = new StateRepository(objectContext);
                        State state = stateRepository.GetSingleState(entityPM.StateId, entityPM.Tenant);
                        if (state != null)
                        {
                            myCard.StateName = state.EnglishName;
                        }
                    }

                    BuildSearchFields(myCard);
                    cardRepository.Update(myCard);
                    cardRepository.SubmitChanges();
                }
            }
        }

        private static void BuildSearchFields(Card entityCard)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityCard.Code))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.Code : mySearchFields + "," + entityCard.Code;
            }

            if (!string.IsNullOrEmpty(entityCard.EnglishName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.EnglishName : mySearchFields + "," + entityCard.EnglishName;
            }

            if (!string.IsNullOrEmpty(entityCard.LocalName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.LocalName : mySearchFields + "," + entityCard.LocalName;
            }

            if (!string.IsNullOrEmpty(entityCard.VatNumber))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.VatNumber : mySearchFields + "," + entityCard.VatNumber;
            }

            if (!string.IsNullOrEmpty(entityCard.ReceivablesAccountingCard))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.ReceivablesAccountingCard : mySearchFields + "," + entityCard.ReceivablesAccountingCard;
            }

            if (!string.IsNullOrEmpty(entityCard.PayablesAccountingCard))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.PayablesAccountingCard : mySearchFields + "," + entityCard.PayablesAccountingCard;
            }

            if (!string.IsNullOrEmpty(entityCard.CityName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.CityName : mySearchFields + "," + entityCard.CityName;
            }

            if (!string.IsNullOrEmpty(entityCard.CountryName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityCard.CountryName : mySearchFields + "," + entityCard.CountryName;
            }

            if (entityCard.PartnerTypeId == "AL")
            {
                AirlineRepository airlineRepository = new AirlineRepository(entityCard.Tenant);
                Airline airline = airlineRepository.GetSingleAirline(entityCard.Id, entityCard.Tenant);
                if (airline != null)
                {
                    if (!string.IsNullOrEmpty(airline.Prefix))
                    {
                        mySearchFields = string.IsNullOrEmpty(mySearchFields) ? airline.Prefix : mySearchFields + "," + airline.Prefix;
                    }
                }
            }

            else if (entityCard.PartnerTypeId == "CS" || entityCard.PartnerTypeId == "PO")
            {
                CustomerRepository customerRepository = new CustomerRepository(entityCard.Tenant);
                Customer customer = customerRepository.GetSingleCustomer(entityCard.Id, entityCard.Tenant, true);
                if (customer != null)
                {
                    CardContactRepository cardContactRepository = new CardContactRepository(entityCard.Tenant);
                    IQueryable<Contact> myContacts = cardContactRepository.GetContactsByCardId(entityCard.Id);
                    foreach (Contact item in myContacts)
                    {
                        if (!string.IsNullOrEmpty(item.Email))
                        {
                            mySearchFields = string.IsNullOrEmpty(mySearchFields) ? item.Email : mySearchFields + "," + item.Email;
                        }

                        if (!string.IsNullOrEmpty(item.EnglishName))
                        {
                            mySearchFields = string.IsNullOrEmpty(mySearchFields) ? item.EnglishName : mySearchFields + "," + item.EnglishName;
                        }
                    }

                    List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Customer", entityCard.Tenant).Where(o => o.DataTypeCode == "Text" || o.DataTypeCode == "nText").ToList();
                    CustomFieldResolver customFieldResolver = new CustomFieldResolver(entityCard.Tenant);
                    foreach (ObjectField field in customFields)
                    {
                        object value = customFieldResolver.GetFieldValue(entityCard, field, entityCard.Tenant);
                        if (value != null)
                        {
                            string myValueString = value.ToString();

                            if (!string.IsNullOrEmpty(myValueString))
                            {
                                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? myValueString : mySearchFields + "," + myValueString;
                            }
                        }
                    }
                }
            }

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityCard.SearchFields = mySearchFields;
        }
    }
}

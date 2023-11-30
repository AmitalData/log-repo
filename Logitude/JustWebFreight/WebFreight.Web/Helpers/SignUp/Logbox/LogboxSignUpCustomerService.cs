using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.InfrastructureModel;

namespace WebFreight.Web.Helpers.SignUp.Logbox
{
    public class LogboxSignUpCustomerService
    {
        public static string CreateNewFromCloud(SignUpInfoClass signUpInfoClass, int tenant)
        {
            if (!signUpInfoClass.IsCreateLogboxTenantFromCloud) return signUpInfoClass.CustomerId;

            CustomerPM customerPM = new CustomerPM
            {
                EnglishName = signUpInfoClass.Company,
                CustomerStatusCode = "ACT",
                Code = CodeCounter.GetNumber("Card", tenant).ToString(),
                VatNumber = signUpInfoClass.VatNumber,
                CountryCode = signUpInfoClass.CountryCode,
                CityName = signUpInfoClass.City,
                Tenant = tenant,
                IsFirstContactToAdd = true,
                PartnerTypeId = "CS",
                IsHybrid = true,
                IsCustomer = true,
                Contacts = new List<ContactPM>(),
            };

            customerPM.Addresses.Add(GetNewAddressPM(signUpInfoClass, tenant));
            ContactPM contactPM = GetContactPM(signUpInfoClass, tenant);
            customerPM.ExistedContactId = contactPM.Id;
            customerPM.Contacts.Add(contactPM);

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CustomerService customerService = new CustomerService(commonContext, customerPM);
            customerService.Create();
            signUpInfoClass.CustomerId = customerPM.Id;

            return customerPM.Id;
        }

        private static ContactPM GetContactPM(SignUpInfoClass signUpInfoClass, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(signUpInfoClass.Email, tenant);
            if (contactPM == null)
            {
                return GetNewContactPM(signUpInfoClass, tenant);
            }

            contactPM.IsCreatedWithPartner = true;
            contactPM.SetAsPrimaryForCard = true;

            return contactPM;
        }

        private static  AddressPM GetNewAddressPM(SignUpInfoClass signUpInfoClass, int tenant)
        {
            CountryRepository countryRepository = new CountryRepository(tenant);
            string countryId = countryRepository.GetCountryIdByCode(signUpInfoClass.CountryCode, tenant);

            return new AddressPM
            {
                CardCode = "new",
                AddressTypeId = "M",
                City = signUpInfoClass.City,
                ContactBusinessPhone = signUpInfoClass.Phone,
                ContactEmail = signUpInfoClass.Email,
                ContactName = signUpInfoClass.Name,
                CountryCode = signUpInfoClass.CountryCode,
                CountryEnglishName = signUpInfoClass.CountryName,
                CountryName = signUpInfoClass.CountryName,
                CountryId = countryId,
                Description = "Main Address",
                Name = signUpInfoClass.Name,
                PhoneNumber = signUpInfoClass.Phone,
                Tenant = tenant,
                VatNumber = signUpInfoClass.VatNumber,
                IsCreatedWithPartner = true,
            };
        }

        private static  ContactPM GetNewContactPM(SignUpInfoClass signUpInfoClass, int tenant)
        {
            return new ContactPM
            {
                BusinessPhone = signUpInfoClass.Phone,
                CardId = "newCard",
                Email = signUpInfoClass.Email,
                EnglishName = signUpInfoClass.Name,
                IsCreatedWithPartner = true,
                LocalName = signUpInfoClass.Name,
                SetAsPrimaryForCard = true,
                Tenant = tenant,
            };
        }

        public static void UpdateFromCloud(SignUpInfoClass signUpInfoClass, ICommonDataContext commonContext, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(signUpInfoClass.Email, tenant);
            CustomerQuery CustomerQuery = new CustomerQuery(tenant);
            var currentCustomer = CustomerQuery.GetSinglePMForLogBox(signUpInfoClass.CustomerId, tenant);
            if (currentCustomer == null)
            {
                throw new ApplicationException("Customer with Id: " + signUpInfoClass.CustomerId + " is not exist!");
            }
            currentCustomer.PrimaryContactId = contactPM.Id;
            currentCustomer.EnglishName = contactPM.EnglishName;
            currentCustomer.LocalName = contactPM.LocalName;
            currentCustomer.ExistedContactId = contactPM.Id;

            CustomerService CustomerService = new CustomerService(commonContext, currentCustomer, contactPM.Id);
            CustomerService.Update();
        }

        public static string CreateStandardNew(SignUpInfoClass signUpInfoClass, ICommonDataContext commonContext, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            ContactQuery contactQuery = new ContactQuery(contactRepository);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly(signUpInfoClass.Email, tenant);
            if(contactPM == null)
            {
                throw new ApplicationException("Contact with Email: " + signUpInfoClass.Email + " is not exist!");
            }
            contactPM.SetAsPrimaryForCard = true;
            contactPM.IsCreatedWithPartner = true;
            CustomerPM customerPM = new CustomerPM()
            {
                Tenant = tenant,
                IsFirstContactToAdd = true,
                PartnerTypeId = "CS",
                Code = CodeCounter.GetNumber("Card", tenant).ToString(),
                IsHybrid = true,
                IsCustomer = true,
                Contacts = new List<ContactPM>(),
            };

            customerPM.PrimaryContactId = contactPM.Id;
            customerPM.EnglishName = contactPM.EnglishName;
            customerPM.LocalName = contactPM.LocalName;
            customerPM.ExistedContactId = contactPM.Id;

            customerPM.Addresses.Add(GetNewAddressPM(signUpInfoClass, tenant));
            customerPM.Contacts.Add(contactPM);

            CustomerService CustomerService = new CustomerService(commonContext, customerPM, contactPM.Id);
            CustomerService.Create();

            return customerPM.Id;
        }
    }
}
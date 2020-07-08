using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class PartnersUploadHelper : BatchTaskExecutionsService
    {
        public PartnersUploadHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
            {
                //this.RunPartnersGenerator(); 
                scope.Complete();
            }

        }

        private void RunPartnersGenerator()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(GenerateTariffsArgs));
            GenerateTariffsArgs parameterArgs = serializer.Deserialize(stringReader) as GenerateTariffsArgs;
            int tenant = parameterArgs.Tenant;
            var LoggedUserEmail = parameterArgs.LoggedUserEmail;
            ContactRepository contactRep = new ContactRepository(tenant);

            Contact systemContact = contactRep.GetSingleContactByEmail(LoggedUserEmail, tenant);
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            CountryRepository countryRep = new CountryRepository(commonDataContext);
            StateRepository stateRep = new StateRepository(commonDataContext);
            Dictionary<string, State> statesDictionary = stateRep.GetStates(tenant).ToDictionary(d => d.Code + ',' + d.CountryId, o => o);
            Dictionary<string, Country> countrieysDictionary = countryRep.GetCountries(tenant).ToDictionary(d => d.Code, o => o);

            Country country = null;
            if (countrieysDictionary.Keys.Contains("MX"))
            {
                country = countrieysDictionary["MX"];
            }

            State state = null;
            if (country != null)
            {

                if (statesDictionary.Keys.Contains("JAL" + ',' + country.Id))
                {
                    state = statesDictionary["JAL" + ',' + country.Id];
                }
            }

            AddressPM address = new AddressPM()
            {
                Name = "Main Address",
                Description = "Main Address",
                Address1 = "address1",
                Address2 = "address2",
                ZipCode = "zip",
                FaxNumber = "fax",
                AddressTypeId = "M",
                Tenant = tenant,
                StateId = state != null ? state.Id : null,
                CountryId = country != null ? country.Id : null,
                City = "city",
                PhoneNumber = "phoneNumber",
                ContactFax = "fax",

            };
            ContactPM contactPM = new ContactPM()
            {
                Email = "contact@email.com",
                EnglishName = "test contact",
                Tenant = tenant,
                CardId = "newCard",
                IsHybrid = true,
                IsCreatedWithPartner = true,
            };

            for (int i = 1; i <= 1000; i++)
            {
                CustomerPM customer = new CustomerPM()
                {
                    EnglishName = "customer " + i,
                    VatNumber = "customervat " + i,
                    Tenant = tenant,
                    IsHybrid = true,
                    Code = CodeCounter.GetNumber("Customer", tenant).ToString(),
                    PartnerTypeId = "CS",
                    CustomerStatusCode = "ACT",
                    IsCustomer = true,
                };
                customer.Addresses.Add(address);
                customer.Contacts.Add(contactPM);
                CustomerService service = new CustomerService(commonDataContext, customer, systemContact.Id);
                service.Create();
            }
        }
    }
}
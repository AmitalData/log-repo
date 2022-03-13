using Logitude.Base.Models.LocationsPreparation;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.UserTenantPreparation;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CommonTests.Models.Builders
{
    public class AddressBuilder
    {
        private AddressPM _addressPM;

        public AddressBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _addressPM = new AddressPM();
        }

        public AddressBuilder Id(string id)
        {
            _addressPM.Id = id;
            return this;
        }

        public AddressBuilder Tenant(int tenant)
        {
            _addressPM.Tenant = tenant;
            return this;
        }

        public AddressBuilder AddressId(string addressId)
        {
            _addressPM.AddressId = addressId;
            return this;
        }
        public AddressBuilder Description(string description)
        {
            _addressPM.Description = description;
            return this;
        }
        public AddressBuilder AddressTypeId(string addressTypeId)
        {
            _addressPM.AddressTypeId = addressTypeId;
            return this;
        }
        public AddressBuilder City(string city)
        {
            _addressPM.City = city;
            return this;
        }
        public AddressBuilder Address1(string address1)
        {
            _addressPM.Address1 = address1;
            return this;
        }
        public AddressBuilder Address2(string address2)
        {
            _addressPM.Address2 = address2;
            return this;
        }

        public AddressBuilder CountryId(string countryId)
        {
            _addressPM.CountryId = countryId;
            return this;
        }

        public AddressBuilder Name(string name)
        {
            _addressPM.Name = name;
            return this;
        }

        public AddressBuilder AgentId(string agentId)
        {
            _addressPM.AgentId = agentId;
            return this;
        }

        public AddressBuilder CurrencyId(string currencyId )
        {
            _addressPM.CurrencyId = currencyId;
            return this;
        }

        public AddressBuilder StateId(string stateId)
        {
            _addressPM.StateId = stateId;
            return this;
        }
        public AddressBuilder ZipCode(string zipCode)
        {
            _addressPM.ZipCode = zipCode;
            return this;
        }
        public AddressBuilder FaxNumber(string faxNumber)
        {
            _addressPM.FaxNumber = faxNumber;
            return this;
        }
        public AddressBuilder PhoneNumber(string phoneNumber)
        {
            _addressPM.PhoneNumber = phoneNumber;
            return this;
        }
        public AddressBuilder CardId(string cardId)
        {
            _addressPM.CardId = cardId;
            return this;
        }
        public AddressBuilder CountryCode(string countryCode)
        {
            _addressPM.CountryCode = countryCode;
            return this;
        }
        public AddressBuilder CountryName(string countryName)
        {
            _addressPM.CountryName = countryName;
            return this;
        }
        public AddressBuilder CountryEnglishName(string countryEnglishName)
        {
            _addressPM.CountryEnglishName = countryEnglishName;
            return this;
        }
        public AddressBuilder StateEnglishName(string stateEnglishName)
        {
            _addressPM.StateEnglishName = stateEnglishName;
            return this;
        }
        public AddressBuilder StateCode(string stateCode)
        {
            _addressPM.StateCode = stateCode;
            return this;
        }
        public AddressBuilder VatNumber(string vatNumber)
        {
            _addressPM.VatNumber = vatNumber;
            return this;
        }
        public AddressBuilder CardCode(string cardCode)
        {
            _addressPM.CardCode = cardCode;
            return this;
        }
        public AddressBuilder CardEnglishName(string cardEnglishName)
        {
            _addressPM.CardEnglishName = cardEnglishName;
            return this;
        }
        public AddressBuilder HasStates(bool hasStates)
        {
            _addressPM.HasStates = hasStates;
            return this;
        }
        public AddressBuilder IsStateRequired(bool isStateRequired)
        {
            _addressPM.IsStateRequired = isStateRequired;
            return this;
        }

        public AddressPM Build()
        {
            AddressPM result = _addressPM;

            this.Reset();

            return result;
        }

        public AddressBuilder WithModel(AddressPM addressPM)
        {
            _addressPM = addressPM;
            return this;
        }

        public AddressBuilder WithDefualtValues()
        {
            _addressPM = new AddressPM
            {
                Tenant = UserTenant.Tenant,
                CountryId = LocationsData.CountryUSId,
                //AgentId = PartnersData.AgentId,
                StateId = LocationsData.StateAKId,
                //CountryName = "United States of America",
                //CountryEnglishName = "United States of America",
                //CountryCode = "US",
                //StateCode = "AK",
                //HasStates=true ,
                //IsStateRequired=true

            };
            return this;
        }

        public AddressBuilder FromDataTable(Table dataTable)
        {
            _addressPM = dataTable.CreateInstance<AddressPM>();
            return this;
        }
    }
}

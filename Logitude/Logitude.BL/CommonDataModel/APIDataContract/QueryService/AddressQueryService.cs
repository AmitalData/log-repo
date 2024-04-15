using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class AddressQueryService
    {
        public AddressPM AddressCustomDataMappingAndValidatin(Address MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new AddressPM();

                if (!string.IsNullOrEmpty(MyEntity.ExternalId))
                {
                    temp = query.GetAddressByExternalId(MyEntity.ExternalId, Tenant);
                    
                    if(temp == null) 
                    {
                        CardRepository cardRepository = new CardRepository(Tenant);
                        string cardId = cardRepository.GetActiveCardIdByCode(MyEntity.ExternalId, Tenant);
                        if(cardId != null )
                            temp = query.GetAddressByCardId(cardId, Tenant);

                    }
                }

               
                if (temp == null)
                {
                    throw new ApplicationException("Address with ExternalId " + MyEntity.ExternalId + " doesn't exist");
                }
                //if (string.IsNullOrEmpty(temp.Id))
                //{
                //    temp.Id = MyEntity.Id;
                //}
                if (string.IsNullOrEmpty(temp.Address1))
                {
                    temp.Address1 = MyEntity.Address1;
                }
                if (string.IsNullOrEmpty(temp.Address2))
                {
                    temp.Address2 = MyEntity.Address2;
                }
                if (string.IsNullOrEmpty(temp.Name))
                {
                    temp.Name = MyEntity.Name;
                }
                if (string.IsNullOrEmpty(temp.ExternalId))
                {
                    temp.ExternalId = MyEntity.ExternalId;
                }
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Address> AddressCustomDataMapping(CardPM EntityPm, List<AddressPM> MyEntityPMs, int Tenant, string ComputingPartnerName = "")
        {
            return this.AddressMapping(MyEntityPMs, Tenant, ComputingPartnerName);
        }

        public List<Address> AddressMapping(List<AddressPM> addresses, int tenant, string computingPartner = "")
        {
            try
            {
                var myList = new List<Address>();
                foreach (AddressPM item in addresses)
                {
                    var address = new Address();
                    address.Id = item.Id;
                    address.Address1 = item.Address1;
                    address.Address2 = item.Address2;                    
                    address.City = item.City;                    
                    address.ExternalId = item.ExternalId;
                    address.FaxNumber = item.FaxNumber;
                    address.Name = item.Name;
                    address.PhoneNumber = item.PhoneNumber;                    
                    address.ZipCode = item.ZipCode;

                    if (item.AddressTypeId != null)
                    {
                        AddressTypeQueryService queryService = new AddressTypeQueryService(tenant);
                        address.AddressType = queryService.GetAddressTypeById(item.AddressTypeId, tenant, computingPartner);
                    }

                    if (item.CountryId != null)
                    {
                        CountryQueryService queryService = new CountryQueryService(tenant);
                        address.Country = queryService.GetCountryById(item.CountryId, tenant, computingPartner);
                    }

                    if (item.StateId != null)
                    {
                        StateQueryService queryService = new StateQueryService(tenant);
                        address.State = queryService.GetStateById(item.StateId, tenant, computingPartner);
                    }

                    myList.Add(address);
                }

                return myList;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Address AddressCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                AddressQueryService AddressService0 = new AddressQueryService(Tenant);
                var Address = AddressService0.GetAddressById(Id, Tenant,ComputingPartnerName);
                return Address;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AddressPM AddressCustomDataMappingAndValidatin_CityCountry(Address MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new AddressPM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                if (temp == null)
                {
                    throw new ApplicationException("Address with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                temp.Name = MyEntity.Name;
                temp.Address1 = MyEntity.Address1;
                temp.Address2 = MyEntity.Address2; CountryQueryService CountryCountryService = new CountryQueryService(Tenant);
                if (MyEntity.Country != null)
                {
                    var myCountryPM = CountryCountryService.CountryDataMappingAndValidatin(MyEntity.Country, Tenant, ComputingPartnerName);
                    if (myCountryPM != null)
                    {
                        temp.CountryId = myCountryPM.Id;
                    }

                }

                //CityQueryService CityCityService = new CityQueryService(Tenant);
                //if (MyEntity.City != null)
                //{
                //    var myCityPM = CityCityService.CityCustomDataMappingAndValidatin(MyEntity.City, temp.CountryId, Tenant, ComputingPartnerName);
                //    if (myCityPM != null)
                //    {
                //        temp.City = myCityPM.EnglishName;
                //        temp.CityCode = myCityPM.Code;
                //    }
                //}

                temp.City = MyEntity.City;
                temp.ZipCode = MyEntity.ZipCode;
                temp.PhoneNumber = MyEntity.PhoneNumber;
                temp.FaxNumber = MyEntity.FaxNumber; StateQueryService StateStateService = new StateQueryService(Tenant);
                if (MyEntity.State != null)
                {
                    var myStatePM = StateStateService.StateDataMappingAndValidatin(MyEntity.State, Tenant, ComputingPartnerName);
                    if (myStatePM != null)
                    {
                        temp.StateId = myStatePM.Id;
                    }

                }


                temp.ExternalId = MyEntity.ExternalId;
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal List<AddressPM> AddressCustomDataMappingAndValidatin(Customer myEntity, List<Address> addresses, int tenant, string computingPartnerName, bool isUpdate)
        {
            throw new NotImplementedException();
        }
    }
}

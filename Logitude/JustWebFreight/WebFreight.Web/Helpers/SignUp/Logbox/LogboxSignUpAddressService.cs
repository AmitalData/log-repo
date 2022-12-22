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
    public class LogboxSignUpAddressService
    {
        public static AddressPM GetNewTenantAddress(SignUpInfoClass signUpInfoClass, ICommonDataContext commonContext, int tenant)
        {
            int requestTenant = signUpInfoClass.IsCreateLogboxTenantFromCloud ? tenant : signUpInfoClass.Tenant;
            CountryRepository CountryRepository = new CountryRepository(tenant);
            AddressRepository addressRepository = new AddressRepository(requestTenant);
            var CustomerAddress = addressRepository.GetMainAddressByCardId(signUpInfoClass.CustomerId, requestTenant);
            if (CustomerAddress == null)
            {
                throw new ApplicationException("Address with Card Id: " + signUpInfoClass.CustomerId + " is not exist!");
            }
            var NewCountry = CountryRepository.GetSingleCountryByCode(CustomerAddress?.Country?.Code, tenant);
            if (NewCountry == null)
            {
                throw new ApplicationException("New Country with Code: " + CustomerAddress?.Country?.Code + " is not exist!");
            }
            AddressService addressService = new AddressService(commonContext, tenant);
            AddressPM TenantAddress = new AddressPM()
            {
                Address1 = CustomerAddress.Address1,
                Address2 = CustomerAddress.Address2,
                AddressTypeId = CustomerAddress.AddressTypeId,
                Name = CustomerAddress.Name,
                City = CustomerAddress.City,
                CountryId = NewCountry.Id,
                IsLocalLanguage = true,
                InActive = false,
                Description = CustomerAddress.Description,
                Tenant = tenant,
                IsHybrid = true,
            };
            addressService.Create(TenantAddress);
            return TenantAddress;
        }
    }
}
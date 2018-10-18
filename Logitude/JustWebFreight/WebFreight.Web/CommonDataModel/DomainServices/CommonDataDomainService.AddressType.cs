using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Linq;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<AddressType> GetAddressTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressTypesRepository = new AddressTypeRepository(tenant);
            return addressTypesRepository.GetAddressTypes();
        }

        public IQueryable<AddressType> GetAddressTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            addressTypesRepository = new AddressTypeRepository(tenant);
            return addressTypesRepository.GetAddressTypes();
        }

        public void InsertAddressType(AddressType addressType)
        {
            addressTypesRepository.Add(addressType);
        }

        public void UpdateAddressType(AddressType currentaddressType)
        {
            addressTypesRepository.Update(currentaddressType);
        }

        public void DeleteAddressType(AddressType addressType)
        {
            addressTypesRepository.Remove(addressType);
        }
    }
}
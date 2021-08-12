using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Services
{
    public class PartnerService
    {
        public string GetAgentId()
        {
            return CreatePartner(BuildPartner())?.PartnerId;
        }


        private static Partner BuildPartner()
        {
            return new Partner
            {
                Tenant = UserTenant.Tenant,
                PartnerTypeId = "AG",
                Address = BuildPartnerAddress("TestAgentExport"),
                Agent = new PartnerInformation
                {
                    Tenant = UserTenant.Tenant,
                    EnglishName = "TestAgentExport",
                    LocalName = "TestAgentExport",
                    PartnerTypeId = "AG",
                    Code = "AG",
                    IsCustomer = false

                }
            };
        }

        private static Address BuildPartnerAddress(string partnerName)
        {
            return new Address
            {
                Tenant = UserTenant.Tenant,
                Name = partnerName + " Address",
                Description = "Main Address",
                AddressTypeId = "M",
                Address1 = "Test Address",
                StateId = LocationsData.StateAKId,
                CountryId = LocationsData.CountryUSId,
                IsCreatedWithPartner = true
            };
        }


        private Partner CreatePartner(Partner partner)
        {
            ApiResponse<Partner> response = APICaller.CallPost<Partner>(partner, Urls.PartnersDomainController, UserTenant.Token);
            return response.Data;
        }
    }
}

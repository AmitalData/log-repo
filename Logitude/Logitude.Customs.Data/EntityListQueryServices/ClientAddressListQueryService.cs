	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ClientAddressListQueryService
    {
	    private IQueryable<ClientAddressList> GetIqueryableList(IQueryable<ClientAddress> iQueryable)
        {
            IQueryable<ClientAddressList> query = (from a in iQueryable
                                                   select new ClientAddressList()
                                          {
                                             AddressId = a.AddressId,
                                             AddressPurposeCode = a.AddressPurposeCode,
                                             AddressTypeCode = a.AddressTypeCode,
                                             AuthorizedSignerPermit1 = a.AuthorizedSignerPermit1,
                                             AuthorizedSignerPermit2 = a.AuthorizedSignerPermit2,
                                             AuthorizedSignerPermit3 = a.AuthorizedSignerPermit3,
                                            
                                             BranchName = a.BranchName,
                                             ClientId = a.ClientId,
                                       
                                             ContactFirstName = a.ContactFirstName,
                                             ContactIdentifier = a.ContactIdentifier,
                                             ContactLastName = a.ContactLastName,
                                             ContactRoleTypeCode = a.ContactRoleTypeCode,
                                             ContactStateCode = a.ContactStateCode,
                                             EnglishCityName = a.EnglishCityName,
                                             EnglishCountryCode = a.EnglishCountryCode,
                                             EnglishMainAddressLine = a.EnglishMainAddressLine,
                                             EnglishPostalCode = a.EnglishPostalCode,
                                             IsHebrewAddress = a.IsHebrewAddress,
                                             LocalApartment = a.LocalApartment,
                                             EnglishSubCountryCode = a.EnglishSubCountryCode,
                                             IsPalestinianCity = a.IsPalestinianCity,
                                             LocalCityCode = a.LocalCityCode,
                                             LocalEntrance = a.LocalEntrance,
                                             LocalHouseLetter = a.LocalHouseLetter,
                                             LocalHouseNumber = a.LocalHouseNumber,
                                             LocalPOBox = a.LocalPOBox,
                                             LocalPostalCode = a.LocalPostalCode,
                                             LocalSecondLine = a.LocalSecondLine,
                                             LocalStreetName = a.LocalStreetName,
                                             Tenant = a.Tenant,
                                             AddressPurposeName = a.AddressPurpose == null ? null : a.AddressPurpose.LocalName,
                                             AddressTypeName = a.AddressType == null ? null : a.AddressType.LocalName,
                                             AuthorizedSignerPermit1Name = a.AuthorizedSigner1 == null ? null : a.AuthorizedSigner1.LocalName,
                                             AuthorizedSignerPermit2Name = a.AuthorizedSigner2 == null ? null : a.AuthorizedSigner2.LocalName,
                                             AuthorizedSignerPermit3Name = a.AuthorizedSigner3 == null ? null : a.AuthorizedSigner3.LocalName,
                                             ContactRoleTypeName = a.ContactRoleType == null ? null : a.ContactRoleType.LocalName,
                                             ContactStateName = a.AddressContactState == null ? null : a.AddressContactState.LocalName,
                                             EnglishCountryName = a.Country == null ? null : a.Country.LocalName,
                                             EnglishSubCountryName = a.SubCountry == null ? null : a.SubCountry.LocalName,
                                             LocalCityName = a.City == null ? null : a.City.LocalName,
                                             
                                             CustomAddressCode= a.CustomAddressCode,
                                   

                                          });
            return query;
		}

        private IQueryable<ClientAddress> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ClientAddress> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	
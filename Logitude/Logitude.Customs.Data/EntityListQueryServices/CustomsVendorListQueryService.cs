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

    public partial class CustomsVendorListQueryService
    {
	    private IQueryable<CustomsVendorList> GetIqueryableList(IQueryable<CustomsVendor> iQueryable)
        {
            IQueryable<CustomsVendorList> query = (from a in iQueryable.Include("Country").Include("SubCountry").Include("VendorType").Include("VendorStatus")
                                                   select new CustomsVendorList()
                                            {
                                                Id = a.Id,
                                                CityName = a.CityName,
                                                CountryCode = a.CountryCode,
                                                CountryName = a.Country.LocalName,
                                                DunsNumber = a.DunsNumber,
                                                MainAddressLine = a.MainAddressLine,
                                                PostalCode = a.PostalCode,
                                                SearchFields = a.SearchFields,
                                                StatusCode = a.StatusCode,
                                                SubCountryCode = a.SubCountryCode,
                                                SubCountryName = a.SubCountry.EnglishName,
                                                Tenant = a.Tenant,
                                                TransactionTypeID = a.TransactionTypeID,
                                                VATNumber = a.VATNumber,
                                                VendorName = a.VendorName,
                                                VendorNumber = a.VendorNumber,
                                                VendorTypeCode = a.VendorTypeCode,
                                                VendorTypeName = a.VendorType.EnglishName,
                                                InActive = a.InActive,
                                                StatusName = a.VendorStatus != null? a.VendorStatus.LocalName : null,


                                            });
            return query;
		}

        private IQueryable<CustomsVendor> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsVendor> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
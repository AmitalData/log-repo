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

    public partial class VehicleOwnerListQueryService
    {
	    private IQueryable<VehicleOwnerList> GetIqueryableList(IQueryable<VehicleOwner> iQueryable)
        {
            IQueryable<VehicleOwnerList> query = (from a in iQueryable.Include("CustomsCountry").Include("PassportType")
                                                  select new VehicleOwnerList()
                                                         {
                                                             ClientId = a.ClientId,
                                                             FirstName = a.FirstName,
                                                             IsMain = a.IsMain,
                                                             LastNameOrCorporationName = a.LastNameOrCorporationName,
                                                             LineNumber = a.LineNumber,
                                                             Tenant = a.Tenant,
                                                             VehicleId = a.VehicleId,
                                                             PassportNumber = a.PassportNumber,
                                                             PassCountryCode = a.PassCountryCode,
                                                             PassCountryName = a.CustomsCountry != null ? a.CustomsCountry.LocalName : null,
                                                             ImporterPassportTypeCode = a.ImporterPassportTypeCode,
                                                             ImporterPassportTypeName = a.PassportType != null ? a.PassportType.LocalName : null,
                                                  });
            return query;
        }

        private IQueryable<VehicleOwner> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<VehicleOwner> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
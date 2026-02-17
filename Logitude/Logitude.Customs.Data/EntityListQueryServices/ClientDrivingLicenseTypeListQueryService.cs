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

    public partial class ClientDrivingLicenseTypeListQueryService
    {
	    private IQueryable<ClientDrivingLicenseTypeList> GetIqueryableList(IQueryable<ClientDrivingLicenseType> iQueryable)
        {
		IQueryable<ClientDrivingLicenseTypeList> query = (from a in iQueryable
                                            select new ClientDrivingLicenseTypeList()
											{
					                          ClientId = a.ClientId,				
					                          Tenant = a.Tenant,
					                          ClientDrivingLicenseLine = a.ClientDrivingLicenseLine,
					                          DriversLicenseTypeCode = a.DriversLicenseTypeCode,
		                    	            });
            return query;
		}

		private IQueryable<ClientDrivingLicenseType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientDrivingLicenseType> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	
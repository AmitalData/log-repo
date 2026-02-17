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

    public partial class ClientDrivingLicenseListQueryService
    {
	    private IQueryable<ClientDrivingLicenseList> GetIqueryableList(IQueryable<ClientDrivingLicense> iQueryable)
        {
		IQueryable<ClientDrivingLicenseList> query = (from a in iQueryable
                                            select new ClientDrivingLicenseList()
											{
					                          ClientId = a.ClientId,					
					                          Tenant = a.Tenant,
					                          Line = a.Line,				
					                          DrivingLicenseNumber = a.DrivingLicenseNumber,
					                          DriverLicenseValidityDate = a.DriverLicenseValidityDate,
					                          DrivingLicenseCountryID = a.DrivingLicenseCountryID,
					
		                    	            });
            return query;
		}

		private IQueryable<ClientDrivingLicense> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClientDrivingLicense> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	
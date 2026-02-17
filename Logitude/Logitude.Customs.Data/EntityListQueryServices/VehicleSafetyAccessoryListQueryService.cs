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

    public partial class VehicleSafetyAccessoryListQueryService
    {
	    private IQueryable<VehicleSafetyAccessoryList> GetIqueryableList(IQueryable<VehicleSafetyAccessory> iQueryable)
        {
            IQueryable<VehicleSafetyAccessoryList> query = (from a in iQueryable
                                                            select new VehicleSafetyAccessoryList()
                                                                            {
                                                                                VehicleSafetyAccessoryCode = a.VehicleSafetyAccessoryCode,
                                                                                VehicleSafAccessoryInstlTypCod = a.VehicleSafAccessoryInstlTypCod,
                                                                                LineNumber = a.LineNumber,
                                                                                Tenant = a.Tenant,
                                                                                VehicleId = a.VehicleId,

                                                                            });
            return query;
		}

        private IQueryable<VehicleSafetyAccessory> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<VehicleSafetyAccessory> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	
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

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoTrackingMilestoneListQueryService
    {
	    private IQueryable<CargoTrackingMilestoneList> GetIqueryableList(IQueryable<CargoTrackingMilestone> iQueryable)
        {
		IQueryable<CargoTrackingMilestoneList> query = (from a in iQueryable
                                            select new CargoTrackingMilestoneList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					                          Weight = a.Weight,
											  ExportWeight = a.ExportWeight,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingMilestone> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingMilestone> iQueryable)
        {
			return iQueryable;
		}
		private IQueryable<CargoTrackingMilestone> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingMilestone> iQueryable)
        {
			return iQueryable;
		}
		
	}


}
	
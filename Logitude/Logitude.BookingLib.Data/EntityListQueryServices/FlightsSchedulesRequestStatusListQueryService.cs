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

using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityLists;

namespace Logitude.BookingLib.Data.EntityListQueryServices
{ 

    public partial class FlightsSchedulesRequestStatusListQueryService
    {
	    private IQueryable<FlightsSchedulesRequestStatusList> GetIqueryableList(IQueryable<FlightsSchedulesRequestStatus> iQueryable)
        {
            return (from a in iQueryable
                    select new FlightsSchedulesRequestStatusList()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    });
        }

		private IQueryable<FlightsSchedulesRequestStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<FlightsSchedulesRequestStatus> iQueryable)
        {
            return iQueryable;

        }

		private IQueryable<FlightsSchedulesRequestStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<FlightsSchedulesRequestStatus> iQueryable)
        {
			return iQueryable;
		}
	}


}
	
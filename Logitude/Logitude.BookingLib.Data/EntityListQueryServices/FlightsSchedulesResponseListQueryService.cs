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

    public partial class FlightsSchedulesResponseListQueryService
    {
	    private IQueryable<FlightsSchedulesResponseList> GetIqueryableList(IQueryable<FlightsSchedulesResponse> iQueryable)
        {
			throw new NotImplementedException();
		}

        private IQueryable<FlightsSchedulesResponse> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<FlightsSchedulesResponse> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<FlightsSchedulesResponse> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<FlightsSchedulesResponse> iQueryable, int tenant)
        {
			return iQueryable;
		}
	}


}
	
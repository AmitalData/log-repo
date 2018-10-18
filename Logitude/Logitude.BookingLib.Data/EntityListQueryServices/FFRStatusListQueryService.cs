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
    public partial class FFRStatusListQueryService
    {
        private IQueryable<FFRStatusList> GetIqueryableList(IQueryable<FFRStatus> iQueryable)
        {
            IQueryable<FFRStatusList> query = (from a in iQueryable
                                                   select new FFRStatusList()
                                                   {
                                                       Code = a.Code,
                                                       Name = a.Name,
                                                       SearchFields = a.SearchFields,
                                                   });
            return query;
        }

        private IQueryable<FFRStatus> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<FFRStatus> iQueryable)
        {
            return iQueryable;
        }

        private IQueryable<FFRStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<FFRStatus> iQueryable)
        {
            return iQueryable;
        }
	}


}
	
	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class LastRunDetailListQueryService
    {
	    private IQueryable<LastRunDetailList> GetIqueryableList(IQueryable<LastRunDetail> iQueryable)
        {
		IQueryable<LastRunDetailList> query = (from a in iQueryable.Include("LastRunByUser").Include("LastRunByUser.Contact")
											   select new LastRunDetailList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          LastRunDate = a.LastRunDate,
					
					                          LastRunByUserId = a.LastRunByUserId,

											  LastRunByUserName = a.LastRunByUser == null ? null : (a.LastRunByUser.Contact == null ? null : a.LastRunByUser.Contact.EnglishName),

											});
            return query;
		}

		private IQueryable<LastRunDetail> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<LastRunDetail> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<LastRunDetail> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<LastRunDetail> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
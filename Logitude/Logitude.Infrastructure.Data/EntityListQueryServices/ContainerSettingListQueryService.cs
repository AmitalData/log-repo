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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class ContainerSettingListQueryService
    {
	    private IQueryable<ContainerSettingList> GetIqueryableList(IQueryable<ContainerSetting> iQueryable)
        {
		IQueryable<ContainerSettingList> query = (from a in iQueryable
                                            select new ContainerSettingList()
											{

												Id = a.Id,

												Tenant = a.Tenant,

											});
            return query;
		}

		private IQueryable<ContainerSetting> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ContainerSetting> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ContainerSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ContainerSetting> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
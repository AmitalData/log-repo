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

    public partial class DigitalTextCodeListQueryService
    {
	    private IQueryable<DigitalTextCodeList> GetIqueryableList(IQueryable<DigitalTextCode> iQueryable)
        {
			IQueryable<DigitalTextCodeList> query = (from a in iQueryable
												select new DigitalTextCodeList()
												{
												  Id = a.Id,
												  Tenant = a.Tenant,
												  CreateDate = a.CreateDate,
												  UpdateDate = a.UpdateDate,
												  ObjectTableId = a.ObjectTableId,
												  Labels = a.Labels,
		                    					});
            return query;
		}

		private IQueryable<DigitalTextCode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DigitalTextCode> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<DigitalTextCode> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DigitalTextCode> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
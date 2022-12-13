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

    public partial class DigitalProfileListQueryService
    {
	    private IQueryable<DigitalProfileList> GetIqueryableList(IQueryable<DigitalProfile> iQueryable)
        {
		IQueryable<DigitalProfileList> query = (from a in iQueryable
                                            select new DigitalProfileList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<DigitalProfile> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DigitalProfile> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}

		private IQueryable<DigitalProfile> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DigitalProfile> iQueryable, int tenant)
        {
			return iQueryable;
		}		
	}
}
	
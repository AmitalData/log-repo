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

    public partial class DigitalFieldSecurityListQueryService
    {
	    private IQueryable<DigitalFieldSecurityList> GetIqueryableList(IQueryable<DigitalFieldSecurity> iQueryable)
        {
		IQueryable<DigitalFieldSecurityList> query = (from a in iQueryable
                                            select new DigitalFieldSecurityList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          ObjectTableId = a.ObjectTableId,
					
					                          DefaultSettings = a.DefaultSettings,
					
					                          ProfileId = a.ProfileId,
					
		                    	            });
            return query;
		}

		private IQueryable<DigitalFieldSecurity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DigitalFieldSecurity> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<DigitalFieldSecurity> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DigitalFieldSecurity> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
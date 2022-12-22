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

    public partial class DigitalPortalScreenListQueryService
    {
	    private IQueryable<DigitalPortalScreenList> GetIqueryableList(IQueryable<DigitalPortalScreen> iQueryable)
        {
		IQueryable<DigitalPortalScreenList> query = (from a in iQueryable
                                            select new DigitalPortalScreenList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          ObjectTableId = a.ObjectTableId,
					
					                          ScreenCode = a.ScreenCode,
					
					                          Name = a.Name,
					
					                          Content = a.Content,
					
		                    	            });
            return query;
		}

		private IQueryable<DigitalPortalScreen> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DigitalPortalScreen> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<DigitalPortalScreen> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DigitalPortalScreen> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
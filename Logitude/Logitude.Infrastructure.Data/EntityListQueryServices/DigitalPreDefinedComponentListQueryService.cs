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

    public partial class DigitalPreDefinedComponentListQueryService
    {
	    private IQueryable<DigitalPreDefinedComponentList> GetIqueryableList(IQueryable<DigitalPreDefinedComponent> iQueryable)
        {
		IQueryable<DigitalPreDefinedComponentList> query = (from a in iQueryable
                                            select new DigitalPreDefinedComponentList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          Name = a.Name,
					
					                          ObjectTableId = a.ObjectTableId,
					
					                          Content = a.Content,
					
		                    	            });
            return query;
		}

		private IQueryable<DigitalPreDefinedComponent> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DigitalPreDefinedComponent> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<DigitalPreDefinedComponent> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DigitalPreDefinedComponent> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
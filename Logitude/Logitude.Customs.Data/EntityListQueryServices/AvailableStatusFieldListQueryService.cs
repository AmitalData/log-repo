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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class AvailableStatusFieldListQueryService
    {
	    private IQueryable<AvailableStatusFieldList> GetIqueryableList(IQueryable<AvailableStatusField> iQueryable)
        {
		IQueryable<AvailableStatusFieldList> query = (from a in iQueryable
                                            select new AvailableStatusFieldList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          FieldCode = a.FieldCode,
					
					                          IsAvailable = a.IsAvailable,
					
		                    	            });
            return query;
		}

		private IQueryable<AvailableStatusField> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AvailableStatusField> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	
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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class DefaultTypeListQueryService
    {
	    private IQueryable<DefaultTypeList> GetIqueryableList(IQueryable<DefaultType> iQueryable)
        {
		IQueryable<DefaultTypeList> query = (from a in iQueryable
                                            select new DefaultTypeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          Distr = a.Distr,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
		                    	            });
            return query;
		}

		private IQueryable<DefaultType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DefaultType> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	
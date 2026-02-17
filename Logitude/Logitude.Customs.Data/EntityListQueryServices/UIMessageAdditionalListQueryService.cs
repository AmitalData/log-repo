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

    public partial class UIMessageAdditionalListQueryService
    {
	    private IQueryable<UIMessageAdditionalList> GetIqueryableList(IQueryable<UIMessageAdditional> iQueryable)
        {
		IQueryable<UIMessageAdditionalList> query = (from a in iQueryable
                                            select new UIMessageAdditionalList()
											{
                     
					                          Id = a.Id,
					                          Tenant = a.Tenant,
					                          Code = a.Code,
					                          Sort = a.Sort,
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<UIMessageAdditional> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<UIMessageAdditional> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	
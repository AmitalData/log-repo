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

    public partial class DefaultValueListQueryService
    {
	    private IQueryable<DefaultValueList> GetIqueryableList(IQueryable<DefaultValue> iQueryable)
        {
		IQueryable<DefaultValueList> query = (from a in iQueryable
                                            select new DefaultValueList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          DefaultTypeId = a.DefaultTypeId,
					
					                          Distr = a.Distr,
					
					                          BranchId = a.BranchId,
					
					                          CardId = a.CardId,
					
					                          ShortValue = a.ShortValue,

                                              DefValue = a.DefValue,
					
		                    	            });
            return query;
		}

		private IQueryable<DefaultValue> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DefaultValue> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	
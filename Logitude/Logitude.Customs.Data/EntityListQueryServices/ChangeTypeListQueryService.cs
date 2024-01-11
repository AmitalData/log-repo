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

    public partial class ChangeTypeListQueryService
    {
	    private IQueryable<ChangeTypeList> GetIqueryableList(IQueryable<ChangeType> iQueryable)
        {
		IQueryable<ChangeTypeList> query = (from a in iQueryable
                                            select new ChangeTypeList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<ChangeType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ChangeType> iQueryable)
        {
            return iQueryable;
        }
			}


}
	
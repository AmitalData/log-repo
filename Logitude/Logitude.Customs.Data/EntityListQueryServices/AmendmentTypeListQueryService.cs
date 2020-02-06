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

    public partial class AmendmentTypeListQueryService
    {
	    private IQueryable<AmendmentTypeList> GetIqueryableList(IQueryable<AmendmentType> iQueryable)
        {
		IQueryable<AmendmentTypeList> query = (from a in iQueryable
                                            select new AmendmentTypeList()
											{
                     
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<AmendmentType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AmendmentType> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
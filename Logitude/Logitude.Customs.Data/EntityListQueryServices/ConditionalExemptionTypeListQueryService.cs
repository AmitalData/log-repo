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

    public partial class ConditionalExemptionTypeListQueryService
    {
	    private IQueryable<ConditionalExemptionTypeList> GetIqueryableList(IQueryable<ConditionalExemptionType> iQueryable)
        {
		IQueryable<ConditionalExemptionTypeList> query = (from a in iQueryable
                                            select new ConditionalExemptionTypeList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<ConditionalExemptionType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ConditionalExemptionType> iQueryable)
        {
            return iQueryable;
        }
			}


}
	
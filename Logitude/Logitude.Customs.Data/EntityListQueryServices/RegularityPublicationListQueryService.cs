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

    public partial class RegularityPublicationListQueryService
    {
	    private IQueryable<RegularityPublicationList> GetIqueryableList(IQueryable<RegularityPublication> iQueryable)
        {
		IQueryable<RegularityPublicationList> query = (from a in iQueryable
                                            select new RegularityPublicationList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<RegularityPublication> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<RegularityPublication> iQueryable)
        {
            return iQueryable;
        }
			}


}
	
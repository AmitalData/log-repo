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

    public partial class CB_LevyExclusionListQueryService
    {
	    private IQueryable<CB_LevyExclusionList> GetIqueryableList(IQueryable<CB_LevyExclusion> iQueryable)
        {
		IQueryable<CB_LevyExclusionList> query = (from a in iQueryable
                                            select new CB_LevyExclusionList()
											{
                     
					                          ID = a.ID,
					
					                          LevyExclusionNumber = a.LevyExclusionNumber,
					
					                          TradeLevyID = a.TradeLevyID,
					
					                          VendorID = a.VendorID,
					
					                          CountryGroupID = a.CountryGroupID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_LevyExclusion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_LevyExclusion> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
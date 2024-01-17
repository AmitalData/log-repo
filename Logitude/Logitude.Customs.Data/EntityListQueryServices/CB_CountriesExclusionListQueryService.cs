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

    public partial class CB_CountriesExclusionListQueryService
    {
	    private IQueryable<CB_CountriesExclusionList> GetIqueryableList(IQueryable<CB_CountriesExclusion> iQueryable)
        {
		IQueryable<CB_CountriesExclusionList> query = (from a in iQueryable
                                            select new CB_CountriesExclusionList()
											{
                     
					                          ID = a.ID,
					
					                          RegularityRequirementID = a.RegularityRequirementID,
					
					                          CountryID = a.CountryID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CountriesExclusion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CountriesExclusion> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
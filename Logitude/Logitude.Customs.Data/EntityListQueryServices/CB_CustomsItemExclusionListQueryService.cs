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

    public partial class CB_CustomsItemExclusionListQueryService
    {
	    private IQueryable<CB_CustomsItemExclusionList> GetIqueryableList(IQueryable<CB_CustomsItemExclusion> iQueryable)
        {
		IQueryable<CB_CustomsItemExclusionList> query = (from a in iQueryable
                                            select new CB_CustomsItemExclusionList()
											{
                     
					                          ID = a.ID,
					
					                          RegularityRequirementID = a.RegularityRequirementID,
					
					                          CustomsItemID = a.CustomsItemID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsItemExclusion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsItemExclusion> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
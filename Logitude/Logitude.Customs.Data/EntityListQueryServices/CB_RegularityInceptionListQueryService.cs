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

    public partial class CB_RegularityInceptionListQueryService
    {
	    private IQueryable<CB_RegularityInceptionList> GetIqueryableList(IQueryable<CB_RegularityInception> iQueryable)
        {
		IQueryable<CB_RegularityInceptionList> query = (from a in iQueryable
                                            select new CB_RegularityInceptionList()
											{
                     
					                          ID = a.ID,
					
					                          RegularityRequirementID = a.RegularityRequirementID,
					
					                          InterConditionsRelationshipID = a.InterConditionsRelationshipID,
					
					                          IsPersonalImportIncluded = a.IsPersonalImportIncluded,
					
					                          RequirementGoodsDescription = a.RequirementGoodsDescription,
					
					                          RegularityRequirementWarnID = a.RegularityRequirementWarnID,
					
					                          IsCarnetIncluded = a.IsCarnetIncluded,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_RegularityInception> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_RegularityInception> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
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

    public partial class CB_LevyConditionListQueryService
    {
	    private IQueryable<CB_LevyConditionList> GetIqueryableList(IQueryable<CB_LevyCondition> iQueryable)
        {
		IQueryable<CB_LevyConditionList> query = (from a in iQueryable
                                            select new CB_LevyConditionList()
											{
                     
					                          ID = a.ID,
					
					                          LevyConditionNumber = a.LevyConditionNumber,
					
					                          LevyGoodsDescription = a.LevyGoodsDescription,
					
					                          CustomsItemID = a.CustomsItemID,
					
					                          VendorID = a.VendorID,
					
					                          CountryGroupID = a.CountryGroupID,
					
					                          IsCountriesGroup = a.IsCountriesGroup,
					
					                          CountryID = a.CountryID,
					
					                          TradeLevyID = a.TradeLevyID,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_LevyCondition> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_LevyCondition> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
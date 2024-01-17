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

    public partial class CB_RegularityRequirementListQueryService
    {
	    private IQueryable<CB_RegularityRequirementList> GetIqueryableList(IQueryable<CB_RegularityRequirement> iQueryable)
        {
		IQueryable<CB_RegularityRequirementList> query = (from a in iQueryable
                                            select new CB_RegularityRequirementList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          CountryID = a.CountryID,
					
					                          IsAllCountries = a.IsAllCountries,
					
					                          CustomsItemID = a.CustomsItemID,
					
					                          IsAllCustomsItems = a.IsAllCustomsItems,
					
					                          IsLimitedCountryRegularRequire = a.IsLimitedCountryRegularRequire,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          InceptionCodeID = a.InceptionCodeID,
					
					                          RegularityPublicationCodeID = a.RegularityPublicationCodeID,
					
					                          RegularitySourceCodeID = a.RegularitySourceCodeID,
					
					                          CustomsBookTypeID = a.CustomsBookTypeID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_RegularityRequirement> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_RegularityRequirement> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
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

    public partial class CB_RequirementComputedDataListQueryService
    {
	    private IQueryable<CB_RequirementComputedDataList> GetIqueryableList(IQueryable<CB_RequirementComputedData> iQueryable)
        {
		IQueryable<CB_RequirementComputedDataList> query = (from a in iQueryable
                                            select new CB_RequirementComputedDataList()
											{
                     
					                          CB_ID = a.CB_ID,
					
					                          ID = a.ID,
					
					                          RegularityRequirementID = a.RegularityRequirementID,
					
					                          CountryID = a.CountryID,
					
					                          CustomsItemID = a.CustomsItemID,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          RegularitySourceCodeID = a.RegularitySourceCodeID,
					
					                          CreateDate = a.CreateDate,
					
					                          InceptionCodeID = a.InceptionCodeID,
					
					                          RegularityPublicationCodeID = a.RegularityPublicationCodeID,
					
					                          AutonomyCustomsItemID = a.AutonomyCustomsItemID,
					
					                          IsAllCustomsItems = a.IsAllCustomsItems,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_RequirementComputedData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_RequirementComputedData> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
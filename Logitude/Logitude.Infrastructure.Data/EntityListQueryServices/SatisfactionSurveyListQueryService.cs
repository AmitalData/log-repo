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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class SatisfactionSurveyListQueryService
    {
	    private IQueryable<SatisfactionSurveyList> GetIqueryableList(IQueryable<SatisfactionSurvey> iQueryable)
        {
		IQueryable<SatisfactionSurveyList> query = (from a in iQueryable
                                            select new SatisfactionSurveyList()
											{
                     
					                          Id = a.Id,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<SatisfactionSurvey> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SatisfactionSurvey> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<SatisfactionSurvey> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<SatisfactionSurvey> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	
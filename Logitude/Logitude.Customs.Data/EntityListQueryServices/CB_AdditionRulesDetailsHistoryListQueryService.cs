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

    public partial class CB_AdditionRulesDetailsHistoryListQueryService
    {
	    private IQueryable<CB_AdditionRulesDetailsHistoryList> GetIqueryableList(IQueryable<CB_AdditionRulesDetailsHistory> iQueryable)
        {
		IQueryable<CB_AdditionRulesDetailsHistoryList> query = (from a in iQueryable
                                            select new CB_AdditionRulesDetailsHistoryList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          Title = a.Title,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          EntityStatusID = a.EntityStatusID,
					
					                          Rules = a.Rules,
					
					                          EnglishRules = a.EnglishRules,
					
					                          RulesRTF = a.RulesRTF,
					
					                          ChangeRequestTypePriority = a.ChangeRequestTypePriority,
					
					                          CustomsBookAdditionID = a.CustomsBookAdditionID,
					
					                          EnglishRulesRTF = a.EnglishRulesRTF,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_AdditionRulesDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_AdditionRulesDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
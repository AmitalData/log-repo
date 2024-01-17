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

    public partial class CB_CustomsBookAdditionsDetailsHistoryListQueryService
    {
	    private IQueryable<CB_CustomsBookAdditionsDetailsHistoryList> GetIqueryableList(IQueryable<CB_CustomsBookAdditionsDetailsHistory> iQueryable)
        {
		IQueryable<CB_CustomsBookAdditionsDetailsHistoryList> query = (from a in iQueryable
                                            select new CB_CustomsBookAdditionsDetailsHistoryList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          TypeID = a.TypeID,
					
					                          Title = a.Title,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          EntityStatusID = a.EntityStatusID,
					
					                          EnglishTitle = a.EnglishTitle,
					
					                          CustomsBookAdditionID = a.CustomsBookAdditionID,
					
					                          ChangeRequestTypePriority = a.ChangeRequestTypePriority,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsBookAdditionsDetailsHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsBookAdditionsDetailsHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
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

    public partial class CB_TradeAgreementHistoryListQueryService
    {
	    private IQueryable<CB_TradeAgreementHistoryList> GetIqueryableList(IQueryable<CB_TradeAgreementHistory> iQueryable)
        {
		IQueryable<CB_TradeAgreementHistoryList> query = (from a in iQueryable
                                            select new CB_TradeAgreementHistoryList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          EntityStatusID = a.EntityStatusID,
					
					                          Version = a.Version,
					
					                          TradeAgreementID = a.TradeAgreementID,
					
					                          ChangeRequestTypePriority = a.ChangeRequestTypePriority,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_TradeAgreementHistory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_TradeAgreementHistory> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
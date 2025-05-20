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

    public partial class CB_RuleClassificationListQueryService
    {
	    private IQueryable<CB_RuleClassificationList> GetIqueryableList(IQueryable<CB_RuleClassification> iQueryable)
        {
		IQueryable<CB_RuleClassificationList> query = (from a in iQueryable
                                            select new CB_RuleClassificationList()
											{
                     
					                          CB_ID = a.CB_ID,
					
					                          CustomsItemID = a.CustomsItemID,
					
					                          ID = a.ID,
					
					                          CustomsBookType = a.CustomsBookType,
					
					                          Rules = a.Rules,
					
					                          ParentID = a.ParentID,
					
					                          Index = a.Index,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_RuleClassification> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_RuleClassification> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
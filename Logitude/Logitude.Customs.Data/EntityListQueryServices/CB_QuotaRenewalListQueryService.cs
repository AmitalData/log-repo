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

    public partial class CB_QuotaRenewalListQueryService
    {
	    private IQueryable<CB_QuotaRenewalList> GetIqueryableList(IQueryable<CB_QuotaRenewal> iQueryable)
        {
		IQueryable<CB_QuotaRenewalList> query = (from a in iQueryable
                                            select new CB_QuotaRenewalList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          AllocationQuantity = a.AllocationQuantity,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          EntityStatusID = a.EntityStatusID,
					
					                          QuotaDetailsHistoryID = a.QuotaDetailsHistoryID,
					
					                          Step = a.Step,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_QuotaRenewal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_QuotaRenewal> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	
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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class GLAccountFollowUpDataListQueryService
    {
	    private IQueryable<GLAccountFollowUpDataList> GetIqueryableList(IQueryable<GLAccountFollowUpData> iQueryable)
        {
		IQueryable<GLAccountFollowUpDataList> query = (from a in iQueryable
                                            select new GLAccountFollowUpDataList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          FollowUpDate = a.FollowUpDate,
					
					                          FollowUpRemarks = a.FollowUpRemarks,
					
					                          GlAccountId = a.GlAccountId,
					
		                    	            });
            return query;
		}

		private IQueryable<GLAccountFollowUpData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountFollowUpData> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<GLAccountFollowUpData> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountFollowUpData> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
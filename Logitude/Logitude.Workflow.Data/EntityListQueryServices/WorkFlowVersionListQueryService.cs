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

using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityLists;

namespace Logitude.Workflow.Data.EntityListQueryServices
{ 

    public partial class WorkFlowVersionListQueryService
    {
	    private IQueryable<WorkFlowVersionList> GetIqueryableList(IQueryable<WorkFlowVersion> iQueryable)
        {
		IQueryable<WorkFlowVersionList> query = (from a in iQueryable
                                            select new WorkFlowVersionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          VersionNumber = a.VersionNumber,
					
					                          Description = a.Description,
					
					                          StatusCode = a.StatusCode,

										      StatusName = a.Status != null ? a.Status.Name : null,

											  WorkflowId = a.WorkflowId,

											  FlowJson = a.FlowJson,

											 Trigger = a.Trigger,

											 Entity = a.Entity,

											 ActivatedDate = a.ActivatedDate,

											 CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : null : null,

										     UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : null : null

											});
            return query;
		}

		private IQueryable<WorkFlowVersion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WorkFlowVersion> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<WorkFlowVersion> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WorkFlowVersion> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	
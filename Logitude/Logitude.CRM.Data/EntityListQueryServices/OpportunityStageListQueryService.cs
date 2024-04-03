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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class OpportunityStageListQueryService
    {
	    private IQueryable<OpportunityStageList> GetIqueryableList(IQueryable<OpportunityStage> iQueryable)
        {
			throw new NotImplementedException();
		}

		private IQueryable<OpportunityStage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OpportunityStage> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}

        public IQueryable<OpportunityStageList> GetOpportunityStagesByTenant(int tenant)
        {
            IQueryable<OpportunityStageList> query = (from a in context.OpportunityStages.Include("FromStage").Include("ToStage").Include("Opportunity")
                                                      where a.Tenant == tenant
                                                      select new OpportunityStageList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     OpportunityId = a.OpportunityId,
                                                     OpportunityTopic = a.Opportunity != null ? a.Opportunity.Subject : null,
                                                     CountryName = a.Opportunity != null ? (a.Opportunity.Customer != null ? (a.Opportunity.Customer.CountryName) : null) : null,
                                                     LastModifiedDate = a.Opportunity != null ? a.Opportunity.UpdateDate : null,
                                                     CreateDate = a.Opportunity != null ? a.Opportunity.CreateDate : null,
                                                     FromStageId = a.FromStageId,
                                                     FromStageName = a.FromStage != null ? a.FromStage.Name : null,
                                                     ToStageId = a.ToStageId,
                                                     ToStageName = a.ToStage != null ? a.ToStage.Name : null,
                                                     StartDate = a.StartDate,
                                                     EndDate = a.EndDate,
                                                     Customer = a.Opportunity != null ? (a.Opportunity.Customer != null ? a.Opportunity.Customer.EnglishName : null) : null,
                                                     OpportunityType = a.Opportunity != null ? (a.Opportunity.OpportunityType != null ? a.Opportunity.OpportunityType.Name : null) : null,
                                                     OwnerName = a.Opportunity != null ? (a.Opportunity.Owner != null ? a.Opportunity.Owner.Contact.EnglishName : null) : null,
                                                     OwnerId = a.Opportunity != null ? (a.Opportunity.Owner != null ? a.Opportunity.Owner.Id : null) : null,
                                                     LastStageDate = a.Opportunity != null ? a.Opportunity.LastStageDate : null,
                                                     IsCancelled = a.Opportunity == null ? false : a.Opportunity.IsCancelled,
                                                     LeadSourceId = a.Opportunity == null ? null : a.Opportunity.LeadSourceId,
                                                     OpportunityTypeId = a.Opportunity != null ? (a.Opportunity.OpportunityType != null ? a.Opportunity.OpportunityType.Id : null) : null,
                                                     CustomerId = a.Opportunity != null ? a.Opportunity.CustomerId : null,
                                                 });
            return query;
        }

        private IQueryable<OpportunityStage> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<OpportunityStage> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	
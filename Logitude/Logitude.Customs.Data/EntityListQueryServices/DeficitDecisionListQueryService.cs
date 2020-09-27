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

    public partial class DeficitDecisionListQueryService
    {
	    private IQueryable<DeficitDecisionList> GetIqueryableList(IQueryable<DeficitDecision> iQueryable)
        {
		IQueryable<DeficitDecisionList> query = (from a in iQueryable.Include("ApprovedProfession").Include("RequestType").Include("DecisionType")
                                                 select new DeficitDecisionList()
											{
                                                DeclarationId = a.DeclarationId,
                                                DeficitId = a.DeficitId,
                                                Tenant = a.Tenant,
                                                RequestDate = a.RequestDate,
                                                ApprovedProfessionCode = a.ApprovedProfessionCode,
                                                ApprovedProfessionName = a.ApprovedProfession.LocalName != null ? a.ApprovedProfession.LocalName : a.ApprovedProfession.EnglishName,
                                                DecisionCode = a.DecisionCode,
                                                DecisionName = a.DecisionType.LocalName != null ? a.DecisionType.LocalName : a.DecisionType.EnglishName,
                                                DecisionNoteForLetter = a.DecisionNoteForLetter,
                                                RequestID = a.RequestID,
                                                RequestTypeCode = a.RequestTypeCode,
                                                RequestTypeName = a.RequestType.LocalName != null ? a.RequestType.LocalName : a.RequestType.EnglishName,
                                                TotalComponentAmount = a.TotalComponentAmount,
                                                TotalEstimatedAmount = a.TotalEstimatedAmount,
                                                TotalFinancialPenaltyAmount = a.TotalFinancialPenaltyAmount,
                                                TotalInterestAmount = a.TotalInterestAmount,
                                                TotalLinkingAmount = a.TotalLinkingAmount,
                                                
                                                 });
            return query;
		}

		private IQueryable<DeficitDecision> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeficitDecision> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	
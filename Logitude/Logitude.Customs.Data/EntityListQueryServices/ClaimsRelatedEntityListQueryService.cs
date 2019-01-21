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

    public partial class ClaimsRelatedEntityListQueryService
    {
	    private IQueryable<ClaimsRelatedEntityList> GetIqueryableList(IQueryable<ClaimsRelatedEntity> iQueryable)
        {
		IQueryable<ClaimsRelatedEntityList> query = (from a in iQueryable
                                            select new ClaimsRelatedEntityList()
											{
                                                ClaimId = a.ClaimId,
					                            Tenant = a.Tenant,	
			                                    EntityCounterKey = a.EntityCounterKey,
                                                ClaimEntityTypeCode = a.ClaimEntityTypeCode,
                                                ClaimEntityNumber = a.ClaimEntityNumber,
                                                ExternalClaimNumber = a.ExternalClaimNumber,
                                                CourtCode = a.CourtCode,
                                                ProceedingNumber = a.ProceedingNumber,
                                                IsFinancialRefundDemand = a.IsFinancialRefundDemand,
                                                SeconderyClaimEntityCode = a.SeconderyClaimEntityCode,
                                                SeconderyClaimEntityID = a.SeconderyClaimEntityID,
                                                ClaimAmount = a.ClaimAmount,
                                                DeclarationVersion = a.DeclarationVersion,
                                                CommitteeDecisionNumber = a.CommitteeDecisionNumber,
                                                AbandonmentDestructionReferenc = a.AbandonmentDestructionReferenc,
                                                WarehouseTypeCode = a.WarehouseTypeCode,
                                                ClaimExplanation = a.ClaimExplanation,
                                                ContinuousMessagesTypeCode = a.ContinuousMessagesTypeCode,
                                                ClaimRequestNumber = a.ClaimRequestNumber,
                                                CustomsExceptions = a.CustomsExceptions,
					                            TapagNumber = a.TapagNumber,
                                                Numeral = a.Numeral,
                                                CustomsBranchCode = a.CustomsBranchCode,
                                                DecisionCode = a.DecisionCode,
                                                DecisionNote = a.DecisionNote,
                                                EilatVatRefoundDecision = a.EilatVatRefoundDecision,
                                                DepositingAmount = a.DepositingAmount,
                                                RefundAmount = a.RefundAmount,
		                    	            });
            return query;
		}

		private IQueryable<ClaimsRelatedEntity> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimsRelatedEntity> iQueryable, int tenant)
        {
            return iQueryable;
		}

        public List<ClaimsRelatedEntityList> GetClaimsRelatedEntityListsForClaim(string claimId, int tenant)
        {
            List<ClaimsRelatedEntityList> result = (from a in context.ClaimsRelatedEntities.Include("ClaimEntityType")
                                                    where a.ClaimId == claimId && a.Tenant == tenant
                                                    select new ClaimsRelatedEntityList()
                                                    {
                                                        ClaimId = a.ClaimId,
                                                        Tenant = a.Tenant,
                                                        EntityCounterKey = a.EntityCounterKey,
                                                        ClaimEntityTypeCode = a.ClaimEntityTypeCode,
                                                        ClaimEntityNumber = a.ClaimEntityNumber,
                                                        ExternalClaimNumber = a.ExternalClaimNumber,
                                                        CourtCode = a.CourtCode,
                                                        ProceedingNumber = a.ProceedingNumber,
                                                        IsFinancialRefundDemand = a.IsFinancialRefundDemand,
                                                        SeconderyClaimEntityCode = a.SeconderyClaimEntityCode,
                                                        SeconderyClaimEntityID = a.SeconderyClaimEntityID,
                                                        ClaimAmount = a.ClaimAmount,
                                                        DeclarationVersion = a.DeclarationVersion,
                                                        CommitteeDecisionNumber = a.CommitteeDecisionNumber,
                                                        AbandonmentDestructionReferenc = a.AbandonmentDestructionReferenc,
                                                        WarehouseTypeCode = a.WarehouseTypeCode,
                                                        ClaimExplanation = a.ClaimExplanation,
                                                        ContinuousMessagesTypeCode = a.ContinuousMessagesTypeCode,
                                                        ClaimRequestNumber = a.ClaimRequestNumber,
                                                        CustomsExceptions = a.CustomsExceptions,
                                                        TapagNumber = a.TapagNumber,
                                                        Numeral = a.Numeral,
                                                        CustomsBranchCode = a.CustomsBranchCode,
                                                        ClaimEntityTypeName = a.ClaimEntityType.LocalName,
                                                        DecisionCode = a.DecisionCode,
                                                        DecisionNote = a.DecisionNote,
                                                        EilatVatRefoundDecision = a.EilatVatRefoundDecision,
                                                        DepositingAmount = a.DepositingAmount,
                                                        RefundAmount = a.RefundAmount,
                                                    }).ToList();

            return result;
        }
    }


}
	
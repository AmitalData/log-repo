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

    public partial class DeclarationConstraintListQueryService
    {
	    private IQueryable<DeclarationConstraintList> GetIqueryableList(IQueryable<DeclarationConstraint> iQueryable)
        {
            IQueryable<DeclarationConstraintList> query = (from a in iQueryable
                                                           select new DeclarationConstraintList()
                                                    {
                                                      AgentExplanation = a.AgentExplanation,
                                                      ApprovalAuthorityDate = a.ApprovalAuthorityDate,
                                                      ApprovalDecision = a.ApprovalDecision,
                                                      ApprovalNote = a.ApprovalNote,
                                                      ApprovalUserName = a.ApprovalUserName,
                                                      ConstraintNumber = a.ConstraintNumber,
                                                      ConstraintStatusCode = a.ConstraintStatusCode,
                                                      ConstraintStatusName = a.ConstraintStatus != null ? a.ConstraintStatus.LocalName : null,
                                                      ConstraintTypeCode = a.ConstraintTypeCode,
                                                      DeclarationID = a.DeclarationID,
                                                      Tenant = a.Tenant,
                                                      ConstraintTypeName = a.ConstraintProcessType != null? a.ConstraintProcessType.LocalName : null,
                                                      CustomsCollateralId = a.CustomsCollateralId,
                                                      ApprovalDecisionName = a.ConstraintApprovalDecision != null? a.ConstraintApprovalDecision.LocalName : null,


                                                    });
            return query;
		}

        private IQueryable<DeclarationConstraint> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationConstraint> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
	}


}
	
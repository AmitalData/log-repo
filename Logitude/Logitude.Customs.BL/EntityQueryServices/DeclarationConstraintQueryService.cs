using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationConstraintQueryService : EntityQueryService<DeclarationConstraint, DeclarationConstraintKeys, DeclarationConstraintPM, DeclarationPM, DeclarationKeys>
    {


        public List<DeclarationConstraintPM> GetDeclarationConstraintsByDeclrationId(string declarationId, int tenant)
        {

            List<DeclarationConstraint> Constraints;

            Constraints = repository.GetDeclarationConstraintsByDeclarationId(declarationId, tenant);

            List<DeclarationConstraintPM> ConstraintsPMs = new List<DeclarationConstraintPM>();
            DeclarationConstraintDataMapping mapping = new DeclarationConstraintDataMapping();
            foreach (DeclarationConstraint constraint in Constraints)
            {

                DeclarationConstraintPM constraintPM = new DeclarationConstraintPM();
                mapping.CustomPOCOToPM(constraintPM, constraint);
                mapping.POCOToPM(constraintPM, constraint);
                //constraintPM.DeclarationID = constraint.DeclarationID;
                //constraintPM.ConstraintNumber = constraint.ConstraintNumber;
                //constraintPM.AgentExplanation  = constraint.AgentExplanation;
                //constraintPM.ApprovalAuthorityDate = constraint.ApprovalAuthorityDate;
                //constraintPM.ApprovalDecision = constraint.ApprovalDecision;
                //constraintPM.ApprovalNote = constraint.ApprovalNote;
                //constraintPM.ApprovalUserName = constraint.ApprovalUserName;
                //constraintPM.ConstraintStatusCode = constraint.ConstraintStatusCode;
                //constraintPM.ConstraintTypeCode = constraint.ConstraintTypeCode;
                //constraintPM.ConstraintStatusName = constraint.ConstraintStatus == null ? null : constraint.ConstraintStatus.LocalName;
                //constraintPM.ConstraintTypeName = constraint.ConstraintType == null ? null : constraint.ConstraintType.LocalName;

                ConstraintsPMs.Add(constraintPM);
            }
            return ConstraintsPMs;
        }


        public string GetDeclarationConstraintsByConstraintId(string constraint, int tenant)
        {
            if (String.IsNullOrWhiteSpace(constraint)) return null;
            return repository.GetDeclarationConstraintsByConstraintId(constraint, tenant);
        }

        public bool GetDeclarationConstraint(string declarationId, int tenant)
        {
            return repository.DeclarationHasConstraint(declarationId, tenant);
        }
    }
}

 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;


namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationConstraintRepository:IRepository<DeclarationConstraint>
   {
        
		public List<DeclarationConstraint> GetMulti(EntityKeyFields entityKeys)
        {
            DeclarationKeys keys = entityKeys as DeclarationKeys;
            List<DeclarationConstraint> constraints;
            constraints = (from a in context.DeclarationConstraints.Include("ConstraintStatus").Include("ConstraintProcessType")
                           where a.DeclarationID == keys.Id
                           select a).ToList();


            return constraints;
        }

        public List<DeclarationConstraint> GetDeclarationConstraintsByDeclarationId(string declarationId, int tenant)
        {

            List<DeclarationConstraint> constraints;
            constraints = (from a in context.DeclarationConstraints.Include("ConstraintStatus").Include("ConstraintProcessType")
                           where a.DeclarationID == declarationId
                         select a).ToList();


            return constraints;
        }

        public bool DeclarationHasConstraint(string declarationId, int tenant)
        {

            bool hasConstraint  = false;
            DeclarationConstraint constraint = (from a in context.DeclarationConstraints
                                                where a.DeclarationID == declarationId && a.Tenant == tenant
                                                select a).FirstOrDefault();
            if (constraint != null)
            {
                hasConstraint = true;
            }


            return hasConstraint;
        }

        public string GetDeclarationConstraintsByConstraintId(string constraint, int tenant)
        {

            List<DeclarationConstraint> constraints;
            constraints = (from a in context.DeclarationConstraints.Include("ConstraintStatus").Include("ConstraintProcessType")
                           where a.ConstraintNumber == constraint && a.Tenant == tenant
                           select a).ToList();


            if (constraints.Count >= 1)
            {
                return null;
            }

            return constraints.FirstOrDefault().DeclarationID;
        }


        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<DeclarationConstraint>(rec => rec.DeclarationID == entityKeyFields.Id);
        }
   }

}
   
 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationConstraintRepository:IRepository<DeclarationConstraint>
   {
   
        private ICustomContext currentContext;
        public DeclarationConstraintRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationConstraintRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationConstraint GetSingle(string declarationid, string constraintnumber, int tenant)
        {
            return (from a in context.DeclarationConstraints
                    where a.DeclarationID == declarationid && a.ConstraintNumber == constraintnumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationConstraint> GetAll(int tenant)
        {
            return from a in context.DeclarationConstraints  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationConstraint GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationConstraintKeys keys = entityKeys as DeclarationConstraintKeys;
            return (from a in context.DeclarationConstraints
                    where a.DeclarationID == keys.DeclarationID && a.ConstraintNumber == keys.ConstraintNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationConstraint entity)
        {
            onAdd();
            context.DeclarationConstraints.Add(entity);
        }

        public void Remove(DeclarationConstraint entity)
        {
            context.DeclarationConstraints.Attach(entity);
            context.DeclarationConstraints.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationConstraint entity)
        {
            onUpdate();
            context.DeclarationConstraints.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationConstraint> All()
        {
            return context.DeclarationConstraints.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 
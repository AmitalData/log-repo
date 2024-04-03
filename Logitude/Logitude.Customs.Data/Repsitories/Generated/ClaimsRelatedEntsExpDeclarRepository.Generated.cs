 
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
   public partial class ClaimsRelatedEntsExpDeclarRepository:IRepository<ClaimsRelatedEntsExpDeclar>
   {
   
        private ICustomContext currentContext;
        public ClaimsRelatedEntsExpDeclarRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimsRelatedEntsExpDeclarRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimsRelatedEntsExpDeclar GetSingle(string claimid, int counterkey, string exportdeclarationnumber, int tenant)
        {
            return (from a in context.ClaimsRelatedEntsExpDeclars
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.ExportDeclarationNumber == exportdeclarationnumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimsRelatedEntsExpDeclar> GetAll(int tenant)
        {
            return from a in context.ClaimsRelatedEntsExpDeclars  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimsRelatedEntsExpDeclar GetSingle(EntityKeyFields entityKeys)
        {
            ClaimsRelatedEntsExpDeclarKeys keys = entityKeys as ClaimsRelatedEntsExpDeclarKeys;
            return (from a in context.ClaimsRelatedEntsExpDeclars
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.ExportDeclarationNumber == keys.ExportDeclarationNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimsRelatedEntsExpDeclar entity)
        {
            onAdd();
            context.ClaimsRelatedEntsExpDeclars.Add(entity);
        }

        public void Remove(ClaimsRelatedEntsExpDeclar entity)
        {
            context.ClaimsRelatedEntsExpDeclars.Attach(entity);
            context.ClaimsRelatedEntsExpDeclars.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimsRelatedEntsExpDeclar entity)
        {
            onUpdate();
            context.ClaimsRelatedEntsExpDeclars.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimsRelatedEntsExpDeclar> All()
        {
            return context.ClaimsRelatedEntsExpDeclars.ToList();
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
	 
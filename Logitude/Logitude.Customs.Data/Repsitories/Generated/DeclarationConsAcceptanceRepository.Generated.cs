 
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
   public partial class DeclarationConsAcceptanceRepository:IRepository<DeclarationConsAcceptance>
   {
   
        private ICustomContext currentContext;
        public DeclarationConsAcceptanceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationConsAcceptanceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationConsAcceptance GetSingle(string declarationid, int consignmentnumber, int linenumber, int tenant)
        {
            return (from a in context.DeclarationConsAcceptances
                    where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationConsAcceptance> GetAll(int tenant)
        {
            return from a in context.DeclarationConsAcceptances  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationConsAcceptance GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationConsAcceptanceKeys keys = entityKeys as DeclarationConsAcceptanceKeys;
            return (from a in context.DeclarationConsAcceptances
                    where a.DeclarationId == keys.DeclarationId && a.ConsignmentNumber == keys.ConsignmentNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationConsAcceptance entity)
        {
            onAdd();
            context.DeclarationConsAcceptances.Add(entity);
        }

        public void Remove(DeclarationConsAcceptance entity)
        {
            context.DeclarationConsAcceptances.Attach(entity);
            context.DeclarationConsAcceptances.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationConsAcceptance entity)
        {
            onUpdate();
            context.DeclarationConsAcceptances.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationConsAcceptance> All()
        {
            return context.DeclarationConsAcceptances.ToList();
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
	 
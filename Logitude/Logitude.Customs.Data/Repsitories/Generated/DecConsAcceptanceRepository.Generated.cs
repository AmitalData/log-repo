 
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
   public partial class DecConsAcceptanceRepository:IRepository<DecConsAcceptance>
   {
   
        private ICustomContext currentContext;
        public DecConsAcceptanceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecConsAcceptanceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecConsAcceptance GetSingle(string declarationid, int consignmentnumber, int linenumber, int tenant)
        {
            return (from a in context.DecConsAcceptances
                    where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DecConsAcceptance> GetAll(int tenant)
        {
            return from a in context.DecConsAcceptances  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DecConsAcceptance GetSingle(EntityKeyFields entityKeys)
        {
            DecConsAcceptanceKeys keys = entityKeys as DecConsAcceptanceKeys;
            return (from a in context.DecConsAcceptances
                    where a.DeclarationId == keys.DeclarationId && a.ConsignmentNumber == keys.ConsignmentNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecConsAcceptance entity)
        {
            onAdd();
            context.DecConsAcceptances.Add(entity);
        }

        public void Remove(DecConsAcceptance entity)
        {
            context.DecConsAcceptances.Attach(entity);
            context.DecConsAcceptances.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecConsAcceptance entity)
        {
            onUpdate();
            context.DecConsAcceptances.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecConsAcceptance> All()
        {
            return context.DecConsAcceptances.ToList();
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
	 
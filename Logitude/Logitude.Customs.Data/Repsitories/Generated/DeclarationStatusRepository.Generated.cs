 
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
   public partial class DeclarationStatusRepository:IRepository<DeclarationStatus>
   {
   
        private ICustomContext currentContext;
        public DeclarationStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationStatus GetSingle(string declarationid, int linenumber, int tenant)
        {
            return (from a in context.DeclarationStatuses
                    where a.DeclarationId == declarationid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationStatus> GetAll(int tenant)
        {
            return from a in context.DeclarationStatuses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationStatus GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationStatusKeys keys = entityKeys as DeclarationStatusKeys;
            return (from a in context.DeclarationStatuses
                    where a.DeclarationId == keys.DeclarationId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationStatus entity)
        {
            onAdd();
            context.DeclarationStatuses.Add(entity);
        }

        public void Remove(DeclarationStatus entity)
        {
            context.DeclarationStatuses.Attach(entity);
            context.DeclarationStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationStatus entity)
        {
            onUpdate();
            context.DeclarationStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationStatus> All()
        {
            return context.DeclarationStatuses.ToList();
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
	 
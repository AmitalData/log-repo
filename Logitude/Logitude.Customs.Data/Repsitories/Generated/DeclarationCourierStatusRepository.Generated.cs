 
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
   public partial class DeclarationCourierStatusRepository:IRepository<DeclarationCourierStatus>
   {
   
        private ICustomContext currentContext;
        public DeclarationCourierStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationCourierStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationCourierStatus GetSingle(string declarationid, int tenant)
        {
            return (from a in context.DeclarationCourierStatuses
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationCourierStatus> GetAll(int tenant)
        {
            return from a in context.DeclarationCourierStatuses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationCourierStatus GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationCourierStatusKeys keys = entityKeys as DeclarationCourierStatusKeys;
            return (from a in context.DeclarationCourierStatuses
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationCourierStatus entity)
        {
            onAdd();
            context.DeclarationCourierStatuses.Add(entity);
        }

        public void Remove(DeclarationCourierStatus entity)
        {
            context.DeclarationCourierStatuses.Attach(entity);
            context.DeclarationCourierStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationCourierStatus entity)
        {
            onUpdate();
            context.DeclarationCourierStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationCourierStatus> All()
        {
            return context.DeclarationCourierStatuses.ToList();
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
	 
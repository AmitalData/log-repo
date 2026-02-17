 
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
   public partial class CourierDeclarationRepository:IRepository<CourierDeclaration>
   {
   
        private ICustomContext currentContext;
        public CourierDeclarationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierDeclarationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierDeclaration GetSingle(string declarationid, string couriermasterid, int tenant)
        {
            return (from a in context.CourierDeclarations
                    where a.DeclarationId == declarationid && a.CourierMasterId == couriermasterid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierDeclaration> GetAll(int tenant)
        {
            return from a in context.CourierDeclarations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CourierDeclaration GetSingle(EntityKeyFields entityKeys)
        {
            CourierDeclarationKeys keys = entityKeys as CourierDeclarationKeys;
            return (from a in context.CourierDeclarations
                    where a.DeclarationId == keys.DeclarationId && a.CourierMasterId == keys.CourierMasterId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierDeclaration entity)
        {
            onAdd();
            context.CourierDeclarations.Add(entity);
        }

        public void Remove(CourierDeclaration entity)
        {
            context.CourierDeclarations.Attach(entity);
            context.CourierDeclarations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierDeclaration entity)
        {
            onUpdate();
            context.CourierDeclarations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierDeclaration> All()
        {
            return context.CourierDeclarations.ToList();
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
	 
 
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
   public partial class EntitlementTypeRepository:IRepository<EntitlementType>
   {
   
        private ICustomContext currentContext;
        public EntitlementTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public EntitlementTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  EntitlementType GetSingle(string code)
        {
            return (from a in context.EntitlementTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<EntitlementType> GetAll()
        {
            return from a in context.EntitlementTypes  
                   select a;
        }
				 
        public EntitlementType GetSingle(EntityKeyFields entityKeys)
        {
            EntitlementTypeKeys keys = entityKeys as EntitlementTypeKeys;
            return (from a in context.EntitlementTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EntitlementType entity)
        {
            onAdd();
            context.EntitlementTypes.Add(entity);
        }

        public void Remove(EntitlementType entity)
        {
            context.EntitlementTypes.Attach(entity);
            context.EntitlementTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EntitlementType entity)
        {
            onUpdate();
            context.EntitlementTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntitlementType> All()
        {
            return context.EntitlementTypes.ToList();
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
	 
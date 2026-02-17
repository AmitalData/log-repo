 
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
   public partial class CustomsHouseTypeAdditionalRepository:IRepository<CustomsHouseTypeAdditional>
   {
   
        private ICustomContext currentContext;
        public CustomsHouseTypeAdditionalRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsHouseTypeAdditionalRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsHouseTypeAdditional GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsHouseTypeAdditionals
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsHouseTypeAdditional> GetAll(int tenant)
        {
            return from a in context.CustomsHouseTypeAdditionals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsHouseTypeAdditional GetSingle(EntityKeyFields entityKeys)
        {
            CustomsHouseTypeAdditionalKeys keys = entityKeys as CustomsHouseTypeAdditionalKeys;
            return (from a in context.CustomsHouseTypeAdditionals
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsHouseTypeAdditional entity)
        {
            onAdd();
            context.CustomsHouseTypeAdditionals.Add(entity);
        }

        public void Remove(CustomsHouseTypeAdditional entity)
        {
            context.CustomsHouseTypeAdditionals.Attach(entity);
            context.CustomsHouseTypeAdditionals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsHouseTypeAdditional entity)
        {
            onUpdate();
            context.CustomsHouseTypeAdditionals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsHouseTypeAdditional> All()
        {
            return context.CustomsHouseTypeAdditionals.ToList();
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
	 
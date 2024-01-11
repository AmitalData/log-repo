 
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
   public partial class CustomsGeneralRepository:IRepository<CustomsGeneral>
   {
   
        private ICustomContext currentContext;
        public CustomsGeneralRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsGeneralRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsGeneral GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsGenerals
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsGeneral> GetAll(int tenant)
        {
            return from a in context.CustomsGenerals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsGeneral GetSingle(EntityKeyFields entityKeys)
        {
            CustomsGeneralKeys keys = entityKeys as CustomsGeneralKeys;
            return (from a in context.CustomsGenerals
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsGeneral entity)
        {
            onAdd();
            context.CustomsGenerals.Add(entity);
        }

        public void Remove(CustomsGeneral entity)
        {
            context.CustomsGenerals.Attach(entity);
            context.CustomsGenerals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsGeneral entity)
        {
            onUpdate();
            context.CustomsGenerals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsGeneral> All()
        {
            return context.CustomsGenerals.ToList();
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
	 
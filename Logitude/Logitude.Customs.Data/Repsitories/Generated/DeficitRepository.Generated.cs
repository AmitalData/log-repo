 
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
   public partial class DeficitRepository:IRepository<Deficit>
   {
   
        private ICustomContext currentContext;
        public DeficitRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeficitRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Deficit GetSingle(string id, int tenant)
        {
            return (from a in context.Deficits
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Deficit> GetAll(int tenant)
        {
            return from a in context.Deficits  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Deficit GetSingle(EntityKeyFields entityKeys)
        {
            DeficitKeys keys = entityKeys as DeficitKeys;
            return (from a in context.Deficits
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Deficit entity)
        {
            onAdd();
            context.Deficits.Add(entity);
        }

        public void Remove(Deficit entity)
        {
            context.Deficits.Attach(entity);
            context.Deficits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Deficit entity)
        {
            onUpdate();
            context.Deficits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Deficit> All()
        {
            return context.Deficits.ToList();
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
	 
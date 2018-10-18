 
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
   public partial class TapagRepository:IRepository<Tapag>
   {
   
        private ICustomContext currentContext;
        public TapagRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TapagRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Tapag GetSingle(string id, int tenant)
        {
            return (from a in context.Tapags
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Tapag> GetAll(int tenant)
        {
            return from a in context.Tapags  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Tapag GetSingle(EntityKeyFields entityKeys)
        {
            TapagKeys keys = entityKeys as TapagKeys;
            return (from a in context.Tapags
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Tapag entity)
        {
            onAdd();
            context.Tapags.Add(entity);
        }

        public void Remove(Tapag entity)
        {
            context.Tapags.Attach(entity);
            context.Tapags.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Tapag entity)
        {
            onUpdate();
            context.Tapags.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Tapag> All()
        {
            return context.Tapags.ToList();
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
	 
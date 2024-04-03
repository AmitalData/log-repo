 
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
   public partial class GenderRepository:IRepository<Gender>
   {
   
        private ICustomContext currentContext;
        public GenderRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GenderRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Gender GetSingle(string code)
        {
            return (from a in context.Genders
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Gender> GetAll()
        {
            return from a in context.Genders  
                   select a;
        }
				 
        public Gender GetSingle(EntityKeyFields entityKeys)
        {
            GenderKeys keys = entityKeys as GenderKeys;
            return (from a in context.Genders
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Gender entity)
        {
            onAdd();
            context.Genders.Add(entity);
        }

        public void Remove(Gender entity)
        {
            context.Genders.Attach(entity);
            context.Genders.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Gender entity)
        {
            onUpdate();
            context.Genders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Gender> All()
        {
            return context.Genders.ToList();
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
	 
 
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
   public partial class RenewalMethodRepository:IRepository<RenewalMethod>
   {
   
        private ICustomContext currentContext;
        public RenewalMethodRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RenewalMethodRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RenewalMethod GetSingle(string code)
        {
            return (from a in context.RenewalMethods
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RenewalMethod> GetAll()
        {
            return from a in context.RenewalMethods  
                   select a;
        }
				 
        public RenewalMethod GetSingle(EntityKeyFields entityKeys)
        {
            RenewalMethodKeys keys = entityKeys as RenewalMethodKeys;
            return (from a in context.RenewalMethods
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RenewalMethod entity)
        {
            onAdd();
            context.RenewalMethods.Add(entity);
        }

        public void Remove(RenewalMethod entity)
        {
            context.RenewalMethods.Attach(entity);
            context.RenewalMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RenewalMethod entity)
        {
            onUpdate();
            context.RenewalMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RenewalMethod> All()
        {
            return context.RenewalMethods.ToList();
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
	 
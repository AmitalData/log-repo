 
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
   public partial class GuaranteeRepository:IRepository<Guarantee>
   {
   
        private ICustomContext currentContext;
        public GuaranteeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GuaranteeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Guarantee GetSingle(string id, int tenant)
        {
            return (from a in context.Guarantees
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Guarantee> GetAll(int tenant)
        {
            return from a in context.Guarantees  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Guarantee GetSingle(EntityKeyFields entityKeys)
        {
            GuaranteeKeys keys = entityKeys as GuaranteeKeys;
            return (from a in context.Guarantees
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Guarantee entity)
        {
            onAdd();
            context.Guarantees.Add(entity);
        }

        public void Remove(Guarantee entity)
        {
            context.Guarantees.Attach(entity);
            context.Guarantees.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Guarantee entity)
        {
            onUpdate();
            context.Guarantees.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Guarantee> All()
        {
            return context.Guarantees.ToList();
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
	 
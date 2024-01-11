 
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
   public partial class QuotaIncrementRepository:IRepository<QuotaIncrement>
   {
   
        private ICustomContext currentContext;
        public QuotaIncrementRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public QuotaIncrementRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuotaIncrement GetSingle(string code)
        {
            return (from a in context.QuotaIncrements
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<QuotaIncrement> GetAll()
        {
            return from a in context.QuotaIncrements  
                   select a;
        }
				 
        public QuotaIncrement GetSingle(EntityKeyFields entityKeys)
        {
            QuotaIncrementKeys keys = entityKeys as QuotaIncrementKeys;
            return (from a in context.QuotaIncrements
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuotaIncrement entity)
        {
            onAdd();
            context.QuotaIncrements.Add(entity);
        }

        public void Remove(QuotaIncrement entity)
        {
            context.QuotaIncrements.Attach(entity);
            context.QuotaIncrements.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuotaIncrement entity)
        {
            onUpdate();
            context.QuotaIncrements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuotaIncrement> All()
        {
            return context.QuotaIncrements.ToList();
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
	 
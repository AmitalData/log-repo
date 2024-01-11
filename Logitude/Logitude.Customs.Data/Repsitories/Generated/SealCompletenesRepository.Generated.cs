 
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
   public partial class SealCompletenesRepository:IRepository<SealCompletenes>
   {
   
        private ICustomContext currentContext;
        public SealCompletenesRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SealCompletenesRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SealCompletenes GetSingle(string code)
        {
            return (from a in context.SealCompleteness
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SealCompletenes> GetAll()
        {
            return from a in context.SealCompleteness  
                   select a;
        }
				 
        public SealCompletenes GetSingle(EntityKeyFields entityKeys)
        {
            SealCompletenesKeys keys = entityKeys as SealCompletenesKeys;
            return (from a in context.SealCompleteness
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SealCompletenes entity)
        {
            onAdd();
            context.SealCompleteness.Add(entity);
        }

        public void Remove(SealCompletenes entity)
        {
            context.SealCompleteness.Attach(entity);
            context.SealCompleteness.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SealCompletenes entity)
        {
            onUpdate();
            context.SealCompleteness.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SealCompletenes> All()
        {
            return context.SealCompleteness.ToList();
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
	 
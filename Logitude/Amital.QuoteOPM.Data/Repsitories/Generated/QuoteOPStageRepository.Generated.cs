 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class QuoteOPStageRepository:IRepository<QuoteOPStage>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPStageRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPStageRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPStage GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPStages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPStage> GetAll(int tenant)
        {
            return from a in context.QuoteOPStages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPStage GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPStageKeys keys = entityKeys as QuoteOPStageKeys;
            return (from a in context.QuoteOPStages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPStage entity)
        {
            onAdd();
            context.QuoteOPStages.Add(entity);
        }

        public void Remove(QuoteOPStage entity)
        {
            context.QuoteOPStages.Attach(entity);
            context.QuoteOPStages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPStage entity)
        {
            onUpdate();
            context.QuoteOPStages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPStage> All()
        {
            return context.QuoteOPStages.ToList();
        }

        private IQuoteOPMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 
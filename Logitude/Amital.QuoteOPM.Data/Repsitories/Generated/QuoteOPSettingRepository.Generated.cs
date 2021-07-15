 
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
   public partial class QuoteOPSettingRepository:IRepository<QuoteOPSetting>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPSettingRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPSettingRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPSetting GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPSetting> GetAll(int tenant)
        {
            return from a in context.QuoteOPSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPSetting GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPSettingKeys keys = entityKeys as QuoteOPSettingKeys;
            return (from a in context.QuoteOPSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPSetting entity)
        {
            onAdd();
            context.QuoteOPSettings.Add(entity);
        }

        public void Remove(QuoteOPSetting entity)
        {
            context.QuoteOPSettings.Attach(entity);
            context.QuoteOPSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPSetting entity)
        {
            onUpdate();
            context.QuoteOPSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPSetting> All()
        {
            return context.QuoteOPSettings.ToList();
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
	 
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class DigitalPortalLanguageRepository:IRepository<DigitalPortalLanguage>
   {
   
        private IInfrastructureContext currentContext;
        public DigitalPortalLanguageRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalPortalLanguageRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  DigitalPortalLanguage GetSingle(string code)
        {
            return (from a in context.DigitalPortalLanguages
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DigitalPortalLanguage> GetAll()
        {
            return from a in context.DigitalPortalLanguages  
                   select a;
        }
				 
        public DigitalPortalLanguage GetSingle(EntityKeyFields entityKeys)
        {
            DigitalPortalLanguageKeys keys = entityKeys as DigitalPortalLanguageKeys;
            return (from a in context.DigitalPortalLanguages
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DigitalPortalLanguage entity)
        {
            onAdd();
            context.DigitalPortalLanguages.Add(entity);
        }

        public void Remove(DigitalPortalLanguage entity)
        {
            context.DigitalPortalLanguages.Attach(entity);
            context.DigitalPortalLanguages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DigitalPortalLanguage entity)
        {
            onUpdate();
            context.DigitalPortalLanguages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DigitalPortalLanguage> All()
        {
            return context.DigitalPortalLanguages.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 
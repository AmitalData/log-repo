 
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
   public partial class QuoteOPTemplateSectionTypeRepository:IRepository<QuoteOPTemplateSectionType>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPTemplateSectionTypeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPTemplateSectionTypeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPTemplateSectionType GetSingle(string code)
        {
            return (from a in context.QuoteOPTemplateSectionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPTemplateSectionType> GetAll()
        {
            return from a in context.QuoteOPTemplateSectionTypes  
                   select a;
        }
				 
        public QuoteOPTemplateSectionType GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPTemplateSectionTypeKeys keys = entityKeys as QuoteOPTemplateSectionTypeKeys;
            return (from a in context.QuoteOPTemplateSectionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPTemplateSectionType entity)
        {
            onAdd();
            context.QuoteOPTemplateSectionTypes.Add(entity);
        }

        public void Remove(QuoteOPTemplateSectionType entity)
        {
            context.QuoteOPTemplateSectionTypes.Attach(entity);
            context.QuoteOPTemplateSectionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPTemplateSectionType entity)
        {
            onUpdate();
            context.QuoteOPTemplateSectionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPTemplateSectionType> All()
        {
            return context.QuoteOPTemplateSectionTypes.ToList();
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
	 
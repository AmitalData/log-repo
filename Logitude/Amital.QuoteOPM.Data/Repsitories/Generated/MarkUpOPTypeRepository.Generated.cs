 
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
   public partial class MarkUpOPTypeRepository:IRepository<MarkUpOPType>
   {
   
        private IQuoteOPMContext currentContext;
        public MarkUpOPTypeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public MarkUpOPTypeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  MarkUpOPType GetSingle(string code)
        {
            return (from a in context.MarkUpOPTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MarkUpOPType> GetAll()
        {
            return from a in context.MarkUpOPTypes  
                   select a;
        }
				 
        public MarkUpOPType GetSingle(EntityKeyFields entityKeys)
        {
            MarkUpOPTypeKeys keys = entityKeys as MarkUpOPTypeKeys;
            return (from a in context.MarkUpOPTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MarkUpOPType entity)
        {
            onAdd();
            context.MarkUpOPTypes.Add(entity);
        }

        public void Remove(MarkUpOPType entity)
        {
            context.MarkUpOPTypes.Attach(entity);
            context.MarkUpOPTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MarkUpOPType entity)
        {
            onUpdate();
            context.MarkUpOPTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MarkUpOPType> All()
        {
            return context.MarkUpOPTypes.ToList();
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
	 
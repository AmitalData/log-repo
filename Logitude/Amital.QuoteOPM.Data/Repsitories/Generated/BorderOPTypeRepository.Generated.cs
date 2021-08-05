 
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
   public partial class BorderOPTypeRepository:IRepository<BorderOPType>
   {
   
        private IQuoteOPMContext currentContext;
        public BorderOPTypeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public BorderOPTypeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  BorderOPType GetSingle(string code)
        {
            return (from a in context.BorderOPTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BorderOPType> GetAll()
        {
            return from a in context.BorderOPTypes  
                   select a;
        }
				 
        public BorderOPType GetSingle(EntityKeyFields entityKeys)
        {
            BorderOPTypeKeys keys = entityKeys as BorderOPTypeKeys;
            return (from a in context.BorderOPTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BorderOPType entity)
        {
            onAdd();
            context.BorderOPTypes.Add(entity);
        }

        public void Remove(BorderOPType entity)
        {
            context.BorderOPTypes.Attach(entity);
            context.BorderOPTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BorderOPType entity)
        {
            onUpdate();
            context.BorderOPTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BorderOPType> All()
        {
            return context.BorderOPTypes.ToList();
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
	 
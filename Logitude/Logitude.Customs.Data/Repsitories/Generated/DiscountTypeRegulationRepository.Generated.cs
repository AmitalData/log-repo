 
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
   public partial class DiscountTypeRegulationRepository:IRepository<DiscountTypeRegulation>
   {
   
        private ICustomContext currentContext;
        public DiscountTypeRegulationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DiscountTypeRegulationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DiscountTypeRegulation GetSingle(string code)
        {
            return (from a in context.DiscountTypeRegulations
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DiscountTypeRegulation> GetAll()
        {
            return from a in context.DiscountTypeRegulations  
                   select a;
        }
				 
        public DiscountTypeRegulation GetSingle(EntityKeyFields entityKeys)
        {
            DiscountTypeRegulationKeys keys = entityKeys as DiscountTypeRegulationKeys;
            return (from a in context.DiscountTypeRegulations
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DiscountTypeRegulation entity)
        {
            onAdd();
            context.DiscountTypeRegulations.Add(entity);
        }

        public void Remove(DiscountTypeRegulation entity)
        {
            context.DiscountTypeRegulations.Attach(entity);
            context.DiscountTypeRegulations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DiscountTypeRegulation entity)
        {
            onUpdate();
            context.DiscountTypeRegulations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DiscountTypeRegulation> All()
        {
            return context.DiscountTypeRegulations.ToList();
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
	 
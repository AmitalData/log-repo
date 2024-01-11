 
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
   public partial class ConditionalExemptionTypeRepository:IRepository<ConditionalExemptionType>
   {
   
        private ICustomContext currentContext;
        public ConditionalExemptionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConditionalExemptionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConditionalExemptionType GetSingle(string code)
        {
            return (from a in context.ConditionalExemptionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConditionalExemptionType> GetAll()
        {
            return from a in context.ConditionalExemptionTypes  
                   select a;
        }
				 
        public ConditionalExemptionType GetSingle(EntityKeyFields entityKeys)
        {
            ConditionalExemptionTypeKeys keys = entityKeys as ConditionalExemptionTypeKeys;
            return (from a in context.ConditionalExemptionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConditionalExemptionType entity)
        {
            onAdd();
            context.ConditionalExemptionTypes.Add(entity);
        }

        public void Remove(ConditionalExemptionType entity)
        {
            context.ConditionalExemptionTypes.Attach(entity);
            context.ConditionalExemptionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConditionalExemptionType entity)
        {
            onUpdate();
            context.ConditionalExemptionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConditionalExemptionType> All()
        {
            return context.ConditionalExemptionTypes.ToList();
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
	 
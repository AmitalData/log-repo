 
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
   public partial class SalesTaxExemptionTypeRepository:IRepository<SalesTaxExemptionType>
   {
   
        private ICustomContext currentContext;
        public SalesTaxExemptionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SalesTaxExemptionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SalesTaxExemptionType GetSingle(string code)
        {
            return (from a in context.SalesTaxExemptionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SalesTaxExemptionType> GetAll()
        {
            return from a in context.SalesTaxExemptionTypes  
                   select a;
        }
				 
        public SalesTaxExemptionType GetSingle(EntityKeyFields entityKeys)
        {
            SalesTaxExemptionTypeKeys keys = entityKeys as SalesTaxExemptionTypeKeys;
            return (from a in context.SalesTaxExemptionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SalesTaxExemptionType entity)
        {
            onAdd();
            context.SalesTaxExemptionTypes.Add(entity);
        }

        public void Remove(SalesTaxExemptionType entity)
        {
            context.SalesTaxExemptionTypes.Attach(entity);
            context.SalesTaxExemptionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SalesTaxExemptionType entity)
        {
            onUpdate();
            context.SalesTaxExemptionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SalesTaxExemptionType> All()
        {
            return context.SalesTaxExemptionTypes.ToList();
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
	 
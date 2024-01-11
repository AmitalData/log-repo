 
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
   public partial class CustomsPaymentTermRepository:IRepository<CustomsPaymentTerm>
   {
   
        private ICustomContext currentContext;
        public CustomsPaymentTermRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsPaymentTermRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsPaymentTerm GetSingle(string code)
        {
            return (from a in context.CustomsPaymentTerms
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsPaymentTerm> GetAll()
        {
            return from a in context.CustomsPaymentTerms  
                   select a;
        }
				 
        public CustomsPaymentTerm GetSingle(EntityKeyFields entityKeys)
        {
            CustomsPaymentTermKeys keys = entityKeys as CustomsPaymentTermKeys;
            return (from a in context.CustomsPaymentTerms
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsPaymentTerm entity)
        {
            onAdd();
            context.CustomsPaymentTerms.Add(entity);
        }

        public void Remove(CustomsPaymentTerm entity)
        {
            context.CustomsPaymentTerms.Attach(entity);
            context.CustomsPaymentTerms.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsPaymentTerm entity)
        {
            onUpdate();
            context.CustomsPaymentTerms.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsPaymentTerm> All()
        {
            return context.CustomsPaymentTerms.ToList();
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
	 
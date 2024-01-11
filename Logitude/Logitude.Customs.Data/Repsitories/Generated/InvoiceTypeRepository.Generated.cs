 
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
   public partial class InvoiceTypeRepository:IRepository<InvoiceType>
   {
   
        private ICustomContext currentContext;
        public InvoiceTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InvoiceTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InvoiceType GetSingle(string code)
        {
            return (from a in context.InvoiceTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InvoiceType> GetAll()
        {
            return from a in context.InvoiceTypes  
                   select a;
        }
				 
        public InvoiceType GetSingle(EntityKeyFields entityKeys)
        {
            InvoiceTypeKeys keys = entityKeys as InvoiceTypeKeys;
            return (from a in context.InvoiceTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InvoiceType entity)
        {
            onAdd();
            context.InvoiceTypes.Add(entity);
        }

        public void Remove(InvoiceType entity)
        {
            context.InvoiceTypes.Attach(entity);
            context.InvoiceTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InvoiceType entity)
        {
            onUpdate();
            context.InvoiceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InvoiceType> All()
        {
            return context.InvoiceTypes.ToList();
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
	 
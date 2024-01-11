 
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
   public partial class ProductIdentificationTypeRepository:IRepository<ProductIdentificationType>
   {
   
        private ICustomContext currentContext;
        public ProductIdentificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProductIdentificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProductIdentificationType GetSingle(string code)
        {
            return (from a in context.ProductIdentificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProductIdentificationType> GetAll()
        {
            return from a in context.ProductIdentificationTypes  
                   select a;
        }
				 
        public ProductIdentificationType GetSingle(EntityKeyFields entityKeys)
        {
            ProductIdentificationTypeKeys keys = entityKeys as ProductIdentificationTypeKeys;
            return (from a in context.ProductIdentificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProductIdentificationType entity)
        {
            onAdd();
            context.ProductIdentificationTypes.Add(entity);
        }

        public void Remove(ProductIdentificationType entity)
        {
            context.ProductIdentificationTypes.Attach(entity);
            context.ProductIdentificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProductIdentificationType entity)
        {
            onUpdate();
            context.ProductIdentificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProductIdentificationType> All()
        {
            return context.ProductIdentificationTypes.ToList();
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
	 
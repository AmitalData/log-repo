 
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
   public partial class ProductNameTypeRepository:IRepository<ProductNameType>
   {
   
        private ICustomContext currentContext;
        public ProductNameTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProductNameTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProductNameType GetSingle(string code)
        {
            return (from a in context.ProductNameTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProductNameType> GetAll()
        {
            return from a in context.ProductNameTypes  
                   select a;
        }
				 
        public ProductNameType GetSingle(EntityKeyFields entityKeys)
        {
            ProductNameTypeKeys keys = entityKeys as ProductNameTypeKeys;
            return (from a in context.ProductNameTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProductNameType entity)
        {
            onAdd();
            context.ProductNameTypes.Add(entity);
        }

        public void Remove(ProductNameType entity)
        {
            context.ProductNameTypes.Attach(entity);
            context.ProductNameTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProductNameType entity)
        {
            onUpdate();
            context.ProductNameTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProductNameType> All()
        {
            return context.ProductNameTypes.ToList();
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
	 
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class ProductNameTypeQueryService: EntityQueryService<ProductNameType,ProductNameTypeKeys,ProductNameTypePM,object,ProductNameTypeKeys>
   {
   
        ProductNameTypeRepository repository;
		ICustomContext  context;
        public ProductNameTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ProductNameTypeRepository(context);
            Repository = repository;
            mapping = new ProductNameTypeDataMapping();
        }

        public ProductNameTypeQueryService(ProductNameTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ProductNameTypeDataMapping();
        }

        public ProductNameTypeQueryService(ICustomContext context)
        {
            this.repository = new ProductNameTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ProductNameTypeDataMapping();
        }
		 
		public  ProductNameTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ProductNameTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ProductNameType entityPOCO)
        {
            ProductNameTypeKeys entityKeys = new ProductNameTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
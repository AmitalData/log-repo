 
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
   public partial class ModificationAndDiscountTypeQueryService: EntityQueryService<ModificationAndDiscountType,ModificationAndDiscountTypeKeys,ModificationAndDiscountTypePM,object,ModificationAndDiscountTypeKeys>
   {
   
        ModificationAndDiscountTypeRepository repository;
		ICustomContext  context;
        public ModificationAndDiscountTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ModificationAndDiscountTypeRepository(context);
            Repository = repository;
            mapping = new ModificationAndDiscountTypeDataMapping();
        }

        public ModificationAndDiscountTypeQueryService(ModificationAndDiscountTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ModificationAndDiscountTypeDataMapping();
        }

        public ModificationAndDiscountTypeQueryService(ICustomContext context)
        {
            this.repository = new ModificationAndDiscountTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ModificationAndDiscountTypeDataMapping();
        }
		 
		public  ModificationAndDiscountTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ModificationAndDiscountTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ModificationAndDiscountType entityPOCO)
        {
            ModificationAndDiscountTypeKeys entityKeys = new ModificationAndDiscountTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
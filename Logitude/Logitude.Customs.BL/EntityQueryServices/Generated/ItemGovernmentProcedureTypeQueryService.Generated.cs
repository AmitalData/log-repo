 
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
   public partial class ItemGovernmentProcedureTypeQueryService: EntityQueryService<ItemGovernmentProcedureType,ItemGovernmentProcedureTypeKeys,ItemGovernmentProcedureTypePM,object,ItemGovernmentProcedureTypeKeys>
   {
   
        ItemGovernmentProcedureTypeRepository repository;
		ICustomContext  context;
        public ItemGovernmentProcedureTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ItemGovernmentProcedureTypeRepository(context);
            Repository = repository;
            mapping = new ItemGovernmentProcedureTypeDataMapping();
        }

        public ItemGovernmentProcedureTypeQueryService(ItemGovernmentProcedureTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ItemGovernmentProcedureTypeDataMapping();
        }

        public ItemGovernmentProcedureTypeQueryService(ICustomContext context)
        {
            this.repository = new ItemGovernmentProcedureTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ItemGovernmentProcedureTypeDataMapping();
        }
		 
		public  ItemGovernmentProcedureTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ItemGovernmentProcedureTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ItemGovernmentProcedureType entityPOCO)
        {
            ItemGovernmentProcedureTypeKeys entityKeys = new ItemGovernmentProcedureTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
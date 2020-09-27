 
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
   public partial class AmountTypeQueryService: EntityQueryService<AmountType,AmountTypeKeys,AmountTypePM,object,AmountTypeKeys>
   {
   
        AmountTypeRepository repository;
		ICustomContext  context;
        public AmountTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AmountTypeRepository(context);
            Repository = repository;
            mapping = new AmountTypeDataMapping();
        }

        public AmountTypeQueryService(AmountTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AmountTypeDataMapping();
        }

        public AmountTypeQueryService(ICustomContext context)
        {
            this.repository = new AmountTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AmountTypeDataMapping();
        }
		 
		public  AmountTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AmountTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AmountType entityPOCO)
        {
            AmountTypeKeys entityKeys = new AmountTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
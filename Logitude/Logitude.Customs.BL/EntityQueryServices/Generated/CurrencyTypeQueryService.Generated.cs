 
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
   public partial class CurrencyTypeQueryService: EntityQueryService<CurrencyType,CurrencyTypeKeys,CurrencyTypePM,object,CurrencyTypeKeys>
   {
   
        CurrencyTypeRepository repository;
		ICustomContext  context;
        public CurrencyTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CurrencyTypeRepository(context);
            Repository = repository;
            mapping = new CurrencyTypeDataMapping();
        }

        public CurrencyTypeQueryService(CurrencyTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CurrencyTypeDataMapping();
        }

        public CurrencyTypeQueryService(ICustomContext context)
        {
            this.repository = new CurrencyTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CurrencyTypeDataMapping();
        }
		 
		public  CurrencyTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CurrencyTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CurrencyType entityPOCO)
        {
            CurrencyTypeKeys entityKeys = new CurrencyTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class TapagTypeQueryService: EntityQueryService<TapagType,TapagTypeKeys,TapagTypePM,object,TapagTypeKeys>
   {
   
        TapagTypeRepository repository;
		ICustomContext  context;
        public TapagTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TapagTypeRepository(context);
            Repository = repository;
            mapping = new TapagTypeDataMapping();
        }

        public TapagTypeQueryService(TapagTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TapagTypeDataMapping();
        }

        public TapagTypeQueryService(ICustomContext context)
        {
            this.repository = new TapagTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TapagTypeDataMapping();
        }
		 
		public  TapagTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TapagTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TapagType entityPOCO)
        {
            TapagTypeKeys entityKeys = new TapagTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
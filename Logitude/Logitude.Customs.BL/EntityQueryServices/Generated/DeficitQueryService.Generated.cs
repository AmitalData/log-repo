 
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
   public partial class DeficitQueryService: EntityQueryService<Deficit,DeficitKeys,DeficitPM,object,DeficitKeys>
   {
   
        DeficitRepository repository;
		ICustomContext  context;
        public DeficitQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeficitRepository(context);
            Repository = repository;
            mapping = new DeficitDataMapping();
        }

        public DeficitQueryService(DeficitRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeficitDataMapping();
        }

        public DeficitQueryService(ICustomContext context)
        {
            this.repository = new DeficitRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeficitDataMapping();
        }
		 
		public  DeficitPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeficitKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Deficit entityPOCO)
        {
            DeficitKeys entityKeys = new DeficitKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
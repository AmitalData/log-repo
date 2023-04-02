 
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
   public partial class DefaultTypeQueryService: EntityQueryService<DefaultType,DefaultTypeKeys,DefaultTypePM,object,DefaultTypeKeys>
   {
   
        DefaultTypeRepository repository;
		ICustomContext  context;
        public DefaultTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DefaultTypeRepository(context);
            Repository = repository;
            mapping = new DefaultTypeDataMapping();
        }

        public DefaultTypeQueryService(DefaultTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DefaultTypeDataMapping();
        }

        public DefaultTypeQueryService(ICustomContext context)
        {
            this.repository = new DefaultTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DefaultTypeDataMapping();
        }
		 
		public  DefaultTypePM GetSingle(string id, string code, string distr,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DefaultTypeKeys(){ Id = id, Code = code, Distr = distr };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DefaultType entityPOCO)
        {
            DefaultTypeKeys entityKeys = new DefaultTypeKeys() { Id = entityPOCO.Id, Code = entityPOCO.Code, Distr = entityPOCO.Distr,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
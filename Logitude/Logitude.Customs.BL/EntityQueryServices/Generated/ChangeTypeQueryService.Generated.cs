 
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
   public partial class ChangeTypeQueryService: EntityQueryService<ChangeType,ChangeTypeKeys,ChangeTypePM,object,ChangeTypeKeys>
   {
   
        ChangeTypeRepository repository;
		ICustomContext  context;
        public ChangeTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ChangeTypeRepository(context);
            Repository = repository;
            mapping = new ChangeTypeDataMapping();
        }

        public ChangeTypeQueryService(ChangeTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ChangeTypeDataMapping();
        }

        public ChangeTypeQueryService(ICustomContext context)
        {
            this.repository = new ChangeTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ChangeTypeDataMapping();
        }
		 
		public  ChangeTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ChangeTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ChangeType entityPOCO)
        {
            ChangeTypeKeys entityKeys = new ChangeTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class ReleaseMessageTypeQueryService: EntityQueryService<ReleaseMessageType,ReleaseMessageTypeKeys,ReleaseMessageTypePM,object,ReleaseMessageTypeKeys>
   {
   
        ReleaseMessageTypeRepository repository;
		ICustomContext  context;
        public ReleaseMessageTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ReleaseMessageTypeRepository(context);
            Repository = repository;
            mapping = new ReleaseMessageTypeDataMapping();
        }

        public ReleaseMessageTypeQueryService(ReleaseMessageTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReleaseMessageTypeDataMapping();
        }

        public ReleaseMessageTypeQueryService(ICustomContext context)
        {
            this.repository = new ReleaseMessageTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReleaseMessageTypeDataMapping();
        }
		 
		public  ReleaseMessageTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReleaseMessageTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReleaseMessageType entityPOCO)
        {
            ReleaseMessageTypeKeys entityKeys = new ReleaseMessageTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
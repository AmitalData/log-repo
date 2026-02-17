 
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
   public partial class MorningMessageTypeQueryService: EntityQueryService<MorningMessageType,MorningMessageTypeKeys,MorningMessageTypePM,object,MorningMessageTypeKeys>
   {
   
        MorningMessageTypeRepository repository;
		ICustomContext  context;
        public MorningMessageTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new MorningMessageTypeRepository(context);
            Repository = repository;
            mapping = new MorningMessageTypeDataMapping();
        }

        public MorningMessageTypeQueryService(MorningMessageTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MorningMessageTypeDataMapping();
        }

        public MorningMessageTypeQueryService(ICustomContext context)
        {
            this.repository = new MorningMessageTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MorningMessageTypeDataMapping();
        }
		 
		public  MorningMessageTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MorningMessageTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MorningMessageType entityPOCO)
        {
            MorningMessageTypeKeys entityKeys = new MorningMessageTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
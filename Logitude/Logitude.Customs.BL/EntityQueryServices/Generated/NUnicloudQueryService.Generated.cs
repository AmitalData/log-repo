 
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
   public partial class NUnicloudQueryService: EntityQueryService<NUnicloud,NUnicloudKeys,NUnicloudPM,object,NUnicloudKeys>
   {
   
        NUnicloudRepository repository;
		ICustomContext  context;
        public NUnicloudQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NUnicloudRepository(context);
            Repository = repository;
            mapping = new NUnicloudDataMapping();
        }

        public NUnicloudQueryService(NUnicloudRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NUnicloudDataMapping();
        }

        public NUnicloudQueryService(ICustomContext context)
        {
            this.repository = new NUnicloudRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NUnicloudDataMapping();
        }
		 
		public  NUnicloudPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NUnicloudKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NUnicloud entityPOCO)
        {
            NUnicloudKeys entityKeys = new NUnicloudKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
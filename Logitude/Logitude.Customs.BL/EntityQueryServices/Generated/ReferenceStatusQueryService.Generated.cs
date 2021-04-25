 
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
   public partial class ReferenceStatusQueryService: EntityQueryService<ReferenceStatus,ReferenceStatusKeys,ReferenceStatusPM,object,ReferenceStatusKeys>
   {
   
        ReferenceStatusRepository repository;
		ICustomContext  context;
        public ReferenceStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ReferenceStatusRepository(context);
            Repository = repository;
            mapping = new ReferenceStatusDataMapping();
        }

        public ReferenceStatusQueryService(ReferenceStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ReferenceStatusDataMapping();
        }

        public ReferenceStatusQueryService(ICustomContext context)
        {
            this.repository = new ReferenceStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ReferenceStatusDataMapping();
        }
		 
		public  ReferenceStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ReferenceStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ReferenceStatus entityPOCO)
        {
            ReferenceStatusKeys entityKeys = new ReferenceStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
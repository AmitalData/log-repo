 
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
   public partial class RemarksClassificationQueryService: EntityQueryService<RemarksClassification,RemarksClassificationKeys,RemarksClassificationPM,object,RemarksClassificationKeys>
   {
   
        RemarksClassificationRepository repository;
		ICustomContext  context;
        public RemarksClassificationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RemarksClassificationRepository(context);
            Repository = repository;
            mapping = new RemarksClassificationDataMapping();
        }

        public RemarksClassificationQueryService(RemarksClassificationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RemarksClassificationDataMapping();
        }

        public RemarksClassificationQueryService(ICustomContext context)
        {
            this.repository = new RemarksClassificationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RemarksClassificationDataMapping();
        }
		 
		public  RemarksClassificationPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RemarksClassificationKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RemarksClassification entityPOCO)
        {
            RemarksClassificationKeys entityKeys = new RemarksClassificationKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
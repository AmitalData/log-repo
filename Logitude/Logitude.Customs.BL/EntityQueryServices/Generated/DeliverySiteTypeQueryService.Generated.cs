 
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
   public partial class DeliverySiteTypeQueryService: EntityQueryService<DeliverySiteType,DeliverySiteTypeKeys,DeliverySiteTypePM,object,DeliverySiteTypeKeys>
   {
   
        DeliverySiteTypeRepository repository;
		ICustomContext  context;
        public DeliverySiteTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeliverySiteTypeRepository(context);
            Repository = repository;
            mapping = new DeliverySiteTypeDataMapping();
        }

        public DeliverySiteTypeQueryService(DeliverySiteTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeliverySiteTypeDataMapping();
        }

        public DeliverySiteTypeQueryService(ICustomContext context)
        {
            this.repository = new DeliverySiteTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeliverySiteTypeDataMapping();
        }
		 
		public  DeliverySiteTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeliverySiteTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeliverySiteType entityPOCO)
        {
            DeliverySiteTypeKeys entityKeys = new DeliverySiteTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
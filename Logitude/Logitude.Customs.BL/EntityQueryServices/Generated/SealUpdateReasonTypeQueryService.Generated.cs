 
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
   public partial class SealUpdateReasonTypeQueryService: EntityQueryService<SealUpdateReasonType,SealUpdateReasonTypeKeys,SealUpdateReasonTypePM,object,SealUpdateReasonTypeKeys>
   {
   
        SealUpdateReasonTypeRepository repository;
		ICustomContext  context;
        public SealUpdateReasonTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SealUpdateReasonTypeRepository(context);
            Repository = repository;
            mapping = new SealUpdateReasonTypeDataMapping();
        }

        public SealUpdateReasonTypeQueryService(SealUpdateReasonTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SealUpdateReasonTypeDataMapping();
        }

        public SealUpdateReasonTypeQueryService(ICustomContext context)
        {
            this.repository = new SealUpdateReasonTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SealUpdateReasonTypeDataMapping();
        }
		 
		public  SealUpdateReasonTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SealUpdateReasonTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SealUpdateReasonType entityPOCO)
        {
            SealUpdateReasonTypeKeys entityKeys = new SealUpdateReasonTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
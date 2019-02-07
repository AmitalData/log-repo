 
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
   public partial class UpdateCodeQueryService: EntityQueryService<UpdateCode,UpdateCodeKeys,UpdateCodePM,object,UpdateCodeKeys>
   {
   
        UpdateCodeRepository repository;
		ICustomContext  context;
        public UpdateCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new UpdateCodeRepository(context);
            Repository = repository;
            mapping = new UpdateCodeDataMapping();
        }

        public UpdateCodeQueryService(UpdateCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new UpdateCodeDataMapping();
        }

        public UpdateCodeQueryService(ICustomContext context)
        {
            this.repository = new UpdateCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new UpdateCodeDataMapping();
        }
		 
		public  UpdateCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new UpdateCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(UpdateCode entityPOCO)
        {
            UpdateCodeKeys entityKeys = new UpdateCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
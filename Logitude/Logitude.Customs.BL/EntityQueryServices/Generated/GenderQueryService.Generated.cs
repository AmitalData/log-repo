 
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
   public partial class GenderQueryService: EntityQueryService<Gender,GenderKeys,GenderPM,object,GenderKeys>
   {
   
        GenderRepository repository;
		ICustomContext  context;
        public GenderQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new GenderRepository(context);
            Repository = repository;
            mapping = new GenderDataMapping();
        }

        public GenderQueryService(GenderRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GenderDataMapping();
        }

        public GenderQueryService(ICustomContext context)
        {
            this.repository = new GenderRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GenderDataMapping();
        }
		 
		public  GenderPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GenderKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Gender entityPOCO)
        {
            GenderKeys entityKeys = new GenderKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
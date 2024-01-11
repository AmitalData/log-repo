 
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
   public partial class RenewalMethodQueryService: EntityQueryService<RenewalMethod,RenewalMethodKeys,RenewalMethodPM,object,RenewalMethodKeys>
   {
   
        RenewalMethodRepository repository;
		ICustomContext  context;
        public RenewalMethodQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RenewalMethodRepository(context);
            Repository = repository;
            mapping = new RenewalMethodDataMapping();
        }

        public RenewalMethodQueryService(RenewalMethodRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RenewalMethodDataMapping();
        }

        public RenewalMethodQueryService(ICustomContext context)
        {
            this.repository = new RenewalMethodRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RenewalMethodDataMapping();
        }
		 
		public  RenewalMethodPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RenewalMethodKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RenewalMethod entityPOCO)
        {
            RenewalMethodKeys entityKeys = new RenewalMethodKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class ClaimQueryService: EntityQueryService<Claim,ClaimKeys,ClaimPM,object,ClaimKeys>
   {
   
        ClaimRepository repository;
		ICustomContext  context;
        public ClaimQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClaimRepository(context);
            Repository = repository;
            mapping = new ClaimDataMapping();
        }

        public ClaimQueryService(ClaimRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClaimDataMapping();
        }

        public ClaimQueryService(ICustomContext context)
        {
            this.repository = new ClaimRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClaimDataMapping();
        }
		 
		public  ClaimPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClaimKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Claim entityPOCO)
        {
            ClaimKeys entityKeys = new ClaimKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
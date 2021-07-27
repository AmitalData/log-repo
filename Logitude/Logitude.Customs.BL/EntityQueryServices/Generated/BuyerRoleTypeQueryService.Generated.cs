 
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
   public partial class BuyerRoleTypeQueryService: EntityQueryService<BuyerRoleType,BuyerRoleTypeKeys,BuyerRoleTypePM,object,BuyerRoleTypeKeys>
   {
   
        BuyerRoleTypeRepository repository;
		ICustomContext  context;
        public BuyerRoleTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new BuyerRoleTypeRepository(context);
            Repository = repository;
            mapping = new BuyerRoleTypeDataMapping();
        }

        public BuyerRoleTypeQueryService(BuyerRoleTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BuyerRoleTypeDataMapping();
        }

        public BuyerRoleTypeQueryService(ICustomContext context)
        {
            this.repository = new BuyerRoleTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BuyerRoleTypeDataMapping();
        }
		 
		public  BuyerRoleTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BuyerRoleTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BuyerRoleType entityPOCO)
        {
            BuyerRoleTypeKeys entityKeys = new BuyerRoleTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
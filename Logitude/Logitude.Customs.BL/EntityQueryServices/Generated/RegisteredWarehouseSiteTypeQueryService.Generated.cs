 
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
   public partial class RegisteredWarehouseSiteTypeQueryService: EntityQueryService<RegisteredWarehouseSiteType,RegisteredWarehouseSiteTypeKeys,RegisteredWarehouseSiteTypePM,object,RegisteredWarehouseSiteTypeKeys>
   {
   
        RegisteredWarehouseSiteTypeRepository repository;
		ICustomContext  context;
        public RegisteredWarehouseSiteTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RegisteredWarehouseSiteTypeRepository(context);
            Repository = repository;
            mapping = new RegisteredWarehouseSiteTypeDataMapping();
        }

        public RegisteredWarehouseSiteTypeQueryService(RegisteredWarehouseSiteTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RegisteredWarehouseSiteTypeDataMapping();
        }

        public RegisteredWarehouseSiteTypeQueryService(ICustomContext context)
        {
            this.repository = new RegisteredWarehouseSiteTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RegisteredWarehouseSiteTypeDataMapping();
        }
		 
		public  RegisteredWarehouseSiteTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RegisteredWarehouseSiteTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RegisteredWarehouseSiteType entityPOCO)
        {
            RegisteredWarehouseSiteTypeKeys entityKeys = new RegisteredWarehouseSiteTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
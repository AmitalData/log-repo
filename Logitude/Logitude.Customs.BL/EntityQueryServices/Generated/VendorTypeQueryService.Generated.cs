 
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
   public partial class VendorTypeQueryService: EntityQueryService<VendorType,VendorTypeKeys,VendorTypePM,object,VendorTypeKeys>
   {
   
        VendorTypeRepository repository;
		ICustomContext  context;
        public VendorTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new VendorTypeRepository(context);
            Repository = repository;
            mapping = new VendorTypeDataMapping();
        }

        public VendorTypeQueryService(VendorTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new VendorTypeDataMapping();
        }

        public VendorTypeQueryService(ICustomContext context)
        {
            this.repository = new VendorTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new VendorTypeDataMapping();
        }
		 
		public  VendorTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new VendorTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(VendorType entityPOCO)
        {
            VendorTypeKeys entityKeys = new VendorTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
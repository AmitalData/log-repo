 
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
   public partial class CustomsVendorQueryService: EntityQueryService<CustomsVendor,CustomsVendorKeys,CustomsVendorPM,object,CustomsVendorKeys>
   {
   
        CustomsVendorRepository repository;
		ICustomContext  context;
        public CustomsVendorQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsVendorRepository(context);
            Repository = repository;
            mapping = new CustomsVendorDataMapping();
        }

        public CustomsVendorQueryService(CustomsVendorRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsVendorDataMapping();
        }

        public CustomsVendorQueryService(ICustomContext context)
        {
            this.repository = new CustomsVendorRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsVendorDataMapping();
        }
		 
		public  CustomsVendorPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsVendorKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsVendor entityPOCO)
        {
            CustomsVendorKeys entityKeys = new CustomsVendorKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class CB_VendorQueryService: EntityQueryService<CB_Vendor,CB_VendorKeys,CB_VendorPM,object,CB_VendorKeys>
   {
   
        CB_VendorRepository repository;
		ICustomContext  context;
        public CB_VendorQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_VendorRepository(context);
            Repository = repository;
            mapping = new CB_VendorDataMapping();
        }

        public CB_VendorQueryService(CB_VendorRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_VendorDataMapping();
        }

        public CB_VendorQueryService(ICustomContext context)
        {
            this.repository = new CB_VendorRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_VendorDataMapping();
        }
		 
		public  CB_VendorPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_VendorKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_Vendor entityPOCO)
        {
            CB_VendorKeys entityKeys = new CB_VendorKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
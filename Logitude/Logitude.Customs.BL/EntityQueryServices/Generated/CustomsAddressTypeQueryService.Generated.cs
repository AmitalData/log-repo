 
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
   public partial class CustomsAddressTypeQueryService: EntityQueryService<CustomsAddressType,CustomsAddressTypeKeys,CustomsAddressTypePM,object,CustomsAddressTypeKeys>
   {
   
        CustomsAddressTypeRepository repository;
		ICustomContext  context;
        public CustomsAddressTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsAddressTypeRepository(context);
            Repository = repository;
            mapping = new CustomsAddressTypeDataMapping();
        }

        public CustomsAddressTypeQueryService(CustomsAddressTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsAddressTypeDataMapping();
        }

        public CustomsAddressTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomsAddressTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsAddressTypeDataMapping();
        }
		 
		public  CustomsAddressTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsAddressTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsAddressType entityPOCO)
        {
            CustomsAddressTypeKeys entityKeys = new CustomsAddressTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
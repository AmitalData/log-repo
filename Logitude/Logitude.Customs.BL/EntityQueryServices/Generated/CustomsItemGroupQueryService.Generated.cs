 
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
   public partial class CustomsItemGroupQueryService: EntityQueryService<CustomsItemGroup,CustomsItemGroupKeys,CustomsItemGroupPM,object,CustomsItemGroupKeys>
   {
   
        CustomsItemGroupRepository repository;
		ICustomContext  context;
        public CustomsItemGroupQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsItemGroupRepository(context);
            Repository = repository;
            mapping = new CustomsItemGroupDataMapping();
        }

        public CustomsItemGroupQueryService(CustomsItemGroupRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsItemGroupDataMapping();
        }

        public CustomsItemGroupQueryService(ICustomContext context)
        {
            this.repository = new CustomsItemGroupRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsItemGroupDataMapping();
        }
		 
		public  CustomsItemGroupPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsItemGroupKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsItemGroup entityPOCO)
        {
            CustomsItemGroupKeys entityKeys = new CustomsItemGroupKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
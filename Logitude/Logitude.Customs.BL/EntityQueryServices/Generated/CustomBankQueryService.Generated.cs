 
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
   public partial class CustomBankQueryService: EntityQueryService<CustomBank,CustomBankKeys,CustomBankPM,object,CustomBankKeys>
   {
   
        CustomBankRepository repository;
		ICustomContext  context;
        public CustomBankQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomBankRepository(context);
            Repository = repository;
            mapping = new CustomBankDataMapping();
        }

        public CustomBankQueryService(CustomBankRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomBankDataMapping();
        }

        public CustomBankQueryService(ICustomContext context)
        {
            this.repository = new CustomBankRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomBankDataMapping();
        }
		 
		public  CustomBankPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomBankKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomBank entityPOCO)
        {
            CustomBankKeys entityKeys = new CustomBankKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
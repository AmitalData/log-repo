 
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
   public partial class CustomBanksCardQueryService: EntityQueryService<CustomBanksCard,CustomBanksCardKeys,CustomBanksCardPM,CustomBankPM,CustomBankKeys>
   {
   
        CustomBanksCardRepository repository;
		ICustomContext  context;
        public CustomBanksCardQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomBanksCardRepository(context);
            Repository = repository;
            mapping = new CustomBanksCardDataMapping();
        }

        public CustomBanksCardQueryService(CustomBanksCardRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomBanksCardDataMapping();
        }

        public CustomBanksCardQueryService(ICustomContext context)
        {
            this.repository = new CustomBanksCardRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomBanksCardDataMapping();
        }
		 
		public  CustomBanksCardPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomBanksCardKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomBanksCard entityPOCO)
        {
            CustomBanksCardKeys entityKeys = new CustomBanksCardKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
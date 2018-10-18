 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class FullAccountingSettingQueryService: EntityQueryService<FullAccountingSetting,FullAccountingSettingKeys,FullAccountingSettingPM,object,FullAccountingSettingKeys>
   {
   
        FullAccountingSettingRepository repository;
		IAccountingContext  context;
        public FullAccountingSettingQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new FullAccountingSettingRepository(context);
            Repository = repository;
            mapping = new FullAccountingSettingDataMapping();
        }

        public FullAccountingSettingQueryService(FullAccountingSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FullAccountingSettingDataMapping();
        }

        public FullAccountingSettingQueryService(IAccountingContext context)
        {
            this.repository = new FullAccountingSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FullAccountingSettingDataMapping();
        }
		 
		public  FullAccountingSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FullAccountingSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FullAccountingSetting entityPOCO)
        {
            FullAccountingSettingKeys entityKeys = new FullAccountingSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
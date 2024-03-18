 
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
   public partial class CB_QuotaQueryService: EntityQueryService<CB_Quota,CB_QuotaKeys,CB_QuotaPM,object,CB_QuotaKeys>
   {
   
        CB_QuotaRepository repository;
		ICustomContext  context;
        public CB_QuotaQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_QuotaRepository(context);
            Repository = repository;
            mapping = new CB_QuotaDataMapping();
        }

        public CB_QuotaQueryService(CB_QuotaRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_QuotaDataMapping();
        }

        public CB_QuotaQueryService(ICustomContext context)
        {
            this.repository = new CB_QuotaRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_QuotaDataMapping();
        }
		 
		public  CB_QuotaPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_QuotaKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_Quota entityPOCO)
        {
            CB_QuotaKeys entityKeys = new CB_QuotaKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
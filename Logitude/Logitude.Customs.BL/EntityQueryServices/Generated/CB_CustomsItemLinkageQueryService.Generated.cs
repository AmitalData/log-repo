 
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
   public partial class CB_CustomsItemLinkageQueryService: EntityQueryService<CB_CustomsItemLinkage,CB_CustomsItemLinkageKeys,CB_CustomsItemLinkagePM,object,CB_CustomsItemLinkageKeys>
   {
   
        CB_CustomsItemLinkageRepository repository;
		ICustomContext  context;
        public CB_CustomsItemLinkageQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CustomsItemLinkageRepository(context);
            Repository = repository;
            mapping = new CB_CustomsItemLinkageDataMapping();
        }

        public CB_CustomsItemLinkageQueryService(CB_CustomsItemLinkageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CustomsItemLinkageDataMapping();
        }

        public CB_CustomsItemLinkageQueryService(ICustomContext context)
        {
            this.repository = new CB_CustomsItemLinkageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CustomsItemLinkageDataMapping();
        }
		 
		public  CB_CustomsItemLinkagePM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CustomsItemLinkageKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CustomsItemLinkage entityPOCO)
        {
            CB_CustomsItemLinkageKeys entityKeys = new CB_CustomsItemLinkageKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
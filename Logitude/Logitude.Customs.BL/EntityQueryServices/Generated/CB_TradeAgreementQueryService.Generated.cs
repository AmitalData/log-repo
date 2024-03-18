 
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
   public partial class CB_TradeAgreementQueryService: EntityQueryService<CB_TradeAgreement,CB_TradeAgreementKeys,CB_TradeAgreementPM,object,CB_TradeAgreementKeys>
   {
   
        CB_TradeAgreementRepository repository;
		ICustomContext  context;
        public CB_TradeAgreementQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_TradeAgreementRepository(context);
            Repository = repository;
            mapping = new CB_TradeAgreementDataMapping();
        }

        public CB_TradeAgreementQueryService(CB_TradeAgreementRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_TradeAgreementDataMapping();
        }

        public CB_TradeAgreementQueryService(ICustomContext context)
        {
            this.repository = new CB_TradeAgreementRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_TradeAgreementDataMapping();
        }
		 
		public  CB_TradeAgreementPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_TradeAgreementKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_TradeAgreement entityPOCO)
        {
            CB_TradeAgreementKeys entityKeys = new CB_TradeAgreementKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
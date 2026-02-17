 
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
   public partial class TradeAgreementQueryService: EntityQueryService<TradeAgreement,TradeAgreementKeys,TradeAgreementPM,object,TradeAgreementKeys>
   {
   
        TradeAgreementRepository repository;
		ICustomContext  context;
        public TradeAgreementQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TradeAgreementRepository(context);
            Repository = repository;
            mapping = new TradeAgreementDataMapping();
        }

        public TradeAgreementQueryService(TradeAgreementRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TradeAgreementDataMapping();
        }

        public TradeAgreementQueryService(ICustomContext context)
        {
            this.repository = new TradeAgreementRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TradeAgreementDataMapping();
        }
		 
		public  TradeAgreementPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TradeAgreementKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TradeAgreement entityPOCO)
        {
            TradeAgreementKeys entityKeys = new TradeAgreementKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
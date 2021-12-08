 
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
   public partial class TradeAgreementProtocolQueryService: EntityQueryService<TradeAgreementProtocol,TradeAgreementProtocolKeys,TradeAgreementProtocolPM,object,TradeAgreementProtocolKeys>
   {
   
        TradeAgreementProtocolRepository repository;
		ICustomContext  context;
        public TradeAgreementProtocolQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TradeAgreementProtocolRepository(context);
            Repository = repository;
            mapping = new TradeAgreementProtocolDataMapping();
        }

        public TradeAgreementProtocolQueryService(TradeAgreementProtocolRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TradeAgreementProtocolDataMapping();
        }

        public TradeAgreementProtocolQueryService(ICustomContext context)
        {
            this.repository = new TradeAgreementProtocolRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TradeAgreementProtocolDataMapping();
        }
		 
		public  TradeAgreementProtocolPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TradeAgreementProtocolKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TradeAgreementProtocol entityPOCO)
        {
            TradeAgreementProtocolKeys entityKeys = new TradeAgreementProtocolKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class ClientIndicationQueryService: EntityQueryService<ClientIndication,ClientIndicationKeys,ClientIndicationPM,ClientPM,ClientKeys>
   {
   
        ClientIndicationRepository repository;
		ICustomContext  context;
        public ClientIndicationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClientIndicationRepository(context);
            Repository = repository;
            mapping = new ClientIndicationDataMapping();
        }

        public ClientIndicationQueryService(ClientIndicationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClientIndicationDataMapping();
        }

        public ClientIndicationQueryService(ICustomContext context)
        {
            this.repository = new ClientIndicationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClientIndicationDataMapping();
        }
		 
		public  ClientIndicationPM GetSingle(string indicationid, string clientid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClientIndicationKeys(){ IndicationId = indicationid, ClientId = clientid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClientIndication entityPOCO)
        {
            ClientIndicationKeys entityKeys = new ClientIndicationKeys() { IndicationId = entityPOCO.IndicationId, ClientId = entityPOCO.ClientId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
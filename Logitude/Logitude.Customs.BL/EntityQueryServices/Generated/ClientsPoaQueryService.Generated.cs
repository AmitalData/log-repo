 
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
   public partial class ClientsPoaQueryService: EntityQueryService<ClientsPoa,ClientsPoaKeys,ClientsPoaPM,ClientPM,ClientKeys>
   {
   
        ClientsPoaRepository repository;
		ICustomContext  context;
        public ClientsPoaQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClientsPoaRepository(context);
            Repository = repository;
            mapping = new ClientsPoaDataMapping();
        }

        public ClientsPoaQueryService(ClientsPoaRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClientsPoaDataMapping();
        }

        public ClientsPoaQueryService(ICustomContext context)
        {
            this.repository = new ClientsPoaRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClientsPoaDataMapping();
        }
		 
		public  ClientsPoaPM GetSingle(string id, string clientid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClientsPoaKeys(){ Id = id, ClientId = clientid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClientsPoa entityPOCO)
        {
            ClientsPoaKeys entityKeys = new ClientsPoaKeys() { Id = entityPOCO.Id, ClientId = entityPOCO.ClientId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
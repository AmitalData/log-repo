 
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
   public partial class ClientItemQueryService: EntityQueryService<ClientItem,ClientItemKeys,ClientItemPM,object,ClientItemKeys>
   {
   
        ClientItemRepository repository;
		ICustomContext  context;
        public ClientItemQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClientItemRepository(context);
            Repository = repository;
            mapping = new ClientItemDataMapping();
        }

        public ClientItemQueryService(ClientItemRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClientItemDataMapping();
        }

        public ClientItemQueryService(ICustomContext context)
        {
            this.repository = new ClientItemRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClientItemDataMapping();
        }
		 
		public  ClientItemPM GetSingle(string clientcode, string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClientItemKeys(){ ClientCode = clientcode, Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClientItem entityPOCO)
        {
            ClientItemKeys entityKeys = new ClientItemKeys() { ClientCode = entityPOCO.ClientCode, Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
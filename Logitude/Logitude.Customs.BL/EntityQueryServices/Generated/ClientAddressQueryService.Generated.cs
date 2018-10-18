 
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
   public partial class ClientAddressQueryService: EntityQueryService<ClientAddress,ClientAddressKeys,ClientAddressPM,ClientPM,ClientKeys>
   {
   
        ClientAddressRepository repository;
		ICustomContext  context;
        public ClientAddressQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClientAddressRepository(context);
            Repository = repository;
            mapping = new ClientAddressDataMapping();
        }

        public ClientAddressQueryService(ClientAddressRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClientAddressDataMapping();
        }

        public ClientAddressQueryService(ICustomContext context)
        {
            this.repository = new ClientAddressRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClientAddressDataMapping();
        }
		 
		public  ClientAddressPM GetSingle(string clientid, string addressid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClientAddressKeys(){ ClientId = clientid, AddressId = addressid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClientAddress entityPOCO)
        {
            ClientAddressKeys entityKeys = new ClientAddressKeys() { ClientId = entityPOCO.ClientId, AddressId = entityPOCO.AddressId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
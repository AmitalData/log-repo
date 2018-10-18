 
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
   public partial class ClientsAddressCommTypeQueryService: EntityQueryService<ClientsAddressCommType,ClientsAddressCommTypeKeys,ClientsAddressCommTypePM,ClientAddressPM,ClientAddressKeys>
   {
   
        ClientsAddressCommTypeRepository repository;
		ICustomContext  context;
        public ClientsAddressCommTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClientsAddressCommTypeRepository(context);
            Repository = repository;
            mapping = new ClientsAddressCommTypeDataMapping();
        }

        public ClientsAddressCommTypeQueryService(ClientsAddressCommTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClientsAddressCommTypeDataMapping();
        }

        public ClientsAddressCommTypeQueryService(ICustomContext context)
        {
            this.repository = new ClientsAddressCommTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClientsAddressCommTypeDataMapping();
        }
		 
		public  ClientsAddressCommTypePM GetSingle(string clientid, string addressid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClientsAddressCommTypeKeys(){ ClientId = clientid, AddressId = addressid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClientsAddressCommType entityPOCO)
        {
            ClientsAddressCommTypeKeys entityKeys = new ClientsAddressCommTypeKeys() { ClientId = entityPOCO.ClientId, AddressId = entityPOCO.AddressId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
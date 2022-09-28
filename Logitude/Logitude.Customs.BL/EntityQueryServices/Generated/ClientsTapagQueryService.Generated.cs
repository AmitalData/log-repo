 
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
   public partial class ClientsTapagQueryService: EntityQueryService<ClientsTapag,ClientsTapagKeys,ClientsTapagPM,ClientPM,ClientKeys>
   {
   
        ClientsTapagRepository repository;
		ICustomContext  context;
        public ClientsTapagQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ClientsTapagRepository(context);
            Repository = repository;
            mapping = new ClientsTapagDataMapping();
        }

        public ClientsTapagQueryService(ClientsTapagRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ClientsTapagDataMapping();
        }

        public ClientsTapagQueryService(ICustomContext context)
        {
            this.repository = new ClientsTapagRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ClientsTapagDataMapping();
        }
		 
		public  ClientsTapagPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ClientsTapagKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ClientsTapag entityPOCO)
        {
            ClientsTapagKeys entityKeys = new ClientsTapagKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
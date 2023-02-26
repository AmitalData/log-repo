 
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
   public partial class AddressCurrencyQueryService: EntityQueryService<AddressCurrency,AddressCurrencyKeys,AddressCurrencyPM,object,AddressCurrencyKeys>
   {
   
        AddressCurrencyRepository repository;
		ICustomContext  context;
        public AddressCurrencyQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AddressCurrencyRepository(context);
            Repository = repository;
            mapping = new AddressCurrencyDataMapping();
        }

        public AddressCurrencyQueryService(AddressCurrencyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AddressCurrencyDataMapping();
        }

        public AddressCurrencyQueryService(ICustomContext context)
        {
            this.repository = new AddressCurrencyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AddressCurrencyDataMapping();
        }
		 
		public  AddressCurrencyPM GetSingle(string addressid, string currency,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AddressCurrencyKeys(){ AddressId = addressid, Currency = currency };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AddressCurrency entityPOCO)
        {
            AddressCurrencyKeys entityKeys = new AddressCurrencyKeys() { AddressId = entityPOCO.AddressId, Currency = entityPOCO.Currency,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
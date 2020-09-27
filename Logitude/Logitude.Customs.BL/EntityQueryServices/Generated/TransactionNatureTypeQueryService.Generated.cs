 
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
   public partial class TransactionNatureTypeQueryService: EntityQueryService<TransactionNatureType,TransactionNatureTypeKeys,TransactionNatureTypePM,object,TransactionNatureTypeKeys>
   {
   
        TransactionNatureTypeRepository repository;
		ICustomContext  context;
        public TransactionNatureTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TransactionNatureTypeRepository(context);
            Repository = repository;
            mapping = new TransactionNatureTypeDataMapping();
        }

        public TransactionNatureTypeQueryService(TransactionNatureTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TransactionNatureTypeDataMapping();
        }

        public TransactionNatureTypeQueryService(ICustomContext context)
        {
            this.repository = new TransactionNatureTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TransactionNatureTypeDataMapping();
        }
		 
		public  TransactionNatureTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TransactionNatureTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TransactionNatureType entityPOCO)
        {
            TransactionNatureTypeKeys entityKeys = new TransactionNatureTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
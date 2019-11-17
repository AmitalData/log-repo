 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class ExternalPageAdditionalDataQueryService: EntityQueryService<ExternalPageAdditionalData,ExternalPageAdditionalDataKeys,ExternalPageAdditionalDataPM,object,ExternalPageAdditionalDataKeys>
   {
   
        ExternalPageAdditionalDataRepository repository;
		IAccountingContext  context;
        public ExternalPageAdditionalDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ExternalPageAdditionalDataRepository(context);
            Repository = repository;
            mapping = new ExternalPageAdditionalDataDataMapping();
        }

        public ExternalPageAdditionalDataQueryService(ExternalPageAdditionalDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExternalPageAdditionalDataDataMapping();
        }

        public ExternalPageAdditionalDataQueryService(IAccountingContext context)
        {
            this.repository = new ExternalPageAdditionalDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExternalPageAdditionalDataDataMapping();
        }
		 
		public  ExternalPageAdditionalDataPM GetSingle(string objecttableid, string entityid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExternalPageAdditionalDataKeys(){ ObjectTableId = objecttableid, EntityId = entityid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExternalPageAdditionalData entityPOCO)
        {
            ExternalPageAdditionalDataKeys entityKeys = new ExternalPageAdditionalDataKeys() { ObjectTableId = entityPOCO.ObjectTableId, EntityId = entityPOCO.EntityId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
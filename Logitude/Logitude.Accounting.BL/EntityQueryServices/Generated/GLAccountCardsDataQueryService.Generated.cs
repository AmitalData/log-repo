 
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
   public partial class GLAccountCardsDataQueryService: EntityQueryService<GLAccountCardsData,GLAccountCardsDataKeys,GLAccountCardsDataPM,object,GLAccountCardsDataKeys>
   {
   
        GLAccountCardsDataRepository repository;
		IAccountingContext  context;
        public GLAccountCardsDataQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new GLAccountCardsDataRepository(context);
            Repository = repository;
            mapping = new GLAccountCardsDataDataMapping();
        }

        public GLAccountCardsDataQueryService(GLAccountCardsDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GLAccountCardsDataDataMapping();
        }

        public GLAccountCardsDataQueryService(IAccountingContext context)
        {
            this.repository = new GLAccountCardsDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GLAccountCardsDataDataMapping();
        }
		 
		public  GLAccountCardsDataPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GLAccountCardsDataKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GLAccountCardsData entityPOCO)
        {
            GLAccountCardsDataKeys entityKeys = new GLAccountCardsDataKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
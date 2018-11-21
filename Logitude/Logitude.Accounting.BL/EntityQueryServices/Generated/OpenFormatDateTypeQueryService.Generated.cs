 
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
   public partial class OpenFormatDateTypeQueryService: EntityQueryService<OpenFormatDateType,OpenFormatDateTypeKeys,OpenFormatDateTypePM,object,OpenFormatDateTypeKeys>
   {
   
        OpenFormatDateTypeRepository repository;
		IAccountingContext  context;
        public OpenFormatDateTypeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new OpenFormatDateTypeRepository(context);
            Repository = repository;
            mapping = new OpenFormatDateTypeDataMapping();
        }

        public OpenFormatDateTypeQueryService(OpenFormatDateTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OpenFormatDateTypeDataMapping();
        }

        public OpenFormatDateTypeQueryService(IAccountingContext context)
        {
            this.repository = new OpenFormatDateTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OpenFormatDateTypeDataMapping();
        }
		 
		public  OpenFormatDateTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OpenFormatDateTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OpenFormatDateType entityPOCO)
        {
            OpenFormatDateTypeKeys entityKeys = new OpenFormatDateTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class CopyFromTenant0QueryService: EntityQueryService<CopyFromTenant0,CopyFromTenant0Keys,CopyFromTenant0PM,object,CopyFromTenant0Keys>
   {
   
        CopyFromTenant0Repository repository;
		IAccountingContext  context;
        public CopyFromTenant0QueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new CopyFromTenant0Repository(context);
            Repository = repository;
            mapping = new CopyFromTenant0DataMapping();
        }

        public CopyFromTenant0QueryService(CopyFromTenant0Repository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CopyFromTenant0DataMapping();
        }

        public CopyFromTenant0QueryService(IAccountingContext context)
        {
            this.repository = new CopyFromTenant0Repository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CopyFromTenant0DataMapping();
        }
		 
		public  CopyFromTenant0PM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CopyFromTenant0Keys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CopyFromTenant0 entityPOCO)
        {
            CopyFromTenant0Keys entityKeys = new CopyFromTenant0Keys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
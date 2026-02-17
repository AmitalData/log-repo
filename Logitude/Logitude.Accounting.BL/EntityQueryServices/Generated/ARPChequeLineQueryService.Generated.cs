 
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
using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class ARPChequeLineQueryService: EntityQueryService<ARPChequeLine,ARPChequeLineKeys,ARPChequeLinePM,object,ARPChequeLineKeys>
   {
   
        ARPChequeLineRepository repository;
		IAccountingContext  context;
        public ARPChequeLineQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new ARPChequeLineRepository(context);
            Repository = repository;
            mapping = new ARPChequeLineDataMapping();
        }

        public ARPChequeLineQueryService(ARPChequeLineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ARPChequeLineDataMapping();
        }

        public ARPChequeLineQueryService(IAccountingContext context)
        {
            this.repository = new ARPChequeLineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ARPChequeLineDataMapping();
        }
		 
		public  ARPChequeLinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ARPChequeLineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ARPChequeLine entityPOCO)
        {
            ARPChequeLineKeys entityKeys = new ARPChequeLineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
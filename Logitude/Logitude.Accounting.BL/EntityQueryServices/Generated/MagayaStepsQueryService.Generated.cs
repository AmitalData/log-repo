 
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
   public partial class MagayaStepQueryService: EntityQueryService<MagayaStep,MagayaStepKeys,MagayaStepPM,object,MagayaStepKeys>
   {
   
        MagayaStepRepository repository;
		IAccountingContext  context;
        public MagayaStepQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new MagayaStepRepository(context);
            Repository = repository;
            mapping = new MagayaStepDataMapping();
        }

        public MagayaStepQueryService(MagayaStepRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MagayaStepDataMapping();
        }

        public MagayaStepQueryService(IAccountingContext context)
        {
            this.repository = new MagayaStepRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MagayaStepDataMapping();
        }
		 
		public  MagayaStepPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MagayaStepKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MagayaStep entityPOCO)
        {
            MagayaStepKeys entityKeys = new MagayaStepKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
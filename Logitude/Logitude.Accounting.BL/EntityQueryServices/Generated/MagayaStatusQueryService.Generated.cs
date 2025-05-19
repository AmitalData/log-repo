 
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
   public partial class MagayaStatusQueryService: EntityQueryService<MagayaStatus,MagayaStatusKeys,MagayaStatusPM,object,MagayaStatusKeys>
   {
   
        MagayaStatusRepository repository;
		IAccountingContext  context;
        public MagayaStatusQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new MagayaStatusRepository(context);
            Repository = repository;
            mapping = new MagayaStatusDataMapping();
        }

        public MagayaStatusQueryService(MagayaStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MagayaStatusDataMapping();
        }

        public MagayaStatusQueryService(IAccountingContext context)
        {
            this.repository = new MagayaStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MagayaStatusDataMapping();
        }
		 
		public  MagayaStatusPM GetSingle(string statuscode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MagayaStatusKeys(){ StatusCode = statuscode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MagayaStatus entityPOCO)
        {
            MagayaStatusKeys entityKeys = new MagayaStatusKeys() { StatusCode = entityPOCO.StatusCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
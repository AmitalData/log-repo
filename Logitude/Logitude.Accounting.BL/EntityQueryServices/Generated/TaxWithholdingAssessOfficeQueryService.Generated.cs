 
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
   public partial class TaxWithholdingAssessOfficeQueryService: EntityQueryService<TaxWithholdingAssessOffice,TaxWithholdingAssessOfficeKeys,TaxWithholdingAssessOfficePM,object,TaxWithholdingAssessOfficeKeys>
   {
   
        TaxWithholdingAssessOfficeRepository repository;
		IAccountingContext  context;
        public TaxWithholdingAssessOfficeQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new TaxWithholdingAssessOfficeRepository(context);
            Repository = repository;
            mapping = new TaxWithholdingAssessOfficeDataMapping();
        }

        public TaxWithholdingAssessOfficeQueryService(TaxWithholdingAssessOfficeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaxWithholdingAssessOfficeDataMapping();
        }

        public TaxWithholdingAssessOfficeQueryService(IAccountingContext context)
        {
            this.repository = new TaxWithholdingAssessOfficeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaxWithholdingAssessOfficeDataMapping();
        }
		 
		public  TaxWithholdingAssessOfficePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaxWithholdingAssessOfficeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaxWithholdingAssessOffice entityPOCO)
        {
            TaxWithholdingAssessOfficeKeys entityKeys = new TaxWithholdingAssessOfficeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
 
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
   public partial class Aur_TimesheetQueryService: EntityQueryService<Aur_Timesheet,Aur_TimesheetKeys,Aur_TimesheetPM,Aur_PaymentPM,Aur_PaymentKeys>
   {
   
        Aur_TimesheetRepository repository;
		IAccountingContext  context;
        public Aur_TimesheetQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new Aur_TimesheetRepository(context);
            Repository = repository;
            mapping = new Aur_TimesheetDataMapping();
        }

        public Aur_TimesheetQueryService(Aur_TimesheetRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new Aur_TimesheetDataMapping();
        }

        public Aur_TimesheetQueryService(IAccountingContext context)
        {
            this.repository = new Aur_TimesheetRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new Aur_TimesheetDataMapping();
        }
		 
		public  Aur_TimesheetPM GetSingle(int line, string paymentid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new Aur_TimesheetKeys(){ Line = line, PaymentId = paymentid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Aur_Timesheet entityPOCO)
        {
            Aur_TimesheetKeys entityKeys = new Aur_TimesheetKeys() { Line = entityPOCO.Line, PaymentId = entityPOCO.PaymentId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 
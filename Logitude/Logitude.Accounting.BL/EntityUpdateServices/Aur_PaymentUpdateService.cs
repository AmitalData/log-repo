 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class Aur_PaymentUpdateService:EntityUpdateService<Aur_Payment,Aur_PaymentPM,EntityPM>
   {

        protected override void OnCreating(Aur_PaymentPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Aur_Payment", entityPM.Tenant);
        }

        protected override void UpdateComposition(Aur_PaymentPM entityPM)
        {
            Aur_ItemUpdateService aur_ItemUpdateServiceUpdateService = new Aur_ItemUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            aur_ItemUpdateServiceUpdateService.UpdateMulti(entityPM.Items, entityPM.DeletedItems, entityPM, false);
            
            Aur_PaymentItemUpdateService aur_PaymentItemUpdateServiceUpdateService = new Aur_PaymentItemUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            aur_PaymentItemUpdateServiceUpdateService.UpdateMulti(entityPM.PaymentItem, entityPM.DeletedPaymentItem, entityPM, false);
             
            Aur_TimesheetUpdateService aur_TimesheetUpdateServiceUpdateService = new Aur_TimesheetUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            aur_TimesheetUpdateServiceUpdateService.UpdateMulti(entityPM.TimeSheets, entityPM.DeletedTimeSheets, entityPM, false);


            base.UpdateComposition(entityPM);
        }

    }
   
}
	 
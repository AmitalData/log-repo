 
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
   public partial class Aur_PaymentItemUpdateService:EntityUpdateService<Aur_PaymentItem,Aur_PaymentItemPM,Aur_PaymentPM>
   {

        protected override void OnCreating(Aur_PaymentItemPM entityPM, Aur_PaymentPM entityParentPM)
        {
            entityPM.PaymentId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
        }

    }
   
}
	 
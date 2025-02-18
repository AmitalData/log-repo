using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class Aur_TimesheetUpdateService : EntityUpdateService<Aur_Timesheet, Aur_TimesheetPM, Aur_PaymentPM>
    {
        protected override void OnCreating(Aur_TimesheetPM entityPM, Aur_PaymentPM entityParentPM)
        {
            entityPM.PaymentId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
        }



    }
         

}

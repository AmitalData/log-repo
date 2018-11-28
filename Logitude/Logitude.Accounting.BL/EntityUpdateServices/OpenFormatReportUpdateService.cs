using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class OpenFormatReportUpdateService
    {


        protected override void OnCreating(OpenFormatReportPM entityPM, EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("OpenFormatReport", entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
            entityPM.CreatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
       
            entityPM.ReportNumber= CodeCounter.GetNumber("OpenFormatReport", entityPM.Tenant).ToString(); ;

            entityPM.StatusTypeCode = "1";
            entityPM.UpdateDate = DateTime.Now; ;
         
          
            Validate(entityPM);
        }
    }
}

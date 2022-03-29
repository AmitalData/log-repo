using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class LogisticPermitQueryService : EntityQueryService<LogisticPermit, LogisticPermitKeys, LogisticPermitPM, object, LogisticPermitKeys>
    {
        public LogisticPermitPM GetSinglePM(int CargoIdentifierType, string CargoIdentifierKey1, string CargoIdentifierKey2, string CargoIdentifierKey3, int tenant)
        {
            LogisticPermitPM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.CargoIdentifierType == CargoIdentifierType.ToString() && a.CargoIdentifierKey1 == CargoIdentifierKey1 && a.CargoIdentifierKey2 == CargoIdentifierKey2 && a.CargoIdentifierKey3 == CargoIdentifierKey3 && a.Tenant == tenant
                 select new LogisticPermitPM()
                 {
                     Id = a.Id,
                     TransmitDate = a.TransmitDate,
                     ActionCode = a.ActionCode,
                     Tenant = a.Tenant,
                 }).FirstOrDefault();

            return entityPM;
        }
       
    }

 
}

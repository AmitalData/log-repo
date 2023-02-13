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
using Logitude.Customs.BL.EntityDataMappings;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class LogisticPermitQueryService : EntityQueryService<LogisticPermit, LogisticPermitKeys, LogisticPermitPM, object, LogisticPermitKeys>
    {
        public LogisticPermitPM GetSinglePM(int CargoIdentifierType, string CargoIdentifierKey1, string CargoIdentifierKey2, string CargoIdentifierKey3, int tenant)
        {
            var LogisticPermit = repository.GetSingleByIdentifierKeys(CargoIdentifierType, CargoIdentifierKey1, CargoIdentifierKey2, CargoIdentifierKey3, tenant);
            if(LogisticPermit == null)   return null;
            var LogisticPermitPM = new LogisticPermitPM();
            LogisticPermitDataMapping mapping = new LogisticPermitDataMapping();
            mapping.CustomPOCOToPM(LogisticPermitPM, LogisticPermit);
            mapping.POCOToPM(LogisticPermitPM, LogisticPermit);
            return LogisticPermitPM;
        }
       
    }

 
}

using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IHSMSignFileService
    {
        byte[] SignCustomsRequest(
            int tenant,
           string invocieId,
           byte[] signBytes,
           string FileName,
           string customsAgentId,
           string personalId,
           FullAccountingSettingPM accountingSettings

           );
    }
}

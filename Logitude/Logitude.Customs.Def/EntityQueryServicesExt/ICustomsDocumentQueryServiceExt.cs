using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityQueryServicesExt
{
    public interface ICustomsDocumentQueryServiceExt
    {
        CustomsDocumentPM GetSingle(string id, bool getComposition, bool getFromCache,int tenant);
    }
}

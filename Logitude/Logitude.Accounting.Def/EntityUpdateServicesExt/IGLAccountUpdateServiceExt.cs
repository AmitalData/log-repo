using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IGLAccountUpdateServiceExt
    {
        void Update(GLAccountPM entityPM);
        void Create(GLAccountPM entityPM);
        void UpdateGLAccountWithAdditionalData(string accountId, int tenant,string excludeCardId=null, string excludeContactId = null, string includeContactId = null);
    }
}

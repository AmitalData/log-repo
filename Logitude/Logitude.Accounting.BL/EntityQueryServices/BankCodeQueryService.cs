using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class BankCodeQueryService : EntityQueryService<BankCode, BankCodeKeys, BankCodePM, object, BankCodeKeys>
    {
        public BankCodePM GetSingleByTenant(int tenant)
        {
            BankCode bankCode = this.repository.GetSingleByTenant(tenant);

            if (bankCode != null)
            {
                EntityPM = new BankCodePM();
                mapping.CustomPOCOToPM(EntityPM, bankCode);
                mapping.POCOToPM(EntityPM, bankCode);
            }

            return EntityPM;
        }

        public BankCodePM GetSingleByCode(string code, int tenant)
        {
            BankCode bankCode = this.repository.GetSingleByCode(code, tenant);

            if (bankCode != null)
            {
                EntityPM = new BankCodePM();
                mapping.CustomPOCOToPM(EntityPM, bankCode);
                mapping.POCOToPM(EntityPM, bankCode);
            }

            return EntityPM;
        }
    }
}
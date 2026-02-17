using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL
{
    public static class JournalLinePMExt
    {
        public static MyJournalActionTypeEnum EnsureSettingActionTypeCodeEnum(this JournalLinePM @this)
        {
            if (String.IsNullOrWhiteSpace(@this.ActionTypeCode))
                {
                    if (!String.IsNullOrWhiteSpace(@this.ActionCode))
                    {
                        var journalActionTypeQueryService = new JournalActionTypeQueryService(@this.Tenant);
                        JournalActionTypePM action = journalActionTypeQueryService.GetSingle(@this.ActionCode, false, true);
                        @this.ActionName = action.EnglishName;
                        @this.ActionTypeCode = action.Code;
                    }
                }
                var codeEnum = MyJournalActionTypeEnum.NotValid;
                Enum.TryParse<MyJournalActionTypeEnum>(@this.ActionTypeCode, out codeEnum);
                return codeEnum;
        }
    }
}

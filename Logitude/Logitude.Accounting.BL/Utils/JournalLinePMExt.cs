using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace //Logitude.Accounting.BL.Utils
    Logitude.Accounting.Def.EntityPMs
{
    public static class JournalLinePMExt
    {
        public static JournalActionTypeEnum ActionTypeCodeEnum_Getter(this JournalLinePM @this)
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
            var codeEnum = JournalActionTypeEnum.NotValid;
            Enum.TryParse<JournalActionTypeEnum>(@this.ActionTypeCode, out codeEnum);
            return codeEnum;
        }

        public static void ActionTypeCodeEnum_Setter(this JournalLinePM @this ,  JournalActionTypeEnum value)
        {
            int iVal = (int)value;
            @this.ActionTypeCode = "";
            @this.ActionName = "";
            if (iVal != 0)
            {
                @this.ActionTypeCode = iVal.ToString();
                @this.ActionName = value.ToString();
            }
        }
    }
}

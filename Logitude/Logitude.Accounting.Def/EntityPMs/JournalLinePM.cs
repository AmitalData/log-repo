using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.Validators;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
    public partial class JournalLinePM : EntityPM
    {
        [DataMember]
        public MyJournalActionTypeEnum ActionTypeCodeEnum
        {
            get
            {

                //if (String.IsNullOrWhiteSpace(this.ActionTypeCode))
                //{
                //    if (!String.IsNullOrWhiteSpace(this.ActionCode))
                //    {
                //        var journalActionTypeQueryService = new JournalActionTypeQueryService(this.Tenant);
                //        JournalActionTypePM action = journalActionTypeQueryService.GetSingle(this.ActionCode, false, true);
                //        this.ActionName = action.EnglishName;
                //        this.ActionTypeCode = action.Code;
                //    }
                //}
                var codeEnum = MyJournalActionTypeEnum.NotValid;
                bool dueActionTypeCodeIsNull = true;//on onsert is null !!
                if (dueActionTypeCodeIsNull)
                {
                    if (!string.IsNullOrWhiteSpace(this.ActionCode))
                    {
                        Enum.TryParse<MyJournalActionTypeEnum>(this.ActionCode, out codeEnum);
                    }
                    else 
                    {
                        Enum.TryParse<MyJournalActionTypeEnum>(this.ActionTypeCode, out codeEnum);
                    }
                    
                }
                else
                {
                    Enum.TryParse<MyJournalActionTypeEnum>(this.ActionTypeCode, out codeEnum);
                }
                return codeEnum;
            }
            set
            {
                int iVal = (int)value;
                if (iVal != 0)
                {
                    this.ActionTypeCode = iVal.ToString();
                }
                else
                {
                    this.ActionTypeCode = null;
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);

        }

        public bool EnsureAllDecimalPrecisionIfChangeChangeUpdate()
        {
            var changed = false;
            if (this.LocalAmount != Math.Round(this.LocalAmount, 4))
            {
                this.LocalAmount = Math.Round(this.LocalAmount, 4);
                changed = true;
            }
            if (this.ForeignAmount != Math.Round(this.ForeignAmount, 4))
            {
                this.ForeignAmount = Math.Round(this.ForeignAmount, 4);
                changed = true;
            }

            if (this.exchangeRate.HasValue && this.exchangeRate.Value != Math.Round(this.exchangeRate.Value, 5))
            {
                this.exchangeRate = Math.Round(this.exchangeRate.Value, 5);
                changed = true;
            }
            if (changed)
            {
                if (this.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.None)
                {
                    this.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }
            }
            return changed;
        }


    }
}

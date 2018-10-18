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

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeficitQueryService : EntityQueryService<Deficit, DeficitKeys, DeficitPM, object, DeficitKeys>
    {
        public string GetIdByDebtNotificationNumber(string debtNotificationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(debtNotificationNumber)) return "";
            return repository.GetIdByDebtNotificationNumber(debtNotificationNumber, tenant);
        }

        public DeficitPM GetDeficitByPaymentOrderNumberOrTapagId(string paymentNumber,string tapagId, int tenant)
        {
            DeficitPM deficitPM = null;
            if (!string.IsNullOrWhiteSpace(paymentNumber) || !string.IsNullOrEmpty(tapagId))
            {
               
                DeficitRepository deficitRepository = new DeficitRepository(context);
                Deficit deficit = deficitRepository.GetDeficitByPaymentNumberOrTapagId(paymentNumber,tapagId, tenant);
                if (deficit != null)
                {
                    deficitPM = new DeficitPM()
                    {
                        DebtNotificationNumber = deficit.DebtNotificationNumber,
                        DebtNotificationReason = deficit.DebtNotificationReason,
                        Id = deficit.Id,
                        NotificationTypeCode = deficit.NotificationTypeCode,
                        NotificationTypeName = deficit.DebtNotificationType != null ? deficit.DebtNotificationType.LocalName : null,
                        ProductionDate = deficit.ProductionDate,
                        RealesGoodsDescription = deficit.RealesGoodsDescription,
                        Tenant = deficit.Tenant,
                        ValidityDateTo = deficit.ValidityDateTo,
                        PaymentOrderNumber = deficit.PaymentOrderNumber,
                        TapagId = deficit.TapagId,
                    

                    };
                }
             
            }
            return deficitPM;
        }
    }
}

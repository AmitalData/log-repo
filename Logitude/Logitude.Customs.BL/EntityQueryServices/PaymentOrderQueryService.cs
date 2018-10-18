using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
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
    public partial class PaymentOrderQueryService : EntityQueryService<PaymentOrder, PaymentOrderKeys, PaymentOrderPM, object, PaymentOrderKeys>
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, PaymentOrderPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            PaymentOrderKeys paymentOrderKeys = entityKeys as PaymentOrderKeys;

            PaymentOrderLineQueryService paymentOrderLineQueryService = new PaymentOrderLineQueryService(context);
            entityPM.PaymentOrderLines = paymentOrderLineQueryService.GetMulti(paymentOrderKeys, false);

            PaymentOrderMethodQueryService paymentOrderMethodQueryService = new PaymentOrderMethodQueryService(context);
            entityPM.PaymentOrderMethods = paymentOrderMethodQueryService.GetMulti(paymentOrderKeys, false);

            PaymentOrderProtestReasonQueryService paymentOrderProtestReasonQueryService = new PaymentOrderProtestReasonQueryService(context);
            entityPM.PaymentOrderProtestReasons = paymentOrderProtestReasonQueryService.GetMulti(paymentOrderKeys, false);

            PaymentOrderConnectionTableQueryService paymentOrderConnectionTableQueryService = new PaymentOrderConnectionTableQueryService(context);
            entityPM.PaymentOrderConnectionTables = paymentOrderConnectionTableQueryService.GetMulti(paymentOrderKeys, false);


        }

        public string GetIdByPaymentNumber(string paymentNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(  paymentNumber)) return "";
            return repository.GetIdByPaymentNumber(paymentNumber, tenant);
        }

        public List<PaymentOrderPM> GetPaymentOrderPMsByDeclarationId(string declarationId, int tenant)
        {
            List<PaymentOrder> orders = repository.GetPaymentOrdersByDeclarationId(declarationId, tenant);
            List<PaymentOrderPM> orderPMs = new List<PaymentOrderPM>();
            foreach (PaymentOrder a in orders)
            {
                PaymentOrderPM paymentOrder = new PaymentOrderPM()
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                  PaymentNumber = a.PaymentNumber,
                  CreateDate = a.CreateDate,
                  LastPayDate = a.LastPayDate,
                  CustomerName = a.CustomerCard != null? a.CustomerCard.LocalName : null,
                  PaymentStatusName = a.PaymentStatus != null? a.PaymentStatus.LocalName : null,
                  IsClosed = a.IsClosed,
                  ImporterName = a.Client != null? a.Client.LocalFirstName : null,
                };
                orderPMs.Add(paymentOrder);
            }

            return orderPMs;
        }

    }
}

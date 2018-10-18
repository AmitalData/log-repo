using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class PaymentOrderReplyResponseData : INF_MSG_GenericResponseData
    {
        public string PaymentNumber { get; set; }
        public decimal PaymentOrderTotalSumToPay { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentStatusName { get; set; }
        public int PaymentOrderType { get; set; }
        public string    PaymentOrderTypeName { get; set; }
        public DateTime PaymentOrderPayDate { get; set; }

        public PaymentDetailData PaymentDetailData { get; set; }
        public int PaymentProcess { get; set; }
        public string PaymentProcessName { get; set; }
        public int CustomsHouse { get; set; }
        public string CustomsHouseName { get; set; }
        public string PaymentOrderReason { get; set; }

        public ConnectedEntityData ConnectedEntityData { get; set; }
        public List<TaxParagraphData> TaxParagraphList { get; set; }
        public List<PaymentMethodData> PaymentMethodsList { get; set; }
    }

    public class PaymentDetailData
    {
        public int? CustomerActivityType { get; set; }
        public string CustomerActivityTypeName { get; set; }
        public int? ExternalID { get; set; }
    }

    public class ConnectedEntityData
    {
        public int EntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityIdKey1 { get; set; }
        public string EntityIdKey2 { get; set; }
        public string EntityIdKey3 { get; set; }
    }

    public class TaxParagraphData
    {
        public int ParagraphType { get; set; }
        public string ParagraphTypeName { get; set; }
        public decimal Amount { get; set; }
    }

    public class PaymentMethodData
    {
        public int PaymentMethodType { get; set; }
        public string PaymentMethodTypeName { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethodStatus { get; set; }
        public string PaymentMethodStatusName { get; set; }
    }
}

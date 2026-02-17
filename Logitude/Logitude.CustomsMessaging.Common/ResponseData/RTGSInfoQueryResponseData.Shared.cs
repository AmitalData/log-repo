using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class RTGSInfoQueryResponseData : ResponseDataBase
    {
        public string ActiveInd { get; set; }
        public string CreationDate { get; set; }
        public string GeneralOutputRemarks { get; set; }
        public string QueryDate { get; set; }
        public string QueryTime { get; set; }
        public string RTGSCurrentBalance { get; set; }
        public string RTGSDeposits { get; set; }
        public string RTGSRefund { get; set; }
        public string RTGSUsed { get; set; }

        public List<TransactionResult> TransactionsList { get; set; }

        public class TransactionResult
        {
            public string EntityID { get; set; }
            public string EntityType { get; set; }
            public string PaymentDate { get; set; }
            public string PaymentID { get; set; }
            public string PaymentStatus { get; set; }
            public string RTGSBalance { get; set; }
            public string TransactionAmount { get; set; }
            public string TransactionType { get; set; }
            public string UpdateUser { get; set; }
        }
    }
}

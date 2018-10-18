using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class MasavPaymentsToAgentResponseData : ResponseDataBase
    {
        public List<AgentMasavPaymentResult> AgentMasavPaymentResultList { get; set; }
    }

    //public class AgentMasavPaymentTotal
    //{
    //    public string Bank { get; set; }
    //    public string Branch { get; set; }
    //    public string AccountNumber { get; set; }
    //    public decimal Totalamount { get; set; }
    //    public List<AgentMasavPaymentResult> AgentMasavPaymentList { get; set; }
    //}

    public class AgentMasavPaymentResult
    {
        public string PaymentProcess { get; set; }
        public string PaymentProcessName { get; set; }
        public string PaymentID { get; set; }
        public string Amount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeName { get; set; }
        public string ExternalID { get; set; }
        public string ExternalName { get; set; }
        public string CustomsUnit { get; set; }
        public string PaymentMethodAmount { get; set; }
        public string Bank { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string AgentAccountPosessionX { get; set; }
        public string AgentAccountPosessionV { get; set; }
        public long EntityID { get; set; }
        public string EntityIdExternalReferenceID { get; set; }
        public string BankCode { get; set; } // moran 3.11.15 - Task 16978
        public string AgentMasavPaymentResultHeader { get; set; } //Yuval Chalup 29.02.2016 TASK-19978
        public List<RelatedEntityResult> RelatedEntityList { get; set; }
    }

    public class RelatedEntityResult
    {
        public string EntityIdExternalReferenceID { get; set; }
        public string EntityIdKey1 { get; set; }
        public string EntityIdKey2 { get; set; }
        public string EntityIdKey3 { get; set; }
        public string EntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityPath { get; set; }
    }
}

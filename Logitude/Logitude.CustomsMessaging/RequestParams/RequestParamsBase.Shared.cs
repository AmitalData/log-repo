using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.CustomsMessaging.RequestParams
{
    public enum SendRequestVIA
    {
        Default,// from CustomsMessaging Library
        WebServiceInteractive,
        WebServiceBatch,
        DCABatch
    }
    public abstract class RequestParamsBase
    {
        public bool LoggingEnabled { get; set; }
        public string LoggingObjectTableId { get; set; }
        public string LoggingEntityReference { get; set; }
        public string LoggingEntityId { get; set; }
        public string LoggingUserId { get; set; }
        public bool IsFakeResponse { get; set; }
        public int Tenant { get; set; }
        public string RequestName { get; set; }
        public string ResponseName { get; set; }
        public TestCase TestCase { get; set; }
        public SendRequestVIA RequestVIA { get; set; }

    }

    public class TestCase
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
        public int IndexOrder { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AccountingMessaging.Common.RequestParams
{
    public enum SendRequestVIA
    {
        Default,// from AccountingMessaging Library
        WebServiceInteractive,
        WebServiceBatch,
        DCABatch
    }
    public abstract class RequestParamsBase : INotifyPropertyChanged
    {


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        public RequestParamsBase()
        {
            this.PBId = Guid.NewGuid().ToString();
        }

        public bool LoggingEnabled { get; set; }

        public string LoggingEntityReference { get; set; }
        public string LoggingObjectTableId { get; set; }
        public string LoggingEntityId { get; set; }
        public string LoggingObjectTableId2 { get; set; }
        public string LoggingEntityId2 { get; set; }

        public string LoggingUserId { get; set; }
        public bool IsFakeResponse { get; set; }
        public int Tenant { get; set; }
        public string RequestName { get; set; }
        public string ResponseName { get; set; }
        public TestCase TestCase { get; set; }


        public SendRequestVIA RequestVIA { get; set; }
        public string DCAFileName { get; set; }  
        public string InterfaceTypeCode { get; set; }
        public string MainInterfaceCode { get; set; }
        public string CustomsRequestsSheetId { get; set; }

        public DateTime? TransmitionDateTime { get; set; }
        /// <summary>
        //the eblity to continue work 1 proccess without Split WorkerRole
        // if 1 WorkerRoleName  =SBQueueNames.CustomsMessagingSheetBQ
        //Else WorkerRoleName Stats with CustomsCommand*********WR 
        //and end with WR
        //to Insure Look At Log 
        //currentWR =CustomsCommandGetCustomRequestWR 
        //currentWR =CustomsCommandSendWSReceiveCorrelationWR 
        /// </summary>
        public bool SuppressSplitWR { get; set; }

        public string PBId { get; set; }

        public bool ForcePersonalSign { get; set; }

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

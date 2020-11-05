using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
    [DataContract(Namespace = "")]
    public class AutomatedBackup
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public string ResultCode { get; set; }

        [DataMember]
        public string Description { get; set; }


        [DataMember]
        public int Delaytime { get; set; }


        [DataMember]
        public string DelaytimeOp { get; set; }


        [DataMember]
        public string SelectedDelaytimeFieldCode { get; set; }

        [DataMember]
        public string DelaytimeIndicator { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public bool IsAutomationResultEmailAllActiveUsers { get; set; }

        [DataMember]
        public AutomationFollowUp AutomationFollowUp { get; set; }

        [DataMember]
        public AutomationQueuedTask AutomationQueuedTask { get; set; }

        private List<AutomationCondition> automationConditionLists;
        [DataMember]
        public List<AutomationCondition> AautomationConditionLists
        {
            get
            {
                return this.automationConditionLists;
            }
            set
            {
                this.automationConditionLists = value;
            }
        }

        private List<AutomationSetValue> automationSetValueLists;
        [DataMember]
        public List<AutomationSetValue> AutomationSetValueLists
        {
            get
            {
                return this.automationSetValueLists;
            }
            set
            {
                this.automationSetValueLists = value;
            }
        }


        private List<AutomationCondition> delayAautomationConditionLists;
        [DataMember]
        public List<AutomationCondition> DelayAautomationConditionLists
        {
            get
            {
                return this.delayAautomationConditionLists;
            }
            set
            {
                this.delayAautomationConditionLists = value;
            }
        }

        [DataMember]
        public AutomationSetSLAValue AutomationSetSLAValue { get; set; }


        [DataMember]
        public AutomationSendInterface AutomationSendInterface { get; set; }

    }


    [DataContract(Namespace = "")]
    public class AutomationSetSLAValue
    {
        [DataMember]
        public string SLAId { get; set; }

        [DataMember]
        public string ObjectFieldId { get; set; }

        [DataMember]
        public string ObjectFieldCode { get; set; }

    }


    [DataContract(Namespace = "")]
    public class AutomationSendInterface
    {
       
        [DataMember]
        public string InterfaceName { get; set; }

        [DataMember]
        public string SendVia { get; set; }


        [DataMember]
        public string Format { get; set; }

        [DataMember]
        public string ComputingPartnerId { get; set; }

        [DataMember]
        public FTPAutomationDetails FTPDetails { get; set; }
    }


    [DataContract(Namespace = "")]
    public class FTPAutomationDetails
    {
        [DataMember]
        public string Host { get; set; }
        [DataMember]
        public string Folder { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string Prefix { get; set; }
        [DataMember]
        public string Suffix { get; set; }
        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public bool IsSFTP { get; set; }

    }


}

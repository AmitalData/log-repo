using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.InfrastructureModel.DataContracts
{
    [DataContract(Namespace = "")]
    public class SchedulerDetails
    {
		[DataMember]
		public int Tenant { get; set; }

		[DataMember]
        public FTPSchedulerDetails FTPDetails { get; set; }

        [DataMember]
        public ReportSchedulerDetails ReportDetails { get; set; }
        [DataMember]
        public bool SendIfEmpty { get; set; }

    }

    [DataContract(Namespace = "")]
    public class FTPSchedulerDetails
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

    [DataContract(Namespace = "")]
    public class ReportSchedulerDetails
    {
        [DataMember]
        public string CreatedByUserId { get; set; }
        [DataMember]
        public string ReportTemplateId { get; set; }
        [DataMember]
        public string BIReportEntityId { get; set; }
        [DataMember]
        public string DWQueryId { get; set; }
        [DataMember]
        public string ReportTemplateType { get; set; }
        [DataMember]
        public ReportSchedulerRecepients Recepients { get; set; }
        [DataMember]
        public List<QueryFilterItem> ReportFilterItems { get; set; }
        [DataMember]
        public string MainCustomerFieldName { get; set; }
        [DataMember]
        public DWObjectFieldsDetails DWQueryFilterData { get; set; }
        [DataMember]
        public string DocumentTypeTemplateId { get; set; }
        [DataMember]
        public string MessageTemplateId { get; set; }
		[DataMember]
		public string ProcedureName { get; set; }
	}

    [DataContract(Namespace = "")]
    public class ReportSchedulerRecepients
    {
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Cc { get; set; }
        [DataMember]
        public string Bcc { get; set; }
    }
}

using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityPMs
{
    public partial class TicketPM
    {
        //[DataMember]
        //public CustomFieldClass Field1 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field2 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field3 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field4 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field5 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field6 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field7 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field8 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field9 { get; set; }
        //[DataMember]
        //public CustomFieldClass Field10 { get; set; }

        [DataMember]
        public bool IsUpdateByAutomation { get; set; }
        
        //private List<DocumentDataPM> ticketDocumentDatas;
        //[DataMember]
        //public virtual List<DocumentDataPM> TicketDocumentData
        //{
        //    get
        //    {
        //        if (ticketDocumentDatas == null)
        //        {
        //            ticketDocumentDatas = new List<DocumentDataPM>();
        //        }
        //        return ticketDocumentDatas;
        //    }
        //    set { ticketDocumentDatas = value; }
        //}


        //private List<CorrespondencePM> ticketCorrespondence;

        //[Include]
        //[Association("TicketCorrespondence", "Id", "EntityId")]
        //public virtual List<CorrespondencePM> TicketCorrespondence
        //{
        //    get
        //    {
        //        if (ticketCorrespondence == null)
        //        {
        //            ticketCorrespondence = new List<CorrespondencePM>();
        //        }
        //        return ticketCorrespondence;
        //    }
        //    set { ticketCorrespondence = value; }
        //}

        //private List<CommunicationLogPM> ticketCommunications;
        //[Include]
        //[Association("TicketCommunicationLog", "Id", "EntityId")]
        //public virtual List<CommunicationLogPM> TicketCommunications
        //{
        //    get
        //    {
        //        if (ticketCommunications == null)
        //        {
        //            ticketCommunications = new List<CommunicationLogPM>();
        //        }
        //        return ticketCommunications;
        //    }
        //    set { ticketCommunications = value; }
        //}

        //[DataMember]
        //public bool IsCreatedFromOutSide { get; set; }

        //[DataMember]
        //public bool IsResolveDue { get; set; }

        //[DataMember]
        //public string ResolveColor { get; set; }

        //[DataMember]
        //public bool IsResolveExamination { get; set; }

        //[DataMember]
        //public bool IsResponseDue { get; set; }

        //[DataMember]
        //public string ResponseColor { get; set; }

        //[DataMember]
        //public bool IsResponseExamination { get; set; }

        //[DataMember]
        //public string CompanyTableName { get; set; }

        //[DataMember]
        //public string RankName { get; set; }
    }
}

using Logitude.CRM.BL.Validators;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityPMs
{
   
    public partial class OpportunityPM
    {
        [Timestamp]
        [DataMember]
        public byte[] LastModified { get; set; }
        //[DataMember]
        //public bool IsCopy { get; set; }
        //[DataMember]
        //public string CopyFromEntityId { get; set; }
        //[DataMember]
        //public string CustomerRankCode { get; set; }
        //[DataMember]
        //public string CustomerRankName { get; set; }
        //[DataMember]
        //public bool PostToFollowersAsWon { get; set; }
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
        public int? StageMaxDays { get; set; }
        //[DataMember]
        //public string ClosingReasonCode { get; set; }
        //[DataMember]
        //public string CustomerExternalId { get; set; }

        //[DataMember]
        //public bool IsCustomerBlockedBusinessUnit { get; set; }

        [DataMember]
        public string CountryName { get; set; }

        [DataMember]
        public string LastCompletedActivityTypeName { get; set; }

        //[DataMember]
        //public bool IsClosedLost { get; set; }
    }
}

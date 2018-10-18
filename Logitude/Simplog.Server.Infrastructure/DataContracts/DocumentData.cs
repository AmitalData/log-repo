using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts
{
    [DataContract]
    public class DocumentDataPM
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string EntityId { get; set; }
        [DataMember]
        public string ChildEntityId { get; set; }
        [DataMember]
        public string DocumentTypeId { get; set; }
        [DataMember]
        public string ObjectTableName { get; set; }
        [DataMember]
        public string ChildObjectTableName { get; set; }
        [DataMember]
        public string FileExtension { get; set; }
        [DataMember]
        public byte[] FileData { get; set; }
        [DataMember]
        public string ChildEntityReference { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string UserEmail { get; set; }
        [DataMember]
        public DateTime? ReceivedDate { get; set; }
        [DataMember]
        public string DocumentId { get; set; }
        [DataMember]
        public string DocumentTypeName { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string Description { get; set; }
        
        [DataMember]
        public string ExternalCode { get; set; }


        [DataMember]
        public string EntityReference { get; set; }
        [DataMember]
        public string ExternalEntityName { get; set; }
        [DataMember]
        public string ExternalEntityReference { get; set; }


        [DataMember]
        public string CorrespondenceId { get; set; }


        [DataMember]
        public string SecurityId { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }


        [DataMember]
        public DateTime? UpdateDate { get; set; }


        [DataMember]
        public double? FileSize { get; set; }

    }
}

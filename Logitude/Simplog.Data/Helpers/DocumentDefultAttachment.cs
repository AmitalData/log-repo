using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{

    [DataContract(Namespace = "")]
    public class DocumentDefultAttachment
    {
        [DataMember]
        public string DocumentTypeId { get; set; }

        [DataMember]
        public string DocumentTypeName { get; set; }

        [DataMember]
        public string DocumentTypeCopyId { get; set; }

        
        [DataMember]
        public string Type { get; set; }
        public bool IsExist { get; set; }
        public string DocumentId { get; set; }

        
    }



}

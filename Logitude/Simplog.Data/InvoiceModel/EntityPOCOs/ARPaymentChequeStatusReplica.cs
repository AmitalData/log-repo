using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.EntityPOCOs
{
   public class ARPaymentChequeStatusReplica
    {

        [Key]
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public string LocalName { get; set; }
        [DataMember]
        public string EnglishName { get; set; }
        [DataMember]
        public bool Inactive { get; set; }


    }
}

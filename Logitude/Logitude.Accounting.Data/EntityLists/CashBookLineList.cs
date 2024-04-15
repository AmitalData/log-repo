using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.Accounting.Data.EntityLists
{
    public partial class CashBookLineList
    {
        [DataMember]
        public string CardId { get; set; }
        [DataMember]
        public string CardLocalName { get; set; }
        [DataMember]
        public string PartnerTypeId { get; set; }
    }

}

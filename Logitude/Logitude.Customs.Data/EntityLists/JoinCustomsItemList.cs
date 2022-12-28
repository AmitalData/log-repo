using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityLists
{
    public partial class JoinCustomsItemList
    {
        [DataMember]
        public int CustomsBookTypeID { get; set; }
        [DataMember]
        public string FullClassification { get; set; }
        [DataMember]
        public int CustomsItemCategoryID { get; set; }
        [DataMember]
        public int? CustomsItemHierarchicLocationID { get; set; }
        [DataMember]
        public string ComputedCheckDigit { get; set; }
        [DataMember]
        public string Title { get; set; }
        [Key]
        [DataMember]
        public string ID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPTemplateTableDesignList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string BorderTypeCode  { get; set; }
       [DataMember]
       public string BorderColor  { get; set; }
       [DataMember]
       public int BorderThickness  { get; set; }
       [DataMember]
       public string HeaderDesignId  { get; set; }
       [DataMember]
       public string LinesDesignId  { get; set; }
       [DataMember]
       public string GroupByDesignId  { get; set; }
   }

}
	 
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
   public partial class QuoteOPTemplateSectionList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public bool IsCancel  { get; set; }
       [DataMember]
       public string QuoteTemplateId  { get; set; }
       [DataMember]
       public string SectionDocId  { get; set; }
       [DataMember]
       public int Order  { get; set; }
       [DataMember]
       public string QuoteOPTemplateSectionTypeCode  { get; set; }
   }

}
	 
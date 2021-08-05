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
   public partial class QuoteOPTemplateTextDesignList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public double FontSize  { get; set; }
       [DataMember]
       public string TextColor  { get; set; }
       [DataMember]
       public string FontFamily  { get; set; }
       [DataMember]
       public string BackgroundColor  { get; set; }
       [DataMember]
       public string FontWeight  { get; set; }
       [DataMember]
       public bool Italic  { get; set; }
       [DataMember]
       public bool UnDerLine  { get; set; }
       [DataMember]
       public string Alignment  { get; set; }
   }

}
	 
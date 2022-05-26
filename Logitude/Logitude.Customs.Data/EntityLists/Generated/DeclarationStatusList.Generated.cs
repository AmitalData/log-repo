using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Customs.Data.EntityLists
{
   [DataContract]
   public partial class DeclarationStatusList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public bool FieldC1  { get; set; }
       [DataMember]
       public bool FieldC2  { get; set; }
       [DataMember]
       public bool FieldC3  { get; set; }
       [DataMember]
       public bool FieldC4  { get; set; }
       [DataMember]
       public bool FieldC5  { get; set; }
       [DataMember]
       public bool FieldC6  { get; set; }
       [DataMember]
       public bool FieldC7  { get; set; }
       [DataMember]
       public bool FieldC8  { get; set; }
       [DataMember]
       public bool FieldC9  { get; set; }
       [DataMember]
       public bool FieldC10  { get; set; }
       [DataMember]
       public bool FieldC11  { get; set; }
       [DataMember]
       public bool FieldC12  { get; set; }
       [DataMember]
       public bool FieldC13  { get; set; }
       [DataMember]
       public DateTime? FieldD1  { get; set; }
       [DataMember]
       public DateTime? FieldD2  { get; set; }
       [DataMember]
       public DateTime? FieldD3  { get; set; }
       [DataMember]
       public DateTime? FieldD4  { get; set; }
       [DataMember]
       public DateTime? FieldD5  { get; set; }
       [DataMember]
       public string FieldR1  { get; set; }
       [DataMember]
       public string FieldR2  { get; set; }
       [DataMember]
       public string FieldR3  { get; set; }
       [DataMember]
       public string FieldR4  { get; set; }
       [DataMember]
       public string FieldR5  { get; set; }
   }

}
	 
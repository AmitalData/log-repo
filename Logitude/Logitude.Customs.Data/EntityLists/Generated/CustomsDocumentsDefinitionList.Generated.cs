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
   public partial class CustomsDocumentsDefinitionList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string DocumentTypeCode  { get; set; }
       [DataMember]
       public string DocumentTypeName  { get; set; }
       [DataMember]
       public string TransportationTypeCode  { get; set; }
       [DataMember]
       public string TransportationTypeName  { get; set; }
       [DataMember]
       public string ProcessTypeCode  { get; set; }
       [DataMember]
       public string ProcessTypeName  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public string CargoTypeName  { get; set; }
       [DataMember]
       public bool Mandatory  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
   }

}
	 
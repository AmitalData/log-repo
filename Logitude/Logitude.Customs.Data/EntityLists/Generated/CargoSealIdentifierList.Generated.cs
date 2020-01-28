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
   public partial class CargoSealIdentifierList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string CargoRowNumber  { get; set; }
       [DataMember]
       public string ContainerNumber  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string ImporterId  { get; set; }
       [DataMember]
       public string CargoIdentifierTypeCode  { get; set; }
       [DataMember]
       public string CargoIdentifierTypeName  { get; set; }
       [DataMember]
       public string CargoIdentifierKey1  { get; set; }
       [DataMember]
       public string CargoIdentifierKey3  { get; set; }
       [DataMember]
       public string Status  { get; set; }
   }

}
	 
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
   public partial class DecCargoSplitCargoIdentifierList
   {
   
       [Key]
       [DataMember]
       public string DeclarationCargoSplitId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string CargoIdentifierKey1  { get; set; }
       [DataMember]
       public string CargoIdentifierKey2  { get; set; }
       [DataMember]
       public string CargoIdentifierKey3  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
   }

}
	 
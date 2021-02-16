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
   public partial class ExportDeclarationClosingDataList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string FinalCargoTypeCode  { get; set; }
       [DataMember]
       public string FinalManifestNumber  { get; set; }
       [DataMember]
       public string FinalSecondCargoId  { get; set; }
       [DataMember]
       public DateTime? LoadingDateTime  { get; set; }
       [DataMember]
       public string FinalShipCode  { get; set; }
       [DataMember]
       public string FinalLoadingSite  { get; set; }
   }

}
	 
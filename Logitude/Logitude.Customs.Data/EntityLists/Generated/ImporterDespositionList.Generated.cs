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
   public partial class ImporterDespositionList
   {
          [DataMember]
       public string DepositionNumber  { get; set; }
       [DataMember]
       public string ImporterDepositionStatusCode  { get; set; }
       [DataMember]
       public string ImporterlId  { get; set; }
       [DataMember]
       public string VendorID  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? EndDate  { get; set; }
       [DataMember]
       public string NotesToAgent  { get; set; }
       [DataMember]
       public string ErrorMessage  { get; set; }
       [DataMember]
       public string ImporterDepositionStatusName  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string Id  { get; set; }
   }

}
	 
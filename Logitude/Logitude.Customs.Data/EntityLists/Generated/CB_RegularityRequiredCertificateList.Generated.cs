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
   public partial class CB_RegularityRequiredCertificateList
   {
          [DataMember]
       public int ID  { get; set; }
       [DataMember]
       public int RegularityInceptionID  { get; set; }
       [DataMember]
       public string ConfirmationTypeID  { get; set; }
       [DataMember]
       public int? Number  { get; set; }
       [DataMember]
       public string TextualCondition  { get; set; }
       [DataMember]
       public int? TrNumber  { get; set; }
       [DataMember]
       public string AuthorityID  { get; set; }

       [Key]
       [DataMember]
       public string CB_ID  { get; set; }
       [DataMember]
       public bool IsVoluntaryOrImporterOfTrust  { get; set; }
   }

}
	 
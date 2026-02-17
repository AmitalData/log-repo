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
   public partial class VendorCommunicationList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }

       [Key]
       [DataMember]
       public string VendorId  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }
       [DataMember]
       public string CommunicationTypeCode  { get; set; }
       [DataMember]
       public string CommunicationAddress  { get; set; }
       [DataMember]
       public string CommunicationTypeName  { get; set; }
   }

}
	 
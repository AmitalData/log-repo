using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.CRM.Data.EntityLists
{
   [DataContract]
   public partial class OccasionContactList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string Email  { get; set; }
       [DataMember]
       public string BusinessPhone  { get; set; }
       [DataMember]
       public string Mobile  { get; set; }
       [DataMember]
       public string Position  { get; set; }
       [DataMember]
       public string Customers  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public bool Invited  { get; set; }
       [DataMember]
       public bool Participated  { get; set; }
   }

}
	 
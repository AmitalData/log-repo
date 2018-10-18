using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class AutomaticReconcileMethodList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string AutomaticReconcile1  { get; set; }
       [DataMember]
       public string AutomaticReconcile2  { get; set; }
       [DataMember]
       public string AutomaticReconcile3  { get; set; }
       [DataMember]
       public string AutomaticReconcileName1  { get; set; }
       [DataMember]
       public string AutomaticReconcileName2  { get; set; }
       [DataMember]
       public string AutomaticReconcileName3  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public bool Inactive  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string LocalName  { get; set; }
   }

}
	 
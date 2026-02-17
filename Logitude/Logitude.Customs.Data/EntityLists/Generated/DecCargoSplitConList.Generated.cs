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
   public partial class DecCargoSplitConList
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
       public string ImporterCode  { get; set; }
       [DataMember]
       public string ConditionCode  { get; set; }
       [DataMember]
       public string ProcedureCurrentCode  { get; set; }
       [DataMember]
       public string ConditionName  { get; set; }
       [DataMember]
       public string ProcedureCurrentName  { get; set; }
   }

}
	 
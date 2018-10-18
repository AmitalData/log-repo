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
   public partial class QuestionnaireAnswerLineList
   {
   
       [Key]
       [DataMember]
       public string QuestionnaireAnswerId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int QuestionNumber  { get; set; }
       [DataMember]
       public string AnswerValue  { get; set; }
   }

}
	 
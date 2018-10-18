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
   public partial class QuestionnaireQuestionList
   {
   
       [Key]
       [DataMember]
       public string QuestioneerId  { get; set; }

       [Key]
       [DataMember]
       public int VersionNumber  { get; set; }

       [Key]
       [DataMember]
       public int QuestionNumber  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Question  { get; set; }
       [DataMember]
       public string QuestionTypeCode  { get; set; }
       [DataMember]
       public bool IsMandatory  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string PickListCode  { get; set; }
       [DataMember]
       public bool IsAddOther  { get; set; }
   }

}
	 
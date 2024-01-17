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
   public partial class CB_TradeAgreementList
   {
   
       [Key]
       [DataMember]
       public string ID  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string Title  { get; set; }
       [DataMember]
       public string AdditionName  { get; set; }
       [DataMember]
       public string CountryGroupID  { get; set; }
       [DataMember]
       public string CustomsBookTypeID  { get; set; }
       [DataMember]
       public string EntityStatusID  { get; set; }
       [DataMember]
       public string TradeAgreementAbbreviation  { get; set; }
   }

}
	 
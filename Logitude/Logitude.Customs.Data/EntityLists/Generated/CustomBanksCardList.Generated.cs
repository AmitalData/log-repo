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
   public partial class CustomBanksCardList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CustomBankId  { get; set; }
       [DataMember]
       public string CustomsBankName  { get; set; }
       [DataMember]
       public string CardId  { get; set; }
       [DataMember]
       public string CardName  { get; set; }
   }

}
	 
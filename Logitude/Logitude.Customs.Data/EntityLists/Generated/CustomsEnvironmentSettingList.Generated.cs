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
   public partial class CustomsEnvironmentSettingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }

       [Key]
       [DataMember]
       public string EnvironmentCode  { get; set; }
       [DataMember]
       public bool UseRabbitMQ  { get; set; }
       [DataMember]
       public string RabbitHost  { get; set; }
       [DataMember]
       public string RabbitUserName  { get; set; }
   }

}
	 
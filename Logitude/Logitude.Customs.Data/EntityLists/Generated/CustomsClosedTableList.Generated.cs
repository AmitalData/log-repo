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
   public partial class CustomsClosedTableList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public string CustomsName  { get; set; }
       [DataMember]
       public string CustomsLocalName  { get; set; }
       [DataMember]
       public string DbName  { get; set; }
       [DataMember]
       public DateTime? LastUpdateDate  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string ObjectTableName  { get; set; }
       [DataMember]
       public string ObjectTableId  { get; set; }
       [DataMember]
       public bool Existed  { get; set; }
       [DataMember]
       public DateTime? RetreiveDateTime  { get; set; }
   }

}
	 
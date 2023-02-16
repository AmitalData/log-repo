using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Infrastructure.Data.EntityLists
{
   [DataContract]
   public partial class ContainerSettingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public int? EmptyReturnClosingDays  { get; set; }
       [DataMember]
       public int? ShipmentATAClosingDays  { get; set; }
       [DataMember]
       public string ShipmentATADateIndicator  { get; set; }
       [DataMember]
       public bool IsExport  { get; set; }
       [DataMember]
       public bool IsDomestic  { get; set; }
       [DataMember]
       public bool IsImport  { get; set; }
       [DataMember]
       public bool IsDrop  { get; set; }
       [DataMember]
       public bool AddedManually  { get; set; }
       [DataMember]
       public DateTime? ActivationDate  { get; set; }
   }

}
	 
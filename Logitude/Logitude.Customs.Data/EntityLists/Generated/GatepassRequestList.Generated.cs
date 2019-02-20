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
   public partial class GatepassRequestList
   {
   
       [Key]
       [DataMember]
       public string MasterCourierId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public int? GatepassNumber  { get; set; }
       [DataMember]
       public string OriginSiteCode  { get; set; }
       [DataMember]
       public string UpdateCode  { get; set; }
       [DataMember]
       public string DesignateSiteCode  { get; set; }
       [DataMember]
       public string TransportationTypeCode  { get; set; }
       [DataMember]
       public string GatepassRequestStatus  { get; set; }
       [DataMember]
       public DateTime? CustomsUpdateDateTime  { get; set; }
       [DataMember]
       public string OriginSiteName  { get; set; }
       [DataMember]
       public string UpdateCodeName  { get; set; }
       [DataMember]
       public string DesignateSiteName  { get; set; }
       [DataMember]
       public string TransportationTypeName  { get; set; }
   }

}
	 
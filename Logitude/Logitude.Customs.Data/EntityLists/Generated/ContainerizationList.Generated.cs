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
   public partial class ContainerizationList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool AgentDeclaration  { get; set; }
       [DataMember]
       public DateTime ContainerizationDate  { get; set; }
       [DataMember]
       public string ContainerizationNumber  { get; set; }
       [DataMember]
       public string ContainerizationStatus  { get; set; }
       [DataMember]
       public string HataraStatus  { get; set; }
       [DataMember]
       public string OperationMode  { get; set; }
       [DataMember]
       public string ExportFile  { get; set; }
       [DataMember]
       public string TransportModeForExport  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public string ContainerizationStatusName  { get; set; }
       [DataMember]
       public string HataraStatusName  { get; set; }
       [DataMember]
       public bool HataraStatusIsNull  { get; set; }
       [DataMember]
       public bool OpenContainerization  { get; set; }
       [DataMember]
       public string IsMultiCustomers  { get; set; }
       [DataMember]
       public bool? IsMultiExportFiles  { get; set; }
       [DataMember]
       public string CargoTypeCode  { get; set; }
       [DataMember]
       public string ManifestNumber  { get; set; }
       [DataMember]
       public string SecondCargoID  { get; set; }
       [DataMember]
       public string ThirdCargoID  { get; set; }
       [DataMember]
       public string ContainerizationCargoID  { get; set; }
       [DataMember]
       public string ExistInCustoms  { get; set; }
   }

}
	 
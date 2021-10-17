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
   public partial class InterfaceManagementList
   {
   
       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string DcaPrefixName  { get; set; }
       [DataMember]
       public string Description  { get; set; }
       [DataMember]
       public string InOut  { get; set; }
       [DataMember]
       public string DefaultSendOptionsCode  { get; set; }
       [DataMember]
       public int? DefaultPriority  { get; set; }
       [DataMember]
       public bool AllowRestore  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public bool Active  { get; set; }
       [DataMember]
       public string TenantSendOptionsCode  { get; set; }
       [DataMember]
       public int? TenantPriority  { get; set; }
       [DataMember]
       public bool HasDefinition  { get; set; }
       [DataMember]
       public string TenantSendOptionName  { get; set; }
       [DataMember]
       public string DefaultSendOptionName  { get; set; }
       [DataMember]
       public bool SendAsDual  { get; set; }
       [DataMember]
       public string ResponseInterfaceCode  { get; set; }
       [DataMember]
       public string SignatureTypeCode  { get; set; }
       [DataMember]
       public string DcaPrefixName2  { get; set; }
       [DataMember]
       public string DcaPrefixName3  { get; set; }
       [DataMember]
       public string DcaPrefixName4  { get; set; }
       [DataMember]
       public string SignatureTypeName  { get; set; }
       [DataMember]
       public bool DcaRenameFileEnable  { get; set; }
       [DataMember]
       public string DcaRenameFilePrefix  { get; set; }
       [DataMember]
       public string InterfaceType  { get; set; }
       [DataMember]
       public string InterfaceTypeName  { get; set; }
       [DataMember]
       public bool UseRabbitMQ  { get; set; }
   }

}
	 
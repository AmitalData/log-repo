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
   public partial class ProceduralFaultList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ProceduralFaultNumber  { get; set; }
       [DataMember]
       public string ProceduralFaultStatusCode  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public string InputTypeCode  { get; set; }
       [DataMember]
       public string InspectionTypeCode  { get; set; }
       [DataMember]
       public string ProceduralFaultCode  { get; set; }
       [DataMember]
       public string ProceduralFaultInputProcesCode  { get; set; }
       [DataMember]
       public string RansomViolationTypeCode  { get; set; }
       [DataMember]
       public decimal? RansomViolationSum  { get; set; }
       [DataMember]
       public string Remarks  { get; set; }
       [DataMember]
       public bool IsCustomerResponsibility  { get; set; }
       [DataMember]
       public bool IsAgentProceduralFaultCountabl  { get; set; }
       [DataMember]
       public bool IsCustProceduralFaultCountabl  { get; set; }
       [DataMember]
       public bool IsAgentResponsibility  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string LeadingDocumentVersion  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public DateTime? CancellationDate  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public string ProceduralFaultStatusName  { get; set; }
       [DataMember]
       public string InputTypeName  { get; set; }
       [DataMember]
       public string InspectionTypeName  { get; set; }
       [DataMember]
       public string ProceduralFaultName  { get; set; }
       [DataMember]
       public string ProceduralFaultInputProcesName  { get; set; }
       [DataMember]
       public string RansomViolationTypeName  { get; set; }
       [DataMember]
       public string CustomFileNo  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string DeclarationNumber  { get; set; }
       [DataMember]
       public string SignedByUserId  { get; set; }
       [DataMember]
       public string SignedByUserName  { get; set; }
   }

}
	 
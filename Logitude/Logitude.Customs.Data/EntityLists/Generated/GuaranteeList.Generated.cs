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
   public partial class GuaranteeList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string TapagID  { get; set; }
       [DataMember]
       public string GuaranteeRequestStatusCode  { get; set; }
       [DataMember]
       public string GuaranteeRequestNumber  { get; set; }
       [DataMember]
       public string NumeralRequest  { get; set; }
       [DataMember]
       public string MsgID  { get; set; }
       [DataMember]
       public string ClientActivityCode  { get; set; }
       [DataMember]
       public string CustomEntityTypeCode  { get; set; }
       [DataMember]
       public string CustomEntityNumber  { get; set; }
       [DataMember]
       public DateTime? RequestValidityDate  { get; set; }
       [DataMember]
       public DateTime? GuaranteeValidityDate  { get; set; }
       [DataMember]
       public string BrandNumber  { get; set; }
       [DataMember]
       public string LawyerNumber  { get; set; }
       [DataMember]
       public DateTime? BirthDate  { get; set; }
       [DataMember]
       public string VehicleChassisNumber  { get; set; }
       [DataMember]
       public string EngineNumber  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string GuaranteeExternalNumber  { get; set; }
       [DataMember]
       public string CustomEntityTypeName  { get; set; }
       [DataMember]
       public string GuaranteeRequestStatusName  { get; set; }
       [DataMember]
       public string TapagNumber  { get; set; }
       [DataMember]
       public string LeadingFileNumber  { get; set; }
       [DataMember]
       public string TapagTypeCode  { get; set; }
       [DataMember]
       public string TapagTypeName  { get; set; }
       [DataMember]
       public string CustomerId  { get; set; }
       [DataMember]
       public string CustomerName  { get; set; }
       [DataMember]
       public string ImporterId  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public string CustomsBranchCode  { get; set; }
       [DataMember]
       public string CustomsBranchName  { get; set; }
       [DataMember]
       public string ProfessionUnitTypeCode  { get; set; }
       [DataMember]
       public string ProfessionUnitTypeName  { get; set; }
       [DataMember]
       public string SpecializationTypeCode  { get; set; }
       [DataMember]
       public string SpecializationTypeName  { get; set; }
       [DataMember]
       public DateTime? CreateDate  { get; set; }
       [DataMember]
       public DateTime? FollowDate  { get; set; }
       [DataMember]
       public DateTime? ValidityDate  { get; set; }
       [DataMember]
       public bool IsClosed  { get; set; }
       [DataMember]
       public string ClientActivityTypeName  { get; set; }
   }

}
	 
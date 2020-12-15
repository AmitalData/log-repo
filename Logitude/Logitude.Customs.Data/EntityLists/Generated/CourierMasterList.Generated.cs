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
   public partial class CourierMasterList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime? CreateDateTime  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public DateTime? UpdateDateTime  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string AirlineId  { get; set; }
       [DataMember]
       public string AirlineName  { get; set; }
       [DataMember]
       public string MAWB  { get; set; }
       [DataMember]
       public string MAWBTypeName  { get; set; }
       [DataMember]
       public string HAWB  { get; set; }
       [DataMember]
       public DateTime? EstimatedArrivalDate  { get; set; }
       [DataMember]
       public string GatewayPortCode  { get; set; }
       [DataMember]
       public string GatewayPortName  { get; set; }
       [DataMember]
       public string OriginPortCode  { get; set; }
       [DataMember]
       public string OriginPortName  { get; set; }
       [DataMember]
       public bool IsOpen  { get; set; }
       [DataMember]
       public bool IsCancelled  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string AirlinePrefix  { get; set; }
       [DataMember]
       public string MAWBTypeCode  { get; set; }
       [DataMember]
       public string ManifestNumber  { get; set; }
       [DataMember]
       public int? PackageQuantity  { get; set; }
       [DataMember]
       public decimal? GrossMassMeasure  { get; set; }
       [DataMember]
       public string ShortHAWB  { get; set; }
       [DataMember]
       public string FlightNumber  { get; set; }
       [DataMember]
       public DateTime? DepartureDate  { get; set; }
       [DataMember]
       public DateTime? EstimatedArrivalDateOnly  { get; set; }
       [DataMember]
       public DateTime? EstimatedArrivalTimeOnly  { get; set; }
       [DataMember]
       public string WeightValueCode  { get; set; }
       [DataMember]
       public string WeightValueName  { get; set; }
       [DataMember]
       public string StorageSiteCode  { get; set; }
       [DataMember]
       public string StorageSiteName  { get; set; }
       [DataMember]
       public string TruckerId  { get; set; }
       [DataMember]
       public string IntegratorCode  { get; set; }
       [DataMember]
       public string IntegratorName  { get; set; }
       [DataMember]
       public string IntegratorNumber  { get; set; }
       [DataMember]
       public bool IsReadyForInvoice  { get; set; }
       [DataMember]
       public bool IsAllDecClosedForFollowUp  { get; set; }
       [DataMember]
       public int CalcClosedForFollowUp  { get; set; }
       [DataMember]
       public int CalcMissingClassification  { get; set; }
       [DataMember]
       public int CalcMissingImporterId  { get; set; }
       [DataMember]
       public int CalcPendingCustoms  { get; set; }
       [DataMember]
       public int CalcPending900  { get; set; }
       [DataMember]
       public int CalcSuspendedDeclarations  { get; set; }
       [DataMember]
       public string NoOfCourierHawb  { get; set; }
       [DataMember]
       public bool IsAutomaticManifestSent  { get; set; }
       [DataMember]
       public bool? IsEstimatedArrivalToDay  { get; set; }
       [DataMember]
       public string EstimatedArrivalColor  { get; set; }
       [DataMember]
       public int? PackageQuantityInMAWB  { get; set; }
       [DataMember]
       public DateTime? LandingDate  { get; set; }
       [DataMember]
       public string UnifreightLeadingFile  { get; set; }
       [DataMember]
       public DateTime? LandingDateDateOnly  { get; set; }
       [DataMember]
       public DateTime? LandingDateTimeOnly  { get; set; }
       [DataMember]
       public string CourierMasterRemarks  { get; set; }
       [DataMember]
       public int OpenDeclarations  { get; set; }
   }

}
	 
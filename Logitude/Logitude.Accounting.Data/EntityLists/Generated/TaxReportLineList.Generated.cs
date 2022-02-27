using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.Accounting.Data.EntityLists
{
   [DataContract]
   public partial class TaxReportLineList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public DateTime LastUpdateDateTime  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }

       [Key]
       [DataMember]
       public string TaxReportId  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public string OutputOrInput  { get; set; }
       [DataMember]
       public string LineTypeCode  { get; set; }
       [DataMember]
       public string VatNumber  { get; set; }
       [DataMember]
       public string Reference  { get; set; }
       [DataMember]
       public string ReferecneGroup  { get; set; }
       [DataMember]
       public DateTime? ReferenceDate  { get; set; }
       [DataMember]
       public decimal? VatAmount  { get; set; }
       [DataMember]
       public decimal? VatableInvoiceAmount  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string TransmitStatusCode  { get; set; }
       [DataMember]
       public string JournalId  { get; set; }
       [DataMember]
       public bool? IsManuallyChanged  { get; set; }
       [DataMember]
       public bool IsEquipment  { get; set; }
       [DataMember]
       public string StatusLocalName  { get; set; }
       [DataMember]
       public string StatusEnglishName  { get; set; }
       [DataMember]
       public string JournalNumber  { get; set; }
       [DataMember]
       public DateTime? TaxReportDate  { get; set; }
       [DataMember]
       public bool IsExternalLine  { get; set; }
       [DataMember]
       public decimal? TotalInvoiceAmount  { get; set; }
       [DataMember]
       public string OriginalReference  { get; set; }
       [DataMember]
       public string UpdatedBUserName  { get; set; }
       [DataMember]
       public string PreviousReference  { get; set; }
       [DataMember]
       public bool IsReconciled  { get; set; }
   }

}
	 
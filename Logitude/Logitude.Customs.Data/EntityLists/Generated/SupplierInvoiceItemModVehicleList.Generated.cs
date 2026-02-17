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
   public partial class SupplierInvoiceItemModVehicleList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceCounterKey  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceItemLineNumber  { get; set; }

       [Key]
       [DataMember]
       public string AdjustmentTypeCode  { get; set; }
       [DataMember]
       public decimal? DeductAmount  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string AdjustmentTypeName  { get; set; }
   }

}
	 
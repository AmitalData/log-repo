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
   public partial class SupplierInvoiceItemsReqListList
   {
          [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }

       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }

       [Key]
       [DataMember]
       public int LineNumber  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceCounterKey  { get; set; }

       [Key]
       [DataMember]
       public int InvoiceItemLineNumber  { get; set; }
       [DataMember]
       public string RequestType  { get; set; }
       [DataMember]
       public string ProductFileNumber  { get; set; }
       [DataMember]
       public string ManufactureCountryCode  { get; set; }
       [DataMember]
       public string ManufactureCountryName  { get; set; }
       [DataMember]
       public string ManufacturerName  { get; set; }
       [DataMember]
       public string Remarks  { get; set; }
       [DataMember]
       public bool DutchRequested  { get; set; }
   }

}
	 
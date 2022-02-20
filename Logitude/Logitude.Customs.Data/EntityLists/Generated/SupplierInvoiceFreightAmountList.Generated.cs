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
   public partial class SupplierInvoiceFreightAmountList
   {
          [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int InvoiceCounterKey  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CurrencyTypeCode  { get; set; }
       [DataMember]
       public decimal? Amount  { get; set; }

       [Key]
       [DataMember]
       public string Id  { get; set; }
   }

}
	 
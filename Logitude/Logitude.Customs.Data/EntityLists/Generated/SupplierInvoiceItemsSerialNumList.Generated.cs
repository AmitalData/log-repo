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
   public partial class SupplierInvoiceItemsSerialNumList
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
       public int LineNumber  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string TypeCode  { get; set; }
       [DataMember]
       public string SerialNumber  { get; set; }
       [DataMember]
       public string TypeName  { get; set; }
   }

}
	 
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
   public partial class Aur_PaymentList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public string DraftNumber  { get; set; }
       [DataMember]
       public string ForMonth  { get; set; }
       [DataMember]
       public string SaleOrder  { get; set; }
       [DataMember]
       public string Customer  { get; set; }
       [DataMember]
       public string CustomerReference1  { get; set; }
       [DataMember]
       public string InvoiceType  { get; set; }
       [DataMember]
       public string PaymentRequestStatus  { get; set; }
       [DataMember]
       public string ErrorMessage  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CustomerReference2  { get; set; }
   }

}
	 
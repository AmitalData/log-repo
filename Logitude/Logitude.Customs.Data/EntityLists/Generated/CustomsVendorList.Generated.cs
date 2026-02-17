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
   public partial class CustomsVendorList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string VendorNumber  { get; set; }
       [DataMember]
       public string VendorTypeCode  { get; set; }
       [DataMember]
       public string VendorName  { get; set; }
       [DataMember]
       public string CountryCode  { get; set; }
       [DataMember]
       public string SubCountryCode  { get; set; }
       [DataMember]
       public string SubCountryName  { get; set; }
       [DataMember]
       public string CityName  { get; set; }
       [DataMember]
       public string MainAddressLine  { get; set; }
       [DataMember]
       public string PostalCode  { get; set; }
       [DataMember]
       public string DunsNumber  { get; set; }
       [DataMember]
       public string VATNumber  { get; set; }
       [DataMember]
       public string StatusCode  { get; set; }
       [DataMember]
       public string TransactionTypeID  { get; set; }
       [DataMember]
       public string VendorTypeName  { get; set; }
       [DataMember]
       public string CountryName  { get; set; }
       [DataMember]
       public bool InActive  { get; set; }
       [DataMember]
       public string ExternalId  { get; set; }
       [DataMember]
       public string StatusName  { get; set; }
   }

}
	 
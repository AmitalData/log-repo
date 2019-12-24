using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.TariffModule.Data.EntityLists
{
   [DataContract]
   public partial class TariffList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public DateTime? StartDate  { get; set; }
       [DataMember]
       public DateTime? ExpirationDate  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public bool InActive  { get; set; }
       [DataMember]
       public string Notes  { get; set; }
       [DataMember]
       public string SellerId  { get; set; }
       [DataMember]
       public string CurrencyId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime UpdateDate  { get; set; }
       [DataMember]
       public string PriceSteps  { get; set; }
       [DataMember]
       public string TypeCode  { get; set; }
       [DataMember]
       public string TypeName  { get; set; }
       [DataMember]
       public int LastVersion  { get; set; }
       [DataMember]
       public string ContractNumber  { get; set; }
       [DataMember]
       public string CreatedByUserName  { get; set; }
       [DataMember]
       public string SellerName  { get; set; }
       [DataMember]
       public string UpdatedByUserName  { get; set; }
       [DataMember]
       public string TariffNumber  { get; set; }
       [DataMember]
       public bool IsFromUpdateScreen  { get; set; }
       [DataMember]
       public bool IsFromCopy  { get; set; }
       [DataMember]
       public string LastActivityTypeName  { get; set; }
       [DataMember]
       public string LastActivityByUserName  { get; set; }
       [DataMember]
       public DateTime? LastActivityDate  { get; set; }
       [DataMember]
       public string TransportModeCode  { get; set; }
   }

}
	 
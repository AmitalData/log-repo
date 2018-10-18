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
   public partial class ClientAddressList
   {
   
       [Key]
       [DataMember]
       public string ClientId  { get; set; }

       [Key]
       [DataMember]
       public string AddressId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string ContactStateCode  { get; set; }
       [DataMember]
       public string AddressTypeCode  { get; set; }
       [DataMember]
       public string AddressPurposeCode  { get; set; }
       [DataMember]
       public bool IsPalestinianCity  { get; set; }
       [DataMember]
       public bool IsHebrewAddress  { get; set; }
       [DataMember]
       public string BranchName  { get; set; }
       [DataMember]
       public string ContactIdentifier  { get; set; }
       [DataMember]
       public string ContactFirstName  { get; set; }
       [DataMember]
       public string ContactLastName  { get; set; }
       [DataMember]
       public string ContactRoleTypeCode  { get; set; }
       [DataMember]
       public string AuthorizedSignerPermit1  { get; set; }
       [DataMember]
       public string AuthorizedSignerPermit2  { get; set; }
       [DataMember]
       public string AuthorizedSignerPermit3  { get; set; }
       [DataMember]
       public string LocalCityCode  { get; set; }
       [DataMember]
       public string LocalSecondLine  { get; set; }
       [DataMember]
       public string LocalStreetName  { get; set; }
       [DataMember]
       public string LocalHouseLetter  { get; set; }
       [DataMember]
       public string LocalEntrance  { get; set; }
       [DataMember]
       public string EnglishCountryCode  { get; set; }
       [DataMember]
       public string EnglishSubCountryCode  { get; set; }
       [DataMember]
       public string EnglishCityName  { get; set; }
       [DataMember]
       public string EnglishMainAddressLine  { get; set; }
       [DataMember]
       public string EnglishPostalCode  { get; set; }
       [DataMember]
       public decimal? LocalApartment  { get; set; }
       [DataMember]
       public string LocalPOBox  { get; set; }
       [DataMember]
       public string LocalPostalCode  { get; set; }
       [DataMember]
       public string LocalHouseNumber  { get; set; }
       [DataMember]
       public string ContactStateName  { get; set; }
       [DataMember]
       public string AddressTypeName  { get; set; }
       [DataMember]
       public string AddressPurposeName  { get; set; }
       [DataMember]
       public string ContactRoleTypeName  { get; set; }
       [DataMember]
       public string AuthorizedSignerPermit1Name  { get; set; }
       [DataMember]
       public string AuthorizedSignerPermit2Name  { get; set; }
       [DataMember]
       public string AuthorizedSignerPermit3Name  { get; set; }
       [DataMember]
       public string LocalCityName  { get; set; }
       [DataMember]
       public string EnglishCountryName  { get; set; }
       [DataMember]
       public string EnglishSubCountryName  { get; set; }
       [DataMember]
       public string CustomAddressCode  { get; set; }
   }

}
	 
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
   public partial class ClientList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string FullName  { get; set; }
       [DataMember]
       public string ClientTypeSpecificCode  { get; set; }
       [DataMember]
       public bool IsActive  { get; set; }
       [DataMember]
       public string LocalFirstName  { get; set; }
       [DataMember]
       public string LocalLastName  { get; set; }
       [DataMember]
       public string LocalCorporationName  { get; set; }
       [DataMember]
       public string EnglishFirstName  { get; set; }
       [DataMember]
       public string EnglishLastName  { get; set; }
       [DataMember]
       public string EnglishCorporationName  { get; set; }
       [DataMember]
       public DateTime? BirthDate  { get; set; }
       [DataMember]
       public string GenderCode  { get; set; }
       [DataMember]
       public string DunsNumber  { get; set; }
       [DataMember]
       public string PassportNumber  { get; set; }
       [DataMember]
       public string PassportCountryCode  { get; set; }
       [DataMember]
       public string PassportTypeCode  { get; set; }
       [DataMember]
       public string PassportFirstName  { get; set; }
       [DataMember]
       public string PassportLastName  { get; set; }
       [DataMember]
       public string EnglishBirthPlace  { get; set; }
       [DataMember]
       public string EnglishFatherName  { get; set; }
       [DataMember]
       public DateTime? PassportExpirationDate  { get; set; }
       [DataMember]
       public DateTime? PassportIssueDate  { get; set; }
       [DataMember]
       public string ClientTypeSpecificName  { get; set; }
       [DataMember]
       public string PassportCountryName  { get; set; }
       [DataMember]
       public string GenderName  { get; set; }
       [DataMember]
       public string PassportTypeName  { get; set; }
       [DataMember]
       public bool IsImporter  { get; set; }
       [DataMember]
       public bool IsExporter  { get; set; }
       [DataMember]
       public string FacilitationTypeCode  { get; set; }
       [DataMember]
       public string NationalIdentificationNumber  { get; set; }
       [DataMember]
       public int? IsExportPoaActive  { get; set; }
   }

}
	 
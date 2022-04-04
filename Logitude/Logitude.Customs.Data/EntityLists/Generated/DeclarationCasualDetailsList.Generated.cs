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
   public partial class DeclarationCasualDetailsList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string CasualImporterAddress1  { get; set; }
       [DataMember]
       public string CasualImporterAddress2  { get; set; }
       [DataMember]
       public string CasualImporterCity  { get; set; }
       [DataMember]
       public string CasualImporterZipCode  { get; set; }
       [DataMember]
       public string CasualImporterFax  { get; set; }
       [DataMember]
       public string CasualImporterEmail  { get; set; }
       [DataMember]
       public string CasualImporterTel  { get; set; }
       [DataMember]
       public string CasualImporterContact  { get; set; }
       [DataMember]
       public string ImporterCode  { get; set; }
       [DataMember]
       public string EntitleImporterCode  { get; set; }
       [DataMember]
       public string ImporterName  { get; set; }
       [DataMember]
       public string ImporterAddress  { get; set; }
       [DataMember]
       public string TransferImporterAddress  { get; set; }
       [DataMember]
       public string EntitleImporterAddress  { get; set; }
       [DataMember]
       public string TransferImporterName  { get; set; }
       [DataMember]
       public string EntitleImporterName  { get; set; }
       [DataMember]
       public string ImporterPassportNumber  { get; set; }
       [DataMember]
       public string TransferPassportNumber  { get; set; }
       [DataMember]
       public string EntitlePassportNumber  { get; set; }
   }

}
	 
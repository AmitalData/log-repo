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
       public string CasualSupplierName  { get; set; }
       [DataMember]
       public string CasualSupplierAddress  { get; set; }
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
   }

}
	 
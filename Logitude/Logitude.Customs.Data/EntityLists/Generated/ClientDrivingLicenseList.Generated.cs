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
   public partial class ClientDrivingLicenseList
   {
   
       [Key]
       [DataMember]
       public string ClientId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public int Line  { get; set; }
       [DataMember]
       public string DrivingLicenseNumber  { get; set; }
       [DataMember]
       public DateTime? DriverLicenseValidityDate  { get; set; }
       [DataMember]
       public string DrivingLicenseCountryID  { get; set; }
       [DataMember]
       public string DrivingLicenseCountryName  { get; set; }
   }

}
	 
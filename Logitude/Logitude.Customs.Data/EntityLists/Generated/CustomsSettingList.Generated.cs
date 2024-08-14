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
   public partial class CustomsSettingList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public bool IsConnectedToUniFreight  { get; set; }
       [DataMember]
       public string CustomsAgentId  { get; set; }
       [DataMember]
       public string SignServiceAddress  { get; set; }
       [DataMember]
       public string IIGServiceAddress  { get; set; }
       [DataMember]
       public string DCAServiceAddress  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string DCAPartnerVault  { get; set; }
       [DataMember]
       public string UServerServiceAddress  { get; set; }
       [DataMember]
       public string DefaultNotificationAssignee  { get; set; }
       [DataMember]
       public string DefaultNotificationAssigneeName  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string CustomsEnvoirmentTypeCode  { get; set; }
       [DataMember]
       public string CustomsEnvoirmentTypeName  { get; set; }
       [DataMember]
       public string OnPremiseFillingService  { get; set; }
       [DataMember]
       public string UnfConnectionString  { get; set; }
       [DataMember]
       public bool TehilaDca  { get; set; }
       [DataMember]
       public bool BlockAgentBankForMasab  { get; set; }
       [DataMember]
       public string PaymentOrderAccCard  { get; set; }
       [DataMember]
       public bool UnifreightCertificateActivated  { get; set; }
       [DataMember]
       public bool AutoFillPaymentScreen  { get; set; }
       [DataMember]
       public bool AutoFillAccountType  { get; set; }
       [DataMember]
       public bool AutoUnitMeasurement  { get; set; }
       [DataMember]
       public string CompanyType  { get; set; }
       [DataMember]
       public string HSMCompanyId  { get; set; }
       [DataMember]
       public string HSMToken  { get; set; }
       [DataMember]
       public bool StandAlone  { get; set; }
       [DataMember]
       public string OcrToken  { get; set; }
       [DataMember]
       public string CourierDocToken  { get; set; }
   }

}
	 
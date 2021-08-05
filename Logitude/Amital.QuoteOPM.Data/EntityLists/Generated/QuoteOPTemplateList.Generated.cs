using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Amital.QuoteOPM.Data.EntityLists
{
   [DataContract]
   public partial class QuoteOPTemplateList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string HeaderDocId  { get; set; }
       [DataMember]
       public string FooterDocId  { get; set; }
       [DataMember]
       public string QuoteOPTemplateSettingId  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public bool IsTemplate  { get; set; }
       [DataMember]
       public string OriginalQuoteOPTemplateId  { get; set; }
       [DataMember]
       public DateTime CreateDate  { get; set; }
       [DataMember]
       public DateTime? UpdateDate  { get; set; }
       [DataMember]
       public string CreatedByUserId  { get; set; }
       [DataMember]
       public string UpdatedByUserId  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string TemplateTypeCode  { get; set; }
       [DataMember]
       public string TemplateTypeName  { get; set; }
       [DataMember]
       public bool IsDefault  { get; set; }
       [DataMember]
       public bool ShowLocalLanguage  { get; set; }
       [DataMember]
       public bool InActive  { get; set; }
       [DataMember]
       public bool IsLastQuoteOPTemplateDocumentVersion  { get; set; }
       [DataMember]
       public bool IsCopiedAtSignup  { get; set; }
       [DataMember]
       public bool IsEnabledForCustomers  { get; set; }
       [DataMember]
       public string TenantName  { get; set; }
   }

}
	 
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
   public partial class CustomsDocumentList
   {
          [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string DocumentsFilingId  { get; set; }
       [DataMember]
       public string CustomsDocId  { get; set; }
       [DataMember]
       public string DocumentStatusCode  { get; set; }
       [DataMember]
       public string DocumentRemarks  { get; set; }
       [DataMember]
       public string DocumentStatusName  { get; set; }
       [DataMember]
       public string Extension  { get; set; }
       [DataMember]
       public double? FileSize  { get; set; }
       [DataMember]
       public string Name  { get; set; }
       [DataMember]
       public string DocumentTypeCode  { get; set; }
       [DataMember]
       public string DocumentTypeName  { get; set; }
       [DataMember]
       public bool IsPartOfDeclaration  { get; set; }
       [DataMember]
       public decimal OcrScore  { get; set; }
       [DataMember]
       public string OcrStatusCode  { get; set; }
       [DataMember]
       public string OcrReference  { get; set; }
       [DataMember]
       public bool OcrNotConnect  { get; set; }
   }

}
	 
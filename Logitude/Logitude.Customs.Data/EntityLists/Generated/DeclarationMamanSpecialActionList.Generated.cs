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
   public partial class DeclarationMamanSpecialActionList
   {
   
       [Key]
       [DataMember]
       public string DeclarationId  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }

       [Key]
       [DataMember]
       public string MamanSpecialActionCode  { get; set; }
       [DataMember]
       public string MamanLabelText1  { get; set; }
       [DataMember]
       public string MamanLabelText2  { get; set; }
       [DataMember]
       public string MamanLabelText3  { get; set; }
       [DataMember]
       public string MamanSpecialActionName  { get; set; }
       [DataMember]
       public string MamanLabelText4  { get; set; }
       [DataMember]
       public string MamanLabelText5  { get; set; }
       [DataMember]
       public string MamanSpecialActionStatusCode  { get; set; }
       [DataMember]
       public string MamanSpecialActionStatusName  { get; set; }
       [DataMember]
       public string MamanSpecialActionsErrorXml  { get; set; }
   }

}
	 
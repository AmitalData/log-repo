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
   public partial class BorderOPTypeList
   {
   
       [Key]
       [DataMember]
       public string Code  { get; set; }
       [DataMember]
       public string Name  { get; set; }
   }

}
	 
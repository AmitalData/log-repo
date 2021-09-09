using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.FullAccounting.Test.Models
{
   public partial class JournalActionTypeList
   {
   
       
       public string Id  { get; set; }
      
       public int Tenant  { get; set; }
       
       public string Code  { get; set; }

       public string LocalName  { get; set; }

       public string EnglishName  { get; set; }

       public string SearchFields  { get; set; }

       public bool Inactive  { get; set; }
   }

}
	 
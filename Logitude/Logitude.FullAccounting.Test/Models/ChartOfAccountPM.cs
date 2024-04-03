using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Logitude.FullAccounting.Test.Models
{
   
   public class ChartOfAccountPM
   {
       
        public string Id { get; set; }
		public int Tenant { get; set; }
		public string Code { get; set; }
		public string LocalName { get; set; }
		public string EnglishName { get; set; }
		public string ParentId { get; set; }
		public string TypeCode { get; set; }
		public bool? Inactive { get; set; }
		public string TypeName { get; set; }
		public string ParentName { get; set; }
		public string SearchFields { get; set; }
		public int? ChartOfAccountSecurityLevel { get; set; }
	}
   
}
	 
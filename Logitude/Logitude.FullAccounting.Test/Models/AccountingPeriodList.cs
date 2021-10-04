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
   public class AccountingPeriodList
   {
   
       
       public string Id  { get; set; }
       
       public int Tenant  { get; set; }
       
       public int Year  { get; set; }
       
       public string PeriodTypeCode  { get; set; }
       
       public string PeriodTypeName  { get; set; }
       
       public int OpenMonth  { get; set; }
       
       public int? ClosedMonth  { get; set; }
   }

}
	 
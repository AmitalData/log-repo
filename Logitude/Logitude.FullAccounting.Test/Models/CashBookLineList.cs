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
   public class CashBookLineList
   {
   
       
       
       public string CashBookId  { get; set; }
       
       public int Tenant  { get; set; }

       
       public string ARPChequeId  { get; set; }
       
       public string ChequeNumber  { get; set; }
       
       public bool IsDeposited  { get; set; }
       
       public DateTime DueDate  { get; set; }
       
       public decimal LocalAmount  { get; set; }
       
       public string Currency  { get; set; }
       
       public decimal ForeignAmount  { get; set; }
       
       public string AccountNumber  { get; set; }
       
       public string Bank  { get; set; }
       
       public string Branch  { get; set; }
       
       public string ARPaymentNumber  { get; set; }
       
       public string ARPaymentId  { get; set; }
       
       public string SearchFields  { get; set; }
       
       public string ARPChequeStatusName  { get; set; }
       
       public string ARPChequeStatusCode  { get; set; }
       
       public string ARPChequeStatusLocalName  { get; set; }
   }

}
	 
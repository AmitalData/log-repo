using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.Accounting.Data.EntityKeys
{
   public partial class CashBookLineKeys : EntityKeyFields
   {
   	  public string CashBookId  { get; set; }
	  
				 
	    			   
	  public string ARPChequeId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return CashBookId+'_'+ARPChequeId;
      }

      public override string GetEntityPMName()
      {
          return "CashBookLinePM";
      }
	 
   }

}
	 
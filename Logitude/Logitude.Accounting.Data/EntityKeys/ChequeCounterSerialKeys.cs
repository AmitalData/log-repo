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
   public partial class ChequeCounterSerialKeys : EntityKeyFields
   {
   	  public int SeriesId  { get; set; }
	  
				 
	    			   
	  public string BankAccountId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return SeriesId+'_'+BankAccountId ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ChequeCounterSerialPM";
      }
	 
   }

}
	 
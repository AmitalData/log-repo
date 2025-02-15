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
   public partial class Aur_TimesheetKeys : EntityKeyFields
   {
   	  public int Line  { get; set; }
	  
				 
	    			   
	  public string PaymentId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return Line+'_'+PaymentId ;
                 
      }

      public override string GetEntityPMName()
      {
          return "Aur_TimesheetPM";
      }
	 
   }

}
	 
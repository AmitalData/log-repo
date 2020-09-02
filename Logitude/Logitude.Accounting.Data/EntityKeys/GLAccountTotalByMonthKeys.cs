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
   public partial class GLAccountTotalByMonthKeys : EntityKeyFields
   {
   	  public string AccountId  { get; set; }
	  
				 
	    			   
	  public string DateTypeCode  { get; set; }
	  
				 
	    			   
	  public int Year  { get; set; }
	  
				 
	    			   
	  public int Month  { get; set; }
	  
				 
	    			   
	  public string CurrencyId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return AccountId+'_'+DateTypeCode+'_'+Year+'_'+Month+'_'+CurrencyId;
      }

      public override string GetEntityPMName()
      {
          return "GLAccountTotalByMonthPM";
      }
	 
   }

}
	 
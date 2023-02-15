using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.Customs.Data.EntityKeys
{
   public partial class VendorCurrencyKeys : EntityKeyFields
   {
   	  public string VendorId  { get; set; }
	  
				 
	    			   
	  public int Tenant  { get; set; }
	  
				 
	    			   
	  public int LineNumber  { get; set; }
	  
				 
	    			   
	  public string CurrencyType  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return VendorId+'_'+Tenant+'_'+LineNumber+'_'+CurrencyType ;
                 
      }

      public override string GetEntityPMName()
      {
          return "VendorCurrencyPM";
      }
	 
   }

}
	 
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
   public partial class InterestReportLineKeys : EntityKeyFields
   {
   	  public string InterestReportId  { get; set; }
	  
				 
	    			   
	  public string InterestTransactionId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return InterestReportId+'_'+InterestTransactionId;
      }

      public override string GetEntityPMName()
      {
          return "InterestReportLinePM";
      }
	 
   }

}
	 
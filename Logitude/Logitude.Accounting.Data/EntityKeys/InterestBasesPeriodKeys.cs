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
   public partial class InterestBasesPeriodKeys : EntityKeyFields
   {
   	  public string InterestBaseTypeId  { get; set; }
	  
				 
	    			   
	  public int LineNumber  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return InterestBaseTypeId+'_'+LineNumber;
      }

      public override string GetEntityPMName()
      {
          return "InterestBasesPeriodPM";
      }
	 
   }

}
	 
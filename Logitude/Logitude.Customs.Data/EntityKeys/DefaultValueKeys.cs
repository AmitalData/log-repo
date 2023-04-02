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
   public partial class DefaultValueKeys : EntityKeyFields
   {
   	  public string Id  { get; set; }
	  
				 
	    			   
	  public string DefaultTypeId  { get; set; }
	  
				 
	    			   
	  public string Distr  { get; set; }
	  
				 
	    			   
	  public string BranchId  { get; set; }
	  
				 
	    			   
	  public string CardId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return Id+'_'+DefaultTypeId+'_'+Distr+'_'+BranchId+'_'+CardId ;
                 
      }

      public override string GetEntityPMName()
      {
          return "DefaultValuePM";
      }
	 
   }

}
	 
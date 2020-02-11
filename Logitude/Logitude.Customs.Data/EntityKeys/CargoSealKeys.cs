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
   public partial class CargoSealKeys : EntityKeyFields
   {
   	  public string CargoSealIdentifierId  { get; set; }
	  
				 
	    			   
	  public string SealNumber  { get; set; }
	  
				 
	    			   
	  public string SealCompletenessStateCode  { get; set; }
	  
				 
	    			   
	  public string SealTypeCode  { get; set; }
	  
				 
	    			   
	  public string UpdateReasonCode  { get; set; }
	  
				 
	    			   
	  public string UpdateTypeCode  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return CargoSealIdentifierId+'_'+SealNumber+'_'+SealCompletenessStateCode+'_'+SealTypeCode+'_'+UpdateReasonCode+'_'+UpdateTypeCode;
      }

      public override string GetEntityPMName()
      {
          return "CargoSealPM";
      }
	 
   }

}
	 
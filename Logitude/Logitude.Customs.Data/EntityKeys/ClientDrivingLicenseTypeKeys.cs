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
   public partial class ClientDrivingLicenseTypeKeys : EntityKeyFields
   {
   	  public string ClientId  { get; set; }
	  
				 
	    			   
	  public int ClientDrivingLicenseLine  { get; set; }
	  
				 
	    			   
	  public string DriversLicenseTypeCode  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
                 return ClientId+'_'+ClientDrivingLicenseLine+'_'+DriversLicenseTypeCode ;
                 
      }

      public override string GetEntityPMName()
      {
          return "ClientDrivingLicenseTypePM";
      }
	 
   }

}
	 
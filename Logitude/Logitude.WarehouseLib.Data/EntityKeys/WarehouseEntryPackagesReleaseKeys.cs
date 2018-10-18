using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.WarehouseLib.Data.EntityKeys
{
   public partial class WarehouseEntryPackagesReleaseKeys : EntityKeyFields
   {
   	  public string EntryPackageId  { get; set; }
	  
				 
	    			   
	  public string ReleasePackageId  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return EntryPackageId+'_'+ReleasePackageId;
      }

      public override string GetEntityPMName()
      {
          return "WarehouseEntryPackagesReleasePM";
      }
	 
   }

}
	 
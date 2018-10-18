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
   public partial class Category2Keys : EntityKeyFields
   {
   	  public string Id  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return Id;
      }

      public override string GetEntityPMName()
      {
          return "Category2PM";
      }
	 
   }

}
	 
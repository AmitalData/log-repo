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
   public partial class JournalStatusTypeKeys : EntityKeyFields
   {
   	  public string JournalStatusID  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return JournalStatusID;
      }

      public override string GetEntityPMName()
      {
          return "JournalStatusTypePM";
      }
	 
   }

}
	 
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure; 
  
namespace Logitude.CRM.Data.EntityKeys
{
   public partial class QuestionnaireQuestionKeys : EntityKeyFields
   {
   	  public string QuestioneerId  { get; set; }
	  
				 
	    			   
	  public int VersionNumber  { get; set; }
	  
				 
	    			   
	  public int QuestionNumber  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return QuestioneerId+'_'+VersionNumber+'_'+QuestionNumber;
      }

      public override string GetEntityPMName()
      {
          return "QuestionnaireQuestionPM";
      }
	 
   }

}
	 
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
   public partial class QuestionnaireAnswerLineKeys : EntityKeyFields
   {
   	  public string QuestionnaireAnswerId  { get; set; }
	  
				 
	    			   
	  public int QuestionNumber  { get; set; }
	  
				 
	    			   
	
	 
	  public override string GetFullKey()
      {
          return QuestionnaireAnswerId+'_'+QuestionNumber;
      }

      public override string GetEntityPMName()
      {
          return "QuestionnaireAnswerLinePM";
      }
	 
   }

}
	 
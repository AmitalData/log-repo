using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.CRM.Data.EntityPOCOs
{
   
    public class QuestionnaireAnswerLine
    {
	 string dbms;

        [Key]
        [ForeignKey("QuestionnaireAnswer")]
        [Column("QuestionnaireAnswerId" ,Order = 1)]
	    public string QuestionnaireAnswerId { get; set; }
	      
        public virtual QuestionnaireAnswer QuestionnaireAnswer { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("QuestionNumber" ,Order = 2)]
	    public int QuestionNumber { get; set; }
        [Column("AnswerValue")]
	    public string AnswerValue { get; set; }
    }
}
	 
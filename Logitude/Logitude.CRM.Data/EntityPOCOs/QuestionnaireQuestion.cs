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
   
    public class QuestionnaireQuestion
    {
	 string dbms;

        [Key]
        [ForeignKey("Questionnaire")]
        [Column("QuestioneerId" ,Order = 1)]
	    public string QuestioneerId { get; set; }
	      
        public virtual Questionnaire Questionnaire { get; set; }
     [Key]
        [Column("VersionNumber" ,Order = 2)]
	    public int VersionNumber { get; set; }
     [Key]
        [Column("QuestionNumber" ,Order = 3)]
	    public int QuestionNumber { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("Question")]
	    public string Question { get; set; }
        [Column("QuestionTypeCode")]
	    public string QuestionTypeCode { get; set; }
        [Column("IsMandatory")]
	    public bool IsMandatory { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("PickListCode")]
	    public string PickListCode { get; set; }
        [Column("IsAddOther")]
	    public bool IsAddOther { get; set; }
    }
}
	 
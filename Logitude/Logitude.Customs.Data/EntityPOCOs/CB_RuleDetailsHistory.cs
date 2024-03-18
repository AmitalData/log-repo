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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class CB_RuleDetailsHistory
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public int ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("Title")]
	    public string Title { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
        [ForeignKey("EntityStatusCode")]
        [Column("EntityStatusID")]
	    public string EntityStatusID { get; set; }
	      
        public virtual CustomsEntityStatus EntityStatusCode { get; set; }
        [ForeignKey("Rule")]
        [Column("RuleID")]
	    public int RuleID { get; set; }
	      
        public virtual CB_Rule Rule { get; set; }
        [Column("Rules")]
	    public string Rules { get; set; }
        [Column("EnglishRules")]
	    public string EnglishRules { get; set; }
        [Column("OrderinalPostion")]
	    public int OrderinalPostion { get; set; }
        [ForeignKey("ParentRuleDetailsHistoryID")]
        [Column("Parent_RuleDetailsHistoryID")]
	    public int? Parent_RuleDetailsHistoryID { get; set; }
	      
        public virtual CB_RuleDetailsHistory ParentRuleDetailsHistoryID { get; set; }
        [Column("ChangeRequestTypePriority")]
	    public int ChangeRequestTypePriority { get; set; }
        [Column("RulesRTF")]
	    public string RulesRTF { get; set; }
        [Column("EnglishRulesRTF")]
	    public string EnglishRulesRTF { get; set; }
    }
}
	 
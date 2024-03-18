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
   
    public class CB_RegularityInception
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public int ID { get; set; }
        [ForeignKey("RegularityRequirement")]
        [Column("RegularityRequirementID")]
	    public int RegularityRequirementID { get; set; }
	      
        public virtual CB_RegularityRequirement RegularityRequirement { get; set; }
        [ForeignKey("InterConditionsRelationshipCode")]
        [Column("InterConditionsRelationshipID")]
	    public string InterConditionsRelationshipID { get; set; }
	      
        public virtual InterConditionsRelationship InterConditionsRelationshipCode { get; set; }
        [Column("IsPersonalImportIncluded")]
	    public bool IsPersonalImportIncluded { get; set; }
        [Column("RequirementGoodsDescription")]
	    public string RequirementGoodsDescription { get; set; }
        [ForeignKey("RegularityRequirementWarningCode")]
        [Column("RegularityRequirementWarnID")]
	    public string RegularityRequirementWarnID { get; set; }
	      
        public virtual RegularityRequirementWarning RegularityRequirementWarningCode { get; set; }
        [Column("IsCarnetIncluded")]
	    public bool IsCarnetIncluded { get; set; }
    }
}
	 
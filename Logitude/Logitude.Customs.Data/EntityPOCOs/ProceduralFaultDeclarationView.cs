using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.EntityPOCOs
{
     [Table("ProceduralFaultDeclarationView", Schema = "dbo")]
    public class ProceduralFaultDeclarationView
    {
        [Key]
        [Required]
        [StringLength(15, MinimumLength = 0)]
        [Column("Id", TypeName = "varchar")]
        public string Id { get; set; }
        [Required]
        [Column("Tenant")]
        public int Tenant { get; set; }
        [StringLength(9, MinimumLength = 0)]
        [Column("ProceduralFaultNumber", TypeName = "varchar")]
        public string ProceduralFaultNumber { get; set; }
      
        [StringLength(2, MinimumLength = 0)]
        [Column("ProceduralFaultStatusCode", TypeName = "varchar")]
        public string ProceduralFaultStatusCode { get; set; }

       
        [Column("CreateDate")]
        public DateTime? CreateDate { get; set; }
     
        [StringLength(2, MinimumLength = 0)]
        [Column("InputTypeCode", TypeName = "varchar")]
        public string InputTypeCode { get; set; }

     
        [StringLength(2, MinimumLength = 0)]
        [Column("InspectionTypeCode", TypeName = "varchar")]
        public string InspectionTypeCode { get; set; }

    
        [StringLength(3, MinimumLength = 0)]
        [Column("ProceduralFaultCode", TypeName = "varchar")]
        public string ProceduralFaultCode { get; set; }

   
        [StringLength(2, MinimumLength = 0)]
        [Column("ProceduralFaultInputProcesCode", TypeName = "varchar")]
        public string ProceduralFaultInputProcesCode { get; set; }

       
        [StringLength(2, MinimumLength = 0)]
        [Column("RansomViolationTypeCode", TypeName = "varchar")]
        public string RansomViolationTypeCode { get; set; }

    
        public decimal? RansomViolationSum { get; set; }
        [StringLength(512, MinimumLength = 0)]
        [Column("Remarks", TypeName = "varchar")]
        public string Remarks { get; set; }
        [Column("IsCustomerResponsibility")]
        public bool IsCustomerResponsibility { get; set; }
        [Column("IsAgentProceduralFaultCountabl")]
        public bool IsAgentProceduralFaultCountabl { get; set; }
        [Column("IsCustProceduralFaultCountabl")]
        public bool IsCustProceduralFaultCountabl { get; set; }
        [Column("IsAgentResponsibility")]
        public bool IsAgentResponsibility { get; set; }
        [Column("UpdateDate")]
        public DateTime? UpdateDate { get; set; }
        [StringLength(5, MinimumLength = 0)]
        [Column("LeadingDocumentVersion", TypeName = "varchar")]
        public string LeadingDocumentVersion { get; set; }
        [StringLength(512, MinimumLength = 0)]
        [Column("Notes", TypeName = "varchar")]
        public string Notes { get; set; }
        [Column("IsCancelled")]
        public bool IsCancelled { get; set; }
        [Column("CancellationDate")]
        public DateTime? CancellationDate { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        [Column("SearchFields", TypeName = "nvarchar")]
        public string SearchFields { get; set; }
        
        [StringLength(15, MinimumLength = 0)]
        [Column("DeclarationId", TypeName = "varchar")]
        public string DeclarationId { get; set; }

        [StringLength(35, MinimumLength = 0)]
        [Column("DeclarationNumber", TypeName = "varchar")]
        public string DeclarationNumber { get; set; }

        [StringLength(12, MinimumLength = 0)]
        [Column("CustomFileNo", TypeName = "varchar")]
        public string CustomFileNo { get; set; }
     
        [StringLength(15, MinimumLength = 0)]
        [Column("CustomerId", TypeName = "varchar")]
        public string CustomerId { get; set; }

        [StringLength(100, MinimumLength = 0)]
        [Column("FaultInspectionName", TypeName = "nvarchar")]
        public string FaultInspectionName { get; set; }

        [StringLength(100, MinimumLength = 0)]
        [Column("ProceduralFaultInputSourceName", TypeName = "nvarchar")]
        public string ProceduralFaultInputSourceName { get; set; }

        [StringLength(100, MinimumLength = 0)]
        [Column("ProceduralFaultTypeName", TypeName = "nvarchar")]
        public string ProceduralFaultTypeName { get; set; }

        [StringLength(100, MinimumLength = 0)]
        [Column("ProceduralFaultInProcesTypName", TypeName = "nvarchar")]
        public string ProceduralFaultInProcesTypName { get; set; }

        [StringLength(100, MinimumLength = 0)]
        [Column("RansomViolationTypeName", TypeName = "nvarchar")]
        public string RansomViolationTypeName { get; set; }


        [StringLength(100, MinimumLength = 0)]
        [Column("ProceduralFaultStatuseName", TypeName = "nvarchar")]
        public string ProceduralFaultStatuseName { get; set; }


        [StringLength(100, MinimumLength = 0)]
        [Column("CustomerName", TypeName = "nvarchar")]
        public string CustomerName { get; set; }
	      
    }
}

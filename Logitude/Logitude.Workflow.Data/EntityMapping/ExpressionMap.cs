using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data;
 
namespace Logitude.Workflow.Data.EntityMapping
{
 
    public class ExpressionMap : EntityTypeConfiguration<Expression>
    {
	    string dbms;
        public ExpressionMap()
        { 
				this.ToTable("Expressions");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Body).HasColumnName("Body").HasMaxLength(1000).IsUnicode(false);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(1000).IsUnicode(false);

            this.Property(t => t.CategoryCode).HasColumnName("CategoryCode").IsRequired().HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 
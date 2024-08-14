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
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data;
 
namespace Logitude.Infrastructure.Data.EntityMapping
{
 
    public class BatchTaskExecutionStatusMap : EntityTypeConfiguration<BatchTaskExecutionStatus>
    {
	    string dbms;
        public BatchTaskExecutionStatusMap()
        { 
				this.ToTable("BatchTaskExecutionStatus");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 
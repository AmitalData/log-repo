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
 
    public class WorkFlowStatusMap : EntityTypeConfiguration<WorkFlowStatus>
    {
	    string dbms;
        public WorkFlowStatusMap()
        { 
				this.ToTable("WorkFlowStatuses");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 
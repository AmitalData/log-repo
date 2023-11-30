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
 
    public class WorkFlowInstanceActivityStatusMap : EntityTypeConfiguration<WorkFlowInstanceActivityStatus>
    {
	    string dbms;
        public WorkFlowInstanceActivityStatusMap()
        { 
		
      dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
      if (dbms == "oracle")
      {
		this.ToTable("WorkFlowInstanceActivityStatus");
      }
	  else
	  {
	    this.ToTable("WorkFlowInstanceActivityStatuses");
	  }

		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);
        }
    }
}
	 
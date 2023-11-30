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
 
    public class TaskExtendedMap : EntityTypeConfiguration<TaskExtended>
    {
	    string dbms;
        public TaskExtendedMap()
        { 
				this.ToTable("TasksExtended");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Fields).HasColumnName("Fields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.ToDoConditions).HasColumnName("ToDoConditions").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DoneConditions).HasColumnName("DoneConditions").IsMaxLength().IsUnicode(true);

            this.Property(t => t.Description).HasColumnName("Description").HasMaxLength(250).IsUnicode(true);
        }
    }
}
	 
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class NotificationTenantDefinitionMap : EntityTypeConfiguration<NotificationTenantDefinition>
    {
	    string dbms;
        public NotificationTenantDefinitionMap()
        { 
			  this.ToTable("NotificationTenantDefinition", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.Code).HasColumnName("Code").HasMaxLength(5).IsUnicode(false);

            this.Property(t => t.DefaultAssigneeId).HasColumnName("DefaultAssigneeId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 
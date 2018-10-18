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
 
    public class TeamMemberBusinessRoleMap : EntityTypeConfiguration<TeamMemberBusinessRole>
    {
	    string dbms;
        public TeamMemberBusinessRoleMap()
        { 
				this.ToTable("TeamMemberBusinessRoles");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.TeamMemberId).HasColumnName("TeamMemberId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AddedByUserId).HasColumnName("AddedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AddDate).HasColumnName("AddDate").IsRequired();

            this.Property(t => t.BusinessRoleId).HasColumnName("BusinessRoleId").IsRequired().HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 
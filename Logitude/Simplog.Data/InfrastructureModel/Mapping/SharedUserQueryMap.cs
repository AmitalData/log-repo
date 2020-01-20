using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class SharedUserQueryMap : EntityTypeConfiguration<SharedUserQuery>
    {
        public SharedUserQueryMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UserId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QueryId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.QueryCode).IsRequired().HasMaxLength(200).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("SharedUserQueries");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.QueryId).HasColumnName("QueryId");
            this.Property(t => t.QueryCode).HasColumnName("QueryCode");

            this.HasRequired(t => t.User).WithMany().HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.Query).WithMany().HasForeignKey(d => d.QueryId);
        }
    }
}

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class SessionPolicyMap : EntityTypeConfiguration<SessionPolicy>
    {
        public SessionPolicyMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                 .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("SessionPolicies");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WebTokenLifeTime).HasColumnName("WebTokenLifeTime");
            this.Property(t => t.WebTokenExpirationWarning).HasColumnName("WebTokenExpirationWarning");

        }
    }
}

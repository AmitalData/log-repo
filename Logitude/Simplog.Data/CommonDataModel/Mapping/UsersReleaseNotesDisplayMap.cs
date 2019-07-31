using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UsersReleaseNotesDisplayMap : EntityTypeConfiguration<UsersReleaseNotesDisplay>
    {
        public UsersReleaseNotesDisplayMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UserId).IsRequired().HasMaxLength(15).IsUnicode(false);            

            this.ToTable("UsersReleaseNotesDisplays");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.UserId).HasColumnName("UserId");

            this.HasRequired(t => t.User).WithMany().HasForeignKey(d => d.UserId);
        }
    }
}

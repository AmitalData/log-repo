using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class GlobalDBMap : EntityTypeConfiguration<GlobalDB>
    {
        public GlobalDBMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(true);
            this.Property(t => t.DBConnection).IsRequired().HasMaxLength(512).IsUnicode(true);
            this.Property(t => t.SharedDWConnection).IsRequired().HasMaxLength(512).IsUnicode(true);
            this.Property(t => t.SecondaryAzureDBConnection).IsRequired().HasMaxLength(512).IsUnicode(true);

            this.ToTable("GlobalDBs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DBConnection).HasColumnName("DBConnection");
            this.Property(t => t.IsUpgrading).HasColumnName("IsUpgrading");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.SharedDWConnection).HasColumnName("SharedDWConnection");
            this.Property(t => t.SecondaryAzureDBConnection).HasColumnName("SecondaryAzureDBConnection");
            this.Property(t => t.IsBlocking).HasColumnName("IsBlocking");
        }
    }
}

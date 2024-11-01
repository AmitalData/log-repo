using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ImageDetailMap : EntityTypeConfiguration<ImageDetail>
    {
        public ImageDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Extension)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ImageDetails");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Extension).HasColumnName("Extension");

//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Size).HasColumnName("ImageSize");
            }
            //#else
            else
            {
                this.Property(t => t.Size).HasColumnName("Size");
            }
//#endif
            
        }
    }
}

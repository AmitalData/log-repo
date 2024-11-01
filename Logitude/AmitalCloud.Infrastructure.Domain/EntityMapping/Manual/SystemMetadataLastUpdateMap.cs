using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class SystemMetadataLastUpdateMap : EntityTypeConfiguration<SystemMetadataLastUpdate>
    {
        public SystemMetadataLastUpdateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.TranslationsUpdateDateGMT).HasColumnName("TranslationsUpdateDateGMT");
            this.Property(t => t.ObjectFieldsUpdateDateGMT).HasColumnName("ObjectFieldsUpdateDateGMT");
        }
    }
}

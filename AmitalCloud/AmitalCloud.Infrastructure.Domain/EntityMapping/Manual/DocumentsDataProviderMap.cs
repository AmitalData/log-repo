using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
  public  class DocumentsDataProviderMap: EntityTypeConfiguration<DocumentsDataProvider>
    {
        
        public DocumentsDataProviderMap()
        {      // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DocumentsDataProviders");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");













       //[ForeignKey("DocumentsDataProviderCode")]
       // public virtual DocumentsDataProvider DocumentsDataProvider { get; set; }
         }
    }
}

using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateSectionTypeMap : EntityTypeConfiguration<QuoteTemplateSectionType>
    {
        public QuoteTemplateSectionTypeMap()
        {
            this.HasKey(t => t.Code);
            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(60)
                .IsUnicode(false);

    
         
            // Table & Column Mappings
            this.ToTable("QuoteTemplateSectionTypes");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
  
            

        }
    }
}

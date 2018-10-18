using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateTableDesignMap : EntityTypeConfiguration<QuoteTemplateTableDesign>
    {


        public QuoteTemplateTableDesignMap()
        {


            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)

                 .IsRequired();

  

            this.Property(t => t.BorderTypeCode)
                .IsRequired()
                .HasMaxLength(40)
                 .IsUnicode(false);

            this.Property(t => t.BorderColor)
                 .IsRequired()
                 .HasMaxLength(10)
                 .IsUnicode(false);


            this.Property(t => t.BorderThickness)
                 .IsRequired();


            this.Property(t => t.HeaderDesignId)
                 .IsRequired()
                 .HasMaxLength(15)
                 .IsUnicode(false);


            this.Property(t => t.LinesDesignId)
                 .IsRequired()
                  .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.GroupByDesignId)
                 .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("QuoteTemplateTableDesigns");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.BorderColor).HasColumnName("BorderColor");
            this.Property(t => t.BorderTypeCode).HasColumnName("BorderTypeCode");
            this.Property(t => t.HeaderDesignId).HasColumnName("HeaderDesignId");
            this.Property(t => t.GroupByDesignId).HasColumnName("GroupByDesignId");

            this.Property(t => t.LinesDesignId).HasColumnName("LinesDesignId");

            // Relationships
            this.HasRequired(t => t.HeaderDesign)
                .WithMany()
                .HasForeignKey(d => d.HeaderDesignId);


            this.HasRequired(t => t.GroupDesign)
           .WithMany()
           .HasForeignKey(d => d.GroupByDesignId);


            this.HasRequired(t => t.LinesDesign)
           .WithMany()
           .HasForeignKey(d => d.LinesDesignId);


            this.HasRequired(t => t.BorderType)
           .WithMany()
          .HasForeignKey(d => d.BorderTypeCode); 
        }

    }
}

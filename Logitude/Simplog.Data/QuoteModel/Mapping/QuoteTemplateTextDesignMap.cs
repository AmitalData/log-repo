using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateTextDesignMap : EntityTypeConfiguration<QuoteTemplateTextDesign>
    {

        public QuoteTemplateTextDesignMap()
        {
            // Primary Key

            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.TextColor)
             .HasMaxLength(10)
             .IsUnicode(false);


            this.Property(t => t.FontFamily)
             .HasMaxLength(100)
             .IsUnicode(false);

            this.Property(t => t.BackgroundColor)
            .HasMaxLength(10)
            .IsUnicode(false);


            this.Property(t => t.FontWeight)
            .HasMaxLength(40)
            .IsUnicode(false);

            this.Property(t => t.Alignment)
            .HasMaxLength(10)
            .IsUnicode(false);


            this.Property(t => t.Italic)
                    .IsRequired();

            this.Property(t => t.UnDerLine)
                    .IsRequired();

            this.Property(t => t.FontSize)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("QuoteTemplateTextDesigns");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FontSize).HasColumnName("FontSize");
            this.Property(t => t.FontFamily).HasColumnName("FontFamily");
            this.Property(t => t.BackgroundColor).HasColumnName("BackgroundColor");
            this.Property(t => t.Italic).HasColumnName("Italic");

            this.Property(t => t.UnDerLine).HasColumnName("UnDerLine");
            this.Property(t => t.TextColor).HasColumnName("TextColor");
            this.Property(t => t.FontWeight).HasColumnName("FontWeight");

            this.Property(t => t.Alignment).HasColumnName("Alignment");
        }

    }
}

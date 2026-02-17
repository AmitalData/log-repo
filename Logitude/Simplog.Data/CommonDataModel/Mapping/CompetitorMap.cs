using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CompetitorMap : EntityTypeConfiguration<Competitor>
    {
        public CompetitorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .HasMaxLength(60)
                .IsUnicode(true);

            this.Property(t => t.Website)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Strengths)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Weaknesses)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Opportunity)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.Threat)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.AddressId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("Competitors");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Website).HasColumnName("Website");
            this.Property(t => t.Strengths).HasColumnName("Strengths");
            this.Property(t => t.Weaknesses).HasColumnName("Weaknesses");
            this.Property(t => t.Opportunity).HasColumnName("Opportunity");
            this.Property(t => t.Threat).HasColumnName("Threat");
            this.Property(t => t.AddressId).HasColumnName("AddressId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.InActive).HasColumnName("InActive");      
           
            // Relationships
            this.HasOptional(t => t.Address)
                .WithMany()
                .HasForeignKey(d => d.AddressId);
        }
    }
}

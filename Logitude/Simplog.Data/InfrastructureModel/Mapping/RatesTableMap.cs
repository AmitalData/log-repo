using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class RatesTableMap : EntityTypeConfiguration<RatesTable>
    {
        public RatesTableMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BaseCurrencyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ForeignCurrencyId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("RatesTables");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.BaseCurrencyId).HasColumnName("BaseCurrencyId");
            this.Property(t => t.ForeignCurrencyId).HasColumnName("ForeignCurrencyId");
            this.Property(t => t.LogDateTime).HasColumnName("LogDateTime");

            // Relationships
            this.HasRequired(t => t.BaseCurrency)
                .WithMany()
                .HasForeignKey(d => d.BaseCurrencyId);
            this.HasRequired(t => t.ForeignCurrency)
                .WithMany()
                .HasForeignKey(d => d.ForeignCurrencyId)
                .WillCascadeOnDelete(false);

        }
    }
}

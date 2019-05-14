using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class CounterDefinitionMap : EntityTypeConfiguration<CounterDefinition>
    {
        public CounterDefinitionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Parameter1)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.Parameter2)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.Prefix)
                .HasMaxLength(20)
                .IsUnicode(false);

			this.Property(t => t.Suffix)
			   .HasMaxLength(20)
			   .IsUnicode(false);

			this.Property(t => t.CounterId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CounterDefinitions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Parameter1).HasColumnName("Parameter1");
            this.Property(t => t.Parameter2).HasColumnName("Parameter2");
            this.Property(t => t.Prefix).HasColumnName("Prefix");
            this.Property(t => t.UniquePerPrefix).HasColumnName("UniquePerPrefix");
            this.Property(t => t.StartNumber).HasColumnName("StartNumber");
            this.Property(t => t.CounterId).HasColumnName("CounterId");
			this.Property(t => t.CounterSize).HasColumnName("CounterSize");
			this.Property(t => t.Suffix).HasColumnName("Suffix");

			// Relationships
			//this.HasRequired(t => t.Counter)
			//    .WithMany(t => t.CounterDefinitions)
			//    .HasForeignKey(d => d.CounterId);

		}
	}
}

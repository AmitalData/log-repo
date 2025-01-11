using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class CounterMap : EntityTypeConfiguration<Counter>
    {
        public CounterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.ChangedByUserId)
              
              .HasMaxLength(15)
              .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("Counters");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");

            this.Property(t => t.ChangedByUserId).HasColumnName("ChangedByUserId");
            // Relationships
            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.Counters)
            //    .HasForeignKey(d => d.ObjectTableId);

            this.HasOptional(t => t.User)
               .WithMany()
               .HasForeignKey(d => d.ChangedByUserId);

        }
    }
}

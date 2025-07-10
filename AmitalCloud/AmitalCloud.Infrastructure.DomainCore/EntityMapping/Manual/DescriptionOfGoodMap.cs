using AmitalCloud.Infrastructure.Domain.EntityClasses;
using System.Data.Entity.ModelConfiguration;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class DescriptionOfGoodMap : EntityTypeConfiguration<DescriptionOfGoods>
    {
        public DescriptionOfGoodMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DescriptionOfGood)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("DescriptionOfGoods");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.DescriptionOfGood).HasColumnName("DescriptionOfGood");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.InActive).HasColumnName("InActive");
        }
    }
}

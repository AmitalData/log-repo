using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class QueryGroupMap : EntityTypeConfiguration<QueryGroup>
    {
        public QueryGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                //.HasMaxLength(40)
                .HasMaxLength(160) //Itzik 
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("QueryGroups");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Domain.EntityMapping
{
    public class ContactLastLoginMap : EntityTypeConfiguration<ContactLastLogin>
    {
        public ContactLastLoginMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ComputerId)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ContactLastLogins");
            this.Property(t => t.ComputerId).HasColumnName("ComputerId");
            this.Property(t => t.LoginDateTime).HasColumnName("LoginDateTime");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");

            // Relationships
            this.HasRequired(t => t.Contact)
                //.WithOptional(t => t.ContactLastLogin)  TODO Vladi Relation to Contact
                ;

        }
    }
}

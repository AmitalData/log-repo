using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ConfirmationNumberDefaultMap : EntityTypeConfiguration<ConfirmationNumberDefault>
    {
        public ConfirmationNumberDefaultMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired();



            this.Property(t => t.Tenant)
                .IsRequired();
           

            this.Property(t => t.FromDate)
              .IsRequired();

            this.Property(t => t.AmountForConfirmationNumber)
           .IsRequired();

            // Table & Column Mappings
            this.ToTable("ConfirmationNumberDefaults");
            this.Property(t => t.AmountForConfirmationNumber).HasColumnName("AmountForConfirmationNumber");
            this.Property(t => t.FromDate).HasColumnName("FromDate");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Id).HasColumnName("Id");

        }


      
    }
}

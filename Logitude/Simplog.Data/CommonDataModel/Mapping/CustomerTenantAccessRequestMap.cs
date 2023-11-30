using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerTenantAccessRequestMap : EntityTypeConfiguration<CustomerTenantAccessRequest>
    {
       public CustomerTenantAccessRequestMap()
       {

           this.HasKey(t => t.Id);
           this.Property(t => t.Id)
           .IsRequired()
           .HasMaxLength(15)
           .IsUnicode(false);

           this.Property(t => t.Tenant).IsRequired();


           this.Property(t => t.ForwarderId)
          .IsRequired()
          .HasMaxLength(15)
          .IsUnicode(false);

           this.Property(t => t.RequestStatus)
          .IsRequired()
          .HasMaxLength(2)
          .IsUnicode(false);

           this.Property(t => t.RequestDateTime)
               .IsRequired();

           // Table & Column Mappings

           this.ToTable("CustomerTenantAccessRequests");
           this.Property(t => t.Id).HasColumnName("Id");
           this.Property(t => t.Tenant).HasColumnName("Tenant");
           this.Property(t => t.ForwarderId).HasColumnName("ForwarderId");
           this.Property(t => t.RequestDateTime).HasColumnName("RequestDateTime");
           this.Property(t => t.RequestStatus).HasColumnName("RequestStatus");
            this.Property(t => t.IsCustoms).HasColumnName("IsCustoms");
            this.Property(t => t.IsExport).HasColumnName("IsExport");


            //relationships
            this.HasRequired(t => t.RequestStatusCode).WithMany().HasForeignKey(d => d.RequestStatus);
           this.HasRequired(t => t.HybridPartnerId).WithMany().HasForeignKey(d => d.ForwarderId);


       }
    }
}

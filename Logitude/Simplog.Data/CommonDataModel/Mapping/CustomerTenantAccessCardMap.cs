using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerTenantAccessCardMap : EntityTypeConfiguration<CustomerTenantAccessCard>
    {
        public CustomerTenantAccessCardMap()
        {

            this.HasKey(t => new {t.CustomerId,t.CustomerTenantAccessId});


            this.Property(t => t.CustomerId)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.CustomerTenantAccessId)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.CreateByUserId)         
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.CreateDate)
            .IsRequired();

           // this.Property(t => t.HybridStartDate)
           //.IsRequired();


            // Table & Column Mappings
            this.ToTable("CustomerTenantAccessCards");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerTenantAccessId).HasColumnName("CustomerTenantAccessId");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.LastShipmentDateInQueue).HasColumnName("LastShipmentDateInQueue");
            this.Property(t => t.CreateByUserId).HasColumnName("CreateByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate"); 
            //this.Property(t => t.HybridStartDate).HasColumnName("HybridStartDate");
            this.Property(t => t.LastMappingDateTime).HasColumnName("LastMappingDateTime");
            this.Property(t => t.UpdateDateTime).HasColumnName("UpdateDateTime");
            this.Property(t => t.StatusTypeCode).HasColumnName("StatusTypeCode");
            this.Property(t => t.IsExportActivated).HasColumnName("IsExportActivated");
            this.Property(t => t.IsImportActivated).HasColumnName("IsImportActivated");



            //relationships

            this.HasOptional(t => t.CreateByUser).WithMany().HasForeignKey(d => d.CreateByUserId);
            this.HasRequired(t => t.Customer).WithMany().HasForeignKey(d => d.CustomerId);
            this.HasRequired(t => t.CustomerTenantAccess).WithMany().HasForeignKey(d => d.CustomerTenantAccessId);


         }
 
    }
}

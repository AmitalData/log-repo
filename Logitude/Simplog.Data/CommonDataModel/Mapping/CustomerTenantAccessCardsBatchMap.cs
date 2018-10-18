using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CustomerTenantAccessCardsBatchMap : EntityTypeConfiguration<CustomerTenantAccessCardsBatch>
    {
        public CustomerTenantAccessCardsBatchMap()
        {
            this.HasKey(t => new { t.CustomerId, t.CustomerTenantAccessId, t.BatchNumber });


            this.Property(t => t.CustomerId)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.CustomerTenantAccessId)
            .IsRequired()
            .HasMaxLength(15)
            .IsUnicode(false);

            this.Property(t => t.BatchNumber).IsRequired()
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);

            this.Property(t => t.Status)
            .HasMaxLength(15)
            .IsRequired()
            .IsUnicode(false);


            this.Property(t => t.CreateDateTime)
            .IsRequired();


            // Table & Column Mappings
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.ToTable("CustomerTenantAccessCardBatchs");
            }
            else
            {
                this.ToTable("CustomerTenantAccessCardsBatches");
            }
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.CustomerTenantAccessId).HasColumnName("CustomerTenantAccessId");
            this.Property(t => t.BatchNumber).HasColumnName("BatchNumber");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
            this.Property(t => t.FromDatetime).HasColumnName("FromDatetime");
            this.Property(t => t.ToDatetime).HasColumnName("ToDatetime");
            this.Property(t => t.TotalShipment).HasColumnName("TotalShipment");
            this.Property(t => t.Totalsucceeded).HasColumnName("Totalsucceeded");
            this.Property(t => t.TotalFailed).HasColumnName("TotalFailed");


            //relationships

            this.HasRequired(t => t.Customer).WithMany().HasForeignKey(d => d.CustomerId);
            this.HasRequired(t => t.CustomerTenantAccess).WithMany().HasForeignKey(d => d.CustomerTenantAccessId);
        }
    }
}

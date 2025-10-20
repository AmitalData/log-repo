using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ExpenseAllocationFlowMap : EntityTypeConfiguration<ExpenseAllocationFlow>
    {

        public ExpenseAllocationFlowMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);          

            this.Property(t => t.SettingId).HasMaxLength(30).IsUnicode(false);
            this.Property(t => t.JournalId).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Status).HasMaxLength(30).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ExpenseAllocationFlows");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.RunDate).HasColumnName("RunDate");
            this.Property(t => t.SettingId).HasColumnName("SettingId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.JournalId).HasColumnName("JournalId");
      
        }

    }
}

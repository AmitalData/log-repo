using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ExpenseAllocationSettingMap : EntityTypeConfiguration<ExpenseAllocationSetting>
    {

        public ExpenseAllocationSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);          

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);
            this.Property(t => t.EntityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PaymentDateType).HasMaxLength(10).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("ExpenseAllocationSettingMaps");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.StartDateTime).HasColumnName("StartDateTime");
            this.Property(t => t.EndDateTime).HasColumnName("EndDateTime");
            this.Property(t => t.NumberOfPayments).HasColumnName("NumberOfPayments");
            this.Property(t => t.MonthInterval).HasColumnName("MonthInterval");
            this.Property(t => t.PaymentDateType).HasColumnName("PaymentDateType");
        }

    }
}

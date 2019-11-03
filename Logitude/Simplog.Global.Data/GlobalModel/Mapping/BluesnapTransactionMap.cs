using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class BluesnapTransactionMap : EntityTypeConfiguration<BluesnapTransaction>
    {
        public BluesnapTransactionMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.DocumentId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CreateDate).IsRequired();
            this.Property(t => t.TransactionDate);
            this.Property(t => t.LogitudeAmital).IsRequired().IsUnicode(true).HasMaxLength(50);

            this.ToTable("BluesnapTransactions");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.TransactionDate).HasColumnName("TransactionDate");
            this.Property(t => t.LogitudeAmital).HasColumnName("LogitudeAmital");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
        }
    }
}

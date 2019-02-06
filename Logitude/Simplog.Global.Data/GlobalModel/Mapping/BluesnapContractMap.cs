using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class BluesnapContractMap : EntityTypeConfiguration<BluesnapContract>
    {
        public BluesnapContractMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Code).IsRequired().HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.ContractId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
            this.Property(t => t.BluesnapContractTypeCode).IsRequired().HasMaxLength(4).IsUnicode(false);

            this.ToTable("BluesnapContracts");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ContractId).HasColumnName("ContractId");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.BluesnapContractTypeCode).HasColumnName("BluesnapContractTypeCode");
        }
    }
}

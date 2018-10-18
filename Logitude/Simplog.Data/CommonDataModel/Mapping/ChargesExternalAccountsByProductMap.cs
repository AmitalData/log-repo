using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ChargesExternalAccountsByProductMap : EntityTypeConfiguration<ChargesExternalAccountsByProduct>
    {
        public ChargesExternalAccountsByProductMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.PayablesGLAccount).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.PayablesCostCenter).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ReceivablesGLAccount).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ReceivablesCostCenter).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.ChargesTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ProductTypeCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.UpdatedByUserId).IsRequired().HasMaxLength(15).IsUnicode(false);

            
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.PayablesGLAccount).HasColumnName("PayablesGLAccount");
            this.Property(t => t.PayablesCostCenter).HasColumnName("PayablesCostCenter");
            this.Property(t => t.ReceivablesGLAccount).HasColumnName("ReceivablesGLAccount");
            this.Property(t => t.ReceivablesCostCenter).HasColumnName("ReceivablesCostCenter");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.ChargesTypeId).HasColumnName("ChargesTypeId");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");

            this.HasRequired(t => t.ChargesType).WithMany().HasForeignKey(d => d.ChargesTypeId);
            this.HasRequired(t => t.ProductType).WithMany().HasForeignKey(d => d.ProductTypeCode);
            this.HasRequired(t => t.UpdatedByUser).WithMany().HasForeignKey(d => d.UpdatedByUserId);

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.ToTable("ChargesExtAccountsByProducts");
            }
            //#else
            else
            {
                this.ToTable("ChargesExternalAccountsByProducts");
            }
            //#endif
        }
    }
}

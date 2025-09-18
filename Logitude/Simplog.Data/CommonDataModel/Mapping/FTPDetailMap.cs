using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class FTPDetailMap : EntityTypeConfiguration<FTPDetail>
    {
        public FTPDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Password)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Host)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.Folder)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.UpdatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);
            
            // Table & Column Mappings
            this.ToTable("FTPDetails");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");            
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Host).HasColumnName("Host");
            this.Property(t => t.Folder).HasColumnName("Folder");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.InActive).HasColumnName("InActive");
			this.Property(t => t.PrivateKey).HasColumnName("PrivateKey");
			this.Property(t => t.Port).HasColumnName("Port");

		}
	}
}

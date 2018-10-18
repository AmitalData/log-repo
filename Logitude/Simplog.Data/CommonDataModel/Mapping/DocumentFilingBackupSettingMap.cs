using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class DocumentFilingBackupSettingMap : EntityTypeConfiguration<DocumentFilingBackupSetting>
    {
       public DocumentFilingBackupSettingMap()
        {
            this.HasKey(t => t.Tenant);
            this.Property(t => t.Tenant).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.FTPDetailId).HasMaxLength(15).IsUnicode(false);
           

            this.ToTable("DocumentFilingBackupSettings");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.ActivationDate).HasColumnName("ActivationDate");
            this.Property(t => t.DeactivationDate).HasColumnName("DeactivationDate");
            this.Property(t => t.FTPDetailId).HasColumnName("FTPDetailId");

            this.HasOptional(t => t.FTPDetail).WithMany().HasForeignKey(d => d.FTPDetailId);
             
        }
    }
}

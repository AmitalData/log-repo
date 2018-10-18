using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AutomationHistoryMap : EntityTypeConfiguration<AutomationHistory>
    {
        public AutomationHistoryMap()
        {
            // Primary Key
              this.HasKey(t => new { t.Version, t.AutomationsId });

            // Properties
            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.AutomationsId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Version)
                .IsRequired();

            this.Property(t => t.AutomationXML)
                .IsMaxLength()
                .IsUnicode(true);


            this.Property(t => t.CreateDate);
       



            // Table & Column Mappings
            this.ToTable("AutomationHistorys");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AutomationsId).HasColumnName("AutomationsId");
            this.Property(t => t.AutomationXML).HasColumnName("AutomationXML");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            
            this.HasRequired(t => t.Automation)
                .WithMany()
                .HasForeignKey(d => d.AutomationsId);


        }
    }
}

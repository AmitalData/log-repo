using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class EmailAlertSettingMap : EntityTypeConfiguration<EmailAlertSetting>
    {


        public EmailAlertSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();

            this.Property(t => t.Code)
               .IsRequired()
               .HasMaxLength(4)
               .IsUnicode(false);

            this.Property(t => t.Description)
              .HasMaxLength(500)
              .IsUnicode(false);


            this.Property(t => t.InActive)
                .IsRequired();

            this.Property(t => t.SettingLevelCode)
            .IsRequired()
            .HasMaxLength(4)
            .IsUnicode(false);

            this.Property(t => t.To)
             .HasMaxLength(1000)
             .IsUnicode(false);


            this.Property(t => t.IndexOrder)
               .IsRequired();
            //this.Property(t => t.ObjectTableId)
            //   .HasMaxLength(15)
            //    .IsRequired()
            //   .IsUnicode(false);


            // Table & Column Mappings
            this.ToTable("EmailAlertSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description).HasColumnName("Description");

            this.Property(t => t.InActive).HasColumnName("InActive");
            this.Property(t => t.SettingLevelCode).HasColumnName("SettingLevelCode");
            
            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");

//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.To).HasColumnName("To_");
            }
            //#else
            else
            {
                this.Property(t => t.To).HasColumnName("To");
            }
//#endif
            
            //this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");


            // Relationships

            //this.HasRequired(t => t.ObjectTable)
            //    .WithMany(t => t.EventTypes)
            //    .HasForeignKey(d => d.ObjectTableId);


            this.HasOptional(t => t.ObjectTable)
                 .WithMany()
                  .HasForeignKey(d => d.ObjectTableId);

        }

    }
}

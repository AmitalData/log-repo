using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
   public class AutomationResultEmailRecipientMap : EntityTypeConfiguration<AutomationResultEmailRecipient>
    {
       public AutomationResultEmailRecipientMap()
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

            this.Property(t => t.AutomationsId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
          
           this.Property(t => t.RecipientType)
               .HasMaxLength(8)
               .IsUnicode(false);


            this.Property(t => t.PartnerObjectFieldCode)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(t => t.RecipientValue)
        
            .HasMaxLength(200)
            .IsUnicode(false);

            // Table & Column Mappings
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.ToTable("AutomationResultEmailRecips");
            }
            else
            {
            this.ToTable("AutomationResultEmailRecipients");
              }
            
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.AutomationsId).HasColumnName("AutomationsId");
            this.Property(t => t.RecipientType).HasColumnName("RecipientType");
            this.Property(t => t.RecipientValue).HasColumnName("RecipientValue");
            this.Property(t => t.PartnerObjectFieldCode).HasColumnName("PartnerObjectFieldCode");
            this.Property(t => t.IsNotifyBack).HasColumnName("IsNotifyBack");




            this.HasRequired(t => t.Automation)
                .WithMany()
                .HasForeignKey(d => d.AutomationsId);
          }
    }
}

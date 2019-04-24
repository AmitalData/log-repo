using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AutomationMap : EntityTypeConfiguration<Automation>
    {

       public AutomationMap()
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

            this.Property(t => t.Version);
           
              this.Property(t => t.AutomationXML)
                 .IsMaxLength()
                 .IsUnicode(true);
          
            this.Property(t => t.ObjectTableId)
                  .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);




            this.Property(t => t.Type)
                 .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(t => t.TemplateId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentTypeId)
                .HasMaxLength(15)
                .IsUnicode(false);

           


            this.Property(t => t.ResultCode)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.Description)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.Inactive);
       
            this.Property(t => t.CreateDate);
            this.Property(t => t.UpdateDate);


            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);


            this.Property(t => t.UpdatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

                 this.Property(t => t.From)
                .HasMaxLength(20)
                .IsUnicode(false);

                 this.Property(t => t.FromEmail)
                .HasMaxLength(30)
                .IsUnicode(false);

                 this.Property(t => t.Order);

            this.Property(t => t.Code)
                .HasMaxLength(7)
                .IsUnicode(false);



            // Table & Column Mappings
            this.ToTable("Automations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.UpdatedByUserId).HasColumnName("UpdatedByUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Inactive).HasColumnName("Inactive");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.AutomationXML).HasColumnName("AutomationXML");
            this.Property(t => t.FromEmail).HasColumnName("FromEmail");
            this.Property(t => t.Code).HasColumnName("Code");

            this.Property(t => t.IsAutomationDone).HasColumnName("IsAutomationDone");
              string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
              if (dbms == "oracle")
              {
                  this.Property(t => t.From).HasColumnName("From1");
                  this.Property(t => t.Order).HasColumnName("ExecutionOrder");

              }
              else
              {
            this.Property(t => t.From).HasColumnName("From");
            this.Property(t => t.Order).HasColumnName("Order");

              }


            this.HasOptional(t => t.CreatedByUser)
           .WithMany()
           .HasForeignKey(d => d.CreatedByUserId);


            this.HasOptional(t => t.UpdatedByUser)
           .WithMany()
           .HasForeignKey(d => d.UpdatedByUserId);










          }
    }
}

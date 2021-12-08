using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
  public  class EntityChangeMap : EntityTypeConfiguration<EntityChange>
    {

      public EntityChangeMap()
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
        
                 this.Property(t => t.ObjectTableId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

              this.Property(t => t.EntityId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

              this.Property(t => t.CreateByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

              this.Property(t => t.CreateDate);
              this.Property(t => t.CheckStartDate);
              this.Property(t => t.DoneDate);


              this.Property(t => t.AutomationConditionFieldsXml)
              .IsMaxLength()
              .IsUnicode(true);

              this.Property(t => t.SetAutomationFailedXml)
              .IsMaxLength()
              .IsUnicode(true);

              this.Property(t => t.EmailAutomationFailedXml)
                  .IsMaxLength()
                  .IsUnicode(true);



              this.Property(t => t.SetAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);


              this.Property(t => t.EmailAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);


              this.Property(t => t.ChangesFieldsXml)
                 .IsMaxLength()
                 .IsUnicode(true);


              this.Property(t => t.ChangesAutomationFieldsXml)
                 .IsMaxLength()
                 .IsUnicode(true);

              this.Property(t => t.HasExecutedRecord);



            this.Property(t => t.SetSLAAutomationFailedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.SetSLAAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.FollowUpAutomationFailedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.FollowUpAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);
                      
            
            this.Property(t => t.SendInterfaceAutomationFailedXml)
            .IsMaxLength()
            .IsUnicode(true);

            this.Property(t => t.SendInterfaceAutomationSsucceedXml)

                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.SendDocumentAutomationFailedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.SendDocumentAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);



            this.Property(t => t.CreateTaskAutomationFailedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.CreateTaskAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.OnUpdateDocumentAutomationFailedXml)
                .IsMaxLength()
                .IsUnicode(true);

            this.Property(t => t.OnUpdateDocumentAutomationSsucceedXml)
                .IsMaxLength()
                .IsUnicode(true);



            // Table & Column Mappings
            this.ToTable("EntityChanges");

              this.Property(t => t.Id).HasColumnName("Id");
              this.Property(t => t.Tenant).HasColumnName("Tenant");
              this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
              this.Property(t => t.EntityId).HasColumnName("EntityId");
              this.Property(t => t.CreateByUserId).HasColumnName("CreateByUserId");
              this.Property(t => t.CreateDate).HasColumnName("CreateDate");
              this.Property(t => t.CheckStartDate).HasColumnName("CheckStartDate");
              this.Property(t => t.DoneDate).HasColumnName("DoneDate");

              this.Property(t => t.AutomationConditionFieldsXml).HasColumnName("AutomationConditionFieldsXml");
              this.Property(t => t.SetAutomationFailedXml).HasColumnName("SetAutomationFailedXml");
              this.Property(t => t.SetAutomationSsucceedXml).HasColumnName("SetAutomationSsucceedXml");
              this.Property(t => t.EmailAutomationSsucceedXml).HasColumnName("EmailAutomationSsucceedXml");

              this.Property(t => t.ChangesFieldsXml).HasColumnName("ChangesFieldsXml");
              this.Property(t => t.ChangesAutomationFieldsXml).HasColumnName("ChangesAutomationFieldsXml");
            
              this.Property(t => t.HasExecutedRecord).HasColumnName("HasExecutedRecord");
              this.Property(t => t.ExecutionTime).HasColumnName("ExecutionTime");


            this.Property(t => t.SetSLAAutomationFailedXml).HasColumnName("SetSLAAutomationFailedXml");
            this.Property(t => t.SetSLAAutomationSsucceedXml).HasColumnName("SetSLAAutomationSsucceedXml");
            this.Property(t => t.FollowUpAutomationFailedXml).HasColumnName("FollowUpAutomationFailedXml");
            this.Property(t => t.FollowUpAutomationSsucceedXml).HasColumnName("FollowUpAutomationSsucceedXml");


            this.Property(t => t.SendInterfaceAutomationFailedXml).HasColumnName("SendInterfaceAutomationFailedXml");
            this.Property(t => t.SendInterfaceAutomationSsucceedXml).HasColumnName("SendInterfaceAutomationSsucceedXml");
            this.Property(t => t.SendDocumentAutomationFailedXml).HasColumnName("SendDocumentAutomationFailedXml");
            this.Property(t => t.SendDocumentAutomationSsucceedXml).HasColumnName("SendDocumentAutomationSsucceedXml");
            this.Property(t => t.OnUpdateDocumentAutomationFailedXml).HasColumnName("OnUpdateDocumentAutomationFailedXml");
            this.Property(t => t.OnUpdateDocumentAutomationSsucceedXml).HasColumnName("OnUpdateDocumentAutomationSsucceedXml");



            this.Property(t => t.CreateTaskAutomationFailedXml).HasColumnName("CreateTaskAutomationFailedXml");
            this.Property(t => t.CreateTaskAutomationSsucceedXml).HasColumnName("CreateTaskAutomationSsucceedXml");


            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.QueuedTaskAutomationSsucceedXml).HasColumnName("QueuedTaskAutomationSucceedXml");
            }
            //#elseelse
            else
            {
                this.Property(t => t.QueuedTaskAutomationSsucceedXml).HasColumnName("QueuedTaskAutomationSsucceedXml");
            }
            //#endif


            this.HasOptional(t => t.CreateByUser)
                  .WithMany()
                  .HasForeignKey(d => d.CreateByUserId);

 

        }
    }
}

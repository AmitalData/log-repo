using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Global.Data.GlobalModel.Mapping
{
    public class AnalyzeQueueMap : EntityTypeConfiguration<AnalyzeQueue>
    {
        public AnalyzeQueueMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                 .IsUnicode(false);

            this.Property(t => t.MessageBody)
                .IsRequired();

            this.Property(t => t.From)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.ErrorMessage)
                .HasMaxLength(8000)
                .IsUnicode(false);

            this.Property(t => t.Log)
              .HasMaxLength(500)
              .IsUnicode(false);

            this.Property(t => t.CommunicationLogId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Subject)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.EntityReference)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableName)
                .HasMaxLength(25)
                .IsUnicode(false);

            this.Property(t => t.StackTrace)
                .HasMaxLength(8000)
                .IsUnicode(false);

            this.Property(t => t.AWBNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.AckReason)
                .HasMaxLength(256)
                .IsUnicode(false);

            this.Property(t => t.FileName)

           .HasMaxLength(120)
           .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("AnalyzeQueues");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MessageBody).HasColumnName("MessageBody");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.DoneDate).HasColumnName("DoneDate");
            
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
            this.Property(t => t.Log).HasColumnName("Log");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Retries).HasColumnName("Retries");
            this.Property(t => t.ConnectedToTenant).HasColumnName("ConnectedToTenant");
            this.Property(t => t.ConnectedToEntity).HasColumnName("ConnectedToEntity");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.ObjectTableName).HasColumnName("ObjectTableName");
            this.Property(t => t.StackTrace).HasColumnName("StackTrace");
            this.Property(t => t.AWBNumber).HasColumnName("AWBNumber");
            this.Property(t => t.AckReason).HasColumnName("AckReason");
            this.Property(t => t.FileName).HasColumnName("FileName");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
            this.Property(t => t.From).HasColumnName("From_");
            }
            //#else
            else
             {
             this.Property(t => t.From).HasColumnName("From");
            }
//#endif
            // Relationships
            this.HasRequired(t => t.AnalyzeQueueStatus)
                .WithMany()
                .HasForeignKey(d => d.Status);

            this.HasRequired(t => t.TenantManagement)
                .WithMany()
                .HasForeignKey(d => d.Tenant);
        }
    }
}

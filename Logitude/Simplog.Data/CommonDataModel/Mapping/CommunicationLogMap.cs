using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class CommunicationLogMap : EntityTypeConfiguration<CommunicationLog>
    {
        public CommunicationLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.CreatedByUserId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Subject)
                .HasMaxLength(500)
                .IsUnicode(true);

            this.Property(t => t.InOut)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            this.Property(t => t.EntityId)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.CC)
                .HasMaxLength(4000)
                .IsUnicode(false);

            this.Property(t => t.DocumentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.DocumentOutId)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.DocumentsFilingId)
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.To)
                .HasMaxLength(4000)
                .IsUnicode(false);

            this.Property(t => t.CommunicationStatusTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.CommunicationLogTypeCode)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);

            this.Property(t => t.BCC)
                .HasMaxLength(4000)
                .IsUnicode(false);

            this.Property(t => t.ObjectTableId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.From)
                .HasMaxLength(1000)
                .IsUnicode(false);

            this.Property(t => t.EntityReference)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.ExceptionMessage)
             .HasMaxLength(8000)
             .IsUnicode(true);

            this.Property(t => t.Logs)
             .IsMaxLength()
             .IsUnicode(true);

            this.Property(t => t.CorrelationID)
             .HasMaxLength(64)
             .IsUnicode(false);

            this.Property(t => t.QueueName)
          .HasMaxLength(200)
          .IsUnicode(true);

            this.Property(t => t.MessageLockId)
         .HasMaxLength(40)
         .IsUnicode(false);


            this.Property(t => t.AWBNumber)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(t => t.ReplyToList)
            .HasMaxLength(500)
            .IsUnicode(false);

            this.Property(t => t.ChildEntityId)
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.ChildObjectTableId)
               .HasMaxLength(15)
               .IsUnicode(false);

            this.Property(t => t.LogSettings)
              .HasMaxLength(350)
              .IsUnicode(false);

            this.Property(t => t.EmailDeliveryError)
            .IsMaxLength()
            .IsUnicode(true);

            this.Property(t => t.ResponseDocumentId)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.AdditionalFields)
               .IsMaxLength()
               .IsUnicode(true);

            this.Property(t => t.UniqueNumber)
              .HasMaxLength(20)
              .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("CommunicationLogs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
            this.Property(t => t.InOut).HasColumnName("InOut");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.DocumentId).HasColumnName("DocumentId");
            this.Property(t => t.DocumentOutId).HasColumnName("DocumentOutId");
            this.Property(t => t.DocumentsFilingId).HasColumnName("DocumentsFilingId");
            this.Property(t => t.CommunicationStatusTypeCode).HasColumnName("CommunicationStatusTypeCode");
            this.Property(t => t.CommunicationLogTypeCode).HasColumnName("CommunicationLogTypeCode");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.EntityReference).HasColumnName("EntityReference");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.AWBNumber).HasColumnName("AWBNumber");
            this.Property(t => t.ChildEntityId).HasColumnName("ChildEntityId");
            this.Property(t => t.ChildObjectTableId).HasColumnName("ChildObjectTableId");
            this.Property(t => t.LogSettings).HasColumnName("LogSettings");

            this.Property(t => t.IsSecured).HasColumnName("IsSecured");
            this.Property(t => t.EmailDeliveryError).HasColumnName("EmailDeliveryError");
            this.Property(t => t.ResponseDocumentId).HasColumnName("ResponseDocumentId");
            this.Property(t => t.AdditionalFields).HasColumnName("AdditionalFields");
            this.Property(t => t.UniqueNumber).HasColumnName("UniqueNumber");
            this.Property(t => t.WasAnalyzed).HasColumnName("WasAnalyzed");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.CreateDate).HasColumnName("CreateDate_");
                this.Property(t => t.Subject).HasColumnName("Subject_");
                this.Property(t => t.DoneDate).HasColumnName("DoneDate_");
                this.Property(t => t.CC).HasColumnName("CC_");
                this.Property(t => t.DoneDateUTC).HasColumnName("DoneDateUTC_");
                this.Property(t => t.CreateDateUTC).HasColumnName("CreateDateUTC_");
                this.Property(t => t.QueueName).HasColumnName("QueueName_");
                this.Property(t => t.Priority).HasColumnName("Priority_");
                this.Property(t => t.MessageLockId).HasColumnName("MessageLockId_");
                this.Property(t => t.Retries).HasColumnName("Retries_");
                this.Property(t => t.BCC).HasColumnName("BCC_");
                this.Property(t => t.LastStatusDateUTC).HasColumnName("LastStatusDateUTC_");
                this.Property(t => t.NextTryDateTimeUTC).HasColumnName("NextTryDateTimeUTC_");
                this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage_");
                this.Property(t => t.CorrelationID).HasColumnName("CorrelationID_");
                this.Property(t => t.Logs).HasColumnName("Logs_");
                this.Property(t => t.From).HasColumnName("From_");
                this.Property(t => t.LastStatusDate).HasColumnName("LastStatusDate_");
                this.Property(t => t.NextTryDateTime).HasColumnName("NextTryDateTime_");
                this.Property(t => t.To).HasColumnName("To_");
                this.Property(t => t.ReplyToList).HasColumnName("ReplyToList_");
            }
            //#else
            else
            {
                this.Property(t => t.CreateDate).HasColumnName("CreateDate");
                this.Property(t => t.Subject).HasColumnName("Subject");
                this.Property(t => t.DoneDate).HasColumnName("DoneDate");
                this.Property(t => t.CC).HasColumnName("CC");
                this.Property(t => t.DoneDateUTC).HasColumnName("DoneDateUTC");
                this.Property(t => t.CreateDateUTC).HasColumnName("CreateDateUTC");
                this.Property(t => t.QueueName).HasColumnName("QueueName");
                this.Property(t => t.Priority).HasColumnName("Priority");
                this.Property(t => t.MessageLockId).HasColumnName("MessageLockId");
                this.Property(t => t.Retries).HasColumnName("Retries");
                this.Property(t => t.BCC).HasColumnName("BCC");
                this.Property(t => t.LastStatusDateUTC).HasColumnName("LastStatusDateUTC");
                this.Property(t => t.NextTryDateTimeUTC).HasColumnName("NextTryDateTimeUTC");
                this.Property(t => t.ExceptionMessage).HasColumnName("ExceptionMessage");
                this.Property(t => t.CorrelationID).HasColumnName("CorrelationID");
                this.Property(t => t.Logs).HasColumnName("Logs");
                this.Property(t => t.From).HasColumnName("From");
                this.Property(t => t.LastStatusDate).HasColumnName("LastStatusDate");
                this.Property(t => t.NextTryDateTime).HasColumnName("NextTryDateTime");
                this.Property(t => t.To).HasColumnName("To");
                this.Property(t => t.ReplyToList).HasColumnName("ReplyToList");
            }


            // Relationships
            this.HasOptional(t => t.ResponseDocument)
                .WithMany()
                .HasForeignKey(d => d.ResponseDocumentId);

            //#endif

        }
    }
}

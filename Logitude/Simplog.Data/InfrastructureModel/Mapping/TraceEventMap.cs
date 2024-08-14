using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class TraceEventMap : EntityTypeConfiguration<TraceEvent>
    {
        public TraceEventMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.EntityId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ObjectTableId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.EventTypeId).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.UserId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalId).HasMaxLength(40).IsUnicode(false);
            this.Property(t => t.CustomerCareUserEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.Location).HasMaxLength(40).IsUnicode(true);
            this.Property(t => t.PartnerName).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.ChildEntityId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ChildObjectTableId).HasMaxLength(15).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("TraceEvents");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId");
            this.Property(t => t.EventTypeId).HasColumnName("EventTypeId");         
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.Deleted).HasColumnName("Deleted");
            this.Property(t => t.ExternalId).HasColumnName("ExternalId");
            this.Property(t => t.IsAddedManually).HasColumnName("IsAddedManually");
            this.Property(t => t.CustomerCareUserEmail).HasColumnName("CustomerCareUserEmail");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.PartnerName).HasColumnName("PartnerName");
            this.Property(t => t.ChildEntityId).HasColumnName("ChildEntityId");
            this.Property(t => t.ChildObjectTableId).HasColumnName("ChildObjectTableId");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Notes)
                 .HasMaxLength(2000)
                 .IsUnicode(true);

                this.Property(t => t.EventDateTime).HasColumnName("EventDateTime_");
                this.Property(t => t.LogDateTime).HasColumnName("LogDateTime_");
            }
            //#else
            else
            {
                this.Property(t => t.EventDateTime).HasColumnName("EventDateTime");
                this.Property(t => t.LogDateTime).HasColumnName("LogDateTime");
                this.Property(t => t.Notes)
                 .HasMaxLength(4000)
                 .IsUnicode(true);
            }
//#endif

            this.HasOptional(t => t.User).WithMany().HasForeignKey(d => d.UserId).WillCascadeOnDelete(false);
        }
    }
}

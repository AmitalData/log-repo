using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class ParticipantMap : EntityTypeConfiguration<Participant>
    {
        public ParticipantMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TTY).HasMaxLength(33).IsUnicode(false);
            this.Property(t => t.RegistrationUpdatedBy).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.FWBNotifyContacts).HasMaxLength(4000).IsUnicode(false);
            this.Property(t => t.FHLNotifyContacts).HasMaxLength(4000).IsUnicode(false);
            this.Property(t => t.FFRNotifyContacts).HasMaxLength(4000).IsUnicode(false);
            this.Property(t => t.PrimaryContactName).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);

            this.ToTable("Participants");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.ForwarderTenant).HasColumnName("ForwarderTenant");
            this.Property(t => t.TTY).HasColumnName("TTY");
            this.Property(t => t.Registered).HasColumnName("Registered");
            this.Property(t => t.RegistrationRequested).HasColumnName("RegistrationRequested");
            this.Property(t => t.RegistrationUpdatedBy).HasColumnName("RegistrationUpdatedBy");
            this.Property(t => t.IsDirect).HasColumnName("IsDirect");
            this.Property(t => t.RegistrationDate).HasColumnName("RegistrationDate");
            this.Property(t => t.FWBNotifyContacts).HasColumnName("FWBNotifyContacts");
            this.Property(t => t.FHLNotifyContacts).HasColumnName("FHLNotifyContacts");
            this.Property(t => t.FFRNotifyContacts).HasColumnName("FFRNotifyContacts");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");

            this.HasRequired(t => t.Card).WithOptional(t => t.Participant);
            this.HasRequired(t => t.Forwarder).WithMany().HasForeignKey(d => d.ForwarderTenant);
        }
    }
}

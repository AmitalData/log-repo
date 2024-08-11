using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class LogitudeMessagesTransmissionLogMap : EntityTypeConfiguration<LogitudeMessagesTransmissionLog>
    {
        public LogitudeMessagesTransmissionLogMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.CCS).IsRequired().HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.AirlineCode).IsRequired().HasMaxLength(2).IsUnicode(false);
            this.Property(t => t.MessageTypeCode).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Prefix).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.AWBNumber).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.HAWB).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.ParticipantId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.Participant).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.IATACode).HasMaxLength(7).IsUnicode(false);
            this.Property(t => t.CASSCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.UserEmail).HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.UserName).HasMaxLength(140).IsUnicode(true);
            this.Property(t => t.Origin).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Destination).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.GrossWeightUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.ChargeableWeightUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.VolumeUnitCode).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.DescriptionOfGoods).HasMaxLength(4000).IsUnicode(true);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);

             string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
             if (dbms == "oracle")
             {
                 // Table & Column Mappings
                 this.ToTable("LogitudeMessagesTransLogs");

             }
             else
             {
            this.ToTable("LogitudeMessagesTransmissionLogs");

             }
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.SourceTenant).HasColumnName("SourceTenant");
            this.Property(t => t.CCS).HasColumnName("CCS");
            this.Property(t => t.AirlineCode).HasColumnName("AirlineCode");
            this.Property(t => t.MessageTypeCode).HasColumnName("MessageTypeCode");
            this.Property(t => t.Prefix).HasColumnName("Prefix");
            this.Property(t => t.AWBNumber).HasColumnName("AWBNumber");
            this.Property(t => t.HAWB).HasColumnName("HAWB");
            this.Property(t => t.SentDate).HasColumnName("SentDate");
            this.Property(t => t.ParticipantId).HasColumnName("ParticipantId");
            this.Property(t => t.Participant).HasColumnName("Participant");
            this.Property(t => t.IATACode).HasColumnName("IATACode");
            this.Property(t => t.CASSCode).HasColumnName("CASSCode");
            this.Property(t => t.UserEmail).HasColumnName("UserEmail");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Origin).HasColumnName("Origin");
            this.Property(t => t.Destination).HasColumnName("Destination");
            this.Property(t => t.Pieces).HasColumnName("Pieces");
            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight");
            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode");
            this.Property(t => t.ChargeableWeight).HasColumnName("ChargeableWeight");
            this.Property(t => t.ChargeableWeightUnitCode).HasColumnName("ChargeableWeightUnitCode");            
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode");
            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods");
            this.Property(t => t.DirectParticipant).HasColumnName("DirectParticipant");
            this.Property(t => t.IsUpdatedinAirlineTenant).HasColumnName("IsUpdatedinAirlineTenant");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}

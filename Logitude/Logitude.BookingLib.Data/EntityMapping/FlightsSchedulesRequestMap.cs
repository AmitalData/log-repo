using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.ModelConfiguration;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data;
 
namespace Logitude.BookingLib.Data.EntityMapping
{
 
    public class FlightsSchedulesRequestMap : EntityTypeConfiguration<FlightsSchedulesRequest>
    {
	    string dbms;
        public FlightsSchedulesRequestMap()
        { 
				this.ToTable("FlightsSchedulesRequests");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AirlineId).HasColumnName("AirlineId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ShipmentId).HasColumnName("ShipmentId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BookingId).HasColumnName("BookingId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ETD).HasColumnName("ETD");

            this.Property(t => t.ETA).HasColumnName("ETA");

            this.Property(t => t.Volume).HasColumnName("Volume").HasPrecision(18, 3);

            this.Property(t => t.GrossWeight).HasColumnName("GrossWeight").HasPrecision(18, 3);

            this.Property(t => t.VolumeUnitCode).HasColumnName("VolumeUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.GrossWeightUnitCode).HasColumnName("GrossWeightUnitCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.ResponseDate).HasColumnName("ResponseDate");

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.AnswerOSI).HasColumnName("AnswerOSI").HasMaxLength(150).IsUnicode(false);

            this.Property(t => t.AnswerReasonForNoReply).HasColumnName("AnswerReasonForNoReply").HasMaxLength(150).IsUnicode(false);

            this.Property(t => t.RequestDetails).HasColumnName("RequestDetails").HasMaxLength(500).IsUnicode(false);
        }
    }
}
	 
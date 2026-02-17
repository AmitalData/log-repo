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
 
    public class FlightsSchedulesResponseMap : EntityTypeConfiguration<FlightsSchedulesResponse>
    {
	    string dbms;
        public FlightsSchedulesResponseMap()
        { 
				this.ToTable("FlightsSchedulesResponses");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.FromPortId).HasColumnName("FromPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ToPortId).HasColumnName("ToPortId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AirlineId).HasColumnName("AirlineId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RequestId).HasColumnName("RequestId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ETD).HasColumnName("ETD");

            this.Property(t => t.ETA).HasColumnName("ETA");

            this.Property(t => t.FlightNumber).HasColumnName("FlightNumber").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.AirplaneType).HasColumnName("AirplaneType").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.NumberOfStops).HasColumnName("NumberOfStops");

            this.Property(t => t.ResultNumber).HasColumnName("ResultNumber");

            this.Property(t => t.LineNumber).HasColumnName("LineNumber");

            this.Property(t => t.FromPortCode).HasColumnName("FromPortCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.FromPortName).HasColumnName("FromPortName").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ToPortCode).HasColumnName("ToPortCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ToPortName).HasColumnName("ToPortName").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.MissingPort).HasColumnName("MissingPort").IsRequired();
        }
    }
}
	 
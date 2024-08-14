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
 
    public class BookingAnswerMap : EntityTypeConfiguration<BookingAnswer>
    {
	    string dbms;
        public BookingAnswerMap()
        { 
				this.ToTable("BookingAnswers");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.BookingId).HasColumnName("BookingId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").IsRequired().HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ETD).HasColumnName("ETD");

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.Origin).HasColumnName("Origin").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.Destination).HasColumnName("Destination").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.CommunicationLogId).HasColumnName("CommunicationLogId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FlightNumber).HasColumnName("FlightNumber").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.BookingSpaceAllocationCode).HasColumnName("BookingSpaceAllocationCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CarrierId).HasColumnName("CarrierId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OtherServicesInformation).HasColumnName("OtherServicesInformation").HasMaxLength(250).IsUnicode(false);

            this.Property(t => t.DescriptionOfGoods).HasColumnName("DescriptionOfGoods").HasMaxLength(512).IsUnicode(false);

            this.Property(t => t.NumberOfPieces).HasColumnName("NumberOfPieces");

            this.Property(t => t.Weight).HasColumnName("Weight").HasPrecision(18, 2);

            this.Property(t => t.WeightUnitCode).HasColumnName("WeightUnitCode").HasMaxLength(3).IsUnicode(false);
        }
    }
}
	 
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
 
    public class BookingProductMap : EntityTypeConfiguration<BookingProduct>
    {
	    string dbms;
        public BookingProductMap()
        { 
				this.ToTable("BookingProducts");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(120).IsUnicode(true);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.AirlineId).HasColumnName("AirlineId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.InActive).HasColumnName("InActive").IsRequired();
        }
    }
}
	 
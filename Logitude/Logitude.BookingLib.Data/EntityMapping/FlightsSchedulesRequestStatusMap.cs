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
 
    public class FlightsSchedulesRequestStatusMap : EntityTypeConfiguration<FlightsSchedulesRequestStatus>
    {
	    string dbms;
        public FlightsSchedulesRequestStatusMap()
        { 
				this.ToTable("FlightsSchedulesRequestStatus");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 
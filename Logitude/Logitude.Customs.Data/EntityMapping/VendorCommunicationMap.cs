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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class VendorCommunicationMap : EntityTypeConfiguration<VendorCommunication>
    {
	    string dbms;
        public VendorCommunicationMap()
        { 
			  this.ToTable("VendorCommunications", "Customs");
		
		    this.HasKey(t => new { t.VendorId, t.LineNumber });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.VendorId).HasColumnName("VendorId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.LineNumber).HasColumnName("LineNumber").IsRequired().HasDatabaseGeneratedOption(null);

            this.Property(t => t.CommunicationTypeCode).HasColumnName("CommunicationTypeCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.CommunicationAddress).HasColumnName("CommunicationAddress").IsRequired().HasMaxLength(50).IsUnicode(false);
        }
    }
}
	 
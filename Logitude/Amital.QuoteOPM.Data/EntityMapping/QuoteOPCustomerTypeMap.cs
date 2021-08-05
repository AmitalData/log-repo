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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data;
 
namespace Amital.QuoteOPM.Data.EntityMapping
{
 
    public class QuoteOPCustomerTypeMap : EntityTypeConfiguration<QuoteOPCustomerType>
    {
	    string dbms;
        public QuoteOPCustomerTypeMap()
        { 
				this.ToTable("QuoteOPCustomerTypes");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(4).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ShowInLOV).HasColumnName("ShowInLOV");
        }
    }
}
	 
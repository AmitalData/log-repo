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
 
    public class QuoteOPRatingMap : EntityTypeConfiguration<QuoteOPRating>
    {
	    string dbms;
        public QuoteOPRatingMap()
        { 
				this.ToTable("QuoteOPRatings");
		
		    this.HasKey(t => new { t.Code });
	 
            this.Property(t => t.Code).HasColumnName("Code").IsRequired().HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Name).HasColumnName("Name").IsRequired().HasMaxLength(60).IsUnicode(false);

            this.Property(t => t.IndexOrder).HasColumnName("IndexOrder");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);
        }
    }
}
	 
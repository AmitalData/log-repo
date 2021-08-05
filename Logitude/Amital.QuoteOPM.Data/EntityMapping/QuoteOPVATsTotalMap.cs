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
 
    public class QuoteOPVATsTotalMap : EntityTypeConfiguration<QuoteOPVATsTotal>
    {
	    string dbms;
        public QuoteOPVATsTotalMap()
        { 
				this.ToTable("NONE");
		
		    this.HasKey(t => new {  });
	         }
    }
}
	 
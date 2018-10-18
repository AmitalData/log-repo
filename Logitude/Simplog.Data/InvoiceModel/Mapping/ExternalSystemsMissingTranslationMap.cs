using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel.Mapping
{
    public class ExternalSystemsMissingTranslationMap : EntityTypeConfiguration<ExternalSystemsMissingTranslation>
    {

        public ExternalSystemsMissingTranslationMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.ExternalCode).HasMaxLength(4).IsUnicode(false);
            this.Property(t => t.LogitudeId).HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.LogitudeTable).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.Split1).HasMaxLength(20).IsUnicode(false);
            this.Property(t => t.Split2).HasMaxLength(20).IsUnicode(false);

//#if ORACLE_DB
   string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
   if (dbms == "oracle")
   {
       this.ToTable("ExternalSysMissingTranslations");
   }
   //#else
   else
   {
       this.ToTable("ExternalSystemsMissingTranslations");
   }

//#endif


            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ExternalCode).HasColumnName("ExternalCode");
            this.Property(t => t.LogitudeTable).HasColumnName("LogitudeTable");
            this.Property(t => t.LogitudeId).HasColumnName("LogitudeId");
            this.Property(t => t.Split1).HasColumnName("Split1");
            this.Property(t => t.Split2).HasColumnName("Split2");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.IsResolved).HasColumnName("IsResolved");

        }


    }
}






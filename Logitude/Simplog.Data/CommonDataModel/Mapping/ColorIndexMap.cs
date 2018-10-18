using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Mapping
{
  public  class ColorIndexMap : EntityTypeConfiguration<ColorIndex>
    {
        public ColorIndexMap()
        {
            this.Property(t => t.Color)
           .HasMaxLength(60)
           .IsUnicode(false);


            this.Property(t => t.IndexNumber)

            .IsRequired();



            this.ToTable("ColorIndexs");
//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Color).HasColumnName("Color_");
            }
            //#else
            else
            {
                this.Property(t => t.Color).HasColumnName("Color");
            }
//#endif
            this.Property(t => t.IndexNumber).HasColumnName("IndexNumber");

        }

        
       
    }
}

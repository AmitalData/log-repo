using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class FBLStockMap : EntityTypeConfiguration<FBLStock>
    {
        public FBLStockMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("FBLStocks");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");

           
            this.Property(t => t.InsertionDate).HasColumnName("InsertionDate");
            this.Property(t => t.Notes).HasColumnName("Notes");


            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Number).HasColumnName("Number_");
            }
            //#else
            else
            {
                this.Property(t => t.Number).HasColumnName("Number");
            }
            

        }
    }
}

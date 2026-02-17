using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class MAWBStackMap : EntityTypeConfiguration<MAWBStack>
    {
        public MAWBStackMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.AirlineId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            // Table & Column Mappings
            this.ToTable("MAWBStacks");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
           
            this.Property(t => t.AirlineId).HasColumnName("AirlineId");
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
//#endif
            // Relationships
            this.HasRequired(t => t.Airline)
                .WithMany()
                .HasForeignKey(d => d.AirlineId);

        }
    }
}

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Mapping
{
    public class DWCategoriesMap : EntityTypeConfiguration<DWCategories>
    {
        
        public DWCategoriesMap()
        {
            this.HasKey(t => t.Code);
            this.Property(t => t.Code).IsRequired().HasMaxLength(50).IsUnicode(false); 
            this.Property(t => t.Name).IsRequired().HasMaxLength(50).IsUnicode(false);
           

            this.ToTable("DWCategories");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Index).HasColumnName("CatIndex");
            }
            //#elseelse
            else
            {
                this.Property(t => t.Index).HasColumnName("Index");
            }
            //#endif

           


        }
    }
}

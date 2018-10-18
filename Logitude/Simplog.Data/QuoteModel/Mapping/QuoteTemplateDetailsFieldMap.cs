using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Mapping
{
    public class QuoteTemplateDetailsFieldMap : EntityTypeConfiguration<QuoteTemplateDetailsField>
    {

        public QuoteTemplateDetailsFieldMap()
        {


            this.HasKey(t => t.Id);


            // Properties


            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Tenant)
                .IsRequired();





    
            this.Property(t => t.FieldCode)
                .IsRequired() 
                .HasMaxLength(40)
                .IsUnicode(false);

            this.Property(t => t.Column)
               .IsRequired();


            this.Property(t => t.Row)
               .IsRequired();



        
                


                 this.Property(t => t.QuoteTemplateId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("QuoteTemplateDetailsFields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.FieldCode).HasColumnName("FieldCode");
            this.Property(t => t.QuoteTemplateId).HasColumnName("QuoteTemplateId");


            this.HasRequired(t => t.QuoteTemplate)
                 .WithMany()
                 .HasForeignKey(d => d.QuoteTemplateId);

//#if ORACLE_DB
          
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.Column).HasColumnName("FieldColumn");
                this.Property(t => t.Row).HasColumnName("FieldRow");
            }
            //#else
            else
            {
                this.Property(t => t.Column).HasColumnName("Column");
                this.Property(t => t.Row).HasColumnName("Row");
            }
//#endif           

        }


    }
}

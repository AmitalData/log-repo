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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
 
namespace Logitude.Accounting.Data.EntityMapping
{
 
    public class ReconcileExternalPageMap : EntityTypeConfiguration<ReconcileExternalPage>
    {
	    string dbms;
        public ReconcileExternalPageMap()
        { 
				this.ToTable("ReconcileExternalPages");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.GLAccountId).HasColumnName("GLAccountId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.PageNo).HasColumnName("PageNo").IsRequired();

            this.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId").IsRequired().HasMaxLength(15).IsUnicode(false);

            dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
              this.Property(t => t.SearchFields).HasMaxLength(2000);
			}
            else
            {
              this.Property(t => t.SearchFields).HasMaxLength(4000);
			}


            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsUnicode(true);

            this.Property(t => t.FromDate).HasColumnName("FromDate").IsRequired();

            this.Property(t => t.ToDate).HasColumnName("ToDate").IsRequired();

            this.Property(t => t.StartBalance).HasColumnName("StartBalance").IsRequired();

            this.Property(t => t.CloseBalance).HasColumnName("CloseBalance").IsRequired();

            this.Property(t => t.CreateDate).HasColumnName("CreateDate").IsRequired();

            this.Property(t => t.ApprovedByUserId).HasColumnName("ApprovedByUserId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.StatusCode).HasColumnName("StatusCode").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntryTypeCode).HasColumnName("EntryTypeCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.ObjectTableId).HasColumnName("ObjectTableId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId).HasColumnName("EntityId").HasMaxLength(15).IsUnicode(false);
        }
    }
}
	 
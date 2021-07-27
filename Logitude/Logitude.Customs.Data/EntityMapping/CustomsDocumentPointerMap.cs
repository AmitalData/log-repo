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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data;
 
namespace Logitude.Customs.Data.EntityMapping
{
 
    public class CustomsDocumentPointerMap : EntityTypeConfiguration<CustomsDocumentPointer>
    {
	    string dbms;
        public CustomsDocumentPointerMap()
        { 
			  this.ToTable("CustomsDocumentPointers", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.ParentEntityCode).HasColumnName("ParentEntityCode").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ParentEntityId).HasColumnName("ParentEntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Child1EntityCode).HasColumnName("Child1EntityCode").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Child1EntityId).HasColumnName("Child1EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Child2EntityCode).HasColumnName("Child2EntityCode").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Child2EntityId).HasColumnName("Child2EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Child3EntityCode).HasColumnName("Child3EntityCode").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Child3EntityId).HasColumnName("Child3EntityId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.CustomsDocumentsTicketId).HasColumnName("CustomsDocumentsTicketId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OriginEntity).HasColumnName("OriginEntity").HasMaxLength(9).IsUnicode(false);
        }
    }
}
	 
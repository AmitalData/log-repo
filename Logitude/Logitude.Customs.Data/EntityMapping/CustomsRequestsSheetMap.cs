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
 
    public class CustomsRequestsSheetMap : EntityTypeConfiguration<CustomsRequestsSheet>
    {
	    string dbms;
        public CustomsRequestsSheetMap()
        { 
			  this.ToTable("CustomsRequestsSheets", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.ObjectTableId1).HasColumnName("ObjectTableId1").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId1).HasColumnName("EntityId1").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.ObjectTableId2).HasColumnName("ObjectTableId2").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.EntityId2).HasColumnName("EntityId2").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.RequestStatusCode).HasColumnName("RequestStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.RequestCreateDate).HasColumnName("RequestCreateDate");

            this.Property(t => t.AnswerCreateDate).HasColumnName("AnswerCreateDate");

            this.Property(t => t.RequestOwnerId).HasColumnName("RequestOwnerId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RequestComminicationId).HasColumnName("RequestComminicationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.RequestDescription).HasColumnName("RequestDescription").HasMaxLength(120).IsUnicode(true);

            this.Property(t => t.InterfaceTypeCode).HasColumnName("InterfaceTypeCode").HasMaxLength(32).IsUnicode(false);

            this.Property(t => t.EntityReference).HasColumnName("EntityReference").HasMaxLength(50).IsUnicode(false);

            this.Property(t => t.CustomFileNo).HasColumnName("CustomFileNo").HasMaxLength(12).IsUnicode(false);

            this.Property(t => t.CorrelationId).HasColumnName("CorrelationId").HasMaxLength(64).IsUnicode(false);

            this.Property(t => t.IsDCA).HasColumnName("IsDCA");

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.IsRestored).HasColumnName("IsRestored");

            this.Property(t => t.AnalyzeDcaAggregateKey).HasColumnName("AnalyzeDcaAggregateKey").HasMaxLength(128).IsUnicode(false);

            this.Property(t => t.TenantPriority).HasColumnName("TenantPriority");

            this.Property(t => t.IsHSM).HasColumnName("IsHSM");
        }
    }
}
	 
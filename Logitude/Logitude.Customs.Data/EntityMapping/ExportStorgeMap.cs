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
 
    public class ExportStorgeMap : EntityTypeConfiguration<ExportStorge>
    {
	    string dbms;
        public ExportStorgeMap()
        { 
			  this.ToTable("ExportStorges", "Customs");
		
		    this.HasKey(t => new { t.Id });
	 
            this.Property(t => t.Id).HasColumnName("Id").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.ExportFileNo).HasColumnName("ExportFileNo").HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.OrderNo).HasColumnName("OrderNo").HasMaxLength(30).IsUnicode(false);

            this.Property(t => t.CustomFileNo).HasColumnName("CustomFileNo").HasMaxLength(12).IsUnicode(false);

            this.Property(t => t.FclLcl).HasColumnName("FclLcl").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.Direction).HasColumnName("Direction").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.TransportModeId).HasColumnName("TransportModeId").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.StorageNo).HasColumnName("StorageNo");

            this.Property(t => t.VoyageNo).HasColumnName("VoyageNo");

            this.Property(t => t.StorageDate).HasColumnName("StorageDate");

            this.Property(t => t.StorageStatus).HasColumnName("StorageStatus").HasMaxLength(1).IsUnicode(false);

            this.Property(t => t.IsOpenStoarge).HasColumnName("IsOpenStoarge");

            this.Property(t => t.IsConnectedToDeclaration).HasColumnName("IsConnectedToDeclaration");

            this.Property(t => t.OperationCode).HasColumnName("OperationCode").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.SenderCodeID).HasColumnName("SenderCodeID").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.MessageFromForm).HasColumnName("MessageFromForm");

            this.Property(t => t.ReplyPhoneNumeric).HasColumnName("ReplyPhoneNumeric");

            this.Property(t => t.OperatorID).HasColumnName("OperatorID");

            this.Property(t => t.InformedParty).HasColumnName("InformedParty").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.DeclarationNumber).HasColumnName("DeclarationNumber").HasMaxLength(14).IsUnicode(false);

            this.Property(t => t.DeclarationsInContainer).HasColumnName("DeclarationsInContainer");

            this.Property(t => t.ExportManifestNumber).HasColumnName("ExportManifestNumber");

            this.Property(t => t.ReceivingSite).HasColumnName("ReceivingSite").HasMaxLength(17).IsUnicode(false);

            this.Property(t => t.StuffingSiteType).HasColumnName("StuffingSiteType").HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.LoadingSite).HasColumnName("LoadingSite").HasMaxLength(10).IsUnicode(false);
        }
    }
}
	 
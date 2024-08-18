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
 
    public class CustomsEnvironmentSettingMap : EntityTypeConfiguration<CustomsEnvironmentSetting>
    {
	    string dbms;
        public CustomsEnvironmentSettingMap()
        { 
			  this.ToTable("CustomsEnvironmentSettings", "Customs");
		
		    this.HasKey(t => new { t.Id, t.EnvironmentCode });
	 
            this.Property(t => t.Id).HasColumnName("Id").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.EnvironmentCode).HasColumnName("EnvironmentCode").HasMaxLength(20).IsUnicode(false);

            this.Property(t => t.UseRabbitMQ).HasColumnName("UseRabbitMQ");

            this.Property(t => t.RabbitHost).HasColumnName("RabbitHost").HasMaxLength(256).IsUnicode(false);

            this.Property(t => t.RabbitUserName).HasColumnName("RabbitUserName").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.RabbitPassword).HasColumnName("RabbitPassword").HasMaxLength(100).IsUnicode(false);

            this.Property(t => t.HSMSignProcess).HasColumnName("HSMSignProcess").HasMaxLength(40).IsUnicode(false);

            this.Property(t => t.HSMToken).HasColumnName("HSMToken").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.HSMActiveCertUrl).HasColumnName("HSMActiveCertUrl").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.HSMSignServiceUrl).HasColumnName("HSMSignServiceUrl").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.OcrToken).HasColumnName("OcrToken").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.UpdateDocOcrServiceUrl).HasColumnName("UpdateDocOcrServiceUrl").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.CourierDocURL).HasColumnName("CourierDocURL").HasMaxLength(1024).IsUnicode(false);

            this.Property(t => t.CourierDocKey).HasColumnName("CourierDocKey").HasMaxLength(1024).IsUnicode(false);
        }
    }
}
	 
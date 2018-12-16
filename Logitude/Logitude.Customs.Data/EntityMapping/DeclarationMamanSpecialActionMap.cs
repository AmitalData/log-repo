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
 
    public class DeclarationMamanSpecialActionMap : EntityTypeConfiguration<DeclarationMamanSpecialAction>
    {
	    string dbms;
        public DeclarationMamanSpecialActionMap()
        { 
			  this.ToTable("DeclarationMamanSpecialActions", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId, t.MamanSpecialActionCode });
	 
            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.Tenant).HasColumnName("Tenant");

            this.Property(t => t.MamanSpecialActionCode).HasColumnName("MamanSpecialActionCode").IsRequired().HasMaxLength(2).IsUnicode(false);

            this.Property(t => t.MamanLabelText1).HasColumnName("MamanLabelText1").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.MamanLabelText2).HasColumnName("MamanLabelText2").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.MamanLabelText3).HasColumnName("MamanLabelText3").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.MamanLabelText4).HasColumnName("MamanLabelText4").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.MamanLabelText5).HasColumnName("MamanLabelText5").HasMaxLength(40).IsUnicode(true);

            this.Property(t => t.MamanSpecialActionStatusCode).HasColumnName("MamanSpecialActionStatusCode").HasMaxLength(3).IsUnicode(false);

            this.Property(t => t.MamanSpecialActionsErrorXml).HasColumnName("MamanSpecialActionsErrorXml").HasMaxLength(1200).IsUnicode(true);
        }
    }
}
	 
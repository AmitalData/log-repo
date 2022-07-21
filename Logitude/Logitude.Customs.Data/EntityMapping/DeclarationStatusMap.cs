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
 
    public class DeclarationStatusMap : EntityTypeConfiguration<DeclarationStatus>
    {
	    string dbms;
        public DeclarationStatusMap()
        { 
			  this.ToTable("DeclarationStatuses", "Customs");
		
		    this.HasKey(t => new { t.DeclarationId });
	 
            this.Property(t => t.Tenant).HasColumnName("Tenant").IsRequired();

            this.Property(t => t.SearchFields).HasColumnName("SearchFields").IsMaxLength().IsUnicode(true);

            this.Property(t => t.DeclarationId).HasColumnName("DeclarationId").IsRequired().HasMaxLength(15).IsUnicode(false);

            this.Property(t => t.FieldC1).HasColumnName("FieldC1");

            this.Property(t => t.FieldC2).HasColumnName("FieldC2");

            this.Property(t => t.FieldC3).HasColumnName("FieldC3");

            this.Property(t => t.FieldC4).HasColumnName("FieldC4");

            this.Property(t => t.FieldC5).HasColumnName("FieldC5");

            this.Property(t => t.FieldC6).HasColumnName("FieldC6");

            this.Property(t => t.FieldC7).HasColumnName("FieldC7");

            this.Property(t => t.FieldC8).HasColumnName("FieldC8");

            this.Property(t => t.FieldC9).HasColumnName("FieldC9");

            this.Property(t => t.FieldC10).HasColumnName("FieldC10");

            this.Property(t => t.FieldC11).HasColumnName("FieldC11");

            this.Property(t => t.FieldC12).HasColumnName("FieldC12");

            this.Property(t => t.FieldC13).HasColumnName("FieldC13");

            this.Property(t => t.FieldC14).HasColumnName("FieldC14");

            this.Property(t => t.FieldC15).HasColumnName("FieldC15");

            this.Property(t => t.FieldC16).HasColumnName("FieldC16");

            this.Property(t => t.FieldC17).HasColumnName("FieldC17");

            this.Property(t => t.FieldC18).HasColumnName("FieldC18");

            this.Property(t => t.FieldC19).HasColumnName("FieldC19");

            this.Property(t => t.FieldC20).HasColumnName("FieldC20");

            this.Property(t => t.FieldC21).HasColumnName("FieldC21");

            this.Property(t => t.FieldC22).HasColumnName("FieldC22");

            this.Property(t => t.FieldC23).HasColumnName("FieldC23");

            this.Property(t => t.FieldC24).HasColumnName("FieldC24");

            this.Property(t => t.FieldC25).HasColumnName("FieldC25");

            this.Property(t => t.FieldC26).HasColumnName("FieldC26");

            this.Property(t => t.FieldC27).HasColumnName("FieldC27");

            this.Property(t => t.FieldC28).HasColumnName("FieldC28");

            this.Property(t => t.FieldC29).HasColumnName("FieldC29");

            this.Property(t => t.FieldC30).HasColumnName("FieldC30");

            this.Property(t => t.FieldC31).HasColumnName("FieldC31");

            this.Property(t => t.FieldC32).HasColumnName("FieldC32");

            this.Property(t => t.FieldC33).HasColumnName("FieldC33");

            this.Property(t => t.FieldC34).HasColumnName("FieldC34");

            this.Property(t => t.FieldC35).HasColumnName("FieldC35");

            this.Property(t => t.FieldC36).HasColumnName("FieldC36");

            this.Property(t => t.FieldC37).HasColumnName("FieldC37");

            this.Property(t => t.FieldC38).HasColumnName("FieldC38");

            this.Property(t => t.FieldC39).HasColumnName("FieldC39");

            this.Property(t => t.FieldC40).HasColumnName("FieldC40");

            this.Property(t => t.FieldC41).HasColumnName("FieldC41");

            this.Property(t => t.FieldC42).HasColumnName("FieldC42");

            this.Property(t => t.FieldC43).HasColumnName("FieldC43");

            this.Property(t => t.FieldC44).HasColumnName("FieldC44");

            this.Property(t => t.FieldC45).HasColumnName("FieldC45");

            this.Property(t => t.FieldC46).HasColumnName("FieldC46");

            this.Property(t => t.FieldC47).HasColumnName("FieldC47");

            this.Property(t => t.FieldC48).HasColumnName("FieldC48");

            this.Property(t => t.FieldC49).HasColumnName("FieldC49");

            this.Property(t => t.FieldC50).HasColumnName("FieldC50");

            this.Property(t => t.FieldD1).HasColumnName("FieldD1");

            this.Property(t => t.FieldD2).HasColumnName("FieldD2");

            this.Property(t => t.FieldD3).HasColumnName("FieldD3");

            this.Property(t => t.FieldD4).HasColumnName("FieldD4");

            this.Property(t => t.FieldD5).HasColumnName("FieldD5");

            this.Property(t => t.FieldD6).HasColumnName("FieldD6");

            this.Property(t => t.FieldD7).HasColumnName("FieldD7");

            this.Property(t => t.FieldD8).HasColumnName("FieldD8");

            this.Property(t => t.FieldD9).HasColumnName("FieldD9");

            this.Property(t => t.FieldD10).HasColumnName("FieldD10");

            this.Property(t => t.FieldD11).HasColumnName("FieldD11");

            this.Property(t => t.FieldD12).HasColumnName("FieldD12");

            this.Property(t => t.FieldD13).HasColumnName("FieldD13");

            this.Property(t => t.FieldD14).HasColumnName("FieldD14");

            this.Property(t => t.FieldD15).HasColumnName("FieldD15");

            this.Property(t => t.FieldD16).HasColumnName("FieldD16");

            this.Property(t => t.FieldD17).HasColumnName("FieldD17");

            this.Property(t => t.FieldD18).HasColumnName("FieldD18");

            this.Property(t => t.FieldD19).HasColumnName("FieldD19");

            this.Property(t => t.FieldD20).HasColumnName("FieldD20");

            this.Property(t => t.FieldD21).HasColumnName("FieldD21");

            this.Property(t => t.FieldD22).HasColumnName("FieldD22");

            this.Property(t => t.FieldD23).HasColumnName("FieldD23");

            this.Property(t => t.FieldD24).HasColumnName("FieldD24");

            this.Property(t => t.FieldD25).HasColumnName("FieldD25");

            this.Property(t => t.FieldD26).HasColumnName("FieldD26");

            this.Property(t => t.FieldD27).HasColumnName("FieldD27");

            this.Property(t => t.FieldD28).HasColumnName("FieldD28");

            this.Property(t => t.FieldD29).HasColumnName("FieldD29");

            this.Property(t => t.FieldD30).HasColumnName("FieldD30");

            this.Property(t => t.FieldD31).HasColumnName("FieldD31");

            this.Property(t => t.FieldD32).HasColumnName("FieldD32");

            this.Property(t => t.FieldD33).HasColumnName("FieldD33");

            this.Property(t => t.FieldD34).HasColumnName("FieldD34");

            this.Property(t => t.FieldD35).HasColumnName("FieldD35");

            this.Property(t => t.FieldD36).HasColumnName("FieldD36");

            this.Property(t => t.FieldD37).HasColumnName("FieldD37");

            this.Property(t => t.FieldD38).HasColumnName("FieldD38");

            this.Property(t => t.FieldD39).HasColumnName("FieldD39");

            this.Property(t => t.FieldD40).HasColumnName("FieldD40");

            this.Property(t => t.FieldD41).HasColumnName("FieldD41");

            this.Property(t => t.FieldD42).HasColumnName("FieldD42");

            this.Property(t => t.FieldD43).HasColumnName("FieldD43");

            this.Property(t => t.FieldD44).HasColumnName("FieldD44");

            this.Property(t => t.FieldD45).HasColumnName("FieldD45");

            this.Property(t => t.FieldD46).HasColumnName("FieldD46");

            this.Property(t => t.FieldD47).HasColumnName("FieldD47");

            this.Property(t => t.FieldD48).HasColumnName("FieldD48");

            this.Property(t => t.FieldD49).HasColumnName("FieldD49");

            this.Property(t => t.FieldD50).HasColumnName("FieldD50");

            this.Property(t => t.FieldR1).HasColumnName("FieldR1").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR2).HasColumnName("FieldR2").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR3).HasColumnName("FieldR3").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR4).HasColumnName("FieldR4").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR5).HasColumnName("FieldR5").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR6).HasColumnName("FieldR6").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR7).HasColumnName("FieldR7").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR8).HasColumnName("FieldR8").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR9).HasColumnName("FieldR9").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR10).HasColumnName("FieldR10").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR11).HasColumnName("FieldR11").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR12).HasColumnName("FieldR12").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR13).HasColumnName("FieldR13").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR14).HasColumnName("FieldR14").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR15).HasColumnName("FieldR15").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR16).HasColumnName("FieldR16").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR17).HasColumnName("FieldR17").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR18).HasColumnName("FieldR18").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR19).HasColumnName("FieldR19").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.FieldR20).HasColumnName("FieldR20").HasMaxLength(1000).IsUnicode(true);

            this.Property(t => t.SVC).HasColumnName("SVC");

            this.Property(t => t.INA).HasColumnName("INA");

            this.Property(t => t.RSG).HasColumnName("RSG");

            this.Property(t => t.RSH).HasColumnName("RSH");
        }
    }
}
	 
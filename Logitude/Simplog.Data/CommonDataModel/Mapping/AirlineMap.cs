using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class AirlineMap : EntityTypeConfiguration<Airline>
    {
        public AirlineMap()
        {
            this.HasKey(t => t.Id);
            this.Property(t => t.Prefix).IsRequired().IsFixedLength().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.AWBAccount).HasMaxLength(10).IsUnicode(false);
            this.Property(t => t.Id).IsRequired().HasMaxLength(15).IsUnicode(false);
            this.Property(t => t.TTY).HasMaxLength(33).IsUnicode(false);
            this.Property(t => t.AccountNumber).HasMaxLength(14).IsUnicode(false);
            this.Property(t => t.GLSHKPIMA).HasMaxLength(25).IsUnicode(false);
            this.Property(t => t.RegistrationNotes).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.ICAO).HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.RegistrationUpdatedBy).HasMaxLength(60).IsUnicode(false);
            this.Property(t => t.DeclineNotes).HasMaxLength(500).IsUnicode(true);
            this.Property(t => t.OldTTY).HasMaxLength(33).IsUnicode(false);
            this.Property(t => t.PrimaryContactName).HasMaxLength(120).IsUnicode(true);
            this.Property(t => t.PrimaryContactEmail).HasMaxLength(70).IsUnicode(false);
            this.Property(t => t.PrimaryContactPhone).HasMaxLength(25).IsUnicode(false);

            this.ToTable("Airlines");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Prefix).HasColumnName("Prefix");
            this.Property(t => t.AWBAccount).HasColumnName("AWBAccount");
            this.Property(t => t.AddedManually).HasColumnName("AddedManually");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CheckDigit).HasColumnName("CheckDigit");
            this.Property(t => t.LimitedLength).HasColumnName("LimitedLength");
            this.Property(t => t.TTY).HasColumnName("TTY");
            this.Property(t => t.ChampNeedsRegistration).HasColumnName("ChampNeedsRegistration");
            this.Property(t => t.IsChampRegistered).HasColumnName("IsChampRegistered");
            this.Property(t => t.AccountNumber).HasColumnName("AccountNumber");
            this.Property(t => t.GLSHKPIMA).HasColumnName("GLSHKPIMA");
            this.Property(t => t.IsGLSHKRegistered).HasColumnName("IsGLSHKRegistered");
            this.Property(t => t.GLSHKNeedsRegistration).HasColumnName("GLSHKNeedsRegistration");
            this.Property(t => t.ChampFWB).HasColumnName("ChampFWB");
            this.Property(t => t.ChampFHL).HasColumnName("ChampFHL");
            this.Property(t => t.ChampFSU).HasColumnName("ChampFSU");
            this.Property(t => t.ChampFSRFSA).HasColumnName("ChampFSRFSA");
            this.Property(t => t.ChampFVRFVA).HasColumnName("ChampFVRFVA");
            this.Property(t => t.ChampFFRFFA).HasColumnName("ChampFFRFFA");
            this.Property(t => t.GLSHKFWB).HasColumnName("GLSHKFWB");
            this.Property(t => t.GLSHKFHL).HasColumnName("GLSHKFHL");
            this.Property(t => t.GLSHKFSU).HasColumnName("GLSHKFSU");
            this.Property(t => t.GLSHKFSRFSA).HasColumnName("GLSHKFSRFSA");
            this.Property(t => t.GLSHKFVRFVA).HasColumnName("GLSHKFVRFVA");
            this.Property(t => t.GLSHKFFRFFA).HasColumnName("GLSHKFFRFFA");
            this.Property(t => t.IsAllowedInAirlinesRestriction).HasColumnName("IsAllowedInAirlinesRestriction");
            this.Property(t => t.RegistrationNotes).HasColumnName("RegistrationNotes");
            this.Property(t => t.ChampRegistrationRequested).HasColumnName("ChampRegistrationRequested");
            this.Property(t => t.GLSHKRegistrationRequested).HasColumnName("GLSHKRegistrationRequested");
            this.Property(t => t.HasAdaptations).HasColumnName("HasAdaptations");
            this.Property(t => t.RegistrationUpdatedBy).HasColumnName("RegistrationUpdatedBy");
            this.Property(t => t.IsManagingProduct).HasColumnName("IsManagingProduct");
            this.Property(t => t.IsProductMandatory).HasColumnName("IsProductMandatory");
            this.Property(t => t.IsDescriptionOfGoodsFromList).HasColumnName("IsDescriptionOfGoodsFromList");
            this.Property(t => t.ScheduleDays).HasColumnName("ScheduleDays");
            this.Property(t => t.NoAvailabilityInFVAMessages).HasColumnName("NoAvailabilityInFVAMessages");
            this.Property(t => t.IsDeclined).HasColumnName("IsDeclined");
            this.Property(t => t.DeclineNotes).HasColumnName("DeclineNotes");
            this.Property(t => t.OldTTY).HasColumnName("OldTTY");
            this.Property(t => t.PrimaryContactName).HasColumnName("PrimaryContactName");
            this.Property(t => t.PrimaryContactEmail).HasColumnName("PrimaryContactEmail");
            this.Property(t => t.PrimaryContactPhone).HasColumnName("PrimaryContactPhone");

            this.HasRequired(t => t.Card).WithOptional(t => t.Airline);
        }
    }
}

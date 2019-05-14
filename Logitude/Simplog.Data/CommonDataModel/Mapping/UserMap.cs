using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .HasMaxLength(250)
                .IsUnicode(true);

            this.Property(t => t.DepartmentId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.BranchId)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.Technology)
                .HasMaxLength(5)
                .IsUnicode(false);          
            
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(t => t.SearchFields)
                .HasMaxLength(1000)
                .IsUnicode(true);

            this.Property(t => t.IsBranchRestricted).IsRequired();
            this.Property(t => t.IsSalesman).IsRequired();

            this.Property(t => t.Code)
                 .HasMaxLength(20)
                 .IsUnicode(false);

            this.Property(d => d.FreelancerId).HasMaxLength(15).IsUnicode(false);
            this.Property(d => d.BusinessUnitId).HasMaxLength(50).IsUnicode(false);
            this.Property(t => t.LicencedUser).IsRequired();
            this.Property(t => t.IsProductRestricted).IsRequired();

            this.Property(t => t.ProductTypeCode)
                .HasMaxLength(2)
                .IsUnicode(false);

            this.Property(t => t.DistributorCode)
             .HasMaxLength(15)
             .IsUnicode(false);
           
            this.Property(t => t.PersonalId)
            .HasMaxLength(20)
            .IsUnicode(false);

            this.Property(t => t.DocumentFilingInbox)
               .HasMaxLength(1000)
               .IsUnicode(false);

            this.Property(t => t.ShowLogBoxToolTip).IsRequired();
            this.Property(t => t.UserRoles).HasMaxLength(400).IsUnicode(false);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.Tenant).HasColumnName("Tenant");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.DepartmentId).HasColumnName("DepartmentId");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsBranchRestricted).HasColumnName("IsBranchRestricted");
            this.Property(t => t.IsSalesman).HasColumnName("IsSalesman");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.FreelancerId).HasColumnName("FreelancerId");
            this.Property(t => t.IsFreelancer).HasColumnName("IsFreelancer");
            this.Property(t => t.BusinessUnitId).HasColumnName("BusinessUnitId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.LicencedUser).HasColumnName("LicencedUser");
            this.Property(t => t.IsProductRestricted).HasColumnName("IsProductRestricted");
            this.Property(t => t.ProductTypeCode).HasColumnName("ProductTypeCode");
            this.Property(t => t.IsDistributor).HasColumnName("IsDistributor");
            this.Property(t => t.DistributorCode).HasColumnName("DistributorCode");
            this.Property(t => t.Technology).HasColumnName("Technology");
            this.Property(t => t.SetAngularAsDefault).HasColumnName("SetAngularAsDefault");            
            this.Property(t => t.DocumentFilingInbox).HasColumnName("DocumentFilingInbox");
            this.Property(t => t.ShowLogBoxToolTip).HasColumnName("ShowLogBoxToolTip");
            this.Property(t => t.ShowLocalNameInLOV).HasColumnName("ShowLocalNameInLOV");
            this.Property(t => t.UserRoles).HasColumnName("UserRoles");

            //#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.Property(t => t.IsShowContactDetailsInTheMobileApp).HasColumnName("IsShowContactDetailsInMobile");
                this.Property(t => t.IsTwoFactorAuthenticationEnabled).HasColumnName("IsTwoFactorAuthenticateEnabled");
            }
            //#else
            else
            {
                this.Property(t => t.IsShowContactDetailsInTheMobileApp).HasColumnName("IsShowContactDetailsInTheMobileApp");
                this.Property(t => t.IsTwoFactorAuthenticationEnabled).HasColumnName("IsTwoFactorAuthenticationEnabled");
            }
//#endif

            this.Property(t => t.PersonalId).HasColumnName("PersonalId");
     // Relationships
            this.HasRequired(t => t.Branch).WithMany().HasForeignKey(d => d.BranchId);
            this.HasRequired(t => t.Contact).WithOptional(t => t.User);
            this.HasRequired(t => t.Department).WithMany().HasForeignKey(d => d.DepartmentId);
            this.HasOptional(t => t.Freelancer).WithMany().HasForeignKey(d => d.FreelancerId);
            this.HasRequired(t => t.BusinessUnit).WithMany().HasForeignKey(d => d.BusinessUnitId);
            this.HasOptional(t => t.ProductType).WithMany().HasForeignKey(d => d.ProductTypeCode);
        }
    }
}

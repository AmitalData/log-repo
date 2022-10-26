using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class User
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public string SearchFields { get; set; }
        public bool IsBranchRestricted { get; set; }
        public bool IsSalesman { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool LicencedUser { get; set; }
        public bool IsFreelancer { get; set; }

        public string Technology { get; set; }
        public bool IsShowContactDetailsInTheMobileApp { get; set; }
        public bool SetAngularAsDefault { get; set; }
        public bool IsTwoFactorAuthenticationEnabled { get; set; }

        public bool IsProductRestricted { get; set; }
        public bool IsDistributor { get; set; }
        public string DistributorCode { get; set; }
        public string PersonalId { get; set; }

        [ForeignKey("DistributorCode")]
        public virtual Distributor Distributor { get; set; }
        public string ProductTypeCode { get; set; }
        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }

        public string BusinessUnitId { get; set; }
        [ForeignKey("BusinessUnitId")]
        public virtual BusinessUnit BusinessUnit { get; set; }

        public string BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public string FreelancerId { get; set; }
        [ForeignKey("FreelancerId")]
        public virtual Card Freelancer { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual UserLastLogin UserLastLogin { get; set; }

        public string DocumentFilingInbox { get; set; }
        public bool ShowLogBoxToolTip { get; set; }
        public bool ShowInboxToolTip { get; set; }
        public bool ShowLocalNameInLOV { get; set; }
        public string UserRoles { get; set; }
        public bool AdditionalPackagesOnly { get; set; }
        public int? SecurityLevel { get; set; }

        public DateTime? AutomaticLastUpdateDate { get; set; }

        public string LayoutDirection { get; set; }

        public string SignatureImageId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.Validators;
using Logitude.Server.Tools;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    [CustomValidation(typeof(UserCustomValidator),"ValidateUser")]
    [DataContract]
    public class UserPM
    {
        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string DepartmentId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string BranchId { get; set; }
        [DataMember]
        public string UserType { get; set; }

        [DataMember]
        public bool IsSalesman { get; set; }

        [DataMember]
        public bool LicencedUser { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InActive { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string FacebookId { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Position { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string EnglishName { get; set; }

        [DataMember]
        public bool IsFreelancer { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string FreelancerId { get; set; }
        [DataMember]
        public string FreelancerName { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string LocalName { get; set; }
        [DataMember]
        public string ComputedLocalName { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public bool IsBranchRestricted { get; set; }
        [DataMember]
        public string Code { get; set; }
        [MyRegularExpression(@"^(([^<>()[\]\\.,;:\s@\""]+"
+ @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
+ @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
+ @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
+ @"[a-zA-Z]{2,}))$", ErrorMessage = "Invalid email format!")]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Email { get; set; }

        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public bool InternetAccess { get; set; }
        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string Password { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string BusinessPhone { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Mobile { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Fax { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? Birthday { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public DateTime? Anniversary { get; set; }
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string Notes { get; set; }

        [DataMember]
        public bool SetAngularAsDefault { get; set; }

        [DataMember]
        public List<RolePM> RolePMLists { get; set; }

        [DataMember]
        public string Technology { get; set; }

        [DataMember]
        public string CardId { get; set; }
        [DataMember]
        public bool  SignupRole { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public string DepartmentName { get; set; }
        [DataMember]
        public bool IsHybrid { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }

        [DataMember]
        public bool DontShowLocal { get; set; }

        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public int ExpirationDaysLeft { get; set; }

        [DataMember]
        public bool IsProductRestricted { get; set; }

        [DataMember]
        public bool IsShowContactDetailsInTheMobileApp { get; set; }

        [DataMember]
        public bool IsTwoFactorAuthenticationEnabled { get; set; }


        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        [DataMember]
        public string ProductTypeCode { get; set; }

        [DataMember]
        public string ProductTypeName { get; set; }

        [DataMember]
        public string RoleCode { get; set; }
        

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string BusinessUnitId { get; set; }

        [DataMember]
        [CustomValidation(typeof(Validators.ValidationClass), "ValidateClass")]
        public string PersonalId { get; set; }

        [Include]
        [Association("ContactPMUserPM", "Id", "Id",IsForeignKey=true)]
        [DataMember]
        public ContactPM Contact { get; set; }

        [Include]
        [Association("UserLastLoginPMUserPM", "Id", "Id", IsForeignKey = true)]
        [DataMember]
        public UserLastLoginPM UserLastLogin { get; set; }

        List<UserRolesPM> roles;
        [Include]
        [Association("RolePMUserPM", "Id", "UserId")]
        [Composition]
        [DataMember]
        public virtual List<UserRolesPM> Roles
        {
            get
            {
                if (roles == null)
                {
                    roles = new List<UserRolesPM>();
                }
                return roles;
            }
            set { roles = value; }
        }

        List<UserPermittedBranchPM> userPermittedBranches;
        [Include]
        [Association("UserPermittedBranchPMUserPM", "Id", "UserId")]
        [Composition]
        [DataMember]
        public virtual List<UserPermittedBranchPM> UserPermittedBranches
        {
            get
            {
                if (userPermittedBranches == null)
                {
                    userPermittedBranches = new List<UserPermittedBranchPM>();
                }
                return userPermittedBranches;
            }
            set { userPermittedBranches = value; }
        }

        List<UserPermittedProductPM> userPermittedProducts;
        [Include]
        [Association("UserPermittedProductPMUserPM", "Id", "UserId")]
        [Composition]
        [DataMember]
        public virtual List<UserPermittedProductPM> UserPermittedProducts
        {
            get
            {
                if (userPermittedProducts == null)
                {
                    userPermittedProducts = new List<UserPermittedProductPM>();
                }
                return userPermittedProducts;
            }
            set { userPermittedProducts = value; }
        }


 


        [DataMember]
        public bool IsDistributor { get; set; }
        [DataMember]
        public string DistributorCode { get; set; }
        // Dummy
        [DataMember]
        public bool EntityChanged { get; set; }
        [DataMember]
        public int ActiveModified { get; set; }
      

        public bool HasPassword { get; set; }
        [DataMember]
        public bool IsCustomerCare { get; set; }
        [DataMember]
        public bool DisplayGettingStarted { get; set; }

        [DataMember]
        public string UserRolesNamesList { get; set; }
        [DataMember]
        public string UserRolesNamesList_db { get; set; }
        [DataMember]
        public string DocumentFilingInbox { get; set; }
        [DataMember]
        public bool ShowLogBoxToolTip { get; set; }

        [DataMember]
        public bool ShowInboxToolTip { get; set; }

        [DataMember]
        public bool ShowLocalNameInLOV { get; set; }

        [DataMember]
        public string UserRoles { get; set; }

        [DataMember]
        public bool AdditionalPackagesOnly { get; set; }
    }
}

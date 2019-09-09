using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class UserList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Email { get; set; }
        public string DepartmentId { get; set; }
        public string BranchId { get; set; }
        public string Notes { get; set; }
        public string SearchFields { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsBranchRestricted { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsSalesman { get; set; }
        public bool IsFreelancer { get; set; }
        public string FreelancerId { get; set; }
        public string FreelancerName { get; set; }
        public string BusinessUnitId { get; set; }
        public string BusinessUnitName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool LicencedUser { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsProductRestricted { get; set; }
        public bool IsShowContactDetailsInTheMobileApp { get; set; }
        public string Technology { get; set; }
        public string BusinessPhone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        
        public bool SetAngularAsDefault { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public bool IsDistributor { get; set; }
        public string DistributorCode { get; set; }
        public string PersonalId { get; set; }
        public bool IsTwoFactorAuthenticationEnabled { get; set; }

        public string DocumentFilingInbox { get; set; }

        public string EmployeeGroupCustomFilter { get; set; }
        public bool ShowLocalNameInLOV { get; set; }
        public string UserRoles { get; set; }
        public bool AdditionalPackagesOnly { get; set; }

        private List<string> groupId = new List<string>();
        public List<string> GroupId 
        {
            get
            {
                if (groupId == null)
                {
                    groupId = new List<string>();
                }
                return groupId;
            }
            set { groupId = value; }
        }
    }
}

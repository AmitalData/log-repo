using AmitalCloud.Infrastructure.Domain.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.DataContracts
{
    public class TenantManagementJS
    {
        public TenantManagementJS(TenantManagementPM entityPM)
        {
            Id = entityPM.Id;
                        Name = entityPM.Name;
                        PackageCode = entityPM.PackageCode;
                        PackageName = entityPM.PackageName;
                        AWBMessagesCCSTypeCode = entityPM.AWBMessagesCCSTypeCode;
                        IsAWBStockPrepaid = entityPM.IsAWBStockPrepaid;
                        PaidUntilDate = entityPM.PaidUntilDate;
                        TTY = entityPM.TTY;
                        PIMA = entityPM.PIMA;
                        TrialEndDate = entityPM.TrialEndDate;
                        TrialStartDate = entityPM.TrialStartDate;
                        BluesnapAccount = entityPM.BluesnapAccount;
                        PaymentChannelCode = entityPM.PaymentChannelCode;
                        BluesnapContractId = entityPM.BluesnapContractId;
                        ChangeHeaderColor = entityPM.ChangeHeaderColor;
                        HeaderColor = entityPM.HeaderColor;
                        IsCargonautEnabled = entityPM.IsCargonautEnabled;
                        IsDEXXConnectionEnabled = entityPM.IsDEXXConnectionEnabled;
                        IsEAWBOnlyDemo = entityPM.IsEAWBOnlyDemo;
                        IsINTTRAOnlyDemo = entityPM.IsINTTRAOnlyDemo;
                        IsMultiPackage = entityPM.IsMultiPackage;
                        IsRecurring = entityPM.IsRecurring;
                        IsRestrictedByAirline = entityPM.IsRestrictedByAirline;
                        IsTrial = entityPM.IsTrial;
                        ManageLicencesPerUser = entityPM.ManageLicencesPerUser;
                        ManagesRegisteredAgent = entityPM.ManagesRegisteredAgent;
                        NumberOfUsers = entityPM.NumberOfUsers == null ? 0 : entityPM.NumberOfUsers.Value;
                        NumberOfFreeUsers = entityPM.FreeUsers == null ? 0 : entityPM.FreeUsers.Value;
                        //PaidDaysLeft = entityPM.PaidDaysLeft;
                        PaymentFailure = entityPM.PaymentFailure;
                        //PrivateLabelId = isLogboxSystem ? null : entityPM.PrivateLabelId;
                        SuspendDate = entityPM.SuspendDate;
                        //SuspendDaysLeft = entityPM.SuspendDaysLeft;
                        TemporalPackageCode = entityPM.TemporalPackageCode;
                        PackagesCodes_BS = entityPM.PackagesCodes_BS;
                        PackagesCodes_PK = entityPM.PackagesCodes_PK;
                        //TrailDaysLeft = entityPM.TrailDaysLeft;
                        TenantManagementLicenses = entityPM.TenantManagementLicenses;
                        CountryName = entityPM.CountryName;
                        //BluesnapContractQTY = entityPM.BluesnapContractQTY;
                        //BluesnapCRMContractQTY = entityPM.BluesnapCRMContractQTY;
                        //BluesnapEAWBContractQTY = entityPM.BluesnapEAWBContractQTY;
                        //BluesnapEAWBSContractQTY = entityPM.BluesnapEAWBSContractQTY;
                        //BluesnapOneTimeContractQTY = entityPM.BluesnapOneTimeContractQTY;
                        BluesnapCRMContractId = entityPM.BluesnapCRMContractId;
                        BluesnapEAWBContractId = entityPM.BluesnapEAWBContractId;
                        BluesnapEAWBSContractId = entityPM.BluesnapEAWBSContractId;
                        BluesnapOneTimeContract = entityPM.BluesnapOneTimeContract;
                        BluesnapInttraStockContractId = entityPM.BluesnapInttraStockContractId;
                        //BluesnapInttraStockContractQTY = entityPM.BluesnapInttraStockContractQTY;
                        MainAdditionalPackageApplied = entityPM.MainAdditionalPackageApplied;
                        CustomerURL = entityPM.CustomerURL;
                        IsContainerTrackingPrepaid = entityPM.IsContainerTrackingPrepaid;

        }


        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PackageCode { get; set; }
        public DateTime? TrialStartDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public DateTime? PaidUntilDate { get; set; }
        public string TTY { get; set; }
        public string PIMA { get; set; }
        public string AWBMessagesCCSTypeCode { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public string PrivateLabelId { get; set; }
        public bool PaymentFailure { get; set; }
        public DateTime? SuspendDate { get; set; }
        public int ExpirationDaysLeft { get; set; }
        public bool IsTrial { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsEAWBOnlyDemo { get; set; }
        public bool IsRestrictedByAirline { get; set; }
        public bool IsCargonautEnabled { get; set; }
        public bool IsDEXXConnectionEnabled { get; set; }
        public bool DoBlocking { get; set; }
        public string BlockType { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool ManageLicencesPerUser { get; set; }
        public bool ChangeHeaderColor { get; set; }
        public string HeaderColor { get; set; }
        public int TrailDaysLeft { get; set; }
        public int PaidDaysLeft { get; set; }
        public int SuspendDaysLeft { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfFreeUsers { get; set; }
        public string BluesnapContractId { get; set; }
        public string BluesnapAccount { get; set; }
        public string PaymentChannelCode { get; set; }
        public string BluesnapCRMContractId { get; set; }
        public string BluesnapEAWBContractId { get; set; }
        public string BluesnapEAWBSContractId { get; set; }
        public string BluesnapOneTimeContract { get; set; }
        public string BluesnapInttraStockContractId { get; set; }
        public int BluesnapContractQTY { get; set; }
        public int BluesnapCRMContractQTY { get; set; }
        public int BluesnapEAWBContractQTY { get; set; }
        public int BluesnapEAWBSContractQTY { get; set; }
        public int BluesnapOneTimeContractQTY { get; set; }
        public int BluesnapInttraStockContractQTY { get; set; }

        public bool ManagesRegisteredAgent { get; set; }
        public bool IsMultiPackage { get; set; }
        public bool IsINTTRAOnlyDemo { get; set; }
        public string PackageName { get; set; }
        public string TemporalPackageCode { get; set; }
        public string CountryName { get; set; }
        public bool MainAdditionalPackageApplied { get; set; }
        public bool IsContainerTrackingPrepaid { get; set; }
        public string CustomerURL { get; set; }


        private List<string> packagesCodes_PK;
        public List<string> PackagesCodes_PK
        {
            get
            {
                if (packagesCodes_PK == null)
                {
                    packagesCodes_PK = new List<string>();
                }

                return packagesCodes_PK;
            }

            set
            {
                packagesCodes_PK = value;
            }
        }

        private List<string> packagesCodes_BS;
        public List<string> PackagesCodes_BS
        {
            get
            {
                if (packagesCodes_BS == null)
                {
                    packagesCodes_BS = new List<string>();
                }

                return packagesCodes_BS;
            }

            set
            {
                packagesCodes_BS = value;
            }
        }

        private List<TenantManagementLicensePM> tenantManagementLicenses;
        public virtual List<TenantManagementLicensePM> TenantManagementLicenses
        {
            get
            {
                if (tenantManagementLicenses == null)
                {
                    tenantManagementLicenses = new List<TenantManagementLicensePM>();
                }

                return tenantManagementLicenses;
            }

            set
            {
                tenantManagementLicenses = value;
            }
        }
    }

}

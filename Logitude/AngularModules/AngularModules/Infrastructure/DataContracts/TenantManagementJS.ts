import { TenantManagementLicensePM } from '../EntityPMs/TenantManagementLicensePM';

export class TenantManagementJS {
    public Id: number;
    public Name: string;
    public PackageCode: string;
    public PackageName: string;
    public TTY: string;
    public PIMA: string;
    public AWBMessagesCCSTypeCode: string;
    public IsAWBStockPrepaid: boolean;
    public TrialStartDate: Date;
    public TrialEndDate: Date;
    public PaidUntilDate: Date;
    public PrivateLabelId: string;
    public PaymentFailure: boolean;
    public SuspendDate: Date;
    public IsTrial: boolean;
    public IsRecurring: boolean;
    public IsEAWBOnlyDemo: boolean;
    public IsRestrictedByAirline: boolean;
    public IsCargonautEnabled: boolean;
    public IsDEXXConnectionEnabled: boolean;
    public ManageLicencesPerUser: boolean;
    public ChangeHeaderColor: boolean;
    public TrailDaysLeft: number;
    public PaidDaysLeft: number;
    public SuspendDaysLeft: number;
    public NumberOfUsers: number;
    public NumberOfFreeUsers: number;
    public BluesnapContractId: string;
    public BluesnapCRMContractId: string;
    public BluesnapEAWBContractId: string;
    public BluesnapEAWBSContractId: string;
    public BluesnapOneTimeContract: string;
    public BluesnapInttraStockContractId: string;
    public BluesnapContractQTY: number;
    public BluesnapCRMContractQTY: number;
    public BluesnapEAWBContractQTY: number;
    public BluesnapEAWBSContractQTY: number;
    public BluesnapOneTimeContractQTY: number;
    public BluesnapInttraStockContractQTY: number;

    public DoBlocking: boolean;
    public BlockType: string;
    public ExpirationDaysLeft: number;

    public BluesnapAccount: string;
    public ManagesRegisteredAgent: boolean;
    public IsINTTRAOnlyDemo: boolean;
    public IsMultiPackage: boolean;
    public TemporalPackageCode: string;
    public PackagesCodes_PK: Array<string> = [];
    public PackagesCodes_BS: Array<string> = [];
    public CountryName: string;
    public MainAdditionalPackageApplied: boolean;

    private tenantManagementLicenses: TenantManagementLicensePM[];
    get TenantManagementLicenses() {
        if (this.tenantManagementLicenses == null) {
            this.tenantManagementLicenses = [];
        }

        return this.tenantManagementLicenses;
    }
    set TenantManagementLicenses(newValue: TenantManagementLicensePM[]) {
        if (this.tenantManagementLicenses != newValue) {
            this.tenantManagementLicenses = newValue;
        }
    }
}

import { UserPM } from '../EntityPMs/UserPM';
import { TenantPM } from '../EntityPMs/TenantPM';
import { TenantManagementJS } from '../DataContracts/TenantManagementJS';
// import {AccountingSettingPM} from '../EntityPMs/AccountingSettingPM';
import { CreditLimitSettingPM } from '../../Infrastructure/EntityPMs/CreditLimitSettingPM';
// import { CustomsInterfaceSettingPM } from '../../Common/EntityPMs/CustomsInterfaceSettingPM';
import { SharedLogisticsSettingPM } from '../EntityPMs/SharedLogisticsSettingPM';
import { Settings } from '../Settings';

export class ObjectsLocator {
	public static TenantPM: TenantPM;
	public static LoggedUserPM: UserPM;
	// public static TenantManagementJS: TenantManagementJS;
	// public static AccountingSettingPM: AccountingSettingPM;
	// public static CustomsInterfaceSettingPM: CustomsInterfaceSettingPM = new CustomsInterfaceSettingPM();
	public static SharedLogisticsSettingPM: SharedLogisticsSettingPM = new SharedLogisticsSettingPM();

	public static LoggedUserId: string;
	public static GlobalSetting: any;
	public static PrivateLableSettings: any;

	private static creditLimitSettingPM: CreditLimitSettingPM;
	public static get CreditLimitSettingPM() {
		if (this.creditLimitSettingPM == null) {
			this.creditLimitSettingPM = new CreditLimitSettingPM();
		}

		return this.creditLimitSettingPM;
	}
	public static set CreditLimitSettingPM(value: CreditLimitSettingPM) {
		if (value) {
			this.creditLimitSettingPM = value;
		} else {
			this.creditLimitSettingPM = new CreditLimitSettingPM();
		}
	}

	public static UpdateTenantPM(value: TenantPM) {
		this.TenantPM = value;
	}
	public static UpdateLoggedUserPM(value: UserPM) {
		this.LoggedUserPM = value;
	}
	public static UpdateCreditLimitSettingPM(value: CreditLimitSettingPM) {
		this.CreditLimitSettingPM = value;
	}
	public static UpdateGlobalSetting(value: any) {
		this.GlobalSetting = value;
	}
	public static SetLayoutDirection(value: any) {
		if (value) {
			Settings.LayoutDirection = value;
		}
	}
	public static UpdatePrivateLableSettings(value: any) {
		this.PrivateLableSettings = value;
	}
	public static IsDemoTenant(value: any): boolean {
		var isDemoTenant = false;

		var demoTenants = this.GlobalSetting.LogitudeDemoTenants?.split(',');
		if (demoTenants && demoTenants.indexOf(value) !== -1) {
			isDemoTenant = true;
		}
		return isDemoTenant;
	}
}

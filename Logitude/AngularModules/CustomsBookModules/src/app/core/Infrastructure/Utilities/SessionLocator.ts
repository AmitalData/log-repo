import { ViewContainerRef } from '@angular/core';
import { UserPM } from '../../Infrastructure/EntityPMs/UserPM';
import { TenantPM } from '../../Infrastructure/EntityPMs/TenantPM';
// import { SessionComponent } from '../Components/Session/SessionComponent';
import { AccountingSettingPM } from '../../Infrastructure/EntityPMs/AccountingSettingPM';
import { VATTypesGroupPM } from '../../Infrastructure/EntityPMs/VATTypesGroupPM';
// import {HomeComponent} from "../Components/HomeComponent/HomeComponent";
import { ExternalParams } from './ExternalParams';
import { LocalStorageManager } from './LocalStorageManager';
import { SignalRChannelService } from '../Services/SignalRServices/SignalRChannelService';
import { SATInterfaceSettingPM } from '../../Infrastructure/EntityPMs/SATInterfaceSettingPM';
import { TenantManagementJS } from '../DataContracts/TenantManagementJS';
import { FeatureToggleList } from '../EntityLists/FeatureToggleList';
import { Subscription } from 'rxjs';

export class SessionLocator {
	public static RootComponent: any;
	public static SustainFocusOnCell: boolean = false;
	public static SustainLostFocusOnCell: boolean = false;
	public static SignalRChannelService: SignalRChannelService;
	public static DisableEntityValidation: boolean = false;
	public static UseCachedData: boolean = true;
	public static IsExternalParams: boolean = false;
	public static IsSiguOut: boolean = false;
	// public static HomeComponent: HomeComponent;
	public static DynamicLoader: any = null;
	public static IsProduction: boolean = false;
	public static ExternalParams: ExternalParams = null;
	public static Index: number = 0;
	public static Tenant: number = null;
	public static LoggedUserId: string = null;
	public static LocalCurrencyId: string = null;
	public static LocalCurrencyCode: string = null;
	public static AccountingCurrencyId: string = null;
	public static IsNewSignupTenant: boolean = false;
	public static UserIcons: { [UserId: string]: Array<string> } = {};
	public static BlockType: string = null;
	public static TenantPM: TenantPM;
	public static TenantManagementJS: TenantManagementJS;
	public static AccountingSettingPM: AccountingSettingPM;
	public static AllVatTypesGroups: VATTypesGroupPM[] = [];
	public static AccountingSystemPM: any;
	public static PrivateLableSettings: any;
	public static TenantSettings: any[];
	public static LoggedUserPM: UserPM;
	public static ApplicationLocation: ViewContainerRef;
	public static SATInterfaceSettings: SATInterfaceSettingPM;
	public static FeatureToggles: FeatureToggleList[] = [];
	// public static SelectedSession: SessionComponent;
	public static ShowUserNewReleaseToolTip: boolean = true;
	// public static AllSessions: Array<SessionComponent>;
	public static ProtractorEmails: Array<string> = [];
	public static WorkerRoleName: string = null;
	// public static AddSession(mySession: SessionComponent) {
	// 	if (SessionLocator.AllSessions == null) {
	// 		SessionLocator.AllSessions = new Array<SessionComponent>();
	// 	}

	// 	if (SessionLocator.AllSessions.indexOf(mySession) == -1) {
	// 		SessionLocator.AllSessions.push(mySession);
	// 	}
	// }
	public static TimersSubscribtions: Array<Subscription> = [];
	public static StopApplicationTimers() {
		for (var key in SessionLocator.TimersSubscribtions) {
			let timerSubscribion: Subscription = SessionLocator.TimersSubscribtions[key];
			timerSubscribion.unsubscribe();

			//if (subscription != null && !subscription.isUnsubscribed()) {
			//    //subscription.unsubscribe();
			//    console.log("timer stopped");
			//    subscription.Dispose();
			//}
		}

		SessionLocator.TimersSubscribtions = [];
	}
	public static ClearExternalParams() {
		this.IsExternalParams = false;
	}
	public static ClearLocalStorage() {
		if (LocalStorageManager.GetItem('Token')) {
			window.localStorage.setItem('Token_' + this.Tenant, '');
			window.localStorage.setItem('CardId_' + this.Tenant, '');
			window.localStorage.setItem('Token', '');
			window.localStorage.setItem('CardId', '');
		}
	}

	public static GetComputerIdFromStorage() {
		return LocalStorageManager.GetItem('UserLastLoginComputerID');
	}

	public static StoreLogedComputerId(computerId: string) {
		LocalStorageManager.SetItem('UserLastLoginComputerID', computerId);
	}
	public static IsMainSidebarCollapsed: boolean = false;
}

import { TenantPM } from '../../Infrastructure/EntityPMs/TenantPM';
import { SessionLocator } from './SessionLocator';
import { ObjectsLocator } from '../Locators/ObjectsLocator';
import { ObjectsUpdater } from '../Locators/ObjectsUpdater';

export class InfraSettings {
	private static tenantPM: TenantPM;
	public static get TenantPM(): TenantPM {
		return this.tenantPM;
	}
	public static set TenantPM(newValue: TenantPM) {
		if (this.tenantPM != newValue) {
			this.tenantPM = newValue;
			SessionLocator.TenantPM = newValue;

			ObjectsLocator.UpdateTenantPM(newValue);
			ObjectsUpdater.UpdateTenantPM(newValue);

			if (newValue) {
				SessionLocator.Tenant = newValue.Id;
				SessionLocator.LocalCurrencyId = newValue.CurrencyId;
				SessionLocator.LocalCurrencyCode = newValue.CurrencyCode;
				SessionLocator.AccountingCurrencyId = newValue.CurrencyId;
			}
		}
	}

	public static LogitudeIndexedDB: any;
	public static IndexedDbService: any;
}

import {ObjectsLocator} from './ObjectsLocator';
import {SessionLocator} from '../Utilities/SessionLocator';

import {AppTool, DateTool} from '../Tools';
import {UserPM} from '../../Common/EntityPMs/UserPM';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import {TenantManagementPM} from '../EntityPMs/TenantManagementPM';
import {AccountingSettingPM} from '../../Common/EntityPMs/AccountingSettingPM';
import {CustomsInterfaceSettingPM} from '../../Common/EntityPMs/CustomsInterfaceSettingPM';

export class ObjectsUpdater {
    public static TenantPM: TenantPM;

    public static UpdateTenantPM(value: TenantPM) {
        this.TenantPM = value;
        AppTool.TenantPM = value;
        DateTool.TenantPM = value;
        ObjectsLocator.TenantPM = value;
        SessionLocator.TenantPM = value;

        if (value) {
            SessionLocator.Tenant = value.Id;
            SessionLocator.LocalCurrencyId = value.CurrencyId;
            SessionLocator.LocalCurrencyCode = value.CurrencyCode;
            SessionLocator.AccountingCurrencyId = value.CurrencyId;
        }
    }

    public static UpdateLoggedUserPM(value: UserPM) {
        ObjectsLocator.LoggedUserPM = value;
    }

    public static UpdateTenantManagementPM(value: TenantManagementPM) {
        if (!value) {
            value = new TenantManagementPM();
        }

        ObjectsLocator.TenantManagementPM = value;
    }

    public static UpdateAccountingSettingPM(value: AccountingSettingPM) {
        if (!value) {
            value = new AccountingSettingPM();
        }

        ObjectsLocator.AccountingSettingPM = value;
        SessionLocator.AccountingSettingPM = value;
    }
   
    public static UpdateCustomsInterfaceSettingPM(value: CustomsInterfaceSettingPM) {
        if (!value) {
            value = new CustomsInterfaceSettingPM();
        }

        ObjectsLocator.CustomsInterfaceSettingPM = value;
    }
    
}
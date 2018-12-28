import {ObjectsLocator} from './ObjectsLocator';
import {SessionLocator} from '../Utilities/SessionLocator';

import {AppTool, DateTool} from '../Tools';
import {UserPM} from '../../Common/EntityPMs/UserPM';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import { TenantManagementJS } from '../DataContracts/TenantManagementJS';
import {AccountingSettingPM} from '../../Common/EntityPMs/AccountingSettingPM';
import {CustomsInterfaceSettingPM} from '../../Common/EntityPMs/CustomsInterfaceSettingPM';
import { SharedLogisticsSettingPM } from '../EntityPMs/SharedLogisticsSettingPM';

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

    public static UpdateTenantManagementJS(value: TenantManagementJS) {
        if (!value) {
            value = new TenantManagementJS();
        }

        ObjectsLocator.TenantManagementJS = value;
        SessionLocator.TenantManagementJS = value;
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

    public static UpdateSharedLogisticsSettingPM(value: SharedLogisticsSettingPM) {
        if (!value) {
            value = new SharedLogisticsSettingPM();
        }

        ObjectsLocator.SharedLogisticsSettingPM = value;
    }    
}

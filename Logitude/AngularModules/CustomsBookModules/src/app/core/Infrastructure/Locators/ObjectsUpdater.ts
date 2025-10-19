import {ObjectsLocator} from './ObjectsLocator';
import {SessionLocator} from '../Utilities/SessionLocator';

import {AppTool, DateTool} from '../Tools';

import { TenantManagementJS } from '../DataContracts/TenantManagementJS';

import { SharedLogisticsSettingPM } from '../EntityPMs/SharedLogisticsSettingPM';

export class ObjectsUpdater {
    public static TenantPM;

    public static UpdateTenantPM(value) {
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

    public static UpdateLoggedUserPM(value) {
        ObjectsLocator.LoggedUserPM = value;
    }

    public static UpdateTenantManagementJS(value: TenantManagementJS) {
        if (!value) {
            value = new TenantManagementJS();
        }


        SessionLocator.TenantManagementJS = value;
    }

    public static UpdateAccountingSettingPM(value) {
        if (!value) {
         
        }


        SessionLocator.AccountingSettingPM = value;
    }
   
    public static UpdateCustomsInterfaceSettingPM(value) {
        if (!value) {

        }

   
    }

    public static UpdateSharedLogisticsSettingPM(value: SharedLogisticsSettingPM) {
        if (!value) {
            value = new SharedLogisticsSettingPM();
        }

        ObjectsLocator.SharedLogisticsSettingPM = value;
    }    
}

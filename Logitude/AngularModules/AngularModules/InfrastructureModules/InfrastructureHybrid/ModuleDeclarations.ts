import {NewHybridPartnerComponent} from './Components/HypridPartner/NewHybridPartnerComponent';
import {HybridPartnerTabComponent} from './Components/HypridPartner/HybridPartnerTabComponent';
import {HybridPartnerUploadLogoComponent} from './Components/HypridPartner/HybridPartnerUploadLogoComponent';
import { HybridTenantStateComponent} from './Components/HybridTenantState/HybridTenantStateComponent';
import {PermissionsHybridPartnerTabComponent} from './Components/HypridPartner/PermissionsHybridPartnerTabComponent';

export const Components =
    [
        NewHybridPartnerComponent,
        HybridPartnerTabComponent,
        HybridPartnerUploadLogoComponent,
        HybridTenantStateComponent,
        PermissionsHybridPartnerTabComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewHybridPartnerComponent": { myResult = NewHybridPartnerComponent; break; }
            case "HybridPartnerTabComponent": { myResult = HybridPartnerTabComponent; break; }
            case "HybridPartnerUploadLogoComponent": { myResult = HybridPartnerUploadLogoComponent; break; }
            case "HybridTenantStateComponent": { myResult = HybridTenantStateComponent; break; }
            case "PermissionsHybridPartnerTabComponent": { myResult = PermissionsHybridPartnerTabComponent; break; }     


        }

        return myResult;
    }
}
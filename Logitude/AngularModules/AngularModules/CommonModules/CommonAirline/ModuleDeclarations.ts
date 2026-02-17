import {NewAirlineComponent} from './Components/NewEntity/NewAirlineComponent';
import {AirlineAdaptationsTabComponent} from './Components/EditTabs/AirlineAdaptationsTabComponent';
import {AirlineAWBStockTabComponent} from './Components/EditTabs/AirlineAWBStockTabComponent';
import {AirlineCCSSettingsTabComponent} from './Components/EditTabs/AirlineCCSSettingsTabComponent';
import {AirlineSurchargeTabComponent} from './Components/EditTabs/AirlineSurchargeTabComponent';
import {AddEditAirlineAdaptationItemComponent} from './Components/AddEdit/AddEditAirlineAdaptationItemComponent';
import {AddEditAirlineMessagingRuleComponent} from './Components/AddEdit/AddEditAirlineMessagingRuleComponent';
import {AddEditTarrifHeaderComponent} from './Components/AddEdit/AddEditTarrifHeaderComponent';
import {AddEditTariffChargeComponent} from './Components/AddEdit/AddEditTariffChargeComponent';
import { AirlineDocsInTabComponent } from './Components/EditTabs/AirlineDocsInTabComponent';

export const Components =
    [
        NewAirlineComponent,
        AirlineAdaptationsTabComponent,
        AirlineAWBStockTabComponent,
        AirlineCCSSettingsTabComponent,
        AirlineSurchargeTabComponent,
        AddEditAirlineAdaptationItemComponent,
        AddEditAirlineMessagingRuleComponent,
        AddEditTarrifHeaderComponent,
        AddEditTariffChargeComponent,
        AirlineDocsInTabComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewAirlineComponent": { myResult = NewAirlineComponent; break; }
            case "AirlineAdaptationsTabComponent": { myResult = AirlineAdaptationsTabComponent; break; }
            case "AirlineAWBStockTabComponent": { myResult = AirlineAWBStockTabComponent; break; }
            case "AirlineCCSSettingsTabComponent": { myResult = AirlineCCSSettingsTabComponent; break; }
            case "AirlineSurchargeTabComponent": { myResult = AirlineSurchargeTabComponent; break; }
            case "AddEditAirlineAdaptationItemComponent": { myResult = AddEditAirlineAdaptationItemComponent; break; }
            case "AddEditAirlineMessagingRuleComponent": { myResult = AddEditAirlineMessagingRuleComponent; break; }
            case "AddEditTarrifHeaderComponent": { myResult = AddEditTarrifHeaderComponent; break; }
            case "AddEditTariffChargeComponent": { myResult = AddEditTariffChargeComponent; break; }
            case "AirlineDocsInTabComponent": { myResult = AirlineDocsInTabComponent; break; }
        }

        return myResult;
    }
}

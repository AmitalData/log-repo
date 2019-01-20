
import {UpdateCurrencyRateComponent} from './Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {UpdateVATPercentageComponent} from './Components/UpdateVATPercentage/UpdateVATPercentageComponent';
import {CitySelectionComponent} from './Components/CitySelection/CitySelectionComponent';
import {LoadSampleDataComponent} from './Components/LoadSampleData/LoadSampleDataComponent';
import { DWQueryBuilderComponent } from './Components/DWQueryBuilder/DWQueryBuilderComponent';
import { DWQueryBuilderFiltersComponent } from './Components/DWQueryBuilder/DWQueryBuilderFiltersComponent'; 
import { CustomsShipperGeneralTabComponent } from './Components/Depositions/EditTab/CustomsShipperGeneralTabComponent';
import { DWFilterSettings } from './Components/DWQueryBuilder/DWFilterSettings';
//import { DWAskUserFiltersComponent } from './Components/DWQueryBuilder/DWAskUserFiltersComponent'; 


export const Components =
    [
        CitySelectionComponent,
        UpdateCurrencyRateComponent,
        UpdateVATPercentageComponent,
        LoadSampleDataComponent,
        DWQueryBuilderComponent,
        DWQueryBuilderFiltersComponent,
        CustomsShipperGeneralTabComponent,
        DWFilterSettings,
        //DWAskUserFiltersComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "UpdateCurrencyRateComponent": { myResult = UpdateCurrencyRateComponent; break; }
            case "UpdateVATPercentageComponent": { myResult = UpdateVATPercentageComponent; break; }
            case "CitySelectionComponent": { myResult = CitySelectionComponent; break; }
            case "LoadSampleDataComponent": { myResult = LoadSampleDataComponent; break; }
            case "DWQueryBuilderComponent": { myResult = DWQueryBuilderComponent; break; }
            case "DWQueryBuilderFiltersComponent": { myResult = DWQueryBuilderFiltersComponent; break; }
            case "CustomsShipperGeneralTabComponent": { myResult = CustomsShipperGeneralTabComponent; break; }
            case "DWFilterSettings": { myResult = DWFilterSettings; break; }
            //case "DWAskUserFiltersComponent": { myResult = DWAskUserFiltersComponent; break; }
                 
                
        }

        return myResult;
    }
}


import {UpdateCurrencyRateComponent} from './Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {UpdateVATPercentageComponent} from './Components/UpdateVATPercentage/UpdateVATPercentageComponent';
import {CitySelectionComponent} from './Components/CitySelection/CitySelectionComponent';
import {LoadSampleDataComponent} from './Components/LoadSampleData/LoadSampleDataComponent';
import {DWQueryBuilderComponent} from './Components/DWQueryBuilder/DWQueryBuilderComponent';


export const Components =
    [
        CitySelectionComponent,
        UpdateCurrencyRateComponent,
        UpdateVATPercentageComponent,
        LoadSampleDataComponent,
        DWQueryBuilderComponent,
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
        }

        return myResult;
    }
}
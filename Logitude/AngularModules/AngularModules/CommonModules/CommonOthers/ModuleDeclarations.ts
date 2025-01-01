

import {UpdateCurrencyRateComponent} from './Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {UpdateVATPercentageComponent} from './Components/UpdateVATPercentage/UpdateVATPercentageComponent';
import {CitySelectionComponent} from './Components/CitySelection/CitySelectionComponent';
import {ZipCodeSelectionComponent} from './Components/ZipCodeSelection/ZipCodeSelectionComponent';
import {LoadSampleDataComponent} from './Components/LoadSampleData/LoadSampleDataComponent';
import { DWQueryBuilderComponent } from './Components/DWQueryBuilder/DWQueryBuilderComponent';
import { DWQueryBuilderFiltersComponent } from './Components/DWQueryBuilder/DWQueryBuilderFiltersComponent'; 
import { CustomsShipperGeneralTabComponent } from './Components/Depositions/EditTab/CustomsShipperGeneralTabComponent';
import { DWFilterSettings } from './Components/DWQueryBuilder/DWFilterSettings';
//import { DWAskUserFiltersComponent } from './Components/DWQueryBuilder/DWAskUserFiltersComponent'; 

import { ProductTypeGeneralTabComponent } from './Components/ProductType/EditTabs/ProductTypeGeneralTabComponent';
import { NewDocumentsFilingComponent } from './Components/NewEntity/DocumnetsFiling/NewDocumentsFilingComponent';
import { CloseSaveButtonsComponent } from 'InfrastructureModules/InfrastructureOthers/AmitalAPI/components/CloseSaveButtonsComponent';




export const Components =
    [
        CitySelectionComponent,
        ZipCodeSelectionComponent,
        UpdateCurrencyRateComponent,
        UpdateVATPercentageComponent,
        LoadSampleDataComponent,
        DWQueryBuilderComponent,
        DWQueryBuilderFiltersComponent,
        CustomsShipperGeneralTabComponent,
        DWFilterSettings,
        //DWAskUserFiltersComponent
        ProductTypeGeneralTabComponent,
        NewDocumentsFilingComponent,
        CloseSaveButtonsComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "UpdateCurrencyRateComponent": { myResult = UpdateCurrencyRateComponent; break; }
            case "UpdateVATPercentageComponent": { myResult = UpdateVATPercentageComponent; break; }
            case "CitySelectionComponent": { myResult = CitySelectionComponent; break; }
            case "ZipCodeSelectionComponent": { myResult = ZipCodeSelectionComponent; break; }
            case "LoadSampleDataComponent": { myResult = LoadSampleDataComponent; break; }
            case "DWQueryBuilderComponent": { myResult = DWQueryBuilderComponent; break; }
            case "DWQueryBuilderFiltersComponent": { myResult = DWQueryBuilderFiltersComponent; break; }
            case "CustomsShipperGeneralTabComponent": { myResult = CustomsShipperGeneralTabComponent; break; }
            case "DWFilterSettings": { myResult = DWFilterSettings; break; }
            //case "DWAskUserFiltersComponent": { myResult = DWAskUserFiltersComponent; break; }
            case "ProductTypeGeneralTabComponent": { myResult = ProductTypeGeneralTabComponent; break; }
            case "NewDocumentsFilingComponent": { myResult = NewDocumentsFilingComponent; break; }
            case "NewDocumentsFilingComponent": { myResult = NewDocumentsFilingComponent; break; }
        }

        return myResult;
    }
}

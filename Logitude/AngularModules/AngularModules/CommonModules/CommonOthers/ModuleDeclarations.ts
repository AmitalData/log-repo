

import {UpdateCurrencyRateComponent} from './Components/UpdateCurrencyRate/UpdateCurrencyRateComponent';
import {UpdateVATPercentageComponent} from './Components/UpdateVATPercentage/UpdateVATPercentageComponent';
import {CitySelectionComponent} from './Components/CitySelection/CitySelectionComponent';
import {ZipCodeSelectionComponent} from './Components/ZipCodeSelection/ZipCodeSelectionComponent';
import {LoadSampleDataComponent} from './Components/LoadSampleData/LoadSampleDataComponent';
import { DWQueryBuilderComponent } from './Components/DWQueryBuilder/DWQueryBuilderComponent';
import { DWQueryBuilderFiltersComponent } from './Components/DWQueryBuilder/DWQueryBuilderFiltersComponent'; 
import { CustomsShipperGeneralTabComponent } from './Components/Depositions/EditTab/CustomsShipperGeneralTabComponent';
import { DWFilterSettings } from './Components/DWQueryBuilder/DWFilterSettings';
import { ProductTypeGeneralTabComponent } from './Components/ProductType/EditTabs/ProductTypeGeneralTabComponent';
import { DocumentsFilingComponent } from './Components/NewEntity/DocumnetsFiling/DocumentsFilingComponent';
import { DragDropFileInputComponent } from './Components/drag-drop-file-input/drag-drop-file-input.component';
import { ExternalLinkComponent } from './Components/ExternalLink/ExternalLinkComponent';




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
        ProductTypeGeneralTabComponent,
        DocumentsFilingComponent,
        DragDropFileInputComponent,
        ExternalLinkComponent,
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
            case "ProductTypeGeneralTabComponent": { myResult = ProductTypeGeneralTabComponent; break; }
            case "DocumentsFilingComponent": { myResult = DocumentsFilingComponent; break; }
            case "DragDropFileInputComponent": { myResult = DragDropFileInputComponent; break; }
            case "ExternalLinkComponent": { myResult = ExternalLinkComponent; break; }                                
        }

        return myResult;
    }
}

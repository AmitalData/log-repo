import {StockNewWizardComponent} from './Components/AWBMessagingStock/StockNewWizardComponent';
import {StockGeneralTabComponent} from './Components/AWBMessagingStock/StockGeneralTabComponent';
import {StockWindowComponent} from './Components/AWBMessagingStock/StockWindowComponent';
import {StockHistoryComponent} from './Components/AWBMessagingStock/StockHistoryComponent';
import {FBLStockFieldComponent} from './Components/FBLStock/FBLStockFieldComponent';
import {FBLStockMainComponent} from './Components/FBLStock/FBLStockMainComponent';
import {NewFBLStockComponent} from './Components/FBLStock/NewFBLStockComponent';
import {FBLStackSelectionComponent} from './Components/FBLStock/FBLStackSelectionComponent';
import {AddEditAWBStockComponent} from './Components/Maintenance/AddEditAWBStockComponent';
import {TenantManagementAWBStockTabComponent} from './Components/Maintenance/TenantManagementAWBStockTabComponent';



export const Components =
    [
        StockNewWizardComponent,
        StockGeneralTabComponent,
        StockWindowComponent,
        StockHistoryComponent,
        FBLStockFieldComponent,
        FBLStockMainComponent,
        NewFBLStockComponent,
        FBLStackSelectionComponent,
        AddEditAWBStockComponent,
        TenantManagementAWBStockTabComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "StockNewWizardComponent": { myResult = StockNewWizardComponent; break; }
            case "StockGeneralTabComponent": { myResult = StockGeneralTabComponent; break; }
            case "StockWindowComponent": { myResult = StockWindowComponent; break; }
            case "StockHistoryComponent": { myResult = StockHistoryComponent; break; }
            case "FBLStockFieldComponent": { myResult = FBLStockFieldComponent; break; }
            case "FBLStockMainComponent": { myResult = FBLStockMainComponent; break; }
            case "NewFBLStockComponent": { myResult = NewFBLStockComponent; break; }
            case "FBLStackSelectionComponent": { myResult = FBLStackSelectionComponent; break; }
            case "AddEditAWBStockComponent": { myResult = AddEditAWBStockComponent; break; }
            case "TenantManagementAWBStockTabComponent": { myResult = TenantManagementAWBStockTabComponent; break; }
        }

        return myResult;
    }
}
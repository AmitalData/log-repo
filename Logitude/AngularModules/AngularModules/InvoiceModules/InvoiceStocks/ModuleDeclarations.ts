import { ManageStocksComponent } from './Components/ManageStocksComponent';
import { NewARInvoiceStockComponent } from './Components/NewEntity/NewARInvoiceStockComponent';
import { ARInvoiceStockInputTemplate } from './Components/ARInvoiceStockInputTemplate';
import { ARInvoiceStockGeneralTabComponent } from './Components/EditTabs/ARInvoiceStockGeneralTabComponent';
import { NewARInvoiceStockLinesComponent } from './Components/NewEntity/NewARInvoiceStockLinesComponent';
import { ARInvoiceStockSelectionComponent } from './Components/StockSelection/ARInvoiceStockSelectionComponent';
export const Components =
    [
        ManageStocksComponent,
        NewARInvoiceStockComponent,
        ARInvoiceStockInputTemplate,
        ARInvoiceStockGeneralTabComponent,
        NewARInvoiceStockLinesComponent,
        ARInvoiceStockSelectionComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ManageStocksComponent": { myResult = ManageStocksComponent; break; }
            case "NewARInvoiceStockComponent": { myResult = NewARInvoiceStockComponent; break; }
            case "ARInvoiceStockInputTemplate": { myResult = ARInvoiceStockInputTemplate; break; }
            case "ARInvoiceStockGeneralTabComponent": { myResult = ARInvoiceStockGeneralTabComponent; break; }
            case "NewARInvoiceStockLinesComponent": { myResult = NewARInvoiceStockLinesComponent; break; }
            case "ARInvoiceStockSelectionComponent": { myResult = ARInvoiceStockSelectionComponent; break; }
        }

        return myResult;
    }
}

import { ManageStocksComponent } from './Components/ManageStocksComponent';
import { NewARInvoiceStockComponent } from './Components/NewARInvoiceStockComponent';

export const Components =
    [
        ManageStocksComponent,
        NewARInvoiceStockComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ManageStocksComponent": { myResult = ManageStocksComponent; break; }
            case "NewARInvoiceStockComponent": { myResult = NewARInvoiceStockComponent; break; }
        }

        return myResult;
    }
}

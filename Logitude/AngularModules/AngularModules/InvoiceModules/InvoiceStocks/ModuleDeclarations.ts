import { ManageStocksComponent } from './Components/ManageStocksComponent';
import { NewARInvoiceStockComponent } from './Components/NewEntity/NewARInvoiceStockComponent';
import { ARInvoiceStockInputTemplate } from './Components/ARInvoiceStockInputTemplate';

export const Components =
    [
        ManageStocksComponent,
        NewARInvoiceStockComponent,
        ARInvoiceStockInputTemplate,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ManageStocksComponent": { myResult = ManageStocksComponent; break; }
            case "NewARInvoiceStockComponent": { myResult = NewARInvoiceStockComponent; break; }
            case "ARInvoiceStockInputTemplate": { myResult = ARInvoiceStockInputTemplate; break; }
        }

        return myResult;
    }
}

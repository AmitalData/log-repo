import { InvoiceQueueComponent } from "./Components/InvoiceQueueComponent";
 

export const Components =
    [
        InvoiceQueueComponent,
      ];
export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "InvoiceQueueComponent": { myResult = InvoiceQueueComponent; break; }
 
         }
        return myResult;
    }
} 

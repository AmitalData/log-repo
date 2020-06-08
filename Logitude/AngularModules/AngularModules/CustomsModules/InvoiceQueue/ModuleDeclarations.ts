import { InvoiceQueue } from "./Components/InvoiceQueue";
 

export const Components =
    [
        InvoiceQueue,
     ];
export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "InvoiceQueue": { myResult = InvoiceQueue; break; }
         }
        return myResult;
    }
} 

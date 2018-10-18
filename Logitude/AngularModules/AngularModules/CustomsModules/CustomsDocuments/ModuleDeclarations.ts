
import { CustomsDocumentsComponent } from './Components/CustomsDocumentsComponent';
import { AddEditCustomsDocumentComponent } from './Components/AddEditCustomsDocumentComponent';
import { DocumentsFilingsQueryComponent } from './Components/DocumentsFilingsQueryComponent';



export const Components =
  [
    
    CustomsDocumentsComponent,

    DocumentsFilingsQueryComponent,

    AddEditCustomsDocumentComponent,
  ];

export class ModuleDeclarations {
  public static Get(name: string) {

    var myResult: any = null;

    switch (name) {
      case "CustomsDocumentsComponent": { myResult = CustomsDocumentsComponent; break; }
      case "AddEditCustomsDocumentComponent": { myResult = AddEditCustomsDocumentComponent; break; }
      case "DocumentsFilingsQueryComponent": { myResult = DocumentsFilingsQueryComponent; break; }

    }

    return myResult;
  }
}

import { CustomsRequestsSheetsComponent } from './Components/CustomsRequestsSheetsComponent';
import { CustomsErrorsComponent } from './Components/CustomsErrorsComponent';
import { CustomSendOptionsComponent } from './Components/CustomSendOptionsComponent';
import { DropdownButtonComponent } from './Components/DropdownButtonComponent';
import { CustomMessageWrapperComponent } from './Components/CustomMessageWrapperComponent';
import { CustomMessageProgressComponent } from './Components/CustomMessageProgressComponent';
import { NotificationComponent } from './Components/NotificationComponent';
import { ObjectViewerComponent } from './Components/ObjectViewerComponent';
import { SendDeclarationTastCaseComponent } from './Components/TestCase/SendDeclarationTastCaseComponent';
import { RemarksPopUp } from './Components/RemarksPopUp';

export const Components =
    [
        CustomsRequestsSheetsComponent,
        CustomsErrorsComponent,
        CustomSendOptionsComponent,
        DropdownButtonComponent,
        CustomMessageWrapperComponent,
        CustomMessageProgressComponent,
        NotificationComponent,
        ObjectViewerComponent,
        SendDeclarationTastCaseComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomsRequestsSheetsComponent": { myResult = CustomsRequestsSheetsComponent; break; }
            case "CustomsErrorsComponent": { myResult = CustomsErrorsComponent; break; }
            case "CustomSendOptionsComponent": { myResult = CustomSendOptionsComponent; break; }
            case "DropdownButtonComponent": { myResult = DropdownButtonComponent; break; }
            case "CustomMessageWrapperComponent": { myResult = CustomMessageWrapperComponent; break; }
            case "CustomMessageProgressComponent": { myResult = CustomMessageProgressComponent; break; }
            case "NotificationComponent": { myResult = NotificationComponent; break; }
            case "ObjectViewerComponent": { myResult = ObjectViewerComponent; break; }
            case "SendDeclarationTastCaseComponent": { myResult = SendDeclarationTastCaseComponent; break; }
            case "RemarksPopUp": { myResult = RemarksPopUp; break; }

        }

        return myResult;
    }
}

import {InboundEmailGeneralTabComponent} from './Components/EditTabs/InboundEmailGeneralTabComponent';
import {ViewInboundLineBodyComponent} from './Components/EditTabs/ViewInboundLineBodyComponent';
import {NewInboundEmailComponent} from './Components/NewEntity/NewInboundEmailComponent';


export const Components =
    [
        InboundEmailGeneralTabComponent,
        ViewInboundLineBodyComponent,
        NewInboundEmailComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "InboundEmailGeneralTabComponent": { myResult = InboundEmailGeneralTabComponent; break; }
            case "ViewInboundLineBodyComponent": { myResult = ViewInboundLineBodyComponent; break; }
            case "NewInboundEmailComponent": { myResult = NewInboundEmailComponent; break; }

        }

        return myResult;
    }
}
import {FilingInboxWorkspaceComponent} from './Components/FilingInboxWorkspaceComponent';
import {ChooseEntityComponent} from './Components/ChooseEntityComponent';
import {ForwarderChooseShipmentsComponent} from './Components/ForwarderChooseShipmentsComponent';

export const Components =
    [
        FilingInboxWorkspaceComponent,
        ChooseEntityComponent,
        ForwarderChooseShipmentsComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FilingInboxWorkspaceComponent": { myResult = FilingInboxWorkspaceComponent; break; }
            case "ChooseEntityComponent": { myResult = ChooseEntityComponent; break; }
            case "ForwarderChooseShipmentsComponent": { myResult = ForwarderChooseShipmentsComponent; break; }     
        }

        return myResult;
    }
}
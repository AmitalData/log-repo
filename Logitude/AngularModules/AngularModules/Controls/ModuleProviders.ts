import {CRMControlsService} from './Services/CRMControlsService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CRMControlsService": { myResult = new CRMControlsService(); break; }
        }

        return myResult;
    }
}
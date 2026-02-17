import {DashboardDomainService} from './Services/DashboardDomainService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
        
            case "DashboardDomainService": { myResult = new DashboardDomainService(); break; }
        }

        return myResult;
    }
}
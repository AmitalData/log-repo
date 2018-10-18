import {ReportsDomainService} from './Services/ReportsDomainService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            case "ReportsDomainService": { myResult = new ReportsDomainService(); break; }
        }

        return myResult;
    }
}
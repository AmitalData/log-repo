import {DashboardDomainService} from './Services/DashboardDomainService';
import { WidgetTypeListService } from 'DashboardModule/Services/StandardLists/WidgetTypeListService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {   
            case "DashboardDomainService": { myResult = new DashboardDomainService(); break; }
            case "WidgetTypeListService": { myResult = new WidgetTypeListService(); break; }
        }

        return myResult;
    }
}
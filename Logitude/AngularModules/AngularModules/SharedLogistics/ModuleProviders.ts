import {SharedLogisticsService} from './Services/Others/SharedLogisticsService';
import {SharedLogisticContactService} from './Services/ExtendedPMs/SharedLogisticContactService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            case "SharedLogisticsService": { myResult = new SharedLogisticsService(); break; }
            case "SharedLogisticContactService": { myResult = new SharedLogisticContactService(); break; }
                
        }

        return myResult;
    }
}
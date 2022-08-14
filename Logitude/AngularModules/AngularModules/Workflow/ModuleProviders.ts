
import { WorkFlowListService } from './Services/StandardLists/WorkFlowListService';
import { WorkFlowPMService } from './Services/StandardPMs/WorkFlowPMService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var service: any = null;

        switch (name) {
            // List
            case "WorkFlowListService": { service = new WorkFlowListService(); break; }

            // PM
            case "WorkFlowPMService": { service = new WorkFlowPMService(); break; }
        }

        return service;
    }
}
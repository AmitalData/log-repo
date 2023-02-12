
import { WorkFlowMenuButtonsHandler } from './Components/MenuButtons/WorkFlowMenuButtonsHandler';
import { WorkFlowInstanceListService } from './Services/StandardLists/WorkFlowInstanceListService';
import { WorkFlowListService } from './Services/StandardLists/WorkFlowListService';
import { WorkFlowStatusListService } from './Services/StandardLists/WorkFlowStatusListService';
import { WorkFlowVersionListService } from './Services/StandardLists/WorkFlowVersionListService';
import { WorkFlowPMService } from './Services/StandardPMs/WorkFlowPMService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var service: any = null;

        switch (name) {
            case "WorkFlowListService": { service = new WorkFlowListService(); break; }
            case "WorkFlowStatusListService": { service = new WorkFlowStatusListService(); break; }

            case "WorkFlowPMService": { service = new WorkFlowPMService(); break; }
            case "WorkFlowMenuButtonsHandler": { service = new WorkFlowMenuButtonsHandler(); break; }
            case "WorkFlowVersionListService": { service = new WorkFlowVersionListService(); break; }
            case "WorkFlowInstanceListService": { service = new WorkFlowInstanceListService(); break; }
            
        }

        return service;
    }
}

import { WorkFlowMenuButtonsHandler } from './Components/MenuButtons/WorkflowMenuButtonsHandler';
import { WorkFlowListService } from './Services/StandardLists/WorkFlowListService';
import { WorkFlowStatusListService } from './Services/StandardLists/WorkFlowStatusListService';
import { WorkFlowPMService } from './Services/StandardPMs/WorkFlowPMService';
import { WorkFlowVersionService } from './Services/WorkFlowVersionService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var service: any = null;

        switch (name) {
            case "WorkFlowListService": { service = new WorkFlowListService(); break; }
            case "WorkFlowStatusListService": { service = new WorkFlowStatusListService(); break; }

            case "WorkFlowPMService": { service = new WorkFlowPMService(); break; }
            case "WorkFlowVersionService": { service = new WorkFlowVersionService(); break; }
            
            case "WorkFlowMenuButtonsHandler": { service = new WorkFlowMenuButtonsHandler(); break; }
        }

        return service;
    }
}
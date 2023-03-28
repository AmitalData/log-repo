import { WorkFlowMenuButtonsHandler } from './Components/MenuButtons/WorkFlowMenuButtonsHandler';
import { WorkFlowInstanceListService } from './Services/StandardLists/WorkFlowInstanceListService';
import { WorkFlowInstanceStatusListService } from './Services/StandardLists/WorkFlowInstanceStatusListService';
import { WorkFlowListService } from './Services/StandardLists/WorkFlowListService';
import { WorkFlowStatusListService } from './Services/StandardLists/WorkFlowStatusListService';
import { WorkFlowVersionListService } from './Services/StandardLists/WorkFlowVersionListService';
import { WorkFlowVersionStatusListService } from './Services/StandardLists/WorkFlowVersionStatusListService';
import { WorkFlowPMService } from './Services/StandardPMs/WorkFlowPMService';
import { TaskListService } from './Services/StandardLists/TaskListService';
import { TaskPriorityListService } from './Services/StandardLists/TaskPriorityListService';
import { TaskStatusListService } from './Services/StandardLists/TaskStatusListService';
import { TaskTypeListService } from './Services/StandardLists/TaskTypeListService';
import { TaskPMService } from './Services/StandardPMs/TaskPMService';
import { TaskPriorityPMService } from './Services/StandardPMs/TaskPriorityPMService';
import { TaskStatusPMService } from './Services/StandardPMs/TaskStatusPMService';
import { TaskTypePMService } from './Services/StandardPMs/TaskTypePMService';
import { WorkFlowTriggerTypeListService } from './Services/StandardLists/WorkFlowTriggerTypeListService';

export class ModuleProviders {
    public static GetInstance(name: string) {
        var service: any = null;
        switch (name) {
            case "WorkFlowListService": { service = new WorkFlowListService(); break; }
            case "WorkFlowStatusListService": { service = new WorkFlowStatusListService(); break; }
            case "WorkFlowVersionStatusListService": { service = new WorkFlowVersionStatusListService(); break; }
            case "WorkFlowInstanceStatusListService": { service = new WorkFlowInstanceStatusListService(); break; }

            case "WorkFlowPMService": { service = new WorkFlowPMService(); break; }
            case "WorkFlowMenuButtonsHandler": { service = new WorkFlowMenuButtonsHandler(); break; }
            case "WorkFlowVersionListService": { service = new WorkFlowVersionListService(); break; }
            case "WorkFlowInstanceListService": { service = new WorkFlowInstanceListService(); break; }
            case "WorkFlowTriggerTypeListService": { service = new WorkFlowTriggerTypeListService(); break; }

            case "TaskListService": { service = new TaskListService(); break; }
            case "TaskPriorityListService": { service = new TaskPriorityListService(); break; }
            case "TaskStatusListService": { service = new TaskStatusListService(); break; }
            case "TaskTypeListService": { service = new TaskTypeListService(); break; }
            case "TaskPMService": { service = new TaskPMService(); break; }
            case "TaskPriorityPMService": { service = new TaskPriorityPMService(); break; }
            case "TaskStatusPMService": { service = new TaskStatusPMService(); break; }
            case "TaskTypePMService": { service = new TaskTypePMService(); break; }
        }
        return service;
    }
}
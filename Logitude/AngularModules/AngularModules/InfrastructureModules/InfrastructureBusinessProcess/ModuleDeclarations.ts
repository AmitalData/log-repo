import {BusinessRoleNewComponent} from './Components/BusinessRoleNewComponent';
import {QueueNewComponent} from './Components/BusinessProcessQueue/QueueNewComponent';
import {TeamNewComponent} from './Components/Team/TeamNewComponent';
import {TeamGeneralTabComponent} from './Components/Team/TeamGeneralTabComponent';
import {TasksWorkspaceComponent} from './Components/Workspaces/TasksWorkspaceComponent';

export const Components =
    [
        BusinessRoleNewComponent,
        QueueNewComponent,
        TeamNewComponent,
        TeamGeneralTabComponent,
        TasksWorkspaceComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "BusinessRoleNewComponent": { myResult = BusinessRoleNewComponent; break; }   
            case "QueueNewComponent": { myResult = QueueNewComponent; break; }          
            case "TeamNewComponent": { myResult = TeamNewComponent; break; }  
            case "TeamGeneralTabComponent": { myResult = TeamGeneralTabComponent; break; }  
            case "TasksWorkspaceComponent": { myResult = TasksWorkspaceComponent; break; }  
        }

        return myResult;
    }
}
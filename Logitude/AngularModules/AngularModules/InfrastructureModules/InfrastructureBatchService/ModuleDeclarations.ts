import {BatchServicesComponent} from './Components/BatchService/BatchServicesComponent';
import {EditBatchServiceComponent} from './Components/BatchService/EditBatchServiceComponent';
import {TaskSchedulerComponent} from './Components/TaskScheduler/TaskSchedulerComponent';
import {AddEditTaskSchedulerComponent} from './Components/TaskScheduler/AddEditTaskSchedulerComponent';

export const Components =
    [
        BatchServicesComponent,
        TaskSchedulerComponent,
        AddEditTaskSchedulerComponent,
        EditBatchServiceComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "BatchServicesComponent": { myResult = BatchServicesComponent; break; }
            case "TaskSchedulerComponent": { myResult = TaskSchedulerComponent; break; }
            case "AddEditTaskSchedulerComponent": { myResult = AddEditTaskSchedulerComponent; break; }
            case "EditBatchServiceComponent": { myResult = EditBatchServiceComponent; break; }
        }

        return myResult;
    }
}
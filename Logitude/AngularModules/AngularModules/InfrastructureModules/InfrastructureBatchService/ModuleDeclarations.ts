import {BatchServicesComponent} from './Components/BatchService/BatchServicesComponent';
import {EditBatchServiceComponent} from './Components/BatchService/EditBatchServiceComponent';
import {TaskSchedulerComponent} from './Components/TaskScheduler/TaskSchedulerComponent';
import {AddEditTaskSchedulerComponent} from './Components/TaskScheduler/AddEditTaskSchedulerComponent';

import {MainSchedulerComponent} from './Components/TaskScheduler/MainSchedulerComponent';

export const Components =
    [
        BatchServicesComponent,
        TaskSchedulerComponent,
        AddEditTaskSchedulerComponent,
        EditBatchServiceComponent,
        MainSchedulerComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "BatchServicesComponent": { myResult = BatchServicesComponent; break; }
            case "TaskSchedulerComponent": { myResult = TaskSchedulerComponent; break; }
            case "AddEditTaskSchedulerComponent": { myResult = AddEditTaskSchedulerComponent; break; }
            case "EditBatchServiceComponent": { myResult = EditBatchServiceComponent; break; }
            case "MainSchedulerComponent": { myResult = MainSchedulerComponent; break; }
        }

        return myResult;
    }
}
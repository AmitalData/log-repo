import {NewAppointmentComponent} from './Components/NewEntity/NewAppointmentComponent';
import {NewActivityComponent} from './Components/NewEntity/NewActivityComponent';
import {ActivityInputTemplate} from './Components/NewEntity/ActivityInputTemplate';
import {AddEditInviteesComponent} from './Components/NewEntity/AddEditInviteesComponent';
import {InviteeCheckBoxComponent} from './Components/NewEntity/InviteeCheckBoxComponent';
import {NewCallComponent} from './Components/NewEntity/NewCallComponent';
import {NewTaskComponent} from './Components/NewEntity/NewTaskComponent';
import {ActivityGeneralTabComponent} from './Components/EditTabs/ActivityGeneralTabComponent';
import {AddEditActivityNotesComponent} from './Components/EditTabs/AddEditActivityNotesComponent';

export const Components =   
    [
        NewAppointmentComponent,
        NewActivityComponent,
        ActivityInputTemplate,
        AddEditInviteesComponent,
        InviteeCheckBoxComponent,
        NewCallComponent,
        NewTaskComponent,
        ActivityGeneralTabComponent,
        AddEditActivityNotesComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewAppointmentComponent": { myResult = NewAppointmentComponent; break; }
            case "NewActivityComponent": { myResult = NewActivityComponent; break; }
            case "ActivityInputTemplate": { myResult = ActivityInputTemplate; break; } 
            case "AddEditInviteesComponent": { myResult = AddEditInviteesComponent; break; } 
            case "InviteeCheckBoxComponent": { myResult = InviteeCheckBoxComponent; break; } 
            case "NewCallComponent": { myResult = NewCallComponent; break; }
            case "NewTaskComponent": { myResult = NewTaskComponent; break; }
            case "ActivityGeneralTabComponent": { myResult = ActivityGeneralTabComponent; break; } 
            case "AddEditActivityNotesComponent": { myResult = AddEditActivityNotesComponent; break; } 

        }

        return myResult;
    }
}
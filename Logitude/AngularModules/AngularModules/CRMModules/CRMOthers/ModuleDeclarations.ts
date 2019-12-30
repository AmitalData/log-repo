import {NewSLAComponent} from './Components/SLA/NewSLAComponent';
import {SLAMainWindowComponent} from './Components/SLA/SLAMainWindowComponent';
import {AddEditEscalationComponent} from './Components/SLA/AddEditEscalationComponent';
import {BlockedCustomerComponent} from './Components/BlockedCustomer/BlockedCustomerComponent';
import {NewBusinessHourAndHolidaysComponent} from './Components/BusinessHour/NewBusinessHourAndHolidaysComponent';
import {AddEditBusinessHourHolidayComponent} from './Components/BusinessHour/AddEditBusinessHourHolidayComponent';
import { TicketSettingsComponent } from './Components/TicketSettings/TicketSettingsComponent';
import { SupportMailBoxComponent } from './Components/SupportMailBox/SupportMailBoxComponent';
import { AddEditSupportMailBoxComponent } from './Components/SupportMailBox/AddEditSupportMailBoxComponent';

//Questionnaire
import { AddEditQuestionnaireComponent } from './Components/Questionnaire/AddEditQuestionnaireComponent';
import { AddEditQuestionnaireQuestionComponent } from './Components/Questionnaire/AddEditQuestionnaireQuestionComponent';
import { QuestionnaireAnswersComponent } from './Components/Questionnaire/QuestionnaireAnswersComponent';
import { AddEditPickListComponent } from './Components/Questionnaire/AddEditPickListComponent';

export const Components =
    [
        NewSLAComponent,
        SLAMainWindowComponent,
        AddEditEscalationComponent,
        BlockedCustomerComponent,
        NewBusinessHourAndHolidaysComponent,
        AddEditBusinessHourHolidayComponent,
        TicketSettingsComponent,
        AddEditQuestionnaireComponent,
        AddEditQuestionnaireQuestionComponent,
        QuestionnaireAnswersComponent,
        AddEditPickListComponent,
        SupportMailBoxComponent,
        AddEditSupportMailBoxComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewSLAComponent": { myResult = NewSLAComponent; break; }
            case "SLAMainWindowComponent": { myResult = SLAMainWindowComponent; break; }
            case "NewBusinessHourAndHolidaysComponent": { myResult = NewBusinessHourAndHolidaysComponent; break; }
            case "AddEditBusinessHourHolidayComponent": { myResult = AddEditBusinessHourHolidayComponent; break; }
            case "AddEditEscalationComponent": { myResult = AddEditEscalationComponent; break; }
            case "BlockedCustomerComponent": { myResult = BlockedCustomerComponent; break; } 
            case "TicketSettingsComponent": { myResult = TicketSettingsComponent; break; }
            case "AddEditQuestionnaireComponent": { myResult = AddEditQuestionnaireComponent; break; }
            case "AddEditQuestionnaireQuestionComponent": { myResult = AddEditQuestionnaireQuestionComponent; break; }
            case "QuestionnaireAnswersComponent": { myResult = QuestionnaireAnswersComponent; break; }
            case "AddEditPickListComponent": { myResult = AddEditPickListComponent; break; }
            case "SupportMailBoxComponent": { myResult = SupportMailBoxComponent; break; }
            case "AddEditSupportMailBoxComponent": { myResult = AddEditSupportMailBoxComponent; break; }
        }

        return myResult;
    }
}

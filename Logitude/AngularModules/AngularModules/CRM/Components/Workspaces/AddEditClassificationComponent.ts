import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {TicketClassificationPM} from '../../EntityPMs/TicketClassificationPM';
import {TicketClassificationPMService} from '../../Services/StandardPMs/TicketClassificationPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ClassificationChildArgs} from './TicketClassificationMaintenanceComponent';
import {TextCodeTranslator} from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import {CachedDataManager} from '../../../Infrastructure/Utilities/CachedDataManager';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditClassificationComponent.html',
})

export class AddEditClassificationComponent {
    private entityPM: TicketClassificationPM;
    public IsNew: boolean;
    public DataContext: any;
    public ObjectTableName: string = "TicketClassification";
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(args: ClassificationChildArgs) {
        this.entityPM = args.entityPM;
        this.IsNew = args.isNew;
        this.DataContext = args;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors: string[] = [];
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (!AppTool.IsNullOrEmpty(this.DataContext.Name) && this.DataContext.Name == "General") {
            if (AppTool.IsNullOrEmpty(this.DataContext.EmployeeGroupId))
                errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("TicketClassification.F.EmployeeGroupId")));
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {

            if (this.IsNew) {
                this.InsertClassification();
            }
            else {
                this.UpdateClassification();
            }
        }
    }
    private InsertClassification() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new TicketClassificationPMService();
        service.insert(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            CachedDataManager.RefreshTableData("TicketClassification", true);

        });
    }
    private UpdateClassification() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new TicketClassificationPMService();
        service.update(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            CachedDataManager.RefreshTableData("TicketClassification", true);
        });
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.entityPM);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('EmployeeGroupId');
        this.myCloner.AddField('DefaultSeverityId');
        this.myCloner.AddField('ManagerUserId');
        this.myCloner.AddField('EscalationNotify');
        this.myCloner.AddField('Inactive');
        this.myCloner.AddEntity(this.entityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

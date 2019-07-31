import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SprintPM} from '../../EntityPMs/SprintPM';
import {SprintPMService} from '../../Services/StandardPMs/SprintPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'NewSprintComponent',
    moduleId: module.id,
    templateUrl: './NewSprintComponent.html',
})

export class NewSprintComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "Sprint";
    public EntityPM: SprintPM;
    public SelectedLocationFilter: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new SprintPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get FromDate() {
        return this.EntityPM.FromDate;
    }
    set FromDate(value: Date) {
        if (this.EntityPM.FromDate != value) {
            this.EntityPM.FromDate = value;
        }
    }

    get ToDate() {
        return this.EntityPM.ToDate;
    }
    set ToDate(value: Date) {
        if (this.EntityPM.ToDate != value) {
            this.EntityPM.ToDate = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService: SprintPMService = new SprintPMService();
        myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
}

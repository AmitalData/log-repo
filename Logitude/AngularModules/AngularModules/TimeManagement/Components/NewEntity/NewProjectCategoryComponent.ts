import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { TMProjectCategoryPM } from '../../EntityPMs/TMProjectCategoryPM';
import { TMProjectCategoryPMService } from '../../Services/StandardPMs/TMProjectCategoryPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {DateTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'NewProjectCategoryComponent',
    moduleId: module.id,
    templateUrl: './NewProjectCategoryComponent.html',
})

export class NewProjectCategoryComponent extends BaseComponent {
    public DataContext = this;
    public ObjectTableName = "TMProjectCategory";
    public EntityPM: TMProjectCategoryPM;
    public SelectedLocationFilter: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new TMProjectCategoryPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Inactive() {
        return this.EntityPM.Inactive;
    }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    
    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService: TMProjectCategoryPMService = new TMProjectCategoryPMService();
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

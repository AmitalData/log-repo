import { Component } from '@angular/core';
import { ScreenExtendedService } from '../../../../Infrastructure/Services/ExtendedPMs/ScreenExtendedService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
declare var window: any;
import { ScreenPM } from '../../../../Infrastructure/EntityPMs/ScreenPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';


const valdationMessageOfName = 'Name is required';

@Component({

    templateUrl: './AddEditScreenComponent.html',
})

export class AddEditScreenComponent extends BaseComponent {

    DataContext: AddEditScreenComponent = this;
    ViewModel: any;
    private screenExtendedService: ScreenExtendedService;
    ValidationErrorsList: any[];
    public EntityPM: ScreenPM;
    IsNew: boolean = true;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.screenExtendedService = new ScreenExtendedService();

        this.UIProperties.SetRequired("Code", "ObjectField", true);
    }

    SetWindowArgs(args: any) {
        this.ViewModel = args.ViewModel;
        this.EntityPM = this.GetNewScreenPM();
    }


    GetNewScreenPM() {
        var screenPM = new ScreenPM();
        screenPM.Type = "LIGHTENING";
        screenPM.Tenant = SessionLocator.Tenant;
        screenPM.NumberOfRows = 1;
        screenPM.NumberOfColumns = 2;

        var objectTable = window.ObjectTables.filter(x => x.Id === this.ViewModel.ObjecttableId)[0];
        screenPM.ObjectTableId = objectTable.Id;
        screenPM.ObjectTableName = objectTable.Name;
        return screenPM;

    }


    get Name() { return this.EntityPM ? this.EntityPM.Name:""; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }




    SaveButtonClicked() {

        let errors = this.ValidateScreen();
        if(errors.length > 0)
            return this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicatorSaving();
        this.screenExtendedService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {

            this.CurrentSession.StopBusyIndicator();

            if (response.HasError)
                return this.HandleException();

            this.ViewModel.AddScreenItem(response.Result);
            this.CurrentSession.CloseCurrentWindow();
        });
    }

    private ValidateScreen()
    {
        let errors = [];
        if (!this.Name)
            errors.push(valdationMessageOfName);
        return errors;
    }

    HandleException() {

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}

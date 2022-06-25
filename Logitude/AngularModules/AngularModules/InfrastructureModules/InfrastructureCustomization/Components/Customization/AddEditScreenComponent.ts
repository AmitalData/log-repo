import { Component } from '@angular/core';
import { ScreenExtendedService } from '../../../../Infrastructure/Services/ExtendedPMs/ScreenExtendedService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
declare var window: any;
import { ScreenPM } from '../../../../Infrastructure/EntityPMs/ScreenPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({

    templateUrl: './AddEditScreenComponent.html',
})

export class AddEditScreenComponent extends BaseComponent {

    DataContext: AddEditScreenComponent = this;
    private screenLayoutComponent: any;
    private screenExtendedService: ScreenExtendedService;
    ValidationErrorsList: any[];
    public EntityPM: ScreenPM;
    IsNew: boolean = true;
    private numberOfRows: number =1;
    private numberOfColumns: number = 3;
    private screenType: string = "LIGHTENING";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.screenExtendedService = new ScreenExtendedService();

    }

    SetWindowArgs(args: any) {
        this.screenLayoutComponent = args.ScreenLayoutComponent;
        this.EntityPM = this.GetNewScreenInStance();
        this.UIProperties.SetRequired("Code", "Screen", true);

    }


    GetNewScreenInStance() {
        var screen = new ScreenPM();
        screen.Type = this.screenType;
        screen.Tenant = SessionLocator.Tenant;
        screen.NumberOfRows = this.numberOfRows;
        screen.NumberOfColumns = this.numberOfColumns;
        this.SetObjectTableFields(screen);
        return screen;

    }


    private SetObjectTableFields(screen: ScreenPM) {
        var objectTable = window.ObjectTables.filter(x => x.Id === this.screenLayoutComponent.ObjecttableId)[0];
        screen.ObjectTableId = objectTable.Id;
        screen.ObjectTableName = objectTable.Name;
    }

    get Name() { return this.EntityPM ? this.EntityPM.Name:""; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }



    
    SaveButtonClicked() {

        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Name is Required");
            return;
        }


        this.CurrentSession.StartBusyIndicatorSaving();
        this.screenExtendedService.insert(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.HandleException(myResponse);
                return;
            }

            this.screenLayoutComponent.AddScreenItem(myResult.Result);
            this.CurrentSession.CloseCurrentWindow();

        });
    }


    HandleException(serviceResponse: ServiceResponse) {
        if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) {
            this.ShowMessageWindow(serviceResponse.ErrorsArray[0], "Logitude Message");
        }
    }


    public ShowMessageWindow(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
        if (!title) return;
        messageWindow.Title = title;

        
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}

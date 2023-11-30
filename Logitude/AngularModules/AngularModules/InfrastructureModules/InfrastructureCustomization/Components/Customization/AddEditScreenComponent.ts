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
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

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
    private numberOfColumnsSubEntities: number = 2;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsEditMode: boolean = false;
    public IsSubEntity: boolean;
    public IsCustomObjectTable: boolean;
    public ScreenTypes: ScreenTypeDetails[] = [];

    private selectedScreenType: ScreenTypeDetails;
    get SelectedScreenType() { return this.selectedScreenType; }
    set SelectedScreenType(value: ScreenTypeDetails) {
        if (this.selectedScreenType != value) {
            this.selectedScreenType = value;
        }
    }

    constructor() {
        super();
        this.screenExtendedService = new ScreenExtendedService();    
    }

    FillScreenTypes() {
        if (this.IsSubEntity && this.IsCustomObjectTable)
            this.ScreenTypes.push(new ScreenTypeDetails("Grid", "Grid Screen Layout"));
        this.ScreenTypes.push(new ScreenTypeDetails("LIGHTENING", "Form Screen Layout"));
        if (!this.IsSubEntity && this.IsCustomObjectTable)
            this.ScreenTypes.push(new ScreenTypeDetails(null, "Header Screen Layout"));
    }

    SetWindowArgs(args: any) {
        this.screenLayoutComponent = args.ScreenLayoutComponent;
        this.IsSubEntity = args.IsSubEntity;

        this.EntityPM = args.Screen || this.GetNewScreenInstance();
        this.Name = this.EntityPM ? this.EntityPM.Name : "";
        this.Inactive = this.EntityPM?.Inactive;

        this.SetObjectTableFields(this.EntityPM);
        this.FillScreenTypes();
        this.IsEditMode = args.Screen != null;
        this.SetScreenType();
    }

    GetNewScreenInstance() {
        var screen = new ScreenPM();
        screen.Tenant = SessionLocator.Tenant;
        screen.NumberOfRows = this.numberOfRows;
        screen.NumberOfColumns = this.IsSubEntity ? this.numberOfColumnsSubEntities : this.numberOfColumns;
        return screen;
    }

    SetScreenType() {
        if (!(this.IsSubEntity && this.IsCustomObjectTable)) {
            this.selectedScreenType = this.ScreenTypes.filter(screenType => screenType.Code == "LIGHTENING")[0];
            return;
        }
        if (!this.IsEditMode && this.IsSubEntity && this.IsCustomObjectTable) {
            this.selectedScreenType = this.ScreenTypes.filter(screenType => screenType.Code == "Grid")[0];
            return;
        }
        if (this.IsEditMode) {
            this.selectedScreenType = this.ScreenTypes.filter(screenType => screenType.Code == this.EntityPM.Type)[0];
            return;
        }
    }

    private SetObjectTableFields(screen: ScreenPM) {
        var objectTable = window.ObjectTables.filter(x => x.Id === this.screenLayoutComponent.ObjecttableId)[0];
        screen.ObjectTableId = objectTable.Id;
        screen.ObjectTableName = objectTable.Name;
        this.IsCustomObjectTable = objectTable.IsCustom;
    }

    private name: string;
    get Name() { return this.name; }
    set Name(newValue: string) {
        if (this.name!= newValue) {
            this.name = newValue;
        }
    }

    private inactive: boolean;
    get Inactive() { return this.inactive; }
    set Inactive(newValue: boolean) {
        if (this.inactive != newValue) {
            this.inactive = newValue;
        }
    }






    SaveButtonClicked() {

        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Screen Name is Required");
            return;
        }

        if (this.Name.length > 200) {
            this.ValidationErrorsList.push("Name Field must be less than 200");
            return;
        }


        if (!this.selectedScreenType) {
            this.ValidationErrorsList.push("Type is Required");
            return;
        }

        if (this.selectedScreenType.Name == "Header Screen Layout" && this.CheckHeaderScreenAvailability()) {
            this.ValidationErrorsList.push("You already have a header screen");
            return;
        }

        this.MapScreenFields();
        this.IsEditMode ? this.SubmitScreenChanges() : this.SubmitNewScreen();
    }


   private MapScreenFields() {
       this.EntityPM.Name = this.Name;
       this.EntityPM.Inactive = this.Inactive;
       this.EntityPM.Type = this.selectedScreenType.Code;
       if (this.selectedScreenType.Name == "Header Screen Layout") {
           this.MapHeaderScreenFields();
       }
    }
    MapHeaderScreenFields() {
        this.EntityPM.IsHeaderScreen = true;
        this.EntityPM.NumberOfColumns = 4;
        this.EntityPM.NumberOfRows = 2;
    }
    CheckHeaderScreenAvailability() {
        if (this.screenLayoutComponent.TableScreensCollection.filter(screen => screen.ScreenPM.IsHeaderScreen == true)[0])
            return true;
    }

    private SubmitNewScreen()
    {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.screenExtendedService.insert(this.EntityPM).subscribe((myResult: ServiceResponse) =>
        {
            var myResponse: ServiceResponse = myResult;
            this.CurrentSession.StopBusyIndicator();

            if (myResponse.HasError) {
                this.HandleException(myResponse);
                return;
            }

            this.screenLayoutComponent.AddScreenItem(myResult.Result);
            this.CurrentSession.CurrentWindow.Close(myResult.Result.Id);

        });
    }
    private SubmitScreenChanges()
    {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.screenExtendedService.update(this.EntityPM).subscribe((response: ServiceResponse) =>
        {
            this.CurrentSession.StopBusyIndicator();

            if (response.HasError)
                return this.HandleException(response);

            this.CurrentSession.CurrentWindow.Close(this.EntityPM.Id);
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

class ScreenTypeDetails {
    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }
    Code: string;
    Name: string;
}

import { Component } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../../Infrastructure/Tools';
import { ScreenPM } from '../../../../../../Infrastructure/EntityPMs/ScreenPM';
import { CustomizationObjectTableService } from '../../../../ExternalService/CustomizationObjectTableService';
import { ObjectTablePM } from '../../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ApiQueryFilters } from '../../../../../../Infrastructure/DataContracts/ApiQueryFilters';
declare var window;

@Component({
    templateUrl: './AddEditGridScreenSectionComponent.html',
})

export class AddEditGridScreenSectionComponent extends BaseComponent {
    DataContext: AddEditGridScreenSectionComponent = this;
    ValidationErrorsList: any[];
    private CurrentSession = SessionLocator.SelectedSession;
    private IsNew: boolean = false;
    private ObjecttableId: string;
    AllGridScreens: ScreenPM[];
    SelectedScreen: string;
    CustomizationObjectTableService: CustomizationObjectTableService;
    ChildObjectTables: ObjectTablePM[] = [];
    ScreenFilterItems: ApiQueryFilters;
    constructor() {
        super();
        this.CustomizationObjectTableService = new CustomizationObjectTableService();
    }

    SetWindowArgs(args: any) {
        this.IsNew = args.IsNew;
        this.ObjecttableId = args.ObjecttableId;
        this.FillChildObjectTableIds();
        this.FillGridScreens();
        this.InitLOVFilters();
        if (this.IsNew) {
            this.IsLogLovReady = true;
            return;
        }
        this.FillEditArgsMode(args);
    }

    FillChildObjectTableIds() {
        this.ChildObjectTables = this.CustomizationObjectTableService.GetChildsById(this.ObjecttableId);
        if (!this.ChildObjectTables) {
            this.ChildObjectTables = [];
        }
    }

    FillGridScreens() {
        this.AllGridScreens = window.Screens.filter(screen => this.ChildObjectTables.some(childObjectTable => childObjectTable.Id == screen.ObjectTableId) && screen.Type == "Grid" && !screen.Inactive);
    }

    public IsLogLovReady: boolean = false;
    InitLOVFilters() {

        this.ScreenFilterItems = new ApiQueryFilters();
        this.ScreenFilterItems.addAdditionalFilter("ChildScreenGrid", this.ObjecttableId, null, null, "Equals", true, false, false, "string");
        this.ScreenFilterItems.Tenant = SessionLocator.Tenant;
        
    }
    FillEditArgsMode(args: any) {
        this.Name = args.Name;
        let selectedScreen = window.Screens.filter(screen => this.ChildObjectTables.some(childObjectTable => childObjectTable.Id == screen.ObjectTableId) && screen.Type == "Grid" && screen.Code == args.RelatedScreenCode && !screen.Inactive);
        if (selectedScreen && selectedScreen[0]) {
            this.GridScreensSelectionChanged(selectedScreen[0])
        }
        this.IsLogLovReady = true;
    }

    private name: string;
    get Name() { return this.name; }
    set Name(newValue: string) {
        if (this.name!= newValue) {
            this.name = newValue;
        }
    }

    public RelatedScreenCode: string;
    //private relatedScreenCode: string;
    //get RelatedScreenCode() { return this.relatedScreenCode; }
    //set RelatedScreenCode(newValue: string) {
    //    if (this.relatedScreenCode != newValue) {
    //        this.relatedScreenCode = newValue;
    //    }
    //}

    GridScreensSelectionChanged(selectedScreen: any) {
        this.SelectedScreen = selectedScreen ? selectedScreen.Id : null;
        this.RelatedScreenCode = selectedScreen ? selectedScreen.Code : "";
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Grid Title is Required");
        }

        if (this.Name && this.Name.length > 100) {
            this.ValidationErrorsList.push("Grid Title Field must be less than 100");
        }

        if (AppTool.IsNullOrEmpty(this.RelatedScreenCode)) {
            this.ValidationErrorsList.push("Choose Component is Required");
        }

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        
        this.CurrentSession.CloseCurrentWindowData({ GridName: this.Name, RelatedScreenCode: this.RelatedScreenCode });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

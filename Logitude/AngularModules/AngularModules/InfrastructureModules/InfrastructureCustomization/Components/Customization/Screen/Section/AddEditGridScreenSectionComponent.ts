import { Component } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../../Infrastructure/Tools';
import { ScreenPM } from '../../../../../../Infrastructure/EntityPMs/ScreenPM';
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
    SelectedScreen: ScreenPM;

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.IsNew = args.IsNew;
        this.ObjecttableId = args.ObjecttableId;
        this.FillGridScreens();
        if (this.IsNew) return;
        this.FillEditArgsMode(args);
    }

    FillGridScreens() {
        this.AllGridScreens = window.Screens.filter(d => d.ObjectTableId == this.ObjecttableId && d.Type == "Grid" && !d.Inactive);
    }

    FillEditArgsMode(args: any) {
        this.Name = args.Name;
        let selectedScreen = window.Screens.filter(d => d.ObjectTableId == this.ObjecttableId && d.Type == "Grid" && d.Code == args.RelatedScreenCode && !d.Inactive);
        if (selectedScreen && selectedScreen[0]) {
            this.GridScreensSelectionChanged(selectedScreen[0])
        }
    }

    private name: string;
    get Name() { return this.name; }
    set Name(newValue: string) {
        if (this.name!= newValue) {
            this.name = newValue;
        }
    }

    private relatedScreenCode: string;
    get RelatedScreenCode() { return this.relatedScreenCode; }
    set RelatedScreenCode(newValue: string) {
        if (this.relatedScreenCode != newValue) {
            this.relatedScreenCode = newValue;
        }
    }

    GridScreensSelectionChanged(selectedScreen: any) {
        this.SelectedScreen = selectedScreen ? selectedScreen : null;
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

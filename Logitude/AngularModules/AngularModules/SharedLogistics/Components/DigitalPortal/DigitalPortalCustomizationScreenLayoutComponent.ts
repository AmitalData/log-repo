import { Component } from '@angular/core';
import { DigitalPortalScreenList } from "../../../infrastructure/entitylists/digitalportalscreenlist"
import { DigitalCustomizationService } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    templateUrl: './DigitalPortalCustomizationScreenLayoutComponent.html',
})

export class DigitalPortalCustomizationScreenLayoutComponent {

    public Screens: Array<DigitalPortalScreenList> = [];
    constructor() {
        this.Screens = [];
    }

    public SelectedItem: DigitalPortalScreenList;
    private newSelectedItem: DigitalPortalScreenList;
    SelectionChanged(Item) {
        this.newSelectedItem = Item;
    }

    private hTMLEditor:string = null;
    get HTMLEditor() {
        return this.hTMLEditor;
    }
    set HTMLEditor(newValue: string) {
        if (newValue != this.hTMLEditor) {
            this.hTMLEditor = newValue;
        }
    }

    AddPredefinedComponentClicked() {

    }

    AddFieldCodesClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;
        logWindow.Title = "Insert Field";
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/AddDigitalFieldCodeComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {

                
            }
        });
    }

    RestoreDefaultLayoutClicked() {

    }

    PublichChangesClicked() {

    }

    PreviewChangesClicked() {

    }
}

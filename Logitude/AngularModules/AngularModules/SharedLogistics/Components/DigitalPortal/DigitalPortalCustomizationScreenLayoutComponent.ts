import { Component } from '@angular/core';
import { DigitalPortalScreenList } from "../../../infrastructure/entitylists/digitalportalscreenlist"
import { DigitalCustomizationService } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';

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

    }

    RestoreDefaultLayoutClicked() {

    }

    PublichChangesClicked() {

    }
}

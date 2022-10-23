import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenSectionPM } from '../../../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { ScreenFieldPM } from '../../../../../../Infrastructure/EntityPMs/ScreenFieldPM';
declare var window;

@Component({
    selector: 'GridScreenSectionPreview',
    templateUrl: './GridScreenSectionPreviewComponent.html',
    inputs: ['ScreenSection']
})

export class GridScreenSectionPreviewComponent extends BaseComponent implements OnInit {
    DataContext: GridScreenSectionPreviewComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ScreenSection: ScreenSectionPM;
    ScreenFields: ScreenFieldPM[];

    constructor() {
        super();
    }

    ngOnInit() {
        this.SetScreenFields();
        this.CurrentSession.SessionEvent.subscribe(($event: any) => {
            if ($event.Name == "ReloadGridSection") {
                this.SetScreenFields();
            }
        });
    }

    SetScreenFields() {
        if (!this.ScreenSection) return;
        this.ScreenFields = window.ScreenFields.filter(screenField => screenField.Tenant == SessionLocator.Tenant && screenField.ScreenCode == this.ScreenSection.RelatedScreenCode);
        if (!this.ScreenFields) return;
        this.OrderScreenFieldsByColumn();
    }

    OrderScreenFieldsByColumn() {
        this.ScreenFields = this.ScreenFields.sort((screenField1, screenField2) => {
            if (screenField1.Column > screenField2.Column) {
                return 1;
            }

            if (screenField1.Column < screenField2.Column) {
                return -1;
            }

            return 0;
        });
    }

    SetWindowArgs(args: any) {

    }
}

import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenSectionPM } from '../../../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { ScreenFieldPM } from '../../../../../../Infrastructure/EntityPMs/ScreenFieldPM';
import { EntityResourceService } from '../../../../../../Infrastructure/Services/EntityResourceService';
declare var window;

@Component({
    selector: 'GridScreenSectionPreview',
    templateUrl: './GridScreenSectionPreviewComponent.html',
    inputs: ['ScreenSection']
})

export class GridScreenSectionPreviewComponent extends BaseComponent implements OnInit {
    DataContext: GridScreenSectionPreviewComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    ScreenSection: ScreenSectionPM;
    ScreenFields: ScreenFieldPM[];
    IsReady: boolean;

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

        let relatedScreen = window.Screens.filter((screen: any) => screen.Code === this.ScreenSection.RelatedScreenCode)[0];
        if (!relatedScreen) return;
        let childObjectTable = window.ObjectTables.filter((table: any) => table.Id === relatedScreen.ObjectTableId)[0];
        if (!childObjectTable) return;

        if (childObjectTable.IsCustom) {
            this.LoadCompleted();
            return;
        }
        this.entityResourceService.getEntityResourceByTableName(childObjectTable.Name).subscribe((response: any) => {
            this.LoadCompleted();
        });
    }

    private LoadCompleted() {
        this.IsReady = true;
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

/// <reference path="stimulsoftviewercomponent.ts" />

import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {StimulsoftViewerComponent} from '../../../Infrastructure/Components/StimulsoftComponent/StimulsoftViewerComponent';


@Component({
    moduleId: module.id,

    selector: 'ExportSettingAdvanceComponent',
    templateUrl: './ExportSettingAdvanceComponent.html',


})

export class ExportSettingAdvanceComponent implements OnInit {

   
    StimulsoftViewerComponent: StimulsoftViewerComponent;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }
    ngOnInit() {


    }

    SetWindowArgs(args: any) {

        this.StimulsoftViewerComponent = args.StimulsoftViewerComponent;
        if (this.StimulsoftViewerComponent != null) {
            this.ExportDataOnly = this.StimulsoftViewerComponent.ExportDataOnly;
            this.ExportObjectFormatting = this.StimulsoftViewerComponent.ExportObjectFormatting;

            //this.UseOnePageHeaderandFooter = this.StimulsoftViewerComponent.UseOnePageHeaderandFooter;
        }
        this.UseOnePageHeaderandFooter = !this.ExportDataOnly;
    }

    private exportDataOnly: boolean = false;
    public get ExportDataOnly() {
        return this.exportDataOnly;

    }
    public set ExportDataOnly(newValue: boolean) {
        if (this.exportDataOnly != newValue) {
            this.exportDataOnly = newValue;
            if (this.exportDataOnly) {

                this.ExportObjectFormatting = false;
                this.UseOnePageHeaderandFooter = false;
            }
        }
    }

    private exportObjectFormatting: boolean = false;
    public get ExportObjectFormatting() {
        return this.exportObjectFormatting;

    }
    public set ExportObjectFormatting(newValue: boolean) {
        if (this.exportObjectFormatting != newValue) {
            this.exportObjectFormatting = newValue;

        }
    }


    private useOnePageHeaderandFooter: boolean = false;
    public get UseOnePageHeaderandFooter() {
        return this.useOnePageHeaderandFooter;

    }
    public set UseOnePageHeaderandFooter(newValue: boolean) {
        if (this.useOnePageHeaderandFooter != newValue) {
            this.useOnePageHeaderandFooter = newValue;

        }
    }


    CloseButtonClicked() {
        
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {

        if (this.StimulsoftViewerComponent != null) {
            this.StimulsoftViewerComponent.SaveToExcelFileAdvanced(this.ExportDataOnly, this.ExportObjectFormatting, this.UseOnePageHeaderandFooter);
        }
        this.CurrentSession.CloseCurrentWindow();

    }


}

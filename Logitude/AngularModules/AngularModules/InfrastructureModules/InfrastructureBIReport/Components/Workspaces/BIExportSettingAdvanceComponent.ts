import {Component, OnInit}  from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportPreviewComponent } from './BIReportPreviewComponent';


@Component({
    selector: 'BIExportSettingAdvanceComponent',
    templateUrl: './BIExportSettingAdvanceComponent.html'
})

export class BIExportSettingAdvanceComponent implements OnInit {
    BIReportPreviewComponent: BIReportPreviewComponent;
    private type: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }
    ngOnInit() {


    }

    SetWindowArgs(args: any) {
        this.BIReportPreviewComponent = args.BIReportPreviewComponent;
        this.type = args.Type;
    }

    private includeTotals: boolean = false;
    public get IncludeTotals() {
        return this.includeTotals;

    }
    public set IncludeTotals(newValue: boolean) {
        if (this.includeTotals != newValue) {
            this.includeTotals = newValue;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        if (this.BIReportPreviewComponent != null && this.BIReportPreviewComponent.BIReportXMLData != null) {
            this.BIReportPreviewComponent.BIReportXMLData.IncludeTotals = this.IncludeTotals;
            this.BIReportPreviewComponent.ExportButtonClicked(this.type);
        }
        this.CurrentSession.CloseCurrentWindow();
    }

}

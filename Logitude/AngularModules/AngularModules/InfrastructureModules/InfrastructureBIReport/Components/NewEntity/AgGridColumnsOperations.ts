import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { BIReportPreviewComponent } from  '../Workspaces/BIReportPreviewComponent'

@Component({
    moduleId: module.id,
    templateUrl: './AgGridColumnsOperations.html',
})

export class AgGridColumnsOperations extends BaseComponent {
    public DataContext: AgGridColumnsOperations = this;
    public ItemsSource: any[];
    private father: BIReportPreviewComponent;

    constructor() {
        super();
        this.ItemsSource = [];
    }

    SetWindowArgs(args: any) {
        this.father = args.father;
        this.ItemsSource = this.father.BIReportXMLData.BITabularViewSettings.Columns;
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        this.father.BuildColumns(this.father.BIReportXMLData);
        SessionLocator.CurrentSession.CloseCurrentWindowEmit('ok');
    }

    btnUp_Click() {

    }

    btnDown_Click() {
        

    }
}

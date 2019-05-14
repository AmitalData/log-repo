import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { BIReportPreviewComponent } from  '../Workspaces/BIReportPreviewComponent'
import { InfrastructureDomainService, Column, BITabularViewSettings, BIReportXMLData} from '../../../../Infrastructure/Services/InfrastructureDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './AgGridColumnsOperations.html',
})

export class AgGridColumnsOperations extends BaseComponent {
    public DataContext: AgGridColumnsOperations = this;
    public ItemsSource: any[];
    private father: BIReportPreviewComponent;
    public IsAll = false;
    itemSource_Unsaved: Array<Column> = []; 
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    BuildList() {
        this.ItemsSource = [];
        this.IsAll = this.father.BIReportXMLData.BITabularViewSettings.Columns.filter(a => !a.IsChecked)[0] != null ? false : true;
        this.father.BIReportXMLData.BITabularViewSettings.Columns.sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
            this.ItemsSource.push(item);
        });
    }
    SetWindowArgs(args: any) {
        this.father = args.father;
        this.itemSource_Unsaved = JSON.parse(JSON.stringify(this.father.BIReportXMLData.BITabularViewSettings.Columns));
        this.BuildList();
    }

    public FieldSelectedItem = null;
    SetFieldSelectedItem(item) {
        this.FieldSelectedItem = item;
    }

    CancelButtonClicked() {
        this.father.BIReportXMLData.BITabularViewSettings.Columns = this.itemSource_Unsaved;
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        var _InfrastructureDomainService = new InfrastructureDomainService();
        var result = new BIReportXMLData();
        result.BITabularViewSettings = this.father.BIReportXMLData.BITabularViewSettings;
        result.BIReportId = this.father.EntityId;
        result.BIReportPM = this.father.EntityPM;
        _InfrastructureDomainService.UpdateBIReportXMLData(result).subscribe(myResult => {
            if (!myResult.HasError) {
                this.father.BIReportXMLData = myResult.Result;
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
        });
    }

    btnUp_Click() {
        var item = this.FieldSelectedItem;
        if (item != null && item.Code != "All") {
            var currIndex = this.father.BIReportXMLData.BITabularViewSettings.Columns.indexOf(item);
            var nextIndex = currIndex - 1;

            var currItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[currIndex];
            var nextItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[nextIndex];

            if (nextItem) {
                currItem.Index = nextIndex;
                nextItem.Index = currIndex;
                this.BuildList();
            }
        }
    }

    btnDown_Click() {
        var item = this.FieldSelectedItem;
        if (item != null && item.Code != "All") {
            var currIndex = this.father.BIReportXMLData.BITabularViewSettings.Columns.indexOf(item);
            var nextIndex = currIndex + 1;

            var currItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[currIndex];
            var nextItem = this.father.BIReportXMLData.BITabularViewSettings.Columns[nextIndex];

            if (nextItem) {
                currItem.Index = nextIndex;
                nextItem.Index = currIndex;
                this.BuildList();
            }
        }
    }

    onValueChanged(item, index, event) {
        item.IsChecked = event;
        item.Index = index;
        this.IsAll = this.father.BIReportXMLData.BITabularViewSettings.Columns.filter(a => !a.IsChecked)[0] != null ? false : true;
    }
    IsAllClicked(event) {
        this.father.BIReportXMLData.BITabularViewSettings.Columns.sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
            item.IsChecked = event;
        });
    }
}

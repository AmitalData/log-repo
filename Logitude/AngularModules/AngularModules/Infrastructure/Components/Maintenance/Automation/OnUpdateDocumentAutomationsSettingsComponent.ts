import { Component, EventEmitter, Output } from '@angular/core';
import { BaseComponent } from '../../LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Utilities/SessionLocator';
import { AutomationItemViewModel } from './ViewModel/AutomationItemViewModel';



@Component({
    selector: 'OnUpdateDocumentAutomationsSettings',
    templateUrl: './OnUpdateDocumentAutomationsSettingsComponent.html',
    inputs: ['OnDocumentUpdateAutomationList'],

})
export class OnUpdateDocumentAutomationsSettingsComponent extends BaseComponent {

    public DataContext: any;
    private CurrentSession = SessionLocator.SelectedSession;
    OnDocumentUpdateAutomationList: AutomationItemViewModel[] = [];
    OnDocumentUpdateAutomationListSelected: AutomationItemViewModel;
    IsInCludeInActiveOnDocumentUpdateCheckBox: boolean;

    @Output() RefreshAutomation = new EventEmitter();
    @Output() AddAutomation = new EventEmitter();
    @Output() EditAutomation = new EventEmitter();
    constructor() {
        super();

        this.DataContext = this;
    }

    SetWindowArgs(args: any) {

    }

    CheckboxInCludeInActiveClick() {
        this.IsInCludeInActiveOnDocumentUpdateCheckBox = !this.IsInCludeInActiveOnDocumentUpdateCheckBox;
        this.RefreshAutomation.emit(({ automationListType: 'OnDocumentUpdate', IsInCludeInActive: this.IsInCludeInActiveOnDocumentUpdateCheckBox }));
    }

    AddDocumentUpdateAutomation() {
        this.AddAutomation.emit(({ type: 'OnDocumentUpdate'}));
    }

    EditDocumentUpdateAutomation(item) {
        this.EditAutomation.emit(({ type: 'OnDocumentUpdate', item: item }));
    }

    AutomationDocumentUpdateListChangeSelected(item: AutomationItemViewModel) {
        this.OnDocumentUpdateAutomationListSelected = item;
        this.OnDocumentUpdateAutomationList.forEach((automation) => {
            automation.IsShowArrowUpDown = false;
        });

        this.OnDocumentUpdateAutomationListSelected.IsShowArrowUpDown = true;

    }

    ArrowUpAutomationButtonClicked(item: AutomationItemViewModel) {
        if (item == null) return;

        var i = this.OnDocumentUpdateAutomationList.indexOf(item);
        var upColumn = this.OnDocumentUpdateAutomationList[i - 1];
        if (i <= 0) return;

        this.OnDocumentUpdateAutomationList = this.OnDocumentUpdateAutomationList.filter(d => d.Id != upColumn.Id);
        var tempOrder = item.Order;
        item.EntityPM.Order = item.Order = upColumn.Order;
        upColumn.Order = upColumn.EntityPM.Order = tempOrder;
        this.OnDocumentUpdateAutomationList.splice(i, 0, upColumn);
    }

    ArrowDownAutomationButtonClicked(item: AutomationItemViewModel) {
        if (item == null) return;

        var i = this.OnDocumentUpdateAutomationList.indexOf(item);
        var downColumn = this.OnDocumentUpdateAutomationList[i + 1];
        if (i >= this.OnDocumentUpdateAutomationList.length - 1) return;

        this.OnDocumentUpdateAutomationList = this.OnDocumentUpdateAutomationList.filter(d => d.Id != downColumn.Id);
        var tempOrder = item.Order;
        item.EntityPM.Order = item.Order = downColumn.Order;
        downColumn.Order = downColumn.EntityPM.Order = tempOrder;
        this.OnDocumentUpdateAutomationList.splice(i, 0, downColumn);
    }
}

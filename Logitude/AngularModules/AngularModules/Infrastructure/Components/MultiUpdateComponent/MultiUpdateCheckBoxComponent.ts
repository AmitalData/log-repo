declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './MultiUpdateCheckBoxComponent.html',
})
export class MultiUpdateCheckBoxComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public RowData;
    constructor(private CD: ChangeDetectorRef) {

    }
    setVariables(rowData: any) {
        var record = window.AllRecords?.filter(item => item.Id == rowData.Id)[0];
        if (record) rowData.IsChecked = (record.IsChecked == undefined || record.IsChecked == true);
        else rowData.IsChecked = true;
        this.RowData = rowData;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        this.RowData.IsChecked = newValue;
        this.CurrentSession.PseventRowSelectEvent.emit(this.RowData);
    }
}


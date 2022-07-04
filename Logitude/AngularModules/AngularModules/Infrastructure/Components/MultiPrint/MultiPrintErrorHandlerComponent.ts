import { Component } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { PrintingRow } from '../../Services/BatchPrintService';
import { AppTool } from '../../Tools';

@Component({
    selector: 'MultiPrintErrorHandlerComponent',
    templateUrl: 'MultiPrintErrorHandlerComponent.html',
})

export class MultiPrintErrorHandlerComponent {
    private CurrentSession = SessionLocator.SelectedSession;    
    public ItemsSource: PrintingRow[];
    public PrintingRows: PrintingRow[];
    constructor() {

    }

    SetWindowArgs(windowArgs: any) {
        this.PrintingRows = windowArgs.PrintingRows;
        this.FillItemsSource();
    }

    private FillItemsSource() {
        this.ItemsSource = this.PrintingRows.filter(f => !AppTool.IsNullOrEmpty(f.Error));
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

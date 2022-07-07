import { Component } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { PrintingRow } from '../../Services/BatchPrintService';
import { AppTool } from '../../Tools';
import { DownloadManager } from '../../Utilities/DownloadManager';

@Component({
    selector: 'MultiPrintErrorHandlerComponent',
    templateUrl: 'MultiPrintErrorHandlerComponent.html',
})

export class MultiPrintErrorHandlerComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: PrintingRow[];
    public PrintingRows: PrintingRow[];
    public DocumentId: string;
    public FileName: string;
    public SecurityId: string;
    constructor() {

    }

    SetWindowArgs(windowArgs: any) {
        this.PrintingRows = windowArgs.PrintingRows;
        this.DocumentId = windowArgs.DocumentId;
        this.FillItemsSource();
    }

    private FillItemsSource() {
        this.ItemsSource = this.PrintingRows.filter(f => !AppTool.IsNullOrEmpty(f.Error));
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    DownloadClicked() {
        DownloadManager.DownloadPage(this.DocumentId, this.SecurityId);
    }
}
